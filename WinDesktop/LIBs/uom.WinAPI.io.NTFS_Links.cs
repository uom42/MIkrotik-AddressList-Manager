using Vanara.PInvoke;


#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.


#if WINDOWS

namespace uom
{

    namespace WinAPI
    {


        internal static partial class io
        {



            /// <summary>Жёсткая ссылка ТОЛЬКО ДЛЯ ФАЙЛОВ. </summary>
            internal static partial class HardLinks
            {


                /// <summary>Creates an enumeration of all the hard links to the specified file. The</summary>
                internal partial class HardLinksEnumeration : Kernel32.SafeSearchHandle
                {


                    private HardLinksEnumeration ( Kernel32.SafeSearchHandle hFind )
                        : base(hFind.DangerousGetHandle()) { }


                    public static (HardLinksEnumeration hFind , string FirstFile) FindFirstFile ( string path )
                    {
                        // Узнаём размер буфера
                        uint cbBuffer = 0;

                        _ = Kernel32.FindFirstFileName(path , 0 , ref cbBuffer , null)
                            .throwLastWin32ErrorIfInvalidExcept(errors.Win32Errors.ERROR_MORE_DATA);

                        StringBuilder buffer = new((int)cbBuffer + 10);
                        var hFind = Kernel32.FindFirstFileName(path , 0 , ref cbBuffer , buffer)
                            .throwLastWin32ErrorIfInvalidExcept(errors.Win32Errors.ERROR_MORE_DATA);

                        var HLE = new HardLinksEnumeration(hFind);
                        return (HLE , buffer.ToString());
                    }


                    public string? FindNextFileName ()
                    {
                        uint cbBuffer = 0;
                        StringBuilder sbLink = new((int)cbBuffer + 1);
                        Kernel32.FindNextFileName(this , ref cbBuffer , sbLink).throwLastWin32ErrorIfTrue();
                        var err = Kernel32.GetLastError();
                        var w32error = (errors.Win32Errors)(uint)err;
                        string? result = null;
                        switch ( w32error )
                        {
                            case errors.Win32Errors.ERROR_HANDLE_EOF: // No more data
                                break;

                            case errors.Win32Errors.ERROR_MORE_DATA: // has data
                                {
                                    var sbBuffer = new StringBuilder((int)cbBuffer + 10);
                                    Kernel32.FindNextFileName(this , ref cbBuffer , sbBuffer)
                                        .throwLastWin32ErrorIfFailed();

                                    result = sbBuffer.ToString();
                                    break;
                                }

                            default:
                                err.ThrowIfFailed();
                                break;
                        }
                        return result;
                    }


                    protected override bool ReleaseHandle ()
                    {
                        bool result = false;

                        if ( handle.isValid ) result = Kernel32.FindClose(handle);

                        if ( result ) SetHandle(IntPtr.Zero);

                        return result;
                    }
                }




                /// <summary>!!!ONLY FOR FILE!!! Получить имена всех жёстких ссылок, связанных с данным файлом<br/>
                /// Технически, в NTFS, любой файл - это жёсткая ссылка с одной ссылкой.<br/>
                /// Поэтому, отличить "обычный" файл от "жёсткой ссылки", можно ТОЛЬКо по количеству ссылок > 1</summary>
                public static uint GetHardLinksCount ( string file )
                {

                    using var hFile = Kernel32.CreateFile(file ,
                        Kernel32.FileAccess.GENERIC_READ ,
                        FileShare.Delete | FileShare.Read | FileShare.Write ,
                        null ,
                        FileMode.Open ,
                        FileFlagsAndAttributes.FILE_FLAG_BACKUP_SEMANTICS ,
                        IntPtr.Zero);

                    return ((HFILE)hFile).getHardLinksCount();
                }


                /// <summary>!!!ONLY FOR FILE!!! Получить имена всех жёстких ссылок, связанных с данным файлом<br/>
                /// Технически, в NTFS, любой файл - это жёсткая ссылка с одной ссылкой.<br/>
                /// Поэтому, отличить "обычный" файл от "жёсткой ссылки", можно ТОЛЬКо по количеству ссылок > 1</summary>
                public static string[] GetHardLinksNames ( string file )
                {

                    using var hFile = Kernel32.CreateFile(file ,
                        Kernel32.FileAccess.GENERIC_READ ,
                        FileShare.Delete | FileShare.Read | FileShare.Write ,
                        null ,
                        FileMode.Open ,
                        FileFlagsAndAttributes.FILE_FLAG_BACKUP_SEMANTICS ,
                        IntPtr.Zero);

                    if ( ((HFILE)hFile).getHardLinksCount() < 1 ) return [];

                    List<string> linkNames = [];

                    (HardLinksEnumeration? hFind , string? foundName) = uom.WinAPI.io.HardLinks.HardLinksEnumeration.FindFirstFile(file);
                    if ( hFind.isValid )
                    {
                        using ( hFind )
                        {
                            while ( foundName.isNotNullOrWhiteSpace )
                            {
                                linkNames.Add(foundName);
                                foundName = hFind.FindNextFileName()!;
                            }
                        }
                    }
                    return [ .. linkNames ];
                }


                /// <inheritdoc cref="Kernel32.CreateHardLink" />
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static bool Create ( string linkFile , string targetFile )
                    => Kernel32.CreateHardLink(linkFile , targetFile , null);





            }


            #region ReparsePoint

            /// <summary>The reparse tags are a DWORD. The 32 bits are laid [In,Out]as follows:<br/>
            /// <code>
            /// 3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
            /// 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
            /// +-+-+-+-+-----------------------+-------------------------------+
            /// |M|R|N|R|     Reserved bits     |       Reparse Tag Value       |
            /// +-+-+-+-+-----------------------+-------------------------------+
            /// M Is the Microsoft bit. When set to 1, it denotes a tag owned by Microsoft.
            /// All ISVs must use a tag with a 0 in this position.
            /// Note: If a Microsoft tag Is used by non-Microsoft software, the behavior Is Not defined.
            /// R Is reserved.  Must be zero for non-Microsoft tags.
            /// N Is name surrogate. When set to 1, the file represents another named entity in the system.
            /// The M And N bits are Or-able.
            /// </code>
            /// The following macros check for the M And N bit values:<br/>
            /// Macro to determine whether a reparse point tag corresponds to a tag owned by Microsoft.<br/>
            /// <code>
            /// IsReparseTagMicrosoft(tag)
            /// </code>
            /// 
            /// Macro to determine whether a reparse point tag Is a name surrogate<br/>
            /// <code>
            /// IsReparseTagNameSurrogate(tag) 
            /// </code>
            /// </summary>
            [Flags()]
            public enum REPARSE_TAG_TYPES : uint
            {
                // The following are Microsoft's predefined reparse tag values; they are defined in WinNT.h:
                // IO_REPARSE_TAG_DEDUP
                // IO_REPARSE_TAG_RESERVED_RANGE


                /// <summary>Reserved reparse tag value.</summary>
                [Description("Неизвестный")]
                IO_REPARSE_TAG_RESERVED_ZERO = 0x0U,
                /// <summary>Reserved reparse tag value.</summary>
                IO_REPARSE_TAG_RESERVED_ONE = 0x1U,

                /// <summary>Точки подключения дисковых томов и символьные связи каталогов. Технология доступна, начиная с Windows 2000.
                /// Used for mount point support, specified in section 2.1.2.5.
                /// Moiunt point or junction, see winnt.h
                /// В файловой системе NTFS (начиная с Windows NT4[1]) поддерживаются жёсткие ссылки.
                /// Жёсткая ссылка может создаваться только в пределах одного логического раздела и только для файлов.
                /// В операционных системах Windows нет возможности создать жесткую ссылку на директорию. Однако похожего эффекта можно добиться используя NTFS junction point.</summary>
                [Description("Точка монтирования")]
                IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003u,

                /// <summary>Hierarchical Storage Management - технология хранения данных, автоматически распределяющая данные между дорогими и дешёвыми накопителями.
                /// Obsolete. Used by legacy Hierarchical Storage Manager Product.</summary>
                [Description("Hierarchical Storage Manager")]
                IO_REPARSE_TAG_HSM = 0xC0000004u,
                /// <summary>Метаданные технологии Windows Home Server Drive Extender, используются для создания ссылок на файлы, продублированные на нескольких физических носителях.</summary>
                [Description("Метаданные технологии Windows Home Server Drive Extender")]
                IO_REPARSE_TAG_DRIVER_EXTENDER = 0x80000005u,
                /// <summary>Obsolete. Used by legacy Hierarchical Storage Manager Product.</summary>
                IO_REPARSE_TAG_HSM2 = 0x80000006U,
                /// <summary>Single Instance Storage (SIS) в Windows Storage Server 2008 R2 технология, увеличивающая размер дискового пространства за счёт размещения дублирующих файлов в общем хранилище.</summary>
                [Description("Single Instance Storage (SIS) в Windows Storage Server")]
                IO_REPARSE_TAG_SIS = 0x80000007U,

                /// <summary>Метаданные формата образа диска Windows Imaging Format, используемого в последних релизах ОС Windows.</summary>
                [Description("Метаданные формата образа диска Windows Imaging Format")]
                IO_REPARSE_TAG_WIM = 0x80000008U,
                /// <summary>Cluster Shared Volumes Windows Server 2008 R2 технология, позволяющая иметь диск, доступный на чтение и запись всем нодам, входящим в кластер системы виртуализации Hyper-V.</summary>
                [Description("Cluster Shared Volumes")]
                IO_REPARSE_TAG_CSV = 0x80000009U,
                /// <summary>Distributed File System (DFS) компонент Microsoft Windows, использующийся для упрощения доступа и управления файлами, физически распределёнными по сети.
                /// Used by the DFS filter. The DFS is described in the Distributed File System (DFS): Referral Protocol Specification [MS-DFSC]. Server-side interpretation only, not meaningful over the wire.</summary>
                [Description("Distributed File System (DFS)")]
                IO_REPARSE_TAG_DFS = 0x8000000AU,
                /// <summary>Used by filter manager test harness.</summary>
                IO_REPARSE_TAG_FILTER_MANAGER = 0x8000000BU,
                /// <summary>Точки повторной обработки, использующиеся в Internet Information Services(?)</summary>
                [Description("Точки повторной обработки Internet Information Services")]
                IO_REPARSE_TAG_IIS_CACHE = 0xA0000010U,
                /// <summary>Used by the DFS filter. The DFS is described in [MS-DFSC]. Server-side interpretation only, not meaningful over the wire.</summary>
                IO_REPARSE_TAG_DFSR = 0x80000012U,

                /// <summary>Used for symbolic link support. See section 2.1.2.4.<br/>
                /// Символьная ссылка - специальный файл в файловой системе, для которого не формируются никакие данные, кроме одной текстовой строки с указателем.<br/>
                /// Эта строка трактуется как путь к файлу, который должен быть открыт при попытке обратиться к данной ссылке (файлу).<br/>
                /// Символьная ссылка занимает ровно столько места в файловой системе, сколько требуется для записи её содержимого (нормальный файл занимает как минимум один блок раздела).<br/>
                /// В отличие от жёстких ссылок, могут указывать на файлы и директории в других томах.<br/>
                /// Целью ссылки может быть любой объект — например, другая ссылка, файл, папка, или даже несуществующий файл (в последнем случае при попытке открыть его должно выдаваться сообщение об отсутствии файла).<br/>
                /// Ссылка, указывающая на несуществующий файл, называется висячей.<br/>
                /// Практически символьные ссылки используются для более удобной организации структуры файлов на компьютере, так как позволяют одному файлу или каталогу иметь несколько имён, различных атрибутов и свободны от некоторых ограничений, присущих жёстким ссылкам (последние действуют только в пределах одного раздела и не могут ссылаться на каталоги).</summary>
                [Description("Символьная ссылка")]
                IO_REPARSE_TAG_SYMLINK = 0xA000000CU,

                // Used by the Network File System (NFS) component. Server-side interpretation only, not meaningful over the wire.
                [Description("Network File System (NFS)")]
                IO_REPARSE_TAG_NFS = 0x80000014U,
                IO_REPARSE_TAG_APPXSTRM = 0xC0000014U,
                IO_REPARSE_TAG_DFM = 0x80000016U,
                [Description("Linux symlink)")]
                IO_REPARSE_TAG_LX_SYMLINK = 0xA000001DU,
                IO_REPARSE_TAG_HFS = 0x8000001EU,
                /// <summary>deduplicated</summary>
                IO_REPARSE_TAG_DEDUP = 0x80000013U,
                IO_REPARSE_TAG_FILE_PLACEHOLDER = 0x80000015U,
                IO_REPARSE_TAG_WOF = 0x80000017U,
                /// <summary>Windows container</summary>
                [Description("Windows container")]
                IO_REPARSE_TAG_WCI = 0x80000018U,
                IO_REPARSE_TAG_GLOBAL_REPARSE = 0x80000019U,
                // IO_REPARSE_TAG_FILE_PLACEHOLDER),
                // IO_REPARSE_TAG_APPEXECLINK),
                // IO_REPARSE_TAG_WCI_TOMBSTONE),
                // IO_REPARSE_TAG_UNHANDLED),
                // IO_REPARSE_TAG_ONEDRIVE),
                // IO_REPARSE_TAG_GVFS_TOMBSTONE),
                IO_REPARSE_TAG_GVFS = 0x9000001CU,
                IO_REPARSE_TAG_CLOUD = 0x9000001AU,
                IO_REPARSE_TAG_CLOUD_1 = 0x9000101AU,
                // { =&h9000201A, "IO_REPARSE_TAG_CLOUD_2" },
                // { =&h9000301A, "IO_REPARSE_TAG_CLOUD_3" },
                // { =&h9000401A, "IO_REPARSE_TAG_CLOUD_4" },
                // { =&h9000501A, "IO_REPARSE_TAG_CLOUD_5" },
                // { =&h9000601A, "IO_REPARSE_TAG_CLOUD_6" },
                // { =&h9000701A, "IO_REPARSE_TAG_CLOUD_7" },
                // { =&h9000801A, "IO_REPARSE_TAG_CLOUD_8" },
                // { =&h9000901A, "IO_REPARSE_TAG_CLOUD_9" },
                // { =&h9000A01A, "IO_REPARSE_TAG_CLOUD_A" },
                // { =&h9000B01A, "IO_REPARSE_TAG_CLOUD_B" },
                // { =&h9000C01A, "IO_REPARSE_TAG_CLOUD_C" },
                // { =&h9000D01A, "IO_REPARSE_TAG_CLOUD_D" },
                // { =&h9000E01A, "IO_REPARSE_TAG_CLOUD_E" },
                // { =&h9000F01A, "IO_REPARSE_TAG_CLOUD_F" },


                IsReparseTagMicrosoft = 0x80000000U,
                TagIsReparseTagNameSurrogate = 0x20000000U
                // IO_REPARSE_TAG_VALID_VALUES = 0xF000FFFFui
            }


            /// <summary>Provides access to NTFS Symlinks or Junction Points in .Net.</summary>
            internal partial class ReparsePoint
            {


                #region API

                /// <summary>Maximum allowed size of the reparse data.</summary>
                private const string
                    NON_INTERPRETED_PATH_PREFIX = @"\??\",
                    VOLUME_MOUNT_POINT = NON_INTERPRETED_PATH_PREFIX + "Volume";


                #region REPARSE BUFFERs


                private const int CHAR_BUFFER_SIZE = 0x3FF0;


                [StructLayout(LayoutKind.Explicit , Size = 4 + (2 * 6) + CHAR_BUFFER_SIZE , Pack = 4)]
                internal struct REPARSE_GUID_DATA_BUFFER
                {
                    [FieldOffset(0)] public REPARSE_TAG_TYPES ReparseTag;
                    [FieldOffset(4)] public ushort ReparseDataLength;
                    [FieldOffset(6)] public ushort Reserved;

                    [FieldOffset(8)] public Guid ReparseGuid; //A GUID is a 128-bit integer (16 bytes) that can be used across all
                    [FieldOffset(24)] public GENERIC_REPARSE_BUFFER GenericReparseBuffer;
                }

                [StructLayout(LayoutKind.Explicit , Pack = 4)]
                internal struct GENERIC_REPARSE_BUFFER
                {
                    [FieldOffset(0)] public byte DataBuffer;
                }

                [StructLayout(LayoutKind.Explicit , Pack = 4)]
                internal struct REPARSE_DATA_BUFFER
                {
                    [FieldOffset(0)] public REPARSE_TAG_TYPES ReparseTag;
                    [FieldOffset(4)] public ushort ReparseDataLength;
                    [FieldOffset(6)] public ushort Reserved;

                    [FieldOffset(8)] public SYM_LINK_REPARSE_BUFFER SymbolicLinkReparseBuffer;
                    [FieldOffset(8)] public MOUNT_POINT_REPARSE_BUFFER MountPointReparseBuffer;
                }

                [StructLayout(LayoutKind.Explicit , Pack = 4)]
                internal struct SYM_LINK_REPARSE_BUFFER
                {
                    [FieldOffset(0)] public ushort SubstituteNameOffset;
                    [FieldOffset(2)] public ushort SubstituteNameLength;
                    [FieldOffset(4)] public ushort PrintNameOffset;
                    [FieldOffset(6)] public ushort PrintNameLength;
                    [FieldOffset(8)] public uint Flags;//#define SYMLINK_FLAG_RELATIVE   1
                    /// <summary>Contains the SubstituteName and the PrintName. The SubstituteName is the path of the target directory.</summary>
                    [FieldOffset(12)] public byte PathBuffer;
                }

                [StructLayout(LayoutKind.Explicit , Pack = 4)]
                internal struct MOUNT_POINT_REPARSE_BUFFER
                {
                    [FieldOffset(0)] public ushort SubstituteNameOffset;
                    [FieldOffset(2)] public ushort SubstituteNameLength;
                    [FieldOffset(4)] public ushort PrintNameOffset;
                    [FieldOffset(6)] public ushort PrintNameLength;
                    /// <summary>Contains the SubstituteName and the PrintName. The SubstituteName is the path of the target directory.</summary>
                    [FieldOffset(8)] public byte PathBuffer;
                }


                #endregion


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private static Kernel32.SafeHFILE OpenReparsePoint ( string reparsePoint , Kernel32.FileAccess accessMode = Kernel32.FileAccess.GENERIC_READ )
                {
                    var reparsePointHandle = Kernel32.CreateFile(reparsePoint ,
                        accessMode ,
                        FileShare.Read | FileShare.Write | FileShare.Delete ,
                        null ,
                        FileMode.Open , FileFlagsAndAttributes.FILE_FLAG_BACKUP_SEMANTICS | FileFlagsAndAttributes.FILE_FLAG_OPEN_REPARSE_POINT ,
                        IntPtr.Zero);

                    return reparsePointHandle.IsInvalid
                        ? throw new Win32Exception()
                        : reparsePointHandle;
                }


                #endregion


                public enum MOUNT_POINT_TYPE : uint
                {
                    UNKNOWN = 0,
                    VOLUME_MOUNT_POINT,
                    DIRECTORY_JUNCTION_POINT
                }

                public readonly string Path = string.Empty;
                public readonly REPARSE_TAG_TYPES TagType = REPARSE_TAG_TYPES.IO_REPARSE_TAG_RESERVED_ZERO;
                public readonly MOUNT_POINT_TYPE MountPointType = MOUNT_POINT_TYPE.UNKNOWN;
                public readonly bool IsKnownType;
                public readonly string Target = string.Empty;
                public readonly bool IsRelative;





                #region Constructor

                /// <summary>Takes a full path to a reparse point and finds the target.</summary>
                /// <param name="path">Full path of the reparse point</param>
                public unsafe ReparsePoint ( string path , bool errorOnUnknownTag = true ) : base()
                {
                    Path = path;

                    const char C_SLASH = '\\';

                    _ = TryGetReparseData(path , out REPARSE_GUID_DATA_BUFFER rgdb);

                    TagType = rgdb.ReparseTag;
                    if ( TagType.isReparseTagMicrosoft )
                    {
                        REPARSE_DATA_BUFFER* rdb = (REPARSE_DATA_BUFFER*)&rgdb;
                        switch ( TagType )
                        {
                            case REPARSE_TAG_TYPES.IO_REPARSE_TAG_SYMLINK:
                                {
                                    if ( path.EndsWith(C_SLASH) ) path = path[ 0..^1 ];

                                    var pathBuffer = new Span<byte>(
                                        &rdb->SymbolicLinkReparseBuffer.PathBuffer ,
                                        rdb->SymbolicLinkReparseBuffer.SubstituteNameOffset + rdb->SymbolicLinkReparseBuffer.SubstituteNameLength
                                        );

                                    if ( rdb->SymbolicLinkReparseBuffer.SubstituteNameOffset > 0 ) pathBuffer = pathBuffer[ rdb->SymbolicLinkReparseBuffer.SubstituteNameOffset.. ];


                                    Target = pathBuffer.toStringUni();
                                    IsRelative = rdb->SymbolicLinkReparseBuffer.Flags == 1;
                                    IsKnownType = true;

                                    break;
                                }

                            case REPARSE_TAG_TYPES.IO_REPARSE_TAG_MOUNT_POINT:
                                {
                                    // This could be a junction or a mounted drive
                                    // a mounted drive starts with "\??\X:\"

                                    var pathBuffer = new Span<byte>(
                                        &rdb->MountPointReparseBuffer.PathBuffer ,
                                        rdb->MountPointReparseBuffer.SubstituteNameOffset + rdb->MountPointReparseBuffer.SubstituteNameLength
                                        );

                                    if ( rdb->MountPointReparseBuffer.SubstituteNameOffset > 0 ) pathBuffer = pathBuffer[ rdb->MountPointReparseBuffer.SubstituteNameOffset.. ];

                                    var target = pathBuffer.toStringUni();

                                    var printNameBuffer = new Span<byte>(
                                        &rdb->MountPointReparseBuffer.PathBuffer ,
                                        rdb->MountPointReparseBuffer.PrintNameOffset + rdb->MountPointReparseBuffer.PrintNameLength
                                        );

                                    if ( rdb->MountPointReparseBuffer.PrintNameOffset > 0 ) printNameBuffer = printNameBuffer[ rdb->MountPointReparseBuffer.PrintNameOffset.. ];
                                    var printName = printNameBuffer.toStringUni();

                                    MountPointType = target.StartsWith(VOLUME_MOUNT_POINT , StringComparison.OrdinalIgnoreCase)
                                        ? MOUNT_POINT_TYPE.VOLUME_MOUNT_POINT
                                        : MOUNT_POINT_TYPE.DIRECTORY_JUNCTION_POINT;

                                    if ( MountPointType == MOUNT_POINT_TYPE.VOLUME_MOUNT_POINT )
                                    {
                                        //target = target[ VOLUME_MOUNT_POINT.Length.. ];
                                    }
                                    IsKnownType = true;
                                    Target = target;

                                    break;
                                }

                            default:
                                {
                                    if ( errorOnUnknownTag ) throw new IOException($"Unknown REPARSE_TAG: {TagType}!");
                                    /*
									// Debug.Assert(outBuffer.ReparseTag = REPARSE_GUID_DATA_BUFFER.TAG_TYPES.IO_REPARSE_TAG_SYMLINK OrElse outBuffer.ReparseTag = REPARSE_GUID_DATA_BUFFER.TAG_TYPES.IO_REPARSE_TAG_MOUNT_POINT, "Unrecognised reparse tag")
									 */

                                    break;
                                }
                        }

                        if ( !IsKnownType ) return;

                        if ( Target.StartsWith(NON_INTERPRETED_PATH_PREFIX) ) Target = Target[ NON_INTERPRETED_PATH_PREFIX.Length.. ];

                        // Remove any final slash for consistency
                        while ( Target.EndsWith(C_SLASH) ) Target = Target.TrimEnd(C_SLASH);

                        // If (Not String.IsNullOrEmpty(printString)) Then
                        // _NormalisedTarget = printString
                        // Else
                        // if not we can use the substring with a bit of tweaking
                        // _NormalisedTarget = subsString
                        // Debug.Assert(_NormalisedTarget.Length > 2, "Target string too short")
                        // Debug.Assert(
                        // (_NormalisedTarget.StartsWith("\??\") AndAlso (_NormalisedTarget(5) = ":" OrElse _NormalisedTarget.StartsWith("\??\Volume")) OrElse
                        // (Not _NormalisedTarget.StartsWith("\??\") AndAlso _NormalisedTarget(1)<>":")), "Malformed subsString")

                        //  Junction points must be absolute
                        // Debug.Assert(
                        // buffer.ReparseTag = IO_REPARSE_TAG_SYMLINK OrElse
                        // _NormalisedTarget.StartsWith("\??\Volume") OrElse
                        // _NormalisedTarget(1) = ":", "Relative junction point")
                        // If (_NormalisedTarget.StartsWith("\??\")) Then
                        // _NormalisedTarget = _NormalisedTarget.Substring(4)
                        // End If
                        // End If
                        // _ActualTarget = _NormalisedTarget

                    }
                }


                /// <summary>
                /// Получаем буфер данных Reparse Point через Win32 API (FSCTL_GET_REPARSE_POINT)
                /// </summary>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                internal static bool TryGetReparseData ( string path , out REPARSE_GUID_DATA_BUFFER buffer )
                {
                    buffer = default;
                    try
                    {
                        Debug.Assert(path.isNotNullOrWhiteSpace && path.Length > 2 && Convert.ToString(path[ 1 ]) == ":" && Convert.ToString(path[ 2 ]) == @"\");

                        uom.AppTools.AdjustProcessPrivilegeAndCloseToken(Vanara.Security.AccessControl.SystemPrivilege.Backup);

                        // Open the file and get its handle
                        using var hFile = OpenReparsePoint(path);

                        if ( !hFile.isValid ) return false;

                        Vanara.PInvoke.SizeT bytesReturned = 0;
                        return Kernel32.DeviceIoControl<REPARSE_GUID_DATA_BUFFER>(
                            hFile ,
                            Kernel32.IOControlCode.FSCTL_GET_REPARSE_POINT ,
                            out buffer ,
                            bytesReturned);
                    }
                    catch
                    {
                        return false;
                    }
                }


                #endregion



                /// <summary>Note that if the symlink target path contains any ..s these are not normalised but returned as is.</summary>
                public string TargetAbsolute
                {
                    get
                    {
                        if ( Target.isNullOrWhiteSpace ) return "";

                        var sAbsolute = Target;

                        switch ( TagType )
                        {
                            case REPARSE_TAG_TYPES.IO_REPARSE_TAG_SYMLINK:
                                {
                                    if ( sAbsolute.StartsWith(NON_INTERPRETED_PATH_PREFIX) ) sAbsolute = Target.Substring(NON_INTERPRETED_PATH_PREFIX.Length);
                                    // Symlinks can be relative.
                                    if ( sAbsolute.Length < 2 || sAbsolute[ 1 ] != ':' )
                                    {
                                        // it's relative, we need to tack it onto the path
                                        if ( sAbsolute[ 0 ] == '\\' ) sAbsolute = sAbsolute.Substring(1);
                                        //Need to take the symlink name off the path
                                        sAbsolute = Path.Substring(0 , Path.LastIndexOf(@"\")) + @"\" + TargetAbsolute;
                                        // Note that if the symlink target path contains any ..s these are not normalised but returned as is.
                                    }
                                    return sAbsolute;
                                }

                            case REPARSE_TAG_TYPES.IO_REPARSE_TAG_MOUNT_POINT:
                                {
                                    return sAbsolute;
                                }

                            default: { return ""; };
                        }
                    }
                }


                public override string ToString ()
                {
                    var s = $"{TagType}:";
                    if ( TagType == REPARSE_TAG_TYPES.IO_REPARSE_TAG_MOUNT_POINT ) s += $"{MountPointType}:";
                    s += $" {Path} -> {Target}, IsRelative: {IsRelative}";
                    return s;
                }










                public enum NewTagType : int
                {
                    [Description("Неизвестный")]
                    None = 0,

                    /// <summary>В файловой системе NTFS (начиная с Windows NT4[1]) поддерживаются жёсткие ссылки. Жёсткая ссылка может создаваться только в пределах одного логического раздела и только для файлов. В операционных системах Windows нет возможности создать жесткую ссылку на директорию. Однако похожего эффекта можно добиться используя NTFS junction point.</summary>
                    [Description("Точка монтирования диска")]
                    MountPoint = 1,

                    /// <summary>Символьная ссылка - специальный файл в файловой системе, для которого не формируются никакие данные, кроме одной текстовой строки с указателем. Эта строка трактуется как путь к файлу, который должен быть открыт при попытке обратиться к данной ссылке (файлу). Символьная ссылка занимает ровно столько места в файловой системе, сколько требуется для записи её содержимого (нормальный файл занимает как минимум один блок раздела).
                    /// В отличие от жёстких ссылок, могут указывать на файлы и директории в других томах
                    /// Целью ссылки может быть любой объект — например, другая ссылка, файл, папка, или даже несуществующий файл (в последнем случае при попытке открыть его должно выдаваться сообщение об отсутствии файла). Ссылка, указывающая на несуществующий файл, называется висячей.
                    /// Практически символьные ссылки используются для более удобной организации структуры файлов на компьютере, так как позволяют одному файлу или каталогу иметь несколько имён, различных атрибутов и свободны от некоторых ограничений, присущих жёстким ссылкам (последние действуют только в пределах одного раздела и не могут ссылаться на каталоги).</summary>
                    [Description("Символическая ссылка")]
                    SymbolicLink = 2,


                    /// <summary>Точка соединения ТОЛЬКО ДЛЯ ПАПОК!. Для файлов надо использовать HardLink (Жёсткая ссылка)</summary>
                    [Description("Точка соединения")]
                    JunctionPoint = 3
                }


                /*
				/// <summary>Creates a junction point from the specified directory to the specified target directory.</summary>
				/// <remarks>Only works on NTFS. (https://msdn.microsoft.com/en-us/library/aa365006%28v=vs.85%29.aspx)</remarks>
				/// <param name="locationPath">The junction point path</param>
				/// <param name="targetPath">The target file or directory</param>
				/// <param name="overwrite">If true overwrites an existing reparse point or empty directory</param>
				/// <exception cref="IOException">Thrown when the junction point could not be created or when an existing directory was found and <paramref name="overwrite" /> if false</exception>
				public static void Create ( string locationPath, string targetPath, NewTagType eTagType, bool overwrite )
				{
					targetPath = System.IO.Path.GetFullPath (targetPath);
					bool targetIsDirectory = targetPath.PathIsDirectory ();
					switch (eTagType)
					{
						case NewTagType.JunctionPoint:
							{
								if (!targetIsDirectory) throw new Exception ("JunctionPoint Valid Only For Directories! For Files use HardLinks instead.");
								if (!Directory.Exists (targetPath)) errors.ThrowWin23Error (errors.Win32Errors.ERROR_FILE_NOT_FOUND);
								if (Directory.Exists (locationPath))
								{
									if (!overwrite) throw new IOException ($"Directory '{locationPath}' already exists and '{nameof (overwrite)}'={overwrite}.");
								}
								else Directory.CreateDirectory (locationPath);

								string S1 = NON_INTERPRETED_PATH_PREFIX + targetPath;
								string S2 = targetPath;
								string SSS = S1 + '\0' + S2;
								var aTargetDirBytes = Encoding.Unicode.GetBytes (SSS);
								using var hReparsePoint = OpenReparsePoint (locationPath, Kernel32.FileAccess.GenericWrite);
								try
								{
									var InBuffer = new REPARSE_GUID_DATA_BUFFER ();
									{
										//var withBlock = InBuffer;
										InBuffer.ReparseTag = REPARSE_TAG_TYPES.IO_REPARSE_TAG_MOUNT_POINT;
										InBuffer.SubstituteNameOffset = 0;
										InBuffer.SubstituteNameLength = (UInt16) ( S1.Length * 2 );
										InBuffer.PrintNameOffset = (UInt16) ( InBuffer.SubstituteNameOffset + InBuffer.SubstituteNameLength + 2 );
										InBuffer.PrintNameLength = (UInt16) ( S2.Length * 2 );
										InBuffer.PathBuffer = REPARSE_GUID_DATA_BUFFER.EMPTY_CHAR_BUFFER;
										Array.Copy (aTargetDirBytes, 0, InBuffer.PathBuffer, 0, aTargetDirBytes.Length);
										InBuffer.ReparseDataLength = (UInt16) ( InBuffer.SubstituteNameLength + InBuffer.PrintNameLength + 12 );
									}

									int dwBytesToSet = InBuffer.ReparseDataLength + REPARSE_GUID_DATA_BUFFER.REPARSE_DATA_BUFFER_HEADER_SIZE;
									int bytesReturned = 0;
									bool result = REPARSE_GUID_DATA_BUFFER.DeviceIoControl (
										hReparsePoint.DangerousGetHandle (),
										REPARSE_GUID_DATA_BUFFER.FSCTL_COMMANDS.FSCTL_SET_REPARSE_POINT,
										ref InBuffer,
										dwBytesToSet,
										IntPtr.Zero,
										0,
										ref bytesReturned,
										IntPtr.Zero);

									result.eThrowLastWin32Exception_AssertFalse ("DeviceIoControl");
								}
								finally { hReparsePoint.Close (); }
								break;
							}

						case NewTagType.SymbolicLink: // Символьная ссылка
							{
								var SFF = targetIsDirectory ? SYM_LINK_FLAG.SYMBOLIC_LINK_FLAG_DIRECTORY : SYM_LINK_FLAG.SYMBOLIC_LINK_FLAG_FILE;
								bool bResult = CreateSymbolicLink (locationPath, targetPath, SFF);
								bResult.eThrowLastWin32Exception_AssertFalse ("CreateSymbolicLink");
								break;
							}

						default:
							throw new NotImplementedException ();
					}
				}

				 */


                #region Delete


                //<summary>
                // '' Deletes a junction point at the specified source directory along with the directory itself.
                // '' Does nothing if the junction point does not exist.
                // '' </summary>
                // '' <remarks>
                // '' Only works on NTFS.
                // '' </remarks>
                // '' <param name="junctionPoint">The junction point path</param>
                // public  shared   sub  Delete(string junctionPoint)
                // {
                // If (!Directory.Exists(junctionPoint)) Then
                // {
                // If (File.Exists(junctionPoint)) Then
                // Throw New IOException("Path is not a junction point.")

                // Return
                // }

                // using (SafeFileHandle handle = OpenReparsePoint(junctionPoint, EFileAccess.GenericWrite))
                // {
                // REPARSE_DATA_BUFFER(reparseDataBuffer = New REPARSE_DATA_BUFFER())

                // reparseDataBuffer.ReparseTag = IO_REPARSE_TAG_MOUNT_POINT
                // reparseDataBuffer.ReparseDataLength = 0
                // reparseDataBuffer.PathBuffer = new byte<0x3ff0> 

                // int(inBufferSize = Marshal.SizeOf(reparseDataBuffer))
                // IntPtr(inBuffer = Marshal.AllocHGlobal(inBufferSize))
                // Try
                // {
                // Marshal.StructureToPtr(reparseDataBuffer, inBuffer, False)

                // int(bytesReturned)
                // bool result = DeviceIoControl(handle.DangerousGetHandle(), FSCTL_DELETE_REPARSE_POINT,
                // inBuffer, 8, IntPtr.Zero, 0, [In,Out]bytesReturned, IntPtr.Zero) 

                // If (!result) Then
                // ThrowLastWin32Error("Unable to delete junction point.")
                // }
                // finally
                // {
                // Marshal.FreeHGlobal(inBuffer)
                // }

                // Try
                // {
                // Directory.Delete(junctionPoint)
                // }
                // catch (IOException ex)
                // {
                // Throw New IOException("Unable to delete junction point.", ex)
                // }
                // }
                // }
                #endregion

                #region Exists


                //<summary>
                // '' Determines whether the specified path exists and refers to a junction point.
                // '' </summary>
                // '' <param name="path">The junction point path</param>
                // '' <returns>True if the specified path represents a junction point</returns>
                // '' <exception cref="IOException">Thrown if the specified path is invalid
                // '' or some other error occurs</exception>
                // public  shared  bool Exists(string path)
                // {
                // If (!Directory.Exists(path)) Then
                // Return False

                // using (SafeFileHandle handle = OpenReparsePoint(path, EFileAccess.GenericRead))
                // {
                // string target = InternalGetTarget(handle) 
                // return target <> null 
                // }
                // }
                #endregion

            }

            #endregion


        }


    }



    namespace Extensions
    {

        internal static partial class Extensions_WinAPI_IO_NTFS
        {



            extension( uom.WinAPI.io.REPARSE_TAG_TYPES tag )
            {


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public uom.WinAPI.io.REPARSE_TAG_TYPES Trim ()
                {
                    const uint SIMPLY_TAG_MASK = 0x5FFFFFFFu;
                    uint B = (uint)tag & SIMPLY_TAG_MASK;
                    return (uom.WinAPI.io.REPARSE_TAG_TYPES)B;
                }



                #region The following macros check for the M And N bit values:

                /// <summary>Determine whether a reparse point tag corresponds to a tag owned by Microsoft.<br/>
                /// <code>
                /// #define IsReparseTagMicrosoft(_tag) (_tag) & 0x80000000)</summary>
                /// </code>
                /// </summary>
                public bool isReparseTagMicrosoft => ((uint)tag & 0x80000000u) != 0;


                /// <summary>Determine whether a reparse point tag Is a name surrogate
                /// <code>			
                /// #define IsReparseTagNameSurrogate(_tag) (((_tag) & 0x20000000)   \
                /// </code>
                /// </summary>
                public bool isReparseTagNameSurrogate => ((uint)tag & 0x20000000) != 0;




                #endregion

            }



            extension( string path )
            {


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public uom.WinAPI.io.ReparsePoint pathGetReparsePoint () => new(path);



                /// <summary>
                /// Проверка на специфические префиксы оконных станций в Windows
                /// Обычно это объекты типа \Windows\WindowStations\...
                /// </summary>
                internal bool pathIsWindowStation ()
                {
                    if ( path.isNullOrEmpty ) return false;

                    // Проверка на специфические префиксы оконных станций в Windows
                    // Обычно это объекты типа \Windows\WindowStations\...
                    return path.StartsWith(@"\\.\" , StringComparison.OrdinalIgnoreCase) ||
                           path.StartsWith(@"\??\" , StringComparison.OrdinalIgnoreCase);
                }

            }


            extension( FileSystemInfo fsi )
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public uom.WinAPI.io.ReparsePoint getReparsePoint ()
                    => fsi.FullName.pathGetReparsePoint();


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                internal string symLinkToString ()
                {
                    if ( !fsi!.isSymLinkMP ) return fsi!.ToString();
                    var s = $"SymLink '{fsi.removeLongPathPrefix()}'";
                    try
                    {
                        if ( fsi!.LinkTarget is not null ) s += $" => '{fsi.LinkTarget}'";
                    }
                    catch { }
                    return s;
                }


                internal uom.I_O.LinkTypes getLinkType ()
                {
                    {
                        if ( fsi is null ) return I_O.LinkTypes.Unknown;

                        try
                        {

                            if ( OSInfo.IsWindows )
                            {

                                switch ( fsi )
                                {
                                    case FileInfo fi:
                                        {
                                            //if ( (fsi is FileInfo fi) && (fi.hardLinksCount > 1) ) return I_O.LinkTypes.HardLink;
                                            if ( fi.hardLinksCount > 1 ) return I_O.LinkTypes.HardLink;
                                            break;

                                            /*
                                            if ( fsi.Attributes.HasFlag(FileAttributes.ReparsePoint) )
                                            {
                                                // В Windows проверяем на HardLink (жесткую ссылку), так как они не являются ReparsePoint
                                                return I_O.LinkTypes.Unknown;
                                            }
                                             */
                                        }

                                    // Проверка, является ли объект ссылкой (ReparsePoint)
                                    case DirectoryInfo di:
                                        {
                                            // Если это ReparsePoint, определяем конкретный тип
                                            //if ( fsi.Attributes.HasFlag(FileAttributes.ReparsePoint) )
                                            {

                                                var lt = di.reparselinkType;
                                                switch ( di.reparselinkType )
                                                {
                                                    case WinAPI.io.REPARSE_TAG_TYPES.IO_REPARSE_TAG_MOUNT_POINT:
                                                        return I_O.LinkTypes.Junction;

                                                    case WinAPI.io.REPARSE_TAG_TYPES.IO_REPARSE_TAG_LX_SYMLINK:
                                                    case WinAPI.io.REPARSE_TAG_TYPES.IO_REPARSE_TAG_SYMLINK:
                                                        return I_O.LinkTypes.SymLink;
                                                }

                                            }
                                            // Если тег другой (например, облачный файл OneDrive), LinkType останется null
                                        }
                                        break;

                                }


                                if ( fsi.FullName.pathIsWindowStation() ) return I_O.LinkTypes.Unknown;


                            }
                            return I_O.LinkTypes.Unknown;
                        }
                        catch
                        {
                            return I_O.LinkTypes.Unknown;
                        }

                    }

                }



            }

            extension( HFILE hFile )
            {

                /// <inheritdoc cref="Kernel32.BY_HANDLE_FILE_INFORMATION.nNumberOfLinks" />
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public uint getHardLinksCount ()
                    => Kernel32.GetFileInformationByHandle(hFile , out var fi)
                    ? fi.nNumberOfLinks
                    : 0;


            }
            extension( FileInfo file )
            {

                /// <inheritdoc cref="uom.WinAPI.io.HardLinks.GetHardLinksNames" />
                public string[] GetHardLinksNames ()
                    => uom.WinAPI.io.HardLinks.GetHardLinksNames(file.FullName);


                /// <inheritdoc cref="Kernel32.BY_HANDLE_FILE_INFORMATION.nNumberOfLinks" />
                internal int hardLinksCount
                {

                    get
                    {
                        try
                        {
                            return (int)uom.WinAPI.io.HardLinks.GetHardLinksCount(file.FullName);
                        }
                        catch { return -1; }
                    }
                }

            }


            extension( DirectoryInfo folder )
            {

                internal WinAPI.io.REPARSE_TAG_TYPES? reparselinkType
                    => uom.WinAPI.io.ReparsePoint.TryGetReparseData(folder.FullName , out var b)
                    ? b.ReparseTag
                    : null;

            }




        }

    }


}


#endif

#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning restore IDE1006 // Naming Styles
