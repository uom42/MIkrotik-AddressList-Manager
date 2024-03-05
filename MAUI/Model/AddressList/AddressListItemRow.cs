#nullable enable

using static MALM.Localization.LStrings;

using CommunityToolkit.Mvvm.ComponentModel;

using MikrotikDotNet.Model.IP.Firewall.AddressList;

using uom.maui;
using CommunityToolkit.Maui.Alerts;


namespace MALM.Model.AddressList;


internal class AddressListItemRow(AddressListItem ali) : ObservableObject
{

	internal AddressListItemRow Clone() => new(MikrotikRow);

	public AddressListItem MikrotikRow { get; private set; } = ali;


	public string Title => MikrotikRow.Address + (string.IsNullOrWhiteSpace(MikrotikRow.Comment)
		? string.Empty
		: $" ({MikrotikRow.Comment})");


	public string CreatedTimestamp => MikrotikRow.CreationTime.e_ToLongDateTimeString();

	public string ID => MikrotikRow.MKID;


	public string GroupName => MikrotikRow.List.Trim();


	public InvertableBool IsStateChanging { get; private set; } = false;


	public bool Checked
	{
		get => MikrotikRow.Enabled;
		set
		{
			bool oldValue = MikrotikRow.Enabled;
			if (value == oldValue) return;
			OnPropertyChanging(nameof(Checked));

			Debug.WriteLine($"\t***\t Trying to write 'Checked' property from '{oldValue}' to '{value}'");

			SetMikrotikRowStateAsync(value);
			//OnPropertyChanged(nameof(Checked));
		}
	}

	private void SetMikrotikRowStateAsync(bool enabled)
	{
		//Trying to change  property async...
		_ = Task.Factory.StartNew(async () => await SetMikrotikRowState(enabled));
	}

	private async Task SetMikrotikRowState(bool enable)
	{

		void setChangingModeInMainThread(bool isChanging)
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				IsStateChanging = isChanging;
				OnPropertyChanged(nameof(IsStateChanging));
			});
		}

		void OnPropertyChangedOnMainThread(string propName)
			=> MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(propName));

		//Disabling Checkbox
		setChangingModeInMainThread(true);
		try
		{
			//Get current row info from kikrotik to avoid modifications of rows with wrong .id
			AddressListItem? mkRow = await MikrotikRow.ReQueryAsync();

			//if item with this id will be not found - error will bw thrown and no modification on item with wrong id will be made.
			await mkRow.EnableAsync(enable);//item with this id was found. Make modification

			//Again requery item to get new properties
			MikrotikRow = await mkRow.ReQueryAsync();
		}
		catch (Exception ex)
		{
			ex.e_LogErrorToast();
			//await QueryMKData();//On any error we refill list with actual data from mikrotik
		}
		finally
		{
			await Task.Delay(500);
			setChangingModeInMainThread(false);
			OnPropertyChangedOnMainThread(nameof(Checked));
		}
	}




	public bool Filter(string filter)
	{
		if (string.IsNullOrWhiteSpace(filter)) return true;

		filter = filter.Trim();
		const StringComparison sc = StringComparison.OrdinalIgnoreCase;

		if (MikrotikRow.Address.Contains(filter, sc)) return true;
		if (MikrotikRow.List.Contains(filter, sc)) return true;
		if (MikrotikRow.CreationTime.ToString().Contains(filter, sc)) return true;
		if (MikrotikRow.Comment.e_IsNotNullOrWhiteSpace() && MikrotikRow.Comment.Contains(filter, sc)) return true;

		return false;
	}
}
