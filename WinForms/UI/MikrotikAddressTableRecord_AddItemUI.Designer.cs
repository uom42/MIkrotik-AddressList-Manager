namespace MALM.UI
{
	partial class MikrotikAddressTableRecord_AddItemUI
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(MikrotikAddressTableRecord_AddItemUI));
			tableLayoutPanel1 = new TableLayoutPanel();
			btnCancel = new Button();
			btnAdd = new Button();
			lblGroup = new Label();
			lblAddress = new Label();
			lblComment = new Label();
			cboGroup = new ComboBox();
			cboAddress = new ComboBox();
			txtComment = new TextBox();
			label1 = new Label();
			grpSources = new GroupBox();
			lvwSources = new ListViewEx();
			colAddress = new ColumnHeader();
			colComment = new ColumnHeader();
			tableLayoutPanel1.SuspendLayout();
			grpSources.SuspendLayout();
			SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 111F));
			tableLayoutPanel1.Controls.Add(btnCancel, 1, 5);
			tableLayoutPanel1.Controls.Add(btnAdd, 2, 5);
			tableLayoutPanel1.Controls.Add(lblGroup, 0, 1);
			tableLayoutPanel1.Controls.Add(lblAddress, 0, 2);
			tableLayoutPanel1.Controls.Add(lblComment, 0, 3);
			tableLayoutPanel1.Controls.Add(cboGroup, 1, 1);
			tableLayoutPanel1.Controls.Add(cboAddress, 1, 2);
			tableLayoutPanel1.Controls.Add(txtComment, 1, 3);
			tableLayoutPanel1.Controls.Add(label1, 0, 0);
			tableLayoutPanel1.Controls.Add(grpSources, 0, 4);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(18, 20);
			tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 6;
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle());
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
			tableLayoutPanel1.Size = new Size(1113, 747);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// btnCancel
			// 
			btnCancel.DialogResult = DialogResult.Cancel;
			btnCancel.Dock = DockStyle.Right;
			btnCancel.Location = new Point(829, 691);
			btnCancel.Margin = new Padding(3, 4, 3, 4);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(170, 52);
			btnCancel.TabIndex = 5;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnAdd
			// 
			btnAdd.Dock = DockStyle.Right;
			btnAdd.Location = new Point(1006, 691);
			btnAdd.Margin = new Padding(3, 4, 3, 4);
			btnAdd.Name = "btnAdd";
			btnAdd.Size = new Size(104, 52);
			btnAdd.TabIndex = 4;
			btnAdd.Text = "Add";
			btnAdd.UseVisualStyleBackColor = true;
			// 
			// lblGroup
			// 
			lblGroup.AutoSize = true;
			lblGroup.Dock = DockStyle.Right;
			lblGroup.Location = new Point(3, 65);
			lblGroup.Name = "lblGroup";
			lblGroup.Size = new Size(177, 41);
			lblGroup.TabIndex = 11;
			lblGroup.Text = "Mikrotik List (Group):";
			lblGroup.TextAlign = ContentAlignment.MiddleRight;
			// 
			// lblAddress
			// 
			lblAddress.AutoSize = true;
			lblAddress.Dock = DockStyle.Right;
			lblAddress.Location = new Point(99, 106);
			lblAddress.Name = "lblAddress";
			lblAddress.Size = new Size(81, 41);
			lblAddress.TabIndex = 5;
			lblAddress.Text = "Address:";
			lblAddress.TextAlign = ContentAlignment.MiddleRight;
			// 
			// lblComment
			// 
			lblComment.AutoSize = true;
			lblComment.Dock = DockStyle.Right;
			lblComment.Location = new Point(85, 147);
			lblComment.Name = "lblComment";
			lblComment.Size = new Size(95, 39);
			lblComment.TabIndex = 6;
			lblComment.Text = "Comment:";
			lblComment.TextAlign = ContentAlignment.MiddleRight;
			// 
			// cboGroup
			// 
			tableLayoutPanel1.SetColumnSpan(cboGroup, 2);
			cboGroup.Dock = DockStyle.Fill;
			cboGroup.ItemHeight = 25;
			cboGroup.Location = new Point(186, 69);
			cboGroup.Margin = new Padding(3, 4, 3, 4);
			cboGroup.MaxDropDownItems = 50;
			cboGroup.Name = "cboGroup";
			cboGroup.Size = new Size(924, 33);
			cboGroup.TabIndex = 3;
			// 
			// cboAddress
			// 
			tableLayoutPanel1.SetColumnSpan(cboAddress, 2);
			cboAddress.Dock = DockStyle.Fill;
			cboAddress.Location = new Point(186, 110);
			cboAddress.Margin = new Padding(3, 4, 3, 4);
			cboAddress.MaxLength = 100;
			cboAddress.Name = "cboAddress";
			cboAddress.Size = new Size(924, 33);
			cboAddress.TabIndex = 0;
			// 
			// txtComment
			// 
			tableLayoutPanel1.SetColumnSpan(txtComment, 2);
			txtComment.Dock = DockStyle.Fill;
			txtComment.Location = new Point(186, 151);
			txtComment.Margin = new Padding(3, 4, 3, 4);
			txtComment.MaxLength = 100;
			txtComment.Name = "txtComment";
			txtComment.Size = new Size(924, 31);
			txtComment.TabIndex = 1;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.BackColor = SystemColors.Info;
			tableLayoutPanel1.SetColumnSpan(label1, 3);
			label1.Dock = DockStyle.Fill;
			label1.Location = new Point(3, 0);
			label1.Name = "label1";
			label1.Padding = new Padding(18, 20, 18, 20);
			label1.Size = new Size(1107, 65);
			label1.TabIndex = 12;
			label1.Text = "New element will be created in disabled state!";
			// 
			// grpSources
			// 
			tableLayoutPanel1.SetColumnSpan(grpSources, 3);
			grpSources.Controls.Add(lvwSources);
			grpSources.Dock = DockStyle.Fill;
			grpSources.Location = new Point(3, 189);
			grpSources.Name = "grpSources";
			grpSources.Padding = new Padding(8);
			grpSources.Size = new Size(1107, 495);
			grpSources.TabIndex = 13;
			grpSources.TabStop = false;
			grpSources.Text = "SRC";
			// 
			// lvwSources
			// 
			lvwSources.Columns.AddRange(new ColumnHeader[] { colAddress, colComment });
			lvwSources.Dock = DockStyle.Fill;
			lvwSources.DragDrop_InsertionLineColor = Color.Empty;
			lvwSources.FullRowSelect = true;
			lvwSources.GridLines = true;
			lvwSources.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			lvwSources.Location = new Point(8, 32);
			lvwSources.Margin = new Padding(3, 2, 3, 2);
			lvwSources.MultiSelect = false;
			lvwSources.Name = "lvwSources";
			lvwSources.Size = new Size(1091, 455);
			lvwSources.TabIndex = 1;
			lvwSources.UseCompatibleStateImageBehavior = false;
			lvwSources.View = View.Details;
			// 
			// colAddress
			// 
			colAddress.Text = "Address";
			// 
			// colComment
			// 
			colComment.Text = "Comment";
			// 
			// MikrotikAddressTableRecord_AddItemUI
			// 
			AcceptButton = btnAdd;
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(1149, 787);
			Controls.Add(tableLayoutPanel1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(3, 4, 3, 4);
			MinimizeBox = false;
			MinimumSize = new Size(640, 480);
			Name = "MikrotikAddressTableRecord_AddItemUI";
			Padding = new Padding(18, 20, 18, 20);
			ShowIcon = false;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "AddressList Editor";
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			grpSources.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion
		private TableLayoutPanel tableLayoutPanel1;
		internal Button btnCancel;
		internal Button btnAdd;
		internal Label lblAddress;
		internal Label lblComment;
		internal ComboBox cboAddress;
		internal TextBox txtComment;
		internal Label lblGroup;
		internal ComboBox cboGroup;
		internal Label label1;
		private GroupBox grpSources;
		private ListViewEx lvwSources;
		private ColumnHeader colAddress;
		private ColumnHeader colComment;
	}
}