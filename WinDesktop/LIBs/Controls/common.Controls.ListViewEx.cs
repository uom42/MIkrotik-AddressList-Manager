#nullable enable

//global using Extensions.Extensions_Controls_Listview;


global using System.Windows.Forms;

global using System.Drawing;

global using uom.controls;

using uom.WinAPI;

using static uom.controls.ListViewEx;
using System.Diagnostics.Eventing.Reader;


namespace uom.controls
{

	public partial class ListViewEx : ListView
	{
		#region WinAPI for ListView


		private const int L_MAX_URL_LENGTH = 2048 + 32 + 4;// '//#define L_MAX_URL_LENGTH    (2048 + 32 + sizeof("://"))
														   //private const int L_MAX_URL_LENGTH = (2048 + 32 + sizeof("://"));

		//public const Int32 L_MAX_URL_LENGTH = 2084;

		[StructLayout(LayoutKind.Sequential)]
		public struct NMHDR
		{
			public IntPtr hwndFrom;
			public IntPtr idFrom;
			public UInt32 code;
		}


		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct NMLVEMPTYMARKUP
		{
			/// <summary>NMHDR  structure that contains basic information about the notification code.</summary>
			public NMHDR hdr;
			public int dwFlags;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = L_MAX_URL_LENGTH)] public string szMarkup;
		}


		/// <summary>ListView messages</summary>
		public enum ListViewMessages : int
		{
			LVM_FIRST = 0x1000,

			LVM_SETGROUPINFO = (LVM_FIRST + 147),
			LVM_GETGROUPINFO = LVM_FIRST + 149,
			LVM_GETGROUPSTATE = (LVM_FIRST + 92),
			LVM_RESETEMPTYTEXT = (LVM_FIRST + 84),
			LVM_SETIMAGELIST = (LVM_FIRST + 3),
			LVM_SCROLL = (LVM_FIRST + 20),
			LVM_GETHEADER = (LVM_FIRST + 31),
			LVM_GETCOUNTPERPAGE = (LVM_FIRST + 40),
			LVM_SETITEMSTATE = (LVM_FIRST + 43),
			LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54),
			LVM_GETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 55),
			LVM_SETITEM = LVM_FIRST + 76,
			LVM_GETTOOLTIPS = 0x1000 + 78,
			LVM_SETTOOLTIPS = 0x1000 + 74,
			LVM_GETCOLUMN = LVM_FIRST + 95,
			LVM_SETCOLUMN = LVM_FIRST + 96,
			LVM_SETSELECTEDCOLUMN = LVM_FIRST + 140,
			LVM_INSERTGROUP = LVM_FIRST + 145,
			LVM_SETGROUPMETRICS = LVM_FIRST + 155,
			LVM_REMOVEALLGROUPS = LVM_FIRST + 160,


			LVM_GETITEMCOUNT = LVM_FIRST + 4,
			LVM_GETNEXTITEM = LVM_FIRST + 12,
			LVM_GETITEMRECT = LVM_FIRST + 14,
			LVM_GETITEMPOSITION = LVM_FIRST + 16,
			LVM_HITTEST = (LVM_FIRST + 18),
			LVM_ENSUREVISIBLE = LVM_FIRST + 19,
			LVM_GETITEMSTATE = LVM_FIRST + 44,
			LVM_GETSUBITEMRECT = LVM_FIRST + 56,
			LVM_SUBITEMHITTEST = LVM_FIRST + 57,
			LVM_APPROXIMATEVIEWRECT = LVM_FIRST + 64,
			LVM_GETITEMW = LVM_FIRST + 75,
			LVM_GETFOCUSEDGROUP = LVM_FIRST + 93,
			LVM_GETGROUPRECT = LVM_FIRST + 98,
			LVM_EDITLABEL = LVM_FIRST + 118,
			LVM_GETVIEW = LVM_FIRST + 143,
			LVM_SETVIEW = LVM_FIRST + 142,
			LVM_GETGROUPINFOBYINDEX = LVM_FIRST + 153,
			LVM_GETGROUPMETRICS = LVM_FIRST + 156,
			LVM_HASGROUP = LVM_FIRST + 161,
			LVM_ISGROUPVIEWENABLED = LVM_FIRST + 175,
			LVM_GETFOCUSEDCOLUMN = LVM_FIRST + 186,
			LVM_GETEMPTYTEXT = LVM_FIRST + 204,
			LVM_GETFOOTERRECT = LVM_FIRST + 205,
			LVM_GETFOOTERINFO = LVM_FIRST + 206,
			LVM_GETFOOTERITEMRECT = LVM_FIRST + 207,
			LVM_GETFOOTERITEM = LVM_FIRST + 208,
			LVM_GETITEMINDEXRECT = LVM_FIRST + 209,
			LVM_SETITEMINDEXSTATE = LVM_FIRST + 210,
			LVM_GETNEXTITEMINDEX = LVM_FIRST + 211
		}

		/// <summary>ListView messages</summary>
		public enum ListViewNotifyMessages : int
		{
			//LVN_FIRST = unchecked(0u - 100u),
			LVN_FIRST = -100,
			LVN_LINKCLICK = (LVN_FIRST - 84),
			LVN_GETEMPTYMARKUP = LVN_FIRST - 87,
		}

		public enum ExStyles : int
		{

			/// <summary>
			/// Displays gridlines around items and subitems. This style is available only in conjunction with the LVS_REPORT style.
			/// </summary>
			LVS_EX_GRIDLINES = 0x00000001,

			/// <summary>
			///Allows images to be displayed for subitems. This style is available only in conjunction with the LVS_REPORT style.
			/// </summary>
			LVS_EX_SUBITEMIMAGES = 0x00000002,

			/// <summary>
			///Version 4.70. Enables check boxes for items in a list-view control. When set to this style, the control creates and sets a state image list with two images using DrawFrameControl. State image 1 is the unchecked box, and state image 2 is the checked box.Setting the state image to zero removes the check box.
			///Version 6.00 and later Check boxes are visible and functional with all list view modes except the tile view mode introduced in ComCtl32.dll version 6. Clicking a checkbox in tile view mode only selects the item; the state does not change.
			///You can obtain the state of the check box for a given item with ListView_GetCheckState.To set the check state, use ListView_SetCheckState. If this style is set, the list-view control automatically toggles the check state when the user clicks the check box or presses the space bar.
			/// </summary>
			LVS_EX_CHECKBOXES = 0x00000004,

			/// <summary>
			///Enables hot-track selection in a list-view control. Hot track selection means that an item is automatically selected when the cursor remains over the item for a certain period of time. The delay can be changed from the default system setting with a LVM_SETHOVERTIME message. This style applies to all styles of list-view control. You can check whether hot-track selection is enabled by calling SystemParametersInfo.
			/// </summary>
			LVS_EX_TRACKSELECT = 0x00000008,

			/// <summary>
			///Enables drag-and-drop reordering of columns in a list-view control. This style is only available to list-view controls that use the LVS_REPORT style.
			/// </summary>
			LVS_EX_HEADERDRAGDROP = 0x00000010,

			/// <summary>
			///When an item is selected, the item and all its subitems are highlighted. This style is available only in conjunction with the LVS_REPORT style.
			/// </summary>
			LVS_EX_FULLROWSELECT = 0x00000020, // applies to report mode only

			/// <summary>
			///The list-view control sends an LVN_ITEMACTIVATE notification code to the parent window when the user clicks an item. This style also enables hot tracking in the list-view control. Hot tracking means that when the cursor moves over an item, it is highlighted but not selected. See the Extended List-View Styles Remarks section for a discussion of item activation.
			/// </summary>
			LVS_EX_ONECLICKACTIVATE = 0x00000040



			/*

			 LVS_EX_AUTOAUTOARRANGE
			Windows Vista and later. Automatically arrange icons if no icon positions have been set (Similar to LVS_AUTOARRANGE).
			LVS_EX_AUTOCHECKSELECT
			Windows Vista and later. Automatically select check boxes on single click.
			LVS_EX_AUTOSIZECOLUMNS
			Windows Vista and later. Automatically size listview columns.
			LVS_EX_BORDERSELECT
			Version 4.71 and later. Changes border color when an item is selected, instead of highlighting the item.
			
			
				LVS_EX_COLUMNOVERFLOW
			Indicates that an overflow button should be displayed in icon/tile view if there is not enough client width to display the complete set of header items. The list-view control sends the LVN_COLUMNOVERFLOWCLICK notification when the overflow button is clicked. This flag is only valid when LVS_EX_HEADERINALLVIEWS is also specified.
			LVS_EX_COLUMNSNAPPOINTS
			Windows Vista and later. Snap to minimum column width when the user resizes a column.
			LVS_EX_DOUBLEBUFFER
			Version 6.00 and later. Paints via double-buffering, which reduces flicker. This extended style also enables alpha-blended marquee selection on systems where it is supported.
			LVS_EX_FLATSB
			Enables flat scroll bars in the list view. If you need more control over the appearance of the list view's scroll bars, you should manipulate the list view's scroll bars directly using the Flat Scroll Bar APIs. If the system metrics change, you are responsible for adjusting the scroll bar metrics with FlatSB_SetScrollProp. See Flat Scroll Bars for further details.
			
			LVS_EX_HEADERINALLVIEWS
			Windows Vista and later. Show column headers in all view modes.
			LVS_EX_HIDELABELS
			Version 6.00 and later. Hides the labels in icon and small icon view.
			LVS_EX_INFOTIP
			When a list-view control uses the LVS_EX_INFOTIP style, the LVN_GETINFOTIP notification code is sent to the parent window before displaying an item's tooltip.
			LVS_EX_JUSTIFYCOLUMNS
			Windows Vista and later. Icons are lined up in columns that use up the whole view.
			LVS_EX_LABELTIP
			If a partially hidden label in any list view mode lacks tooltip text, the list-view control will unfold the label. If this style is not set, the list-view control will unfold partly hidden labels only for the large icon mode.
			LVS_EX_MULTIWORKAREAS
			If the list-view control has the LVS_AUTOARRANGE style, the control will not autoarrange its icons until one or more work areas are defined (see LVM_SETWORKAREAS). To be effective, this style must be set before any work areas are defined and any items have been added to the control.
			LVS_EX_REGIONAL
			Version 4.71 through Version 5.80 only. Not supported on Windows Vista and later. Sets the list view window region to include only the item icons and text using SetWindowRgn. Any area that is not part of an item is excluded from the window region. This style is only available to list-view controls that use the LVS_ICON style.
			LVS_EX_SIMPLESELECT
			Version 6.00 and later. In icon view, moves the state image of the control to the top right of the large icon rendering. In views other than icon view there is no change. When the user changes the state by using the space bar, all selected items cycle over, not the item with the focus.
			LVS_EX_SINGLEROW
			Version 6.00 and later. Not used.
			LVS_EX_SNAPTOGRID
			Version 6.00 and later. In icon view, icons automatically snap into a grid.
			
				
			LVS_EX_TRANSPARENTBKGND
			Windows Vista and later. Background is painted by the parent via WM_PRINTCLIENT.
			LVS_EX_TRANSPARENTSHADOWTEXT
			Windows Vista and later. Enable shadow text on transparent backgrounds only.
			LVS_EX_TWOCLICKACTIVATE
			The list-view control sends an LVN_ITEMACTIVATE notification code to the parent window when the user double-clicks an item. This style also enables hot tracking in the list-view control. Hot tracking means that when the cursor moves over an item, it is highlighted but not selected. See the Extended List-View Styles Remarks section for a discussion of item activation.
			LVS_EX_UNDERLINECOLD
			Causes those non-hot items that may be activated to be displayed with underlined text. This style requires that LVS_EX_TWOCLICKACTIVATE be set also. See the Extended List-View Styles Remarks section for a discussion of item activation.
			LVS_EX_UNDERLINEHOT
			Causes those hot items that may be activated to be displayed with underlined text. This style requires that LVS_EX_ONECLICKACTIVATE or LVS_EX_TWOCLICKACTIVATE also be set. See the Extended List-View Styles Remarks section for a discussion of item activation.

			 */

		}



		#endregion


		#region Constructors


		/// <summary>Initializes a new instance of the <see cref="ListView"/> class.</summary>
		public ListViewEx() : base()
		{

			SetStyle(
				ControlStyles.ResizeRedraw
				| ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.EnableNotifyMessage
				, true);

			AddKeyboardAndMousehandlers();

			this._dragDrop_InsertionIndex = -1;
		}


		#endregion


		public void SelectAllItems()
		{
			try
			{
				BeginUpdate();
				{
					foreach (ListViewItem li in Items) li.Selected = true;
				}
			}
			finally { EndUpdate(); }
		}


		public class QueryEmptyTextEventArgs : System.EventArgs
		{
			public string EmptyListViewText = string.Empty;
			public bool CenterText = false;
		}


		public event EventHandler<QueryEmptyTextEventArgs> QueryEmptyText = delegate { };

#if !NET
		public event EventHandler<string> GroupsCollapsedStateChangedByMouse = delegate { };
#endif


		#region item editing and Clipboard copy/pasting


		#region Events for item editing and Clipboard copy/pasting


		///<summary>When doubleclick on item 
		///or on  return  key with some selected items
		///if CheckBoxes - doubleclicks is not processing...</summary>
		public event EventHandler<ListViewItem[]> Items_NeedEdit = delegate { };

		///<summary>When DEL key pressed</summary>
		public event EventHandler<ListViewItem[]> Items_NeedDelete = delegate { };

		///<summary>When INS key pressed</summary>
		public event EventHandler Items_NeedInsert = delegate { };

		///<summary>When F5 key pressed</summary>
		public event EventHandler Items_NeedRefreshList = delegate { };

		///<summary>When Ctrl+C pressed</summary>
		public event EventHandler<ListViewItem[]> ClipboardCopy = delegate { };

		///<summary>When Ctrl+V pressed</summary>
		public event EventHandler ClipboardPaste = delegate { };


		#endregion


		#region Processing UI keyboard and mouse events


		private void AddKeyboardAndMousehandlers()
		{
			MouseDoubleClick += On_MouseDoubleClick!;
			this.KeyDown += On_KeyDown!;
			this.KeyPress += On_KeyPress!;
			this.KeyUp += On_KeyUp!;
		}


		private void On_MouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;
			if (this.CheckBoxes) return;

			var POS = e.Location;
			var LI = this.GetItemAt(POS.X, POS.Y);
			if (null == LI) return;
			this.On_Items_NeedEdit(LI.eToArrayOf());
		}

		private void On_KeyDown(Object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.A when (e.Modifiers == Keys.Control && MultiSelect):
					{
						e.SuppressKeyPress = true;
						SelectAllItems();
						break;
					}

				case Keys.Insert when (e.Modifiers == Keys.None):
					{
						e.SuppressKeyPress = this.On_Items_NeedInsert();
						break;
					}

				case Keys.Insert when (e.Modifiers == Keys.Control):
				case Keys.C when (e.Modifiers == Keys.Control):
					{
						e.SuppressKeyPress = this.On_Clipboard_Copy();
						break;
					}

				case Keys.Insert when (e.Modifiers == Keys.Shift):
				case Keys.V when (e.Modifiers == Keys.Control):
					{
						e.SuppressKeyPress = this.On_Clipboard_Paste();
						break;
					}

				default:
					break;
			}
		}
		private void On_KeyPress(Object sender, KeyPressEventArgs e) //,Handles this.KeyPress
		{
			if (e.KeyChar != 0xD) return;
			e.Handled = this.On_Items_NeedEdit();
		}
		private void On_KeyUp(Object sender, KeyEventArgs e)
		{

			switch (e.KeyCode)
			{
				case Keys.Delete when (e.Modifiers == Keys.None):
					{
						e.SuppressKeyPress = this.On_Items_NeedDelete();
						break;
					}

				case Keys.F5:
					{
						e.SuppressKeyPress = this.On_Items_NeedRefreshList();
						break;
					}


				default:
					break;
			}
		}



		#endregion


		protected virtual bool On_Items_NeedRefreshList()
		{
			Items_NeedRefreshList?.Invoke(this, EventArgs.Empty);
			return true;
		}

		protected virtual bool On_Items_NeedEdit(ListViewItem[]? aSel = null)
		{
			if (null == aSel || !aSel.Any()) aSel = this.eSelectedItemsAsIEnumerable().ToArray();
			if (!aSel.Any()) return false;
			if (!this.MultiSelect) aSel = aSel.First().eToArrayOf();
			Items_NeedEdit?.Invoke(this, aSel);
			return true;
		}

		protected virtual bool On_Items_NeedDelete()
		{
			var aSel = this.eSelectedItemsAsIEnumerable().ToArray();
			if (!aSel.Any()) return false;
			Items_NeedDelete?.Invoke(this, aSel);
			return true;
		}

		private bool On_Items_NeedInsert()
		{
			Items_NeedInsert?.Invoke(this, EventArgs.Empty);
			return true;
		}

		private bool On_Clipboard_Copy()
		{
			var aSel = this.eSelectedItemsAsIEnumerable().ToArray();
			if (!aSel.Any()) return false;
			ClipboardCopy?.Invoke(this, aSel);
			return true;
		}

		private bool On_Clipboard_Paste()
		{
			ClipboardPaste?.Invoke(this, EventArgs.Empty);
			return true;
		}


		#endregion


		#region Avoid flickering

		/*
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			//,e.Graphics.DrawString("FUCK!", Font, Brushes.Red, ClientRectangle);
		}
		 */
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//,base.OnPaintBackground(pevent);
		}


		#endregion


		#region Item drag visual support
		// Dragging items in a ListView control with visual insertion guides
		// http://www.cyotek.com/blog/dragging-items-in-a-listview-control-with-visual-insertion-guides



		public abstract class DragDrop_ExternalFilesBaseEventArgs : EventArgs
		{
			public FileInfo[] Files { get; protected set; } = { };
			public DragDrop_ExternalFilesBaseEventArgs(FileInfo[] files) : base() { Files = files; }
		}

		public sealed class DragDrop_DragEnterExternalFilesEventArgs : DragDrop_ExternalFilesBaseEventArgs
		{
			public bool Cancel = false;
			public DragDrop_DragEnterExternalFilesEventArgs(FileInfo[] files) : base(files) { }

			public void SetFiles(FileInfo[] files)
			{
				Files = files;
			}
		}

		public sealed class DragDrop_DropExternalFilesEventArgs : DragDrop_ExternalFilesBaseEventArgs
		{
			public readonly int InsertionIndex;
			public DragDrop_DropExternalFilesEventArgs(FileInfo[] files, int insertionIndex) : base(files) { InsertionIndex = insertionIndex; }
		}


		public class DragDrop_ItemReorderEventArgs : CancelListViewItemDragEventArgs
		{
			public ListViewItem DropItem { get; protected set; }

			/// <summary>Gets the insertion index of the drag operation.</summary>
			public int InsertionIndex { get; protected set; }

			/// <summary>Gets the relation of the <see cref="InsertionIndex"/></summary>
			public DragDropInsertionModes InsertionMode { get; protected set; }

			/// <summary>Gets the coordinates of the mouse (in pixels) during the generating event.</summary>
			public Point Mouse { get; protected set; }


			//protected DragDrop_ItemReorderEventArgs() : base() { }

			public DragDrop_ItemReorderEventArgs(ListViewItem sourceItem, ListViewItem dropItem, int insertionIndex, DragDropInsertionModes insertionMode, Point mouse) : base(sourceItem)
			{
				Item = sourceItem;
				DropItem = dropItem;
				Mouse = mouse;
				InsertionIndex = insertionIndex;
				InsertionMode = insertionMode;
			}
		}


		public class CancelListViewItemDragEventArgs : CancelEventArgs
		{
			//protected CancelListViewItemDragEventArgs() : base() { }

			public CancelListViewItemDragEventArgs(ListViewItem item) : base() { this.Item = item; }

			public ListViewItem Item { get; protected set; }
		}


		/// <summary>Determines the insertion mode of a drag operation</summary>
		public enum DragDropInsertionModes
		{
			/// <summary>The source item will be inserted before the destination item</summary>
			Before,

			/// <summary>The source item will be inserted after the destination item</summary>
			After
		}

		[Flags]
		public enum DragDropModes : int
		{
			None = 0,
			ItemsReorder = 1,
			DropFromExternal = 2,
			DragToExternal = 4
		}


		private const string C_CATEGORY_DRAG_DROP = "Drag Drop";
		private const string C_CATEGORY_PROPERTY_CHANGED = "Property Changed";

		#region Item Drag Events

		[Category(C_CATEGORY_PROPERTY_CHANGED)]
		public event EventHandler DragDropModeChanged = delegate { };

		/// <summary>Occurs when the InsertionLineColor property value changes.</summary>
		[Category(C_CATEGORY_PROPERTY_CHANGED)]
		public event EventHandler InsertionLineColorChanged = delegate { };



		/// <summary>Occurs when a drag-and-drop operation for an item is completed.</summary>
		[Category(C_CATEGORY_DRAG_DROP)]
		public event EventHandler<DragDrop_ItemReorderEventArgs> DragDrop_ItemReorder = delegate { };

		/// <summary>Occurs when the user begins dragging an item.</summary>
		[Category(C_CATEGORY_DRAG_DROP)]
		public event EventHandler<CancelListViewItemDragEventArgs> ItemDragStart = delegate { };


		[Category(C_CATEGORY_DRAG_DROP)]
		public event EventHandler<DragDrop_DragEnterExternalFilesEventArgs> DragDrop_DragEnterExternalFiles = delegate { };

		[Category(C_CATEGORY_DRAG_DROP)]
		public event EventHandler<DragDrop_DropExternalFilesEventArgs> DragDrop_DropExternalFiles = delegate { };

		#endregion



		private int _dragDrop_InsertionIndex;
		private bool _dragDrop_ItemDragInProgress;
		private DragDropInsertionModes _dragDrop_InsertionMode;
		private DragDropModes _dragDrop_Mode = DragDropModes.None;
		private Color _dragDrop_InsertionLineColor;
		private FileInfo[] _dragDrop_FilesFromExternalSource = { };

		private bool _dragDrop_DropFilesFromExternalSourceInProgress => _dragDrop_FilesFromExternalSource.Any();


		#region public Properties


		/// <summary> Hiding parent AllowDrop property</summary>
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
		protected bool AllowDrop => base.AllowDrop;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword


		private bool DragDrop_AllowItemDrag => _dragDrop_Mode.HasFlag(DragDropModes.ItemsReorder) || _dragDrop_Mode.HasFlag(DragDropModes.DragToExternal);


		[Category(C_CATEGORY_DRAG_DROP)]
		[DefaultValue(DragDropModes.None)]
		[RefreshProperties(RefreshProperties.All)]
		public DragDropModes DragDropMode
		{
			get => _dragDrop_Mode;// AllowItemDrag && this.AllowDrop;
			set
			{
				_dragDrop_Mode = value;
				//bool itemDrag = false;
				bool allowDrop = false;

				try
				{

					if (_dragDrop_Mode == DragDropModes.None)
					{
						//itemDrag = false;
						//itemDrag = false;
						return;
					}

					if (_dragDrop_Mode.HasFlag(DragDropModes.ItemsReorder))
					{
						//itemDrag = true;
						allowDrop = true;
						return;
					}

					if (_dragDrop_Mode.HasFlag(DragDropModes.DropFromExternal))
					{
						allowDrop = true;
					}
					if (_dragDrop_Mode.HasFlag(DragDropModes.DragToExternal))
					{
						//itemDrag = true;
					}
				}
				finally
				{
					//AllowItemDrag = itemDrag;
					base.AllowDrop = allowDrop;

					OnDragDropModeChanged(EventArgs.Empty);
				}
			}
		}



		/// <summary>Gets or sets the color of the insertion line drawn when dragging items within the control.</summary>
		[Category(C_CATEGORY_DRAG_DROP)]
		[DefaultValue(typeof(Color), "Red")]
		[RefreshProperties(RefreshProperties.All)]
		public Color DragDrop_InsertionLineColor
		{
			get => _dragDrop_InsertionLineColor;
			set => _dragDrop_InsertionLineColor.eUpdateIfNotEquals(value, () => OnInsertionLineColorChanged(EventArgs.Empty));
		}


		#endregion



		#region private Members

		private void DrawInsertionLine()
		{
			const int InsertionMarkWidth = 3;
			const int InsertionMarkArrowSize = 8;

			if (_dragDrop_InsertionIndex < 0 || _dragDrop_InsertionIndex >= this.Items.Count) return;

			Rectangle rcItemToDrop = this.Items[_dragDrop_InsertionIndex].GetBounds(ItemBoundsPortion.Entire);
			using Graphics g = CreateGraphics();

			Color clr = SystemColors.HotTrack;
			using Pen pnArrowEdge = new(clr, InsertionMarkWidth);
			using Brush brArrowFill = new SolidBrush(clr);

			Point[] ptsStartArrow;
			Point[] ptsEndArrow;
			int ArrowSize2 = (InsertionMarkArrowSize / 2);

			switch (View)
			{
				case View.Details:  //Vertical grid
					{
						int x1 = 0; // aways fit the line to the client area, regardless of how the user is scrolling
						int y = this._dragDrop_InsertionMode == DragDropInsertionModes.Before
							? rcItemToDrop.Top
							: rcItemToDrop.Bottom;

						// again, make sure the full width fits in the client area
						int width = Math.Min(rcItemToDrop.Width - rcItemToDrop.Left, this.ClientSize.Width);


						int x2 = x1 + width;
						ptsStartArrow = new[]
						{
							new Point(x1, y - ArrowSize2),
							new Point(x1 + InsertionMarkArrowSize, y),
							new Point(x1, y + ArrowSize2)
						};

						ptsEndArrow = new[]
						{
							new Point(x2, y - ArrowSize2),
							new Point(x2 - InsertionMarkArrowSize, y),
							new Point(x2, y + ArrowSize2)
						};

					}
					break;

				default:
					{

						int x = this._dragDrop_InsertionMode == DragDropInsertionModes.Before
							? rcItemToDrop.Left
							: rcItemToDrop.Right;

						int y1 = rcItemToDrop.Top;
						int y2 = rcItemToDrop.Bottom;

						ptsStartArrow = new[]
						{
							new Point(x-ArrowSize2, y1),
							new Point(x , y1+InsertionMarkArrowSize),
							new Point(x+ArrowSize2, y1)
						};
						ptsEndArrow = new[]
						{
							new Point(x-ArrowSize2, y2),
							new Point(x , y2-InsertionMarkArrowSize),
							new Point(x+ArrowSize2, y2)
						};

					}
					break;
			}

			//g.DrawRectangle(Pens.Green, rcItemToDrop);
			/*
			g.DrawLine(pnArrowEdge, x1, y, x2 - 1, y);
			g.FillPolygon(brArrowFill, ptsStartArrow);
			g.FillPolygon(brArrowFill, ptsEndArrow);
			 */

			g.FillPolygon(brArrowFill, ptsStartArrow);
			g.FillPolygon(brArrowFill, ptsEndArrow);
			g.DrawLine(pnArrowEdge, ptsStartArrow[1], ptsEndArrow[1]);

		}

		#endregion


		#region OnDrag...


		protected override void OnItemDrag(ItemDragEventArgs e)
		{
			if (this.DragDrop_AllowItemDrag && this.Items.Count > 1 && e.Item != null)
			{
				CancelListViewItemDragEventArgs args = new((ListViewItem)e.Item);
				OnItemDragStart(args);

				if (!args.Cancel)
				{
					this._dragDrop_ItemDragInProgress = true;
					SelectedListViewItemCollection sel = SelectedItems;
					if (sel.Count > 0)
					{
						DragDropEffects eff = DragDropEffects.None;

						if (DragDropMode.HasFlag(DragDropModes.ItemsReorder))
						{
							eff = DragDropEffects.Move;
						}
						if (DragDropMode.HasFlag(DragDropModes.DragToExternal))
						{
							eff = DragDropEffects.Move | DragDropEffects.Copy;
						}

						this.DoDragDrop(sel, eff);
						//this.DoDragDrop(sel, eff DragDropEffects.Move);
					}
					else
					{
						this.DoDragDrop(e.Item, DragDropEffects.Move);
					}
				}
			}

			base.OnItemDrag(e);
		}


		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			e.Effect = DragDropEffects.None;

			if (!DragDropMode.HasFlag(DragDropModes.DropFromExternal)) return;


			DataObject? shellData = e.Data as DataObject;
			if (shellData == null) return;
			if (!shellData.ContainsFileDropList()) return;

			FileInfo[] files = shellData.GetFileDropList()
				.Cast<string>()
				.Select(s => new FileInfo(s))
				.ToArray();

			DragDrop_DragEnterExternalFilesEventArgs ea = new(files);
			DragDrop_DragEnterExternalFiles?.Invoke(this, ea);
			if (ea.Cancel || !ea.Files.Any()) return;

			_dragDrop_FilesFromExternalSource = ea.Files;
			e.Effect = DragDropEffects.Copy;
		}


		protected override void OnDragOver(DragEventArgs e)
		{

			if (_dragDrop_ItemDragInProgress || (_dragDrop_DropFilesFromExternalSourceInProgress))
			{

				int newInsertionIndex;
				DragDropInsertionModes newInsMode;

				Point ptCursor = PointToClient(new Point(e.X, e.Y));
				ListViewItem? dropToItem = null;
				if (DragDropMode.HasFlag(DragDropModes.ItemsReorder))//Allow draw insertion mark only when DragDropModes.ItemsReorder is set!
				{
					var nearestToCursor = this.eGetNearestItem(ptCursor);
					dropToItem = nearestToCursor.Item;
				}

				if (dropToItem != null)
				{
					newInsertionIndex = dropToItem.Index;

					Rectangle dropToItemBounds = dropToItem.GetBounds(ItemBoundsPortion.Entire);
					var ptDropToItemCenter = dropToItemBounds.eGetCenter().eRoundToInt();

					switch (View)
					{
						case View.Details:
							newInsMode = ptCursor.Y < ptDropToItemCenter.Y
								? DragDropInsertionModes.Before
								: DragDropInsertionModes.After;
							break;

						default:
							newInsMode = ptCursor.X < ptDropToItemCenter.X
								? DragDropInsertionModes.Before
								: DragDropInsertionModes.After;
							break;
					}

					e.Effect = DragDropEffects.Move;
				}
				else
				{
					newInsertionIndex = -1;
					newInsMode = _dragDrop_InsertionMode;
					e.Effect = DragDropEffects.Copy;
				}

				if (newInsertionIndex != _dragDrop_InsertionIndex || newInsMode != _dragDrop_InsertionMode)
				{
					this._dragDrop_InsertionMode = newInsMode;
					this._dragDrop_InsertionIndex = newInsertionIndex;
					this.Invalidate();
				}
			}

			base.OnDragOver(e);
		}


		protected override void OnDragLeave(EventArgs e)
		{
			_dragDrop_FilesFromExternalSource = Array.Empty<FileInfo>();
			_dragDrop_InsertionIndex = -1;

			Invalidate();

			base.OnDragLeave(e);
		}


		protected override void OnDragDrop(DragEventArgs e)
		{
			try
			{
				if (_dragDrop_ItemDragInProgress)
				{
					OnDragDrop_ItemReorder(e);
				}
				else if (_dragDrop_DropFilesFromExternalSourceInProgress)
				{
					OnDragDrop_DropExternalFiles(e);
				}
			}
			finally { this.Invalidate(); }

			base.OnDragDrop(e);
		}


		protected void OnDragDrop_DropExternalFiles(DragEventArgs e)
		{
			if (!this._dragDrop_FilesFromExternalSource.Any()) return;

			try
			{
				BeginUpdate();
				{

					int dropIndex = _dragDrop_InsertionIndex;
					if (_dragDrop_InsertionMode == DragDropInsertionModes.After) dropIndex++;

					dropIndex = (dropIndex >= Items.Count)
						? dropIndex = -1
						: dropIndex;

					DragDrop_DropExternalFilesEventArgs ea = new(_dragDrop_FilesFromExternalSource, dropIndex);
					DragDrop_DropExternalFiles?.Invoke(this, ea);
				}
			}
			finally
			{
				EndUpdate();
				Refresh();
				_dragDrop_InsertionIndex = -1;
				_dragDrop_ItemDragInProgress = false;
				_dragDrop_FilesFromExternalSource = Array.Empty<FileInfo>();
			}
		}


		protected void OnDragDrop_ItemReorder(DragEventArgs e)
		{
			if (!this._dragDrop_ItemDragInProgress) return;

			try
			{

				ListViewItem? dropToItem = (_dragDrop_InsertionIndex != -1)
					? this.Items[this._dragDrop_InsertionIndex]
					: null;

				if (dropToItem != null)
				{
					object? sel = e.Data?.GetData(typeof(SelectedListViewItemCollection));
					if (sel != null)
					{
						var dragItems = ((SelectedListViewItemCollection)sel)
							.Cast<ListViewItem>()
							.ToArray();

						if (!dragItems.Any()) return;

						try
						{
							BeginUpdate();
							{
								foreach (var dragItem in dragItems)
								{
									int dropIndex = dropToItem.Index;

									if (dragItem.Index < dropIndex)
										dropIndex--;

									if (this._dragDrop_InsertionMode == DragDropInsertionModes.After && dragItem.Index < this.Items.Count - 1)
										dropIndex++;

									if (dropIndex == dragItem.Index) return;//Drop on itself

									Point clientPoint = PointToClient(new Point(e.X, e.Y));
									DragDrop_ItemReorderEventArgs args = new(
										dragItem,
										dropToItem,
										dropIndex,
										this._dragDrop_InsertionMode,
										clientPoint);

									this.OnDragDrop_ItemReorder(args);
									if (args.Cancel) continue;

									this.Items.Remove(dragItem);
									this.Items.Insert(dropIndex, dragItem);
									dragItem.Selected = true;
								}
							}
						}
						finally
						{
							FocusedItem = dragItems.First();
							EndUpdate();
							Refresh();
						}
					}
				}
			}
			finally
			{
				this._dragDrop_InsertionIndex = -1;
				this._dragDrop_ItemDragInProgress = false;
			}
		}




		protected virtual void OnDragDropModeChanged(EventArgs e) => this.DragDropModeChanged?.Invoke(this, e);


		protected virtual void OnInsertionLineColorChanged(EventArgs e) => this.InsertionLineColorChanged?.Invoke(this, e);


		protected virtual void OnDragDrop_ItemReorder(DragDrop_ItemReorderEventArgs e) => this.DragDrop_ItemReorder?.Invoke(this, e);


		protected virtual void OnItemDragStart(CancelListViewItemDragEventArgs e) => this.ItemDragStart?.Invoke(this, e);


		protected virtual void OnWmPaint(ref Message m) => this.DrawInsertionLine();





		#endregion


		#endregion


		#region EmptyText


		private string _emptyText = string.Empty;
		[DefaultValue("")]
		public string EmptyText
		{
			get { return _emptyText; }
			set
			{
				//,'Можно переназначить сообщение LVN_GETEMPTYMARKUP в список, отправив сообщение LVM_RESETEMPTYTEXT = (LVM_FIRST + 84) в список или получив интерфейс IListView и выполнив метод
				//,'ResetEmptyText.Таким образом вы можете условно изменить или очистить пустой текст :)
				_emptyText = value;
				ResetEmptyText();
				Invalidate();
			}
		}

		private bool _emptyTextDisplayInCenter = false;
		[DefaultValue(false)]
		public bool EmptyTextDisplayInCenter
		{
			get { return _emptyTextDisplayInCenter; }
			set
			{
				_emptyTextDisplayInCenter = value;
				ResetEmptyText();
				Invalidate();
			}
		}


		private void ResetEmptyText()
		{
			if (!IsHandleCreated) return;
			uom.WinAPI.windows.SendMessage(Handle, (int)ListViewMessages.LVM_RESETEMPTYTEXT, 0, 0);
		}


		#endregion

















		/* 
		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data. </param>
		protected override void OnPaint(PaintEventArgs e) => base.OnPaint(e);
		 */

		[DebuggerStepThrough]
		protected override void WndProc(ref Message m)
		{
			var msg = (uom.WinAPI.windows.WindowMessages)m.Msg;
			switch (msg)
			{
				case uom.WinAPI.windows.WindowMessages.OCM_NOTIFY:
					OnOCNNotifyMessage(ref m);
					break;

			}

			base.WndProc(ref m);

			switch (msg)
			{
				case uom.WinAPI.windows.WindowMessages.WM_PAINT:
					this.OnWmPaint(ref m);
					break;

				case uom.WinAPI.windows.WindowMessages.WM_LBUTTONUP:
					//This provides collapsing/expanding groups by clicking on the right triangle.
					//Collapsing/expanding by doubleclicking on group header - occurs independently from this by itself.
#if !NET
					base.DefWndProc(ref m);
					CheckGroupsCollapsedStatesForChanges();
#endif
					break;
			}
		}


		private void OnOCNNotifyMessage(ref Message m)
		{
			var nmhdr = (NMHDR)m.GetLParam(typeof(NMHDR))!;
			var ocmNotifyCode = (ListViewNotifyMessages)nmhdr.code;
			switch (ocmNotifyCode)
			{
				case ListViewNotifyMessages.LVN_GETEMPTYMARKUP:
					if (Control.FromHandle(nmhdr.hwndFrom) == this)
					{
						var markup = (NMLVEMPTYMARKUP)m.GetLParam(typeof(NMLVEMPTYMARKUP))!;
						markup.szMarkup = _emptyText;

						const int EMF_CENTERED = 1;
						//if (_emptyTextDisplayInCenter) markup.dwFlags = EMF_CENTERED;
						markup.dwFlags = _emptyTextDisplayInCenter ? EMF_CENTERED : 0;

						Marshal.StructureToPtr(markup, m.LParam, false);
						m.Result = new IntPtr(1);
						return;
					}
					break;
			}


		}


		protected override void OnNotifyMessage(Message m)
		{
			var msg = (uom.WinAPI.windows.WindowMessages)m.Msg;
			base.OnNotifyMessage(m);
		}







		#region Group Tools

		private const string DEFAULT_LIST_VIEW_GROUP_NAME = "default";

		private Dictionary<string, bool> _knownGroupsStates = [];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Dictionary<string, bool> GetCurrentGroupsCollapsedStates()
			=> this
				.eGroupsAsIEnumerable()
				.Select(grp =>
				{
					string id = grp.eGetStringID(DEFAULT_LIST_VIEW_GROUP_NAME).ToLower().Trim();
					bool collapsed = false;
#if NET
					collapsed = grp.CollapsedState == ListViewGroupCollapsedState.Collapsed;
#else
					collapsed = grp.eGetState_IsCollapsed();
#endif
					return (Name: id, Collapsed: collapsed);
				}
				)
				.Where(grp => !string.IsNullOrWhiteSpace(grp.Name))
				.OrderBy(grp => grp.Name)
				.ToDictionary(grp => grp.Name, grp => grp.Collapsed);


#if !NET
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckGroupsCollapsedStatesForChanges()
		{
			var currentStates = GetCurrentGroupsCollapsedStates();
			bool hasAnyChanges = !currentStates.eIsDictionaryEqualTo(_knownGroupsStates);
			if (hasAnyChanges)
			{
				this.GroupsCollapsedStateChangedByMouse?.Invoke(this, "");
			}
		}
#endif


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private FileInfo GetGroupsCollapsedStateStorage(string? @directory = null, string dataID = "")
		{
			const string GROUPS_SATES_EXT = ".groups.xml";
			string lvID = this.eCreateListViewID();
			if (!string.IsNullOrWhiteSpace(dataID)) lvID += $"_{dataID}";

			DirectoryInfo di = (@directory != null)
				? new(@directory)
				: uom.AppInfo.UserAppDataPath(true);

			return System.IO.Path.Combine(di.FullName, lvID + GROUPS_SATES_EXT)
				.eToFileInfo()!;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public FileInfo SaveAllGroupsCollapsedStates(string? @directory = null, string dataID = "", bool append = true)
		{
			Dictionary<string, bool>? states = null;
			if (append)//Do not replace all file data, but update known groups states
			{
				states = LoadGroupsCollapsedStateFromStorage(@directory, dataID) ?? [];
				var currentStates = GetCurrentGroupsCollapsedStates();
				foreach (var kvp in currentStates) states[kvp.Key] = kvp.Value;
			}
			else//Just only save groups existing in the listview
				states = GetCurrentGroupsCollapsedStates();

			_knownGroupsStates = states;

			var sortedRows = _knownGroupsStates
				.Select(kvp => (Name: kvp.Key, Collapsed: kvp.Value))
				.Where(grp => !string.IsNullOrWhiteSpace(grp.Name))
				.OrderBy(grp => grp.Name)
				.ToArray();

			var dd = _knownGroupsStates.eDumpArrayToString();
			Debug.WriteLine($"SaveAllGroupsCollapsedStates: {dd}");

			var text = sortedRows.eSerializeAsXML();

			//Debug.WriteLine($"RestoreAllGroupsCollapsedStateFromStorage (dataID: '{dataID}') GroupID: '{grpID}', State: '{loadedCollapsedState}'");

			FileInfo fi = GetGroupsCollapsedStateStorage(@directory, dataID);
			using (var sw = fi.eCreateWriter(FileMode.OpenOrCreate, encoding: System.Text.Encoding.Unicode))
			{
				sw.BaseStream.eTruncate(); //trim previous file data										   
				sw.WriteLine(text); //writing actual data
				sw.Flush();
			}
			return fi;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Dictionary<string, bool>? LoadGroupsCollapsedStateFromStorage(string? @directory = null, string dataID = "")
		{
			try
			{
				FileInfo fi = GetGroupsCollapsedStateStorage(@directory, dataID);
				if (!fi.Exists) return null;

				return fi
					.eReadAsText()!
					.Trim()
					.eDeSerializeXML<(string Name, bool Collapsed)[]>(throwOnError: true)!
					.Where(grp => !string.IsNullOrWhiteSpace(grp.Name))
					.OrderBy(grp => grp.Name)
					.ToDictionary(r => r.Name, r => r.Collapsed);
			}
			catch (Exception ex)
			{
				//just ignore errors
				ex.eLogError(false);
				return null;
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RestoreAllGroupsCollapsedStateFromStorage(string? @directory = null, string dataID = "")
		{
			var loadedGroupStates = LoadGroupsCollapsedStateFromStorage(@directory, dataID);
			try
			{
				this.erunOnLockedUpdate(delegate
				{
					this.eGroupsAsIEnumerable()
					.eForEach(grp =>
						{
#if !NET
							grp.eSetStateFlag(ListViewGroupState.Collapsible);
#endif
							string grpID = grp.eGetStringID(DEFAULT_LIST_VIEW_GROUP_NAME).ToLower().Trim();

							if (loadedGroupStates != null && loadedGroupStates.TryGetValue(grpID, out bool loadedCollapsedState))
							{
#if NET
								grp.CollapsedState = loadedCollapsedState
									? ListViewGroupCollapsedState.Collapsed
									: ListViewGroupCollapsedState.Expanded;

								Debug.WriteLine($"RestoreAllGroupsCollapsedStateFromStorage (dataID: '{dataID}') GroupID: '{grpID}', State: '{loadedCollapsedState}'");
#else
								grp.eSetState_Collapsed(loadedCollapsedState);
#endif
							}
						});
				});

			}
			catch (Exception ex)
			{
				//just ignore errors
				ex.eLogError(false);
			}
		}






#if !NET




		#region Groups API


		[Flags]
		public enum ListViewGroupMask : Int32
		{
			LVGF_NONE = 0x0,
			LVGF_HEADER = 0x1,
			LVGF_FOOTER = 0x2,
			LVGF_STATE = 0x4,
			LVGF_ALIGN = 0x8,
			LVGF_GROUPID = 0x10,
			#region Version 6.00 and Windows Vista"
			LVGF_SUBTITLE = 0x100,
			LVGF_TASK = 0x200,
			LVGF_DESCRIPTIONTOP = 0x400,
			LVGF_DESCRIPTIONBOTTOM = 0x800,
			LVGF_TITLEIMAGE = 0x1000,
			LVGF_EXTENDEDIMAGE = 0x2000,
			LVGF_ITEMS = 0x4000,
			LVGF_SUBSET = 0x8000,
			LVGF_SUBSETITEMS = 0x10000
			#endregion
		}

		[Flags]
		public enum ListViewGroupState : int
		{
			///<summary>This is non WinAPI value used to specify that state flag must not be changed!</summary>
			Invalid = -1,

			///<summary>Groups are expanded, the group name is displayed, and all items in the group are displayed.</summary>
			Normal = 0,
			///<summary>The group is collapsed.</summary>
			Collapsed = 1,
			///<summary>The group is hidden.</summary>
			Hidden = 2,
			#region Version 6.00 and Windows Vista
			///<summary>Version 6.00 and Windows Vista. The group does not display a header.</summary>
			NoHeader = 4,
			///<summary>Version 6.00 and Windows Vista. The group can be collapsed.</summary>
			Collapsible = 8,
			///<summary>Version 6.00 and Windows Vista. The group has keyboard focus.</summary>
			Focused = 16,
			///<summary>Version 6.00 and Windows Vista. The group is selected.</summary>
			Selected = 32,
			///<summary>Version 6.00 and Windows Vista. The group displays only a portion of its items.</summary>
			SubSeted = 64,
			///<summary>Version 6.00 and Windows Vista. The subset link of the group has keyboard focus.</summary>
			SubSetLinkFocused = 128
			#endregion
		}

		public enum ListViewGroupAlign : int
		{
			LVGA_HEADER_LEFT = 0x1,
			LVGA_HEADER_CENTER = 0x2,
			LVGA_HEADER_RIGHT = 0x4,  // Don//t forget to validate exclusivity
			LVGA_FOOTER_LEFT = 0x8,
			LVGA_FOOTER_CENTER = 0x10,
			LVGA_FOOTER_RIGHT = 0x20   // Don//t forget to validate exclusivity
		}


		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode), Description("LVGROUP StructureUsed to set and retrieve groups.")]
		public struct LVGROUP
		{
			///<summary>Size of this structure, in bytes.</summary>
			public Int32 CbSize;

			///<summary>Mask that specifies which members of the structure are valid input.
			///One or more of the following values:LVGF_NONENo other items are valid.</summary>
			public ListViewGroupMask Mask = ListViewGroupMask.LVGF_NONE;

			///<summary>Pointer to a null-terminated string that contains the header text when item information is being set.  if  group information is being retrieved, this member specifies the address of the buffer that receives the header text.</summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string PszHeader = string.Empty;

			///<summary> Size in TCHARs of the buffer pointed to by the pszHeader member.
			/// if the structure is not receiving information about a group, this member is ignored. </summary>
			public Int32 CchHeader = 0;

			///<summary>
			///Pointer to a null-terminated string that contains the footer text
			///when item information is being set.  if  group information is being retrieved,
			///this member specifies the address of the buffer that receives the footer text.
			///</summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string PszFooter = string.Empty;

			///<summary> Size in TCHARs of the buffer pointed to by the pszFooter member.
			/// if the structure is not receiving information about a group, this member is ignored.</summary>
			public Int32 CchFooter = 0;

			///<summary>ID of the group.</summary>
			public Int32 IGroupId = 0;

			///<summary>Mask used with LVM_GETGROUPINFO (Microsoft Windows XP and Windows Vista)
			///and LVM_SETGROUPINFO (Windows Vista only) to specify which flags in the
			///state value are being retrieved or set.</summary>
			public Int32 StateMask = 0;

			///<summary>Flag that can have one of the following values:LVGS_NORMALGroups are expanded,
			///the group name is displayed, and all items in the group are displayed.</summary>
			public ListViewGroupState State = ListViewGroupState.Normal;

			///<summary>Indicates the alignment of the header or footer text for the group.
			///It can have one or more of the following values. Use one of the header flags.
			///Footer flags are optional.
			///Windows XP: Footer flags are reserved.LVGA_FOOTER_CENTER Reserved.</summary>
			public ListViewGroupAlign UAlign = ListViewGroupAlign.LVGA_HEADER_LEFT;

			///<summary>
			///Windows Vista. Pointer to a null-terminated string that contains the
			///subtitle text when item information is being set.  if  group information
			///is being retrieved, this member specifies the address of the buffer that
			///receives the subtitle text. This element is drawn under the header text.
			///</summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string PszSubtitle = string.Empty;

			///<summary>Windows Vista. Size, in TCHARs, of the buffer pointed to by the
			///pszSubtitle member.  if  the structure is not receiving information
			///about a group, this member is ignored.</summary>
			public int CchSubtitle = 0;

			///<summary>Windows Vista. Pointer to a null-terminated string that contains the text
			///for a task link when item information is being set.  if  group information
			///is being retrieved, this member specifies the address of the buffer
			///that receives the task text. This item is drawn right-aligned opposite
			///the header text. When clicked by the user,
			///the task link generates an LVN_LINKCLICK notification.</summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string PszTask = string.Empty;

			///<summary>Windows Vista. Size in TCHARs of the buffer pointed to by the pszTask member.
			/// if  the structure is not receiving information about a group, this member is ignored.</summary>
			public int CchTask = 0;

			///<summary>Windows Vista. Pointer to a null-terminated string that contains the top description text when item information is being set.
			/// if  group information is being retrieved, this member specifies the address of the buffer that receives the top description text.
			///This item is drawn opposite the title image when there is a title image, no extended image, and uAlign==LVGA_HEADER_CENTER.
			///</summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string PszDescriptionTop = string.Empty;

			///<summary>Windows Vista. Size in TCHARs of the buffer pointed to by the
			///pszDescriptionTop member.  if  the structure is not receiving information
			///about a group, this member is ignored.
			///</summary>
			public int CchDescriptionTop = 0;

			///<summary>
			///Windows Vista. Pointer to a null-terminated string that contains the
			///bottom description text when item information is being set.
			/// if  group information is being retrieved, this member specifies the address
			///of the buffer that receives the bottom description text.
			///This item is drawn under the top description text when there is a title image,
			///no extended image, and uAlign==LVGA_HEADER_CENTER.
			///</summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string PszDescriptionBottom = string.Empty;

			///<summary>Windows Vista. Size in TCHARs of the buffer pointed to by the pszDescriptionBottom member.  if  the structure is not receiving information about a group, this member is ignored. </summary>
			public int CchDescriptionBottom = 0;

			///<summary>Windows Vista. Index of the title image in the control imagelist.</summary>
			public Int32 ITitleImage = 0;

			///<summary>Windows Vista. Index of the extended image in the control imagelist.</summary>
			public Int32 IExtendedImage = 0;

			///<summary>Windows Vista. Read-only.</summary>
			public Int32 IFirstItem = 0;

			///<summary>Windows Vista. Read-only in non-owner data mode.</summary>
			public IntPtr CItems = IntPtr.Zero;

			///<summary> Windows Vista. NULL if group is not a subset.
			///Pointer to a null-terminated string that contains the subset title text when item information is being set.
			/// if  group information is being retrieved, this member specifies the address
			///of the buffer that receives the subset title text. </summary>
			public IntPtr PszSubsetTitle = IntPtr.Zero;

			///<summary>Windows Vista. Size in TCHARs of the buffer pointed to by the pszSubsetTitle member.
			/// if  the structure is not receiving information about a group, this member is ignored.</summary>
			public IntPtr CchSubsetTitle = IntPtr.Zero;

			public LVGROUP() { this.CbSize = Marshal.SizeOf(typeof(LVGROUP)); }
		}


		[DllImport(core.WINDLL_USER, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
		internal static extern Int32 SendMessage(
		[In] IntPtr hwnd,
		[In, MarshalAs(UnmanagedType.I4)] ListViewMessages wMsg,
		[In] Int32 groupId,
		[In, Out] int lParam);


		[DllImport(core.WINDLL_USER, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
		internal static extern Int32 SendMessage(
		[In] IntPtr hwnd,
		[In, MarshalAs(UnmanagedType.I4)] ListViewMessages wMsg,
		[In] Int32 groupId,
		[In, Out] ref LVGROUP lParam);

		#endregion


		#region SetGroup

		public static Int32 SetGroup(ListViewGroup lstvwgrp, LVGROUP group)
		{
			int result = 0;
			lstvwgrp.ListView!.eRunInUIThread(delegate
			{
				Int32 groupId = group.IGroupId;
				result = SendMessage(lstvwgrp.ListView!.Handle, ListViewMessages.LVM_SETGROUPINFO, groupId, ref group);
			});
			return result;
		}

		public static Int32 SetGroup(
			ListViewGroup lstvwgrp,
			string? Header = null,
			string? SubTitle = null,
			string? Footer = null,
			string? TaskLinkText = null,
			ListViewGroupAlign align = 0,
			ListViewGroupState state = ListViewGroupState.Invalid)
		{

			int groupId = 0;
			lstvwgrp!.ListView!.eRunInUIThread(delegate { groupId = lstvwgrp.eGetWin32ID(); });

			ListViewGroupMask eMask = ListViewGroupMask.LVGF_NONE;
			if (Header != null) eMask |= ListViewGroupMask.LVGF_HEADER;
			if (Footer != null) eMask |= ListViewGroupMask.LVGF_FOOTER;
			if (SubTitle != null) eMask |= ListViewGroupMask.LVGF_SUBTITLE;
			if (TaskLinkText != null) eMask |= ListViewGroupMask.LVGF_TASK;
			if (align != 0) eMask |= ListViewGroupMask.LVGF_ALIGN;
			if (state != ListViewGroupState.Invalid) eMask |= ListViewGroupMask.LVGF_STATE;

			LVGROUP group = new()
			{
				PszHeader = Header ?? string.Empty,
				PszFooter = Footer ?? string.Empty,
				PszSubtitle = SubTitle ?? string.Empty,
				PszTask = TaskLinkText ?? string.Empty,
				UAlign = align,
				State = state,
				Mask = eMask,
				IGroupId = groupId
			};

			return SetGroup(lstvwgrp, group);
		}



		#endregion


		#region SetGroupState

		public static void SetGroupState(ListViewGroup grp, ListViewGroupState state)
		{
			SetGroup(grp, state: state);
			grp.ListView!.eRunInUIThread(delegate { grp.ListView!.Refresh(); });
		}

		public static void SetGroupStateFlag(ListViewGroup grp, ListViewGroupState flag, bool flagState = true)
		{
			ListViewGroupState state = GetGroupState(grp);

			state |= flag;
			if (!flagState) state ^= flag;

			SetGroupState(grp, state);
		}




		/// <summary>Returns the combination of state values that are set. For example, if dwMask is LVGS_COLLAPSED and the value returned is zero, the LVGS_COLLAPSED state is not set. Zero is returned if the group is not found.</summary>
		/// <param name="Mask">Specifies the state values to retrieve. This is a combination of the flags listed for the state member of LVGROUP.</param>
		public static ListViewGroupState GetGroupState(ListViewGroup grp, ListViewGroupState? bitsToGet = null)
		{
			if (grp == null || grp.ListView == null) return default;
			if (bitsToGet == null) bitsToGet = (ListViewGroupState)ListViewGroupState.Collapsed.eMixFlagsAsInt32(ListViewGroupState.Invalid);

			ListViewGroupState result = 0;
			grp.ListView.eRunInUIThread(delegate
			{
				int groupID = grp.eGetWin32ID();
				result = (ListViewGroupState)SendMessage(grp.ListView.Handle, ListViewMessages.LVM_GETGROUPSTATE, groupID, (int)bitsToGet.Value);
			});
			return result;
		}

		#endregion


		private delegate void CallbackSetGroupString(ListViewGroup lstvwgrp, string value);


		#region SetGroupText

		/*	  	 

		/// <summary>Задаёт текст для группы, но не меняет флаг состояния группы (!!! Стандартный .Header= string  меняет флаг состояния!!!)</summary>
		public static void SetGroupText(lstvwgrp As ListViewGroup, Text As  string )
					 if (lstvwgrp == null) || (lstvwgrp.ListView == null)  {
				return
					 if (lstvwgrp.ListView.InvokeRequired)
				{
					Call lstvwgrp.ListView.Invoke(new CallbackSetGroupString(AddressOf SetGroupText), lstvwgrp, Text)
					 else
					{
						var GrpId = GetGroupID(lstvwgrp)
						 var LVG As new LVGROUP With
		{.CbSize = Marshal.SizeOf(LVG),
							.PszHeader = Text,
							.Mask = LVGROUP.ListViewGroupMask.LVGF_HEADER,
							.IGroupId = GrpId
		}

		Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessages.LVM_SETGROUPINFO, GrpId, LVG)
					 }
				}
		 */
		#endregion

		#region SetGroupFooter
		/*
		public static void SetGroupFooter(lstvwgrp As ListViewGroup, Footer As  string )
				  if (lstvwgrp == null) || (lstvwgrp.ListView == null)  {
		return
				  if (lstvwgrp.ListView.InvokeRequired)
		{
		Call lstvwgrp.ListView.Invoke(new CallbackSetGroupString(AddressOf SetGroupFooter), lstvwgrp, Footer)
				  else
		{

		 var GrpId = GetGroupID(lstvwgrp)
					  var group As  new LVGROUP With
		{.CbSize = Marshal.SizeOf(group),
													.PszFooter = Footer,
													.Mask = LVGROUP.ListViewGroupMask.LVGF_FOOTER,
													.IGroupId = GrpId
		}

		 Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessages.LVM_SETGROUPINFO, GrpId, group)
				  }
		}
		*/
		#endregion

		#region SetGroupSubTitle
		/*


		public static void SetGroupSubTitle(lstvwgrp As ListViewGroup, SubTitle As  string )
					 if (lstvwgrp == null) || (lstvwgrp.ListView == null)  {
		return
					 if (lstvwgrp.ListView.InvokeRequired)
		{
			Call lstvwgrp.ListView.Invoke(new CallbackSetGroupString(AddressOf SetGroupSubTitle), lstvwgrp, SubTitle)
					 else
			{
				var GrpId = GetGroupID(lstvwgrp)
						 var group As new LVGROUP With
		{.CbSize = Marshal.SizeOf(group),
							.PszSubtitle = SubTitle,
							.Mask = LVGROUP.ListViewGroupMask.LVGF_SUBTITLE
		}
		Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessages.LVM_SETGROUPINFO, GrpId, group)
					 }
		}
			*/
		#endregion

		#region SetGroupTaskLink
		/*

		/// <summary>Обработку кликов надо делать в событии Listview.TaskLinkClick</summary>
		public static void SetGroupTaskLink(lstvwgrp As ListViewGroup, TaskLinkText As  string )
		 if  Not(uomvb.OS.VistaOrLater)
		{
		return  //Only Vista and forward allows footer on ListViewGroups

		 if (lstvwgrp == null) || (lstvwgrp.ListView == null)  {
		return
		 if (lstvwgrp.ListView.InvokeRequired)
		{
		Call lstvwgrp.ListView.Invoke(new CallbackSetGroupString(AddressOf SetGroupTaskLink), lstvwgrp, TaskLinkText)
		 else
		{
			var GrpId = GetGroupID(lstvwgrp)
			 var group As  new LVGROUP With
		{.CbSize = Marshal.SizeOf(group),
										   .PszTask = TaskLinkText,
										   .Mask = LVGROUP.ListViewGroupMask.LVGF_TASK
		}

			Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessages.LVM_SETGROUPINFO, GrpId, group)
		 }
		}
		*/
		#endregion

		#region SetGroupAlign

		/*

		public static void SetGroupAlign(lstvwgrp As ListViewGroup, A As LVGROUP.ListViewGroupAlign)

		if (lstvwgrp == null)
		{
		throw new ArgumentNullException("lstvwgrp")
		if (lstvwgrp.ListView == null)
		{
		  throw new ArgumentException("Group must ge Added to ListView before.", "lstvwgrp")

		if (lstvwgrp.ListView.InvokeRequired)
		  {
			  var Proc = new Action(Of ListViewGroup, LVGROUP.ListViewGroupAlign)(AddressOf SetGroupAlign)
		  Call lstvwgrp.ListView.Invoke(Proc, lstvwgrp, A)
		else
			  {
				  var GrpId = GetGroupID(lstvwgrp)
		   var group As new LVGROUP With
		{.CbSize = Marshal.SizeOf(group),
										 .Mask = LVGROUP.ListViewGroupMask.LVGF_ALIGN,
										 .UAlign = A
		}

		Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessages.LVM_SETGROUPINFO, GrpId, group)
		}
		  }
		*/

		#endregion

#endif


		#endregion







		/*



		To get this result you need to..:

Set OwnerDraw = true for the LV
Set UseItemStyleForSubItems = false for all Items
Code all three Drawxxx events.
Decide on how to store the references to the 2nd (etc) image, since the SubItem class doesn't have an ImageIndex.
You can either use the Tag of the SubItem to hold the ImageIndex number or, if you don't need the Text, you can set the text so you can use it as index or even as Key into the ImageList.

Two of the events are simple:

		 private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			e.DrawDefault = true;
		}








		   private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
    {
        e.DrawBackground();
        Size sz = listView1.SmallImageList.ImageSize;
        int idx = 0;
        if (e.SubItem.Tag != null) idx = (int)e.SubItem.Tag;
        Bitmap bmp = (Bitmap)listView1.SmallImageList.Images[idx];
        Rectangle rTgt = new Rectangle(e.Bounds.Location, sz);
        bool selected = e.ItemState.HasFlag(ListViewItemStates.Selected);
        // optionally show selection:
        if (selected ) e.Graphics.FillRectangle(Brushes.CornflowerBlue, e.Bounds);

        if (bmp != null) e.Graphics.DrawImage(bmp, rTgt);

        // optionally draw text
        e.Graphics.DrawString(e.SubItem.Text, listView1.Font,
                              selected  ? Brushes.White: Brushes.Black,
                              e.Bounds.X + sz.Width + 2, e.Bounds.Y + 2);
    }











		 */






	}


}


namespace uom.Extensions
{


	[DebuggerNonUserCode, DebuggerStepThrough]
	internal static partial class Extensions_Controls_Listview
	{


#if !NET


		#region Set group state


		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSetState(this ListViewGroup grp, ListViewEx.ListViewGroupState state = DEFAULT_GROUP_STATE)
			=> ListViewEx.SetGroupState(grp, state);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSetStateFlag(this ListViewGroup grp, ListViewGroupState flag, bool flagState = true)
			=> ListViewEx.SetGroupStateFlag(grp, flag, flagState);


		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSetState_Collapsed(this ListViewGroup grp, bool makeCollapsed)
		{
			bool needChangeState = false;
			ListViewEx.ListViewGroupState state = grp.eGetState();

			if (!state.HasFlag(ListViewGroupState.Collapsible))//check that also Collapsible flag already set
			{
				needChangeState = true;
				state |= ListViewGroupState.Collapsible;
			}

			if (state.HasFlag(ListViewGroupState.Collapsed) != makeCollapsed)
			{
				needChangeState = true;
				state ^= ListViewEx.ListViewGroupState.Collapsed;//revering Collapsed state
			}

			if (needChangeState) grp.eSetState(state);
		}


		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSetGroupsState(this ListView? lvw, ListViewEx.ListViewGroupState state = DEFAULT_GROUP_STATE)
			=> lvw?.eGroupsAsIEnumerable().ToList().ForEach(grp => grp.eSetState(state));


		#endregion



		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ListViewEx.ListViewGroupState eGetState(this ListViewGroup grp)
			=> ListViewEx.GetGroupState(grp);


		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool eGetState_IsCollapsed(this ListViewGroup grp)
			=> grp.eGetState().HasFlag(ListViewEx.ListViewGroupState.Collapsed);


		internal const ListViewEx.ListViewGroupState DEFAULT_GROUP_STATE = ListViewEx.ListViewGroupState.Collapsible;

#endif


		private const string ERROR_LIST_VIEW_GROUP_NAME_NULL = "ListViewGroup.Name = NULL!";
		private const string LISTVIEW_GROUPS_STATE_KEY_PREFIX = @"ListView Groups states\";

		[Obsolete("Do not save group states in to registry! Save to file instead!", true)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSaveGroupsCollapsedStates_Reg(this ListView lvw, string listViewID = "")
			=> lvw
			.eGroupsAsIEnumerable()
			.ToList()
			.ForEach(grp => grp.eSaveGroupCollapsedState_Reg(listViewID));


		[Obsolete("Do not save group states in to registry! Save to file instead!", true)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSaveGroupCollapsedState_Reg(this ListViewGroup grp, string listViewID = "")
		{
			if (string.IsNullOrWhiteSpace(grp.Name)) throw new ArgumentNullException(ERROR_LIST_VIEW_GROUP_NAME_NULL);

			if (string.IsNullOrWhiteSpace(listViewID))
			{
				if (grp.ListView == null) return; // Группе не назначен ListView - Просто игнорируем
				listViewID = grp.ListView.eCreateListViewID();
			}

			var sFullGroupIDRegKey = LISTVIEW_GROUPS_STATE_KEY_PREFIX + listViewID;
			//uomvb.Settings.SaveSetting(grp.Name, grp.GetState_IsCollapsed, null, sFullGroupIDRegKey);
		}





		[Obsolete("Do not save group states in to registry! Save to file instead!", true)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eLoadGroupCollapsedState_Reg(this ListViewGroup grp, string listViewID = "", bool SearchPreVersion = false)
		{
			if (string.IsNullOrWhiteSpace(grp.Name)) throw new ArgumentNullException(ERROR_LIST_VIEW_GROUP_NAME_NULL);

			if (string.IsNullOrWhiteSpace(listViewID))
			{
				if (grp.ListView == null) return; // Группе не назначен ListView - Просто игнорируем
				listViewID = grp.ListView.eCreateListViewID();
			}

			var sFullGroupIDRegKey = LISTVIEW_GROUPS_STATE_KEY_PREFIX + listViewID;
			/*
			var bCollapsed = uomvb.Settings.GetSetting_Boolean(grp.Name, false, null, SearchPreVersion, sFullGroupIDRegKey).Value;
			ListViewEx.ListViewGroupState State = DEFAULT_GROUP_STATE;
			if (bCollapsed) State |= ListViewEx.ListViewGroupState.Collapsed;
			grp.eSetState(State);
			 */
		}


		/// <summary>Generate ListView ID with Form Name</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string eCreateListViewID(this ListView lvw) => $"{lvw.FindForm()!.Name}.{lvw.Name}";

#if !NET

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSetGroupsTitlesBy_Count(
			this ListView lvw,
			Func<ListViewGroup, string>? callbackGroupTitleProvider = null,
			Action<ListViewGroup, string, ListViewEx.ListViewGroupState>? callbackGroupTitleApplier = null)
		{
			foreach (var grp in lvw.eGroupsAsIEnumerable())
			{
				{
					var bCollapsed = grp.eGetState_IsCollapsed();

					ListViewEx.ListViewGroupState state = DEFAULT_GROUP_STATE;
					if (bCollapsed) state |= ListViewEx.ListViewGroupState.Collapsed;

					var sTitle = (callbackGroupTitleProvider != null)
						? callbackGroupTitleProvider.Invoke(grp)
						: $"{grp.Name} ({grp.Items.Count})";

					if (callbackGroupTitleApplier != null)
						callbackGroupTitleApplier.Invoke(grp, sTitle, state);
					else
						grp.eSetGroup(Header: sTitle, state: state);
				}
			}
		}


		/// <summary>Safely sets group Header and don't broke groups collapswd states</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void eSetGroupsTitlesFastW32Safe(
			this ListView? lvw,
			Func<ListViewGroup, string>? getGroupHeader = null)
				=> lvw?.eGroupsAsIEnumerable().eForEach(g =>
				{
					string sTitle = g.Name ?? "";
					if (getGroupHeader != null)
						sTitle = getGroupHeader.Invoke(g);
					else
						sTitle = $"{sTitle} ({g.Items.Count:N0})".Trim();

					if (!string.IsNullOrWhiteSpace(sTitle))
					{
						g.eSetText(sTitle);
					}
				});



		///<summary>
		///Safely sets group Header and don't broke groups collapswd states
		///MT Safe!!!</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void erunOnLockedUpdateW32Safe(
			this ListView? lvw,
			Action a,
			bool autoSizeColumns = false,
			bool fastUpdateGroupHeaders = false)
		{
			_ = a ?? throw new ArgumentNullException(nameof(a));

			void a2()
			{
				lvw?.BeginUpdate();
				try { a!.Invoke(); }
				finally
				{
					if (autoSizeColumns) lvw?.eAutoSizeColumnsAuto();
					if (fastUpdateGroupHeaders) lvw?.eSetGroupsTitlesFastW32Safe();
					lvw?.EndUpdate();
				}
			};

			if (lvw != null && lvw.InvokeRequired)
				lvw.eRunInUIThread(a2);
			else
				a2();
		}


		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void AddToListViewAndSetState(this IEnumerable<ListViewGroup>? GG, ListView lvw, ListViewEx.ListViewGroupState state = DEFAULT_GROUP_STATE)
		{
			if (GG == null || lvw == null) return;
			foreach (var G in GG)
			{
				lvw.Groups.Add(G);
				G.eSetState(state);
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSetGroup(
			this ListViewGroup lvg,
			string? Header = null,
			string? SubTitle = null,
			string? Footer = null,
			string? TaskLinkText = null,
			ListViewGroupAlign align = 0,
			ListViewGroupState state = ListViewGroupState.Invalid)
			=> ListViewEx.SetGroup(lvg, Header, SubTitle, Footer, TaskLinkText, align, state);


		/// <summary>Задаёт текст для группы, но не меняет флаг состояния группы (!!! Стандартный .Header=String меняет флаг состояния!!!)</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSetText(this ListViewGroup lvg, string Text) => lvg.eSetGroup(Text);



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSetSubTitle(this ListViewGroup lvg, string subTitle) => lvg.eSetGroup(SubTitle: subTitle);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eSetFooter(this ListViewGroup lvg, string footerText) => lvg.eSetGroup(Footer: footerText);


		internal static Int32 eGetWin32ID(this ListViewGroup lstvwgrp)
		{
			_ = lstvwgrp!.ListView ?? throw new ArgumentException("Group must ge Added to ListView before!", nameof(lstvwgrp));

			var groupID = lstvwgrp.eGetPropertyValue_Int32("ID");
			if (!groupID.HasValue) groupID = lstvwgrp.ListView.Groups.IndexOf(lstvwgrp);
			return groupID.Value;
		}

#endif


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string eGetStringID(this ListViewGroup g, string defaultID = "default")
		{
			string?[] fields = [g.Name, g.Header];
			return fields.FirstOrDefault(s => s.eIsNotNullOrWhiteSpace()) ?? defaultID;
		}





#if NETCOREAPP3_0_OR_GREATER

#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static (ListViewGroup Group, bool Created) eGroupsCreateGroupByKey(
			this ListViewEx lvw,
			string key,
			string? header = null,
			ListViewGroupState newGroupState = ListViewGroupState.Collapsible)
			=> lvw.eGroupsCreateGroupByKey(key, header, new Action<ListViewGroup>(grp => grp.eSetStateFlag(newGroupState)));


#endif






		#region Allow SubItemImages

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct LV_ITEM
		{
			public MaskFlags mask;
			public Int32 iItem;
			public Int32 iSubItem;
			public UInt32 state;
			public UInt32 stateMask;
			public String pszText;
			public Int32 cchTextMax;
			public Int32 iImage;
			public IntPtr lParam;

			public enum MaskFlags : UInt32
			{
				LVIF_TEXT = 0x0001,
				LVIF_IMAGE = 0x0002
			}

			public void Apply(IntPtr h)
			{
				[DllImport("user32.dll")]
				static extern bool SendMessage(IntPtr hWnd, ListViewMessages msg, Int32 wParam, ref LV_ITEM lParam);

				SendMessage(h, ListViewMessages.LVM_SETITEM, 0, ref this);
			}
		}


		/*
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			AllowSubItemImages();
		}
		 */


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr SendMessage_IntPtr(this ListView lvw, ListViewMessages m, IntPtr? wParam = default, IntPtr? lParam = default)
			=> uom.WinAPI.windows.SendMessage(lvw.Handle, (int)m, wParam.HasValue ? wParam.Value : IntPtr.Zero, lParam.HasValue ? lParam.Value : IntPtr.Zero);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SendMessage_Int(this ListView lvw, ListViewMessages m, int? wParam = 0, int? lParam = 0)
			=> uom.WinAPI.windows.SendMessage(lvw.Handle, (int)m, wParam.HasValue ? wParam.Value : 0, lParam.HasValue ? lParam.Value : 0);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ExStyles GetExtendedListViewStyle(this ListView lvw) => (ExStyles)lvw.SendMessage_Int(ListViewMessages.LVM_GETEXTENDEDLISTVIEWSTYLE);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetExtendedListViewStyle(this ListView lvw, ExStyles s) => lvw.SendMessage_Int(ListViewMessages.LVM_SETEXTENDEDLISTVIEWSTYLE, 0, (int)s);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetExtendedListViewStyleFlag(this ListView lvw, ExStyles s) => lvw.SetExtendedListViewStyle(lvw.GetExtendedListViewStyle() | s);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AllowSubItemImages(this ListView lvw) => lvw.SetExtendedListViewStyleFlag(ExStyles.LVS_EX_SUBITEMIMAGES);

		/*
		public void SetSubItem(ListView lvw, int row, int col, string text, int iconIndex)
		{
			LV_ITEM lvi = new()
			{
				iItem = row,
				iSubItem = col,
				pszText = text,
				mask = LV_ITEM.MaskFlags.LVIF_IMAGE | LV_ITEM.MaskFlags.LVIF_TEXT,
				iImage = iconIndex
			};
			lvi.Apply(this.Handle);
			//SendMessage(this.Handle, ListViewMessages.LVM_SETITEM, 0, ref lvi);
		}

		public static void SetSubItemImage(this ListView lvw, int row, int col, int iconIndex)
		{
			LV_ITEM lvi = new()
			{
				iItem = row,
				iSubItem = col,
				mask = LV_ITEM.MaskFlags.LVIF_IMAGE,
				iImage = iconIndex
			};
			lvi.Apply(lvw.Handle);
		}
		 */

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool SetSubItemImage(this ListViewItem li, int col, int iconIndex)
		{
			//ListViewItem.ListViewSubItem? lsi = li.SubItems[col];
			if (li.ListView == null) return false;// throw new ArgumentNullException(nameof(li), "ListViewItem.ListView = NULL!");
			LV_ITEM lvi = new()
			{
				iItem = li.Index,
				iSubItem = col,
				mask = LV_ITEM.MaskFlags.LVIF_IMAGE,
				iImage = iconIndex
			};
			lvi.Apply(li.ListView.Handle);
			return true;
		}

		#endregion









	}




}
