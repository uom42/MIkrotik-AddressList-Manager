#nullable enable


global using System.Drawing;
global using System.Windows.Forms;

global using uom.controls;

using System.Diagnostics.Eventing.Reader;

using uom.WinAPI;

using Vanara.PInvoke;

using static uom.controls.ListViewEx;
using static Vanara.PInvoke.ComCtl32;
using static Vanara.PInvoke.User32;


namespace uom.controls
{


    /// <summary>
    /// ReflectNotifyManager обеспечивает надёжную "рефлектацию" WM_NOTIFY из родителя в дочерние контролы.
    /// Когда родитель получает WM_NOTIFY от зарегистрованного child hwnd, менеджер форвардит OCM_NOTIFY (WM_NOTIFY + 0x2000) на hwnd child.
    /// </summary>
    /// <remarks>Created by ChatGPT 2025 12 03</remarks>
    internal sealed class ReflectNotifyManager : NativeWindow, IDisposable
    {

        private class ControlNotifySettings ( Control control , int [] notifyMesageCodes )
        {
            public readonly Control Control = control;
            public readonly int [] NotifyMesageCodes = notifyMesageCodes;
        }


        // used for store created instances of ReflectNotifyManager
        private static readonly Dictionary<Control,ReflectNotifyManager> _instancesForParents = [];

        // thread-safe map hwnd -> weak reference to Control
        private readonly System.Collections.Concurrent.ConcurrentDictionary<IntPtr, ControlNotifySettings> _registeredControls = new ();


        private readonly IntPtr _parentHandle;
        private bool _assigned;


        private ReflectNotifyManager ( IntPtr parentControlHandle )
        {
            AssignHandle( parentControlHandle );
            _parentHandle = parentControlHandle;
            _assigned = true;
        }


        [MethodImpl( MethodImplOptions.Synchronized )]
        public static void CreateForControl ( Control control , int [] notifyMesageCodes )
        {
            control.HandleCreated += ( _ , _ ) => Rattach_( control , notifyMesageCodes );
            control.ParentChanged += ( _ , _ ) => Rattach_( control , notifyMesageCodes );
            control.Disposed += ( _ , _ ) =>
            {
                try
                {
                    var prnt = control.Parent;
                    if ( prnt != null && _instancesForParents.TryGetValue( prnt , out var rnm ) )
                        rnm?.UnregisterControl( control );
                }
                catch { }
            };
        }


        private static  Control? _alreadyAttachingCtl = null;

        [MethodImpl( MethodImplOptions.Synchronized )]
        private static void Rattach_ ( Control control , int [] notifyMesageCodes )
        {
            var prnt = control.Parent;
            if ( prnt == null || _alreadyAttachingCtl == control ) return; // Ensure parent exists and manager is attached

            // Create a manager attached to parent handle and register this control
            // If parent already has a manager, reuse it via a ParentTag

            _alreadyAttachingCtl = control;
            try
            {
                if ( !_instancesForParents.TryGetValue( prnt , out var rnm ) )
                {
                    rnm = new( prnt.Handle );
                    _instancesForParents.Add( prnt , rnm );
                }
                rnm.RegisterControl( control , notifyMesageCodes );
            }
            finally { _alreadyAttachingCtl = null; }
        }



        [MethodImpl( MethodImplOptions.NoOptimization )] // Disable compiler optimization
        private void RegisterControl ( Control control , int [] notifyMesageCodes )
        {
            if ( control == null ) throw new ArgumentNullException( nameof( control ) );

            // ensure handle exists
            if ( !control.IsHandleCreated ) _ = control.Handle; // forces handle creation
            _registeredControls [control.Handle] = new ControlNotifySettings( control , notifyMesageCodes );
        }

        private void UnregisterControl ( Control control )
        {
            if ( control == null ) return;
            _registeredControls.TryRemove( control.Handle , out _ );
        }




        protected override void WndProc ( ref Message m )
        {
            if ( m.Msg == ( int )User32.WindowMessage.WM_NOTIFY )
            {
                try
                {
                    User32.NMHDR hdr = Marshal.PtrToStructure<User32.NMHDR> (m.LParam);
                    var nmhdr = (NMHDR) m.GetLParam (typeof (NMHDR))!;
                    var ocmNotifyCode = nmhdr.code;

                    if ( hdr.hwndFrom != IntPtr.Zero && _registeredControls.TryGetValue( ( IntPtr )hdr.hwndFrom , out var cns ) )
                    {
                        if ( !cns.Control.IsDisposed )
                        {

                            //Use notify message subscribtion to avoid double events fired in control when conflicts with NetCore controls message marshaling
                            if ( cns.NotifyMesageCodes.Contains( ocmNotifyCode ) )
                            {
                                //Control subscribed for this message


                                // Forward OCM_NOTIFY to the child control's window procedure.
                                // We use SendMessage to deliver the reflected notification directly.
                                User32.SendMessage( hdr.hwndFrom , User32.OCM_NOTIFY , m.WParam , m.LParam );
                                // We do NOT mark the parent's WndProc as handled here; we allow default processing.
                                // But if desired, you can set m.Result here. For compatibility we leave it intact.
                            }
                        }
                        else
                        {
                            // Remove dead entry
                            _registeredControls.TryRemove( ( IntPtr )hdr.hwndFrom , out _ );
                        }
                    }
                }
                catch
                {
                    // Swallow exceptions so message loop remains stable
                }
            }

            base.WndProc( ref m );
        }


        [MethodImpl( MethodImplOptions.Synchronized )]
        public void Dispose ()
        {
            if ( _assigned )
            {
                ReleaseHandle();
                _assigned = false;
            }
            _registeredControls.Clear();

            var key =_instancesForParents.FirstOrDefault(kvp=>kvp.Value==this).Key;
            if ( key != null ) _instancesForParents.Remove( key );
        }
    }



    public partial class ListViewEx : ListView
    {
        #region WinAPI

        //private const int WM_NOTIFY = 0x004E;

        //private const int LVM_FIRST = 0x1000;
        //private const int LVM_RESETEMPTYTEXT = LVM_FIRST + 84;  // = 0x1054)

        private const int LVN_FIRST = -100;
        private const int LVN_GETEMPTYMARKUP = LVN_FIRST - 87; // = -187


        //private const int L_MAX_URL_LENGTH = 2048 + 32 + 4; 
        // #define L_MAX_URL_LENGTH    (2048 + 32 + sizeof("://"))                                                            
        //private const int L_MAX_URL_LENGTH = (2048 + 32 + sizeof("://"));

        #endregion


        #region Constructors


        /// <summary>Initializes a new instance of the <see cref="ListView"/> class.</summary>
        public ListViewEx () : base()
        {
            SetStyle(
                ControlStyles.ResizeRedraw
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                , true );

            AddKeyboardAndMousehandlers();
            _dragDrop_InsertionIndex = -1;

#if NET5_0_OR_GREATER
            ReflectNotifyManager.CreateForControl( this , [LVN_GETEMPTYMARKUP] );
#endif
        }


        #endregion


        public class QueryEmptyTextEventArgs : System.EventArgs
        {
            public string EmptyListViewText = string.Empty;
            public bool CenterText = false;
        }

        public event EventHandler<QueryEmptyTextEventArgs> QueryEmptyText = delegate { };

#if !NET5_0_OR_GREATER
		public event EventHandler<string> GroupCollapsedStateChanged = delegate { };

#endif

        #region Items Editing and Clipboard copy/pasting


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


        private void AddKeyboardAndMousehandlers ()
        {
            MouseDoubleClick += On_MouseDoubleClick!;
            KeyDown += On_KeyDown!;
            KeyPress += On_KeyPress!;
            KeyUp += On_KeyUp!;
        }


        private void On_MouseDoubleClick ( object sender , MouseEventArgs e )
        {
            if ( e.Button != MouseButtons.Left )
            {
                return;
            }

            if ( CheckBoxes )
            {
                return;
            }

            var POS = e.Location;
            var LI = GetItemAt (POS.X, POS.Y);
            if ( null == LI )
            {
                return;
            }

            On_Items_NeedEdit( LI.toArrayFromSingleElement() );
        }

        private void On_KeyDown ( object sender , KeyEventArgs e )
        {
            switch ( e.KeyCode )
            {
                case Keys.A when e.Modifiers == Keys.Control && MultiSelect:
                    {
                        e.SuppressKeyPress = true;
                        this.selectAllItems();
                        break;
                    }

                case Keys.Insert when e.Modifiers == Keys.None:
                    {
                        e.SuppressKeyPress = On_Items_NeedInsert();
                        break;
                    }

                case Keys.Insert when e.Modifiers == Keys.Control:
                case Keys.C when e.Modifiers == Keys.Control:
                    {
                        e.SuppressKeyPress = On_Clipboard_Copy();
                        break;
                    }

                case Keys.Insert when e.Modifiers == Keys.Shift:
                case Keys.V when e.Modifiers == Keys.Control:
                    {
                        e.SuppressKeyPress = On_Clipboard_Paste();
                        break;
                    }

                default:
                    break;
            }
        }
        private void On_KeyPress ( object sender , KeyPressEventArgs e ) //,Handles this.KeyPress
        {
            if ( e.KeyChar != 0xD ) return;
            e.Handled = On_Items_NeedEdit();
        }
        private void On_KeyUp ( object sender , KeyEventArgs e )
        {

            switch ( e.KeyCode )
            {
                case Keys.Delete when e.Modifiers == Keys.None:
                    {
                        e.SuppressKeyPress = On_Items_NeedDelete();
                        break;
                    }

                case Keys.F5:
                    {
                        e.SuppressKeyPress = On_Items_NeedRefreshList();
                        break;
                    }


                default:
                    break;
            }
        }


        #endregion


        #region On_Items_Needxxxx


        protected virtual bool On_Items_NeedRefreshList ()
        {
            Items_NeedRefreshList?.Invoke( this , EventArgs.Empty );
            return true;
        }

        protected virtual bool On_Items_NeedEdit ( ListViewItem []? aSel = null )
        {
            if ( null == aSel || aSel.Length == 0 ) aSel = [.. this.selectedItemsAsIEnumerable()];
            if ( aSel.Length == 0 ) return false;
            if ( !MultiSelect ) aSel = aSel.First().toArrayFromSingleElement();
            Items_NeedEdit?.Invoke( this , aSel );
            return true;
        }

        protected virtual bool On_Items_NeedDelete ()
        {
            var aSel = this.selectedItemsAsIEnumerable ().ToArray ();
            if ( aSel.Length == 0 ) return false;
            Items_NeedDelete?.Invoke( this , aSel );
            return true;
        }

        private bool On_Items_NeedInsert ()
        {
            Items_NeedInsert?.Invoke( this , EventArgs.Empty );
            return true;
        }

        private bool On_Clipboard_Copy ()
        {
            var aSel = this.selectedItemsAsIEnumerable ().ToArray ();
            if ( aSel.Length == 0 ) return false;
            ClipboardCopy?.Invoke( this , aSel );
            return true;
        }

        private bool On_Clipboard_Paste ()
        {
            ClipboardPaste?.Invoke( this , EventArgs.Empty );
            return true;
        }


        #endregion


        #endregion


        #region Avoid flickering

        /*
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			//,e.Graphics.DrawString("FUCK!", Font, Brushes.Red, ClientRectangle);
		}
		 */
        protected override void OnPaintBackground ( PaintEventArgs pevent )
        {
            //,base.OnPaintBackground(pevent);
        }


        #endregion


        #region Item drag visual support
        // Dragging items in a ListView control with visual insertion guides
        // http://www.cyotek.com/blog/dragging-items-in-a-listview-control-with-visual-insertion-guides


        public abstract class DragDrop_ExternalFilesBaseEventArgs ( FileInfo [] files ) : EventArgs()
        {
            public FileInfo [] Files { get; protected set; } = files;
        }


        public sealed class DragDrop_DragEnterExternalFilesEventArgs ( FileInfo [] files ) : DragDrop_ExternalFilesBaseEventArgs( files )
        {
            public bool Cancel = false;

            public void SetFiles ( FileInfo [] files )
            {
                Files = files;
            }
        }

        public sealed class DragDrop_DropExternalFilesEventArgs ( FileInfo [] files , int insertionIndex ) : DragDrop_ExternalFilesBaseEventArgs( files )
        {
            public readonly int InsertionIndex = insertionIndex;
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

            public DragDrop_ItemReorderEventArgs ( ListViewItem sourceItem , ListViewItem dropItem , int insertionIndex , DragDropInsertionModes insertionMode , Point mouse ) : base( sourceItem )
            {
                Item = sourceItem;
                DropItem = dropItem;
                Mouse = mouse;
                InsertionIndex = insertionIndex;
                InsertionMode = insertionMode;
            }
        }


        public class CancelListViewItemDragEventArgs ( ListViewItem item ) : CancelEventArgs()
        {
            //protected CancelListViewItemDragEventArgs() : base() { }
            public ListViewItem Item { get; protected set; } = item;
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

        [Category (C_CATEGORY_PROPERTY_CHANGED)]
        public event EventHandler DragDropModeChanged = delegate { };

        /// <summary>Occurs when the InsertionLineColor property value changes.</summary>
        [Category (C_CATEGORY_PROPERTY_CHANGED)]
        public event EventHandler InsertionLineColorChanged = delegate { };



        /// <summary>Occurs when a drag-and-drop operation for an item is completed.</summary>
        [Category (C_CATEGORY_DRAG_DROP)]
        public event EventHandler<DragDrop_ItemReorderEventArgs> DragDrop_ItemReorder = delegate { };

        /// <summary>Occurs when the user begins dragging an item.</summary>
        [Category (C_CATEGORY_DRAG_DROP)]
        public event EventHandler<CancelListViewItemDragEventArgs> ItemDragStart = delegate { };


        [Category (C_CATEGORY_DRAG_DROP)]
        public event EventHandler<DragDrop_DragEnterExternalFilesEventArgs> DragDrop_DragEnterExternalFiles = delegate { };

        [Category (C_CATEGORY_DRAG_DROP)]
        public event EventHandler<DragDrop_DropExternalFilesEventArgs> DragDrop_DropExternalFiles = delegate { };

        #endregion



        private int _dragDrop_InsertionIndex;
        private bool _dragDrop_ItemDragInProgress;
        private DragDropInsertionModes _dragDrop_InsertionMode;
        private DragDropModes _dragDrop_Mode = DragDropModes.None;
        private Color _dragDrop_InsertionLineColor;
        private FileInfo[] _dragDrop_FilesFromExternalSource = { };

        private bool _dragDrop_DropFilesFromExternalSourceInProgress => _dragDrop_FilesFromExternalSource.Length != 0;


        #region public Properties


        /// <summary> Hiding parent AllowDrop property</summary>
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        protected bool AllowDrop => base.AllowDrop;
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword


        private bool DragDrop_AllowItemDrag => _dragDrop_Mode.HasFlag( DragDropModes.ItemsReorder ) || _dragDrop_Mode.HasFlag( DragDropModes.DragToExternal );


        [Category( C_CATEGORY_DRAG_DROP )]
        [DefaultValue( DragDropModes.None )]
        [RefreshProperties( RefreshProperties.All )]
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

                    if ( _dragDrop_Mode == DragDropModes.None )
                    {
                        //itemDrag = false;
                        //itemDrag = false;
                        return;
                    }

                    if ( _dragDrop_Mode.HasFlag( DragDropModes.ItemsReorder ) )
                    {
                        //itemDrag = true;
                        allowDrop = true;
                        return;
                    }

                    if ( _dragDrop_Mode.HasFlag( DragDropModes.DropFromExternal ) )
                    {
                        allowDrop = true;
                    }
                    if ( _dragDrop_Mode.HasFlag( DragDropModes.DragToExternal ) )
                    {
                        //itemDrag = true;
                    }
                }
                finally
                {
                    //AllowItemDrag = itemDrag;
                    base.AllowDrop = allowDrop;

                    OnDragDropModeChanged( EventArgs.Empty );
                }
            }
        }



        /// <summary>Gets or sets the color of the insertion line drawn when dragging items within the control.</summary>
        [Category( C_CATEGORY_DRAG_DROP )]
        [DefaultValue( typeof( Color ) , "Red" )]
        [RefreshProperties( RefreshProperties.All )]
        public Color DragDrop_InsertionLineColor
        {
            get => _dragDrop_InsertionLineColor;
            set => _dragDrop_InsertionLineColor.updateIfNotEquals( value , () => OnInsertionLineColorChanged( EventArgs.Empty ) );
        }


        #endregion



        #region private Members

        private void DrawInsertionLine ()
        {
            const int InsertionMarkWidth = 3;
            const int InsertionMarkArrowSize = 8;

            if ( _dragDrop_InsertionIndex < 0 || _dragDrop_InsertionIndex >= Items.Count )
            {
                return;
            }

            Rectangle rcItemToDrop = Items[ _dragDrop_InsertionIndex ].GetBounds (ItemBoundsPortion.Entire);
            using Graphics g = CreateGraphics ();

            Color clr = SystemColors.HotTrack;
            using Pen pnArrowEdge = new (clr, InsertionMarkWidth);
            using Brush brArrowFill = new SolidBrush (clr);

            Point[] ptsStartArrow;
            Point[] ptsEndArrow;
            int ArrowSize2 =  InsertionMarkArrowSize / 2 ;

            switch ( View )
            {
                case View.Details:  //Vertical grid
                    {
                        int x1 = 0; // aways fit the line to the client area, regardless of how the user is scrolling
                        int y = _dragDrop_InsertionMode == DragDropInsertionModes.Before
                            ? rcItemToDrop.Top
                            : rcItemToDrop.Bottom;

                        // again, make sure the full width fits in the client area
                        int width = Math.Min (rcItemToDrop.Width - rcItemToDrop.Left, ClientSize.Width);


                        int x2 = x1 + width;
                        ptsStartArrow = new []
                        {
                            new Point(x1, y - ArrowSize2),
                            new Point(x1 + InsertionMarkArrowSize, y),
                            new Point(x1, y + ArrowSize2)
                        };

                        ptsEndArrow = new []
                        {
                            new Point(x2, y - ArrowSize2),
                            new Point(x2 - InsertionMarkArrowSize, y),
                            new Point(x2, y + ArrowSize2)
                        };

                    }
                    break;

                default:
                    {

                        int x = _dragDrop_InsertionMode == DragDropInsertionModes.Before
                            ? rcItemToDrop.Left
                            : rcItemToDrop.Right;

                        int y1 = rcItemToDrop.Top;
                        int y2 = rcItemToDrop.Bottom;

                        ptsStartArrow = new []
                        {
                            new Point(x-ArrowSize2, y1),
                            new Point(x , y1+InsertionMarkArrowSize),
                            new Point(x+ArrowSize2, y1)
                        };
                        ptsEndArrow = new []
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

            g.FillPolygon( brArrowFill , ptsStartArrow );
            g.FillPolygon( brArrowFill , ptsEndArrow );
            g.DrawLine( pnArrowEdge , ptsStartArrow [1] , ptsEndArrow [1] );

        }

        #endregion


        #region OnDrag...


        protected override void OnItemDrag ( ItemDragEventArgs e )
        {
            if ( DragDrop_AllowItemDrag && Items.Count > 1 && e.Item != null )
            {
                CancelListViewItemDragEventArgs args = new ((ListViewItem) e.Item);
                OnItemDragStart( args );

                if ( !args.Cancel )
                {
                    _dragDrop_ItemDragInProgress = true;
                    SelectedListViewItemCollection sel = SelectedItems;
                    if ( sel.Count > 0 )
                    {
                        DragDropEffects eff = DragDropEffects.None;

                        if ( DragDropMode.HasFlag( DragDropModes.ItemsReorder ) )
                        {
                            eff = DragDropEffects.Move;
                        }
                        if ( DragDropMode.HasFlag( DragDropModes.DragToExternal ) )
                        {
                            eff = DragDropEffects.Move | DragDropEffects.Copy;
                        }

                        DoDragDrop( sel , eff );
                        //this.DoDragDrop(sel, eff DragDropEffects.Move);
                    }
                    else
                    {
                        DoDragDrop( e.Item , DragDropEffects.Move );
                    }
                }
            }

            base.OnItemDrag( e );
        }


        protected override void OnDragEnter ( DragEventArgs e )
        {
            base.OnDragEnter( e );
            e.Effect = DragDropEffects.None;

            if ( !DragDropMode.HasFlag( DragDropModes.DropFromExternal ) )
                return;

            if ( e.Data is not DataObject shellData )
                return;

            if ( !shellData.ContainsFileDropList() )
                return;

            FileInfo[] files = shellData.GetFileDropList ()
                .Cast<string> ()
                .Select (s => new FileInfo (s))
                .ToArray ();

            DragDrop_DragEnterExternalFilesEventArgs ea = new (files);
            DragDrop_DragEnterExternalFiles?.Invoke( this , ea );
            if ( ea.Cancel || ea.Files.Length == 0 )
            {
                return;
            }

            _dragDrop_FilesFromExternalSource = ea.Files;
            e.Effect = DragDropEffects.Copy;
        }


        protected override void OnDragOver ( DragEventArgs e )
        {

            if ( _dragDrop_ItemDragInProgress || _dragDrop_DropFilesFromExternalSourceInProgress )
            {

                int newInsertionIndex;
                DragDropInsertionModes newInsMode;

                Point ptCursor = PointToClient (new Point (e.X, e.Y));
                ListViewItem? dropToItem = null;
                if ( DragDropMode.HasFlag( DragDropModes.ItemsReorder ) )//Allow draw insertion mark only when DragDropModes.ItemsReorder is set!
                {
                    var nearestToCursor = this.getNearestItem (ptCursor);
                    dropToItem = nearestToCursor.Item;
                }

                if ( dropToItem != null )
                {
                    newInsertionIndex = dropToItem.Index;

                    Rectangle dropToItemBounds = dropToItem.GetBounds (ItemBoundsPortion.Entire);
                    var ptDropToItemCenter = dropToItemBounds.eGetCenter ().eRoundToInt ();

                    switch ( View )
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

                if ( newInsertionIndex != _dragDrop_InsertionIndex || newInsMode != _dragDrop_InsertionMode )
                {
                    _dragDrop_InsertionMode = newInsMode;
                    _dragDrop_InsertionIndex = newInsertionIndex;
                    Invalidate();
                }
            }

            base.OnDragOver( e );
        }


        protected override void OnDragLeave ( EventArgs e )
        {
            _dragDrop_FilesFromExternalSource = Array.Empty<FileInfo>();
            _dragDrop_InsertionIndex = -1;

            Invalidate();

            base.OnDragLeave( e );
        }


        protected override void OnDragDrop ( DragEventArgs e )
        {
            try
            {
                if ( _dragDrop_ItemDragInProgress )
                {
                    OnDragDrop_ItemReorder( e );
                }
                else if ( _dragDrop_DropFilesFromExternalSourceInProgress )
                {
                    OnDragDrop_DropExternalFiles( e );
                }
            }
            finally { Invalidate(); }

            base.OnDragDrop( e );
        }


        protected void OnDragDrop_DropExternalFiles ( DragEventArgs e )
        {
            if ( _dragDrop_FilesFromExternalSource.Length == 0 )
            {
                return;
            }

            try
            {
                BeginUpdate();
                {

                    int dropIndex = _dragDrop_InsertionIndex;
                    if ( _dragDrop_InsertionMode == DragDropInsertionModes.After )
                    {
                        dropIndex++;
                    }

                    dropIndex = (dropIndex >= Items.Count)
                        ? dropIndex = -1
                        : dropIndex;

                    DragDrop_DropExternalFilesEventArgs ea = new (_dragDrop_FilesFromExternalSource, dropIndex);
                    DragDrop_DropExternalFiles?.Invoke( this , ea );
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


        protected void OnDragDrop_ItemReorder ( DragEventArgs e )
        {
            if ( !_dragDrop_ItemDragInProgress )
            {
                return;
            }

            try
            {

                ListViewItem? dropToItem = ( _dragDrop_InsertionIndex != -1 )
                    ? Items[ _dragDrop_InsertionIndex ]
                    : null;

                if ( dropToItem != null )
                {
                    object? sel = e.Data?.GetData (typeof (SelectedListViewItemCollection));
                    if ( sel != null )
                    {
                        var dragItems = ( (SelectedListViewItemCollection) sel )
                            .Cast<ListViewItem> ()
                            .ToArray ();

                        if ( dragItems.Length == 0 )
                        {
                            return;
                        }

                        try
                        {
                            BeginUpdate();
                            {
                                foreach ( var dragItem in dragItems )
                                {
                                    int dropIndex = dropToItem.Index;

                                    if ( dragItem.Index < dropIndex )
                                    {
                                        dropIndex--;
                                    }

                                    if ( _dragDrop_InsertionMode == DragDropInsertionModes.After && dragItem.Index < Items.Count - 1 )
                                    {
                                        dropIndex++;
                                    }

                                    if ( dropIndex == dragItem.Index )
                                    {
                                        return;//Drop on itself
                                    }

                                    Point clientPoint = PointToClient (new Point (e.X, e.Y));
                                    DragDrop_ItemReorderEventArgs args = new (
                                        dragItem,
                                        dropToItem,
                                        dropIndex,
                                        _dragDrop_InsertionMode,
                                        clientPoint);

                                    OnDragDrop_ItemReorder( args );
                                    if ( args.Cancel )
                                    {
                                        continue;
                                    }

                                    Items.Remove( dragItem );
                                    Items.Insert( dropIndex , dragItem );
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
                _dragDrop_InsertionIndex = -1;
                _dragDrop_ItemDragInProgress = false;
            }
        }




        protected virtual void OnDragDropModeChanged ( EventArgs e ) => DragDropModeChanged?.Invoke( this , e );


        protected virtual void OnInsertionLineColorChanged ( EventArgs e ) => InsertionLineColorChanged?.Invoke( this , e );


        protected virtual void OnDragDrop_ItemReorder ( DragDrop_ItemReorderEventArgs e ) => DragDrop_ItemReorder?.Invoke( this , e );


        protected virtual void OnItemDragStart ( CancelListViewItemDragEventArgs e ) => ItemDragStart?.Invoke( this , e );


        protected virtual void OnWmPaint ( ref Message m ) => DrawInsertionLine();





        #endregion


        #endregion


        #region EmptyText


        [DefaultValue( "" )]
        public string EmptyText
        {
            get => field;
            set
            {
                field = value;
                ResetEmptyText();
                Invalidate();
            }
        } = string.Empty;


        [DefaultValue( false )]
        public bool EmptyTextDisplayInCenter
        {
            get => field;
            set
            {
                field = value;
                ResetEmptyText();
                Invalidate();
            }
        } = false;






        [MethodImpl( MethodImplOptions.NoOptimization )] // Disable compiler optimization
        private void ResetEmptyText ()
        {
            // ensure handle exists
            if ( !IsHandleCreated ) _ = Handle; // forces handle creation
            if ( !IsHandleCreated ) return;

            User32.SendMessage<ComCtl32.ListViewMessage>( Handle , ComCtl32.ListViewMessage.LVM_RESETEMPTYTEXT );
        }


        #endregion


        /* 
		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data. </param>
		protected override void OnPaint(PaintEventArgs e) => base.OnPaint(e);
        [DebuggerStepThrough]
		 */

        protected override void WndProc ( ref Message m )
        {
            base.WndProc( ref m );

            var msg = (WindowMessage) m.Msg;
            switch ( msg )
            {
                case ( WindowMessage )User32.OCM_NOTIFY:
                    {
                        var nmhdr = (NMHDR) m.GetLParam (typeof (NMHDR))!;
                        var ocmNotifyCode = (ListViewNotification) nmhdr.code;
                        switch ( ocmNotifyCode )
                        {
                            case ( ListViewNotification )LVN_GETEMPTYMARKUP: // ListViewNotification.LVN_GETEMPTYMARKUP:
                                //if ( Control.FromHandle( ( IntPtr )nmhdr.hwndFrom ) == this )
                                {
                                    //Debug.WriteLine( $"LVN_GETEMPTYMARKUP" );

                                    var markup = (NMLVEMPTYMARKUP) m.GetLParam (typeof (NMLVEMPTYMARKUP))!;
                                    markup.szMarkup = EmptyText;
                                    markup.dwFlags = EmptyTextDisplayInCenter
                                        ? EMF.EMF_CENTERED
                                        : 0;

                                    Marshal.StructureToPtr( markup , m.LParam , false );
                                    m.Result = new IntPtr( 1 );
                                    return;
                                }
                                //break;
                        }
                        //return;

                    }
                    break;

                case WindowMessage.WM_PAINT:
                    OnWmPaint( ref m );
                    break;

#if !NET
                case WindowMessage.WM_LBUTTONUP:
                    //This provides collapsing/expanding groups by clicking on the right triangle.
                    //Collapsing/expanding by doubleclicking on group header - occurs independently from this by itself.
					base.DefWndProc (ref m);
					UpdateGroupsStateInStorage ();
                    break;
#endif
            }
        }


        protected void WndProc222 ( ref Message m )
        {

            if ( m.Msg == ( int )WindowMessage.WM_NOTIFY )
            {
                NMHDR hdr = Marshal.PtrToStructure<NMHDR>(m.LParam);

                // Проверяем "отражённость": уведомление пришло от нашего собственного окна
                //if ( hdr.hwndFrom == Handle && hdr.code == ( int )ListViewNotification.LVN_GETEMPTYMARKUP )
                if ( hdr.code == LVN_GETEMPTYMARKUP )
                {
                    string text = EmptyText;// "Нет элементов для отображения 22222222222222";
                    if ( text.Length >= 256 ) text = text.Substring( 0 , 255 );


                    NMLVEMPTYMARKUP markup =                    Marshal.PtrToStructure<NMLVEMPTYMARKUP>(m.LParam);
                    //markup.szMarkup = text;
                    //markup.dwFlags = 0; // 0 = показывать текст (не FLAG_NO_TEXT)
                    markup.dwFlags = EmptyTextDisplayInCenter
                        ? EMF.EMF_CENTERED
                        : 0;

                    //Array.Copy( text.ToCharArray() , markup.szMarkup , text.Length );
                    // Записываем структуру обратно
                    Marshal.StructureToPtr( markup , m.LParam , false );

                    m.Result = 1; // сообщаем системе, что обработали
                    return;
                }
            }

            base.WndProc( ref m );
        }




        /*
protected override void OnNotifyMessage ( Message m )
{
base.OnNotifyMessage( m );
var mm = (WindowMessage)m.Msg;
switch ( mm )
{
case WindowMessage.WM_NOTIFY:
{
    var nmhdr = m.GetLParamOf<NMHDR> ();
    //if ( nmhdr != null )
    {

        Debug.WriteLine( $"OnNotifyMessage: WM_NOTIFY {nmhdr}" );
                    var ocmNotifyCode = (ListViewNotification) nmhdr.code;
                    Debug.WriteLine( ocmNotifyCode );
    }

}
break;

default:
break;
}

}
         */



        #region Group Tools


        private const string DEFAULT_LIST_VIEW_GROUP_NAME = "default";


        private Dictionary<string, bool> _knownGroupsStates = [];





        private Dictionary<string , bool> GetGroupsState ()
            => this
                .groupsAsIEnumerable()
                .Select( grp =>
                {
                    string id = grp.eGetStringID (DEFAULT_LIST_VIEW_GROUP_NAME).ToLower ().Trim ();
                    bool collapsed = false;
#if NET
                    collapsed = grp.CollapsedState == ListViewGroupCollapsedState.Collapsed;
#else
					collapsed = grp.eGetState_IsCollapsed ();
#endif
                    return (ID: id, Collapsed: collapsed);
                }
                )
                .Where( grp => grp.ID.isNotNullOrWhiteSpace )
                .OrderBy( grp => grp.ID )
                .ToDictionary( grp => grp.ID , grp => grp.Collapsed );



        private FileInfo GetGroupsStateStorageFile ( string? @directory = null , string dataID = "" )
        {
            const string GROUPS_SATES_EXT = ".groups.xml";
            string lvID = this.eCreateListViewID ();
            if ( !dataID.isNullOrWhiteSpace ) lvID += $"_{dataID}";

            DirectoryInfo di = ( @directory != null )
                ? new (@directory)
                : uom.AppInfo.UserAppDataPath (true);

            return System.IO.Path.Combine( di.FullName , lvID + GROUPS_SATES_EXT ).eToFileInfo()!;
        }


        /// <summary>Write all groups collapsed/epanded state fo file</summary>
        /// <param name="directory">Folder where settings file will be placed</param>
        /// <param name="dataID">Some user custom file suffix</param>
        /// <param name="append">Do not replace all file data, but update/append groups. When append mode is not used only current groups existing in the ListView will be saved</param>
        /// <returns>Saved settings file</returns>

        public FileInfo SaveGroupsState ( string? @directory = null , string dataID = "" , bool append = true )
        {
            Dictionary<string, bool>? states = null;
            if ( append )
            {
                states = LoadGroupsState( @directory , dataID ) ?? [];
                var currentStates = GetGroupsState ();
                foreach ( var kvp in currentStates )
                {
                    if ( !states.TryAdd( kvp.Key , kvp.Value ) ) // Already exist just modify
                        states [kvp.Key] = kvp.Value;
                }
            }
            else // Only save existing listview groups.
            {
                states = GetGroupsState();
            }

            _knownGroupsStates = states;

            var sortedRows = _knownGroupsStates
                .Select (kvp => (Name: kvp.Key, Collapsed: kvp.Value))
                .Where (grp => grp.Name.isNotNullOrWhiteSpace )
                .OrderBy (grp => grp.Name)
                .ToArray ();

#if DEBUG
            var dd = _knownGroupsStates.dumpArrayToString ();
            Debug.WriteLine( $"SaveGroupsState: {dd}" );
#endif

            var text = sortedRows.eSerializeAsXML ();

            FileInfo fi = GetGroupsStateStorageFile (@directory, dataID);
            using ( var sw = fi.eCreateWriter( FileMode.OpenOrCreate , encoding: System.Text.Encoding.Unicode ) )
            {
                sw.BaseStream.eTruncate(); //trim previous file data										   
                sw.WriteLine( text ); //writing actual data
                sw.Flush();
            }
            return fi;
        }


        /// <summary>Load groups collapsed/expanded state from storage file</summary>
        /// <param name="directory">Folder where settings file will be placed</param>
        /// <param name="dataID">Some user custom file suffix</param>

        private Dictionary<string , bool>? LoadGroupsState ( string? @directory = null , string dataID = "" )
        {
            try
            {
                FileInfo fi = GetGroupsStateStorageFile (@directory, dataID);
                return !fi.Exists
                    ? null
                    : fi
                    .eReadAsText()!
                    .Trim()
                    .eDeSerializeXML<(string Name, bool Collapsed) []>( throwOnError: true )!
                    .Where( grp => grp.Name.isNotNullOrWhiteSpace )
                    .OrderBy( grp => grp.Name )
                    .ToDictionary( r => r.Name , r => r.Collapsed );
            }
            catch ( Exception ex )
            {
                //just ignore any errors
                ex.eLogError( false );
                return null;
            }
        }



        /// <summary>Load groups collapsed/expanded state from storage file and apply to ListView</summary>
        /// <param name="directory">Folder where settings file will be placed</param>
        /// <param name="dataID">Some user custom file suffix</param>

        public void RestoreGroupsStateFromStorage ( string? @directory = null , string dataID = "" )
        {
            var savedStates = LoadGroupsState (@directory, dataID);
            try
            {
                this.runOnLockedUpdate( () =>
                {
                    this.groupsAsIEnumerable()
                    .forEach( grp =>
                        {
#if !NET
							grp.eSetStateFlag (ListViewGroupState.LVGS_COLLAPSIBLE);
#endif
                            string grpID = grp.eGetStringID (DEFAULT_LIST_VIEW_GROUP_NAME).ToLower ().Trim ();

                            if ( savedStates != null && savedStates.TryGetValue( grpID , out bool savedState ) )
                            {
#if NET
                                grp.CollapsedState = savedState ? ListViewGroupCollapsedState.Collapsed : ListViewGroupCollapsedState.Expanded;
#else
								grp.eSetState_Collapsed (savedState);
#endif
                            }
                        } );
                } );

#if DEBUG
                var dd = savedStates.dumpArrayToString ();
                Debug.WriteLine( $"RestoreGroupsStateFromStorage: {dd}" );
#endif

            }
            catch ( Exception ex )
            {
                //just ignore any errors
                ex.eLogError( false );
            }
        }


        //#if !NET
#if !NET



		
		private void UpdateGroupsStateInStorage ()
		{
			var currentStates = GetGroupsState ();
			bool hasAnyChanges = !currentStates.eIsDictionaryEqualTo (_knownGroupsStates);
			if (hasAnyChanges)
			{
				this.GroupsCollapsedStateChangedByMouse?.Invoke (this, "");
			}
		}



        #region SetGroupState


		public static void SetGroupState ( ListViewGroup grp, ListViewGroupState state )
		{
			grp.eSetGroup (state: state);
			grp.ListView!.runInUIThread (() => grp.ListView!.Refresh ());
		}

		public static void SetGroupStateFlag ( ListViewGroup grp, ListViewGroupState flag, bool flagState = true )
		{
			ListViewGroupState state = GetGroupState (grp);

			state |= flag;
			if (!flagState) state ^= flag;

			SetGroupState (grp, state);
		}


		/// <summary>Returns the combination of state values that are set. For example, if dwMask is LVGS_COLLAPSED and the value returned is zero, the LVGS_COLLAPSED state is not set. Zero is returned if the group is not found.</summary>
		/// <param name="Mask">Specifies the state values to retrieve. This is a combination of the flags listed for the state member of LVGROUP.</param>
		public static ListViewGroupState GetGroupState ( ListViewGroup grp, ListViewGroupState? bitsToGet = null )
		{
			if (grp?.ListView == null) return default;

			if (!bitsToGet.HasValue) bitsToGet = ListViewGroupState.LVGS_COLLAPSED;

			ListViewGroupState result = grp.ListView.runInUIThread (() =>
			{
				return (ListViewGroupState) SendMessage<ListViewMessage> (
					grp.ListView.Handle,
					ListViewMessage.LVM_GETGROUPSTATE,
					grp.eGetWin32ID (),
					(int) bitsToGet.Value);
			});
			return result;
		}

        #endregion






		private delegate void CallbackSetGroupString ( ListViewGroup lstvwgrp, string value );


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

		Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessage.LVM_SETGROUPINFO, GrpId, LVG)
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

		 Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessage.LVM_SETGROUPINFO, GrpId, group)
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
		Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessage.LVM_SETGROUPINFO, GrpId, group)
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

			Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessage.LVM_SETGROUPINFO, GrpId, group)
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

		Call SendMessage(lstvwgrp.ListView.Handle, ListViewMessage.LVM_SETGROUPINFO, GrpId, group)
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


    //[DebuggerNonUserCode, DebuggerStepThrough]
    internal static partial class Extensions_WinForms_Controls_Listview
    {


#if !NET


        #region Group State


		internal const ListViewGroupState DEFAULT_GROUP_STATE = ListViewGroupState.LVGS_COLLAPSIBLE;


		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		
		internal static void eSetState ( this ListViewGroup grp, ListViewGroupState state = DEFAULT_GROUP_STATE )
			=> ListViewEx.SetGroupState (grp, state);


		
		internal static void eSetStateFlag ( this ListViewGroup grp, ListViewGroupState flag, bool flagState = true )
			=> ListViewEx.SetGroupStateFlag (grp, flag, flagState);


		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		
		internal static void eSetState_Collapsed ( this ListViewGroup grp, bool collapse )
		{
			bool needChangeState = false;
			ListViewGroupState state = grp.eGetState ();

			if (!state.HasFlag (ListViewGroupState.LVGS_COLLAPSIBLE))
			{
				needChangeState = true;
				state |= ListViewGroupState.LVGS_COLLAPSIBLE;
			}

			if (state.HasFlag (ListViewGroupState.LVGS_COLLAPSED) != collapse)
			{
				needChangeState = true;
				state ^= ListViewGroupState.LVGS_COLLAPSED; // revering Collapsed state
			}

			if (needChangeState) grp.eSetState (state);
		}


		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		
		internal static void eSetGroupsState ( this ListView? lvw, ListViewGroupState state = DEFAULT_GROUP_STATE )
			=> lvw?.eGroupsAsIEnumerable ().ToList ().ForEach (grp => grp.eSetState (state));





		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		
		internal static ListViewGroupState eGetState ( this ListViewGroup grp )
			=> ListViewEx.GetGroupState (grp);


		/// <summary>Группа будет сворачиваться/разворачиваться только на ListViewNF (или надо реализовать соответствующую функциональность самостоятельно)</summary>
		
		internal static bool eGetState_IsCollapsed ( this ListViewGroup grp )
			=> grp.eGetState ().HasFlag (ListViewGroupState.LVGS_COLLAPSED);



        #endregion


		
		public static (ListViewGroup Group, bool Created) eGroupsCreateGroupByKey(
			this ListViewEx lvw,
			string key,
			string? header = null,
			ListViewGroupState newGroupState = ListViewGroupState.Collapsible)
			=> lvw.eGroupsCreateGroupByKey(key, header, new Action<ListViewGroup>(grp => grp.eSetStateFlag(newGroupState)));



		
		internal static void eSetGroupsTitlesBy_Count (
			this ListView lvw,
			Func<ListViewGroup, string>? callbackGroupTitleProvider = null,
			Action<ListViewGroup, string, ListViewGroupState>? callbackGroupTitleApplier = null )
		{
			foreach (var grp in lvw.eGroupsAsIEnumerable ())
			{
				{
					var collapsed = grp.eGetState_IsCollapsed ();

					ListViewGroupState state = DEFAULT_GROUP_STATE;
					if (collapsed) state |= ListViewGroupState.LVGS_COLLAPSED;

					var title = ( callbackGroupTitleProvider != null )
						? callbackGroupTitleProvider.Invoke (grp)
						: $"{grp.Name} ({grp.Items.Count})";

					if (callbackGroupTitleApplier != null)
						callbackGroupTitleApplier.Invoke (grp, title, state);
					else
						grp.eSetGroup (header: title, state: state);
				}
			}
		}


		/// <summary>Safely sets group Header and don't broke groups collapswd states</summary>
		
		public static void eSetGroupsTitlesFastW32Safe (
			this ListView? lvw,
			Func<ListViewGroup, string>? getGroupHeader = null )
				=> lvw?.eGroupsAsIEnumerable ().forEach (g =>
				{
					string sTitle = g.Name ?? "";
					if (getGroupHeader != null)
						sTitle = getGroupHeader.Invoke (g);
					else
						sTitle = $"{sTitle} ({g.Items.Count:N0})".Trim ();

					if (!string.isNullOrWhiteSpace (sTitle))
					{
						g.eSetText (sTitle);
					}
				});



		///<summary>
		///Safely sets group Header and don't broke groups collapswd states
		///MT Safe!!!</summary>
		
		public static void erunOnLockedUpdateW32Safe (
			this ListView? lvw,
			Action a,
			bool autoSizeColumns = false,
			bool fastUpdateGroupHeaders = false )
		{
			_ = a ?? throw new ArgumentNullException (nameof (a));

			void a2 ()
			{
				lvw?.BeginUpdate ();
				try { a!.Invoke (); }
				finally
				{
					if (autoSizeColumns) lvw?.eAutoSizeColumnsAuto ();
					if (fastUpdateGroupHeaders) lvw?.eSetGroupsTitlesFastW32Safe ();
					lvw?.EndUpdate ();
				}
			}
			;

			if (lvw != null && lvw.InvokeRequired)
				lvw.runInUIThread (a2);
			else
				a2 ();
		}


		
		internal static void eAddGroupsWithState ( this ListView lvw, IEnumerable<ListViewGroup>? groups, ComCtl32.ListViewGroupState state = DEFAULT_GROUP_STATE )
		{
			if (groups == null || lvw == null) return;
			foreach (var G in groups)
			{
				lvw.Groups.Add (G);
				G.eSetState (state);
			}
		}




        #region SetGroup

		internal static Int32 SetGroupInfo ( this ListViewGroup lstvwgrp, LVGROUP group )
			=> lstvwgrp.ListView!.runInUIThread (() =>
				SendMessage (lstvwgrp.ListView!.Handle, ListViewMessage.LVM_SETGROUPINFO, group.iGroupId, group));


		internal static Int32 eSetGroup (
			this ListViewGroup lstvwgrp,
			string? header = null,
			string? subTitle = null,
			string? footer = null,
			string? taskLinkText = null,
			ListViewGroupAlignment align = 0,
			ListViewGroupState? state = null )
		{

			int groupId = lstvwgrp!.ListView!.runInUIThread (() => lstvwgrp.eGetWin32ID ());
			LVGROUP group = new (ListViewGroupMask.LVGF_NONE) { iGroupId = groupId, };

			if (header != null)
			{
				group.mask |= ListViewGroupMask.LVGF_HEADER;
				group.pszHeader.Assign (header);
			}

			if (footer != null)
			{
				group.mask |= ListViewGroupMask.LVGF_FOOTER;
				group.pszFooter.Assign (footer);
			}

			if (subTitle != null)
			{
				group.mask |= ListViewGroupMask.LVGF_SUBTITLE;
				group.pszSubtitle.Assign (subTitle);
			}

			if (taskLinkText != null)
			{
				group.mask |= ListViewGroupMask.LVGF_TASK;
				group.pszTask.Assign (taskLinkText);
			}

			if (align != 0)
			{
				group.mask |= ListViewGroupMask.LVGF_ALIGN;
				group.uAlign = align;
			}

			if (state.HasValue)
			{
				group.mask |= ListViewGroupMask.LVGF_STATE;
				group.state = state.Value;
			}

			return SetGroupInfo (lstvwgrp, group);
		}


        #endregion




		/// <summary>Задаёт текст для группы, но не меняет флаг состояния группы (!!! Стандартный .Header=String меняет флаг состояния!!!)</summary>
		
		internal static void eSetText ( this ListViewGroup lvg, string Text )
			=> lvg.eSetGroup (Text);

		
		internal static void eSetSubTitle ( this ListViewGroup lvg, string subTitle )
			=> lvg.eSetGroup (subTitle: subTitle);

		
		internal static void eSetFooter ( this ListViewGroup lvg, string footerText )
			=> lvg.eSetGroup (footer: footerText);


		internal static Int32 eGetWin32ID ( this ListViewGroup lstvwgrp )
		{
			_ = lstvwgrp!.ListView ?? throw new ArgumentException ("Group must ge Added to ListView before!", nameof (lstvwgrp));

			var groupID = lstvwgrp.eGetPropertyValue_Int32 ("ID");
			if (!groupID.HasValue) groupID = lstvwgrp.ListView.Groups.IndexOf (lstvwgrp);
			return groupID.Value;
		}


        #region Obsolete! DO NOT USE! Only for compatibility!


		private const string ERROR_LIST_VIEW_GROUP_NAME_NULL = "ListViewGroup.Name = NULL!";
		private const string LISTVIEW_GROUPS_STATE_KEY_PREFIX = @"ListView Groups states\";

		[Obsolete ("Do not save group states in to registry! Save to file instead!", true)]
		
		internal static void eSaveGroupsCollapsedStates_Reg ( this ListView lvw, string listViewID = "" )
			=> lvw
			.eGroupsAsIEnumerable ()
			.ToList ()
			.ForEach (grp => grp.eSaveGroupCollapsedState_Reg (listViewID));


		[Obsolete ("Do not save group states in to registry! Save to file instead!", true)]
		
		internal static void eSaveGroupCollapsedState_Reg ( this ListViewGroup grp, string listViewID = "" )
		{
			if (grp.Name.isNullOrWhiteSpace )
				throw new ArgumentNullException (ERROR_LIST_VIEW_GROUP_NAME_NULL);

			if (listViewID.isNullOrWhiteSpace )
			{
				if (grp.ListView == null) return; // Group has no assigned ListView - ignore
				listViewID = grp.ListView.eCreateListViewID ();
			}

			//var sFullGroupIDRegKey = LISTVIEW_GROUPS_STATE_KEY_PREFIX + listViewID;
			//uomvb.Settings.SaveSetting(grp.Name, grp.GetState_IsCollapsed, null, sFullGroupIDRegKey);
		}


		[Obsolete ("Do not save group states in to registry! Save to file instead!", true)]
		
		internal static void eLoadGroupCollapsedState_Reg ( this ListViewGroup grp, string listViewID = "", bool SearchPreVersion = false )
		{
			if (grp.Name.isNullOrWhiteSpace )
				throw new ArgumentNullException (ERROR_LIST_VIEW_GROUP_NAME_NULL);

			if (listViewID.isNullOrWhiteSpace )
			{
				if (grp.ListView == null) return; // Group has no assigned ListView - ignore
				listViewID = grp.ListView.eCreateListViewID ();
			}

			/*
			var sFullGroupIDRegKey = LISTVIEW_GROUPS_STATE_KEY_PREFIX + listViewID;
			var bCollapsed = uomvb.Settings.GetSetting_Boolean(grp.Name, false, null, SearchPreVersion, sFullGroupIDRegKey).Value;
			ListViewEx.ListViewGroupState State = DEFAULT_GROUP_STATE;
			if (bCollapsed) State |= ListViewEx.ListViewGroupState.Collapsed;
			grp.eSetState(State);
			 */
		}


        #endregion


#endif


        /// <summary>Generate ListView ID with Form Name</summary>

        internal static string eCreateListViewID ( this ListView lvw ) => $"{lvw.FindForm()!.Name}.{lvw.Name}";




        internal static string eGetStringID ( this ListViewGroup g , string defaultID = "default" )
        {
            string?[] fields = [ g.Name, g.Header ];
            return fields.FirstOrDefault( s => s.isNotNullOrWhiteSpace ) ?? defaultID;
        }








        /*
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			AllowSubItemImages();
		}


		
		 */



        public static ListViewStyleEx GetExtendedStyle ( this ListView lvw )
           => ( ListViewStyleEx )User32.SendMessage<ListViewMessage>( lvw.Handle , ListViewMessage.LVM_GETEXTENDEDLISTVIEWSTYLE );



        public static void SetExtendedStyle ( this ListView lvw , ListViewStyleEx s )
            => User32.SendMessage<ListViewMessage>( lvw.Handle , ListViewMessage.LVM_SETEXTENDEDLISTVIEWSTYLE , 0 , ( int )s );


        public static void SetExtendedStyleFlag ( this ListView lvw , ListViewStyleEx s )
            => lvw.SetExtendedStyle( lvw.GetExtendedStyle() | s );


        #region Allow SubItemImages


        public static bool Apply ( this ComCtl32.LVITEM lvi , IntPtr h )
            => SendMessage( h , ListViewMessage.LVM_SETITEM , 0 , lvi ) != 0;



        public static void AllowSubItemImages ( this ListView lvw )
            => lvw.SetExtendedStyleFlag( ListViewStyleEx.LVS_EX_SUBITEMIMAGES );



        public static bool SetSubItemImage ( this ListViewItem li , int col , int iconIndex )
        {
            if ( li.ListView == null ) return false;// throw new ArgumentNullException(nameof(li), "ListViewItem.ListView = NULL!");

            ComCtl32.LVITEM lvi = new (li.Index, col, ListViewItemMask.LVIF_IMAGE)
            {
                iImage = iconIndex
            };
            return lvi.Apply( li.ListView.Handle );
        }


        #endregion



    }




}
