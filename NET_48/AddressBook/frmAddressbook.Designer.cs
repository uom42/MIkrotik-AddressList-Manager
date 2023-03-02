namespace malm.AddressBook
{
	partial class frmAddressbook
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddressbook));
			this.lvwAddressbook = new common.Controls.ListViewEx();
			this.colAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnConnect = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnSetMasterKey = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lvwAddressbook
			// 
			this.lvwAddressbook.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAddress,
            this.colUser});
			this.lvwAddressbook.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwAddressbook.DragDrop_InsertionLineColor = System.Drawing.Color.Empty;
			this.lvwAddressbook.FullRowSelect = true;
			this.lvwAddressbook.GridLines = true;
			this.lvwAddressbook.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvwAddressbook.HideSelection = false;
			this.lvwAddressbook.Location = new System.Drawing.Point(3, 2);
			this.lvwAddressbook.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.lvwAddressbook.Name = "lvwAddressbook";
			this.tableLayoutPanel1.SetRowSpan(this.lvwAddressbook, 7);
			this.lvwAddressbook.Size = new System.Drawing.Size(372, 388);
			this.lvwAddressbook.TabIndex = 0;
			this.lvwAddressbook.UseCompatibleStateImageBehavior = false;
			this.lvwAddressbook.View = System.Windows.Forms.View.Details;
			// 
			// colAddress
			// 
			this.colAddress.Text = "Address";
			// 
			// colUser
			// 
			this.colUser.Text = "User";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.tableLayoutPanel1.Controls.Add(this.lvwAddressbook, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnConnect, 2, 6);
			this.tableLayoutPanel1.Controls.Add(this.btnAdd, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnDelete, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnEdit, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnSetMasterKey, 2, 4);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(586, 392);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnConnect
			// 
			this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnConnect.Location = new System.Drawing.Point(389, 347);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(194, 42);
			this.btnConnect.TabIndex = 1;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			// 
			// btnAdd
			// 
			this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnAdd.Location = new System.Drawing.Point(389, 3);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(194, 42);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			// 
			// btnDelete
			// 
			this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnDelete.Location = new System.Drawing.Point(389, 99);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(194, 42);
			this.btnDelete.TabIndex = 4;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			// 
			// btnEdit
			// 
			this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnEdit.Location = new System.Drawing.Point(389, 51);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(194, 42);
			this.btnEdit.TabIndex = 3;
			this.btnEdit.Text = "Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			// 
			// btnSetMasterKey
			// 
			this.btnSetMasterKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSetMasterKey.Location = new System.Drawing.Point(389, 195);
			this.btnSetMasterKey.Name = "btnSetMasterKey";
			this.btnSetMasterKey.Size = new System.Drawing.Size(194, 42);
			this.btnSetMasterKey.TabIndex = 5;
			this.btnSetMasterKey.Text = "Set Master Key";
			this.btnSetMasterKey.UseVisualStyleBackColor = true;
			// 
			// frmAddressbook
			// 
			this.AcceptButton = this.btnConnect;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(618, 424);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(640, 480);
			this.Name = "frmAddressbook";
			this.Padding = new System.Windows.Forms.Padding(16);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Addressbook";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private ColumnHeader colAddress;
		private ColumnHeader colUser;
		internal common.Controls.ListViewEx lvwAddressbook;
		private TableLayoutPanel tableLayoutPanel1;
		internal Button btnAdd;
		internal Button btnDelete;
		internal Button btnConnect;
		internal Button btnEdit;
		internal Button btnSetMasterKey;
	}
}