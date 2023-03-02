#nullable enable

///////////////////////////////////////////////////////////
// This programm uses Icons from 'https://www.flaticon.com/free-icons/google-plus'
// <a href="https://www.flaticon.com/free-icons/google-plus" title="google plus icons">Google plus icons created by Smashicons - Flaticon</a>
///////////////////////////////////////////////////////////


global using System.Windows.Forms;

global using MikrotikDotNet;

global using static MALM.Localization.Strings;


using MALM.Model;
using MALM.UI;

namespace MALM
{

	internal static class Program
	{

		[STAThread]
		private static void Main()
		{
			//Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
			//Debug.WriteLine(Thread.CurrentThread.GetApartmentState());

			{
				//Localization Test

				/*
				var cul = new CultureInfo("en-US");
				var cul = new CultureInfo("uk-UA");
				
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk-UA");
				 */
			}



			{
				// To customize application configuration such as set high DPI settings or default font, see https://aka.ms/applicationconfiguration.
				ApplicationConfiguration.Initialize();
			}
			{
				/*
							Application.EnableVisualStyles();
							Application.SetCompatibleTextRenderingDefault(false);
				 */
			}

			var loginResult = MasterKeyManager.OpenDevicesDatabaseWindows().e_RunSync();
			if (loginResult == null)
			{
				// User canceleg login
				return;
			}

			var conDev = UI.DevicesListUI.SelectDeviceAndConnect(loginResult);
			if (conDev == null) return;  // User canceleg device selection

			using MikrotikAddressTableRecordsListUI fm = new(conDev);
			Application.Run(fm);
		}
	}
}