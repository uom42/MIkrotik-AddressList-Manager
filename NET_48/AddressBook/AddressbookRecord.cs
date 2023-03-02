#if DEBUG
//#define DONT_ENCRYPT_ADDRESSBOOK
#endif
#nullable enable


using System.Drawing;

using Extensions;

using MikrotikDotNet;

using uom.Extensions;

namespace malm.AddressBook
{

	/// <summary>
	/// Addressbook file
	/// </summary>
	[Serializable]
	public class AddressbookRecord
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
		private const string RECENT_FILE = "Recent.dat";

		public string Address { get; set; } = string.Empty;

		public string UserName { get; set; } = string.Empty;

		//Storing password safe in memory
		//This protection only works to protect against random memory scanner programs.
		//In case of a deliberate attack on running programm, this will not save your data.
		private SecureString PwdSafe;

		/// <summary>Password stored in file as PlainText, but whole addressbook is encrypted</summary>
		public string Pwd
		{
			get => PwdSafe.e_FromSecureStringToUnsafeString();
			set { this.PwdSafe = value.e_ToSecureString(); }
		}

		public string Group { get; set; } = string.Empty;

		public string Port { get; set; } = string.Empty;


		#region Constructors

		/// <summary>Just for serialization/deserialization</summary>
		public AddressbookRecord()
		{
			PwdSafe = new();
			PwdSafe.MakeReadOnly();
		}

		internal AddressbookRecord(string address, string user, string pwd, string group, string port) : this()
		{
			Address = address.Trim();
			Port = port.Trim();
			UserName = user.Trim();
			Pwd = pwd;
			Group = group.Trim();
		}

		#endregion


		#region Recent Load / Save

		private static FileInfo GetRecentStorage() => RECENT_FILE.e_GetFileIn_AppData_Roaming().e_ToFileInfo()!;

		private static void SaveRecent(string host) => GetRecentStorage().e_WriteAllText(host);

		internal static string? LoadRecent() => GetRecentStorage().e_ReadAsText();

		#endregion


		#region AddressBook Load / Save

		private static AddressbookRecord FromEditor(frmAddressBookRecordEditor e)
			=> new(e.txtAddress.Text, e.txtUser.Text, e.txtPWD.Text, e.txtGroup.Text, e.txtPort.Text);


		internal static void SaveAddressBook(AddressbookRecord[] rows, string masterP)
		{
			using var sw = ADDRESSBOOK_FILE
				.e_GetFileIn_AppData_Roaming()
				.e_ToFileInfo()!
				.e_CreateWriter(FileMode.OpenOrCreate, encoding: Encoding.Unicode);

			sw.BaseStream.e_Truncate();
			string xml = rows
				.e_SerializeAsXML();

#if !DONT_ENCRYPT_ADDRESSBOOK || !DEBUG
			xml = xml
				.e_Encrypt_AES256_ToBase64String((masterP.Length > 0) ? masterP : DEFAULT_MASTER_KEY, iterations: AES_ITERATIONS);
#endif

			sw.WriteLine(xml);
			sw.Flush();
		}


		internal static AddressbookRecord[] LoadAddressBook(string masterP)
		{

			var fiBase = ADDRESSBOOK_FILE
			.e_GetFileIn_AppData_Roaming()
			.e_ToFileInfo()!;

			if (!fiBase.Exists) return Array.Empty<AddressbookRecord>();
			string xml = fiBase!.e_ReadAsText()!;

#if !DONT_ENCRYPT_ADDRESSBOOK || !DEBUG
			xml = xml
				.e_Decrypt_AES256_FromBase64String((masterP.Length > 0) ? masterP : DEFAULT_MASTER_KEY, createSaltFromPassword: true, iterations: AES_ITERATIONS)
				.e_ToStringUnicodeFast();
#endif

			AddressbookRecord[]? rawRows = xml.e_DeSerializeXML<AddressbookRecord[]>();
			if (rawRows == null) return Array.Empty<AddressbookRecord>();

			var rows = rawRows
				.OrderBy(r => r.Group).ThenBy(r => r.Address).ThenBy(r => r.UserName)
				.ToArray();

			return rows;
		}

		#endregion

		private int? PortInt { get => (string.IsNullOrWhiteSpace(Port)) ? null : Int32.Parse(Port); }

		private MKConnection CreateConnection() => (PortInt.HasValue)
			? new(Address, UserName, Pwd, PortInt.Value)
			: new(Address, UserName, Pwd);


		public static MKConnection? Login()
		{
			string? masterKey = MasterKey_Ask();
			if (masterKey == null) return null;

			AddressbookRecord[] records = AddressbookRecord.LoadAddressBook(masterKey);

			using frmAddressbook fab = new();

			#region Internal Helpers

			void updateRecordInList(ListViewItem li)
			{
				AddressbookRecord abr = (AddressbookRecord)li.Tag;
				li.e_UpdateTexts(
					abr.Address,
					abr.PortInt.HasValue
						? abr.PortInt.Value.ToString()
						: "default",
					abr.UserName);

				li.Group = fab.lvwAddressbook.e_GroupsCreateGroupByKey(
					string.IsNullOrWhiteSpace(abr.Group)
						? "Default"
						: abr.Group,
					onNewGroup: (grp => grp.e_SetState(common.Controls.ListViewEx.ListViewGroupState.Collapsible)))
				.Group;
				fab.lvwAddressbook.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}

			ListViewItem addRecordToList(AddressbookRecord abr)
			{
				var li = new ListViewItem(abr.Address);
				li.e_AddFakeSubitems(fab.lvwAddressbook);
				li.Tag = abr;
				fab.lvwAddressbook.Items.Add(li);
				updateRecordInList(li);
				return li;
			}

			void saveAddrBook()
			{
				AddressbookRecord[] records = fab.lvwAddressbook
				.e_ItemsAndTags<AddressbookRecord>()
				.Select(li => li.Tag!)
				.ToArray();

				AddressbookRecord.SaveAddressBook(records, masterKey!);
			}

			bool checkUserInputValid(frmAddressBookRecordEditor fe)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(fe.txtAddress.Text.Trim())) throw new ArgumentNullException("Address");
					if (!string.IsNullOrWhiteSpace(fe.txtPort.Text.Trim()) && !UInt16.TryParse(fe.txtPort.Text.Trim(), out ushort iPort)) throw new ArgumentOutOfRangeException("Port");
					if (string.IsNullOrWhiteSpace(fe.txtUser.Text.Trim())) throw new ArgumentNullException("User");
					return true;
				}
				catch (Exception ex) { ex.FIX_ERROR(true); }
				return false;
			}

			void onSelectedChanged()
			{
				AddressbookRecord? sel = fab.lvwAddressbook.e_SelectedItemsAndTags<AddressbookRecord>().FirstOrDefault()?.Tag;
				var hasSel = (sel != null);

				fab.btnConnect.Enabled = hasSel;
				fab.btnDelete.Enabled = hasSel;
				fab.btnEdit.Enabled = hasSel;

				try { SaveRecent(sel?.Address ?? ""); } catch { }
			};

			#endregion


			fab.lvwAddressbook.e_ClearItems();
			fab.e_runDelayedOnFormShown(delegate
			{
				string? recent = null;
				try { recent = LoadRecent(); } catch { }//Just ignore

				fab.lvwAddressbook.MultiSelect = false;

				foreach (var abr in records)
				{
					var li = addRecordToList(abr);
					if (recent != null && li.Text.ToLower().Trim() == recent.ToLower().Trim())
					{
						li.e_Activate();
					}
				}
				onSelectedChanged();
			});

			fab.lvwAddressbook.SelectedIndexChanged += delegate { onSelectedChanged(); };

			fab.btnDelete.Click += delegate
			{
				var sel = fab.lvwAddressbook.e_SelectedItemsAndTags<AddressbookRecord>().FirstOrDefault();
				if (sel == null) return;

				if ($"Delete record '{sel.Tag!.Address}'?".e_MsgboxAsk(false) != DialogResult.Yes) return;

				fab.lvwAddressbook.Items.Remove(sel.Item);
				saveAddrBook();
			};

			fab.btnEdit.Click += delegate
			{
				var sel = fab.lvwAddressbook.e_SelectedItemsAndTags<AddressbookRecord>().FirstOrDefault();
				if (sel == null) return;

				AddressbookRecord abr = sel.Tag!;

				using frmAddressBookRecordEditor fe = new();
				fe.txtPort.e_SetVistaCueBanner("Leave empty for default");
				fe.txtAddress.Text = abr.Address;
				fe.txtPort.Text = abr.Port;
				fe.txtUser.Text = abr.UserName;
				fe.txtPWD.Text = abr.Pwd;
				fe.txtGroup.Text = abr.Group;

				fe.btnSave.Click += delegate { if (checkUserInputValid(fe)) fe.DialogResult = DialogResult.OK; };
				if (fe.ShowDialog(fab) != DialogResult.OK) return;

				abr = AddressbookRecord.FromEditor(fe);
				sel.Item.Tag = abr;

				updateRecordInList(sel.Item);
				saveAddrBook();
			};

			fab.btnAdd.Click += delegate
			{
				using frmAddressBookRecordEditor fe = new();
				fe.btnSave.Click += delegate { if (checkUserInputValid(fe)) fe.DialogResult = DialogResult.OK; };
				if (fe.ShowDialog(fab) != DialogResult.OK) return;

				AddressbookRecord abr = AddressbookRecord.FromEditor(fe);
				addRecordToList(abr);
				saveAddrBook();
			};

			fab.btnSetMasterKey.Click += delegate
			{
#if DONT_ENCRYPT_ADDRESSBOOK && DEBUG
				"Addressbook Encryption disabled by DONT_ENCRYPT_ADDRESSBOOK flag!".e_MsgboxShow();
				return;
#else
				string? newKey = MasterKey_Edit(masterKey, fab);
				if (newKey == null) return;
				masterKey = newKey;
				saveAddrBook();
#endif
			};

			Action connectToHost = async delegate
						{
							var sel = fab.lvwAddressbook.e_SelectedItemsAndTags<AddressbookRecord>().FirstOrDefault();
							if (sel == null) return;

							Control[] buttonsToDisable = { fab.btnAdd, fab.btnEdit, fab.btnDelete, fab.btnConnect, fab.btnSetMasterKey };

							fab.UseWaitCursor = true;
							fab.Update();

							buttonsToDisable.e_Enable(false);
							try
							{
								var con = sel.Tag!.CreateConnection();

								using Task tskConnect = new(() => con.Open(), TaskCreationOptions.LongRunning);
								tskConnect.Start();
								await tskConnect;

								if (!con.IsOpen) throw new Exception("Failed to connect to mikrotik!");

								//Connected successfully
								fab.Tag = con;
								fab.DialogResult = DialogResult.OK;
							}
							catch (Exception ex) { ex.FIX_ERROR(true); }
							finally
							{
								buttonsToDisable.e_Enable(true);
								fab.UseWaitCursor = false;
								onSelectedChanged();
							}
						};

			fab.btnConnect.Click += async (_, _) => { connectToHost(); };
			fab.lvwAddressbook.DoubleClick += async (_, _) => { connectToHost(); };

			var dr = fab.ShowDialog();
			if (dr != DialogResult.OK) return null;

			var con = (MKConnection)fab.Tag;
			return con;
		}









		private static string? MasterKey_Ask()
		{

#if DONT_ENCRYPT_ADDRESSBOOK && DEBUG
			return "";
#endif
			using frmMasterKey fe = new()
			{
				StartPosition = FormStartPosition.CenterScreen,
				ShowInTaskbar = true
			};
			fe.lblMasterPwd.Text = "Enter " + fe.lblMasterPwd.Text;
			fe.btnOk.Text = "Ok";
			if (fe.ShowDialog() != DialogResult.OK) return null;
			return fe.txtKey.Text;
		}

		private static string? MasterKey_Edit(string oldKey, Form parent)
		{
			using frmMasterKey fe = new() { StartPosition = FormStartPosition.CenterParent };
			fe.lblMasterPwd.Text = "New " + fe.lblMasterPwd.Text;
			fe.txtKey.Text = oldKey;
			fe.btnOk.Text = "Save";
			if (fe.ShowDialog(parent) != DialogResult.OK) return null;
			return fe.txtKey.Text;
		}
	}

}
