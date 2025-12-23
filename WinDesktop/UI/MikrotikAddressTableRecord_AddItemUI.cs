#nullable enable


using MALM.UI.AddressListSuggestions;

using MikrotikDotNet.Model.IP.Firewall.AddressList;


namespace MALM.UI
{
    internal partial class MikrotikAddressTableRecord_AddItemUI : Form
    {
        private const string DOUBLE_DOT = ":";

        private readonly MKConnection _connection;
        private readonly Dictionary<string , ListViewGroupCollapsedState> _groups;
        private readonly string _selectedGroupName;
        private readonly SuggestionItemBase[] _addressSuggestionList;

        internal AddressListItem? _dialogResult;

        public MikrotikAddressTableRecord_AddItemUI ( MKConnection connection , SuggestionItemBase[] addressSuggestions , Dictionary<string , ListViewGroupCollapsedState> groups , string selectedGroupName ) : base()
        {
            InitializeComponent();

            LocalizeUI();

            _connection = connection;
            _groups = groups;
            _selectedGroupName = selectedGroupName;
            _addressSuggestionList = addressSuggestions;

            Load += ( _ , _ ) => OnLoad();
            btnAdd.Click += async ( _ , _ ) => await OnAddBtn();
        }


        private void LocalizeUI ()
        {
            // Localization

            Text = L_MK_ADDRESS_LIST_EDITOR;
            label1.Text = L_NEW_ELEMENT_WILL_BE_DISABLED;


            lblGroup.Text = L_DEVICE_GROUP + DOUBLE_DOT;
            lblAddress.Text = L_DEVICE_ADDRESS + DOUBLE_DOT;
            lblComment.Text = L_COMMENT + DOUBLE_DOT;


            grpSources.Text = L_ATR_SAMPLE_GROUP;

            btnCancel.Text = L_CANCEL;
            btnAdd.Text = L_OK;

        }



        [STAThread]
        private void OnLoad ()
        {

            ValidateUserInput();

            //Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            //var ApS = Thread.CurrentThread.GetApartmentState();

            {
                cboGroup.Items.Clear();
                cboGroup.Items.AddRange(_groups.Keys.ToArray());
                if ( !string.IsNullOrWhiteSpace(_selectedGroupName) ) cboGroup.Text = _selectedGroupName;
                cboGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                cboGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            }

            {
                cboAddress.Items.Clear();

                var addressLust = _addressSuggestionList
                    .Select(si => si.Address)
                    .Distinct()
                    .OrderBy(a => a)
                    .ToArray();

                cboAddress.Items.AddRange(addressLust);
                cboAddress.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboAddress.AutoCompleteSource = AutoCompleteSource.ListItems;

                cboAddress.SelectedIndexChanged += delegate
                {
                    //If the user selected an item from suggestion List - reset selected object and leave only host address
                    var mko = GetSelectedAddress();
                    if ( mko != null )
                    {
                        cboAddress.runDelayedInUIThread(delegate
                        {
                            cboAddress.SelectedItem = null;
                            cboAddress.Text = mko.Address;
                        } , 10);
                        txtComment.Text = mko.MikrotikRow.Comment;
                    }
                };

                cboAddress.TextChanged += delegate { ValidateUserInput(); };
            }


            {
                //lvwSources.eClear(false);

                var ll = _addressSuggestionList
                    .Select(si =>
                    {
                        var li = new uom.controls.ListViewItemTRO<SuggestionItemBase>(si);
                        li.addFakeSubitems(lvwSources);

                        li.updateTexts(
                            si.Address ,
                            si.Comment);

                        ListViewGroupCollapsedState gs = _groups.TryGetValue(si.GrpupName , out var s)
                            ? s
                            : ListViewGroupCollapsedState.Expanded;

                        li.Group = lvwSources!.groupsCreateGroupByKey(si.GrpupName , newGroupState: gs).Group;

                        return li;
                    })
                    .ToArray();

                lvwSources.addItems(ll , clearItemsBefore: true);

                lvwSources
                    .SelectedIndexChanged += ( _ , _ ) =>
                {
                    var sel = lvwSources.selectedItemsAs<uom.controls.ListViewItemTRO<SuggestionItemBase>>()
                        .FirstOrDefault();

                    if ( sel != null )
                    {
                        cboAddress.Text = sel.Value.Address;
                        txtComment.Text = sel.Value.Comment ?? string.Empty;
                    }
                };
            }

        }


        private bool ValidateUserInput ()
        {
            //Validate user input 
            string address = cboAddress.Text.Trim();
            string group = cboGroup.Text.Trim();

            bool canAdd = !string.IsNullOrWhiteSpace(address) && !string.IsNullOrWhiteSpace(group);
            btnAdd.Enabled = canAdd;
            return canAdd;
        }


        private SuggestionItemBase? GetSelectedAddress ()
        {
            switch ( cboAddress.SelectedItem )
            {
                case SuggestionItemBase mk: return mk;
                default: return null;
            }
        }


        private async Task OnAddBtn ()
        {
            if ( !ValidateUserInput() ) return;

            string address = cboAddress.Text;
            string group = cboGroup.Text;
            string comment = txtComment.Text;

            UseWaitCursor = true;


            AddressListItem? newRow = await AddressListItem.Add(
                 _connection ,
                    false ,
                    group ,
                    address ,
                      true ,
                      comment);

            if ( newRow == null ) throw new Exception(string.Format(E_FAILED_TO_ADD_ITEM , address));

            _dialogResult = newRow;
            DialogResult = DialogResult.OK;

        }




    }
}
