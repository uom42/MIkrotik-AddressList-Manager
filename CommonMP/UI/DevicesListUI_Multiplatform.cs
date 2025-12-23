
using MALM.Model;
using MikrotikDotNet;
using static MALM.Localization.LStrings;

#nullable enable

namespace MALM.UI;
#if !WINDOWS

using System.Collections.ObjectModel;

using uom.maui;

using MALM.Pages;
using MALM.Model.Mikrotik;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;

#endif


internal partial class DevicesListUI
{

    private readonly MasterKeyManager? _mkm = null;


#if !WINDOWS
	private ObservableCollection<DevicesListRecord> _devices = [];
#else
    private MKConnection? _dialogResult = null;
    private readonly DevicesListRecord[] _devices = [];
#endif


    public DevicesListUI ()
    {

#if !WINDOWS
		//Checking reference to MauiIcon (AathifMahir.Maui.MauiIcons.Material.Outlined) http://www.aathifmahir.com/dotnet/2022/maui/icons
		_ = new MauiIcons.Core.MauiIcon();
#endif

        InitializeComponent();

        LocalizeUI();


#if !WINDOWS
		btnAdd.Clicked += OnAdd!;
		btnSetMasterKey.Clicked += OnChangeMasterKey!;



		this.Loaded += async (s, e) => await OnLoad();

		this.Appearing += async (s, e) => await onAppearing();
		this.Disappearing += async (s, e) => await onDisappearing();
#else
        lvwDevices.MultiSelect = false;
        lvwDevices.SelectedIndexChanged += delegate { OnDeviceSelected(); };

        btnAdd.Click += OnAdd!;
        btnEdit.Click += Device_Edit!;
        btnDelete.Click += Device_Delete!;
        btnSetMasterKey.Click += OnChangeMasterKey!;

        btnConnect.Click += async ( s , e ) => await OnTryConnectDevice(s , e);
        lvwDevices.DoubleClick += async ( s , e ) => await OnTryConnectDevice(s , e);

        Load += ( _ , _ ) => OnLoad();
#endif

        OUI.Manager.BeginInitialize(2);
    }


    /// <summary>	Only Windows</summary>
    internal DevicesListUI ( LoginResult l ) : this()
    {
        _mkm = l.Manager;
#if ANDROID
		_devices = new(l.Devices);
#else
        _devices = l.Devices;
#endif

    }



    private void LocalizeUI ()
    {

#if !WINDOWS
		Title = L_DEVICES_LIST;

		lvwDevices.EmptyView(new Label()
			.Text(L_LIST_IS_EMPTY)
			.TextCenterVertical()
			.TextCenterHorizontal()
			);

		btnExitApp.Text = L_APP_EXIT;
		btnExitApp.Clicked += (_, _) => OnExit();

#else
        Text = L_DEVICES_LIST;

        btnDelete.Text = L_DELETE;
        btnEdit.Text = L_EDIT;

        btnConnect.Text = L_CONNECT;

        colAddress.Text = L_DEVICE_ADDRESS;
        colUser.Text = L_DEVICE_USER;

#endif

        btnAdd.Text = L_ADD;
        btnSetMasterKey.Text = $"{L_SET} {L_MASTER_KEY.ToLower()}";
    }



    #region Edit list



    private async void OnAdd ( object s , EventArgs e )
    {

#if !WINDOWS

		//TODO: Change android to dialogresult with AutoresetEvent


		void AppendDeviceToDatabase(DevicesListRecord? dev)
		{
			if (dev == null) return;
			_devices.Add(dev);

			var fr = FindDeviceGroup(dev, true);
			DevicesListRecordsGroup g = fr.Group!;
			if (fr.Added)
			{
				//Adding new group with to Old Existing Group
				_deviceGroups.Add(g);
			}
			g.Add(dev);
			g.IsCollapsed = false;
			//lvwDevices.ScrollTo(dev);
		}

		DevicesListRecord md = new();
		await this.eGoToWithReturnAsync<DevicesListRecord>(
			nameof(DevicesListRecordEditorUI),
			retDev => AppendDeviceToDatabase(retDev),
			DevicesListRecordEditorUI.C_INPUT_PARAM_KEY, md);

#else


        using DevicesListRecordEditorUI fe = DevicesListRecordEditorUI.InitUI(null);
        if ( fe.ShowDialog(this) != DialogResult.OK ) return;

        DevicesListRecord abr = DevicesListRecord.FromEditor(fe);
        var li = AddRecordToList(abr);
        await SaveDevicesList();
        li.activate();

#endif

    }





    #endregion



    private async void OnChangeMasterKey ( object s , EventArgs e )
    {
#if !WINDOWS
		await _mkm!.ShowEditorUI(SaveDevicesList);
#else
        await _mkm!.ShowEditorUI(this , SaveDevicesList);
#endif
    }


    private async Task SaveDevicesList ()
    {

#if !WINDOWS
		var rows = _devices;
#else

        //uom.controls.ListViewItemT<DevicesListRecord>? SelectedDeviceeeeee		=> lvwDevices.eSelectedItemsAs<uom.controls.ListViewItemT<DevicesListRecord>>().FirstOrDefault();


        var rows = lvwDevices
            .itemsAs<uom.controls.ListViewItemT<DevicesListRecord>>()
            .Select(li => li.Value2);
#endif
        await _mkm!.Database_WriteEncrypted([ .. rows ]);
    }








}