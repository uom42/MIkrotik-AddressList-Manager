namespace MALM.UI
{
	partial class LocalSubnetBrowserUI
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(LocalSubnetBrowserUI));
			llFreeIcons = new ToolStripStatusLabel();
			btnViewARPList = new ToolStripButton();
			toolStripSeparator4 = new ToolStripSeparator();
			txtFilter = new ToolStripTextBox();
			toolStripSeparator3 = new ToolStripSeparator();
			btnRows_Add = new ToolStripButton();
			toolStripSeparator2 = new ToolStripSeparator();
			btnRows_Disable = new ToolStripButton();
			btnRows_Enable = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			btnRows_Refresh = new ToolStripButton();
			toolBarMain = new ToolStrip();
			colID = new ColumnHeader();
			colCreated = new ColumnHeader();
			colComment = new ColumnHeader();
			colAddress = new ColumnHeader();
			lvwRows = new ListViewEx();
			colMAC = new ColumnHeader();
			columnHeader2 = new ColumnHeader();
			statusStrip1 = new StatusStrip();
			toolBarMain.SuspendLayout();
			statusStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// llFreeIcons
			// 
			llFreeIcons.Name = "llFreeIcons";
			llFreeIcons.Size = new Size(179, 25);
			llFreeIcons.Text = "toolStripStatusLabel1";
			// 
			// btnViewARPList
			// 
			btnViewARPList.Image = (Image)resources.GetObject("btnViewARPList.Image");
			btnViewARPList.ImageTransparentColor = Color.Magenta;
			btnViewARPList.Name = "btnViewARPList";
			btnViewARPList.Size = new Size(104, 29);
			btnViewARPList.Text = "ARP List";
			// 
			// toolStripSeparator4
			// 
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new Size(6, 34);
			// 
			// txtFilter
			// 
			txtFilter.Alignment = ToolStripItemAlignment.Right;
			txtFilter.Name = "txtFilter";
			txtFilter.Size = new Size(222, 34);
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new Size(6, 34);
			// 
			// btnRows_Add
			// 
			btnRows_Add.Image = Properties.Resources.Plus;
			btnRows_Add.ImageTransparentColor = Color.Magenta;
			btnRows_Add.Name = "btnRows_Add";
			btnRows_Add.Size = new Size(74, 29);
			btnRows_Add.Text = "Add";
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(6, 34);
			// 
			// btnRows_Disable
			// 
			btnRows_Disable.Image = Properties.Resources.Stop;
			btnRows_Disable.ImageTransparentColor = Color.Magenta;
			btnRows_Disable.Name = "btnRows_Disable";
			btnRows_Disable.Size = new Size(98, 29);
			btnRows_Disable.Text = "Disable";
			// 
			// btnRows_Enable
			// 
			btnRows_Enable.Image = Properties.Resources.Check;
			btnRows_Enable.ImageTransparentColor = Color.Magenta;
			btnRows_Enable.Name = "btnRows_Enable";
			btnRows_Enable.Size = new Size(92, 29);
			btnRows_Enable.Text = "Enable";
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(6, 34);
			// 
			// btnRows_Refresh
			// 
			btnRows_Refresh.Image = Properties.Resources.Refresh;
			btnRows_Refresh.ImageTransparentColor = Color.Magenta;
			btnRows_Refresh.Name = "btnRows_Refresh";
			btnRows_Refresh.Size = new Size(98, 29);
			btnRows_Refresh.Text = "Refresh";
			// 
			// toolBarMain
			// 
			toolBarMain.ImageScalingSize = new Size(24, 24);
			toolBarMain.Items.AddRange(new ToolStripItem[] { btnRows_Refresh, toolStripSeparator1, btnRows_Enable, btnRows_Disable, toolStripSeparator2, btnRows_Add, toolStripSeparator3, txtFilter, toolStripSeparator4, btnViewARPList });
			toolBarMain.Location = new Point(0, 0);
			toolBarMain.Name = "toolBarMain";
			toolBarMain.Size = new Size(1589, 34);
			toolBarMain.TabIndex = 4;
			toolBarMain.Text = "toolStrip1";
			// 
			// colID
			// 
			colID.Text = "ID";
			colID.Width = 100;
			// 
			// colCreated
			// 
			colCreated.Text = "Created";
			colCreated.Width = 100;
			// 
			// colComment
			// 
			colComment.Text = "Comment";
			colComment.Width = 100;
			// 
			// colAddress
			// 
			colAddress.Text = "Address";
			colAddress.Width = 100;
			// 
			// lvwRows
			// 
			lvwRows.Columns.AddRange(new ColumnHeader[] { colAddress, colMAC, colComment, colCreated, colID, columnHeader2 });
			lvwRows.Dock = DockStyle.Fill;
			lvwRows.DragDrop_InsertionLineColor = Color.Empty;
			lvwRows.FullRowSelect = true;
			lvwRows.GridLines = true;
			lvwRows.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			lvwRows.Location = new Point(0, 34);
			lvwRows.Margin = new Padding(3, 2, 3, 2);
			lvwRows.Name = "lvwRows";
			lvwRows.Size = new Size(1589, 918);
			lvwRows.TabIndex = 3;
			lvwRows.UseCompatibleStateImageBehavior = false;
			lvwRows.View = View.Details;
			// 
			// colMAC
			// 
			colMAC.Text = "MAC";
			colMAC.Width = 100;
			// 
			// columnHeader2
			// 
			columnHeader2.Width = 100;
			// 
			// statusStrip1
			// 
			statusStrip1.ImageScalingSize = new Size(24, 24);
			statusStrip1.Items.AddRange(new ToolStripItem[] { llFreeIcons });
			statusStrip1.Location = new Point(0, 952);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Padding = new Padding(1, 0, 16, 0);
			statusStrip1.Size = new Size(1589, 32);
			statusStrip1.TabIndex = 5;
			statusStrip1.Text = "statusStrip1";
			// 
			// ARPLIstUI
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1589, 984);
			Controls.Add(lvwRows);
			Controls.Add(toolBarMain);
			Controls.Add(statusStrip1);
			Name = "ARPLIstUI";
			StartPosition = FormStartPosition.CenterParent;
			Text = "ARPLIstUI";
			toolBarMain.ResumeLayout(false);
			toolBarMain.PerformLayout();
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private ToolStripStatusLabel llFreeIcons;
		private ToolStripButton btnViewARPList;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripTextBox txtFilter;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripButton btnRows_Add;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton btnRows_Disable;
		private ToolStripButton btnRows_Enable;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton btnRows_Refresh;
		private ToolStrip toolBarMain;
		private ColumnHeader colID;
		private ColumnHeader colCreated;
		private ColumnHeader colComment;
		private ColumnHeader colAddress;
		private ListViewEx lvwRows;
		private StatusStrip statusStrip1;
		private ColumnHeader colMAC;
		private ColumnHeader columnHeader2;
	}
}