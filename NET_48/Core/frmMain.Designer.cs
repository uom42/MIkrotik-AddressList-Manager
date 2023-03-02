using uom;

namespace malm.Core
{
	partial class frmMain
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.lvwRows = new common.Controls.ListViewEx();
			this.colAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.toolBarMain = new System.Windows.Forms.ToolStrip();
			this.btnRows_Refresh = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnRows_Enable = new System.Windows.Forms.ToolStripButton();
			this.btnRows_Disable = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnRows_Add = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.txtFilter = new System.Windows.Forms.ToolStripTextBox();
			this.lblFilter = new System.Windows.Forms.ToolStripLabel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.llFreeIcons = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolBarMain.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lvwRows
			// 
			this.lvwRows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAddress,
            this.colComment,
            this.colCreated,
            this.colID});
			this.lvwRows.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwRows.DragDrop_InsertionLineColor = System.Drawing.Color.Empty;
			this.lvwRows.FullRowSelect = true;
			this.lvwRows.GridLines = true;
			this.lvwRows.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvwRows.HideSelection = false;
			this.lvwRows.Location = new System.Drawing.Point(0, 34);
			this.lvwRows.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.lvwRows.Name = "lvwRows";
			this.lvwRows.Size = new System.Drawing.Size(1157, 643);
			this.lvwRows.TabIndex = 0;
			this.lvwRows.UseCompatibleStateImageBehavior = false;
			this.lvwRows.View = System.Windows.Forms.View.Details;
			this.lvwRows.GroupsCollapsedStateChangedByMouse += new System.EventHandler<string>(this.lvwRows_GroupsCollapsedStateChangedByMouse);
			this.lvwRows.Items_NeedRefreshList += new System.EventHandler(this.lwvRows_Items_NeedRefreshList);
			this.lvwRows.SelectedIndexChanged += new System.EventHandler(this.lwvRows_SelectedIndexChanged);
			// 
			// colAddress
			// 
			this.colAddress.Text = "Address";
			// 
			// colComment
			// 
			this.colComment.Text = "Comment";
			// 
			// colCreated
			// 
			this.colCreated.Text = "Created";
			// 
			// colID
			// 
			this.colID.Text = "ID";
			// 
			// toolBarMain
			// 
			this.toolBarMain.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolBarMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRows_Refresh,
            this.toolStripSeparator1,
            this.btnRows_Enable,
            this.btnRows_Disable,
            this.toolStripSeparator2,
            this.btnRows_Add,
            this.toolStripSeparator3,
            this.txtFilter,
            this.lblFilter});
			this.toolBarMain.Location = new System.Drawing.Point(0, 0);
			this.toolBarMain.Name = "toolBarMain";
			this.toolBarMain.Size = new System.Drawing.Size(1157, 34);
			this.toolBarMain.TabIndex = 1;
			this.toolBarMain.Text = "toolStrip1";
			// 
			// btnRows_Refresh
			// 
			this.btnRows_Refresh.Image = global::malm.Properties.Resources.Refresh;
			this.btnRows_Refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRows_Refresh.Name = "btnRows_Refresh";
			this.btnRows_Refresh.Size = new System.Drawing.Size(98, 29);
			this.btnRows_Refresh.Text = "Refresh";
			this.btnRows_Refresh.Click += new System.EventHandler(this.btnRows_Refresh_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
			// 
			// btnRows_Enable
			// 
			this.btnRows_Enable.Image = global::malm.Properties.Resources.Check;
			this.btnRows_Enable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRows_Enable.Name = "btnRows_Enable";
			this.btnRows_Enable.Size = new System.Drawing.Size(92, 29);
			this.btnRows_Enable.Text = "Enable";
			this.btnRows_Enable.Click += new System.EventHandler(this.btnRows_Enable_Click);
			// 
			// btnRows_Disable
			// 
			this.btnRows_Disable.Image = global::malm.Properties.Resources.Stop;
			this.btnRows_Disable.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRows_Disable.Name = "btnRows_Disable";
			this.btnRows_Disable.Size = new System.Drawing.Size(98, 29);
			this.btnRows_Disable.Text = "Disable";
			this.btnRows_Disable.Click += new System.EventHandler(this.btnRows_Disable_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 34);
			// 
			// btnRows_Add
			// 
			this.btnRows_Add.Image = global::malm.Properties.Resources.Plus;
			this.btnRows_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRows_Add.Name = "btnRows_Add";
			this.btnRows_Add.Size = new System.Drawing.Size(74, 29);
			this.btnRows_Add.Text = "Add";
			this.btnRows_Add.Click += new System.EventHandler(this.btnRows_Add_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 34);
			// 
			// txtFilter
			// 
			this.txtFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.txtFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(200, 34);
			// 
			// lblFilter
			// 
			this.lblFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.lblFilter.Name = "lblFilter";
			this.lblFilter.Size = new System.Drawing.Size(54, 29);
			this.lblFilter.Text = "Filter:";
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.llFreeIcons});
			this.statusStrip1.Location = new System.Drawing.Point(0, 677);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1157, 32);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// llFreeIcons
			// 
			this.llFreeIcons.Name = "llFreeIcons";
			this.llFreeIcons.Size = new System.Drawing.Size(179, 25);
			this.llFreeIcons.Text = "toolStripStatusLabel1";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1157, 709);
			this.Controls.Add(this.lvwRows);
			this.Controls.Add(this.toolBarMain);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Main";
			this.toolBarMain.ResumeLayout(false);
			this.toolBarMain.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private common.Controls.ListViewEx lvwRows;
		private ColumnHeader colAddress;
		private ColumnHeader colComment;
		private ColumnHeader colCreated;
		private ColumnHeader colID;
		private ToolStrip toolBarMain;
		private ToolStripButton btnRows_Enable;
		private ToolStripButton btnRows_Disable;
		private ToolStripButton btnRows_Refresh;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton btnRows_Add;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripTextBox txtFilter;
		private ToolStripLabel lblFilter;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel llFreeIcons;
	}
}