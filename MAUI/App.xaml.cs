using Android.Runtime;

using MALM.Model;
using MALM.UI;

using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

using static MALM.Localization.LStrings;

namespace MALM;

public partial class App : Application
{


	internal string? _DevID;

	public App(uom.maui.IGetDeviceInfo getDeviceInfo)
	{
		_ = new MauiIcons.Core.MauiIcon();

		InitializeComponent();

		_DevID = getDeviceInfo.GetDeviceID();
		MainPage = new AppShell();
	}

	/*
	protected override async void OnStart()
	{
		await Task.Delay(2000);
		var isBiometricAvailable = await CrossFingerprint.Current.IsAvailableAsync();
		if (isBiometricAvailable)
		{

			AuthenticationRequestConfiguration request = new(Q_MASTERKEY_BIO_REQ, "Confirm login with your biometrics")
			{
				FallbackTitle = "Use PIN",
				AllowAlternativeAuthentication = true,
			};
			var authResult = await CrossFingerprint.Current.AuthenticateAsync(request);

			if (authResult.Authenticated)
			{
				MainPage = new AppShell();
			}
			else
			{
				MainPage = new AppShell();
			}
		}
		else
		{
			MainPage = new AppShell();
		}
}
	 */
}

