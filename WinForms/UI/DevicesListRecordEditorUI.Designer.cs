

namespace MALM.UI
{
	partial class DevicesListRecordEditorUI
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(DevicesListRecordEditorUI));
			tableLayoutPanel1 = new TableLayoutPanel();
			btnCancel = new Button();
			btnSave = new Button();
			lblDeviceAddress = new Label();
			lblDeviceUser = new Label();
			lblDevicePWD = new Label();
			txtAddress = new TextBox();
			txtUser = new TextBox();
			txtPWD = new TextBox();
			lblDeviceGroup = new Label();
			txtGroup = new TextBox();
			lblDevicePort = new Label();
			txtPort = new TextBox();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 111F));
			tableLayoutPanel1.Controls.Add(btnCancel, 1, 6);
			tableLayoutPanel1.Controls.Add(btnSave, 2, 6);
			tableLayoutPanel1.Controls.Add(lblDeviceAddress, 0, 0);
			tableLayoutPanel1.Controls.Add(lblDeviceUser, 0, 2);
			tableLayoutPanel1.Controls.Add(lblDevicePWD, 0, 3);
			tableLayoutPanel1.Controls.Add(txtAddress, 1, 0);
			tableLayoutPanel1.Controls.Add(txtUser, 1, 2);
			tableLayoutPanel1.Controls.Add(txtPWD, 1, 3);
			tableLayoutPanel1.Controls.Add(lblDeviceGroup, 0, 4);
			tableLayoutPanel1.Controls.Add(txtGroup, 1, 4);
			tableLayoutPanel1.Controls.Add(lblDevicePort, 0, 1);
			tableLayoutPanel1.Controls.Add(txtPort, 1, 1);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(18, 20);
			tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 7;
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
			tableLayoutPanel1.Size = new Size(601, 310);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// btnCancel
			// 
			btnCancel.DialogResult = DialogResult.Cancel;
			btnCancel.Dock = DockStyle.Right;
			btnCancel.Location = new Point(340, 254);
			btnCancel.Margin = new Padding(3, 4, 3, 4);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(147, 52);
			btnCancel.TabIndex = 6;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSave
			// 
			btnSave.Dock = DockStyle.Right;
			btnSave.Location = new Point(494, 254);
			btnSave.Margin = new Padding(3, 4, 3, 4);
			btnSave.Name = "btnSave";
			btnSave.Size = new Size(104, 52);
			btnSave.TabIndex = 5;
			btnSave.Text = "Save";
			btnSave.UseVisualStyleBackColor = true;
			// 
			// lblDeviceAddress
			// 
			lblDeviceAddress.AutoSize = true;
			lblDeviceAddress.Dock = DockStyle.Right;
			lblDeviceAddress.Location = new Point(13, 0);
			lblDeviceAddress.Name = "lblDeviceAddress";
			lblDeviceAddress.Size = new Size(81, 39);
			lblDeviceAddress.TabIndex = 5;
			lblDeviceAddress.Text = "Address:";
			lblDeviceAddress.TextAlign = ContentAlignment.MiddleRight;
			// 
			// lblDeviceUser
			// 
			lblDeviceUser.AutoSize = true;
			lblDeviceUser.Dock = DockStyle.Right;
			lblDeviceUser.Location = new Point(43, 78);
			lblDeviceUser.Name = "lblDeviceUser";
			lblDeviceUser.Size = new Size(51, 39);
			lblDeviceUser.TabIndex = 6;
			lblDeviceUser.Text = "User:";
			lblDeviceUser.TextAlign = ContentAlignment.MiddleRight;
			// 
			// lblDevicePWD
			// 
			lblDevicePWD.AutoSize = true;
			lblDevicePWD.Dock = DockStyle.Right;
			lblDevicePWD.Location = new Point(3, 117);
			lblDevicePWD.Name = "lblDevicePWD";
			lblDevicePWD.Size = new Size(91, 39);
			lblDevicePWD.TabIndex = 7;
			lblDevicePWD.Text = "Password:";
			lblDevicePWD.TextAlign = ContentAlignment.MiddleRight;
			// 
			// txtAddress
			// 
			tableLayoutPanel1.SetColumnSpan(txtAddress, 2);
			txtAddress.Dock = DockStyle.Fill;
			txtAddress.Location = new Point(100, 4);
			txtAddress.Margin = new Padding(3, 4, 3, 4);
			txtAddress.MaxLength = 100;
			txtAddress.Name = "txtAddress";
			txtAddress.Size = new Size(498, 31);
			txtAddress.TabIndex = 0;
			// 
			// txtUser
			// 
			tableLayoutPanel1.SetColumnSpan(txtUser, 2);
			txtUser.Dock = DockStyle.Fill;
			txtUser.Location = new Point(100, 82);
			txtUser.Margin = new Padding(3, 4, 3, 4);
			txtUser.MaxLength = 100;
			txtUser.Name = "txtUser";
			txtUser.Size = new Size(498, 31);
			txtUser.TabIndex = 2;
			// 
			// txtPWD
			// 
			tableLayoutPanel1.SetColumnSpan(txtPWD, 2);
			txtPWD.Dock = DockStyle.Fill;
			txtPWD.Location = new Point(100, 121);
			txtPWD.Margin = new Padding(3, 4, 3, 4);
			txtPWD.MaxLength = 100;
			txtPWD.Name = "txtPWD";
			txtPWD.PasswordChar = '*';
			txtPWD.Size = new Size(498, 31);
			txtPWD.TabIndex = 3;
			txtPWD.UseSystemPasswordChar = true;
			// 
			// lblDeviceGroup
			// 
			lblDeviceGroup.AutoSize = true;
			lblDeviceGroup.Dock = DockStyle.Right;
			lblDeviceGroup.Location = new Point(28, 156);
			lblDeviceGroup.Name = "lblDeviceGroup";
			lblDeviceGroup.Size = new Size(66, 39);
			lblDeviceGroup.TabIndex = 11;
			lblDeviceGroup.Text = "Group:";
			lblDeviceGroup.TextAlign = ContentAlignment.MiddleRight;
			// 
			// txtGroup
			// 
			tableLayoutPanel1.SetColumnSpan(txtGroup, 2);
			txtGroup.Dock = DockStyle.Fill;
			txtGroup.Location = new Point(100, 160);
			txtGroup.Margin = new Padding(3, 4, 3, 4);
			txtGroup.MaxLength = 100;
			txtGroup.Name = "txtGroup";
			txtGroup.Size = new Size(498, 31);
			txtGroup.TabIndex = 4;
			// 
			// lblDevicePort
			// 
			lblDevicePort.AutoSize = true;
			lblDevicePort.Dock = DockStyle.Right;
			lblDevicePort.Location = new Point(46, 39);
			lblDevicePort.Name = "lblDevicePort";
			lblDevicePort.Size = new Size(48, 39);
			lblDevicePort.TabIndex = 12;
			lblDevicePort.Text = "Port:";
			lblDevicePort.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// txtPort
			// 
			tableLayoutPanel1.SetColumnSpan(txtPort, 2);
			txtPort.Dock = DockStyle.Fill;
			txtPort.Location = new Point(100, 43);
			txtPort.Margin = new Padding(3, 4, 3, 4);
			txtPort.MaxLength = 10;
			txtPort.Name = "txtPort";
			txtPort.Size = new Size(498, 31);
			txtPort.TabIndex = 1;
			// 
			// DevicesListRecordEditorUI
			// 
			AcceptButton = btnSave;
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(637, 350);
			Controls.Add(tableLayoutPanel1);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(3, 4, 3, 4);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "DevicesListRecordEditorUI";
			Padding = new Padding(18, 20, 18, 20);
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Addressbook Editor";
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			ResumeLayout(false);
		}

		#endregion
		private TableLayoutPanel tableLayoutPanel1;
		internal Button btnCancel;
		internal Button btnSave;
		internal Label lblDeviceAddress;
		internal Label lblDeviceUser;
		internal Label lblDevicePWD;
		internal TextBox txtAddress;
		internal TextBox txtUser;
		internal TextBox txtPWD;
		internal Label lblDeviceGroup;
		internal TextBox txtGroup;
		internal Label lblDevicePort;
		internal TextBox txtPort;
	}
}