namespace MALM.UI;

using MALM.Model;

using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

using uom.maui;



public partial class MasterKeyUI : ContentPage
{

	private readonly IFingerprint fingerprint;


	/// <summary>EntryPoint for Non Windows Login</summary>
	public MasterKeyUI(IFingerprint fingerprint) : this()
	{
		this.fingerprint = fingerprint;
	}


	private async Task<bool> IsBiometricAvailableAsync() => await CrossFingerprint.Current.IsAvailableAsync(true);

	private async Task<(bool Available, bool Authenticated)> AuthAuthenticateFingerprintAsync()
	{
		var isAvailable = await IsBiometricAvailableAsync();
		Debug.WriteLine($"IsBiometricAvailableAsync: {isAvailable}");
		if (isAvailable)
		{
			AuthenticationRequestConfiguration request = new("Login using biometrics", "Confirm login with your biometrics")
			{
				FallbackTitle = "Use PIN",
				AllowAlternativeAuthentication = true,
			};
			var fpResult = await CrossFingerprint.Current.AuthenticateAsync(request);

			return (fpResult.Authenticated, fpResult.Authenticated);

		}
		return (isAvailable, false);
	}


	/*
private async static Task<(bool, bool)> AuthAuthenticateFingerprintAsync()
{
var isAvailable = await CrossFingerprint.Current.IsAvailableAsync(true);
if (isAvailable)
{
AuthenticationRequestConfiguration request = new("Login using biometrics", "Confirm login with your biometrics");
request.AllowAlternativeAuthentication = true;
var fpResult = await CrossFingerprint.Current.AuthenticateAsync(request);

return (fpResult.Authenticated, true);

}
return (false, false);
}

	 */

	private async void OnCancel(object sender, EventArgs e)
	{
		switch (_mkm!.LoginMode)
		{
			case MasterKeyManager.LoginModes.Login:
			case MasterKeyManager.LoginModes.InitialSetupMK:
				Application.Current?.Quit();
				break;

			case MasterKeyManager.LoginModes.Edit2:
				await this.e_SetDialogResultAndPopBackAsync(false);
				break;
		}
	}
}
