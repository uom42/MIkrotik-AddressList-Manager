#nullable enable

//using uom.mem;

using Vanara.InteropServices;
using Vanara.PInvoke;


#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.


namespace uom
{


    namespace WinAPI
    {


        internal static partial class io
        {


            #region NTFSStreamInfo


            /// <summary>Represents the details of an alternative data stream.</summary>
            [DebuggerDisplay("{FullPath}")]
            internal unsafe partial class NTFSStreamInfo
            {
                // Implements  IEquatable(of  <NTFSStreamInfo>

                public const char STREAM_SEPARATOR = ':';
                public const int DEFAULT_BUFFER_SIZE = 0x1000;

                public const int PATH_LIMIT = Kernel32.MAX_PATH - 4; // 256I
                public const string FULL_STREAM_PATH_SUFFIX = @":$DATA";
                /// <summary>http:'msdn.microsoft.com/en-us/library/aa365247(v=VS.85).aspx</summary></summary>					
                public static readonly char[] InvalidStreamNameChars = System.IO.Path.GetInvalidFileNameChars();


                #region API

                /// <summary>Fixed version of <see cref="Kernel32.WIN32_STREAM_ID"/></summary>
                [StructLayout(LayoutKind.Sequential , Pack = 4 , Size = 20)]
                //[StructLayout (LayoutKind.Sequential, Pack = 4)]
                public struct WIN32_STREAM_ID_HEADER2222222
                {
                    public Kernel32.BACKUP_STREAM_ID dwStreamId;
                    public Kernel32.BACKUP_STREAM_ATTR dwStreamAttributes;
                    public long Size;
                    public uint dwStreamNameSize;
                    // WCHAR cStreamName[1];

                    //Vanara.PInvoke.Kernel32.WIN32_STREAM_ID_HEADER ddddddd;
                }


                #endregion

                /// <summary>Full path of the file which contains the stream.</summary>
                public readonly string Path;
                /// <summary>Name of the stream.</summary>
                public readonly string Name;
                /// <summary>Full path of this stream.</summary>
                public readonly string FullPath;

                public readonly Kernel32.WIN32_STREAM_ID_HEADER Header;

                #region Constructor


                public NTFSStreamInfo ( string path , string streamName , FileMode mode )
                {
                    ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
                    if ( !path.pathFileExists() ) throw new FileNotFoundException(null , path);

                    streamName.pathValidateStreamName();
                    if ( mode == FileMode.Truncate || mode == FileMode.Append ) throw new NotSupportedException($"FileMode = {mode} is not supported!");

                    Path = path;
                    Name = streamName;
                    FullPath = path.pathBuildStreamPath(streamName);
                    Header.dwStreamId = Kernel32.BACKUP_STREAM_ID.BACKUP_ALTERNATE_DATA;

                    var attr = Kernel32.GetFileAttributes(FullPath);

                    //bool exists = Kernel32.GetFileAttributes (fullStreamPath) != ( FileFlagsAndAttributes - 1 );
                    //if (!exists && FileMode.Open == mode) throw new FileNotFoundException (null, fileName: fullStreamPath);
                    //if (exists && FileMode.CreateNew == mode) throw new System.IO.IOException ($"Stream '{fullStreamPath}' already exists!");
                }


                private NTFSStreamInfo ( string filePath , string streamName , Kernel32.WIN32_STREAM_ID_HEADER hdr )
                {
                    Path = filePath;
                    Name = streamName;
                    FullPath = filePath.pathBuildStreamPath(streamName);
                    Header = hdr;
                }


                //https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-fscc/c54dec26-1551-4d3a-a0ea-4fa40f848eb3
                //https://stackoverflow.com/questions/604960/how-to-read-and-modify-ntfs-alternate-data-streams-using-net
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static unsafe NTFSStreamInfo[] BackupReadStreams ( string path , Kernel32.BACKUP_STREAM_ID? filter = Kernel32.BACKUP_STREAM_ID.BACKUP_ALTERNATE_DATA , bool bProcessSecurity = false )
                {
                    ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
                    List<NTFSStreamInfo> streams = [];

                    using var hFile = Kernel32.CreateFile(
                        path ,
                        Kernel32.FileAccess.GENERIC_READ ,
                        FileShare.Read ,
                        null ,
                        FileMode.Open ,
                        FileFlagsAndAttributes.FILE_FLAG_BACKUP_SEMANTICS ,
                        IntPtr.Zero);

                    if ( hFile.IsInvalid ) throw new Win32Exception();


                    var hdrSize = (uint)sizeof(Kernel32.WIN32_STREAM_ID_HEADER);//Must be 20 bytes
                    Debug.Assert(hdrSize == 20u , $"Invalid {nameof(Kernel32.WIN32_STREAM_ID_HEADER)} length = {hdrSize} - must be 20 bytes!");
                    //var pool = ArrayPool<byte>.Shared;

                    var lpContext = IntPtr.Zero;
                    try
                    {
                        while ( true )
                        {
                            Kernel32.WIN32_STREAM_ID_HEADER hdr = new();
                            // Read the next stream header:
                            var ret = Kernel32.BackupRead(hFile , (IntPtr)(void*)&hdr , hdrSize , out var bytesRead , false , bProcessSecurity , ref lpContext);

                            if ( !ret ) Kernel32.GetLastError().ThrowIfFailed();

                            if ( bytesRead == 0 ) break;// Last stream found, so exit loop

                            if ( bytesRead != hdrSize ) ((Win32Error)Win32Error.ERROR_INVALID_DATA).ThrowIfFailed();                            // Unexpected error -- this should always be right

                            if ( filter is null || !filter.HasValue || (filter.HasValue && filter.Value == hdr.dwStreamId) )
                            {
                                // Get the name if available
                                string name = string.Empty;
                                if ( hdr.dwStreamNameSize > 0 )
                                {
                                    using SafeLPWSTR pName = new(((int)hdr.dwStreamNameSize >> 1) + 1);
                                    bool nameRead = Kernel32.BackupRead(hFile , pName , hdr.dwStreamNameSize , out bytesRead , false , bProcessSecurity , ref lpContext);
                                    if ( !nameRead ) Kernel32.GetLastError().ThrowIfFailed();
                                    name = pName!;

                                    int separatorIndex = name!.IndexOf(STREAM_SEPARATOR , 1);
                                    if ( separatorIndex >= 1 ) name = name[ 1..separatorIndex ];
                                }
                                // Add the stream info to the result:
                                //if (name.IsNotNullOrWhiteSpace ())

                                var ss = new NTFSStreamInfo(path , name! , hdr);
                                streams.Add(ss);
                            }
                            // Skip the contents of the stream:						
                            if ( !Kernel32.BackupSeek(hFile , hdr.Size.LowPart() , (uint)hdr.Size.HighPart() , out _ , out _ , ref lpContext) )
                                Kernel32.GetLastError().ThrowIfFailed();
                        }
                    }
                    finally
                    {
                        // Abort the backup:
                        Kernel32.BackupRead(hFile , IntPtr.Zero , 0 , out var bytesRead , true , false , ref lpContext);
                    }

                    return [ .. streams ];
                }


                public FileStream Open ( FileMode mode , FileAccess? access = null , FileShare share = FileShare.None , int bufferSize = DEFAULT_BUFFER_SIZE , bool overlappedAsync = false )
                {
                    if ( 0 >= bufferSize ) throw new ArgumentOutOfRangeException(nameof(bufferSize) , bufferSize , null);

                    var flags = overlappedAsync
                        ? Vanara.PInvoke.FileFlagsAndAttributes.FILE_FLAG_OVERLAPPED
                        : 0;

                    access ??= (mode == FileMode.Append)
                        ? FileAccess.Write
                        : FileAccess.ReadWrite;

                    var handle = FullPath
                        .pathCreateFileHandleNotDisk(access.Value.toNative() , share , mode , flags)
                        .throwLastWin32ErrorIfInvalid();

                    return new FileStream(new SafeFileHandle(handle.DangerousGetHandle() , true) , access.Value , bufferSize , overlappedAsync);
                }


                #endregion



                #region Vanara Test

                /*


	 //[MethodImpl (MethodImplOptions.AggressiveInlining)]
	 //public static WinAPI.io.NTFSStreamInfo[] GetStreams ( this HFILE path ) => WinAPI.io.NTFSStreamInfo.GetStreams (path);

	 public static Win32Error BackupRead ( [In] HFILE hFile, [Optional] bool bProcessSecurity, out List<WIN32_STREAM_INFO> lpBuffers )
	 {
		 lpBuffers = [];
		 unsafe
		 {
			 // Use header to prevent unknown allocation of name value
			 WIN32_STREAM_ID_HEADER hdr = new ();
			 var hdrSize = (uint) sizeof (WIN32_STREAM_ID_HEADER);

			 IntPtr lpContext = default;
			 try
			 {
				 while (true)
				 {
					 // Read the next stream header:
					 var ret = Kernel32.BackupRead (hFile, (IntPtr) (void*) &hdr, hdrSize, out var bytesRead, false, bProcessSecurity, out lpContext);
					 if (!ret)
						 return Kernel32.GetLastError ();
					 // Last stream found, so exit loop
					 if (bytesRead == 0)
						 break;
					 // Unexpected error -- this should always be right
					 if (bytesRead != hdrSize)
						 return Win32Error.ERROR_INVALID_DATA;

					 // Get the name if available
					 string? name = null;
					 if (hdr.dwStreamNameSize > 0)
					 {
						 using SafeLPWSTR pName = new (( (int) hdr.dwStreamNameSize >> 1 ) + 1);
						 bool nameRead = Kernel32.BackupRead (hFile, pName, hdr.dwStreamNameSize, out bytesRead, false, bProcessSecurity, out lpContext);
						 if (!nameRead)
							 return Kernel32.GetLastError ();
						 name = pName;
					 }

					 // Capture the details about the stream and allocated memory with the contents
					 //(BACKUP_STREAM_ID id, BACKUP_STREAM_ATTR attr, string? name, SafeAllocatedMemoryHandle? buffer) entry = (hdr.dwStreamId, hdr.dwStreamAttributes, name, null);

					 WIN32_STREAM_INFO entry = new (name, hdr);
					 lpBuffers.Add (entry);

					 if (!Kernel32.BackupSeek (hFile, hdr.Size.LowPart (), (uint) hdr.Size.HighPart (), out _, out _, ref lpContext))
						 return Kernel32.GetLastError ();
				 }
			 }
			 finally
			 {
				 // Close the backup
				 Kernel32.BackupRead (hFile, IntPtr.Zero, 0, out _, true, bProcessSecurity, out lpContext);
			 }
		 }
		 return 0;
	 }


	 [MethodImpl (MethodImplOptions.AggressiveInlining)]
	 unsafe static public NTFSStreamInfo[] GetStreams_Vanara_Test ( string path )
	 {
		 if (path.IsNullOrWhiteSpace ()) throw new ArgumentNullException (nameof (path));
		 List<NTFSStreamInfo> streams = [];

		 using var hFile = Kernel32.CreateFile (
			 path,
			 Kernel32.FileAccess.GENERIC_READ,
			 FileShare.Read,
			 null,
			 FileMode.Open,
			 FileFlagsAndAttributes.FILE_FLAG_BACKUP_SEMANTICS,
			 IntPtr.Zero);
		 if (hFile.IsInvalid) throw new Win32Exception ();


		 BackupRead (hFile, false, out var foundStreams)
			 .ThrowIfFailed ();

		 var aaaaaaa = foundStreams.ToArray ();

		 return [ .. streams ];
	 }
			*/
                #endregion


                public string ReadablePath
                {
                    get
                    {
                        string sPath = FullPath;
                        if ( sPath.EndsWith(FULL_STREAM_PATH_SUFFIX) )
                        {
                            sPath = sPath.Replace(FULL_STREAM_PATH_SUFFIX , "");
                        }

                        return sPath;
                    }
                }


            }
            #endregion


        }


    }


    namespace Extensions
    {

        internal static partial class Extensions_WinAPI_IO_NTFS
        {



            extension( WinAPI.io.NTFSStreamInfo nsi )
            {


                #region OpenRead / OpenWrite / OpenText


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public FileStream openRead ()
                    => nsi.Open(FileMode.Open , FileAccess.Read , FileShare.Read);

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public FileStream openWrite ()
                    => nsi.Open(FileMode.OpenOrCreate , FileAccess.Write , FileShare.None);

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public StreamReader openText ()
                {
                    var fs = nsi.Open(FileMode.Open , FileAccess.Read , FileShare.Read);
                    return new StreamReader(fs);
                }


                #endregion


                /// <inheritdoc cref="Kernel32.DeleteFile" />
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool delete ()
                    => Kernel32.DeleteFile(nsi.FullPath);
            }

            extension( string streamName )
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public void pathValidateStreamName ()
                {
                    if ( streamName.isNullOrEmpty || streamName.IndexOfAny(WinAPI.io.NTFSStreamInfo.InvalidStreamNameChars) != -1 )
                    {
                        throw new ArgumentException("ValidateStreamName Failed due to InvalidStreamNameChars!" , nameof(streamName));
                    }
                }

            }

            extension( string path )
            {


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public string pathBuildStreamPath ( string streamName )
                {
                    ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
                    return WinAPI.core.UNC_PATH_PREFIX + path + $"{WinAPI.io.NTFSStreamInfo.STREAM_SEPARATOR}{streamName}{WinAPI.io.NTFSStreamInfo.FULL_STREAM_PATH_SUFFIX}";
                }


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool pathStreamExists ( string streamName )
                {
                    ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
                    streamName.pathValidateStreamName();
                    return path.pathBuildStreamPath(streamName).pathFileExists();
                }


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public WinAPI.io.NTFSStreamInfo[] pathGetNTFSStreams ( Kernel32.BACKUP_STREAM_ID? filter = Kernel32.BACKUP_STREAM_ID.BACKUP_ALTERNATE_DATA , bool bProcessSecurity = false )
                    => WinAPI.io.NTFSStreamInfo.BackupReadStreams(path , filter , bProcessSecurity);




            }

            extension( FileSystemInfo fso )
            {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                [Obsolete("Need to rework!" , true)]
                public bool deleteStream ( string streamName )
                {
                    ArgumentNullException.ThrowIfNull(fso);

                    streamName.pathValidateStreamName();
                    if ( !fso.Exists ) return true;

                    string path = pathBuildStreamPath(fso.FullName , streamName);
                    //if (-1 != SafeGetFileAttributes (path)) result = SafeDeleteFile (path);
                    return true;
                }


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public WinAPI.io.NTFSStreamInfo getNTFSStream ( string streamName , FileMode mode )
                    => new(fso.FullName , streamName , mode);


                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public WinAPI.io.NTFSStreamInfo[] getNTFSStreams ( Kernel32.BACKUP_STREAM_ID? filter = Kernel32.BACKUP_STREAM_ID.BACKUP_ALTERNATE_DATA , bool bProcessSecurity = false )
                    => fso.Exists
                    ? fso.FullName.pathGetNTFSStreams(filter , bProcessSecurity)
                    : throw new FileNotFoundException(null , fso.FullName);


            }




        }

    }






}


#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning restore IDE1006 // Naming Styles
