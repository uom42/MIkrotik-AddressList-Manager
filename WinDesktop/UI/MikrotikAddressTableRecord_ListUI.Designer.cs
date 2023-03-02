using uom;

namespace MALM.UI
{
	partial class MikrotikAddressTableRecord_ListUI
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(MikrotikAddressTableRecord_ListUI));
			lvwRows = new ListViewEx();
			colAddress = new ColumnHeader();
			colComment = new ColumnHeader();
			colCreated = new ColumnHeader();
			colID = new ColumnHeader();
			toolBarMain = new ToolStrip();
			btnRows_Refresh = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			btnRows_Enable = new ToolStripButton();
			toolStripSeparator2 = new ToolStripSeparator();
			btnRows_Add = new ToolStripButton();
			toolStripSeparator3 = new ToolStripSeparator();
			txtFilter = new ToolStripTextBox();
			statusStrip1 = new StatusStrip();
			llFreeIcons = new ToolStripStatusLabel();
			btnViewARPList = new ToolStripButton();
			toolStripSeparator4 = new ToolStripSeparator();
			toolBarMain.SuspendLayout();
			statusStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// lvwRows
			// 
			lvwRows.Columns.AddRange(new ColumnHeader[] { colAddress, colComment, colCreated, colID });
			lvwRows.Dock = DockStyle.Fill;
			lvwRows.DragDrop_InsertionLineColor = Color.Empty;
			lvwRows.FullRowSelect = true;
			lvwRows.GridLines = true;
			lvwRows.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			lvwRows.Location = new Point(0, 34);
			lvwRows.Margin = new Padding(3, 2, 3, 2);
			lvwRows.Name = "lvwRows";
			lvwRows.Size = new Size(1286, 820);
			lvwRows.TabIndex = 0;
			lvwRows.UseCompatibleStateImageBehavior = false;
			lvwRows.View = View.Details;
			// 
			// colAddress
			// 
			colAddress.Text = "Address";
			// 
			// colComment
			// 
			colComment.Text = "Comment";
			// 
			// colCreated
			// 
			colCreated.Text = "Created";
			// 
			// colID
			// 
			colID.Text = "ID";
			// 
			// toolBarMain
			// 
			toolBarMain.ImageScalingSize = new Size(24, 24);
			toolBarMain.Items.AddRange(new ToolStripItem[] { btnRows_Refresh, toolStripSeparator1, btnRows_Enable, toolStripSeparator2, btnRows_Add, toolStripSeparator3, txtFilter, toolStripSeparator4, btnViewARPList });
			toolBarMain.Location = new Point(0, 0);
			toolBarMain.Name = "toolBarMain";
			toolBarMain.Size = new Size(1286, 34);
			toolBarMain.TabIndex = 1;
			toolBarMain.Text = "toolStrip1";
			// 
			// btnRows_Refresh
			// 
			btnRows_Refresh.Image = Properties.Resources.Refresh;
			btnRows_Refresh.ImageTransparentColor = Color.Magenta;
			btnRows_Refresh.Name = "btnRows_Refresh";
			btnRows_Refresh.Size = new Size(98, 29);
			btnRows_Refresh.Text = "Refresh";
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(6, 34);
			// 
			// btnRows_Enable
			// 
			btnRows_Enable.Image = Properties.Resources.Check;
			btnRows_Enable.CheckOnClick = true;
			btnRows_Enable.ImageTransparentColor = Color.Magenta;
			btnRows_Enable.Name = "btnRows_Enable";
			btnRows_Enable.Size = new Size(92, 29);
			btnRows_Enable.Text = "Enable";
			// 
			// btnRows_Disable
			// 
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(6, 34);
			// 
			// btnRows_Add
			// 
			btnRows_Add.Image = Properties.Resources.Plus;
			btnRows_Add.ImageTransparentColor = Color.Magenta;
			btnRows_Add.Name = "btnRows_Add";
			btnRows_Add.Size = new Size(74, 29);
			btnRows_Add.Text = "Add";
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new Size(6, 34);
			// 
			// txtFilter
			// 
			txtFilter.Alignment = ToolStripItemAlignment.Right;
			txtFilter.Name = "txtFilter";
			txtFilter.Size = new Size(222, 34);
			// 
			// statusStrip1
			// 
			statusStrip1.ImageScalingSize = new Size(24, 24);
			statusStrip1.Items.AddRange(new ToolStripItem[] { llFreeIcons });
			statusStrip1.Location = new Point(0, 854);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Padding = new Padding(1, 0, 16, 0);
			statusStrip1.Size = new Size(1286, 32);
			statusStrip1.TabIndex = 2;
			statusStrip1.Text = "statusStrip1";
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
			// MikrotikAddressTableRecord_ListUI
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1286, 886);
			Controls.Add(lvwRows);
			Controls.Add(toolBarMain);
			Controls.Add(statusStrip1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Margin = new Padding(3, 2, 3, 2);
			Name = "MikrotikAddressTableRecord_ListUI";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Main";
			toolBarMain.ResumeLayout(false);
			toolBarMain.PerformLayout();
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private uom.controls.ListViewEx lvwRows;
		private ColumnHeader colAddress;
		private ColumnHeader colComment;
		private ColumnHeader colCreated;
		private ColumnHeader colID;
		private ToolStrip toolBarMain;
		private ToolStripButton btnRows_Enable;
		private ToolStripButton btnRows_Refresh;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton btnRows_Add;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripTextBox txtFilter;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel llFreeIcons;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripButton btnViewARPList;
	}
}