using System.Windows.Input;
using System.Xml.Serialization;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MALM.UI;

using MikrotikDotNet;

using uom.maui;

using static MALM.Localization.Strings;

namespace MALM.Model
{

	//Android Specific Code
	partial class DevicesListRecord : ObservableObject, INotifyPropertyChanged
	{


		[XmlIgnore] public string Title => $"{AddressString} ({UserName})";


		[XmlIgnore]
		public string PortString2 => PortInt.HasValue
			? $":{PortInt.Value}"
			: string.Empty;


		public void OnPropertyChanged2([CallerMemberName] string propName = "")
			=> OnPropertyChanged(new PropertyChangedEventArgs(propName));






		/*

		[RelayCommand]
		public async void CloseBreadcrumbTapped(DevicesListRecord selectedItem)
		{
			//UpdateCommodityTaxonomyWhenBreadCrumbChange(selectedItem);
			await Toast
					.Make("CloseBreadcrumbTapped!", CommunityToolkit.Maui.Core.ToastDuration.Long, 14)
					.Show();

		}
		 */


		public ICommand DeviceTappedCommand { get; } = new Command(async o =>
		{
			DevicesListRecord dr = (DevicesListRecord)o!;
			await dr.OnTryConnectDevice();
		});

		private async Task OnTryConnectDevice()
		{

			var isGranted = await uom.maui.security.PermissionsHelper.CheckAndRequestPermissionAsync_Net_AccessNetworkStateAndInternet();
			if (!isGranted)
			{
				await Toast
						.Make("Network permission not granted!", CommunityToolkit.Maui.Core.ToastDuration.Long, 14)
						.Show();

				return;
			}

			ConnectDevicePage cp = new(this);
			await cp.e_ShowDialogAsync<MKConnection>(true,
				async con =>
				{
					MikrotikAddressTableRecord_ListUI p = new(con!);
					await Shell.Current.Navigation.PushAsync(p);
				}
,
				async () =>
				{
					//Connection Failed!
					Debug.WriteLine("\t***\tConnection Failed");
					await Task.Delay(1);
				}
			);
		}

	}
}
