
using static MALM.Localization.LStrings;

using System.Collections.ObjectModel;
using MALM.Model;
using MALM.Model.Mikrotik;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using uom.maui;
using MALM.Pages;


namespace MALM.UI;


partial class DevicesListUI : ContentPage
{

	private ObservableCollection<DevicesListRecordsGroup> _deviceGroups = [];


	//Disallowing returns to Login Screen on Back Button
	protected override bool OnBackButtonPressed() => true;


	/// <summary>Non Windows Code</summary>
	private async Task OnLoad()
	{
		_devices.CollectionChanged += async (_, _) =>
		{
			//Debug.WriteLine($"***************** CollectionChanged!");
			await SaveDevicesList(); //Save database changes
		};

		var groups =
				(from row in _devices
				 group row by row.Group into newGroup
				 orderby newGroup.Key
				 select new
				 {
					 Name = newGroup.Key,
					 GroupItems = newGroup.ToArray()
				 })
				.Select(g => new DevicesListRecordsGroup(lvwDevices, g.Name, g.GroupItems))
				.ToArray();

		_deviceGroups = new(groups);
		lvwDevices.ItemsSource = _deviceGroups;

		if (!_devices.Any())
		{
			//Automaticaly adding new element if list is empty
			OnAdd(this, EventArgs.Empty);
		}

		//uom.maui.ui.KeyboardHelper.HideKeyboard();
		await Task.Delay(1);
	}


	private (DevicesListRecordsGroup? Group, bool Added) FindDeviceGroup(DevicesListRecord? dev, bool createIfNotExist)
	{
		if (dev == null) return (null, false);

		var foundGroup = _deviceGroups
			.Where(g => (dev.Group ?? string.Empty).Trim().Equals(g.Name, StringComparison.CurrentCultureIgnoreCase))
			.FirstOrDefault();

		if (foundGroup != null) return (foundGroup!, false);
		foundGroup = new DevicesListRecordsGroup(lvwDevices, (dev.Group ?? string.Empty).Trim(), []);
		return (foundGroup, true);
	}



	private void OnExit() => Application.Current?.Quit();


	#region Edit Devices list


	[RelayCommand]
	private async Task Device_Edit(DevicesListRecord dev)
	{
		var oldGroupFindResult = FindDeviceGroup(dev, false);

		void updateChangedDevice(int index, DevicesListRecord? dev2)
		{
			if (dev2 == null) return;   //Edit was canceled by User 

			_devices[index] = dev2!;
			var oldGroup = oldGroupFindResult.Group;
			var newGroupFindResult = FindDeviceGroup(dev2, true);
			if (oldGroup != null && object.ReferenceEquals(oldGroup, newGroupFindResult.Group))
			{
				//Changing in the some group
				index = oldGroup.IndexOf(dev);
				oldGroup[index] = dev2!;
			}
			else
			{
				//Group was changed
				oldGroup!.Remove(dev2);
				if (oldGroup.CachedItemsCount < 1) _deviceGroups.Remove(oldGroup); //Removing group with 0 items from the UI

				DevicesListRecordsGroup newGroup = newGroupFindResult.Group!;
				if (newGroupFindResult.Added)
				{
					//Adding new group with to Old Existing Group
					_deviceGroups.Add(newGroup);
				}
				newGroup.Add(dev2);
				newGroup.IsCollapsed = false;
			}
		}

		int idx = _devices.IndexOf(dev);
		await this.eGoToWithReturnAsync<DevicesListRecord>(
			nameof(DevicesListRecordEditorUI),
			retDev => updateChangedDevice(idx, retDev),
			DevicesListRecordEditorUI.C_INPUT_PARAM_KEY, dev);

	}


	[RelayCommand]
	private async Task Device_Delete(DevicesListRecord dev)
	{
		string q = string.Format(Q_ADDRESSBOOK_DELETE_RECORD, dev.AddressString);
		if (!await DisplayAlert(L_DELETE, q, L_YES, L_NO)) return;

		var grp = FindDeviceGroup(dev, false).Group;
		_devices.Remove(dev);
		if (grp != null)
		{
			grp.Remove(dev);
			if (grp.CachedItemsCount < 1) _deviceGroups.Remove(grp); //Removing group with 0 items from the UI
		}
	}

	#endregion






	//Start device pings...
	private async Task onAppearing()
	{
		await Task.Delay(1);

		foreach (var dev in _devices)
		{
			await dev.StatusPingBegin();
		}
	}

	private async Task onDisappearing()
	{
		await Task.Delay(1);


		foreach (var dev in _devices)
		{
			await dev.StatusPingStop();
		}

	}


}