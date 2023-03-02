using MALM.Model.Mikrotik;


using MikrotikDotNet.Model.IP.Firewall.AddressList;

using MikrotikDotNet;

using uom.maui;

using static MALM.Localization.LStrings;

namespace MALM.UI;

partial class MikrotikAddressTableRecord_ListUI : ContentPage
{

	private void OnExit() => Application.Current?.Quit();


	/*

	private void OnGroupTap(object sender, TappedEventArgs e)
	{

		var v = sender as View;
		if (v == null) return;

		try
		{
			var grp = v?.BindingContext as AddressListItemRowsGroup;
			grp?.SwitchExpandedState();

		}
		catch (Exception ex)
		{
			Debug.WriteLine($"\t*****\t {ex.Message}");
		}
	}


	 */


}