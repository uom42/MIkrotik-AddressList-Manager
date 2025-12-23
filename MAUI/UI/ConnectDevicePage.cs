using CommunityToolkit.Maui.Alerts;

using MALM.Model;

using uom.maui;

using static MALM.Localization.LStrings;

namespace MALM.UI
{

    internal class ConnectDevicePage : ContentPage
    {

        private readonly ActivityIndicator _circle = new()
        {
            IsRunning = true ,
            Color = Colors.SeaGreen ,
        };
        private readonly Label _lblProgress = new()
        {
            HorizontalTextAlignment = TextAlignment.Center ,
            HorizontalOptions = LayoutOptions.Center ,
            FontAttributes = FontAttributes.Bold ,
            FontSize = 20
        };

        private readonly DevicesListRecord _deviceToConnect;

        public ConnectDevicePage ( DevicesListRecord dev )
        {
            _deviceToConnect = dev;

            StackLayout stackLayout = new() { Padding = 20 };

            stackLayout.Children.Add(_lblProgress);
            stackLayout.Children.Add(_circle);

            Content = stackLayout;

            Loaded += async ( s , e ) => await ConnectDevice();
        }


        private async Task ConnectDevice ( int timeout = MikrotikDotNet.Model.Helpers.DEVICE_CONNECT_TIMEOUT_DEFAULT )
        {
            _lblProgress.Text = L_CONNECTING_TO.format(_deviceToConnect.AddressString ?? string.Empty);

            await Task.Delay(500);
            try
            {
                var con = await MikrotikDotNet.Model.Helpers.ConnectDeviceAsync(_deviceToConnect.CreateConnection() , timeout);
                //Connected successfully
                con.Close();

                await this.eSetDialogResultAndPopBackAsync(con);
            }
            catch ( Exception ex )
            {
                _circle.IsRunning = false;
                ex.eLogErrorToast();
                await Shell.Current.Navigation.PopAsync();
            }
        }
    }
}
