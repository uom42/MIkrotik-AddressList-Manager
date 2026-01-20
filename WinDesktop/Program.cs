#nullable enable

///////////////////////////////////////////////////////////
// This programm uses Icons from 'https://www.flaticon.com/free-icons/google-plus'
// <a href="https://www.flaticon.com/free-icons/google-plus" title="google plus icons">Google plus icons created by Smashicons - Flaticon</a>
///////////////////////////////////////////////////////////


global using System.Windows.Forms;

global using MikrotikDotNet;

global using static MALM.Localization.LStrings;

using MALM.Model;
using MALM.UI;

namespace MALM;


internal static class Program
{

	[STAThread]
	private static void Main ()
	{

#if DEBUG
        //Localization Test

        /*
		var cul = new CultureInfo("en-US");
		var cul = new CultureInfo("uk-UA");

		Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk-UA");
		 */
#endif

		// To customize application configuration such as set high DPI settings or default font, see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();
		Application.SetColorMode(SystemColorMode.System);
		uom.AppTools.RegisterEncodings();

		/*
		uom.Network.IP4AddressWithMask ipmRecursiveCloudFlareDNS = new(IPAddress.Parse("1.0.1.0"), 8);
		var ip1 = IPAddress.Parse("1.1.1.1");
		bool b = ip1.eIsInSubnet(ipmRecursiveCloudFlareDNS);
		return;
		 */

		using uom.AppTools.SingleInstanceController<uom.AppTools.SecondAppLaunchContext> controller = uom.AppTools.CreateDefaultSingleInstanceController();

		controller.FirstInstanceRunOrSignal(() =>
		{
			var inst = uom.AppTools.SingleInstanceController<uom.AppTools.SecondAppLaunchContext>.Default;

			var loginResult = MasterKeyManager.OpenDevicesDatabaseWindows().runSync();
			if ( loginResult == null )
			{
				// User canceleg login
				return null;
			}

			var conDev = UI.DevicesListUI.SelectDeviceAndConnect(loginResult);
			if ( conDev == null ) return null;  // User canceleg device selection


			return new MikrotikAddressTableRecord_ListUI(conDev);
		});

	}
}