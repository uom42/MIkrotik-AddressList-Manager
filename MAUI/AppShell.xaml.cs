using MALM.Pages;
using MALM.UI;

namespace MALM;

public partial class AppShell : Shell
{



	public AppShell()
	{
		InitializeComponent();

		RegisterRoutes();
	}

	private static void RegisterRoutes()
	{
		Routing.RegisterRoute(nameof(MasterKeyUI), typeof(MasterKeyUI));

		//Routing.RegisterRoute(nameof(DevicesListUI), typeof(DevicesListUI));

		Routing.RegisterRoute(nameof(DevicesListRecordEditorUI), typeof(DevicesListRecordEditorUI));
		Routing.RegisterRoute(nameof(ConnectDevicePage), typeof(ConnectDevicePage));
		Routing.RegisterRoute(nameof(MikrotikAddressTableRecord_ListUI), typeof(MikrotikAddressTableRecord_ListUI));
	}
}
