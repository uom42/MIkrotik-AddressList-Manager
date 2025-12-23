#nullable enable

using MALM.Model;

using static MALM.Localization.LStrings;


#if WINDOWS
//
#else
using Android.OS;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Alerts;
using uom.maui;
using uom.controls.MAUI.Animations;
#endif

namespace MALM.UI
{

    internal partial class MasterKeyUI
    {

        private MasterKeyManager? _mkm;

#if WINDOWS
        private DevicesListRecord[] LoginResult = [];
#endif
        private bool _isBiometricLoginOk = false;


        public MasterKeyUI () : base()
        {
            InitializeComponent();
            LocalizeUI();
            AttachUIEvents();
        }


        /// <summary>EntryPoint ONLY FOR Windows Login</summary>
        internal MasterKeyUI ( MasterKeyManager mkm ) : this() { _mkm = mkm; }

        private void LocalizeUI ()
        {

#if !WINDOWS
            Title = L_MASTER_KEY;
#else
            Text = L_MASTER_KEY;
#endif

            btnCancel.Text = L_CANCEL;
        }

        private void AttachUIEvents ()
        {

#if !WINDOWS
            Loaded += async ( s , e ) => await InitOnLoad();

            btnOk.Clicked += OnOk!;
            btnCancel.Clicked += OnCancel!;

#else
            Load += async ( _ , _ ) => await InitOnLoad();
            btnOk.Click += OnOk!;

            txtMasterKey2.TextChanged += ( _ , _ ) => OnEdited();
#endif
            txtMasterKey1.TextChanged += ( _ , _ ) => OnEdited();


            //var bb = txtMasterKey1.Behaviors;
            //int hhhh = 8;

            //btnOk.Clicked

        }

        private async Task InitOnLoad ()
        {

#if !WINDOWS
            Entry[] txt;
#else
            TextBox[] txt;
#endif
            txt = [ txtMasterKey1 , txtMasterKey2 ];
            foreach ( var t in txt )
            {
                t.MaxLength = MasterKeyManager.MAX_MASTERKEY_LEN;
                t.Text = string.Empty;
#if WINDOWS
                t.UseSystemPasswordChar = true;
#endif
            }

#if WINDOWS
            btnCancel.Text = L_CANCEL;

            txtMasterKey1.SetCueBanner(L_MASTER_KEY);
            txtMasterKey2.SetCueBanner($"{L_REPEAT} {L_MASTER_KEY.ToLower()}");
#else
            txtMasterKey1.Placeholder = L_MASTER_KEY;
            txtMasterKey2.Placeholder = $"{L_REPEAT} {L_MASTER_KEY.ToLower()}";
#endif
            chkRememberMK.Checked = false;

            //btnOk.Text = L_OK;
            await InitUIFromLoginMode();
        }

        private async Task InitUIFromLoginMode ()
        {
            _isBiometricLoginOk = false;
            chkRememberMK.Text = L_MASTER_KEY_CACHING;

            try
            {
                _mkm ??= await MasterKeyManager.Load();
            }
            catch ( Exception ex )
            {
#if !WINDOWS
                await
#endif
                ex.eLogError(true);
            }

            btnOk.Text = _mkm!.LoginMode switch
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


            switch ( _mkm.LoginMode )
            {
                case MasterKeyManager.LoginModes.Login:
                    {
#if !WINDOWS
                        try
                        {
                            if ( _mkm.CanAutologin )
                            {
                                if ( !await _mkm.IsKeyValidForDatabase() ) throw new Exception("Invalid cached Masterkey");

                                _isBiometricLoginOk = (await AuthAuthenticateFingerprintAsync()).Authenticated;
                                if ( !_isBiometricLoginOk ) throw new Exception("Invalid fingerprint auth");

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
                        bool isInitialSetup = _mkm.LoginMode == MasterKeyManager.LoginModes.InitialSetupMK;
                        if ( isInitialSetup )
                        {
#if !WINDOWS
                            Entry[] txt;
#else
                            TextBox[] txt;
#endif
                            txt = [ txtMasterKey1 , txtMasterKey2 ];
                            foreach ( var t in txt )
                            {
                                t.Text = string.Empty;
                            }
                        }

#if !WINDOWS
                        chkRememberMK.Text = !await IsBiometricAvailableAsync()
                            ? L_MASTER_KEY_CACHING
                            : L_MASTER_KEY_CACHING_USE_BIO;


#endif
                        txtMasterKey2.TextChanged += ( _ , _ ) => OnEdited();

                        chkRememberMK.Checked = _mkm.CanAutologin;

                        txtMasterKey1.Focus();
                        break;
                    }
            }

            OnEdited();
        }



        #region Validate Usert Input


#if !WINDOWS
        private void OnEdited () => btnOk.IsEnabled = ValidateUserInput();
#else
        private void OnEdited () => btnOk.Enabled = (txtMasterKey2.Visible == false && txtMasterKey2.Text.isNotNullOrWhiteSpace) || ValidateUserInput();
#endif


        private bool ValidateUserInput ()
        {
            return !txtMasterKey1.Text.isNullOrWhiteSpace && (_mkm!.LoginMode == MasterKeyManager.LoginModes.Login || txtMasterKey1.Text == txtMasterKey2.Text);
        }



        #endregion


        private async void OnOk ( object sender , EventArgs e ) => await OnOk();
        private async Task OnOk ()
        {
#if !WINDOWS
            await btnOk.WaitForButtonAnimation();
#endif

            switch ( _mkm!.LoginMode )
            {
                case MasterKeyManager.LoginModes.Login:
                    {
                        try
                        {
                            if ( !_isBiometricLoginOk ) _mkm.InitFromUI(txtMasterKey1);
                        }
                        catch ( Exception ex )
                        {
#if !WINDOWS
                            ex.eLogErrorToast();
#else
                            ex.eLogError(true , E_TITLE_LOGIN_FAILED);
#endif
                            return;
                        }

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

                            uom.maui.ui.KeyboardHelper.HideKeyboard();

                            LoginResult lr = new(_mkm , rows);
                            var devListUI = new DevicesListUI(lr);
                            await Shell.Current.Navigation.PushAsync(devListUI);
#endif
                            return;
                        }

                        //logon failed. 
                        catch ( Exception ex )
                        {
#if WINDOWS
                            ex.eLogError(false);
#endif
                            //Checking wrong logon count before display ResetPasswordUI...
                            if ( !_mkm.IsExceededBadLogonCount )
                            {
                                string errMsg = ex switch
                                {
                                    CryptographicException cex => E_ENCRYPT_FAILED,
                                    InvalidOperationException ioex => E_ENCRYPT_FAILED,
                                    _ => ex.Message
                                };

                                Exception err = new(errMsg , ex);
#if !WINDOWS
                                err.eLogErrorToast();
                                //await DisplayAlert(E_TITLE_LOGIN_FAILED, err, L_OK);
#else
                                err.eLogError(true , E_TITLE_LOGIN_FAILED);
#endif
                                return;
                            }

                            //Bad logon limit exceeeded!
#if !WINDOWS
                            bool result = await DisplayAlert(E_ENCRYPT_FAILED , Q_RESET_MASTER_KEY.eWrap() , L_YES , L_NO);
#else
                            bool result = Q_RESET_MASTER_KEY.eWrap().eMsgboxAskIsYes(false , E_ENCRYPT_FAILED);
#endif

                            if ( !result ) return;

                            //Need to reset pawsword!
                            _mkm.SetPasswordResetMode();
                            await InitUIFromLoginMode();
                            return;

                        }

                        //break;
                    }


                case MasterKeyManager.LoginModes.Edit2:
                case MasterKeyManager.LoginModes.InitialSetupMK:
                    {
                        if ( !ValidateUserInput() ) return;
                        bool isInitialSetup = _mkm.LoginMode == MasterKeyManager.LoginModes.InitialSetupMK;

                        try
                        {
                            _mkm!.InitFromUI(txtMasterKey1 , chkRememberMK);
                            await _mkm.Save();
                        }
                        catch ( Exception ex )
                        {
#if !WINDOWS
                            ex.eLogErrorToast();
#else
                            ex.eLogError(true);
#endif
                            return;
                        }

#if !WINDOWS

                        uom.maui.ui.KeyboardHelper.HideKeyboard();

                        if ( isInitialSetup )
                        {
                            var rows = await _mkm.Database_WriteEncryptedEmpty();// Write an empty Datatase.
                            LoginResult lr = new(_mkm , rows);
                            var devListUI = new DevicesListUI(lr);
                            await Shell.Current.Navigation.PushAsync(devListUI);
                        }
                        else
                        {
                            var ns = Shell.Current.Navigation.NavigationStack;
                            await this.eSetDialogResultAndPopBackAsync(true);
                        }
#else
                        if ( isInitialSetup )
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
        internal static async Task<bool> EditKey ( MasterKeyManager mkm )
#else
        internal static bool EditKey ( MasterKeyManager mkm , Form parent )
#endif
        {
            MasterKeyUI ui = new(mkm)
#if !ANDROID
            {
                StartPosition = FormStartPosition.CenterParent ,
                ShowInTaskbar = false
            }
#endif
        ;

#if !WINDOWS
            var isOk = await ui.eShowDialogAsync<bool>(true);
#else
            var isOk = ui.ShowDialog(parent) == DialogResult.OK;
#endif
            return isOk;
        }
    }
}