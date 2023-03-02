using MALM.Pages;
using MALM.UI;

namespace MALM;

public partial class AppShell : Shell
{
	//internal string SSSSSS = "@";
	public AppShell()
	{
		InitializeComponent();

		//Title = Application. .Current.Resources.  "TITLE";
		//this.FlyoutContent = new DevicesListUI();
		RegisterRoutes();
		//SSSSSS = s;
		//Routing.RegisterRoute("buddydetail", typeof(BuddyDetail));
		//Routing.RegisterRoute(nameof(MasterKeyUI), typeof(MasterKeyUI));
	}

	private static void RegisterRoutes()
	{
		Routing.RegisterRoute(nameof(MasterKeyUI), typeof(MasterKeyUI));

		//Routing.RegisterRoute(nameof(DevicesListUI), typeof(DevicesListUI));

		Routing.RegisterRoute(nameof(MikrotikDevicesListRecordEditorPage), typeof(MikrotikDevicesListRecordEditorPage));
		Routing.RegisterRoute(nameof(ConnectDevicePage), typeof(ConnectDevicePage));
		Routing.RegisterRoute(nameof(MikrotikAddressTableRecordsListUI), typeof(MikrotikAddressTableRecordsListUI));


		/*

Routing.RegisterRoute(nameof(Register), typeof(Register));
Routing.RegisterRoute(nameof(ForgetPassword), typeof(ForgetPassword));
Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		 */
	}
}
