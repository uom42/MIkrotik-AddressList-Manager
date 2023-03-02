using System.Windows.Input;
using System.Xml.Serialization;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MALM.UI;

using MikrotikDotNet;

using uom.maui;

using static MALM.Localization.LStrings;

namespace MALM.Model
{

	partial class DevicesListRecord : ObservableObject, INotifyPropertyChanged
	{

		[XmlIgnore] public string Title => $"{AddressString} ({UserName})";


		[XmlIgnore]
		public string PortString2 => PortInt.HasValue
			? $"{L_DEVICE_PORT}: {PortInt.Value}"
			: string.Empty;

		//[XmlIgnore]					 		public string DeviceStatusString { get; private set; } = string.Empty;

		[XmlIgnore]
		public MikrotikDotNet.Model.System.DeviceStatus? OnlineDeviceStatus { get; private set; }


		[XmlIgnore]
		public InvertableBool IsOnline => OnlineDeviceStatus != null;


		public void OnPropertyChanged2([CallerMemberName] string propName = "")
			=> OnPropertyChanged(new PropertyChangedEventArgs(propName));


		private Timer? _tmrPulse;
		private CancellationTokenSource _ctsPulse = new();


		public async Task StatusPingBegin()
		{
			await StatusPingStop();

			_ctsPulse = new CancellationTokenSource();
			_tmrPulse = new(
				   async o => await StatusPing_Proc(o),
					_ctsPulse,
					 3000,
					 5000);
		}


		/// <summary>Gets device status (health + Voltage)</summary>
		private async Task<MikrotikDotNet.Model.System.DeviceStatus> GetDeviceStatus()
		{
			var con = await MikrotikDotNet.Model.Helpers.ConnectDeviceAsync(CreateConnection());
			var ds = await MikrotikDotNet.Model.System.DeviceStatus.GetDeviceStatusAsync(con);
			return ds;
		}


		[XmlIgnore]
		public ICommand PulseAnimationCommand { get; set; }
		//public ICommand PulseAnimationCommand { get; set; } = new Microsoft.Maui.Controls.Command(o => { });


		private async Task StatusPing_Proc(object? o)
		{
			CancellationTokenSource ct = (CancellationTokenSource)o!;
			try
			{
				var ds = await GetDeviceStatus();
				if (ct.IsCancellationRequested) return;

#if DEBUG
				ds.Test_SetErrorSate();
#endif

				OnlineDeviceStatus = ds;

				OnPropertyChanged(new PropertyChangedEventArgs(nameof(OnlineDeviceStatus)));
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsOnline)));

				//CancellationTokenSource cts = new();
				//MainThread.BeginInvokeOnMainThread(() => PulseAnimationCommand?.Execute(cts.Token));
				_ = PulseAnimationCommand?.ExecuteForBindedAnimation();

			}
			catch (Exception ex)
			{
				if (ct.IsCancellationRequested) return;
				OnlineDeviceStatus = null;

				OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsOnline)));
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(OnlineDeviceStatus)));

				//OnPulse?.Invoke(this, EventArgs.Empty);

				//$"{AddressString!} FAILED! {ex.Message}".eToastShow();
			}
			finally
			{
				/*
				if (!ct.IsCancellationRequested)
				{
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(OnlineDeviceStatus)));
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsOnline)));
				}
				 */
			}

		}


		public async Task StatusPingStop(bool fast = false)
		{
			_ctsPulse?.Cancel();
			_tmrPulse?.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
			_tmrPulse?.Dispose();

			//	$"StatusPingStop: {AddressString!}".eToastShow();

			await Task.Delay(1);
		}


		[XmlIgnore] public ICommand ClickedAnimationCommand { get; set; }


		[RelayCommand]
		private async Task DeviceConnect()
		{
			var cts = ClickedAnimationCommand?.ExecuteForBindedAnimation();
			await uom.controls.MAUI.Animations.Helpers.WaitForButtonAnimation(800);

			var isGranted = await uom.maui.security.PermissionsHelper.CheckAndRequestPermissionAsync_Net_AccessNetworkStateAndInternet();
			if (!isGranted)
			{
				await Toast
						.Make(E_PERMISSION_NETWORK_ERROR, CommunityToolkit.Maui.Core.ToastDuration.Long, 14)
						.Show();

				return;
			}

			ConnectDevicePage cp = new(this);
			await cp.eShowDialogAsync<MKConnection>(true,
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
