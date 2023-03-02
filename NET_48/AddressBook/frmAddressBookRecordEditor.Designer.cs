namespace malm.AddressBook
{
	partial class frmAddressBookRecordEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddressBookRecordEditor));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.txtUser = new System.Windows.Forms.TextBox();
			this.txtPWD = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtGroup = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.Controls.Add(this.btnCancel, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.btnSave, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtAddress, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtUser, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtPWD, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.txtGroup, 1, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(541, 227);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Location = new System.Drawing.Point(358, 182);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 42);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSave
			// 
			this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnSave.Location = new System.Drawing.Point(444, 182);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(94, 42);
			this.btnSave.TabIndex = 4;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Right;
			this.label1.Location = new System.Drawing.Point(13, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 32);
			this.label1.TabIndex = 5;
			this.label1.Text = "Address:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Right;
			this.label2.Location = new System.Drawing.Point(38, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(47, 32);
			this.label2.TabIndex = 6;
			this.label2.Text = "User:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Right;
			this.label3.Location = new System.Drawing.Point(3, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(82, 32);
			this.label3.TabIndex = 7;
			this.label3.Text = "Password:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtAddress
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.txtAddress, 2);
			this.txtAddress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtAddress.Location = new System.Drawing.Point(91, 3);
			this.txtAddress.MaxLength = 100;
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(447, 26);
			this.txtAddress.TabIndex = 0;
			// 
			// txtUser
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.txtUser, 2);
			this.txtUser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtUser.Location = new System.Drawing.Point(91, 35);
			this.txtUser.MaxLength = 100;
			this.txtUser.Name = "txtUser";
			this.txtUser.Size = new System.Drawing.Size(447, 26);
			this.txtUser.TabIndex = 1;
			// 
			// txtPWD
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.txtPWD, 2);
			this.txtPWD.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPWD.Location = new System.Drawing.Point(91, 67);
			this.txtPWD.MaxLength = 100;
			this.txtPWD.Name = "txtPWD";
			this.txtPWD.PasswordChar = '*';
			this.txtPWD.Size = new System.Drawing.Size(447, 26);
			this.txtPWD.TabIndex = 2;
			this.txtPWD.UseSystemPasswordChar = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Right;
			this.label4.Location = new System.Drawing.Point(27, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 32);
			this.label4.TabIndex = 11;
			this.label4.Text = "Group:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtGroup
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.txtGroup, 2);
			this.txtGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtGroup.Location = new System.Drawing.Point(91, 99);
			this.txtGroup.MaxLength = 100;
			this.txtGroup.Name = "txtGroup";
			this.txtGroup.Size = new System.Drawing.Size(447, 26);
			this.txtGroup.TabIndex = 3;
			// 
			// frmAddressBookRecordEditor
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(573, 259);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAddressBookRecordEditor";
			this.Padding = new System.Windows.Forms.Padding(16);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Addressbook Editor";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private TableLayoutPanel tableLayoutPanel1;
		internal Button btnCancel;
		internal Button btnSave;
		private Label label1;
		private Label label2;
		private Label label3;
		internal TextBox txtAddress;
		internal TextBox txtUser;
		internal TextBox txtPWD;
		private Label label4;
		internal TextBox txtGroup;
	}
}