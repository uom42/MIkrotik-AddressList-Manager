
namespace MALM.UI
{
	partial class MasterKeyUI
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(MasterKeyUI));
			tableLayoutPanel1 = new TableLayoutPanel();
			btnCancel = new Button();
			btnOk = new Button();
			txtMasterKey1 = new TextBox();
			chkRememberMK = new CheckBox();
			txtMasterKey2 = new TextBox();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
			tableLayoutPanel1.Controls.Add(btnCancel, 1, 5);
			tableLayoutPanel1.Controls.Add(btnOk, 2, 5);
			tableLayoutPanel1.Controls.Add(txtMasterKey1, 0, 0);
			tableLayoutPanel1.Controls.Add(chkRememberMK, 0, 4);
			tableLayoutPanel1.Controls.Add(txtMasterKey2, 0, 2);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(18, 20);
			tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 6;
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
			tableLayoutPanel1.Size = new Size(468, 190);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// btnCancel
			// 
			btnCancel.DialogResult = DialogResult.Cancel;
			btnCancel.Dock = DockStyle.Fill;
			btnCancel.Location = new Point(251, 144);
			btnCancel.Margin = new Padding(3, 4, 3, 4);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(94, 42);
			btnCancel.TabIndex = 4;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			btnOk.Dock = DockStyle.Fill;
			btnOk.Location = new Point(351, 144);
			btnOk.Margin = new Padding(3, 4, 3, 4);
			btnOk.Name = "btnOk";
			btnOk.Size = new Size(114, 42);
			btnOk.TabIndex = 3;
			btnOk.Text = "Save";
			btnOk.UseVisualStyleBackColor = true;
			// 
			// txtMasterKey1
			// 
			tableLayoutPanel1.SetColumnSpan(txtMasterKey1, 3);
			txtMasterKey1.Dock = DockStyle.Fill;
			txtMasterKey1.Location = new Point(3, 4);
			txtMasterKey1.Margin = new Padding(3, 4, 3, 4);
			txtMasterKey1.Name = "txtMasterKey1";
			txtMasterKey1.PasswordChar = '*';
			txtMasterKey1.Size = new Size(462, 31);
			txtMasterKey1.TabIndex = 0;
			txtMasterKey1.UseSystemPasswordChar = true;
			// 
			// chkRememberMK
			// 
			chkRememberMK.AutoSize = true;
			tableLayoutPanel1.SetColumnSpan(chkRememberMK, 3);
			chkRememberMK.Dock = DockStyle.Top;
			chkRememberMK.Location = new Point(3, 102);
			chkRememberMK.Margin = new Padding(3, 4, 3, 4);
			chkRememberMK.Name = "chkRememberMK";
			chkRememberMK.Size = new Size(462, 29);
			chkRememberMK.TabIndex = 2;
			chkRememberMK.Text = "Save password";
			chkRememberMK.UseVisualStyleBackColor = true;
			chkRememberMK.Visible = false;
			// 
			// txtMasterKey2
			// 
			tableLayoutPanel1.SetColumnSpan(txtMasterKey2, 3);
			txtMasterKey2.Dock = DockStyle.Top;
			txtMasterKey2.Location = new Point(3, 53);
			txtMasterKey2.Margin = new Padding(3, 4, 3, 4);
			txtMasterKey2.Name = "txtMasterKey2";
			txtMasterKey2.PasswordChar = '*';
			txtMasterKey2.Size = new Size(462, 31);
			txtMasterKey2.TabIndex = 1;
			txtMasterKey2.Visible = false;
			// 
			// MasterKeyUI
			// 
			AcceptButton = btnOk;
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(504, 230);
			Controls.Add(tableLayoutPanel1);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(3, 4, 3, 4);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "MasterKeyUI";
			Padding = new Padding(18, 20, 18, 20);
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Master Password";
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private TableLayoutPanel tableLayoutPanel1;
		internal Button btnCancel;
		internal Button btnOk;
		internal TextBox txtMasterKey1;
		internal CheckBox chkRememberMK;
		private TextBox txtMasterKey2;
	}
}