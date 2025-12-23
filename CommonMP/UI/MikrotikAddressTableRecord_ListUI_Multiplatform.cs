#nullable enable

using MikrotikDotNet;
using MikrotikDotNet.Model.IP.Firewall.AddressList;

#if !WINDOWS

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Markup;


using uom.maui;
using MALM.Model.AddressList;

#endif

using static MALM.Localization.LStrings;


namespace MALM.UI;


partial class MikrotikAddressTableRecord_ListUI
{

    private bool _tableRowsReady = false;
    private readonly MKConnection _connection;

#if !WINDOWS

    private bool _isRefreshingState = false;

    private AddressListItemRow[] _mikrotikRows = [];
    private AddressListItemRowsGroup[] _groups = [];

#else
    private AddressListItemRow[] _mikrotikRows = [];
#endif



    #region Constructors

    /// <summary>Creates new instance with specifed Connection object</summary>
    public MikrotikAddressTableRecord_ListUI ( MKConnection c ) : base()
    {
        _connection = c;

        InitializeComponent();

        //CommunityToolkit.Maui.Core.Platform.KeyboardExtensions.HideKeyboardAsync();
        //this.HideKeyboardAsync(CancellationToken.None);


#if WINDOWS
        this.attach_PositionAndStateSaver();
#endif

        LocalizeUI();

#if WINDOWS
        {
            const string URL_FREE_ICONS = "https://www.flaticon.com/free-icons/google-plus";

            llFreeIcons.setTextAsLink(
                string.Format(L_WE_USE_FREE_ICONS , URL_FREE_ICONS) ,
                delegate { try { URL_FREE_ICONS.eOpenURLInBrowser(); } catch { } }
                );
        }


        var iconSize = System.Windows.Forms.SystemInformation.SmallIconSize;

        var bmIconGreen = uom.AppInfo.AppAssembly
            .eLoadSVGFromResourceFile("ball_green.svg")
            .eToBitmap(iconSize);

        var bmIconGray = bmIconGreen.eToGrayscaled_ToolStripRenderer();
        var iml = new ImageList()
        {
            ColorDepth = ColorDepth.Depth32Bit ,
            ImageSize = iconSize ,
        };
        iml.Images.Add(AddressListItemRow.C_IMAGE_KEY_GRAY , bmIconGray);
        iml.Images.Add(AddressListItemRow.C_IMAGE_KEY_GREEN , bmIconGreen);
        lvwRows.SmallImageList = iml;



        lvwRows.SelectedIndexChanged += ( _ , _ ) => UpdateUIState();


        lvwRows.Items_NeedRefreshList += async ( _ , _ ) => await RefreshList();
        btnRows_Refresh.Click += async ( _ , _ ) => await RefreshList();

        //btnRows_Enable.Click += async (s, e) => await SelectedRows_EnableDisable(true);
        //btnRows_Disable.Click += async (s, e) => await SelectedRows_EnableDisable(false);
        btnRows_Enable.CheckStateChanged += async ( s , e ) => await SelectedRows_EnableDisable(btnRows_Enable.CheckState == CheckState.Checked);
        lvwRows.KeyPress += async ( _ , e ) =>
        {
            if ( e.KeyChar == ' ' )
            {
                //Inverting selected rows checkstate
                await SelectedRows_EnableDisable(!(btnRows_Enable.CheckState == CheckState.Checked));

                //Refreshing UI State
                UpdateUIState();
            }
        };

        btnRows_Add.Click += async ( s , e ) => await OnRows_Add();


        btnViewARPList.Click += ( _ , _ ) => ShowLANDevices();


        Load += async ( s , e ) => await OnLoad();


#else
        Loaded += async ( s , e ) => await OnLoad();
        btnExitApp.Clicked += ( _ , _ ) => OnExit();

        btnRows_Refresh.Clicked += async ( _ , _ ) => await RefreshList();
        rvData.Refreshing += async ( _ , _ ) =>
        {
            if ( _isRefreshingState ) return;
            await RefreshList();
        };
#endif
    }


    private void LocalizeUI ()
    {
#if WINDOWS
        Text = $"{Application.ProductName} ({_connection.Host})";
        colAddress.Text = L_DEVICE_ADDRESS;
        colComment.Text = L_COMMENT;
        colCreated.Text = L_CREATED;
        colID.Text = L_ID;
        lvwRows.EmptyText = L_INITIALIZING;

        btnRows_Refresh.Text = L_REFRESH;
        btnRows_Enable.Text = L_ENABLE;
        //btnRows_Disable.Text = L_DISABLE;

#else
        Title = _connection.Host;
        btnExitApp.Text = L_APP_EXIT;

        lvwRows.EmptyView(new Label()
           .Text(L_LIST_IS_EMPTY)
           .TextCenterVertical()
           .TextCenterHorizontal()
           );
#endif

        btnRows_Add.Text = L_ADD;
    }


    #endregion


    private async Task OnLoad ()
    {

#if WINDOWS
        lvwRows.GroupCollapsedStateChanged += ( _ , _ ) =>
        {
            if ( !_tableRowsReady ) return;
            lvwRows.SaveGroupsState(dataID: GetHostDataID());
        };

        toolBarMain.enableItems(false);
        txtFilter.attach_DelayedFilter(OnFilterChanged , cueBanner: L_FILTER);
        this.runDelayed_OnShown(QueryDataFromDevice);
        await Task.Delay(1);
#else


        await RefreshList();
#endif
    }


    #region Fill UI with data

    /// <summary>
    /// Called once on form load, and then when user click Refresh button or swipe down list
    /// </summary>	
    private async Task RefreshList ()
    {

        try
        {

#if !WINDOWS
            _isRefreshingState = true;
            if ( !rvData.IsRefreshing ) rvData.IsRefreshing = true;
#endif

            await QueryDataFromDevice();

        }
        finally
        {
#if !WINDOWS
            if ( rvData.IsRefreshing ) rvData.IsRefreshing = false;
            _isRefreshingState = false;
#endif
        }
    }


    /// <summary>Queries router for data</summary>
    private async Task QueryDataFromDevice ()
    {

#if WINDOWS
        UseWaitCursor = true;
        lvwRows.EmptyText = L_QUERING_DATA;

        lvwRows.clearItems();
        toolBarMain.enableItems(false);
#else

        lvwRows.ItemsSource = null;
#endif
        _mikrotikRows = [];

        try
        {
            //Query router for data
            var mikrotikRows = (await AddressListItem.GetItemsAsync(_connection! , false))
                .Where(r => r.Dynamic == false);


#if WINDOWS
            AddressListItemRow[] uiRows = [ .. mikrotikRows.Select(ip => new AddressListItemRow(ip , lvwRows)) ];

#else
            AddressListItemRow[] uiRows = [ .. mikrotikRows.Select(ip => new AddressListItemRow(ip)) ];
            _groups = (from row in uiRows
                       group row by row.GroupName into newGroup
                       orderby newGroup.Key
                       select new
                       {
                           Name = newGroup.Key ,
                           GroupItems = newGroup.ToArray()
                       })
                .Select(g => new AddressListItemRowsGroup(lvwRows , g.Name , g.GroupItems))
                .ToArray();

            //await DisplayAlert("DEBUG", "QueryMKData 1", L_OK);
            lvwRows.ItemsSource = _groups;
            //await DisplayAlert("DEBUG", "QueryMKData 2", L_OK);
#endif
            _mikrotikRows = uiRows;




            DisplayFilteredMKData();

        }
        catch ( Exception ex )
        {

#if WINDOWS
            lvwRows.EmptyText = ex.Message;
            ex.eLogError(true , E_TITLE_DEFAULT);
#else
            ex.eLogErrorToast();

            //Return to Select Device Dialog
            await Shell.Current.Navigation.PopAsync();
#endif
        }
        finally
        {
#if WINDOWS
            UseWaitCursor = false;
#endif
        }
    }



    /// <summary>Populates ListView with filtered rows</summary>
    private void DisplayFilteredMKData ()
    {
        try
        {

#if WINDOWS

            lvwRows.EmptyText = _mikrotikRows.Any()
                ? L_FILTERING_DATA
                : L_LIST_IS_EMPTY;

            string filter = txtFilter.Text.Trim();
            var rowsToDisplay = _mikrotikRows
                .Where(r => r.Filter(filter));

            if ( !rowsToDisplay.Any() ) lvwRows.EmptyText = rowsToDisplay.Any()
                    ? L_FILTERING_DATA
                    : string.Format(L_FILTERED_DATA_EMPTY , filter);

            lvwRows.runOnLockedUpdate(() =>
            {
                lvwRows.clearItems();
                rowsToDisplay.forEach(r => r.UpdateGroupFromModel(lvwRows));

                lvwRows.Items.AddRange(rowsToDisplay.ToArray());

            } , true , true);

            lvwRows.RestoreGroupsStateFromStorage(dataID: GetHostDataID());
#endif

        }
        finally
        {
            _tableRowsReady = true;
#if WINDOWS
            UpdateUIState();
#endif
        }
    }


    #endregion





}