namespace MALM.UI;

using MALM.Model;

using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

using uom.controls.MAUI.Animations;
using uom.maui;

using static MALM.Localization.LStrings;


partial class MasterKeyUI : ContentPage
{

	private async Task<bool> IsBiometricAvailableAsync()
	{
#if ANDROID
		var bp = await uom.maui.security.PermissionsHelper.CheckAndRequestPermissionAsync_Biometric_Fingerprint();
		if (!bp)
		{
			return false;
		}

		//uom.controls.MAUI.Animations.SampleScaleAnimation

#endif
		return await CrossFingerprint.Current.IsAvailableAsync(true);
	}

	private async Task<(bool Available, bool Authenticated)> AuthAuthenticateFingerprintAsync()
	{
		var isAvailable = await IsBiometricAvailableAsync();
		Debug.WriteLine($"IsBiometricAvailableAsync: {isAvailable}");
		if (isAvailable)
		{
			AuthenticationRequestConfiguration request = new(Q_MASTERKEY_BIO_REQ, Q_MASTERKEY_BIO_REQ_2)
			{
				FallbackTitle = Q_MASTERKEY_BIO_USE_PIN,
				AllowAlternativeAuthentication = true,
			};
			var fpResult = await CrossFingerprint.Current.AuthenticateAsync(request);

			return (fpResult.Authenticated, fpResult.Authenticated);

		}
		return (isAvailable, false);
	}


	private async void OnCancel(object sender, EventArgs e)
	{
		await btnCancel.WaitForButtonAnimation();

		switch (_mkm!.LoginMode)
		{
			case MasterKeyManager.LoginModes.Login:
			case MasterKeyManager.LoginModes.InitialSetupMK:
				Application.Current?.Quit();
				break;

			case MasterKeyManager.LoginModes.Edit2:
				await this.eSetDialogResultAndPopBackAsync(false);
				break;
		}
	}
}
