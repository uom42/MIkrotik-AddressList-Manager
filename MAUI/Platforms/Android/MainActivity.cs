using Android.App;
using Android.Content.PM;
using Android.OS;

using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace MALM;

[Activity(Label = "@string/app_name", Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{


	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		CrossFingerprint.SetCurrentActivityResolver(() => this);
	}

	/*
protected override async void OnResume()
{
	base.OnResume();
	await Task.Delay(1);

	var isAvailable = await CrossFingerprint.Current.IsAvailableAsync(true);
	if (isAvailable)
	{
		var request = new AuthenticationRequestConfiguration
		("Login using biometrics", "Confirm login with your biometrics");
		request.AllowAlternativeAuthentication = true;
		var result = await CrossFingerprint.Current.AuthenticateAsync(request);
		Console.Write(result);

	}
}
	 */

}
