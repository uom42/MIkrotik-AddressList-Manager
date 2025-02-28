#if DEBUG
//#define DONT_ENCRYPT_ADDRESSBOOK
#endif
#nullable enable

using static MALM.Localization.LStrings;

using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Input;

using MikrotikDotNet;
using MALM.UI;


#if !WINDOWS

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Alerts;

using uom.maui;
#endif



namespace MALM.Model
{

	//All Platforms Code

	[Serializable]
	public partial class DevicesListRecord
	{

#if DONT_ENCRYPT_ADDRESSBOOK && DEBUG
		/// <summary>Addressbook file content is not encrypted and saved as plain text xml file</summary>
		private const string ADDRESSBOOK_FILE = "Addressbook.Unencryped.xml";
#else
		/// <summary>Default Master Key when user use empty password</summary>
		private const string DEFAULT_MASTER_KEY = "49be7021e55b4b0b8f777e03f132db8c";

		/// <summary>Addressbook file content is encrypted using AES-256 encryption with user provided Master password</summary>
		private const string ADDRESSBOOK_FILE = "Addressbook.bin";
		private const int AES_ITERATIONS = 10_000;
#endif



		internal static Lazy<FileInfo> DatabseFile = new (
#if !WINDOWS
			new FileInfo(Path.Combine(FileSystem.Current.AppDataDirectory, ADDRESSBOOK_FILE))
#else
			uom.AppTools.GetFileIn_AppData (ADDRESSBOOK_FILE, true)
#endif
		);



		#region Serializable Properties

#if !WINDOWS
		[ObservableProperty]
		[property: DefaultValue("")]
		[property: XmlElement("Address")]
		private string? _addressString = string.Empty;


		[ObservableProperty]
		[property: DefaultValue("")]
		[property: XmlElement("UserName")]
		private string? _userName = string.Empty;


		[ObservableProperty]
		[property: DefaultValue("")]
		[property: XmlElement("Group")]
		private string? _group = string.Empty;

#else

		[DefaultValue (""), XmlElement ("Address")]
		public string AddressString { get; set; } = string.Empty;


		[DefaultValue (""), XmlElement ("UserName")]
		public string UserName { get; set; } = string.Empty;


		[DefaultValue (""), XmlElement ("Group")]
		public string Group { get; set; } = string.Empty;
#endif







		[DefaultValue (""), XmlElement ("Port")]
		public string PortString
		{
			get => PortInt.HasValue ? PortInt.Value.ToString () : string.Empty;
			set
			{
#if !WINDOWS
				this.OnPropertyChanging(nameof(PortString));
#endif
				string v = value;
				if (string.IsNullOrWhiteSpace (v))
					PortInt = null;
				else
					PortInt = UInt16.Parse (v);

#if !WINDOWS
				this.OnPropertyChanged(nameof(PortString));
#endif
			}
		}



		/// <summary>Password stored in file as PlainText, but whole addressbook is encrypted</summary>
		[DefaultValue (""), XmlElement ("Password")]
		public string PwdString
		{
			get => PwdSafe.eFromSecureStringToUnsafeString ();
			set
			{
#if !WINDOWS
				this.OnPropertyChanging();
#endif
				this.PwdSafe = value.eToSecureString ();
#if !WINDOWS
				this.OnPropertyChanged();
#endif
			}
		}



		#endregion


		#region Local Properties


		[XmlIgnore]
		public UInt16? PortInt { get; set; } = null;

		/// <summary>
		/// Storing password safe in memory. This protection only works to protect against random memory scanner programs.
		/// In case of a deliberate attack on running programm, this will not save your data.
		/// </summary>
		private SecureString PwdSafe;



		/// <summary>User for sorting Rows in List</summary>
		private UInt32 AddressParsedString
		{
			get
			{
				try
				{
					if (!string.IsNullOrWhiteSpace (AddressString))
					{
						if (IPAddress.TryParse (AddressString, out var ipa) && ipa != null)
						{
							return ipa.eToUInt32CalculableOrder ();
						}
					}
				}
				catch { }
				return UInt32.MaxValue;
			}
		}


		private static DevicesListRecord[] Sort ( DevicesListRecord[] rows )
		{
			rows =
			[
				.. rows
								.OrderBy(r => r.Group)
								.ThenBy(r => r.AddressParsedString)
								.ThenBy(r => r.UserName)
,
			];

			return rows;
		}


		#endregion


		#region Constructors

		/// <summary>Just for serialization/deserialization</summary>
		public DevicesListRecord ()
		{
			PwdSafe = new ();
			PwdSafe.MakeReadOnly ();
		}

		internal DevicesListRecord ( string address, string user, string pwd, string group, string port ) : this ()
		{
			AddressString = address.Trim ();
			PortString = port.Trim ();
			UserName = user.Trim ();
			PwdString = pwd;
			Group = group.Trim ();
		}


		#endregion


		#region Load / Save


		/// <returns>Returns NULL if file not exist</returns>
		internal static async Task<DevicesListRecord[]?> LoadDevicesListAsync ( string masterP )
		{
			var fiDatabse = DatabseFile.Value;
			if (!fiDatabse.Exists) return null;

			string xml = ( await fiDatabse.eReadAsTextAsync () )!;

#if !DONT_ENCRYPT_ADDRESSBOOK || !DEBUG
			masterP = ( masterP.Length > 0 )
				? masterP
				: DEFAULT_MASTER_KEY;

			xml = xml
				.eDecrypt_AES_FromBase64String (masterP, createSaltFromPassword: true, iterations: AES_ITERATIONS)
				.eToStringUni ();
#endif

			DevicesListRecord[]? rawRows = xml.eDeSerializeXML<DevicesListRecord[]> (null, true);
			if (rawRows == null) return null;
			return Sort (rawRows);
		}


		internal static async Task SaveAddressBookAsync ( DevicesListRecord[] rows, string masterP )
		{
			Debug.WriteLine ($"***************** SaveAddressBook");

			string xml = rows.ToList ().eSerializeAsXML ();
			var fiDatabse = DatabseFile.Value;
			using var sw = fiDatabse.eCreateWriter (FileMode.OpenOrCreate, encoding: Encoding.Unicode);
			sw.BaseStream.eTruncate ();

#if !DONT_ENCRYPT_ADDRESSBOOK || !DEBUG

			masterP = ( masterP.Length > 0 )
				? masterP
				: DEFAULT_MASTER_KEY;

			xml = xml.eEncrypt_AES_ToBase64String (masterP, iterations: AES_ITERATIONS);

#endif

			await sw.WriteLineAsync (xml);
			await sw.FlushAsync ();
		}


		#region OLD WINDOWS


		/*
	 

	internal static DevicesListRecord[] LoadAddressBook(string masterP)
	{
		var fiDatabse = DatabseFile.Value;
		if (!fiDatabse.Exists) return null;

		string xml = (await fiDatabse.eReadAsTextAsync())!;

#if !DONT_ENCRYPT_ADDRESSBOOK || !DEBUG
		masterP = (masterP.Length > 0)
			? masterP
			: DEFAULT_MASTER_KEY;

		xml = xml
			.eDecrypt_AES_FromBase64String(masterP, createSaltFromPassword: true, iterations: AES_ITERATIONS)
			.eToStringUni();
#endif

		DevicesListRecord[]? rawRows = xml.eDeSerializeXML<DevicesListRecord[]>(null, true);
		if (rawRows == null) return null;
		return Sort(rawRows);
	}


		internal static void SaveAddressBook(DevicesListRecord[] rows, string masterP)
		{
			using var sw = uom.AppTools.GetFileIn_AppData(ADDRESSBOOK_FILE, true)
				.eCreateWriter(FileMode.OpenOrCreate, encoding: Encoding.Unicode);

			sw.BaseStream.eTruncate();
			string xml = rows.eSerializeAsXML();

#if !DONT_ENCRYPT_ADDRESSBOOK || !DEBUG
			xml = xml
				.eEncrypt_AES_ToBase64String((masterP.Length > 0)
					? masterP
					: DEFAULT_MASTER_KEY,
				iterations: AES_ITERATIONS);
#endif

			sw.WriteLine(xml);
			sw.Flush();
		}

	 */


		#endregion


		#endregion

#if WINDOWS

		internal static DevicesListRecord FromEditor ( DevicesListRecordEditorUI e )
			=> new (e.txtAddress.Text, e.txtUser.Text, e.txtPWD.Text, e.txtGroup.Text, e.txtPort.Text);

#endif


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl (MethodImplOptions.AggressiveInlining)]
		public MKConnection CreateConnection ()
			=> PortInt.HasValue
			? MikrotikDotNet.Model.Helpers.CreateConnection (AddressString!, UserName!, PwdString, PortInt.Value)
			: MikrotikDotNet.Model.Helpers.CreateConnection (AddressString!, UserName!, PwdString);



	}

}
