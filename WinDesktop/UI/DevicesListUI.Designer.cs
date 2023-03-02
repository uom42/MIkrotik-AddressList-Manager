

using System.Windows.Forms;

namespace MALM.UI
{
	partial class DevicesListUI
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(DevicesListUI));
			lvwDevices = new ListViewEx();
			colAddress = new ColumnHeader();
			colUser = new ColumnHeader();
			tableLayoutPanel1 = new TableLayoutPanel();
			btnConnect = new Button();
			btnAdd = new Button();
			btnDelete = new Button();
			btnEdit = new Button();
			btnSetMasterKey = new Button();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// lvwDevices
			// 
			lvwDevices.Columns.AddRange(new ColumnHeader[] { colAddress, colUser });
			lvwDevices.Dock = DockStyle.Fill;
			lvwDevices.DragDrop_InsertionLineColor = Color.Empty;
			lvwDevices.FullRowSelect = true;
			lvwDevices.GridLines = true;
			lvwDevices.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			lvwDevices.Location = new Point(3, 2);
			lvwDevices.Margin = new Padding(3, 2, 3, 2);
			lvwDevices.Name = "lvwDevices";
			tableLayoutPanel1.SetRowSpan(lvwDevices, 7);
			lvwDevices.Size = new Size(358, 486);
			lvwDevices.TabIndex = 0;
			lvwDevices.UseCompatibleStateImageBehavior = false;
			lvwDevices.View = View.Details;
			// 
			// colAddress
			// 
			colAddress.Text = "Address";
			// 
			// colUser
			// 
			colUser.Text = "User";
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 9F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 278F));
			tableLayoutPanel1.Controls.Add(lvwDevices, 0, 0);
			tableLayoutPanel1.Controls.Add(btnConnect, 2, 6);
			tableLayoutPanel1.Controls.Add(btnAdd, 2, 0);
			tableLayoutPanel1.Controls.Add(btnDelete, 2, 2);
			tableLayoutPanel1.Controls.Add(btnEdit, 2, 1);
			tableLayoutPanel1.Controls.Add(btnSetMasterKey, 2, 4);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(18, 20);
			tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 7;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
			tableLayoutPanel1.Size = new Size(651, 490);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// btnConnect
			// 
			btnConnect.Dock = DockStyle.Fill;
			btnConnect.Location = new Point(376, 434);
			btnConnect.Margin = new Padding(3, 4, 3, 4);
			btnConnect.Name = "btnConnect";
			btnConnect.Size = new Size(272, 52);
			btnConnect.TabIndex = 1;
			btnConnect.Text = "Connect";
			btnConnect.UseVisualStyleBackColor = true;
			// 
			// btnAdd
			// 
			btnAdd.Dock = DockStyle.Fill;
			btnAdd.Location = new Point(376, 4);
			btnAdd.Margin = new Padding(3, 4, 3, 4);
			btnAdd.Name = "btnAdd";
			btnAdd.Size = new Size(272, 52);
			btnAdd.TabIndex = 2;
			btnAdd.Text = "Add";
			btnAdd.UseVisualStyleBackColor = true;
			// 
			// btnDelete
			// 
			btnDelete.Dock = DockStyle.Fill;
			btnDelete.Location = new Point(376, 124);
			btnDelete.Margin = new Padding(3, 4, 3, 4);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new Size(272, 52);
			btnDelete.TabIndex = 4;
			btnDelete.Text = "Delete";
			btnDelete.UseVisualStyleBackColor = true;
			// 
			// btnEdit
			// 
			btnEdit.Dock = DockStyle.Fill;
			btnEdit.Location = new Point(376, 64);
			btnEdit.Margin = new Padding(3, 4, 3, 4);
			btnEdit.Name = "btnEdit";
			btnEdit.Size = new Size(272, 52);
			btnEdit.TabIndex = 3;
			btnEdit.Text = "Edit";
			btnEdit.UseVisualStyleBackColor = true;
			// 
			// btnSetMasterKey
			// 
			btnSetMasterKey.Dock = DockStyle.Fill;
			btnSetMasterKey.Location = new Point(376, 244);
			btnSetMasterKey.Margin = new Padding(3, 4, 3, 4);
			btnSetMasterKey.Name = "btnSetMasterKey";
			btnSetMasterKey.Size = new Size(272, 52);
			btnSetMasterKey.TabIndex = 5;
			btnSetMasterKey.Text = "Set Master Key";
			btnSetMasterKey.UseVisualStyleBackColor = true;
			// 
			// DevicesListUI
			// 
			AcceptButton = btnConnect;
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(687, 530);
			Controls.Add(tableLayoutPanel1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(3, 4, 3, 4);
			MinimumSize = new Size(709, 586);
			Name = "DevicesListUI";
			Padding = new Padding(18, 20, 18, 20);
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Addressbook";
			tableLayoutPanel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion
		internal ColumnHeader colAddress;
		internal ColumnHeader colUser;
		internal uom.controls.ListViewEx lvwDevices;
		private TableLayoutPanel tableLayoutPanel1;
		internal Button btnAdd;
		internal Button btnDelete;
		internal Button btnConnect;
		internal Button btnEdit;
		internal Button btnSetMasterKey;
	}
}