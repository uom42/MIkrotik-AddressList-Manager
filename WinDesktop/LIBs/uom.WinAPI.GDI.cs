using Extensions;

using uom;
using uom.AutoDisposable;
using uom.WinAPI.gdi;

using Vanara.PInvoke;

#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.


namespace uom.WinAPI.gdi
{


    /// <summary>Extened HDC</summary>
    [DebuggerDisplay("HDC2={Handle}")]
    internal class HDC2 : AutoDisposable1, IHandle
    {

        public enum FreeModes : int
        {
            None = 0,

            /// <summary>DeleteDC(handle)</summary>
            DeleteDC,

            /// <summary>ReleaseDC(AttachedWindowHanle, handle)</summary>
            ReleaseDC,

            /// <summary>AttachedGraphics.ReleaseHdc(handle)</summary>
            ReleaseGraphicsDC
        }

        private readonly FreeModes _destructorMode = FreeModes.DeleteDC;
        private IWin32Window? _attachedWindow;
        private Graphics? _attachedGraphics = null;

        private HDC _hdc = HDC.NULL;


        #region Constructor / Destructor

        /// <summary>Attach to an Existing DC</summary>
        [DebuggerNonUserCode, DebuggerStepThrough]
        public HDC2 ( HDC hdcSource , FreeModes ddm ) : base()
        {
            if ( hdcSource.IsNull ) throw new ArgumentNullException(nameof(hdcSource));
            _hdc = hdcSource;
            _destructorMode = ddm;
        }

        /// <summary>
        /// /// <summary>Use GetDC. retrieves a handle to a display device context (DC) for the client area of a specified window or for the entire screen.</summary>
        /// </summary>				
        /// <param name="clientAreaOnly">
        /// If TRUE we using GetDC - retrieves a handle to a display device context (DC) for the client area of a specified window or for the entire screen.
        /// If FALSE we using GetWindowDC - retrieves the device context (DC) for the entire window, including title bar, menus, and scroll bars.
        /// </param>
        public HDC2 ( IWin32Window window , bool clientAreaOnly )
         : this(clientAreaOnly
                 ? User32.GetDC(window.Handle)
                 : User32.GetWindowDC(window.Handle) ,
             FreeModes.ReleaseDC)
        {
            _attachedWindow = window;
        }


        protected override void FreeUnmanagedObjects ()
        {
            base.FreeUnmanagedObjects();

            if ( _hdc.isNotValid ) return;

            bool FreeGraphics ()
            {
                _attachedGraphics?.ReleaseHdc((IntPtr)_hdc);
                return true;
            }

            var hdcPtr = (IntPtr)_hdc;
            bool result = _destructorMode switch
            {
                FreeModes.None => true,
                FreeModes.DeleteDC => Gdi32.DeleteDC(hdcPtr),
                FreeModes.ReleaseDC => User32.ReleaseDC(_attachedWindow!.Handle , hdcPtr),
                FreeModes.ReleaseGraphicsDC => FreeGraphics(),
                _ => throw new NotImplementedException()
            };

            if ( result )
            {
                _hdc = HDC.NULL;
                _attachedGraphics = null;
                _attachedWindow = null;
            }
        }


        #endregion


        #region Shared Constructors


        /// <inheritdoc cref="Gdi32.CreateDC" />

        internal static HDC2 CreateDC_Driver ( string driver , string deviceName , ref DEVMODE pDevMode )
        {
            var hDC = Gdi32.CreateDC(driver , deviceName , null , ref pDevMode);
            return hDC.IsInvalid ? throw new Win32Exception() : new HDC2(hDC , FreeModes.DeleteDC);
        }


        internal static HDC2 CreateDC_Driver ( string driver , string deviceName , string? port , IntPtr lpInitData )
        {
            var hDC = Gdi32.CreateDC(driver , deviceName , port , lpInitData);
            return hDC.IsInvalid ? throw new System.ComponentModel.Win32Exception() : new HDC2(hDC , FreeModes.DeleteDC);
        }


        internal static HDC2 CreateDC_Printer ( string deviceName , IntPtr lpInitData )
            => CreateDC_Driver(core.WINDLL_WINSPOOL , deviceName , null , lpInitData);



#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.


        internal static HDC2 CreateDC_Display ( IntPtr lpInitData )
            => CreateDC_Driver(core.DISPLAY , null , null , lpInitData);

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.



        public static HDC2 CreateCompatible ( HDC hdcSource )
        {
            var hdcCompat = Gdi32.CreateCompatibleDC(hdcSource);
            return hdcCompat.IsInvalid ? throw new Win32Exception() : new HDC2(hdcCompat , FreeModes.DeleteDC);
        }


        #endregion


        /// <summary>Uses Graphics.GetHdc</summary>
        [DebuggerNonUserCode, DebuggerStepThrough]

        public static HDC2 GetGraphicsDC ( Graphics g )
        {
            g.ThrowIfNull();
            var hdc = g.GetHdc();
            return hdc.isNotValid
                ? throw new NotSupportedException("Graphics.GetHdc FAILED!")
                : new HDC2(hdc , FreeModes.ReleaseGraphicsDC) { _attachedGraphics = g };
        }

        /// <summary>Graphics.FromHdc(Me.DangerousGetHandle)</summary>
        [DebuggerNonUserCode, DebuggerStepThrough]

        public Graphics CreateGraphics () => Graphics.FromHdc(_hdc.DangerousGetHandle());


        #region Operators

        public static bool operator == ( HDC2 DC1 , HDC2 DC2 ) => DC1.DangerousGetHandle() == DC2.DangerousGetHandle();

        public static bool operator != ( HDC2 DC1 , HDC2 DC2 ) => !(DC1 == DC2);


        public static explicit operator HDC2 ( HDC hdc ) => new HDC2(hdc , FreeModes.None);

        public static explicit operator HDC2 ( HandleRef HR ) => (HDC2)HR.Wrapper!;

        public static implicit operator IntPtr ( HDC2 DC1 ) => DC1.DangerousGetHandle();

        public static implicit operator HandleRef ( HDC2 DC1 ) => new(DC1 , DC1.DangerousGetHandle());


        #endregion


        #region GetDeviceCaps


        #region Ext caps

        /// <summary>Device technology</summary>
        public enum CAPS_TECHNOLOGY
        {
            DT_PLOTTER = 0, // Vector plotter
            DT_RASDISPLAY = 1, // Raster display
            DT_RASPRINTER = 2, // Raster printer
            DT_RASCAMERA = 3, // Raster camera
            DT_CHARSTREAM = 4, // Character-stream, PLP
            DT_METAFILE = 5, // Metafile, VDM
            DT_DISPFILE = 6 // Display-file
        }

        [Flags()]
        public enum CAPS_CURVECAPS // Curve Capabilities
        {
            CC_NONE = 0, // Curves not supported
            CC_CIRCLES = 1, // Can do circles
            CC_PIE = 2, // Can do pie wedges
            CC_CHORD = 4, // Can do chord arcs
            CC_ELLIPSES = 8, // Can do ellipese
            CC_WIDE = 16, // Can do wide lines
            CC_STYLED = 32, // Can do styled lines
            CC_WIDESTYLED = 64, // Can do wide styled lines
            CC_INTERIORS = 128, // Can do interiors
            CC_ROUNDRECT = 256 // 
        }

        [Flags()]
        public enum CAPS_LINECAPS // Line Capabilities
        {
            LC_NONE = 0, // Lines not supported
            LC_POLYLINE = 2, // Can do polylines
            LC_MARKER = 4, // Can do markers
            LC_POLYMARKER = 8, // Can do polymarkers
            LC_WIDE = 16, // Can do wide lines
            LC_STYLED = 32, // Can do styled lines
            LC_WIDESTYLED = 64, // Can do wide styled lines
            LC_INTERIORS = 128 // Can do interiors
        }

        [Flags()]
        public enum CAPS_POLYGONALCAPS // Polygonal Capabilities
        {
            PC_NONE = 0, // Polygonals not supported
            PC_POLYGON = 1, // Can do polygons
            PC_RECTANGLE = 2, // Can do rectangles
            PC_WINDPOLYGON = 4, // Can do winding polygons

            // PC_TRAPEZOID = 4			 ' Can do trapezoids
            PC_SCANLINE = 8, // Can do scanlines

            PC_WIDE = 16, // Can do wide borders
            PC_STYLED = 32, // Can do styled borders
            PC_WIDESTYLED = 64, // Can do wide styled borders
            PC_INTERIORS = 128, // Can do interiors
            PC_POLYPOLYGON = 256, // Can do polypolygons
            PC_PATHS = 512 // Can do paths
        }

        [Flags()]
        public enum CAPS_CLIP // Clipping Capabilities
        {
            CP_NONE = 0, // No clipping of output
            CP_RECTANGLE = 1, // Output clipped to rects
            CP_REGION = 2 // obsolete
        }

        [Flags()]
        public enum CAPS_TEXTCAPS // Text Capabilities
        {
            TC_OP_CHARACTER = 0x1, // Can do OutputPrecision CHARACTER
            TC_OP_STROKE = 0x2, // Can do OutputPrecision STROKE
            TC_CP_STROKE = 0x4, // Can do ClipPrecision STROKE
            TC_CR_90 = 0x8, // Can do CharRotAbility 90
            TC_CR_ANY = 0x10, // Can do CharRotAbility ANY
            TC_SF_X_YINDEP = 0x20, // Can do ScaleFreedom X_YINDEPENDENT
            TC_SA_DOUBLE = 0x40, // Can do ScaleAbility DOUBLE
            TC_SA_INTEGER = 0x80, // Can do ScaleAbility INTEGER
            TC_SA_CONTIN = 0x100, // Can do ScaleAbility CONTINUOUS
            TC_EA_DOUBLE = 0x200, // Can do EmboldenAbility DOUBLE
            TC_IA_ABLE = 0x400, // Can do ItalisizeAbility ABLE
            TC_UA_ABLE = 0x800, // Can do UnderlineAbility ABLE
            TC_SO_ABLE = 0x1000, // Can do StrikeOutAbility ABLE
            TC_RA_ABLE = 0x2000, // Can do RasterFontAble ABLE
            TC_VA_ABLE = 0x4000, // Can do VectorFontAble ABLE
            TC_RESERVED = 0x8000,
            TC_SCROLLBLT = 0x10000 // Don't do text scroll with blt
        }

        [Flags()]
        public enum CAPS_RASTERCAPS // Raster Capabilities
        {
            RC_NONE = 0,
            RC_BITBLT = 1, // Can do standard BLT.
            RC_BANDING = 2, // Device requires banding support
            RC_SCALING = 4, // Device requires scaling support
            RC_BITMAP64 = 8, // Device can support >64K bitmap
            RC_GDI20_OUTPUT = 0x10, // has 2.0 output calls
            RC_GDI20_STATE = 0x20,
            RC_SAVEBITMAP = 0x40,
            RC_DI_BITMAP = 0x80, // supports DIB to memory
            RC_PALETTE = 0x100, // supports a palette
            RC_DIBTODEV = 0x200, // supports DIBitsToDevice
            RC_BIGFONT = 0x400, // supports >64K fonts
            RC_STRETCHBLT = 0x800, // supports StretchBlt
            RC_FLOODFILL = 0x1000, // supports FloodFill
            RC_STRETCHDIB = 0x2000, // supports StretchDIBits
            RC_OP_DX_OUTPUT = 0x4000,
            RC_DEVBITS = 0x8000
        }

        [Flags()]
        public enum CAPS_SHADEBLENDCAPS // Shading and blending caps
        {
            SB_NONE = 0x0,
            SB_CONST_ALPHA = 0x1,
            SB_PIXEL_ALPHA = 0x2,
            SB_PREMULT_ALPHA = 0x4,

            SB_GRAD_RECT = 0x10,
            SB_GRAD_TRI = 0x20,

            // Color Management caps
            CM_NONE = 0x0,

            CM_DEVICE_ICM = 0x1,
            CM_GAMMA_RAMP = 0x2,
            CM_CMYK_COLOR = 0x4

        }

        #endregion



        /// <inheritdoc cref="Gdi32.GetDeviceCaps" />

        public int GetDeviceCaps ( Gdi32.DeviceCap index )
            => Gdi32.GetDeviceCaps(DangerousGetHandle() , index);


        #endregion


        #region Structure MarginsF

        internal struct MarginsF
        {
            public float Left;
            public float Top;
            public float Right;
            public float Bottom;

            public SizeF ClientSize ()
            {
                return new SizeF(Right - Left , Bottom - Top);
            }

            public override string ToString ()
                => string.Format("({0}|{1})-({2}|{3})" , Math.Round(Left , 1) , Math.Round(Top , 1) , Math.Round(Right , 1) , Math.Round(Bottom , 1));


        }

        #endregion


        #region Sizes


        /// <summary>Точек На Дюйм (Logical pixels/inch in X)</summary>

        public Point DPI ()
            => new(GetDeviceCaps(Gdi32.DeviceCap.LOGPIXELSX) , GetDeviceCaps(Gdi32.DeviceCap.LOGPIXELSY));


        /// <summary>in pixels</summary>

        public SizeF PhysicalSize ( GraphicsUnit ScaleTo )
        {
            var sz = new Size(GetDeviceCaps(Gdi32.DeviceCap.PHYSICALWIDTH) , GetDeviceCaps(Gdi32.DeviceCap.PHYSICALHEIGHT));
            var szf = new SizeF(ScaleX(sz.Width , GraphicsUnit.Pixel , ScaleTo) , ScaleY(sz.Height , GraphicsUnit.Pixel , ScaleTo));
            return szf;
        }


        /// <summary>in pixels</summary>

        public SizeF ClientSize ( GraphicsUnit ScaleTo )
        {
            var sz = new Size(GetDeviceCaps(Gdi32.DeviceCap.HORZRES) , GetDeviceCaps(Gdi32.DeviceCap.VERTRES));
            var szf = new SizeF(ScaleX(sz.Width , GraphicsUnit.Pixel , ScaleTo) , ScaleY(sz.Height , GraphicsUnit.Pixel , ScaleTo));
            return szf;
        }


        #endregion


        #region Margins


        public MarginsF HardwareMargins ( GraphicsUnit ScaleTo )
        {
            var pixMargins = new Point(GetDeviceCaps(Gdi32.DeviceCap.PHYSICALOFFSETX) , GetDeviceCaps(Gdi32.DeviceCap.PHYSICALOFFSETY));
            var MarginLT = new PointF(ScaleX(pixMargins.X , GraphicsUnit.Pixel , ScaleTo) , ScaleY(pixMargins.Y , GraphicsUnit.Pixel , ScaleTo));
            var szfSize = PhysicalSize(ScaleTo);
            var szfClient = ClientSize(ScaleTo);
            var szfMarginRB = new SizeF(szfSize.Width - MarginLT.X - szfClient.Width , szfSize.Height - MarginLT.Y - szfClient.Height);

            MarginsF MR;
            MR.Left = MarginLT.X;
            MR.Top = MarginLT.Y;
            MR.Right = szfMarginRB.Width;
            MR.Bottom = szfMarginRB.Height;
            return MR;
        }

        public RectangleF HardwareMarginsAsRect ( GraphicsUnit ScaleTo )
        {
            var m = HardwareMargins(ScaleTo);
            return new RectangleF(m.Left , m.Top , m.Right - m.Left , m.Bottom - m.Top);
        }


        #endregion


        #region Scale XXX



        private static float Scale ( float value , int dpi , GraphicsUnit sourceUnits = GraphicsUnit.Millimeter , GraphicsUnit targetUnits = GraphicsUnit.Pixel )
        {
            if ( value == 0f || sourceUnits == targetUnits ) return value;

            //First Converting to inches
            float inches = sourceUnits switch
            {
                GraphicsUnit.Inch => value,
                GraphicsUnit.Pixel => value / dpi,
                GraphicsUnit.Millimeter => value.distance_MMToInches(),
                GraphicsUnit.Display => value / 100f,
                GraphicsUnit.Document => value / 300f,
                GraphicsUnit.Point => value / 72f,
                _ => throw new ArgumentOutOfRangeException(nameof(sourceUnits))
            };

            //Than Converting to target
            value = targetUnits switch
            {
                GraphicsUnit.Inch => inches,
                GraphicsUnit.Pixel => inches * dpi,
                GraphicsUnit.Millimeter => inches.distance_MMToInches(),
                GraphicsUnit.Display => inches * 100f,
                GraphicsUnit.Document => inches * 300f,
                GraphicsUnit.Point => inches * 72f,
                _ => throw new ArgumentOutOfRangeException(nameof(targetUnits))
            };
            return value;
        }


        public float ScaleX ( float Value , GraphicsUnit sourceUnits = GraphicsUnit.Millimeter , GraphicsUnit targetUnits = GraphicsUnit.Pixel )
            => Scale(Value , DPI().X , sourceUnits , targetUnits);


        public float ScaleY ( float Value , GraphicsUnit sourceUnits = GraphicsUnit.Millimeter , GraphicsUnit targetUnits = GraphicsUnit.Pixel )
            => Scale(Value , DPI().Y , sourceUnits , targetUnits);


        public PointF Scale ( PointF PT , GraphicsUnit sourceUnits = GraphicsUnit.Millimeter , GraphicsUnit targetUnits = GraphicsUnit.Pixel )
            => new(ScaleX(PT.X , sourceUnits , targetUnits) , ScaleY(PT.Y , sourceUnits , targetUnits));


        #endregion


        public override bool Equals ( object? obj )
        {
            return obj is not null && (ReferenceEquals(this , obj) ? true : throw new NotImplementedException());
        }


        public nint Handle => ((IHandle)_hdc).DangerousGetHandle();

        public nint DangerousGetHandle () => Handle;

        public override int GetHashCode ()
            => DangerousGetHandle().GetHashCode();

        public bool IsInvalid
            => ((IHandle)_hdc).IsInvalid;
    }


    namespace Imaging
    {

        internal class ProcessFrameEventArgs ( Image F , int iFrame ) : EventArgs(), IDisposable
        {

            #region IDisposable Support
            private bool disposedValue; // To detect redundant calls

            // IDisposable
            protected virtual void Dispose ( bool disposing )
            {
                if ( !disposedValue )
                {
                    if ( disposing )
                    {
                        if ( DisposeImage && Frame is not null )
                        {
                            Frame.Dispose();
                            Frame = null;
                        }
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                    // TODO: set large fields to null.
                }
                disposedValue = true;
            }

            // TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
            // Protected Overrides Sub Finalize()
            // ' Do not change this code. Put cleanup code in Dispose(disposing As Boolean) above.
            // Dispose(False)
            // MyBase.Finalize()
            // End Sub

            // This code added by Visual Basic to correctly implement the disposable pattern.
            public void Dispose ()
            {
                // Do not change this code. Put cleanup code in Dispose(disposing As Boolean) above.
                Dispose(true);
                // TODO: uncomment the following line if Finalize() is overridden above.
                GC.SuppressFinalize(this);
            }
            #endregion

            public Image? Frame { get; private set; } = F;
            public int CurrentFrame { get; private set; } = iFrame;
            public bool DisposeImage { get; set; } = true;
        }


    }

    namespace DevModes
    {

        #region Enum's

        public enum SpecVersions : short // current version of specification
        {

            // #if (WINVER >= 0x0500) || (_WIN32_WINNT >= 0x0400)
            DM_SPECVERSION_401 = 0x401,

            // #elif (WINVER >= 0x0400)
            DM_SPECVERSION_400 = 0x400,

            // #else
            DM_SPECVERSION_320 = 0x320

            // #endif ' WINVER */
        }

        [Flags()]
        public enum FieldBits : int // field selection bits
        {
            DM_ORIENTATION = 0x1,
            DM_PAPERSIZE = 0x2,
            DM_PAPERLENGTH = 0x4,
            DM_PAPERWIDTH = 0x8,
            DM_SCALE = 0x10,
            DM_POSITION = 0x20,
            DM_NUP = 0x40,
            DM_DISPLAYORIENTATION = 0x80,
            DM_COPIES = 0x100,
            DM_DEFAULTSOURCE = 0x200,
            DM_PRINTQUALITY = 0x400,
            DM_COLOR = 0x800,
            DM_DUPLEX = 0x1000,
            DM_YRESOLUTION = 0x2000,
            DM_TTOPTION = 0x4000,
            DM_COLLATE = 0x8000,
            DM_FORMNAME = 0x10000,
            DM_LOGPIXELS = 0x20000,
            DM_BITSPERPEL = 0x40000,
            DM_PELSWIDTH = 0x80000,
            DM_PELSHEIGHT = 0x100000,
            DM_DISPLAYFLAGS = 0x200000,
            DM_DISPLAYFREQUENCY = 0x400000,
            DM_ICMMETHOD = 0x800000,
            DM_ICMINTENT = 0x1000000,
            DM_MEDIATYPE = 0x2000000,
            DM_DITHERTYPE = 0x4000000,
            DM_PANNINGWIDTH = 0x8000000,
            DM_PANNINGHEIGHT = 0x10000000,
            DM_DISPLAYFIXEDOUTPUT = 0x20000000
        }

        public enum Orientations : short
        {
            DMORIENT_PORTRAIT = 1,
            DMORIENT_LANDSCAPE = 2
        }

        public enum BinSelections : short
        {

            // DMBIN_FIRST = DMBIN_UPPER
            DMBIN_UPPER = 1,

            DMBIN_ONLYONE = 1,
            DMBIN_LOWER = 2,
            DMBIN_MIDDLE = 3,
            DMBIN_MANUAL = 4,
            DMBIN_ENVELOPE = 5,
            DMBIN_ENVMANUAL = 6,
            DMBIN_AUTO = 7,
            DMBIN_TRACTOR = 8,
            DMBIN_SMALLFMT = 9,
            DMBIN_LARGEFMT = 10,
            DMBIN_LARGECAPACITY = 11,
            DMBIN_CASSETTE = 14,
            DMBIN_FORMSOURCE = 15,

            // DMBIN_LAST = DMBIN_FORMSOURCE
            DMBIN_USER = 256 // /* device specific bins start here */

        }

        public enum PrintQualities : short
        {
            DMRES_DRAFT = -1,
            DMRES_LOW = -2,
            DMRES_MEDIUM = -3,
            DMRES_HIGH = -4
        }

        public enum PaperSelections : short
        {
            DMPAPER_LETTER = 1, // Letter 8 1/2 x 11 in */
            DMPAPER_LETTERSMALL = 2, // Letter Small 8 1/2 x 11 in */
            DMPAPER_TABLOID = 3, // Tabloid 11 x 17 in  */
            DMPAPER_LEDGER = 4, // Ledger 17 x 11 in  */
            DMPAPER_LEGAL = 5, // Legal 8 1/2 x 14 in */
            DMPAPER_STATEMENT = 6, // Statement 5 1/2 x 8 1/2 in */
            DMPAPER_e_ExecUTIVE = 7, // eExecutive 7 1/4 x 10 1/2 in */
            DMPAPER_A3 = 8, // A3 297 x 420 mm  */
            DMPAPER_A4 = 9, // A4 210 x 297 mm  */
            DMPAPER_A4SMALL = 10, // A4 Small 210 x 297 mm */
            DMPAPER_A5 = 11, // A5 148 x 210 mm  */
            DMPAPER_B4 = 12, // B4 (JIS) 250 x 354  */
            DMPAPER_B5 = 13, // B5 (JIS) 182 x 257 mm */
            DMPAPER_FOLIO = 14, // Folio 8 1/2 x 13 in */
            DMPAPER_QUARTO = 15, // Quarto 215 x 275 mm */
            DMPAPER_10X14 = 16, // 10x14 in  */
            DMPAPER_11X17 = 17, // 11x17 in  */
            DMPAPER_NOTE = 18, // Note 8 1/2 x 11 in  */
            DMPAPER_ENV_9 = 19, // Envelope #9 3 7/8 x 8 7/8 */
            DMPAPER_ENV_10 = 20, // Envelope #10 4 1/8 x 9 1/2 */
            DMPAPER_ENV_11 = 21, // Envelope #11 4 1/2 x 10 3/8 */
            DMPAPER_ENV_12 = 22, // Envelope #12 4 \276 x 11 */
            DMPAPER_ENV_14 = 23, // Envelope #14 5 x 11 1/2 */
            DMPAPER_CSHEET = 24, // C size sheet  */
            DMPAPER_DSHEET = 25, // D size sheet  */
            DMPAPER_ESHEET = 26, // E size sheet  */
            DMPAPER_ENV_DL = 27, // Envelope DL 110 x 220mm */
            DMPAPER_ENV_C5 = 28, // Envelope C5 162 x 229 mm */
            DMPAPER_ENV_C3 = 29, // Envelope C3 324 x 458 mm */
            DMPAPER_ENV_C4 = 30, // Envelope C4 229 x 324 mm */
            DMPAPER_ENV_C6 = 31, // Envelope C6 114 x 162 mm */
            DMPAPER_ENV_C65 = 32, // Envelope C65 114 x 229 mm */
            DMPAPER_ENV_B4 = 33, // Envelope B4 250 x 353 mm */
            DMPAPER_ENV_B5 = 34, // Envelope B5 176 x 250 mm */
            DMPAPER_ENV_B6 = 35, // Envelope B6 176 x 125 mm */
            DMPAPER_ENV_ITALY = 36, // Envelope 110 x 230 mm */
            DMPAPER_ENV_MONARCH = 37, // Envelope Monarch 3.875 x 7.5 in */
            DMPAPER_ENV_PERSONAL = 38, // 6 3/4 Envelope 3 5/8 x 6 1/2 in */
            DMPAPER_FANFOLD_US = 39, // US Std Fanfold 14 7/8 x 11 in */
            DMPAPER_FANFOLD_STD_GERMAN = 40, // German Std Fanfold 8 1/2 x 12 in */
            DMPAPER_FANFOLD_LGL_GERMAN = 41, // German Legal Fanfold 8 1/2 x 13 in */

            // #if(WINVER >= 0x0400)
            DMPAPER_ISO_B4 = 42, // B4 (ISO) 250 x 353 mm */

            DMPAPER_JAPANESE_POSTCARD = 43, // Japanese Postcard 100 x 148 mm */
            DMPAPER_9X11 = 44, // 9 x 11 in  */
            DMPAPER_10X11 = 45, // 10 x 11 in  */
            DMPAPER_15X11 = 46, // 15 x 11 in  */
            DMPAPER_ENV_INVITE = 47, // Envelope Invite 220 x 220 mm */
            DMPAPER_RESERVED_48 = 48, // RESERVED--DO NOT USE */
            DMPAPER_RESERVED_49 = 49, // RESERVED--DO NOT USE */
            DMPAPER_LETTER_EXTRA = 50, // Letter Extra 9 \275 x 12 in */
            DMPAPER_LEGAL_EXTRA = 51, // Legal Extra 9 \275 x 15 in */
            DMPAPER_TABLOID_EXTRA = 52, // Tabloid Extra 11.69 x 18 in */
            DMPAPER_A4_EXTRA = 53, // A4 Extra 9.27 x 12.69 in */
            DMPAPER_LETTER_TRANSVERSE = 54, // Letter Transverse 8 \275 x 11 in */
            DMPAPER_A4_TRANSVERSE = 55, // A4 Transverse 210 x 297 mm */
            DMPAPER_LETTER_EXTRA_TRANSVERSE = 56, // Letter Extra Transverse 9\275 x 12 in */
            DMPAPER_A_PLUS = 57, // SuperA/SuperA/A4 227 x 356 mm */
            DMPAPER_B_PLUS = 58, // SuperB/SuperB/A3 305 x 487 mm */
            DMPAPER_LETTER_PLUS = 59, // Letter Plus 8.5 x 12.69 in */
            DMPAPER_A4_PLUS = 60, // A4 Plus 210 x 330 mm */
            DMPAPER_A5_TRANSVERSE = 61, // A5 Transverse 148 x 210 mm */
            DMPAPER_B5_TRANSVERSE = 62, // B5 (JIS) Transverse 182 x 257 mm */
            DMPAPER_A3_EXTRA = 63, // A3 Extra 322 x 445 mm */
            DMPAPER_A5_EXTRA = 64, // A5 Extra 174 x 235 mm */
            DMPAPER_B5_EXTRA = 65, // B5 (ISO) Extra 201 x 276 mm */
            DMPAPER_A2 = 66, // A2 420 x 594 mm  */
            DMPAPER_A3_TRANSVERSE = 67, // A3 Transverse 297 x 420 mm */
            DMPAPER_A3_EXTRA_TRANSVERSE = 68, // A3 Extra Transverse 322 x 445 mm */
                                              // #endif ' WINVER >= 0x0400 */

            // #if(WINVER >= 0x0500)
            DMPAPER_DBL_JAPANESE_POSTCARD = 69, // Japanese Double Postcard 200 x 148 mm */

            DMPAPER_A6 = 70, // A6 105 x 148 mm  */
            DMPAPER_JENV_KAKU2 = 71, // Japanese Envelope Kaku #2 */
            DMPAPER_JENV_KAKU3 = 72, // Japanese Envelope Kaku #3 */
            DMPAPER_JENV_CHOU3 = 73, // Japanese Envelope Chou #3 */
            DMPAPER_JENV_CHOU4 = 74, // Japanese Envelope Chou #4 */
            DMPAPER_LETTER_ROTATED = 75, // Letter Rotated 11 x 8 1/2 11 in */
            DMPAPER_A3_ROTATED = 76, // A3 Rotated 420 x 297 mm */
            DMPAPER_A4_ROTATED = 77, // A4 Rotated 297 x 210 mm */
            DMPAPER_A5_ROTATED = 78, // A5 Rotated 210 x 148 mm */
            DMPAPER_B4_JIS_ROTATED = 79, // B4 (JIS) Rotated 364 x 257 mm */
            DMPAPER_B5_JIS_ROTATED = 80, // B5 (JIS) Rotated 257 x 182 mm */
            DMPAPER_JAPANESE_POSTCARD_ROTATED = 81, // Japanese Postcard Rotated 148 x 100 mm */
            DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED = 82, // Double Japanese Postcard Rotated 148 x 200 mm */
            DMPAPER_A6_ROTATED = 83, // A6 Rotated 148 x 105 mm */
            DMPAPER_JENV_KAKU2_ROTATED = 84, // Japanese Envelope Kaku #2 Rotated */
            DMPAPER_JENV_KAKU3_ROTATED = 85, // Japanese Envelope Kaku #3 Rotated */
            DMPAPER_JENV_CHOU3_ROTATED = 86, // Japanese Envelope Chou #3 Rotated */
            DMPAPER_JENV_CHOU4_ROTATED = 87, // Japanese Envelope Chou #4 Rotated */
            DMPAPER_B6_JIS = 88, // B6 (JIS) 128 x 182 mm */
            DMPAPER_B6_JIS_ROTATED = 89, // B6 (JIS) Rotated 182 x 128 mm */
            DMPAPER_12X11 = 90, // 12 x 11 in  */
            DMPAPER_JENV_YOU4 = 91, // Japanese Envelope You #4 */
            DMPAPER_JENV_YOU4_ROTATED = 92, // Japanese Envelope You #4 Rotated*/
            DMPAPER_P16K = 93, // PRC 16K 146 x 215 mm */
            DMPAPER_P32K = 94, // PRC 32K 97 x 151 mm */
            DMPAPER_P32KBIG = 95, // PRC 32K(Big) 97 x 151 mm */
            DMPAPER_PENV_1 = 96, // PRC Envelope #1 102 x 165 mm */
            DMPAPER_PENV_2 = 97, // PRC Envelope #2 102 x 176 mm */
            DMPAPER_PENV_3 = 98, // PRC Envelope #3 125 x 176 mm */
            DMPAPER_PENV_4 = 99, // PRC Envelope #4 110 x 208 mm */
            DMPAPER_PENV_5 = 100, // PRC Envelope #5 110 x 220 mm */
            DMPAPER_PENV_6 = 101, // PRC Envelope #6 120 x 230 mm */
            DMPAPER_PENV_7 = 102, // PRC Envelope #7 160 x 230 mm */
            DMPAPER_PENV_8 = 103, // PRC Envelope #8 120 x 309 mm */
            DMPAPER_PENV_9 = 104, // PRC Envelope #9 229 x 324 mm */
            DMPAPER_PENV_10 = 105, // PRC Envelope #10 324 x 458 mm */
            DMPAPER_P16K_ROTATED = 106, // PRC 16K Rotated  */
            DMPAPER_P32K_ROTATED = 107, // PRC 32K Rotated  */
            DMPAPER_P32KBIG_ROTATED = 108, // PRC 32K(Big) Rotated */
            DMPAPER_PENV_1_ROTATED = 109, // PRC Envelope #1 Rotated 165 x 102 mm */
            DMPAPER_PENV_2_ROTATED = 110, // PRC Envelope #2 Rotated 176 x 102 mm */
            DMPAPER_PENV_3_ROTATED = 111, // PRC Envelope #3 Rotated 176 x 125 mm */
            DMPAPER_PENV_4_ROTATED = 112, // PRC Envelope #4 Rotated 208 x 110 mm */
            DMPAPER_PENV_5_ROTATED = 113, // PRC Envelope #5 Rotated 220 x 110 mm */
            DMPAPER_PENV_6_ROTATED = 114, // PRC Envelope #6 Rotated 230 x 120 mm */
            DMPAPER_PENV_7_ROTATED = 115, // PRC Envelope #7 Rotated 230 x 160 mm */
            DMPAPER_PENV_8_ROTATED = 116, // PRC Envelope #8 Rotated 309 x 120 mm */
            DMPAPER_PENV_9_ROTATED = 117, // PRC Envelope #9 Rotated 324 x 229 mm */
            DMPAPER_PENV_10_ROTATED = 118, // PRC Envelope #10 Rotated 458 x 324 mm */
                                           // #endif ' WINVER >= 0x0500 */

            DMPAPER_USER = 256
        }

        public enum DisplayOrientations : int
        {
            DMDO_DEFAULT = 0,
            DMDO_90 = 1,
            DMDO_180 = 2,
            DMDO_270 = 3
        }

        public enum DisplayFixedOutputs : int
        {
            DMDFO_DEFAULT = 0,
            DMDFO_STRETCH = 1,
            DMDFO_CENTER = 2
        }

        public enum ColorModes : short
        {
            DMCOLOR_MONOCHROME = 1,
            DMCOLOR_COLOR = 2
        }

        public enum DuplexModes : short
        {
            DMDUP_SIMPLEX = 1,
            DMDUP_VERTICAL = 2,
            DMDUP_HORIZONTAL = 3
        }

        public enum TrueTypeOptions : short
        {
            DMTT_BITMAP = 1, // print TT fonts as graphics */
            DMTT_DOWNLOAD = 2, // download TT fonts as soft fonts */
            DMTT_SUBDEV = 3, // substitute device fonts for TT fonts */
            DMTT_DOWNLOAD_OUTLINE = 4 // download TT fonts as outline soft fonts */
        }

        public enum CollateModes : short
        {
            DMCOLLATE_FALSE = 0,
            DMCOLLATE_TRUE = 1
        }

        public enum DisplayFlags : int
        {
            DM_GRAYSCALE = 0x1, // /* This flag is no longer valid */
            DM_INTERLACED = 0x2, // /* This flag is no longer valid */
            DMDISPLAYFLAGS_TEXTMODE = 0x4
        }

        public enum NUPs : int // multiple logical page per physical page options
        {
            DMNUP_SYSTEM = 1,
            DMNUP_ONEUP = 2
        }

        public enum ICMMethods : int
        {
            DMICMMETHOD_NONE = 1, // ICM disabled */
            DMICMMETHOD_SYSTEM = 2, // ICM handled by system */
            DMICMMETHOD_DRIVER = 3, // ICM handled by driver */
            DMICMMETHOD_DEVICE = 4, // ICM handled by device */
            DMICMMETHOD_USER = 256 // Device-specific methods start here */
        }

        public enum ICMIntents : int
        {
            DMICM_SATURATE = 1, // Maximize color saturation */
            DMICM_CONTRAST = 2, // Maximize color contrast */
            DMICM_COLORIMETRIC = 3, // Use specific color metric */
            DMICM_ABS_COLORIMETRIC = 4, // Use specific color metric */
            DMICM_USER = 256 // Device-specific intents start here */
        }

        public enum MediaTypes : int
        {
            DMMEDIA_STANDARD = 1, // Standard paper */
            DMMEDIA_TRANSPARENCY = 2, // Transparency */
            DMMEDIA_GLOSSY = 3, // Glossy paper */
            DMMEDIA_USER = 256 // Device-specific media start here */
        }

        public enum DitherTypes : int
        {
            DMDITHER_NONE = 1, // No dithering */
            DMDITHER_COARSE = 2, // Dither with a coarse brush */
            DMDITHER_FINE = 3, // Dither with a fine brush */
            DMDITHER_LINEART = 4, // LineArt dithering */
            DMDITHER_ERRORDIFFUSION = 5, // LineArt dithering */
            DMDITHER_RESERVED6 = 6, // LineArt dithering */
            DMDITHER_RESERVED7 = 7, // LineArt dithering */
            DMDITHER_RESERVED8 = 8, // LineArt dithering */
            DMDITHER_RESERVED9 = 9, // LineArt dithering */
            DMDITHER_GRAYSCALE = 10, // Device does grayscaling */
            DMDITHER_USER = 256 // Device-specific dithers start here */
        }

        #endregion

        #region Structure DEVMODE_PRINTER

        [StructLayout(LayoutKind.Sequential , CharSet = CharSet.Auto)]
        internal struct DEVMODE_PRINTER
        {

            // all values in this structure that specify a physical length, are in tenths of a millimeter.
            internal const int CCHDEVICENAME = 32;

            internal const int CCHFORMNAME = CCHDEVICENAME;

            /// <summary>Specifies the "friendly" name of the printer or display; for example, "PCL/HP LaserJet" in the case of PCL/HP LaserJet. This string is unique among device drivers. Note that this name may be truncated to fit in the dmDeviceName array.</summary>
            [MarshalAs(UnmanagedType.ByValTStr , SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;

            /// <summary>Specifies the version number of the initialization data specification on which the structure is based. To ensure the correct version is used for any operating system, use DM_SPECVERSION.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public SpecVersions dmSpecVersion;

            /// <summary>Specifies the driver version number assigned by the driver developer.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public short dmDriverVersion;

            /// <summary>Specifies the size, in bytes, of the DEVMODE structure, not including any private driver-specific data that might follow the structure's public members. Set this member to sizeof(DEVMODE) to indicate the version of the DEVMODE structure being used.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public short dmSize;

            /// <summary>Contains the number of bytes of private driver-data that follow this structure. If a device driver does not use device-specific information, set this member to zero.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public short dmDriverExtra;

            /// <summary>Specifies whether certain members of the DEVMODE structure have been initialized. If a member is initialized, its corresponding bit is set, otherwise the bit is clear. A driver supports only those DEVMODE members that are appropriate for the printer or display technology.
            ///The following values are defined, and are listed here with the corresponding structure members.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public FieldBits dmFields;

            // Printer
            /// <summary>For printer devices only, selects the orientation of the paper. This member can be either DMORIENT_PORTRAIT (1) or DMORIENT_LANDSCAPE (2).</summary>
            [MarshalAs(UnmanagedType.U2)]
            public Orientations dmOrientation;

            /// <summary>For printer devices only, selects the size of the paper to print on. This member can be set to zero if the length and width of the paper are both set by the dmPaperLength and dmPaperWidth members. Otherwise, the dmPaperSize member can be set to one of the following predefined values.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public PaperSelections dmPaperSize;

            /// <summary>For printer devices only, overrides the length of the paper specified by the dmPaperSize member, either for custom paper sizes or for devices such as dot-matrix printers that can print on a page of arbitrary length. These values, along with all other values in this structure that specify a physical length, are in tenths of a millimeter.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public short dmPaperLength;

            /// <summary>For printer devices only, overrides the width of the paper specified by the dmPaperSize member.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public short dmPaperWidth;

            /// <summary>Specifies the factor by which the printed output is to be scaled. The apparent page size is scaled from the physical page size by a factor of dmScale/100. For example, a letter-sized page with a dmScale value of 50 would contain as much data as a page of 17- by 22-inches because the output text and graphics would be half their original height and width.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public short dmScale;

            /// <summary>Selects the number of copies printed if the device supports multiple-page copies.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public short dmCopies;

            /// <summary>Specifies the paper source. To retrieve a list of the available paper sources for a printer, use the DeviceCapabilities function with the DC_BINS flag. This member can be one of the following values, or it can be a device-specific value greater than or equal to DMBIN_USER.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public BinSelections dmDefaultSource;

            /// <summary>Specifies the printer resolution. There are four predefined device-independent values:</summary>
            [MarshalAs(UnmanagedType.U2)]
            public PrintQualities dmPrintQuality;

            /// <summary>Switches between color and monochrome on color printers. Following are the possible values:</summary>
            [MarshalAs(UnmanagedType.U2)]
            public ColorModes dmColor;

            /// <summary>Selects duplex or double-sided printing for printers capable of duplex printing. Following are the possible values.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public DuplexModes dmDuplex;

            /// <summary>Specifies the y-resolution, in dots per inch, of the printer. If the printer initializes this member, the dmPrintQuality member specifies the x-resolution, in dots per inch, of the printer.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public short dmYResolution;

            /// <summary>Specifies how TrueType fonts should be printed. This member can be one of the following values.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public TrueTypeOptions dmTTOption;

            /// <summary>Specifies whether collation should be used when printing multiple copies. (This member is ignored unless the printer driver indicates support for collation by setting the dmFields member to DM_COLLATE.) This member can be one of the following values.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public CollateModes dmCollate;

            /// <summary>Windows NT/2000/XP: Specifies the name of the form to use; for example, "Letter" or "Legal". A complete set of names can be retrieved by using the EnumForms function. Windows 95/98/Me: Printer drivers do not use this member.</summary>
            [MarshalAs(UnmanagedType.ByValTStr , SizeConst = CCHFORMNAME)]
            public string dmFormName;

            /// <summary>Specifies the number of pixels per logical inch. Printer drivers do not use this member.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public short dmLogPixels;

            /// <summary>Specifies the color resolution, in bits per pixel, of the display device (for example: 4 bits for 16 colors, 8 bits for 256 colors, or 16 bits for 65,536 colors). Display drivers use this member, for example, in the ChangeDisplaySettings function. Printer drivers do not use this member.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public int dmBitsPerPel;

            /// <summary>Specifies the width, in pixels, of the visible device surface. Display drivers use this member, for example, in the ChangeDisplaySettings function. Printer drivers do not use this member.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public int dmPelsWidth;

            /// <summary>Specifies the height, in pixels, of the visible device surface. Display drivers use this member, for example, in the ChangeDisplaySettings function. Printer drivers do not use this member.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public int dmPelsHeight;

            // ///<summary>Specifies the device's display mode. This member can be a combination of the following values.
            // /// Display drivers use this member, for example, in the ChangeDisplaySettings function. Printer drivers do not use this member.</summary>
            // <MarshalAs(UnmanagedType.U4)> 'Public dmDisplayFlags As DisplayFlags

            /// <summary>Specifies where the NUP is done. It can be one of the following.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public NUPs dmNup;

            /// <summary></summary>
            [MarshalAs(UnmanagedType.U4)]
            public int dmDisplayFrequency;

            /// <summary>Windows 95/98/Me; Windows 2000/XP: Specifies how ICM is handled. For a non-ICM application, this member determines if ICM is enabled or disabled. For ICM applications, the system examines this member to determine how to handle ICM support. This member can be one of the following predefined values, or a driver-defined value greater than or equal to the value of DMICMMETHOD_USER.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public ICMMethods dmICMMethod;

            /// <summary>Windows 95/98/Me, Windows 2000/XP: Specifies which color matching method, or intent, should be used by default. This member is primarily for non-ICM applications. ICM applications can establish intents by using the ICM functions. This member can be one of the following predefined values, or a driver defined value greater than or equal to the value of DMICM_USER.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public ICMIntents dmICMIntent;

            /// <summary>Windows 95/98/Me, Windows 2000/XP: Specifies the type of media being printed on. The member can be one of the following predefined values, or a driver-defined value greater than or equal to the value of DMMEDIA_USER. Windows XP: To retrieve a list of the available media types for a printer, use the DeviceCapabilities function with the DC_MEDIATYPES flag.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public MediaTypes dmMediaType;

            /// <summary>Windows 95/98/Me, Windows 2000/XP: Specifies how dithering is to be done. The member can be one of the following predefined values, or a driver-defined value greater than or equal to the value of DMDITHER_USER.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public DitherTypes dmDitherType;

            /// <summary></summary>
            [MarshalAs(UnmanagedType.U4)]
            public int dmReserved1;

            /// <summary></summary>
            [MarshalAs(UnmanagedType.U4)]
            public int dmReserved2;

            /// <summary>Windows NT/2000/XP: This member must be zero. Windows 95/98/Me: This member is not supported.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public int dmPanningWidth;

            /// <summary>Windows NT/2000/XP: This member must be zero. Windows 95/98/Me: This member is not supported.</summary>
            [MarshalAs(UnmanagedType.U4)]
            public int dmPanningHeight;

            #region Shared

            // Public Shared Function FromPtr(ByVal DEVMODEptr As IntPtr) As DEVMODE
            // Return CType(Marshal.PtrToStructure(DEVMODEptr, GetType(DEVMODE)), DEVMODE)
            // End Function
            // Public Shared Function FromArray(ByVal abData() As Byte) As PrnDEVMODE
            // Dim GCH As GCHandle = GCHandle.Alloc(abData, GCHandleType.Pinned)
            // Dim ArrPtr As IntPtr = Marshal.UnsafeAddrOfPinnedArrayElement(abData, 0)
            // Dim DM As PrnDEVMODE = FromPtr(ArrPtr)
            // Call GCH.Free()
            // Return DM
            // End Function
            // Public Function SetDEVMODE(ByVal DMptr As IntPtr) As Boolean
            // If MMem.IsHandleValid(DMptr) Then
            // Call Marshal.StructureToPtr(Me, DMptr, False)
            // Return True
            // End If
            // Return False
            // End Function

            #endregion

            public int TotalSize
            {
                get
                {
                    return dmSize + dmDriverExtra;
                }
            }

        }

        #endregion

        [StructLayout(LayoutKind.Sequential , CharSet = CharSet.Auto)]
        internal struct DEVMODE_DISPLAY
        {

            // all values in this structure that specify a physical length, are in tenths of a millimeter.
            internal const int CCHDEVICENAME = 32;

            internal const int CCHFORMNAME = CCHDEVICENAME;

            /// <summary>Specifies the "friendly" name of the printer or display; for example, "PCL/HP LaserJet" in the case of PCL/HP LaserJet. This string is unique among device drivers. Note that this name may be truncated to fit in the dmDeviceName array.</summary>
            [MarshalAs(UnmanagedType.ByValTStr , SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;

            /// <summary>Specifies the version number of the initialization data specification on which the structure is based. To ensure the correct version is used for any operating system, use DM_SPECVERSION.</summary>
            [MarshalAs(UnmanagedType.U2)]
            public SpecVersions dmSpecVersion;

            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;

            public short dmOrientation;
            public short dmPaperSize;
            public short dmPaperLength;
            public short dmPaperWidth;

            public short dmScale;
            public short dmCopies;
            public short dmDefaultSource;
            public short dmPrintQuality;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            /// <summary>Windows NT/2000/XP: Specifies the name of the form to use; for example, "Letter" or "Legal". A complete set of names can be retrieved by using the EnumForms function. Windows 95/98/Me: Printer drivers do not use this member.</summary>
            [MarshalAs(UnmanagedType.ByValTStr , SizeConst = CCHFORMNAME)]
            public string dmFormName;

            public short dmLogPixels;
            public short dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;

            public int dmDisplayFlags;
            public int dmDisplayFrequency;

            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;

            public int dmPanningWidth;
            public int dmPanningHeight;

            public void Init ()
            {
                // Me.dmDeviceName = New String(Chr(0), 32)
                // Me.dmFormName = New String(Chr(0), 32)
                dmSize = (short)Marshal.SizeOf(this);
            }

        }

    }



    internal abstract class GDIObjectBase ( bool ownHandle ) : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid(ownHandle)
    {


        protected HGDIOBJ _oldObject = IntPtr.Zero;


        #region Constructor / Destructor

        public GDIObjectBase ( IntPtr H , bool ownHandle ) : this(ownHandle) { SetHandle(H); }


        protected override bool ReleaseHandle ()
        {
            var h = DangerousGetHandle();
            if ( h.isValid ) Gdi32.DeleteObject(h);
            SetHandleAsInvalid();
            return true;
        }

        #endregion



        public void SelectOnDC ( HDC hdc )
            => _oldObject = Gdi32.SelectObject(hdc , DangerousGetHandle());


        public void SelectOnDC ( HDC2 DC ) => SelectOnDC(DC.DangerousGetHandle());

        public void UnSelectOnDC ( HDC hdc )
        {
            if ( ((IntPtr)_oldObject).isValid ) Gdi32.SelectObject(hdc , _oldObject);
            _oldObject = IntPtr.Zero;
        }

        public void UnSelectOnDC ( HDC2 DC ) => UnSelectOnDC(DC.DangerousGetHandle());


        protected void ReCreateHandle ()
        {
            ReleaseHandle();
            SetHandle(CreateHandle());
        }


        protected virtual IntPtr CreateHandle () => IntPtr.Zero;


        #region Operator


        public static implicit operator IntPtr ( GDIObjectBase GOB )
            => GOB.DangerousGetHandle();

        public static implicit operator HandleRef ( GDIObjectBase GOB )
            => new(GOB , GOB.DangerousGetHandle());

        public static bool operator == ( GDIObjectBase GOB1 , GDIObjectBase GOB2 )
            => GOB1.DangerousGetHandle() == GOB2.DangerousGetHandle();

        public static bool operator != ( GDIObjectBase GOB1 , GDIObjectBase GOB2 )
            => !(GOB1 == GOB2);


        #endregion


    }


    internal class pen : GDIObjectBase
    {


        #region API

        internal enum PenStyles : int
        {
            PS_SOLID = 0,

            /// <summary>-------</summary>
            PS_DASH = 1,

            /// <summary>.......</summary>
            PS_DOT = 2,

            /// <summary>_._._._</summary>
            PS_DASHDOT = 3,

            /// <summary>_.._.._</summary>
            PS_DASHDOTDOT = 4,

            PS_NULL = 5,
            PS_INSIDEFRAME = 6,
            PS_USERSTYLE = 7,
            PS_ALTERNATE = 8,
            PS_STYLE_MASK = 0xF,
            PS_ENDCAP_ROUND = 0x0,
            PS_ENDCAP_SQUARE = 0x100,
            PS_ENDCAP_FLAT = 0x200,
            PS_ENDCAP_MASK = 0xF00,
            PS_JOIN_ROUND = 0x0,
            PS_JOIN_BEVEL = 0x1000,
            PS_JOIN_MITER = 0x2000,
            PS_JOIN_MASK = 0xF000,
            PS_COSMETIC = 0x0,
            PS_GEOMETRIC = 0x10000,
            PS_TYPE_MASK = 0xF0000
        }


        [StructLayout(LayoutKind.Sequential)]
        internal struct LOGPEN
        {
            public PenStyles lopnStyle;
            public int lopnWidth;
            public int lopnHeight;
            public int lopnColor;
        }


        #endregion


        /// <inheritdoc cref="Gdi32.CreatePen" />

        internal static Gdi32.SafeHPEN CreatePen ( PenStyles nPenStyle , int nWidth , Color crColor )
            => Gdi32.CreatePen((uint)nPenStyle , nWidth , new COLORREF(crColor));


        protected Color _color = Color.Black;
        protected int _width = 1;
        protected PenStyles _style = PenStyles.PS_SOLID;

        #region Constructor

        public pen ( Color color , int width = 1 , PenStyles style = PenStyles.PS_SOLID ) : base(true)
        {
            _color = color;
            _width = width;
            _style = style;

            ReCreateHandle();
        }

        #endregion

        protected override nint CreateHandle () => CreatePen(_style , _width , _color)
            .DangerousGetHandle();


        #region Properties
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                ReCreateHandle();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                ReCreateHandle();
            }
        }

        public PenStyles Style
        {
            get => _style;
            set
            {
                _style = value;
                ReCreateHandle();
            }
        }

        #endregion

    }


    internal sealed class brush : GDIObjectBase
    {

        #region HatchStyles

        internal enum HatchStyles : int
        {
            HS_HORIZONTAL = 0,  // -----
            HS_VERTICAL = 1, // |||||
            HS_FDIAGONAL = 2,  // \\\\\
            HS_BDIAGONAL = 3,  // ''/
            HS_CROSS = 4,  // +++++
            HS_DIAGCROSS = 5,  // xxxxx
            HS_FDIAGONAL1 = 6,
            HS_BDIAGONAL1 = 7,
            HS_SOLID = 8,
            HS_DENSE1 = 9,
            HS_DENSE2 = 10,
            HS_DENSE3 = 11,
            HS_DENSE4 = 12,
            HS_DENSE5 = 13,
            HS_DENSE6 = 14,
            HS_DENSE7 = 15,
            HS_DENSE8 = 16,
            HS_NOSHADE = 17,
            HS_HALFTONE = 18,
            HS_SOLIDCLR = 19,
            HS_DITHEREDCLR = 20,
            HS_SOLIDTEXTCLR = 21,
            HS_DITHEREDTEXTCLR = 22,
            HS_SOLIDBKCLR = 23,
            HS_DITHEREDBKCLR = 24,
            HS_API_MAX = 25
        }

        #endregion


        private Gdi32.LOGBRUSH _lgBrush;
        private readonly IntPtr _hBitmap = IntPtr.Zero;

        #region Constructor

        public brush ( Color color , Gdi32.BrushStyle style = Gdi32.BrushStyle.BS_SOLID , HatchStyles hatch = HatchStyles.HS_SOLID ) : base(true)
        {
            _lgBrush.lbColor = new COLORREF(color);
            _lgBrush.lbStyle = style;
            _lgBrush.lbHatch = (nint)hatch;

            ReCreateHandle();
        }

        public brush ( IntPtr hBitmap ) : base(true)
        {
            _hBitmap = hBitmap;
            ReCreateHandle();
        }

        protected override nint CreateHandle ()
        {
            return _hBitmap != IntPtr.Zero
                ? Gdi32.CreatePatternBrush(_hBitmap).DangerousGetHandle()
                : Style == Gdi32.BrushStyle.BS_SOLID && Hatch == HatchStyles.HS_SOLID
                ? Gdi32.CreateSolidBrush(_lgBrush.lbColor).DangerousGetHandle()
                : Gdi32.CreateBrushIndirect(_lgBrush).DangerousGetHandle();
        }

        #endregion

        #region properties

        public Color Color => (Color)_lgBrush.lbColor;

        public HatchStyles Hatch => (HatchStyles)_lgBrush.lbHatch;

        public Gdi32.BrushStyle Style => _lgBrush.lbStyle;


        #endregion

    }


    internal sealed class bitmap : GDIObjectBase
    {


        #region Constructor / destructor


        /// <summary>CreateCompatibleBitmap</summary>
        public bitmap ( HDC hdcCompat , int width , int height ) : base(true)
        {
            var h = Gdi32.CreateCompatibleBitmap(hdcCompat , width , height);
            InitFromHandle(h);
        }


        /// <summary>Create a DIB Section for this bitmap from the graphics object</summary>
        public bitmap ( IntPtr MainHDC , Gdi32.BITMAPINFO BMI , Gdi32.DIBColorMode cm = Gdi32.DIBColorMode.DIB_RGB_COLORS ) : base(true)
        {
            var dibSection = Gdi32.CreateDIBSection(MainHDC , BMI , cm , out var ppvBits , IntPtr.Zero , 0U);
            InitFromHandle(dibSection);
        }



        public void InitFromHandle ( IntPtr hBitmap )
        {
            SetHandle(hBitmap);
            if ( IsInvalid ) throw new System.ComponentModel.Win32Exception();
        }



        public void InitFromHandle ( Gdi32.SafeHBITMAP hBitmap )
            => InitFromHandle(hBitmap.DangerousGetHandle());

        #endregion


    }


    internal static class icon
    {


        #region ExtractIconEx


        /// Returns the total number Of icons In the specified file. If the file Is an eExecutable file Or DLL, the Return value Is the number Of RT_GROUP_ICON resources. If the file Is an .ico file, the Return value Is 1.</summary>
        /// <param name="sLib">name of an eExecutable file, DLL, or icon file from which icons will be extracted.</param>
        [Obsolete("!!! VERY SLOW !!!")]
        public static uint ExtractIconEx_GetIconsCount ( string file )
            => Shell32.ExtractIconEx(file , -1 , null , null , uint.MaxValue);


        public static (System.Drawing.Icon? Large, System.Drawing.Icon? Small) ExtractIconEx ( string sLib , int iIndex = 0 )
        {
            HICON[] ptrLarge = [ IntPtr.Zero ];
            HICON[] ptrSmall = [ IntPtr.Zero ];
            var result = Shell32.ExtractIconEx(sLib , iIndex , ptrLarge , ptrSmall , 1);

            System.Drawing.Icon? HandleToIcon ( HICON hIcon )
            {
                return !hIcon.IsNull ? System.Drawing.Icon.FromHandle(hIcon.DangerousGetHandle()) : null;
            }
            ;

            var rIconLarge = HandleToIcon(ptrLarge[ 0 ]);
            var rIconSmall = HandleToIcon(ptrSmall[ 0 ]);
            return (rIconLarge, rIconSmall);
        }


        /*

	public static (System.Drawing.Icon Small, System.Drawing.Icon Large)[] ExtractIconEx_GetAllIcons(string sLib, bool bGetlargeIcon = true)
	{

		var lResult = new List<(System.Drawing.Icon Small, System.Drawing.Icon Large)>();

		int iIconCount = ExtractIconEx_GetIconsCount(sLib);
		if (iIconCount > 0)
		{
			for (int iIcon = 1, loopTo = iIconCount; iIcon <= loopTo; iIcon++)
			{
				var aIcons = ExtractIconEx(sLib, iIcon - 1);
				(System.Drawing.Icon Small, System.Drawing.Icon Large) RR;
				RR.Small = aIcons.Small;
				RR.Large = aIcons.Large;
				// Dim IMG = bGetlargeIcon.IIF(rIcons.Large, rIcons.Small)
				lResult.Add(RR);
			}
		}

		(System.Drawing.Icon Small, System.Drawing.Icon Large)[] aResult = lResult.ToArray();
		return aResult;
	}

		 */


        #endregion



        /*



		[StructLayout(LayoutKind.Sequential)]
		public struct ICONINFO
		{
			public int fIcon;
			public int xHotspot;
			public int yHotspot;
			public IntPtr hbmMask;
			public IntPtr hbmColor;
		}


		/// <summary>LoadIcon loads the icon resource only if it has not been loaded; otherwise, it retrieves a handle to the existing resource. 
		/// The function searches the icon resource for the icon most appropriate for the current display. 
		/// The icon resource can be a color or monochrome bitmap.
		/// LoadIcon can only load an icon whose size conforms To the SM_CXICON And SM_CYICON system metric values. 
		/// Use the LoadImage Function To load icons Of other sizes.
		/// </summary>
		/// <param name="hInstance"></param>
		/// <param name="lpIconName"></param>
		/// <returns></returns>
		[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
		public static extern IntPtr LoadIcon(IntPtr hInstance, [MarshalAs(UnmanagedType.LPTStr)] string lpIconName);

		[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
		public static extern IntPtr CreateIconIndirect(ICONINFO piconinfo);

		#region CreateIconFromResource/Ex

		[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern IntPtr CreateIconFromResource(IntPtr pbIconBits, int cbIconBits, bool fIcon, int dwVersion = 0x30000);


		[DebuggerNonUserCode, DebuggerStepThrough]
		
		public static System.Drawing.Icon CreateIconFromResource(IntPtr hMod, string ID)
		{

			var hRsrc = Win32.Resources.mResourcesAPI.FindResource(hMod, ID, Win32.Resources.RESOURCE_TYPES.RT_ICON);

			using (var hGlobal = new Win32.Resources.Win32ResourceHandle(hMod, hRsrc))
			{
				var lpData = hGlobal.LockResource();
				int dwSize = Win32.Resources.Win32ResourceHandle.SizeofResource(hMod, hRsrc);
				var hIcon = mIconTools.CreateIconFromResource(lpData, dwSize, true);
				if (hIcon.IsInValid())
				{
					Win32.Errors.mErrors.ThrowLastWin23ErrorAssert(Win32.Errors.mErrors.Win32Errors.ERROR_SUCCESS);
					throw new Win32.Errors.mErrors.Win32Exception("CreateIconFromResourceEx Failed with unknown error!");
				}
				var rNewicon = System.Drawing.Icon.FromHandle(hIcon);
				if (rNewicon is null) throw new Exception("System.Drawing.Icon.FromHandle Failed!");
				return rNewicon;
			}
		}

		[DebuggerNonUserCode, DebuggerStepThrough]
		
		public static System.Drawing.Icon CreateIconFromResource(IntPtr hMod, int ID)
		{
			string sID = Win32.Resources.mResourcesAPI.MAKEINTRESOURCE(ID);
			return CreateIconFromResource(hMod, sID);
		}



		[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern IntPtr CreateIconFromResourceEx(byte[] pbIconBits, uint cbIconBits, bool fIcon, int dwVersion = 0x30000, int cxDesired = 0, int cyDesired = 0, Imaging.LoadImagefuLoad uFlags = Imaging.LoadImagefuLoad.LR_DEFAULTCOLOR);
		[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto)]
		private static extern IntPtr CreateIconFromResourceEx(IntPtr pbIconBits, int cbIconBits, bool fIcon, int dwVersion = 0x30000, int cxDesired = 0, int cyDesired = 0, Imaging.LoadImagefuLoad uFlags = Imaging.LoadImagefuLoad.LR_DEFAULTCOLOR);

		[DebuggerNonUserCode]
		[DebuggerStepThrough]
		
		private static System.Drawing.Icon CreateIconFromResourceEx(IntPtr hMod, string ID, Imaging.LoadImagefuLoad uFlags = Imaging.LoadImagefuLoad.LR_DEFAULTCOLOR)
		{

			var hRsrc = Win32.Resources.mResourcesAPI.FindResource(hMod, ID, Win32.Resources.RESOURCE_TYPES.RT_ICON);

			using (var hGlobal = new Win32.Resources.Win32ResourceHandle(hMod, hRsrc))
			{
				var lpData = hGlobal.LockResource();
				int dwSize = Win32.Resources.Win32ResourceHandle.SizeofResource(hMod, hRsrc);
				var hIcon = mIconTools.CreateIconFromResourceEx(lpData, dwSize, true, 196608, 0, 0, uFlags);
				if (hIcon.IsInValid())
				{
					Win32.Errors.mErrors.ThrowLastWin23ErrorAssert(Win32.Errors.mErrors.Win32Errors.ERROR_SUCCESS);
					throw new Win32.Errors.mErrors.Win32Exception("CreateIconFromResourceEx Failed with unknown error!");
				}
				var rNewicon = System.Drawing.Icon.FromHandle(hIcon);
				if (rNewicon is null)
					throw new Exception("System.Drawing.Icon.FromHandle Failed!");
				return rNewicon;
			}
		}

		[DebuggerNonUserCode]
		[DebuggerStepThrough]
		
		public static System.Drawing.Icon CreateIconFromResourceEx(IntPtr hMod, int ID, Imaging.LoadImagefuLoad uFlags = Imaging.LoadImagefuLoad.LR_DEFAULTCOLOR)
		{

			string sID = Win32.Resources.mResourcesAPI.MAKEINTRESOURCE(ID);
			return CreateIconFromResourceEx(hMod, sID, uFlags);
		}

		#endregion


		/// <summary>Retrieves information about the specified icon or cursor.</summary>
		/// <param name="hIcon">A handle to the icon or cursor.
		/// To retrieve information about a standard icon or cursor, specify one of the following values.</param>
		/// <param name="piconinfo"></param>
		/// <returns>GetIconInfo creates bitmaps for the hbmMask and hbmColor members of ICONINFO. 
		/// The calling application must manage these bitmaps and delete them when they are no longer necessary.</returns>
		[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
		public static extern bool GetIconInfo(IntPtr hIcon, ref ICONINFO piconinfo);

		public static ICONINFO? GetIconInfo(IntPtr hIcon)
		{
			var II = new ICONINFO();
			if (GetIconInfo(hIcon, ref II)) return II;

			return new global::UOMNetworkCenter.uomvb.Win32.GDI.GDIObjects.Icon.mIconTools.ICONINFO?();
		}

		#region DrawIcon

		[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
		internal static extern bool DrawIcon(HDC hdc, int X, int Y, IntPtr hIcon);

		[Flags()]
		public enum DrawIconExFlags : int
		{
			DI_MASK = 0x1,  // Draws the icon or cursor using the mask.
			DI_IMAGE = 0x2,  // Draws the icon or cursor using the image.
			DI_NORMAL = 0x3,  // Combination of DI_IMAGE and DI_MASK.
			DI_COMPAT = 0x4,  // Draws the icon or cursor using the system default image rather than the user-specified image. For more information, see About Cursors. Windows NT4.0 and later: This flag is ignored.
			DI_DEFAULTSIZE = 0x8,  // Draws the icon or cursor using the width and height specified by the system metric values for cursors or icons, if the cxWidth and cyWidth parameters are set to zero. If this flag is not specified and cxWidth and cyWidth are set to zero, the function uses the actual resource size.

			// #if(_WIN32_WINNT >= 0x0501)
			DI_NOMIRROR = 0x10  // Windows XP: Draws the icon as an unmirrored icon. By default, the icon is drawn as a mirrored icon if hdc is mirrored.

			/
	}

	[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
	// Parameters:
	// hdc 		 [in] Handle to the device context into which the icon or cursor will be drawn.
	// xLeft 		 [in] Specifies the logical x-coordinate of the upper-left corner of the icon or cursor.
	// yTop 		 [in] Specifies the logical y-coordinate of the upper-left corner of the icon or cursor.
	// hIcon 		 [in] Handle to the icon or cursor to be drawn. This parameter can identify an animated cursor.
	// cxWidth 		 [in] Specifies the logical width of the icon or cursor. If this parameter is zero and the diFlags parameter is DI_DEFAULTSIZE, the function uses the SM_CXICON or SM_CXCURSOR system metric value to set the width. If this parameter is zero and DI_DEFAULTSIZE is not used, the function uses the actual resource width.
	// cyWidth 		 [in] Specifies the logical height of the icon or cursor. If this parameter is zero and the diFlags parameter is DI_DEFAULTSIZE, the function uses the SM_CYICON or SM_CYCURSOR system metric value to set the width. If this parameter is zero and DI_DEFAULTSIZE is not used, the function uses the actual resource height.
	// istepIfAniCur 		[in] Specifies the index of the frame to draw, if hIcon identifies an animated cursor. This parameter is ignored if hIcon does not identify an animated cursor.
	// hbrFlickerFreeDraw [in] Handle to a brush that the system uses for flicker-free drawing. If hbrFlickerFreeDraw is a valid brush handle, the system creates an offscreen bitmap using the specified brush for the background color, draws the icon or cursor into the bitmap, and then copies the bitmap into the device context identified by hdc. If hbrFlickerFreeDraw is NULL, the system draws the icon or cursor directly into the device context.
	internal static extern bool DrawIconEx(HDC hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyWidth, int istepIfAniCur, IntPtr hbrFlickerFreeDraw, DrawIconExFlags flags);

	#endregion







	[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
	internal static extern bool DestroyIcon(IntPtr hIcon);

	[DllImport(Lib.USER, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
	internal static extern IntPtr CopyIcon(IntPtr hIcon);


	#region CreateCursor



	/// <summary>CreateCursor</summary>
	/// <remarks>creates a custom cursor from a bitmap</remarks>
	public static Cursor CreateCursor(System.Drawing.Bitmap bmp, int xHotspot, int yHotspot)
	{

		// Setup the Cursors IconInfo
		var II = new ICONINFO()
		{
			xHotspot = xHotspot,
			yHotspot = yHotspot,
			fIcon = 0, // False
			hbmMask = bmp.GetHbitmap(),
			hbmColor = bmp.GetHbitmap()
		};

		// Create the Pointer for the Cursor Icon
		var curPtr = CreateIconIndirect(II);
		// Clean Up
		API.DeleteObject(II.hbmMask);
		API.DeleteObject(II.hbmColor);
		return new Cursor(curPtr);
	}

		#endregion
	 */
    }


    internal sealed class font : GDIObjectBase
    {


        private const short LOGPIXELSY = 90;
        internal const short LF_FACESIZE = 32;
        internal const short LF_FULLFACESIZE = 64;


        #region Constructor / destructor


        public void InitFromHandle ( Gdi32.SafeHFONT hf )
        {
            SetHandle(hf.DangerousGetHandle());
            if ( IsInvalid )
            {
                var WEX = new System.ComponentModel.Win32Exception();
                throw WEX;
            }
        }

        protected override bool ReleaseHandle ()
        {
            if ( !IsInvalid )
            {
                bool bResult = Gdi32.DeleteObject(handle);
                if ( bResult )
                    SetHandle(IntPtr.Zero);
                return bResult;
            }
            return true;
        }


        public font ( LOGFONT LF ) : base(true)
        {
            var H = Gdi32.CreateFontIndirect(LF);
            InitFromHandle(H);
        }


        #endregion
    }


}


namespace Extensions
{



    internal static partial class Extensions_WinAPI_GDI
    {


        #region GDI sample: Drawing in the Client Area

        // LRESULT APIENTRY WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
        // {
        // PAINTSTRUCT ps;
        // HDC hdc;
        // switch (message)
        // {
        // case WM_PAINT:
        // hdc = BeginPaint(hwnd, &ps);
        // TextOut(hdc, 0, 0, "Hello, Windows!", 15);
        // EndPaint(hwnd, &ps);
        // return 0L;
        // // Process other messages.
        // }
        // }
        // int APIENTRY WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
        // {
        // HWND hwnd;
        // hwnd = CreateWindowEx(
        // // parameters
        // );
        // ShowWindow(hwnd, SW_SHOW);
        // UpdateWindow(hwnd);
        // return msg.wParam;
        // }

        #endregion


        public const uint COLOR_INVALID = 0xFFFFFFFF;
        public const ushort CLR_INVALID = 0xFFFF;



        #region ScreenShot



        internal static Bitmap GetWindowScreenShot ( this IWin32Window wnd , System.Drawing.Imaging.PixelFormat f , bool clientAreaOnly )
        {
            var rcWindow = wnd.GetWindowRect();
            using HDC2 dcSrc = new(wnd , clientAreaOnly);
            {
                Bitmap bmTarget = new(rcWindow.Width , rcWindow.Height , f);
                using ( Graphics g = Graphics.FromImage(bmTarget) )
                {
                    HDC dcTarget = g.GetHdc();
                    try
                    {
                        dcTarget.BitBlt(0 , 0 , rcWindow.Width , rcWindow.Height , dcSrc.DangerousGetHandle() , 0 , 0);
                    }
                    finally { g.ReleaseHdc((IntPtr)dcTarget); }
                }
                return bmTarget;
            }
        }


        internal static Bitmap GetWindowScreenShot ( this IntPtr hwnd , System.Drawing.Imaging.PixelFormat f , bool clientAreaOnly )
            => new Win32Window(hwnd).GetWindowScreenShot(f , clientAreaOnly);



        internal static Bitmap GetDesktopScreenShot ( System.Drawing.Imaging.PixelFormat Format = System.Drawing.Imaging.PixelFormat.Format32bppArgb )
            => GetWindowScreenShot((IntPtr)((Win32Window)User32.GetDesktopWindow()).Handle , Format , true);



        #endregion



        extension( Color source )
        {
            internal int toWin32GDIColor () => ColorTranslator.ToWin32(source);

        }


        extension( Graphics target )
        {

            /// <inheritdoc cref="BitBlt" />
            internal bool BitBlt ( int Target_X , int Target_Y , int Target_W , int Target_H , HDC hDCSource , int Source_X , int Source_Y , Gdi32.RasterOperationMode dwRop = Gdi32.RasterOperationMode.SRCCOPY )
            {
                HDC hdc = target.GetHdc();
                try
                {
                    return Gdi32.BitBlt(hdc , Target_X , Target_Y , Target_W , Target_H , hDCSource , Source_X , Source_Y , dwRop);
                }
                finally { target.ReleaseHdc((IntPtr)hdc); }
            }


        }



        extension( HDC? target )
        {

            internal bool isValid => (target != null) && (IntPtr)target.Value != IntPtr.Zero;
            internal bool isNotValid => !target.isValid;

        }
        extension( HDC hdc )
        {

            internal bool isValid => hdc != IntPtr.Zero;
            internal bool isNotValid => !hdc.isValid;



            /// <inheritdoc cref="Gdi32.BitBlt" />
            internal bool BitBlt ( int Target_X , int Target_Y , int Target_W , int Target_H , HDC hDCSource , int Source_X , int Source_Y )
                => Gdi32.BitBlt(hdc , Target_X , Target_Y , Target_W , Target_H , hDCSource , Source_X , Source_Y , Gdi32.RasterOperationMode.SRCCOPY);


            /// <inheritdoc cref="User32.DrawState" />
            internal bool DrawState ( IntPtr hBrush , User32.DrawStateProc lpDrawStateProc , IntPtr lParam , IntPtr wParam , int X , int Y , int W , int H , User32.DrawStateFlags flags )
                => User32.DrawState(hdc , hBrush , lpDrawStateProc , lParam , wParam , X , Y , W , H , flags);


            internal bool roundRect ( Rectangle RC , Size Ellipse )
                => Gdi32.RoundRect(hdc , RC.Left , RC.Top , RC.Right , RC.Bottom , Ellipse.Width , Ellipse.Height);


            #region GraphicsMode


            /// <inheritdoc cref="Gdi32.GetGraphicsMode" />
            internal Gdi32.GraphicsMode getGraphicsMode ()
                => Gdi32.GetGraphicsMode(hdc);


            /// <summary>
            /// <br/>
            /// There are three areas in which graphics output differs according to the graphics mode:<br/>
            /// Text Output: In the GM_COMPATIBLE mode, TrueType (Or vector font) text output behaves much the same way as raster font text output with respect to the world-to-device transformations in the DC.<br/>
            /// The TrueType text Is always written from left to right And right side up, even if the rest of the graphics will be flipped on the x Or y axis. <br/>
            /// Only the height of the TrueType (Or vector font) text Is scaled. <br/>
            /// The only way to write text that Is Not horizontal in the GM_COMPATIBLE mode Is to specify nonzero escapement And orientation for the logical font selected in this device context.
            /// In the GM_ADVANCED mode, TrueType (Or vector font) text output fully conforms to the world-to-device transformation in the device context. <br/>
            /// The raster fonts only have very limited transformation capabilities (stretching by some integer factors). <br/>
            /// Graphics device interface (GDI) tries to produce the best output it can with raster fonts for nontrivial transformations.<br/>
            /// Rectangle Exclusion : If the Default GM_COMPATIBLE graphics mode Is Set, the system excludes bottom And rightmost edges When it draws rectangles.<br/>
            /// The GM_ADVANCED graphics mode Is required If applications want To draw rectangles that are lower-right inclusive.<br/>
            /// Arc Drawing : If the Default GM_COMPATIBLE graphics mode Is Set, GDI draws arcs Using the current arc direction In the device space. <br/>
            /// With this convention, arcs Do Not respect page-To-device transforms that require a flip along the x Or y axis.<br/>
            /// If the GM_ADVANCED graphics mode Is Set, GDI always draws arcs In the counterclockwise direction In logical space. <br/>
            /// This Is equivalent To the statement that, In the GM_ADVANCED graphics mode, both arc control points And arcs themselves fully respect the device context's world-to-device transformation.
            /// <br/>
            /// </summary>
            /// <inheritdoc cref="Gdi32.GetGraphicsMode" />
            internal Gdi32.GraphicsMode setGraphicsMode ( Gdi32.GraphicsMode mode )
                => Gdi32.SetGraphicsMode(hdc , mode);


            #endregion


            #region ScrollDC


            /// <inheritdoc cref="User32.ScrollDC" />
            internal bool scrollDC ( int dx , int dy , PRECT? lprcScroll , PRECT? lprcClip , IntPtr hrgnUpdate , out RECT lprcUpdate )
                => User32.ScrollDC(hdc , dx , dy , lprcScroll , lprcClip , hrgnUpdate , out lprcUpdate);

            /// <inheritdoc cref="ScrollDC" />
            internal RECT? scrollDC ( int dx , int dy )
                => scrollDC(hdc , dx , dy , null , null , IntPtr.Zero , out var rcUpdate)
                ? rcUpdate
                : null;

            /// <inheritdoc cref="ScrollDC" />
            internal RECT? scrollDC ( Point offset ) => hdc.scrollDC(offset.X , offset.Y);


            #endregion


            #region Colors / BkMode


            /// <inheritdoc cref="Gdi32.SetTextColor" />
            internal COLORREF setTextColor ( COLORREF color )
                => Gdi32.SetTextColor(hdc , color);

            /// <inheritdoc cref="SetTextColor" />
            internal COLORREF setTextColor ( Color color )
                => setTextColor(hdc , new COLORREF(color));

            internal void setColors ( Color textColor , Color backColor )
            {
                setTextColor(hdc , textColor);
                if ( backColor == Color.Transparent )
                {
                    Gdi32.SetBkMode(hdc , Gdi32.BackgroundMode.TRANSPARENT);
                }
                else
                {
                    Gdi32.SetBkMode(hdc , Gdi32.BackgroundMode.OPAQUE);
                    Gdi32.SetBkColor(hdc , backColor.toWin32GDIColor());
                }
            }


            #endregion


            #region MoveToEx


            public bool moveToEx ( int x , int y )
                => Gdi32.MoveToEx(hdc , x , y , out var lppt);


            public bool moveToEx ( Point Pt )
                => hdc.moveToEx(Pt.X , Pt.Y);


            #endregion


            #region Line


            /// <inheritdoc cref="Gdi32.LineTo" />
            internal bool lineTo ( Point Pt )
                => Gdi32.LineTo(hdc , Pt.X , Pt.Y);


            /// <inheritdoc cref="Gdi32.LineTo" />
            internal void line ( Point PtFrom , Point PtTo )
            {
                hdc.moveToEx(PtFrom.X , PtFrom.Y);
                hdc.lineTo(PtTo);
            }


            /// <inheritdoc cref="Gdi32.LineTo" />
            internal void line ( int X1 , int Y1 , int X2 , int Y2 )
            {
                hdc.moveToEx(X1 , Y1);
                Gdi32.LineTo(hdc , X2 , Y2);
            }


            #endregion


            #region Text


            /// <inheritdoc cref="Gdi32.TextOut" />
            internal bool textOut ( int x , int y , string text )
                => Gdi32.TextOut(hdc , x , y , text , text.Length);


            /// <inheritdoc cref="TextOut" />
            internal bool textOut ( Point ptText , string text )
                => hdc.textOut(ptText.X , ptText.Y , text);


            /// <summary>То же, что g.MeasureString</summary>
            /// <inheritdoc cref="Gdi32.GetTextExtentPoint" />
            internal Size getTextExtentPoint ( string sText )
                => Gdi32.GetTextExtentPoint(hdc , sText , sText.Length , out var szText)
                ? szText
                : throw new Win32Exception();


            #endregion


        }






        #region Fade Bitmap

        // void CFadeWnd::OnDraw(CDC* pDC)
        // {
        // CRect rc;	GetWindowRect(rc);

        // // create a DC for the bitmap drawing
        // HDC		hBmDc	= CreateCompatibleDC(pDC->GetSafeHdc());

        // // select new bitmap into memory DC
        // HBITMAP hOldBitmap = (HBITMAP)::SelectObject(hBmDc, (HGDIOBJ)m_hNewBitmap);

        // // bitblt screen DC to memory DC
        // BitBlt(pDC->GetSafeHdc(), 0,0, rc.Width(),rc.Height(), hBmDc, 0,0, SRCCOPY);

        // // select old bitmap back into memory DC and get handle to
        // // bitmap of the screen
        // ::SelectObject(hBmDc, (HGDIOBJ)hOldBitmap);

        // DeleteDC(hBmDc);
        // }

        // // Code from:	Barretto VN
        // // found:		http://www.codeproject.com/gdi/barry_s_screen_capture.asp

        // HBITMAP CFadeWnd::CopyScreenToBitmap(LPRECT lpRect)
        // {
        // HDC		hScrDC, hMemDC; // screen DC and memory DC
        // int		nX, nY, nX2, nY2; // coordinates of rectangle to grab
        // int		nWidth, nHeight; // DIB width and height
        // int		xScrn, yScrn; // screen resolution

        // HGDIOBJ	hOldBitmap, hBitmap;

        // // check for an empty rectangle
        // if (IsRectEmpty(lpRect))
        // return NULL;

        // // create a DC for the screen and create
        // // a memory DC compatible to screen DC
        // hScrDC = CreateDC("DISPLAY", NULL, NULL, NULL);
        // hMemDC = CreateCompatibleDC(hScrDC);

        // // get points of rectangle to grab
        // nX = lpRect->left;
        // nY = lpRect->top;
        // nX2 = lpRect->right;
        // nY2 = lpRect->bottom;

        // // get screen resolution
        // xScrn = GetDeviceCaps(hScrDC, HORZRES);
        // yScrn = GetDeviceCaps(hScrDC, VERTRES);

        // // make sure bitmap rectangle is visible
        // if (nX < 0)		nX = 0;
        // if (nY < 0)		nY = 0;
        // if (nX2 > xScrn)	nX2 = xScrn;
        // if (nY2 > yScrn)	nY2 = yScrn;

        // nWidth	= nX2 - nX;
        // nHeight	= nY2 - nY;

        // // create a bitmap compatible with the screen DC
        // hBitmap = CreateCompatibleBitmap(hScrDC, nWidth, nHeight);

        // // select new bitmap into memory DC
        // hOldBitmap = SelectObject(hMemDC, hBitmap);

        // // bitblt screen DC to memory DC
        // BitBlt(hMemDC, 0, 0, nWidth, nHeight, hScrDC, nX, nY, SRCCOPY);

        // // select old bitmap back into memory DC and get handle to
        // // bitmap of the screen
        // hBitmap = SelectObject(hMemDC, hOldBitmap);

        // // clean up
        // DeleteDC(hScrDC);
        // DeleteDC(hMemDC);

        // // return handle to the bitmap
        // return (HBITMAP)hBitmap;
        // }

        // //	COLORREF	0x00bbggrr
        // //	DIBRGB		0x00rrggbb

        // #define COLORREF2DIBRGB(clr)	\
        // ((((clr) << 16) & 0x00ff0000)	|\
        // ( (clr) & 0x0000ff00)	|\
        // (((clr) >> 16) & 0x000000ff))

        // #define GetDibR(drgb)	(((drgb) & 0x00ff0000) >> 16)
        // #define GetDibG(drgb)	(((drgb) & 0x0000ff00) >> 8)
        // #define GetDibB(drgb)	 ((drgb) & 0x000000ff)

        // #define DIBRGB2COLORREF(clr)	COLORREF2DIBRGB(clr)

        // // Code from:	Dimitri Rochette drochette@ltezone.net
        // // found:		http://www.codeproject.com/bitmap/rplcolor.asp

        // HBITMAP CFadeWnd::FadeBitmap(
        // HBITMAP		hBmp,
        // double		dfTrans,
        // HDC			hBmpDC)
        // {
        // HBITMAP hRetBmp = NULL;

        // if (hBmp)
        // {
        // // DC for Source Bitmap
        // HDC hBufferDC = CreateCompatibleDC(NULL);
        // if (hBufferDC)
        // {
        // HBITMAP hTmpBitmap = (HBITMAP) NULL;
        // if (hBmpDC)
        // if (hBmp == (HBITMAP)GetCurrentObject(hBmpDC, OBJ_BITMAP))
        // {
        // hTmpBitmap = CreateBitmap(1, 1, 1, 1, NULL);
        // SelectObject(hBmpDC, hTmpBitmap);
        // }

        // // here hBufferDC contains the bitmap
        // HGDIOBJ hPrevBufObject = SelectObject(hBufferDC, hBmp);

        // HDC hDirectDC = CreateCompatibleDC(NULL);	// DC for working
        // if (hDirectDC)
        // {
        // // Get bitmap size
        // BITMAP bm;
        // GetObject(hBmp, sizeof(bm), &bm);

        // // create a BITMAPINFO with minimal initilisation for the CreateDIBSection
        // BITMAPINFO bmInfo;
        // ZeroMemory(&bmInfo,sizeof(bmInfo));
        // bmInfo.bmiHeader.biSize		= sizeof(BITMAPINFOHEADER);
        // bmInfo.bmiHeader.biWidth	= bm.bmWidth;
        // bmInfo.bmiHeader.biHeight	= bm.bmHeight;
        // bmInfo.bmiHeader.biPlanes	= 1;
        // bmInfo.bmiHeader.biBitCount	= 32;

        // UINT* ptPixels;	// pointer used for direct Bitmap pixels access

        // HBITMAP hDirectBitmap = CreateDIBSection(hDirectDC, (BITMAPINFO*)&bmInfo, DIB_RGB_COLORS,(void**)&ptPixels, NULL, 0);
        // if (hDirectBitmap)
        // {
        // // here hDirectBitmap!=NULL so ptPixels!=NULL no need to test
        // HGDIOBJ hPrevBufDirObject = SelectObject(hDirectDC, hDirectBitmap);
        // BitBlt(hDirectDC,0,0,bm.bmWidth,bm.bmHeight,hBufferDC,0,0,SRCCOPY);

        // // process loop
        // dfTrans /= 100.0;
        // double	dfTo = 1.0 - dfTrans;

        // int nSize = bm.bmWidth * bm.bmHeight;
        // for (int i=0; i<nSize; i++)
        // {
        // ptPixels[i] = RGB(
        // dfTo * GetDibB(ptPixels[i]) + dfTrans * GetDibG(ptPixels[i]),
        // dfTo * GetDibG(ptPixels[i]) + dfTrans * GetDibG(ptPixels[i]),
        // dfTo * GetDibR(ptPixels[i]) + dfTrans * GetDibG(ptPixels[i]));
        // }

        // // little clean up
        // // Don't delete the result of SelectObject because it's our modified bitmap (hDirectBitmap)
        // SelectObject(hDirectDC,hPrevBufDirObject);

        // // finish
        // hRetBmp = hDirectBitmap;
        // }

        // // clean up
        // DeleteDC(hDirectDC);
        // }

        // if (hTmpBitmap)
        // {
        // SelectObject(hBmpDC, hBmp);
        // DeleteObject(hTmpBitmap);
        // }

        // SelectObject(hBufferDC, hPrevBufObject);

        // // hBufferDC is now useless
        // DeleteDC(hBufferDC);
        // }
        // }

        // return hRetBmp;
        // }

        #endregion



        /*
		 
	

		public enum LRI_METHOD : int
		{
			ExtractIconExByIndex,
			// LoadIconByResID
			Load_RT_ICON,
			Load_RT_ICON_GROUP
		}
		internal const LRI_METHOD C_DEFAULT_LRI_METHOD = LRI_METHOD.Load_RT_ICON;

		public enum LRI_ICON_SIZE : int
		{
			Small = 0,
			Large = 1
			// Any other size will be processed to APIo
		}
		internal const LRI_ICON_SIZE C_LRI_ICON_SIZE = LRI_ICON_SIZE.Large;

		#region LoadResIcon_Safe
		private static Icon LoadResWin32Icon_Safe(int IconIndexOrResID, string sLib, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = C_LRI_ICON_SIZE, bool UseDefultIconOnError = true)
		{

			try
			{
				switch (eMethod)
				{
					case LRI_METHOD.ExtractIconExByIndex:
						{
							int iIconIndex = IconIndexOrResID;
							var LR = uomvb.Win32.GDI.GDIObjects.Icon.mIconTools.ExtractIconEx(sLib, iIconIndex);

							var rIcon = LR.Large;
							switch (eIconSize)
							{
								case LRI_ICON_SIZE.Small:
									{
										rIcon = LR.Small;
										break;
									}
								case LRI_ICON_SIZE.Large:
									{
										rIcon = LR.Large;
										break;
									}
									// Case Else
							}
							if (rIcon is null)
								throw new Exception("ExtractIconEx({0}, {1}) Failed!".format(sLib, (object)iIconIndex));
							return rIcon;
						}




					// Case LRI_METHOD.LoadIconByResID
					// Dim hIcon = uomvb.Win32.GDI.GDIObjects.Icon.LoadIcon(hLib.DangerousGetHandle, resID)




					case LRI_METHOD.Load_RT_ICON:
						{
							// Dim resID = LI.Value.ID.MAKEINTRESOURCE
							// Dim hIcon = uomvb.Win32.GDI.GDIObjects.Icon.LoadImage(hLib.DangerousGetHandle, resID, 1, iIconSize, iIconSize, 0)

							int iIconResID = IconIndexOrResID;
							var hLib = uomvb.Win32.SafeHandles.Win32LibHandle.GetRunningLibraryOrLoad_AsResource(sLib, true);
							if (!hLib.IsValid())
								throw new Exception("Failed to load '{0}' as resource lib!".format(sLib));

							int iIconSize = (int)eIconSize;
							switch (eIconSize)
							{
								case LRI_ICON_SIZE.Small:
									{
										iIconSize = SystemInformation.SmallIconSize.Width;
										break;
									}
								case LRI_ICON_SIZE.Large:
									{
										iIconSize = SystemInformation.IconSize.Width;
										break;
									}
							}
							var hIcon = uomvb.Win32.GDI.Imaging.mImagingTools.LoadImage(hLib, iIconResID, uomvb.Win32.GDI.Imaging.LoadImageuType.IMAGE_ICON, iIconSize, iIconSize);

							if (!hIcon.IsValid())
								throw new Exception("LoadImage() Failed to load resource with ID = {0} from '{1}'!".format((object)iIconResID, sLib));
							var ricon = Icon.FromHandle(hIcon);
							return ricon;
						}




					case LRI_METHOD.Load_RT_ICON_GROUP:
						{

							int iIconResID = IconIndexOrResID;
							var IDR = uomvb.Win32.Resources.mResourcesAPI.GetIconDirectory(sLib, iIconResID);
							uomvb.Win32.Resources.mResourcesAPI.GRPICONDIRENTRY[] aIconDirs = IDR.IconDirectory;
							if (!aIconDirs.Any())
								throw new Exception("GetIconDirectory({0}, {1}) Failed!".format(sLib, (object)iIconResID));


							int iIconSize = (int)eIconSize;
							switch (eIconSize)
							{
								case LRI_ICON_SIZE.Small:
									{
										iIconSize = SystemInformation.SmallIconSize.Width;
										break;
									}
								case LRI_ICON_SIZE.Large:
									{
										iIconSize = SystemInformation.IconSize.Width;
										break;
									}
							}

							var szTarget = new Size(iIconSize, iIconSize);

							aIconDirs = uomvb.Win32.Resources.mResourcesAPI.GRPICONDIRENTRY.SortByQuality(aIconDirs);

							var rDir = aIconDirs.Last();

							aIconDirs = (from R in aIconDirs
										 where (int)R.bWidth >= iIconSize && (int)R.bHeight >= iIconSize
										 select R).ToArray();

							if (aIconDirs.Any())
							{
								aIconDirs = (from R in aIconDirs
											 orderby R.bWidth descending, R.bHeight descending, R.wBitCount, R.wPlanes
											 select R).ToArray();

								rDir = aIconDirs.First();
							}

							// Dim aFound = (From R In aIconsDirectory
							// Order By R.wBitCount, R.wPlanes, R.bWidth, R.bHeight).ToArray
							// Return aFound
							var rIcon = rDir.CreateIconFromResourceEx(IDR.hLib);
							if (rIcon is null)
								throw new Exception("CreateIconFromResource({0}, RT_GROUP_ICON, {1}) Failed!".format(sLib, (object)iIconResID));
							return rIcon;
						}

					default:
						{

							throw new ArgumentOutOfRangeException("eMethod");
						}
				}
			}


			catch (Exception ex)
			{
				if (UseDefultIconOnError)
				{
					ex.eFIX_ERROR(false);
					return uomvb.mConst.ICON_SYSTEM_APP;
				}

				throw ex;
			}
		}

		internal enum LRI_Libs : int
		{
			shell32,
			imageres
		}
		internal const LRI_Libs C_DEFAULT_LRI_Libs = LRI_Libs.shell32;
		internal static Icon LoadResWin32Icon_Safe(int IconIndexOrResID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = C_LRI_ICON_SIZE, bool UseDefultIconOnError = true)
		{

			string sLib = Constants.vbNullString;
			switch (eLib)
			{
				case LRI_Libs.shell32:
					{
						sLib = uomvb.Lib.SHELL;
						break;
					}
				case LRI_Libs.imageres:
					{
						sLib = "imageres.dll";
						break;
					}

				default:
					{
						throw new ArgumentOutOfRangeException("eLib");
					}
			}

			return LoadResWin32Icon_Safe(IconIndexOrResID, sLib, eMethod, eIconSize, UseDefultIconOnError);
		}


		public enum LRI_ID : int
		{
			shell32_FileBlank = 1,
			shell32_FileDocument = 2,
			shell32_App = 3,
			shell32_Lupa = 24,
			shell32_PowerShell = 25,

			shell32_Folder_Open = 4,
			shell32_Folder_Fonts = 39,

			shell32_RecycleEmpty = 32,
			shell32_RecycleFull = 33,

			// shell32_Shield = 224
			shell32_Rename = 242,
			shell32_Question = 24, // 263
			shell32_Question3D = 324,

			shell32_Camera = 309,
			shell32_Cellphone = 310,
			shell32_PDA = 314,
			shell32_Scanner = 315,
			shell32_ScannerPhoto = 316,
			shell32_VideoCamera = 317,


			shell32_Disk_SSD = 306,
			shell32_Disk_SDCardBlue = 307,
			shell32_Disk_SDCardBlack = 308

		}
		internal static Icon LoadResWin32Icon_Safe(LRI_ID ResID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = C_LRI_ICON_SIZE, bool UseDefultIconOnError = true)
		{

			return LoadResWin32Icon_Safe((int)ResID, eLib, eMethod, eIconSize, UseDefultIconOnError);
		}












		public enum RT_ICON_PICK_MODES : int
		{
			GetFirst,

			/// <summary>GetMaxQality</summary>
			GetLast
		}


		// Function LoadResIcon_RT_GROUP_ICON_Safe(sLib As String,
		// ID As Integer,
		// Optional Modes As RT_ICON_PICK_MODES = RT_ICON_PICK_MODES.GetLast,
		// Optional UseDefultIconOnError As Boolean = True) As Global.System.Drawing.Icon

		// Try
		// Dim R = uomvb.Win32.Resources.GetIconDirectory(sLib, ID)
		// If (Not R.IconDirectory.Any) Then Throw New Exception("GetIconDirectory({0}, {1}) Failed!".format(sLib, ID))

		// Dim rIconDirectory = uomvb.Win32.Resources.GRPICONDIRENTRY.SortByQuality(R.IconDirectory)

		// Dim rDir = rIconDirectory.First
		// Select Case Modes
		// Case RT_ICON_PICK_MODES.GetLast : rDir = rIconDirectory.Last
		// Case Else : Throw New ArgumentOutOfRangeException("Modes")
		// End Select

		// Dim rIcon = rDir.CreateIcon(R.hLib.DangerousGetHandle)
		// If (rIcon Is Nothing) Then Throw New Exception("CreateIconFromResource({0}, RT_GROUP_ICON, {1}) Failed!".format(sLib, ID))

		// Return rIcon

		// Catch ex As Exception
		// If (UseDefultIconOnError) Then
		// Call ex.eFIX_ERROR(False)
		// Return uomvb.DEFAULT_APP_ICON
		// End If

		// Throw ex
		// End Try
		// End Function

		// Function LoadResIcon_RT_GROUP_ICON_Safe_Shell32(ID As Integer,
		// Optional Modes As RT_ICON_PICK_MODES = RT_ICON_PICK_MODES.GetLast,
		// Optional UseDefultIconOnError As Boolean = True) As Global.System.Drawing.Icon

		// Return LoadResIcon_RT_GROUP_ICON_Safe(uomvb.Win32.WINDLL_SHELL, ID, Modes, UseDefultIconOnError)
		// End Function






		#endregion



		
		internal static Bitmap CreateWingdingsIcon(this char C, Size szIcon, Brush brText)
		{

			string sChar = Conversions.ToString(C);
			var bmMem = new Bitmap(szIcon.Width, szIcon.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			using (var G = Graphics.FromImage(bmMem))
			{
				G.Clear(Color.Transparent);
				// .Clear(Color.Red)

				G.PageUnit = GraphicsUnit.Pixel;
				G.CompositingQuality = CompositingQuality.HighQuality;
				G.InterpolationMode = InterpolationMode.HighQualityBicubic;
				G.PixelOffsetMode = PixelOffsetMode.HighQuality;

				float sngFontSize = szIcon.Width;
				using (var fntWingdings = new Font("Wingdings 3", sngFontSize, FontStyle.Regular, GraphicsUnit.Pixel))
				{
					SizeF argretMeasuredTextSize = null;
					var ptArrow = G.MeasureStringLocation(sChar, fntWingdings, szIcon.eToRectangle().eGetCenter(), ContentAlignment.MiddleCenter, retMeasuredTextSize: ref argretMeasuredTextSize);
					G.DrawString(sChar, fntWingdings, brText, ptArrow);
				}
			}

			// Call bmMem.UOMShowImageObjectInPropertyGridDialog
			return bmMem;
		}
		internal enum WINDONGS_3_CHARS
		{
			UP_p, // = "p"
			DN_q // = "q"
		}
		internal static Bitmap CreateWingdingsIcon(WINDONGS_3_CHARS eChar, Size szIcon, Brush brText)
		{

			char C = eChar.ToString().ToCharArray().Last();
			return C.CreateWingdingsIcon(szIcon, brText);
		}







		
		internal static void TIFF_ProcessFrames(this Image imgTiff, Action<uomvb.Win32.GDI.Imaging.ProcessFrameEventArgs> FrameProcessor)
		{

			Guid[] aSizeGuids = imgTiff.FrameDimensionsList;

			// Всего кадров в файле
			int iTotalFrameInFile = 0;

			foreach (var SizeGuid in aSizeGuids)
			{
				var FD = new System.Drawing.Imaging.FrameDimension(SizeGuid);

				// Количество кадров данного размера
				int iFramesCountInDimension = imgTiff.GetFrameCount(FD);

				for (int iFrameIndexInDimension = 1, loopTo = iFramesCountInDimension; iFrameIndexInDimension <= loopTo; iFrameIndexInDimension++)
				{
					iTotalFrameInFile += 1;

					imgTiff.SelectActiveFrame(FD, iFrameIndexInDimension - 1);
					using (var smBitmapFrame = new System.IO.MemoryStream())
					{
						imgTiff.Save(smBitmapFrame, System.Drawing.Imaging.ImageFormat.Bmp);
						smBitmapFrame.Seek(0L, System.IO.SeekOrigin.Begin);

						var imgFrame = Image.FromStream(smBitmapFrame);
						using (var EA = new uomvb.Win32.GDI.Imaging.ProcessFrameEventArgs(imgFrame, iTotalFrameInFile))
						{
							FrameProcessor.Invoke(EA);
						}
					}
				}
			}
		}

		
		internal static int TIFF_GetTotalFramesCount(this Image imgTiff)
		{
			Guid[] aSizeGuids = imgTiff.FrameDimensionsList;

			int iTotalFramesCount = (from SizeGuid in aSizeGuids
									 let FD = new System.Drawing.Imaging.FrameDimension(SizeGuid)
									 let iFrames = imgTiff.GetFrameCount(FD)
									 select iFrames).Sum();

			return iTotalFramesCount;
		}

		[DebuggerNonUserCode]
		[DebuggerStepThrough]
		
		internal static Icon CreateIconFromResourceEx(this uomvb.Win32.SafeHandles.Win32LibHandle hLib, int ID, uomvb.Win32.GDI.Imaging.LoadImagefuLoad uFlags = uomvb.Win32.GDI.Imaging.LoadImagefuLoad.LR_DEFAULTCOLOR)
		{

			return uomvb.Win32.GDI.GDIObjects.Icon.mIconTools.CreateIconFromResourceEx(hLib, ID, uFlags);

		}




		[DebuggerNonUserCode]
		[DebuggerStepThrough]
		
		internal static void eDrawPathShadow(this Graphics g, GraphicsPath gp, int radius, int intensity = 100)
		{
			double alpha = 0d;
			double astep = 0d;
			double astepstep = intensity / (double)radius / (double)(radius / 2m);

			for (int thickness = radius; thickness >= 0; thickness -= 1)
			{
				var clr = Color.FromArgb((int)Math.Round(alpha), 0, 0, 0);
				using (var p = new Pen(clr, thickness))
				{
					p.LineJoin = LineJoin.Round;
					g.DrawPath(p, gp);
				}
				alpha += astep;
				astep += astepstep;
			}
		}

		 	 */
    }

}

#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning restore IDE1006 // Naming Styles
