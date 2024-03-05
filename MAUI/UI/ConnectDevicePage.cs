using CommunityToolkit.Maui.Alerts;

using MALM.Model;

using uom.maui;

using static MALM.Localization.Strings;

namespace MALM.UI
{

	internal class ConnectDevicePage : ContentPage
	{

		private readonly ActivityIndicator _circle = new()
		{
			IsRunning = true,
			Color = Colors.SeaGreen,
		};
		private readonly Label _lblProgress = new()
		{
			HorizontalTextAlignment = TextAlignment.Center,
			HorizontalOptions = LayoutOptions.Center,
			FontAttributes = FontAttributes.Bold,
			FontSize = 20
		};

		private readonly DevicesListRecord _deviceToConnect;

		public ConnectDevicePage(DevicesListRecord dev)
		{
			_deviceToConnect = dev;

			StackLayout stackLayout = new() { Padding = 20 };

			stackLayout.Children.Add(_lblProgress);
			stackLayout.Children.Add(_circle);

			Content = stackLayout;

			Loaded += async (s, e) => await ConnectDevice();
		}


		private async Task ConnectDevice()
		{
			_lblProgress.Text = L_CONNECTING_TO.e_Format(_deviceToConnect.AddressString ?? string.Empty);

			await Task.Delay(500);
			try
			{
				var con = await DevicesListRecord.OpenConnection(_deviceToConnect);
				//Connected successfully

				await this.e_SetDialogResultAndPopBackAsync(con);
			}
			catch (Exception ex)
			{
				_circle.IsRunning = false;
				await ex.e_LogErrorToast();

				await Shell.Current.Navigation.PopAsync();
			}
		}
	}
}
