#nullable enable

using MALM.UI;

#if !WINDOWS

using uom.controls.MAUI;
using uom.maui;

using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

using Switch = Microsoft.Maui.Controls.Switch;


#else

using System.IO.IsolatedStorage;
using System.Management;

#endif


namespace MALM.Model
{


	internal class MasterKeyManager
	{


		public const int MAX_MASTERKEY_LEN = 50;
		private const int MAX_WRONG_KEY_COUNT = 3;

#if !WINDOWS
		private const string MKey_Value = "MKey_Value";
#else
		private const string I_STORE_FILE = "MKey.bin";
#endif

		public enum LoginModes : int
		{
			/// <summary>Initial Setup MasterKey when not found in storage</summary>
			InitialSetupMK,

			Login,

			/// <summary>MasterkeyEditor called from main UI menu</summary>
			Edit2,

		}

		private int _badLogonCount = 0;

		private LoginModes _lm = LoginModes.InitialSetupMK;
		private string? Key = null;
		public bool SavePasswordAndDontAsk = false;

		public LoginModes LoginMode => _lm;


		public bool HasSavedKey => (Key != null);
		public bool CanAutologin => HasSavedKey && SavePasswordAndDontAsk;
		public bool IsExceededBadLogonCount => _badLogonCount > MAX_WRONG_KEY_COUNT;


		private MasterKeyManager() { }



#if WINDOWS
		/// <summary>
		/// Get saved decrypted deviceList, or create new empty if not exist or wrong password
		/// </summary>		
		/// <returns>
		/// Return (null) when user canceleg login
		/// or normal Key and decrypted Rows
		/// </returns>
		internal static async Task<LoginResult?> OpenDevicesDatabaseWindows()
		{
			MasterKeyManager mk = await Load();
			try
			{
				if (mk.CanAutologin)
				{




					//Trying to load and decrypt database with old saved Masterkey...
					DevicesListRecord[]? rows = await mk!.Database_Load();

					//Decrypted OK or Database file not exist
					rows ??= await mk.Database_WriteEncryptedEmpty();// In case database file not exist - save empty Datatase.

					//Datatabse file exist and loaded ok.
					return new(mk, rows!);
				}
			}
			catch
			{
				//Autologon failed. Need Ask user for password.
			}

			//Autologon disabled or we must ask key on every login.
			{
				var rows = await MasterKeyUI.Login(mk);
				if (rows == null)
				{
					//User canceleg login
					return null;
				}

				//Login successfull
				return new(mk, rows);
			}
		}
#endif


		#region Load / Reload


		/// <summary>Load Key from secured storage</summary>
		public static async Task<MasterKeyManager> Load()
		{
			MasterKeyManager mk = new();
			await mk.LoadKey();
			mk._lm = LoginModes.Login;
			return mk;
		}

		private async Task LoadKey()
		{
			static async Task<(string?, bool)> GetMasterKey()
			{
#if !WINDOWS
				//await Task.Delay(1000);

				string? loadedKeyString = null;
				try
				{
					loadedKeyString = await SecureStorage.GetAsync(MKey_Value) ?? string.Empty;
					//await $"loadedKeyString = '{loadedKeyString}'".eMsgboxShow("SecureStorage.GetAsync");
				}
				catch (Exception ex)
				{
					ex.eLogErrorNoUI();
					loadedKeyString = string.Empty;
				}

				bool dontAskKey = loadedKeyString.eIsNotNullOrWhiteSpace();
#if DEBUG
				Debug.WriteLine($"loadedKeyString = '{loadedKeyString}'");
#endif

				if (!dontAskKey) return (null, false);//Saved key nof found, need ask key always!
#else
				using IsolatedStorageFile isoStore = GetISO();
				bool dontAskKey = ISOExist(isoStore);
				if (!dontAskKey) return (null, false);//Saved key nof found, need ask key always!

				//We have an isolated storage file with Encrypted MasterKey
				static string? DecryptMasterKey(IsolatedStorageFile isf)
				{
					//Extracting key from ISO
					using IsolatedStorageFileStream issfs = isf.OpenFile(I_STORE_FILE, FileMode.Open);
					if (issfs.Length < 2) return null;

					byte[] encryptedKey = issfs.eReadAllBytes();
					try
					{
						byte[] masterKeyBytes = ProtectedData.Unprotect(encryptedKey, GetEntrophyBytes(), DataProtectionScope.CurrentUser);
						return masterKeyBytes.eToStringUnicodeFast();
					}
					catch (CryptographicException)
					{ }

					//Storage is damaged, or OS/user profile is reinstalled
					return null;

				}
				var loadedKeyString = await Task.Factory.StartNew(() => DecryptMasterKey(isoStore));
#endif
				return (loadedKeyString, dontAskKey);
			}

			(Key, SavePasswordAndDontAsk) = await GetMasterKey();
		}

		/// <summary>Reload Key from secured storage</summary>
		public async Task ReLoad()
		{
			await LoadKey();
			_lm = HasSavedKey
				? LoginModes.Login
				: LoginModes.InitialSetupMK;
		}



		#endregion


		#region UI interaction

		/// <summary>Read user entered password from textbox</summary>
#if !WINDOWS
		internal void InitFromUI(Entry txt, SwitchLabel? chk = null)
#else
		internal void InitFromUI(TextBox txt, CheckBox? chk = null)
#endif
		{
			string userKey = (txt.Text ?? string.Empty);
			if (userKey.eIsNullOrWhiteSpace()) throw new ArgumentNullException("MasterKey");

			//We never store user entered Masterkey, we encrypt it...
			Key = userKey.eEncrypt_AES_ToBase64String(GetEntrophyString(), createSaltFromPassword: true);
#if DEBUG
			Debug.WriteLine($"\n\nuserKey: '{userKey}'\nKey: '{Key}'\n\n");
#endif
			//In Logon mode chk == null andwe dont save any changes
			if (chk != null) SavePasswordAndDontAsk = chk.Checked;
		}


		/// <summary>Read user entered password from textbox</summary>
#if !WINDOWS
		internal void FillTextBoxes(Entry txt1, Entry txt2)
#else
		[Obsolete("DO NOT USE!", true)]
		internal void FillTextBoxes(TextBox txt1, TextBox txt2)
#endif
		{
			txt1.Text = Key;
			txt2.Text = Key;
		}


		#endregion


		public void SetPasswordResetMode()
		{
			Key = null;
			_lm = LoginModes.InitialSetupMK;
		}

		#region Save


		/// <summary>Saving Secrets</summary>
		public async Task Save()
		{
			//if (!CanAutologin) throw new Exception("SAVING ONLY VALID ON AUTOLOGIN MODE!");
			await WriteOrDeleteMK(SavePasswordAndDontAsk
				? Key!
				: null);
		}

		private static async Task WriteOrDeleteMK(string? masterKey)
		{

#if !ANDROID
			using IsolatedStorageFile isoStore = GetISO();
#endif

			//if (masterKey == null) //Delete value allways!
			{
				//Deleting saved masterkey
#if !WINDOWS
				try
				{
					//DONT WORKS!!! SecureStorage.RemoveAll() is Broken: https://github.com/dotnet/maui/issues/19983
					//SecureStorage.RemoveAll();

					SecureStorage.Remove(MKey_Value);
				}
				catch { }
#else
				if (ISOExist(isoStore)) isoStore.DeleteFile(I_STORE_FILE);
#endif
				if (masterKey == null) return;
			}
			//else
			{
				if (masterKey.eIsNullOrWhiteSpace()) throw new ArgumentNullException("MasterKey");
				//Saving Masterkey

#if !WINDOWS
				try
				{
					await SecureStorage.SetAsync(MKey_Value, masterKey);
				}
				catch (Javax.Crypto.AEADBadTagException jcex)
				{
					//Looks like android secured storage is damaged!
					//Trying to clear and rewrite secure storage
					SecureStorage.RemoveAll();
					SecureStorage.SetAsync(MKey_Value, masterKey).Wait();
				}

				//Try to read ancrypted data
				var readedKey = await SecureStorage.GetAsync(MKey_Value);
				if (readedKey != masterKey)
				{
					throw new Exception("Failed to ckeck saved key - data is not equal!");
				}
				//await $"Saved Key = '{masterKey}'\n\nLoaded Key: '{readedKey}'".eMsgboxShow("SecureStorage.SetAsync");
#else
				await Task.Factory.StartNew(delegate
				{
					byte[] abKey = ProtectedData.Protect(masterKey.eGetBytes_Unicode(), GetEntrophyBytes(), DataProtectionScope.CurrentUser);
					using IsolatedStorageFileStream of = isoStore.CreateFile(I_STORE_FILE);
					of.eWriteAllBytes(abKey);
					of.Flush();
				});
#endif
			}
		}


		public async Task Database_WriteEncrypted(DevicesListRecord[] rows) => await DevicesListRecord.SaveAddressBookAsync(rows, Key!);


		/// <summary>Creating new empty encrypted Database</summary>
		public async Task<DevicesListRecord[]> Database_WriteEncryptedEmpty()
		{
			await Database_WriteEncrypted([]);
			return [];
		}


		#endregion

		#region Load

		internal async Task<bool> IsKeyValidForDatabase()
		{
			try
			{
				_ = await Database_Load();
				//Datatase successfully decrypted and loaded
				return true;
			}
			catch
			{
				return false;
			}
		}

		internal async Task<DevicesListRecord[]?> Database_Load()
		{
			try
			{
				var r = await DevicesListRecord.LoadDevicesListAsync(Key!);
				//Datatase successfully decrypted and loaded
				_badLogonCount = 0;
				return r;
			}
			catch
			{
				//Somethig go bad when loading or decrypting database.
				_badLogonCount++;
				throw;
			}
		}



		#endregion




		/// <summary>Called from Main UI</summary>
#if !WINDOWS
		public async Task ShowEditorUI(Func<Task> onMasterKeyChanged)
#else
		public async Task ShowEditorUI(Form parent, Func<Task> onMasterKeyChanged)
#endif
		{
			_lm = LoginModes.Edit2;

#if !WINDOWS
			if (!await MasterKeyUI.EditKey(this)) return;
#else
			if (!MasterKeyUI.EditKey(this, parent)) return;
#endif
			await onMasterKeyChanged.Invoke();
		}



		#region Windows IsolatedStorage helpers


#if !ANDROID

		private static IsolatedStorageFile GetISO() => IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

		private static bool ISOExist(IsolatedStorageFile isoStore) => isoStore.FileExists(I_STORE_FILE);

#endif


		#endregion





		private static String GetEntrophyString()
		{
			const string DEF_KEY = "6588b18c-25ed-4d5a-b44d-e681212667a3cea0a68b-7aa1-4d3c-b724-c27eacb5eedd";

			string es = DEF_KEY;
#if WINDOWS
			try
			{
				//GetPCBoardID
				ManagementClass mc = new("Win32_ComputerSystemProduct");
				ManagementObjectCollection moc = mc.GetInstances();
				ManagementObject? mo = moc.Cast<ManagementObject>().FirstOrDefault();
				string? mbID = mo?.Properties["UUID"].Value.ToString();
				es = mbID ?? DEF_KEY;
			}
			catch { }

#elif ANDROID
			var devID = (Application.Current as App)?._DevID ?? DEF_KEY;
			var di = DeviceInfo.Current;
			es = $"{di.Manufacturer}-{di.Model}-{devID}";
			/*
			var context = Android.App.Application.Context;
			string? id = Android.Provider.Settings.Secure.GetString(context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
			 */
#else
			var di = DeviceInfo.Current;
			es = $"{di.Manufacturer}-{di.Model}-{DEF_KEY}";
#endif
			return es;
		}

		private static byte[] GetEntrophyBytes()
		{
			try
			{
				string? en = GetEntrophyString();
				if (!string.IsNullOrWhiteSpace(en)) return en!.eGetBytes_Unicode();
			}
			catch { }
			return [9, 8, 7, 6, 5];
		}





	}


	internal class LoginResult(MasterKeyManager m, DevicesListRecord[] rows)
	{
		public readonly MasterKeyManager Manager = m;
		public readonly DevicesListRecord[] Devices = rows;
	}
}
