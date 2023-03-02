namespace malm.AddressBook
{
	partial class frmMasterKey
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMasterKey));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.lblMasterPwd = new System.Windows.Forms.Label();
			this.txtKey = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.Controls.Add(this.btnCancel, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnOk, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblMasterPwd, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtKey, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(433, 108);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Location = new System.Drawing.Point(230, 63);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 42);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnOk.Location = new System.Drawing.Point(336, 63);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(94, 42);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Save";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// lblMasterPwd
			// 
			this.lblMasterPwd.AutoSize = true;
			this.lblMasterPwd.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblMasterPwd.Location = new System.Drawing.Point(3, 0);
			this.lblMasterPwd.Name = "lblMasterPwd";
			this.lblMasterPwd.Size = new System.Drawing.Size(135, 32);
			this.lblMasterPwd.TabIndex = 5;
			this.lblMasterPwd.Text = "Master Password:";
			this.lblMasterPwd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtKey
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.txtKey, 2);
			this.txtKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtKey.Location = new System.Drawing.Point(144, 3);
			this.txtKey.Name = "txtKey";
			this.txtKey.PasswordChar = '*';
			this.txtKey.Size = new System.Drawing.Size(286, 26);
			this.txtKey.TabIndex = 0;
			this.txtKey.UseSystemPasswordChar = true;
			// 
			// frmMasterKey
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(465, 140);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMasterKey";
			this.Padding = new System.Windows.Forms.Padding(16);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Master Password";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private TableLayoutPanel tableLayoutPanel1;
		internal Button btnCancel;
		internal Button btnOk;
		internal TextBox txtKey;
		internal Label lblMasterPwd;
	}
}