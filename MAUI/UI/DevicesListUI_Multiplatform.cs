#nullable enable

namespace MALM.UI;

using static MALM.Localization.Strings;

using MALM.Model;
using MikrotikDotNet;


#if !WINDOWS

using System.Collections.ObjectModel;

using uom.maui;

using MALM.Pages;
using MALM.Model.Mikrotik;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Alerts;

#endif



partial class DevicesListUI
{

	private MasterKeyManager? _mkm = null;


#if !WINDOWS
	private ObservableCollection<DevicesListRecord> _devices = [];
#else
	private MKConnection? _dialogResult = null;
	private DevicesListRecord[] _devices = [];
#endif


	public DevicesListUI()
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
#else
		lvwDevices.MultiSelect = false;
		lvwDevices.SelectedIndexChanged += delegate { OnDeviceSelected(); };

		btnAdd.Click += OnAdd!;
		btnEdit.Click += OnEdit!;
		btnDelete.Click += OnDelete!;
		btnSetMasterKey.Click += OnChangeMasterKey!;

		btnConnect.Click += async (s, e) => await OnTryConnectDevice(s, e);
		lvwDevices.DoubleClick += async (s, e) => await OnTryConnectDevice(s, e);

		this.Load += (_, _) => OnLoad();
#endif

	}


	/// <summary>	Only Windows</summary>
	internal DevicesListUI(LoginResult l) : this()
	{
		_mkm = l.Manager;
#if ANDROID
		_devices = new(l.Devices);
#else
							 _devices = l.Devices;
#endif


	}






	private void LocalizeUI()
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



	#region UI Actions



#if !ANDROID
	private DevicesListRecord? selectedDevice
	{
		get
		{
			//return lvwDevices.SelectedItem as DevicesListRecord;
			return lvwDevices.e_SelectedItemsAndTags<DevicesListRecord>().FirstOrDefault()?.Tag;
		}
	}

	private void OnDeviceSelected()
	{
		DevicesListRecord? sel = selectedDevice;
		var hasSel = (sel != null);

		btnConnect.Enabled = hasSel;
		btnDelete.Enabled = hasSel;
		btnEdit.Enabled = hasSel;

		try { SaveRecent(sel?.AddressString ?? ""); } catch { }
	}
#endif



	#endregion


	#region Edit list


	private async void OnAdd(object s, EventArgs e)
	{

#if !WINDOWS

		//TODO: Change android to dialogresult with AutoresetEvent


		void addDevice(DevicesListRecord? dev)
		{
			if (dev == null) return;
			_devices.Add(dev);

			var fr = FindDeviceGroup(dev, true);
			DevicesListRecordRowsGroup g = fr.Group!;
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
		await this.e_GoToWithReturnAsync<DevicesListRecord>(
			nameof(DevicesListRecordEditorUI),
			retDev => addDevice(retDev),
			DevicesListRecordEditorUI.C_INPUT_PARAM_KEY, md);

#else


		using DevicesListRecordEditorUI fe = DevicesListRecordEditorUI.InitUI(null);
		if (fe.ShowDialog(this) != DialogResult.OK) return;

		DevicesListRecord abr = DevicesListRecord.FromEditor(fe);
		var li = AddRecordToList(abr);
		await SaveDevicesList();
		li.e_Activate();

#endif

	}


	private async void OnEdit(object s, EventArgs e)
	{

#if !WINDOWS

		var ss = s as Button;
		var md = ss?.BindingContext as DevicesListRecord;
		if (md == null) return;


		//TODO: Change android to dialogresult with AutoresetEvent
		var oldGroupFindResult = FindDeviceGroup(md, false);

		void updateDevice(int index, DevicesListRecord? dev)
		{
			if (dev == null) return;
			_devices[index] = dev!;

			var gOld = oldGroupFindResult.Group;
			var newGroupFindResult = FindDeviceGroup(dev, true);
			if (gOld != null && object.ReferenceEquals(gOld, newGroupFindResult.Group))
			{
				//Changing in some group
				index = gOld.IndexOf(md);
				gOld[index] = dev!;
			}
			else
			{
				//Group was changed
				gOld!.Remove(dev);
				if (gOld.CachedItemsCount < 1) _deviceGroups.Remove(gOld);


				DevicesListRecordRowsGroup gNew = newGroupFindResult.Group!;
				if (newGroupFindResult.Added)
				{
					//Adding new group with to Old Existing Group
					_deviceGroups.Add(gNew);
				}
				gNew.Add(dev);
				gNew.IsCollapsed = false;
				//lvwDevices.ScrollTo(dev);
			}
		}

		int idx = _devices.IndexOf(md);
		await this.e_GoToWithReturnAsync<DevicesListRecord>(
			nameof(DevicesListRecordEditorUI),
			retDev => updateDevice(idx, retDev),
			DevicesListRecordEditorUI.C_INPUT_PARAM_KEY, md);


#else

		var sel = lvwDevices.e_SelectedItemsAndTags<DevicesListRecord>().FirstOrDefault();
		if (sel == null) return;

		DevicesListRecord dev = sel.Tag!;
		using DevicesListRecordEditorUI fe = DevicesListRecordEditorUI.InitUI(dev);

		if (fe.ShowDialog(this) != DialogResult.OK) return;

		dev = DevicesListRecord.FromEditor(fe);
		sel.Item.Tag = dev;

		UpdateRecordInList(sel.Item);
		await SaveDevicesList();

#endif


	}


	private async void OnDelete(object s, EventArgs e)
	{

#if !WINDOWS
		//var md = selectedDevice;
		var ss = s as Button;
		var md = ss?.BindingContext as DevicesListRecord;
#else
		var sel = lvwDevices.e_SelectedItemsAndTags<DevicesListRecord>().FirstOrDefault();
		var md = sel?.Tag;

#endif

		if (md == null) return;

		string q = string.Format(Q_ADDRESSBOOK_DELETE_RECORD, md.AddressString);

#if !WINDOWS
		if (!await DisplayAlert(L_DELETE, q, L_YES, L_NO)) return;

		var grp = FindDeviceGroup(md, false).Group;
		_devices.Remove(md);
		if (grp != null)
		{
			grp.Remove(md);
			if (grp.CachedItemsCount < 1) _deviceGroups.Remove(grp);
		}


#else

		if (!q.e_MsgboxAskIsYes(false, L_DEVICES_LIST_EDITOR)) return;
		lvwDevices.Items.Remove(sel!.Item);
		await SaveDevicesList();
		OnDeviceSelected();
#endif

	}


	#endregion





	private async void OnChangeMasterKey(object s, EventArgs e)
	{

#if DONT_ENCRYPT_ADDRESSBOOK && DEBUG
		   "Addressbook Encryption disabled by DONT_ENCRYPT_ADDRESSBOOK flag!".e_MsgboxShow();
	   return;
#endif




#if !WINDOWS
		await _mkm!.ShowEditorUI(SaveDevicesList);
#else
		await _mkm!.ShowEditorUI(this, SaveDevicesList);
#endif
	}





	private async Task SaveDevicesList()
	{

#if !WINDOWS
		var rows = _devices;
#else
		var rows = lvwDevices
		.e_ItemsAndTags<DevicesListRecord>()
		.Select(li => li.Tag!);
#endif
		await _mkm!.Database_WriteEncrypted([.. rows]);
	}






	#region Connect


	private async Task OnTryConnectDevice(object s, EventArgs e)
	{

#if !WINDOWS
		var md = s as DevicesListRecord;
#else
		var sel = lvwDevices.e_SelectedItemsAndTags<DevicesListRecord>().FirstOrDefault();
		var md = sel?.Tag;

#endif
		if (md == null || string.IsNullOrWhiteSpace(md.AddressString)) return;

#if !WINDOWS

		var isGranted = await uom.maui.security.PermissionsHelper.CheckAndRequestPermissionAsync_Net_AccessNetworkStateAndInternet();
		if (!isGranted)
		{
			await Toast
					.Make("Network permission not granted!", CommunityToolkit.Maui.Core.ToastDuration.Long, 14)
					.Show();

			return;
		}

		ConnectDevicePage cp = new(md);
		await cp.e_ShowDialogAsync<MKConnection>(true, OnConnectedOK,
			async () =>
			{
				//Connection Failed!
				Debug.WriteLine("\t***\tConnection Failed");
				await Task.Delay(1);
			}
		);


#else
		Control[] buttonsToDisable = [btnAdd, btnEdit, btnDelete, btnConnect, btnSetMasterKey];
		UseWaitCursor = true;
		Update();

		buttonsToDisable.e_Enable(false);

		try
		{
			var con = await DevicesListRecord.Connect(md);

			//Connected successfully
			await OnConnectedOK(con!);
		}
		catch (Exception ex) { ex.e_LogError(true, E_TITLE_DEFAULT); }
		finally
		{
			buttonsToDisable.e_Enable(true);
			UseWaitCursor = false;
			OnDeviceSelected();
		}
#endif

	}


	private async Task OnConnectedOK(MKConnection? con)
	{
#if !WINDOWS
		//await DisplayAlert("Connection OK", "Connection OK", L_OK);
		MikrotikAddressTableRecord_ListUI p = new(con!);
		await Shell.Current.Navigation.PushAsync(p);
#else
		await Task.Delay(1);
		_dialogResult = con!;
		DialogResult = DialogResult.OK;
#endif
	}


	#endregion




}