namespace malm.Core
{
	partial class frmAddressListItemEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddressListItemEditor));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lblGroup = new System.Windows.Forms.Label();
			this.lblAddress = new System.Windows.Forms.Label();
			this.lblComment = new System.Windows.Forms.Label();
			this.cboGroup = new System.Windows.Forms.ComboBox();
			this.cboAddress = new System.Windows.Forms.ComboBox();
			this.txtComment = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
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
			this.tableLayoutPanel1.Controls.Add(this.btnAdd, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.lblGroup, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblAddress, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblComment, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.cboGroup, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.cboAddress, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtComment, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1002, 229);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Location = new System.Drawing.Point(819, 184);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 42);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnAdd
			// 
			this.btnAdd.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnAdd.Location = new System.Drawing.Point(905, 184);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(94, 42);
			this.btnAdd.TabIndex = 4;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			// 
			// lblGroup
			// 
			this.lblGroup.AutoSize = true;
			this.lblGroup.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblGroup.Location = new System.Drawing.Point(3, 52);
			this.lblGroup.Name = "lblGroup";
			this.lblGroup.Size = new System.Drawing.Size(155, 34);
			this.lblGroup.TabIndex = 11;
			this.lblGroup.Text = "Mikrotik List (Group):";
			this.lblGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblAddress
			// 
			this.lblAddress.AutoSize = true;
			this.lblAddress.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblAddress.Location = new System.Drawing.Point(86, 86);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(72, 34);
			this.lblAddress.TabIndex = 5;
			this.lblAddress.Text = "Address:";
			this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblComment
			// 
			this.lblComment.AutoSize = true;
			this.lblComment.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblComment.Location = new System.Drawing.Point(76, 120);
			this.lblComment.Name = "lblComment";
			this.lblComment.Size = new System.Drawing.Size(82, 32);
			this.lblComment.TabIndex = 6;
			this.lblComment.Text = "Comment:";
			this.lblComment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cboGroup
			// 
			this.cboGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cboGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.tableLayoutPanel1.SetColumnSpan(this.cboGroup, 2);
			this.cboGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboGroup.ItemHeight = 20;
			this.cboGroup.Location = new System.Drawing.Point(164, 55);
			this.cboGroup.MaxDropDownItems = 50;
			this.cboGroup.Name = "cboGroup";
			this.cboGroup.Size = new System.Drawing.Size(835, 28);
			this.cboGroup.TabIndex = 3;
			// 
			// cboAddress
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.cboAddress, 2);
			this.cboAddress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboAddress.Location = new System.Drawing.Point(164, 89);
			this.cboAddress.MaxLength = 100;
			this.cboAddress.Name = "cboAddress";
			this.cboAddress.Size = new System.Drawing.Size(835, 28);
			this.cboAddress.TabIndex = 0;
			// 
			// txtComment
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.txtComment, 2);
			this.txtComment.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtComment.Location = new System.Drawing.Point(164, 123);
			this.txtComment.MaxLength = 100;
			this.txtComment.Name = "txtComment";
			this.txtComment.Size = new System.Drawing.Size(835, 26);
			this.txtComment.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.SystemColors.Info;
			this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(16);
			this.label1.Size = new System.Drawing.Size(996, 52);
			this.label1.TabIndex = 12;
			this.label1.Text = "New element will be created in disabled state!";
			// 
			// frmAddressListItemEditor
			// 
			this.AcceptButton = this.btnAdd;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(1034, 261);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAddressListItemEditor";
			this.Padding = new System.Windows.Forms.Padding(16);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AddressList Editor";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private TableLayoutPanel tableLayoutPanel1;
		internal Button btnCancel;
		internal Button btnAdd;
		private Label lblAddress;
		private Label lblComment;
		internal ComboBox cboAddress;
		internal TextBox txtComment;
		private Label lblGroup;
		internal ComboBox cboGroup;
		private Label label1;
	}
}