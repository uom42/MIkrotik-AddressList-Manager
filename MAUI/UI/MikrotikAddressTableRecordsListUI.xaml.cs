using MALM.Model.Mikrotik;


using Mikrotik.API.Model.IP.Firewall.AddressList;

using MikrotikDotNet;

using uom.maui;

using static MALM.Localization.Strings;

namespace MALM.UI;

public partial class MikrotikAddressTableRecordsListUI : ContentPage
{

	private void OnExit() => Application.Current?.Quit();


	private async void OnGroupTap(object sender, TappedEventArgs e)
	{
		var l = sender as Layout;
		if (l == null) return;

		try
		{
			var grp = l?.BindingContext as AddressListItemRowsGroup;
			if (grp == null) return;

			if (grp.SwitchCollapsed()) lvwRows.ItemsSource = _groups;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"\t*****\t {ex.Message}");
		}

		await Task.Delay(1);
	}


}