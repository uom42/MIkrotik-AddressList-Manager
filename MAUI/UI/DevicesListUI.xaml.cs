
using static MALM.Localization.Strings;

using System.Collections.ObjectModel;
using MALM.Model;
using MALM.Model.Mikrotik;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Plugin.CurrentActivity;



namespace MALM.UI;



public partial class DevicesListUI : ContentPage
{

	private ObservableCollection<DevicesListRecordRowsGroup> _deviceGroups = [];


	//Disallowing returns to Login Screen on Back Button
	protected override bool OnBackButtonPressed() => true;


	/// <summary>Non Windows Code</summary>
	private async Task OnLoad()
	{

		/*
		var loginResult = await MasterKeyManager.OpenDevicesDatabase();
		if (loginResult == null)
		{
			// User canceleg login
			OnExit();
			return;
		}
		_mkm = loginResult.Manager;
		_devices = new(loginResult.Devices);

		 */

		_devices.CollectionChanged += OnCollectionChanged;


		var groups =
				(from row in _devices
				 group row by row.Group into newGroup
				 orderby newGroup.Key
				 select new
				 {
					 Name = newGroup.Key,
					 GroupItems = newGroup.ToArray()
				 })
				.Select(g => new DevicesListRecordRowsGroup(lvwDevices, g.Name, g.GroupItems))
				.ToArray();

		//await DisplayAlert("DEBUG", "QueryMKData 1", L_OK);
		_deviceGroups = new(groups);
		lvwDevices.ItemsSource = _deviceGroups;

		//await DisplayAlert("DEBUG", "QueryMKData 2", L_OK);
		//OnDeviceSelected();

		if (!_devices.Any())
		{

			//Automaticaly adding new element if list is empty
			OnAdd(this, EventArgs.Empty);
		}

		await Task.Delay(1);

	}


	private (DevicesListRecordRowsGroup? Group, bool Added) FindDeviceGroup(DevicesListRecord? dev, bool createIfNotExist)
	{
		if (dev == null) return (null, false);

		var foundGroup = _deviceGroups
			.Where(g => (dev.Group ?? string.Empty).Trim().Equals(g.Name, StringComparison.CurrentCultureIgnoreCase))
			.FirstOrDefault();

		if (foundGroup != null) return (foundGroup!, false);
		foundGroup = new DevicesListRecordRowsGroup(lvwDevices, (dev.Group ?? string.Empty).Trim(), []);
		return (foundGroup, true);
	}





	private async void OnCollectionChanged(object? s, NotifyCollectionChangedEventArgs e)
	{
		//Debug.WriteLine($"***************** CollectionChanged!");
		await SaveDevicesList();
	}


	private void OnExit() => Application.Current?.Quit();


	#region Tap gestures



	private async void OnDeviceRow_Tapped(object sender, TappedEventArgs e)
	{
		var v = sender as View;
		var md = v?.BindingContext as DevicesListRecord;
		if (md == null) return;

		await OnTryConnectDevice(md, EventArgs.Empty);

	}


	private async void OnGroupTap(object sender, TappedEventArgs e)
	{
		var v = sender as View;
		if (v == null) return;

		try
		{
			var grp = v?.BindingContext as DevicesListRecordRowsGroup;
			if (grp == null) return;

			if (grp.SwitchCollapsed()) lvwDevices.ItemsSource = _deviceGroups;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"\t*****\t {ex.Message}");
		}
		await Task.Delay(1);
	}


	#endregion


}