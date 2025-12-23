using MALM.Model;

using uom.controls.MAUI.Animations;
using uom.maui;

using static MALM.Localization.LStrings;

namespace MALM.Pages;


[QueryProperty(nameof(DeviceToEdit) , C_INPUT_PARAM_KEY)]
partial class DevicesListRecordEditorUI : ContentPage
{

    public const string C_INPUT_PARAM_KEY = "EditDevice";


    public DevicesListRecordEditorUI ()
    {
        InitializeComponent();

        //LocalizeUI
        txtAddress.Placeholder = L_DEVICE_ADDRESS;
        txtPort.Placeholder = $"{L_DEVICE_PORT} ({L_LEAVE_EMPTY_FOR_DEFAULT.ToLower()})";
        txtUser.Placeholder = L_DEVICE_USER;
        txtPassword.Placeholder = L_DEVICE_PASSWORD;
        txtDeviceGroup.Placeholder = L_DEVICE_GROUP;
    }


    public DevicesListRecord? DeviceToEdit { get; set; }


    private async void _Loaded ( object sender , EventArgs e )
    {
        Title = L_DEVICES_LIST_EDITOR;
        if ( DeviceToEdit != null )
        {
            txtAddress.Text = DeviceToEdit!.AddressString;
            txtPort.Text = DeviceToEdit!.PortString;
            txtUser.Text = DeviceToEdit!.UserName;
            txtPassword.Text = DeviceToEdit!.PwdString;
            txtDeviceGroup.Text = DeviceToEdit!.Group;
        }


        Entry[] textFields = [ txtAddress , txtPort , txtUser , txtPassword , txtDeviceGroup ];
        foreach ( var txt in textFields )
        {
            txt.TextChanged += ( _ , _ ) => OnEdited();
        }

        OnEdited();
        await Task.Delay(1);
    }

    private void OnEdited () => btnOk.IsEnabled = ValidateUserInput();

    private bool ValidateUserInput ()
    {
        try
        {
            if ( string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtUser.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) )
                return false;

            if ( !string.IsNullOrWhiteSpace(txtPort.Text) )
            {
                uint port32 = uint.Parse(txtPort.Text.Trim());
                if ( port32 > ushort.MaxValue ) return false;
                //UInt16 port = (UInt16)port32.eCheckRange(0, UInt16.MaxValue);
                //UInt16 port = UInt16.Parse(txtPort.Text);
            }
        }
        catch { return false; }
        return true;
    }



    private async void OnOk ( object sender , EventArgs e )
    {

        if ( !ValidateUserInput() ) return;

        await btnOk.WaitForButtonAnimation();


        DevicesListRecord md = DeviceToEdit!;
        md.AddressString = txtAddress.Text;

        if ( string.IsNullOrWhiteSpace(txtPort.Text) )
        {
            md.PortInt = null;
        }
        else
        {
            uint port32 = uint.Parse(txtPort.Text.Trim());
            ushort port16 = (ushort)port32.checkRange(0 , ushort.MaxValue);
            md.PortInt = port16;
        }

        md.UserName = txtUser.Text;
        md.PwdString = txtPassword.Text;
        md.Group = txtDeviceGroup.Text;

        md.OnPropertyChanged2(nameof(md.AddressString));

        await md.eReturnAsDialogResult();
        /*
		WeakReferenceMessenger.Default.Send(new SendItemMessage(md));
		await Shell.Current.Navigation.PopAsync();
		 */
    }


}