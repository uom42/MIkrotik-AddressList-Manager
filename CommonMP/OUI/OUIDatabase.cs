using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

#if !WINDOWS

using uom.maui;

#endif

namespace MALM.OUI
{

    internal partial class OUIDatabase
    {

        #region RegExp

        private const string C_REGEXP = @"^
(?<MAC_SPLITTED>([0-9A-Fa-f]{2}[-:]){2}[0-9A-Fa-f]{2})
\s{3}
\(hex\)
\t{2}
(?<COMPANY>.+)\r\n
^(?<MAC_SINGLE>([0-9A-Fa-f]{6}))
\s{5}\((base\s16)\)
\t{2}
(?<COMPANY2>.+)\r\n
\t{4}(?<ADDR_1>.+)\r\n
\t{4}(?<ADDR_CITY>.+)\r\n
\t{4}(?<ADDR_COUNTRY>.+)\r\n";

        #endregion

        [GeneratedRegex(C_REGEXP , RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant)]
        private static partial Regex rxOUI ();

        public const int DATABSE_OBSOLETE_DAYS = 10;

        ///<remarks>Файл http://standards.ieee.org/develop/regauth/oui/oui.txt - ежедневно обновляемый список назначеных кодов производителей в MAC-адресах</remarks>
        private const string OUI_FILE = "oui.txt";
        public const string REMOTE_OIU_DATABASE = $@"http://standards.ieee.org/develop/regauth/oui/{OUI_FILE}";
        private const int DOWNLOAD_TIMEOUT_MIN = 5;
        //private static readonly string C_LOCAL_OUI_DATABASE =  $@"C:\Users\uom\AppData\Local\UOM\UOM Network Center\{C_LOCAL_OUI_FILE}";

        /*
		private const int C_DATABASE_SIZE_MIN_MB = 2;
		private const int C_DATABASE_SIZE_MAX_MB = 20;
		private static readonly Encoding C_OUI_ENCODING = Encoding.UTF8;
		 */

        private readonly EventArgs _syncObject = new();

        /// <summary>All Records (more than 30K recs...)</summary>
        private readonly ReadOnlyDictionary<string , MacRecordsGroup> _fullList;

        /// <summary>Last queries records we add to this fast cache</summary>
        private readonly Dictionary<string , MacRecordsGroup> _fastCache = [];


        public readonly FileInfo? DatabaseFile;

        public bool UseInternalDatabase => DatabaseFile == null;



        private OUIDatabase ( FileInfo? fi , ReadOnlyDictionary<string , MacRecordsGroup> dic )
        {
            DatabaseFile = fi;
            _fullList = dic;
        }


        #region TryCreate, Parse


        internal static OUIDatabase? TryCreate ()
        {
            string body = string.Empty;
            FileInfo? fiBase = null;

#if WINDOWS
            FileInfo fiDefaultBase = uom.AppTools.GetFileIn_AppData(OUI_FILE , false);
            fiBase = fiDefaultBase;
#endif
            bool needUpdateDB = false;
            if ( fiBase == null || !fiBase.Exists )
            {
                //Using DB from Resources
                //Debug.WriteLine($"External OUI-base '{fiBase}' not found. Using internal base.");
                body = uom.AppTools.GetEmbeddedResourceAsString("OUI.oui.txt");
                fiBase = null;
                needUpdateDB = true;
            }
            else
            {
                //Loading
                body = fiBase.eReadAsText(detectEncodingFromByteOrderMarks: true)!;
                //Debug.WriteLine($"Loaded External OUI-base from '{fiBase}'");

                var baseAge = DateTime.Now - fiBase.LastWriteTime;
                needUpdateDB = baseAge.TotalDays >= DATABSE_OBSOLETE_DAYS;
            }

#if WINDOWS
            if ( needUpdateDB ) BeginCheckForDatabaseUpdates(fiDefaultBase);
#endif
            var records = ParseOUIFile(body);
            if ( !records.Any() ) throw new NotSupportedException(Localization.LStrings.E_OUI_FAILED_TO_PARSE_OUI_CONTENT);
            var db = new OUIDatabase(fiBase , records);
            return db;
        }


        private static ReadOnlyDictionary<string , MacRecordsGroup> ParseOUIFile ( string file )
        {
            Regex rx = rxOUI();
            var matches = rx.Matches(file);

            var parsed = matches.AsParallel()
                .Select(m => MacRecordInfo.Parse(m));

            var grouppedByMAC =
                from ri in parsed
                group ri by ri.MFGCode into grp
                orderby grp.Key
                select new
                {
                    MAC = grp.Key ,
                    InfoList = (from p in grp select p.Info).ToArray()
                };


            var mrgList = grouppedByMAC
                .Select(r => new MacRecordsGroup(r.MAC , r.InfoList));

            var dic = mrgList
                .ToDictionary(r => r.MFGCode)
                .AsReadOnly();

            return dic;
        }


        #endregion


        #region Update


        //Проверяем как давно обновлялась и обновляем если надо
        public static void BeginCheckForDatabaseUpdates ( FileInfo localDBTargetPath )
            => Task.Factory.StartNew(() => UpdateDatabase(localDBTargetPath) , TaskCreationOptions.LongRunning);


        public static async Task UpdateDatabase ( FileInfo localDBTargetPath )
        {
            Thread.Sleep(5_000);

            CancellationToken ct = new();
            TimeSpan downloadTimeout = TimeSpan.FromMinutes(DOWNLOAD_TIMEOUT_MIN);

            string tempFile = System.IO.Path.GetTempFileName();
#if DEBUG
			//tempFile = @"t:\_UOI_Database_DownloadTempFile.txt";
#endif
            FileInfo fiTemp = new(tempFile);
            fiTemp.eDeleteIfExistSafe();

            try
            {
                FileInfo? f = await uom.Network.Helpers.DownloadFile_HttpClient_Async(REMOTE_OIU_DATABASE , tempFile , downloadTimeout , ct);
                if ( f == null || !f.Exists || (ulong)f.Length < 3u.fileSize_MBToBytes() )
                {
                    f?.eDeleteIfExistSafe();
                    return;//Failed to download
                }
                localDBTargetPath.Refresh();
                localDBTargetPath.eDeleteIfExist();
                //if (fiDatabaseTarget.Exists) fiDatabaseTarget.eMoveToArhive().eClearArhiveOldFiles(DateTime.Now.AddMonths(-2));
                f.MoveTo(localDBTargetPath.FullName);

            }
            catch ( HttpRequestException exweb )
            {
                fiTemp.eDeleteIfExistSafe();

                HttpStatusCode? errCode = exweb.StatusCode;

#if WINDOWS
                exweb.eLogError(false);
#else
				exweb.eLogErrorNoUI();
#endif
            }
            catch ( Exception ex )
            {
                fiTemp.eDeleteIfExistSafe();
#if WINDOWS
                ex.eLogError(false);
#else
				ex.eLogErrorNoUI();
#endif
            }


            //return f;

            /*
			using (WebClient WC = new())
			{
				var TMR = Stopwatch.StartNew();
				try
				{
					string tempFile = System.IO.Path.GetTempFileName();
					bool bCanceled = false;
					int iProgress = -1;

					using ManualResetEvent evtDownloadFinished = new(false);

					AsyncCompletedEventArgs? ACEA;

					WC.DownloadFileCompleted += (sender, e) =>
						{
							ACEA = e;
							if (bCanceled || ACEA.Cancelled) return;
							evtDownloadFinished.Set();
						};

					WC.DownloadProgressChanged += (sender, e) =>
						{
							return;
							if (iProgress == e.ProgressPercentage) return;
							iProgress = e.ProgressPercentage;
							string sProgress = "{0} - Downloading file {1}% from {2} to {3}".eFormat(TMR.Elapsed.ToString(), iProgress, C_REMOTE_OIU_FILE, tempFile);
							Debug.WriteLine(sProgress);
						};

					Debug.WriteLine("Start downloading file from {0} to {1}".eFormat(C_REMOTE_OIU_FILE, tempFile));
					TMR = Stopwatch.StartNew();
					WC.DownloadFileAsync(new Uri(C_REMOTE_OIU_FILE), tempFile); // File downloaded as UTF-8

					int iWaitSec = 1000 * DownloadWaitTimeoutMin * 60;
					bool bDownloaded = await evtDownloadFinished.eWaitAsync(iTimeout: iWaitSec);
					if (!bDownloaded)
					{
						bCanceled = true;
						if (WC.IsBusy) WC.CancelAsync();

						var toex = new TimeoutException("Не удалось загрузить '{0}' за {1}".eFormat(C_REMOTE_OIU_FILE, ((int)TMR.ElapsedMilliseconds).eToShellTimeString(5)));
						throw toex;
					}

					if (ACEA.Error is not null) throw ACEA.Error;

					// Downloaded ok.
					Debug.WriteLine("Download finished at {0}".eFormat(TMR.Elapsed.ToString()));

					if (!System.IO.File.Exists(tempFile))
						throw new System.IO.FileNotFoundException("", tempFile);

					// Check size
					var fiTemp = new System.IO.FileInfo(tempFile);
					ulong lOUISizeBytes_Min = C_DATABASE_SIZE_MIN_MB.eMBToBytes();
					ulong lOUISizeBytes_Max = C_DATABASE_SIZE_MAX_MB.eMBToBytes();

					if (fiTemp.Length < (decimal)lOUISizeBytes_Min || fiTemp.Length > (decimal)lOUISizeBytes_Max)
					{
						const string C_ERROR = "Скачан файл '{0}'->'{1}'|Размер = {2}, должно быть {3} - {4} !";
						throw new Exception(C_ERROR.eFormat(C_REMOTE_OIU_FILE, tempFile, fiTemp.Length.eFormatByteSize(), lOUISizeBytes_Min.eFormatByteSize(), lOUISizeBytes_Max.eFormatByteSize()).eWrapVB());
					}

					// Move to Release
					C_LOCAL_OUI_DATABASE.eMakeBackUpIfExist(true);
					Debug.WriteLine("Move file {0} to {1}".eFormat(tempFile, C_LOCAL_OUI_DATABASE));
					System.IO.File.Move(tempFile, C_LOCAL_OUI_DATABASE);

					Settings.mAppSettings.SaveSetting(C_SETTING_NAME, DateTime.Now, SubKey: C_SETTING_KEY);
					return true;
				}

				catch (Exception ex)
				{
					ex.eLogError(false);
					throw;
				}
				finally
				{
					TMR.Stop();
				}
			}
			  */


        }


        #endregion

        private MacRecordsGroup? GetMacRecord ( PhysicalAddress mac )
        {
            string id = mac.FormatMfgOctets();

            lock ( _syncObject )
            {
                if ( _fastCache.TryGetValue(id , out var mrg) ) return mrg;
                if ( !_fullList.TryGetValue(id , out mrg) ) return null;
                //Add found value to fast cache
                _fastCache.Add(id , mrg);
                return mrg;
            }
        }

        public bool GetMacRecordString ( PhysicalAddress mac , out string? mfgString , out MacRecordsGroup? mrg )
        {
            mfgString = null;
            mrg = GetMacRecord(mac);
            if ( mrg == null ) return false;// null;

            MacRecordInfo[] mriList = mrg.InfoList;
            mfgString = (mriList.Length == 1)
                            ? mriList[ 0 ].ToString()
                            : mriList.Select(r => r.ToString()).join(" ,") ?? string.Empty;

            return true;
        }



        public ReadOnlyDictionary<string , MacRecordsGroup> GetRows ()
        {
            lock ( _syncObject )
            {
                return _fullList;
            }
        }

    }


}
