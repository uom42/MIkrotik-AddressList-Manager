#nullable enable

using MALM.Model;


namespace MALM.UI;


internal partial class DevicesListRecordEditorUI : Form
{


	const string C_DDOT = ":";

	private bool _canValidateUserInput = true;


	private DevicesListRecordEditorUI() : base()
	{
		InitializeComponent();

		Text = L_DEVICES_LIST_EDITOR;

		lblDeviceAddress.Text = L_DEVICE_ADDRESS + C_DDOT;
		lblDeviceUser.Text = L_DEVICE_USER + C_DDOT;
		lblDevicePWD.Text = L_DEVICE_PASSWORD + C_DDOT;
		lblDeviceGroup.Text = L_DEVICE_GROUP + C_DDOT;
		lblDevicePort.Text = L_DEVICE_PORT + C_DDOT;

		txtPort.eSetVistaCueBanner(L_LEAVE_EMPTY_FOR_DEFAULT);

		btnCancel.Text = L_CANCEL;

		btnSave.Text = L_OK;
		btnSave.Click += OnSave!;

		this.Load += OnLoad!;
	}

	private void OnLoad(object sender, EventArgs e)
	{
		TextBox[] tb = [txtAddress,
			txtPort,
			txtUser,
			txtPWD,
			txtGroup];

		foreach (var t in tb)
		{
			t.TextChanged += (_, _) => OnValidateUserInput();
		}

		_canValidateUserInput = true;
		OnValidateUserInput();
	}




	public static DevicesListRecordEditorUI InitUI(DevicesListRecord? dev)
	{
		DevicesListRecordEditorUI fe = new();
		if (dev != null)
		{
			fe.txtAddress.Text = dev.AddressString;
			fe.txtPort.Text = dev.PortString;
			fe.txtUser.Text = dev.UserName;
			fe.txtPWD.Text = dev.PwdString;
			fe.txtGroup.Text = dev.Group;
			fe._canValidateUserInput = true;
		}

		return fe;
	}


	private void OnValidateUserInput()
	{
		if (!_canValidateUserInput) return;
		btnSave.Enabled = ValidateUserInput();
	}

	void OnSave(object s, EventArgs e)
	{

		if (ValidateUserInput()) DialogResult = DialogResult.OK;
	}


	private bool ValidateUserInput()
	{
		try
		{
			if (string.IsNullOrWhiteSpace(txtAddress.Text.Trim())) throw new ArgumentNullException(L_DEVICE_ADDRESS);
			if (!string.IsNullOrWhiteSpace(txtPort.Text.Trim()) && !UInt16.TryParse(txtPort.Text.Trim(), out ushort iPort)) throw new ArgumentOutOfRangeException(L_DEVICE_PORT);
			if (string.IsNullOrWhiteSpace(txtUser.Text.Trim())) throw new ArgumentNullException(L_DEVICE_USER);
			return true;
		}
		catch { }
		return false;
	}






}

