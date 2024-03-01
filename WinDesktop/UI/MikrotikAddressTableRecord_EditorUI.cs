#nullable enable


using MALM.UI.AddressListSuggestions;

using Mikrotik.API.Model.IP.Firewall.AddressList;


namespace MALM.UI
{
	internal partial class MikrotikAddressTableRecord_EditorUI : Form
	{

		private const string DOUBLE_DOT = ":";

		private readonly MKConnection _connection;
		private string[] _groupNames;
		private string _selectedGroupName;
		private MikrotikObjectBase[] _addressSuggestionList;

		internal AddressListItem? _dialogResult;

		public MikrotikAddressTableRecord_EditorUI(MKConnection connection, MikrotikObjectBase[] addressSuggestions, string[] groupNames, string selectedGroupName) : base()
		{
			InitializeComponent();

			LocalizeUI();

			_connection = connection;
			_groupNames = groupNames;
			_selectedGroupName = selectedGroupName;
			_addressSuggestionList = addressSuggestions;

			this.Load += (_, _) => OnLoad();
			btnAdd.Click += async (_, _) => await OnAddBtn();
		}


		private void LocalizeUI()
		{
			// Localization

			Text = L_MK_ADDRESS_LIST_EDITOR;
			label1.Text = L_NEW_ELEMENT_WILL_BE_DISABLED;


			lblGroup.Text = L_DEVICE_GROUP + DOUBLE_DOT;
			lblAddress.Text = L_DEVICE_ADDRESS + DOUBLE_DOT;
			lblComment.Text = L_COMMENT + DOUBLE_DOT;

			btnCancel.Text = L_CANCEL;
			btnAdd.Text = L_OK;

		}



		[STAThread]
		private void OnLoad()
		{

			ValidateUserInput();

			//Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
			//var ApS = Thread.CurrentThread.GetApartmentState();

			{
				cboGroup.Items.Clear();
				cboGroup.Items.AddRange(_groupNames);
				if (!string.IsNullOrWhiteSpace(_selectedGroupName)) cboGroup.Text = _selectedGroupName;
				cboGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
				cboGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			}

			{
				cboAddress.Items.Clear();
				cboAddress.Items.AddRange(_addressSuggestionList);
				cboAddress.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
				cboAddress.AutoCompleteSource = AutoCompleteSource.ListItems;

				cboAddress.SelectedIndexChanged += delegate
				{
					//If the user selected an item from suggestion List - reset selected object and leave only host address
					var mko = GetSelectedAddress();
					if (mko != null)
					{
						cboAddress.e_RunDelayedInUIThread(delegate
						{
							cboAddress.SelectedItem = null;
							cboAddress.Text = mko.GetAddress();
						}, 10);
						txtComment.Text = mko.MikrotikRow.Comment;
					}
				};

				cboAddress.TextChanged += delegate { ValidateUserInput(); };
			}
		}


		private bool ValidateUserInput()
		{
			//Validate user input 
			string address = cboAddress.Text.Trim();
			string group = cboGroup.Text.Trim();

			bool canAdd = !string.IsNullOrWhiteSpace(address) && !string.IsNullOrWhiteSpace(group);
			btnAdd.Enabled = canAdd;
			return canAdd;
		}


		private MikrotikObjectBase? GetSelectedAddress()
		{
			switch (cboAddress.SelectedItem)
			{
				case MikrotikObjectBase mk: return mk;
				default: return null;
			}
		}


		private async Task OnAddBtn()
		{
			if (!ValidateUserInput()) return;

			string address = cboAddress.Text;
			string group = cboGroup.Text;
			string comment = txtComment.Text;

			this.UseWaitCursor = true;


			AddressListItem? newRow = await AddressListItem.Add(_connection, group, address, true, comment);
			if (newRow == null) throw new Exception(string.Format(E_FAILED_TO_ADD_ITEM, address));

			_dialogResult = newRow;
			this.DialogResult = DialogResult.OK;

		}




	}
}
