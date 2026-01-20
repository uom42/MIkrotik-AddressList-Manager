using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.NetworkInformation;

#if !WINDOWS
using uom.maui;
#endif


namespace uom.network.OUI;


/// <summary>Singleton for OUI Database access and management <br/>
/// http://standards.ieee.org/develop/regauth/oui/oui.txt - daily updated list of assigned manufacturer codes in MAC addresses
/// 
/// </summary>
internal partial class OUIDatabase
{


    private const string FAILED_TO_PARSE_OUI_CONTENT = "Failed to parse OUI database!";
    private const string EMBEDDED_OUI_DATABASE_FILE_RESOURCE_SUFFIX = "oui.txt";
    private const string LOCAL_APP_DATA_CACHED_OUI_FILE = "oui.txt";
    private const int DATABSE_OBSOLETE_DAYS = 10;
    private const int REMOTE_DOWNLOAD_TIMEOUT_MIN = 5;

    ///<remarks>Ежедневно обновляемый список назначеных кодов производителей в MAC-адресах</remarks>
    public const string REMOTE_OUI_DATABASE = $@"http://standards.ieee.org/develop/regauth/oui/oui.txt";

#if WINDOWS
    private static FileInfo DefaultOUIDatabaseFile => uom.AppTools.GetFileIn_AppData(LOCAL_APP_DATA_CACHED_OUI_FILE , false);
#endif

    [GeneratedRegex(@"^
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
\t{4}(?<ADDR_COUNTRY>.+)\r\n" ,
        RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant)]
    private static partial Regex rxOUI ();


    /// <summary>Handlers to be invoked when the database becomes available.</summary>
    private static readonly List<Action<OUIDatabase>> _dbReadyHandlers = [];

    /// <summary>object to be used as a synchronization lock for thread-safe operations within the class.</summary>
    private static readonly EventArgs _databaseLockObject = new();


    #region STATIC SIGLETON

    /// <summary>Singleton instance</summary>
    private static OUIDatabase? _instance;


    /// <summary>Singleton instance</summary>
    public static OUIDatabase? Instance { get { lock ( _databaseLockObject ) return _instance; } }



    /// <summary>
    /// Begins asynchronous initialization of the <see cref="OUIDatabase"/> instance.
    /// </summary>
    /// <param name="delaySec">
    /// Number of seconds to wait before attempting database creation. Defaults to 3 to allow the main application to finish loading.
    /// </param>
    /// <param name="onDbReady">
    /// Optional callback invoked when the database is ready. If the database is already initialized the callback is invoked synchronously
    /// and the method returns early. If the database is not yet available the callback will be registered and invoked once initialization completes.
    /// </param>
    /// <remarks>
    /// The method returns immediately and starts a background long-running task to perform initialization. The background task:
    /// - Sleeps for <paramref name="delaySec"/> seconds to avoid slowing application startup.
    /// - Attempts to create the database using <see cref="OUIDatabase.TryCreate"/>.
    /// - If creation succeeds, stores the instance into the private backing field under a lock and invokes all registered ready handlers.
    ///
    /// Thread-safety:
    /// - The <see cref="Instance"/> property and assignment use a private lock object to protect access to the backing field.
    /// - Registered handlers are invoked while holding the handlers collection lock; exceptions thrown by individual handlers are swallowed to avoid disrupting other handlers.
    ///
    /// Errors thrown during initialization are logged via <c>ex.eLogError(false)</c>.
    /// </remarks>
    public static void BeginInitInstance ( int delaySec = 3 , Action<OUIDatabase>? onDbReady = null , string ouiEmbeddedResourceFileSuffix = EMBEDDED_OUI_DATABASE_FILE_RESOURCE_SUFFIX , int maxAgeDays = DATABSE_OBSOLETE_DAYS )
    {
        if ( onDbReady  is not null  && AttachDatabaseReadyHandler(onDbReady) ) return; //Database already initialized, callback invoked synchronously.
        if ( Instance  is not null  ) return; // onDbReady was null and database already initialized.

        // Start background task to initialize database.
        _ = Task.Factory.StartNew(async () =>
        {
            //Make initial pause to allow main App load faster.
            await Task.Delay(delaySec * 1000);

            try
            {

                var db = OUIDatabase.TryCreateInstance(ouiEmbeddedResourceFileSuffix , maxAgeDays);
                if ( db is null ) return; //Failed To Create Database!

                lock ( _databaseLockObject )
                    _instance = db;

                lock ( _dbReadyHandlers )
                {
                    // Invoke all registered handlers
                    _dbReadyHandlers.ForEach(h =>
                    {
                        try { h.Invoke(Instance!); }
                        catch { }
                    }
                    );
                }
            }
            catch ( Exception ex )
            {
#if WINDOWS                
                ex.logError(false);
#else
                ex.eLogErrorNoUI();
#endif
            }
        } , TaskCreationOptions.LongRunning);
    }


    #endregion


    /// <summary>
    /// Attaches a callback to be invoked when the <see cref="OUIDatabase"/> is available.
    /// </summary>
    /// <param name="onDbReady">A callback that receives the initialized <see cref="OUIDatabase"/> instance.</param>
    /// <returns>
    /// <c>true</c> if the database was already initialized and the callback was invoked synchronously;
    /// <c>false</c> if the callback was queued and will be invoked later when initialization completes.
    /// </returns>
    /// <remarks>
    /// Behavior and thread-safety:
    /// - If the database is not yet initialized, the callback is added to the internal handlers list under a lock and will be invoked
    ///   later by the initialization task when the database becomes available.
    /// - If the database is already initialized, the callback is invoked immediately on the calling thread. In this synchronous case,
    ///   any exception thrown by the callback will propagate to the caller.
    /// - Queued callbacks are invoked by the background initializer; exceptions thrown during those invocations are swallowed
    ///   to avoid disrupting other registered handlers.
    /// 
    /// Callers should keep synchronous handlers lightweight and handle exceptions if they must not propagate.
    /// </remarks>
    public static bool AttachDatabaseReadyHandler ( Action<OUIDatabase> onDbReady )
    {
        if ( Instance is null ) // Database not yet initialized. Queue the handler to be called when database will be ready.
        {
            lock ( _dbReadyHandlers ) _dbReadyHandlers.Add(onDbReady);
            return false;
        }
        else
        {
            onDbReady.Invoke(Instance!); // Database already initialized, invoke handler immediately.
            return true;
        }
    }


    private readonly EventArgs _syncObject = new();

    /// <summary>All OUI Records (more than 30K)</summary>
    private readonly ReadOnlyDictionary<string , MacRecordsGroup> _fullOUIDatabase;

    /// <summary>Last queries records we add to this fast cache</summary>
    private readonly Dictionary<string , MacRecordsGroup> _fastOUICache = [];


    public readonly FileInfo? DatabaseFile;
    public bool UseInternalDatabase => DatabaseFile is null ;


    private OUIDatabase ( FileInfo? fi , ReadOnlyDictionary<string , MacRecordsGroup> dic )
    {
        DatabaseFile = fi;
        _fullOUIDatabase = dic;
    }


    #region TryCreateInstance


    private static OUIDatabase? TryCreateInstance ( string ouiEmbeddedResourceFileSuffix , int maxAgeDays )
    {


        FileInfo? ouiDatabaseFile
#if WINDOWS
            = DefaultOUIDatabaseFile;
#else
            = null;
#endif

        string ouiFileBody = string.Empty;
        bool needUpdateDB = false;

        if ( ouiDatabaseFile is null || !ouiDatabaseFile.Exists )
        {
            //External DB not found, use DB from Resources
            //Debug.WriteLine($"External OUI-base '{fiBase}' not found. Using internal base.");
            ouiFileBody = uom.AppTools.GetEmbeddedResourceAsString(ouiEmbeddedResourceFileSuffix);
            ouiDatabaseFile = null;
            needUpdateDB = true;
        }
        else
        {
            // Exist external downloaded DB file in AppData
            ouiFileBody = ouiDatabaseFile.readAsText(detectEncodingFromByteOrderMarks: true)!;
            //Debug.WriteLine($"Loaded External OUI-base from '{fiBase}'");

            var baseAge = DateTime.Now - ouiDatabaseFile.LastWriteTime;
            needUpdateDB = baseAge.TotalDays >= maxAgeDays;
        }



        var records = ParseOUIFile(ouiFileBody);
        if ( records.Count == 0 ) throw new Exception(FAILED_TO_PARSE_OUI_CONTENT);
        var db = new OUIDatabase(ouiDatabaseFile , records);

#if WINDOWS
        if ( needUpdateDB ) Task.Factory.StartNew(UpdateDatabase , TaskCreationOptions.LongRunning);
#endif

        return db;
    }


    private static ReadOnlyDictionary<string , MacRecordsGroup> ParseOUIFile ( string fileBody )
    {
        Regex rx = rxOUI();
        var matches = rx.Matches(fileBody);

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




#if WINDOWS



    public static async Task UpdateDatabase ()
    {
        await Task.Delay(5_000);

        FileInfo localDBTargetPath = DefaultOUIDatabaseFile;

        CancellationToken ct = new();
        TimeSpan downloadTimeout = TimeSpan.FromMinutes(REMOTE_DOWNLOAD_TIMEOUT_MIN);

        //string tempFile = System.IO.Path.GetTempFileName();
#if DEBUG
        //tempFile = @"t:\_UOI_Database_DownloadTempFile.txt";
#endif
        FileInfo fiTemp = new(System.IO.Path.GetTempFileName());
        _ = fiTemp.tryDeleteIfExist();

        try
        {
            FileInfo? newDatabase = await REMOTE_OUI_DATABASE.urlHttpClient_DownloadFileAsync(fiTemp.FullName , downloadTimeout , ct);
            if ( newDatabase is null || !newDatabase.Exists || (ulong)newDatabase.Length < 3u.fileSize_MBToBytes() )
            {
                // Failed to download or downloded less than 3 MB (invalid data)
                _ = newDatabase?.tryDeleteIfExist();
                return;
            }

            _ = localDBTargetPath.tryDeleteIfExist();
            newDatabase.MoveTo(localDBTargetPath.FullName);

        }
        catch ( HttpRequestException exweb )
        {
            _ = fiTemp.tryDeleteIfExist();

            HttpStatusCode? errCode = exweb.StatusCode;
            exweb.logError(false);
        }
        catch ( Exception ex )
        {
            _ = fiTemp.tryDeleteIfExist();
            ex.logError(false);
        }


    }

#endif


    #region GetRecord___


    private MacRecordsGroup? GetMacRecord ( PhysicalAddress mac )
    {
        string id = mac.FormatMfgOctets();

        lock ( _syncObject )
        {
            if ( _fastOUICache.TryGetValue(id , out var mrg) ) return mrg;
            if ( !_fullOUIDatabase.TryGetValue(id , out mrg) ) return null;
            //Add found value to fast cache
            _fastOUICache.Add(id , mrg);
            return mrg;
        }
    }

    public bool GetMacRecordString ( PhysicalAddress mac , out string? mfgString , out MacRecordsGroup? mrg )
    {
        mfgString = null;
        mrg = GetMacRecord(mac);
        if ( mrg is null ) return false;// null;

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
            return _fullOUIDatabase;
        }
    }


    #endregion

}
