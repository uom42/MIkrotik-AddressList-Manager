﻿#nullable enable

using static MALM.Localization.Strings;

using MALM.Model;


#if !WINDOWS

using Android.OS;
using CommunityToolkit.Maui.Markup;

using uom.maui;


#else

#endif

namespace MALM.UI
{

	partial class MasterKeyUI
	{

		private MasterKeyManager? _mkm;

#if WINDOWS
		private DevicesListRecord[] LoginResult = [];
#endif
		private bool _isBiometricLoginOk = false;


		private MasterKeyUI() : base()
		{
			InitializeComponent();
			LocalizeUI();
			AttachUIEvents();
		}


		/// <summary>EntryPoint ONLY FOR Windows Login</summary>
		internal MasterKeyUI(MasterKeyManager mkm) : this() { _mkm = mkm; }

		private void LocalizeUI()
		{

#if !WINDOWS
			Title = L_MASTER_KEY;
#else
			Text = L_MASTER_KEY;
#endif

			btnCancel.Text = L_CANCEL;
		}

		private void AttachUIEvents()
		{

#if !WINDOWS
			this.Loaded += async (s, e) => await InitOnLoad();

			btnOk.Clicked += OnOk!;
			btnCancel.Clicked += OnCancel!;

#else
			this.Load += async (_, _) => await InitOnLoad();
			btnOk.Click += OnOk!;

			txtMasterKey2.TextChanged += (_, _) => OnEdited();
#endif
			txtMasterKey1.TextChanged += (_, _) => OnEdited();

		}

		private async Task InitOnLoad()
		{

#if !WINDOWS
			Entry[] txt;
#else
			TextBox[] txt;
#endif

			txt = [txtMasterKey1, txtMasterKey2];
			foreach (var t in txt)
			{
				t.MaxLength = MasterKeyManager.MAX_MASTERKEY_LEN;
				t.Text = string.Empty;
#if WINDOWS
				//t.PasswordChar = '*';
				t.UseSystemPasswordChar = true;
#else
				t.IsPassword = true;
#endif
			}

#if WINDOWS
			btnCancel.Text = L_CANCEL;

			txtMasterKey1.e_SetVistaCueBanner(L_MASTER_KEY);
			txtMasterKey2.e_SetVistaCueBanner($"{L_REPEAT} {L_MASTER_KEY.ToLower()}");
#else
			txtMasterKey1.Placeholder = L_MASTER_KEY;
			txtMasterKey2.Placeholder = $"{L_REPEAT} {L_MASTER_KEY.ToLower()}";
#endif
			chkRememberMK.Checked = false;

			//btnOk.Text = L_OK;

			await InitUIFromLoginMode();
		}

		private async Task InitUIFromLoginMode()
		{
			_isBiometricLoginOk = false;
			chkRememberMK.Text = L_MASTER_KEY_CACHING;

			_mkm ??= await MasterKeyManager.Load();

			btnOk.Text = _mkm.LoginMode switch
			{
				MasterKeyManager.LoginModes.InitialSetupMK => $"{L_SET} {L_MASTER_KEY.ToLower()}",
				MasterKeyManager.LoginModes.Edit2 => L_APPLY,
				_ => L_OK
			};


			bool _2EditControls = _mkm.LoginMode != MasterKeyManager.LoginModes.Login;
#if !WINDOWS

			txtMasterKey2.IsVisible = _2EditControls;
			chkRememberMK.IsVisible = _2EditControls;
#else
			txtMasterKey2.Visible = _2EditControls;
			chkRememberMK.Visible = _2EditControls;
#endif

			switch (_mkm.LoginMode)
			{
				case MasterKeyManager.LoginModes.Login:
					{
#if !WINDOWS
						/*
						txtMasterKey2.IsVisible = false;
						chkRememberMK.IsVisible = false;
						 */

						try
						{
							if (_mkm.CanAutologin)
							{
								if (!await _mkm.IsKeyValidForDatabase()) throw new Exception("Invalid cached Masterkey");

								_isBiometricLoginOk = (await AuthAuthenticateFingerprintAsync()).Authenticated;
								if (!_isBiometricLoginOk) throw new Exception("Invalid fingerprint auth");

								await OnOk();
								return;
							}
						}
						catch
						{
							//Autologon failed. Need Ask user for password.
						}
#endif

						break;
					}

				case MasterKeyManager.LoginModes.Edit2:
				case MasterKeyManager.LoginModes.InitialSetupMK:
					{
#if !WINDOWS
						chkRememberMK.Text = !(await IsBiometricAvailableAsync())
							? L_MASTER_KEY_CACHING
							: L_MASTER_KEY_CACHING_USE_BIO;
#endif
						txtMasterKey2.TextChanged += (_, _) => OnEdited();
						//_mkm!.FillTextBoxes(txtMasterKey1, txtMasterKey2);

						chkRememberMK.Checked = _mkm.CanAutologin;

						txtMasterKey1.Focus();
						break;
					}
			}

			OnEdited();
		}



		#region Validate Usert Input


#if !WINDOWS
		private void OnEdited() => btnOk.IsEnabled = ValidateUserInput();
#else
		private void OnEdited() => btnOk.Enabled = (txtMasterKey2.Visible == false && txtMasterKey2.Text.e_IsNOTNullOrWhiteSpace()) || ValidateUserInput();
#endif


		private bool ValidateUserInput()
		{
			if (txtMasterKey1.Text.e_IsNullOrWhiteSpace()) return false;

			if (_mkm!.LoginMode == MasterKeyManager.LoginModes.Login) return true;
			if (txtMasterKey1.Text != txtMasterKey2.Text) return false;
			return true;
		}



		#endregion


		private async void OnOk(object sender, EventArgs e) => await OnOk();
		private async Task OnOk()
		{
			switch (_mkm!.LoginMode)
			{
				case MasterKeyManager.LoginModes.Login:
					{
						if (!_isBiometricLoginOk) _mkm.InitFromUI(txtMasterKey1);
						try
						{
							//Trying to load and decrypt database with old saved Masterkey...
							DevicesListRecord[]? rows = await _mkm.Database_Load();

							//Decrypted OK or Database file not exist
							rows ??= await _mkm.Database_WriteEncryptedEmpty();// In case database file not exist - save empty Datatase.
#if WINDOWS
							await _mkm.Save();
							LoginResult = rows;
							DialogResult = DialogResult.OK;
#else
							//if (!_isBiometricLoginOk) await _mkm.Save();//Don't save any changes while logon using biometric login

							LoginResult lr = new(_mkm, rows);
							var devListUI = new DevicesListUI(lr);
							await Shell.Current.Navigation.PushAsync(devListUI);

							//await this.e_SetDialogResultAndPopBackAsync(true);
#endif
							return;
						}

						//logon failed. 
						catch (Exception ex)
						{
#if WINDOWS
							ex.e_LogError(false, supressAnyModalPopEvenInDEBUG: true);
#endif
							//Checking wrong logon count before display ResetPasswordUI...
							if (!_mkm.IsExceededBadLogonCount)
							{
								string err = ex switch
								{
									CryptographicException cex => E_ENCRYPT_FAILED,
									InvalidOperationException ioex => E_ENCRYPT_FAILED,
									_ => ex.Message
								};

#if !WINDOWS
								await DisplayAlert(E_TITLE_DEFAULT, err, L_OK);
#else
								err.e_MsgboxShow(Extensions_UI_Win.MsgBoxFlags.Btn_OK | Extensions_UI_Win.MsgBoxFlags.Icn_Error, E_TITLE_DEFAULT);
#endif
								return;
							}

							//Bad logon limit exceeeded!
#if !WINDOWS
							bool result = await DisplayAlert(E_ENCRYPT_FAILED, Q_RESET_MASTER_KEY.e_Wrap(), L_YES, L_NO);
#else
							bool result = Q_RESET_MASTER_KEY.e_Wrap().e_MsgboxAskIsYes(false, E_ENCRYPT_FAILED);
#endif

							if (!result) return;

							//Need to reset pawsword!
							//await 							//.Reset(false);
							_mkm.SetPasswordResetMode();
							await InitUIFromLoginMode();
							return;

						}

						break;
					}


				case MasterKeyManager.LoginModes.Edit2:
				case MasterKeyManager.LoginModes.InitialSetupMK:
					{
						if (!ValidateUserInput()) return;
						bool isInitialSetup = _mkm.LoginMode == MasterKeyManager.LoginModes.InitialSetupMK;

						_mkm!.InitFromUI(txtMasterKey1, chkRememberMK);
						await _mkm.Save();

#if !WINDOWS
						var ns = Shell.Current.Navigation.NavigationStack;
						await this.e_SetDialogResultAndPopBackAsync(true);
#else
						if (isInitialSetup)
						{
							//Write empty database
							LoginResult = await _mkm.Database_WriteEncryptedEmpty();
						}
						DialogResult = DialogResult.OK;
#endif
						break;
					}
			}
		}




		/// <summary>Display UI for login or Reset masterkey</summary>
		/// <returns><see langword="false"/> when user canceled edit key, or <see langword="true"/> when ok</returns>
#if !WINDOWS
		internal static async Task<bool> EditKey(MasterKeyManager mkm)
#else
		internal static bool EditKey(MasterKeyManager mkm, Form parent)
#endif
		{
			MasterKeyUI ui = new(mkm)
#if !ANDROID
			{
				StartPosition = FormStartPosition.CenterParent,
				ShowInTaskbar = false
			}
#endif
		;

#if !WINDOWS
			var isOk = await ui.e_ShowDialogAsync<bool>(true);
#else
			var isOk = ui.ShowDialog(parent) == DialogResult.OK;
#endif
			return isOk;
		}
	}
}