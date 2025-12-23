#nullable enable


global using System.Drawing;
global using System.Drawing.Drawing2D;
global using System.Drawing.Imaging;
global using static uom.Extensions.Extensions_DebugAndErrors;
global using DWORD = System.Int32;
global using QWORD = System.Int64;
global using WORD = System.Int16;

#if WINDOWS

global using Microsoft.Win32;
global using Microsoft.Win32.SafeHandles;
global using System.Windows.Forms;

using Vanara.PInvoke;
using Vanara.Security.AccessControl;
using static Vanara.PInvoke.AdvApi32;
using static Vanara.PInvoke.Kernel32
    ;
using Application = System.Windows.Forms.Application;
using uom.WinAPI;

#endif

using System.Diagnostics.CodeAnalysis;
using System.IO.IsolatedStorage;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using uom.AutoDisposable;
using uom.Extensions;

//using Point = System.Drawing.Point;
//using Size = System.Drawing.Size;
using Vanara.Extensions;

//using System.Linq.Expressions;
//using System.Runtime.Remoting;


using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;
using static Vanara.PInvoke.Ws2_32;
using static uom.WinAPI.win32Resources;
using static Vanara.PInvoke.User32;
using Vanara.Marshaler;
using static Vanara.PInvoke.IpHlpApi;
using System.Windows.Documents;
//using ExCSS;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Vanara.PInvoke.ComCtl32;
using static uom.WinAPI.safeHandles;

//https://github.com/dahall/Vanara
/*
  <ItemGroup>
		<PackageReference Include="Vanara.Library" Version="4.1.3" />
		
OR 

		<PackageReference Include="Vanara.Pinvoke.Kernel32" Version="4.1.3" />
		<PackageReference Include="Vanara.Pinvoke.User32" Version="4.1.3" />
		<PackageReference Include="Vanara.Pinvoke.Gdi32" Version="4.1.3" />

	
		<PackageReference Include="Vanara.Security" Version="4.1.3" />
		<PackageReference Include="Vanara.Pinvoke.Security" Version="4.1.3" />		

		<PackageReference Include="Vanara.Pinvoke.Shell32" Version="4.1.3" />
		<PackageReference Include="Vanara.Pinvoke.ShlwApi" Version="4.1.3" />
		
		<PackageReference Include="Vanara.Pinvoke.IpHlpApi" Version="4.1.3" />
		<PackageReference Include="Vanara.Pinvoke.NtDll" Version="4.1.3" />

		<PackageReference Include="Vanara.PInvoke.Accessibility" Version="4.1.3" />

		<PackageReference Include="Vanara.Pinvoke.DwmApi" Version="4.1.3" />
		<PackageReference Include="Vanara.Windows.Forms" Version="4.1.3" />
	</ItemGroup>

 */


#if WINDOWS


#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.

/// <summary>Commnon Tools For Net Apps (WINDOWN PLATFORM)
/// (C)UOM 2000 - 2022 </ summary >
namespace uom
{



    internal static partial class AppInfo
    {
        /// <summary>в режиме дизайнера WPF?</summary>
        private static bool IsInDesignerMode_WPF => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        /// <summary>в режиме дизайнера WinForms?</summary>
        private static bool IsInDesignerMode_WinForms
            => Process
            .GetCurrentProcess()
            .ProcessName
            .valueOrEmpty
            .Equals("devenv" , StringComparison.InvariantCultureIgnoreCase);


        #region Helper Functions for Admin Privileges and Elevation Status

        /// <summary>The function gets the elevation information of the current process. It dictates whether the process is elevated or not.<br/>
        /// Token elevation is only available on Windows Vista and newer, thus IsProcessElevated throws a C++ exception if it is called on systems prior to Windows Vista.<br/>
        /// <c>It is not appropriate to use this function to determine whether a process is run as administartor.</c>
        /// </summary>
        /// <returns>True if the process is elevated.<br/>False if it is not.</returns>
        /// <remarks> TOKEN_INFORMATION_CLASS provides TokenElevationType to check the elevation type (TokenElevationTypeDefault / TokenElevationTypeLimited / TokenElevationTypeFull) of the process.<br/>
        /// It is different from TokenElevation in that, when UAC is turned off, elevation type always returns TokenElevationTypeDefault even though the process is elevated (Integrity Level == High).<br/>
        /// In other words, it is not safe to say if the process is elevated based on elevation type. Instead, we should use TokenElevation.
        /// </remarks>
        internal static AdvApi32.TOKEN_ELEVATION_TYPE GetProcessElevation ()
        {
            uom.OS.CheckVistaOrLater();
            using var hToken = AdvApi32.TokenAccess.TOKEN_QUERY.OpenCurrentProcessToken();
            return ((HTOKEN)hToken.DangerousGetHandle()).GetTokenInformation_Elevation();
        }


        /// <inheritdoc cref="GetProcessElevation" />
        internal static bool IsElevated ()
            => GetProcessElevation() == AdvApi32.TOKEN_ELEVATION_TYPE.TokenElevationTypeFull;


        /// <inheritdoc cref="GetProcessElevation" />
        internal static bool AssertIsElevated ()
            => IsElevated()
            ? true
            : throw new Win32Exception((int)uom.WinAPI.errors.Win32Errors.ERROR_ELEVATION_REQUIRED);



        /// <summary>integrity level of the process. Integrity level is only available on Windows Vista and newer operating systems, thus 
        /// GetProcessIntegrityLevel throws a C++ exception if it is called on systems prior to Windows Vista.</summary>
        public enum IntegrityLevels : uint
        {
            //ERROR = -1,

            /// <summary>SECURITY_MANDATORY_UNTRUSTED_RID - means untrusted level. It is used by processes started by the Anonymous group. Blocks most write access. (SID: S-1-16-0x0)</summary>
            SECURITY_MANDATORY_UNTRUSTED_RID = 0,

            /// <summary>SECURITY_MANDATORY_LOW_RID - means low integrity level. It is used by Protected Mode Internet Explorer. Blocks write acess to most objects (such as files and registry keys) on the system. (SID: S-1-16-0x1000)</summary>
            SECURITY_MANDATORY_LOW_RID = 0x1000,

            /// <summary>SECURITY_MANDATORY_MEDIUM_RID - means medium integrity level. It is used by normal applications being launched while UAC is enabled. (SID: S-1-16-0x2000)</summary>
            SECURITY_MANDATORY_MEDIUM_RID = 0x2000,

            /// <summary>SECURITY_MANDATORY_HIGH_RID - means high integrity level. It is used by administrative applications launched through elevation when UAC is enabled, or normal applications if UAC is disabled and the user is an administrator. (SID: S-1-16-0x3000)</summary>
            SECURITY_MANDATORY_HIGH_RID = 0x3000,

            /// <summary>SECURITY_MANDATORY_SYSTEM_RID - means system integrity level. It is used by services and other system-level applications (such as Wininit, Winlogon, Smss, etc.)  (SID: S-1-16-0x4000)</summary>
            SECURITY_MANDATORY_SYSTEM_RID = 0x4000
        }



        /// <summary>The function gets the integrity level of the current process.<br/>
        /// Integrity level is only available on Windows Vista and newer, thus GetProcessIntegrityLevel throws a C++ exception if it is called on systems prior to Windows Vista.</summary>
        /// <exception cref="System.ComponentModel.Win32Exception">When any native Windows API call fails, the function throws a Win32Exception with the last error code.</exception>
        internal static IntegrityLevels GetProcessIntegrityLevel ()
        {
            OS.CheckVistaOrLater();

            using var hToken = AdvApi32.TokenAccess.TOKEN_QUERY.OpenCurrentProcessToken(); // Open the access token of the current process with TOKEN_QUERY.

            // Then we must query the size of the integrity level information 
            // associated with the token. Note that we expect GetTokenInformation to 
            // return False with the ERROR_INSUFFICIENT_BUFFER error code because we 
            // have given it a null buffer. On exit cbTokenIL will tell the size of 
            // the group information.

            //WinAPI.security.IntegrityLevels IL;//= WinAPI.security.IntegrityLevels.ERROR;

            AdvApi32.GetTokenInformation(
                hToken ,
                AdvApi32.TOKEN_INFORMATION_CLASS.TokenIntegrityLevel ,
                IntPtr.Zero ,
                0 ,
                out var cbTokenIL)
                .ThrowLastWin32ErrorIfFailedUnless(WinAPI.errors.Win32Errors.ERROR_INSUFFICIENT_BUFFER);

            //AdvApi32.GetTokenInformation<Int32> (hToken, AdvApi32.TOKEN_INFORMATION_CLASS.TokenIntegrityLevel)				.eThrowLastWin32Exception_AssertFalse (WinAPI.errors.Win32Errors.ERROR_INSUFFICIENT_BUFFER);


            // Now we allocate a buffer for the integrity level information.
            using var tokenILMem = new memory.WinApiMemory(cbTokenIL);
            // Now we ask for the integrity level information again. This may fail if an administrator has added this account to an additional group between our first call to GetTokenInformation and this one.
            AdvApi32.GetTokenInformation(hToken ,
                AdvApi32.TOKEN_INFORMATION_CLASS.TokenIntegrityLevel ,
                tokenILMem ,
                cbTokenIL ,
                out cbTokenIL)
                .ThrowLastWin32ErrorIfFailed();

            var tokenIL = tokenILMem.DangerousGetHandle().eToStructure<AdvApi32.TOKEN_MANDATORY_LABEL>();

            // Integrity Level SIDs are in the form of S-1-16-0xXXXX. (e.g. S-1-16-0x1000 stands for low integrity level SID). There is one and only one subauthority.
            //var pIL = AdvApi32.GetSidSubAuthority (tokenIL.Label.Sid, 0);
            //int iIL = Marshal.ReadInt32 (pIL);
            var il = (IntegrityLevels)AdvApi32.GetSidSubAuthority(tokenIL.Label.Sid , 0);
            return il;
            //IL = (AdvApi32.IntegrityLevel) iIL;
            //return IL;
        }


        #endregion

    }


    internal static partial class AppTools
    {

        internal partial class ElevationCanceledByUser : Win32Exception { public ElevationCanceledByUser () : base((int)WinAPI.errors.Win32Errors.ERROR_CANCELLED) { } }

        internal static Kernel32.SafeHINSTANCE GetCurrentModuleHandle ()
        {
            using Process cp = Process.GetCurrentProcess();
            using ProcessModule cm = cp.MainModule!;
            return Kernel32.GetModuleHandle(cm.ModuleName!);
        }



        public static void AdjustProcessPrivilegeAndCloseToken ( Vanara.Security.AccessControl.SystemPrivilege pn )
            => pn.AdjustProcessPrivilegeAndCloseToken();


        #region GetConsoleWindowForCurrentProcess


        [Obsolete("Didn't work with Windows Terminal!")]

        public static IntPtr GetConsoleWindowForCurrentProcess_Net ()
            => Process.GetCurrentProcess().MainWindowHandle;


#if WINDOWS

        [Obsolete("Don't work with windows terminal")]
        /// <inheritdoc cref="Kernel32.GetConsoleWindow" />

        public static HWND? GetConsoleWindowForCurrentProcess_Kernel32 ()
            => Kernel32.GetConsoleWindow();


        public static IntPtr GetConsoleWindowForCurrentProcess_2 ()
        {
            // Fetch current window title.
            StringBuilder sbOldTitle = new(2048);
            _ = Kernel32.GetConsoleTitle(sbOldTitle , (uint)sbOldTitle.Capacity);

            // Change current window title.
            string tempTitle = Guid.NewGuid().ToString();
            Kernel32.SetConsoleTitle(tempTitle);
            // Ensure window title has been updated.
            Kernel32.Sleep(40);
            // Look for NewWindowTitle.
            HWND hwndFound = User32.FindWindow(null , tempTitle);
            // Restore original window title.
            Kernel32.SetConsoleTitle(sbOldTitle.ToString());
            return hwndFound.DangerousGetHandle();
        }

#endif

        #endregion


#if !UWP

        [Obsolete("!NEED TO REDESIGN!!!" , true)]
        /// <summary>Start copy of current process</summary>
        internal static Process? StartMySelf (
            string? Arguments = null ,
            bool elevated = false ,
            bool CloseThisApp = false ,
            bool WaitExit = false )
            => StartProcess(
                System.Windows.Forms.Application.ExecutablePath ,
                Arguments ,
                elevated: elevated ,
                closeThisApp: CloseThisApp ,
                waitExit: WaitExit);




#endif


        #region StartProcess


        internal static Process? StartCMDProcess (
            string Arguments ,
            bool elevated = false ,
            bool CloseThisApp = false ,
            bool WaitExit = false )
            => StartWinSys32Process(
                "cmd.exe" ,
                @"/c " + Arguments ,
                elevated ,
                CloseThisApp ,
                WaitExit);

        internal static Process? StartMMCProcess (
            string Оснастка ,
            bool elevated = false ,
            bool CloseThisApp = false ,
            bool WaitExit = false )
            => StartWinSys32Process(
                "mmc.exe" ,
                Оснастка ,
                elevated ,
                CloseThisApp ,
                WaitExit);


        /// <returns>Return Process ExitCode</returns>
        internal static Process? StartWinSys32Process (
            string FileName ,
            string? Arguments = null ,
            bool elevated = false ,
            bool CloseThisApp = false ,
            bool WaitExit = false )
            => StartProcess(
                OS.GetWinSys32Path(FileName) ,
                Arguments ,
                OS.GetWinSys32Path() ,
                elevated ,
                CloseThisApp ,
                WaitExit);

        /// <returns>Return Process ExitCode</returns>
        internal static Process? StartWinDirProcess (
            string FileName ,
            string? Arguments = null ,
            bool elevated = false ,
            bool CloseThisApp = false ,
            bool WaitExit = false )
            => StartProcess(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows) , FileName) ,
                Arguments ,
                null ,
                elevated ,
                CloseThisApp ,
                WaitExit);

        /// <returns>Return Process ExitCode</returns>
        internal static Process? StartProcess (
            string file ,
            string? arguments = null ,
            string? workingDirectory = null ,
            bool elevated = false ,
            bool closeThisApp = false ,
            bool waitExit = false )
        {
            try
            {
                file = file.eEncloseC();
                var StartInfo = new ProcessStartInfo() { UseShellExecute = true , FileName = file };
                if ( arguments.isNotNullOrWhiteSpace ) StartInfo.Arguments = arguments;

                if ( workingDirectory.isNotNullOrWhiteSpace ) StartInfo.WorkingDirectory = workingDirectory;

                if ( elevated && !uom.AppInfo.IsElevated() ) StartInfo.Verb = "runas";
                // .UseShellExecute = False
                StartInfo.ErrorDialog = false;
                var prc = Process.Start(StartInfo);
                if ( waitExit )
                {
                    prc!.WaitForExit();
                }
                else
                {

#if !UWP
                    if ( closeThisApp )
                    {
                        System.Windows.Forms.Application.Exit();
                    }
#endif

                }
                return prc;
            }
            catch ( Win32Exception wex )
            {
                if ( wex.eToWin32Error() == WinAPI.errors.Win32Errors.ERROR_CANCELLED )
                {
                    wex = new ElevationCanceledByUser();// user cancel elevation
                }
                throw wex;
            }
        }


        #region StartWinSysTool...

        internal enum WindowsExplorerPathModes : int
        {
            OpenPath,
            SelectItem
        }

        internal const WindowsExplorerPathModes C_DEFAULT_EXPLORER_MODE = WindowsExplorerPathModes.SelectItem;

        internal static void StartWinSysTool_Explorer (
            string? pathToShow = null ,
            WindowsExplorerPathModes pathMode = C_DEFAULT_EXPLORER_MODE )
        {
            string? explorerArgs = null;

            FileSystemInfo? fsi = pathToShow?.eFindFirstExistingFileSystemInfo();
            if ( fsi != null )
            {
                pathToShow = fsi.FullName.eEnclose();
                explorerArgs = pathMode switch
                {
                    WindowsExplorerPathModes.OpenPath => pathToShow,
                    WindowsExplorerPathModes.SelectItem => $@"/select,{pathToShow}",
                    _ => throw new ArgumentOutOfRangeException(nameof(pathMode))
                };

                // /separate 	show in separate EXPLORER process 
                // /idlist,:handle:process 	specifies object as ITEMIDLIST in shared memory block with given handle in context of given process 

                // /n  Opens a New window in single-paned (My Computer) view for each item
                // selected, even if the New window duplicates a window that Is already open.

                // /e  Uses Windows Explorer view. Windows Explorer view Is most similar
                // to File Manager in Windows version 3.x. Note that the default view
                // Is Open view.

                // /root,<object>  Specifies the root level of the specified view. The
                // Default is To use the normal Namespace root (the desktop). Whatever Is specified Is the root for the
                // display.
                // /root,/idlist,HANDLE : Process
                // /root,clsid
                // /root,clsid,path
                // /root,path 	

                // Examples

                // To open a Windows Explorer view to explore only objects on \\<server name>, use the following syntax
                // explorer /e,/ROOT,\\ <server name>
                // To view the C:\WINDOWS folder and select CALC.EXE, use the following syntax:
                // explorer /select,c:\windows\calc.exe 


            }
            StartWinDirProcess("explorer.exe" , explorerArgs);
        }

        #region Control Panel

        internal static void StartWinSysTool_CPL_NetworkAndSharingCenter ()
            => StartWinSys32Process(
                "control.exe" ,
                "/name Microsoft.NetworkAndSharingCenter");

        /// <summary>Работает и в XP</summary>
        internal static void StartWinSysTool_CPL_WinFirewall ()
            => StartWinSys32Process(
                "rundll32.exe" ,
                "shell32.dll,Control_RunDLL firewall.cpl" ,
                true);

        internal static void StartWinSysTool_CPL_NetworkConnections ()
            => StartWinSys32Process(
                "rundll32.exe" ,
                "shell32.dll,Control_RunDLL ncpa.cpl" ,
                true);

        /// <summary>rundll32.exe netplwiz.dll,UsersRunDll</summary>
        internal static void StartWinSysTool_CPL_UserAccounts_1 ()
            => StartWinSys32Process(
                "rundll32.exe" ,
                "netplwiz.dll,UsersRunDll" ,
                true);

        /// <summary>control userpasswords2</summary>
        internal static void StartWinSysTool_CPL_UserAccounts_2 ()
            => StartWinSys32Process(
                "control.exe" ,
                "userpasswords2" ,
                true);

        #endregion

        /// <summary>"sfc /scannow"</summary>
        internal static void StartWinSysTool_CMD_SFC ()
            => StartCMDProcess(
                @"sfc /scannow" ,
                true);


        #region MMC


        internal static void StartWinSysTool_MMC_WinFirewallRules ()
        {
            OS.CheckVistaOrLater(); // возможно только начиная с Vist'ы
            StartMMCProcess("wf.msc" , true);
        }

        /// <summary>Локальная политика безопасности (там есть Safer)</summary>
        internal static void StartWinSysTool_MMC_SecPol ()
            => StartMMCProcess("secpol.msc" , true);

        /// <summary>Групповые политики</summary>
        internal static void StartWinSysTool_MMC_Gpedit ()
            => StartMMCProcess("gpedit.msc" , true);

        /// <summary>Консоль службы</summary>
        internal static void StartWinSysTool_MMC_Services ()
            => StartMMCProcess("services.msc" , true);

        /// <summary>Консоль локальные пользователи и группы</summary>
        internal static void StartWinSysTool_MMC_lusrmgr ()
            => StartMMCProcess("lusrmgr.msc" , true);

        /// <summary>Консоль диспетчер устройств</summary>
        internal static void StartWinSysTool_MMC_devmgmt ()
            => StartMMCProcess("devmgmt.msc" , true);

        internal static void StartWinSysTool_MMC_diskmgmt ()
            => StartMMCProcess("diskmgmt.msc" , true);


        #endregion


        #endregion


        #endregion


        /// <inheritdoc cref="WindowsIdentity.RunImpersonated(SafeAccessTokenHandle, Action)" />
        public static void RunImpersonated ( Action a , string domain , string user , string pwd , AdvApi32.LogonUserType logonType = AdvApi32.LogonUserType.LOGON32_LOGON_INTERACTIVE )
        {
            // Call LogonUser to obtain a handle to an access token.
            AdvApi32.LogonUser(
                user ,
                domain ,
                pwd ,
                logonType ,
                 AdvApi32.LogonUserProvider.LOGON32_PROVIDER_DEFAULT ,
                 out var tokenHandle)
                .ThrowLastWin32ErrorIfFailed();

            using ( tokenHandle )
            {
                // Use the token handle returned by LogonUser.
                SafeAccessTokenHandle safeTokenHandle = new(tokenHandle.DangerousGetHandle());
                WindowsIdentity.RunImpersonated(safeTokenHandle , a);
            }
        }


        internal static partial class AppSettings
        {

            [DebuggerNonUserCode]

            private static string RegBuildProductPath ( Version? ver = null )
            {
                FileVersionInfo fvi = uom.AppInfo.AssemblyFileVersionInfo;
                if ( fvi.CompanyName.isNullOrWhiteSpace )
                {
                    throw new ArgumentNullException("AssemblyFileVersionInfo.CompanyName");
                }

                if ( fvi.ProductName.isNullOrWhiteSpace )
                {
                    throw new ArgumentNullException("AssemblyFileVersionInfo.ProductName");
                }

                string productPath = @$"Software\{fvi.CompanyName}\{fvi.ProductName}";

                if ( ver != null )
                {
                    productPath += @$"\{ver.Major}";
                }

                return productPath;
            }


            [DebuggerNonUserCode]

            private static RegistryKey? OpenProductKey ( string subKey = "" , Version? ver = null , bool writable = true )
            {
                if ( ver == null )
                {
                    FileVersionInfo fvi = uom.AppInfo.AssemblyFileVersionInfo;
                    if ( !Version.TryParse(fvi.FileVersion , out ver) )
                    {
                        throw new ArgumentException($"AssemblyFileVersionInfo.FileVersion = '{fvi.FileVersion}' is invalid!");
                    }
                    //return Application.UserAppDataRegistry;
                }
                string path = RegBuildProductPath(ver);

                if ( subKey.isNotNullOrWhiteSpace )
                {
                    path += '\\' + subKey;
                }

                var keyProduct = global::Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path , writable);

                if ( keyProduct == null && writable )
                {
                    keyProduct = global::Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path , true);
                    if ( keyProduct == null )
                    {
                        throw new Exception($"Failed to create key '{path}'!");
                    }
                }

                return keyProduct!;
            }

            [DebuggerNonUserCode]

            private static RegistryKey? OpenProductRootKey ( bool writable = true )
            {
                string path = RegBuildProductPath(null);
                var keyProduct = global::Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path , writable);
                return keyProduct!;
            }


            #region Save / Delete

            /// <summary>if value == null, reg value will be deleted!</summary>

            internal static void Save<T> ( string name , T? val , string subKey = "" ) where T : unmanaged
            {
                using RegistryKey keySetting = OpenProductKey(subKey , writable: true)!;
                keySetting.eSetValue<T>(name , val);
                keySetting.Flush();
            }

            /// <summary>if value == null, reg value will be deleted!</summary>

            internal static void Save ( string name , string? val , string subKey = "" )
            {
                using RegistryKey keySetting = OpenProductKey(subKey , writable: true)!;
                keySetting.eSetValueString(name , val);
                keySetting.Flush();
            }

            /// <inheritdoc cref="Save" />

            internal static void SaveMultiString ( string name , string? val , string subKey = "" )
            {
                var lines = val.eReadLines(true);
                Save(name , lines.ToArray() , subKey);
            }



            internal static void Save ( string name , string[]? val , string subKey = "" )
            {
                using RegistryKey keySetting = OpenProductKey(subKey , writable: true)!;
                keySetting.eSetValueStrings(name , val);
                keySetting.Flush();
            }



            internal static void Save ( string name , DateTime? val , string subKey = "" )
                => Save<DateTime>(name , val , subKey);





            internal static void Delete ( string name , string subKey = "" )
            {
                using RegistryKey? keySetting = OpenProductKey(subKey , writable: true);
                keySetting?.DeleteValue(name);
                keySetting?.Flush();
            }





            #endregion


            [DebuggerNonUserCode]
            internal static T? Get_T<T> ( string name , T? defaultValue = default , string subKey = "" , bool searchPreviousVersions = true )
            {
                using ( RegistryKey? keyCurrentVersion = OpenProductKey(subKey , writable: false) )
                {
                    if ( keyCurrentVersion != null )
                    {
                        var (valueFound, _, value) = keyCurrentVersion.eGetValueT<T>(name , defaultValue);
                        if ( valueFound )
                        {
                            return value;
                        }
                    }
                }

                if ( !searchPreviousVersions )
                {
                    return defaultValue;
                }

                //Searching Previous Versions
                FileVersionInfo fvi = uom.AppInfo.AssemblyFileVersionInfo;

                int verCurrent = fvi.FileMajorPart;

                using RegistryKey? keyProductRoot = OpenProductRootKey(false);
                if ( keyProductRoot == null )
                {
                    return defaultValue;
                }

                int[] foundVersionNumbers = [..
                    keyProductRoot
                    .GetSubKeyNames()
                    .Select(s =>
                    {
                        Int32 ver = -1;
                        if (uint.TryParse(s, out var uintVal)) { ver = (Int32)uintVal; } return ver;
                    }
                    )
                    .Where(verFound => verFound > 0)
                    .OrderByDescending(k => k)
                    ];

                foundVersionNumbers = [..
                    foundVersionNumbers
                    .Where(verFound => verFound < verCurrent)
                    .OrderByDescending(k => k)
                    ];

                foreach ( var major in foundVersionNumbers )
                {
                    Version v = new Version(major , 0 , 0 , 0);
                    using ( RegistryKey? keyVersioned = OpenProductKey(subKey , v , false) )
                    {
                        if ( keyVersioned != null )
                        {
                            var (valueFound, _, value) = keyVersioned.eGetValueT<T>(name , defaultValue);
                            if ( valueFound )
                            {
                                return value;
                            }
                        }
                    }
                }

                return defaultValue;
            }



            internal static string Get_string ( string name , string defaultValue , string subKey = "" , bool searchPreviousVersions = true )
                => Get_T<string>(name , defaultValue , subKey , searchPreviousVersions)!;

            internal static string[] Get_strings ( string name , string[] defaultValue , string subKey = "" , bool searchPreviousVersions = true )
                => Get_T<string[]>(name , defaultValue , subKey , searchPreviousVersions)!;

            internal static string Get_stringsAsText ( string name , string defaultValue , string subKey = "" , bool searchPreviousVersions = true )
            {
                var lines = Get_strings(name , [] , subKey , searchPreviousVersions);
                if ( lines != null && lines.Length != 0 )
                {
                    var s = lines.join(Environment.NewLine)!;
                    return s;
                }
                return defaultValue;
            }

            internal static bool Get_bool ( string name , bool defaultValue , string subKey = "" , bool searchPreviousVersions = true )
                => Get_T<bool>(name , defaultValue , subKey , searchPreviousVersions)!;

            internal static Int32 Get_Int32 ( string name , Int32 defaultValue , string subKey = "" , bool searchPreviousVersions = true )
                => Get_T<Int32>(name , defaultValue , subKey , searchPreviousVersions);

            internal static Int64 Get_Int64 ( string name , Int64 defaultValue , string subKey = "" , bool searchPreviousVersions = true )
                => Get_T<Int64>(name , defaultValue , subKey , searchPreviousVersions);


            internal static DateTime Get_DateTime ( string name , DateTime defaultValue , string subKey = "" , bool searchPreviousVersions = true )
            {
                long dtBinary = Get_Int64(name , 0L , subKey , searchPreviousVersions);
                return (dtBinary == 0)
                    ? defaultValue
                    : DateTime.FromBinary(dtBinary);
            }


        }


    }


    internal static partial class OS
    {


        internal const int DEFAULT_SCREEN_DPI = 96;



        public static readonly CultureInfo CurrentUICulture = CultureInfo.CurrentUICulture;
        public static readonly RegionInfo CurrentRegionInfo = new(CurrentUICulture.LCID);



        internal static ulong TickCount_64
        {
            get
            {
#if NET
                return (ulong)Environment.TickCount64;//Use new builtin Net function (net 5, 6, Core 3.0, Core 3.1)
#else
#if WINDOWS
				return Kernel32.GetTickCount64();//old net versions
#else
				throw new NotImplementedException();
#endif
#endif
            }
        }



#if WINDOWS

        public static bool Is64BitOperatingSystem
        {
            get
            {
                var isWow64ProcessProcDefined = Kernel32.GetProcAddress(Kernel32.GetModuleHandle("kernel32") , "IsWow64Process") != IntPtr.Zero;
                return isWow64ProcessProcDefined
                    && Kernel32.IsWow64Process(Kernel32.GetCurrentProcess() , out _);
            }
        }

        /// <summary>Used when system uses High DPI modes</summary>
        /// <returns>
        /// 1.25 = 125%
        /// 1.5 = 150%
        /// </returns>

        public static float GetScreenScalingFactor ()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr hdcDesktop = g.GetHdc();
            try
            {
                int logPixelsY = Gdi32.GetDeviceCaps(hdcDesktop , Gdi32.DeviceCap.LOGPIXELSY);
                return logPixelsY == DEFAULT_SCREEN_DPI ? 1f : logPixelsY / (float)DEFAULT_SCREEN_DPI;
            }
            finally { g.ReleaseHdc(hdcDesktop); }
        }



        public static Kernel32.SYSTEM_INFO GetNativeSystemInfo ()
        {
            Kernel32.GetNativeSystemInfo(out var si);
            return si;
        }


#endif


        /// <summary>On hybernation counter is not resets and looks like PC was not shutting down</summary>
        /// <remarks>
        /// Windows 8 and old, hibernation reset this counter to zerro.
        /// </remarks>
        public static TimeSpan GetSystemUpTime_FromTickCount64 ()
            => TimeSpan.FromMilliseconds(TickCount_64);



        internal static string GetWinSys32Path ( string? childPath = null )
            => childPath.isNullOrWhiteSpace
            ? Environment.GetFolderPath(Environment.SpecialFolder.System)
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System) , childPath!);


        #region Win Version


        internal static bool IsNT
            => Environment.OSVersion.Platform.HasFlag(PlatformID.Win32NT);




        public static (Kernel32.OSVERSIONINFOEX DisplayVersion, NtDll.OSVERSIONINFOW KernelVersion) GetVersion ()
        {
            NtDll.RtlGetVersion(out NtDll.OSVERSIONINFOW rtlVersion).ThrowIfFailed();
            #region RtlGetNtVersionNumbers

            //    [DllImport(WinAPI.WINDLL_NTDLL, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
            //    using fnRtlGetNtVersionNumbers = void (WINAPI*) (LPDWORD major, LPDWORD minor, LPDWORD build);
            //    // 1809 17763
            //     fnRtlGetNtVersionNumbers RtlGetNtVersionNumbers = reinterpret_cast<fnRtlGetNtVersionNumbers>(GetProcAddress(GetModuleHandleW(L"ntdll.dll"), "RtlGetNtVersionNumbers"));
            //private static extern void RtlGetNtVersionNumbers(out uint major, [Out ] uint minor, [Out ] ushort build);

            ///// <summary>Use RtlGetNtVersionNumbers from ntdll.dll</summary>
            //internal static Version RtlGetNtVersionNumbers()
            //{
            //    uint major, minor;
            //    ushort build;
            //    RtlGetNtVersionNumbers(ref major, ref minor, ref build);
            //    // build = build And Not 0xF0000000
            //    // build = (build And 0xF0000000)
            //    return new Version((int)major, (int)minor, build);
            //}
            #endregion


            Kernel32.OSVERSIONINFOEX ovix = new()
            {
                dwOSVersionInfoSize = (uint)Marshal.SizeOf<Kernel32.OSVERSIONINFOEX>()
            };
            return !Kernel32.GetVersionEx(ref ovix) ? throw new Win32Exception() : ((Kernel32.OSVERSIONINFOEX DisplayVersion, NtDll.OSVERSIONINFOW KernelVersion))(ovix, rtlVersion);

            /*
			var rtlVer = RtlGetVersion();
			var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<NtDll.OSVERSIONINFOW>());
			Marshal.StructureToPtr(rtlVer, ptr, false);
			var ve = Marshal.PtrToStructure<Kernel32.OSVERSIONINFOEX>(ptr);
			return ve;
			 */
        }







        #region IsWinVersionMoreThan_

        #region Client versions

        /// <summary>Windows NT 3.51 Workstation</summary>
        internal static readonly Version OS_VERSION_CLIENT_NT_3_51 = new(3 , 51 , 0 , 0);

        /// <summary>Windows 95</summary>
        internal static readonly Version OS_VERSION_CLIENT_W95 = new(4 , 0 , 0 , 0);

        /// <summary>Windows NT 4.0 Workstation	1381 (Service Pack 6a)</summary>
        internal static readonly Version OS_VERSION_CLIENT_WIN_NT_4_0 = new(4 , 0 , 0 , 0);

        /// <summary>Windows 98 / Windows 98 Second Edition</summary>
        internal static readonly Version OS_VERSION_CLIENT_W98 = new(4 , 10 , 0 , 0);

        /// <summary>NT 5.0	Professional</summary>
        internal static readonly Version OS_VERSION_CLIENT_W2000 = new(5 , 0);

        /// <summary>Windows Me</summary>
        internal static readonly Version OS_VERSION_CLIENT_ME = new(4 , 90 , 0 , 0);

        /// <summary>Windows XP</summary>
        internal static readonly Version OS_VERSION_CLIENT_XP = new(5 , 1);

        /// <summary>Windows XP 64-bit Edition</summary>
        internal static readonly Version OS_VERSION_CLIENT_XP_x64 = new(5 , 2);

        /// <summary>Windows Vista</summary>
        internal static readonly Version OS_VERSION_CLIENT_VISTA = new(6 , 0);

        /// <summary>Windows 7</summary>
        internal static readonly Version OS_VERSION_CLIENT_W7 = new(6 , 1);

        /// <summary>Windows 8</summary>
        internal static readonly Version OS_VERSION_CLIENT_W8 = new(6 , 2);

        /// <summary>Windows 8.1</summary>
        internal static readonly Version OS_VERSION_CLIENT_W81 = new(6 , 3);

        /// <summary>Windows 10</summary>
        internal static readonly Version OS_VERSION_CLIENT_W10 = new(10 , 0);

        /// <summary>Windows 11</summary>
        internal static readonly Version OS_VERSION_CLIENT_W11 = new(10 , 0 , 22000);
        #endregion


        #region Server versions
        /// <summary>Windows 2000 Server</summary>
        internal static readonly Version OS_VERSION_SERVER_2000 = new(5 , 0);

        /// <summary>Windows Server 2003</summary>
        internal static readonly Version OS_VERSION_SERVER_2003 = new(5 , 2);

        /// <summary>Windows Server 2003 R2</summary>
        internal static readonly Version OS_VERSION_SERVER_2003R2 = new(5 , 2);

        /// <summary>Windows Server 2008</summary>
        internal static readonly Version OS_VERSION_SERVER_2008 = new(6 , 0);

        /// <summary>Windows Server 2008 R2</summary>
        internal static readonly Version OS_VERSION_SERVER_2008R2 = new(6 , 1);

        /// <summary>Windows Server 2012</summary>
        internal static readonly Version OS_VERSION_SERVER_2012 = new(6 , 2);

        /// <summary>Windows Server 2012 R2</summary>
        internal static readonly Version OS_VERSION_SERVER_2012R2 = new(6 , 3);

        /// <summary>Windows Server 2016</summary>
        internal static readonly Version OS_VERSION_SERVER_2016 = new(10 , 0 , 14393);

        internal static readonly Version OS_VERSION_SERVER_2019 = new(10 , 0 , 17763);

        internal static readonly Version OS_VERSION_SERVER_2022 = new(10 , 0 , 20348);
        #endregion



        internal static bool IsVistaOrLater => IsNT && CheckOS(OS_VERSION_CLIENT_VISTA);
        internal static bool IsWin7OrLater => IsNT && CheckOS(OS_VERSION_CLIENT_W7);
        internal static bool IsWin8OrLater => IsNT && CheckOS(OS_VERSION_CLIENT_W8);
        internal static bool IsWin10OrLater => IsNT && CheckOS(OS_VERSION_CLIENT_W10);
        internal static bool IsWin11OrLater => IsNT && CheckOS(OS_VERSION_CLIENT_W11);

        /// <summary>Aero UI supports only w7. W8 has flat UI</summary>
        internal static bool IsSupportAeroUI => IsVistaOrLater && !IsWin8OrLater;

        internal static bool IsSupportMetroAppX => IsWin8OrLater;


        /// <summary><c>Starting with Windows 8, the OSVersion Property returns the same major And minor version numbers For all Windows platforms!</c><br/>
        /// Therefore, we do Not recommend that you retrieve the value of this property to determine the operating system version.<br/>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.environment.osversion"/>
        /// </summary>
        public static bool CheckOS ( Version NeedOSVersion )
            => GetVersion().KernelVersion.ToVersion() >= NeedOSVersion;

        internal static void CheckVistaOrLater ()
        {
            if ( !IsVistaOrLater ) throw new NotSupportedException("Требуется минимум Windows Vista!");
        }

        internal static void CheckWin7OrLater ()
        {
            if ( !IsWin7OrLater ) throw new NotSupportedException("Требуется минимум Windows7!");
        }


        #endregion


        internal enum KnownVersionNames : int
        {
            /// <summary>Unsupported OS</summary>
            Unknown = -1,

            NT4 = 0x400,
            WIN2K = 0x500,
            WS03 = 0x502,


            WIN6 = 0x600,
            VISTA = 0x600,
            WS08 = 0x600,
            LONGHORN = 0x600,


            /// <summary>Windows XP</summary>
            WinXP = 0x501,

            /// <summary>Windows 7</summary>
            Win7 = 0x601,

            /// <summary>Windows 8</summary>
            Win8 = 0x602,

            /// <summary>Windows 8.1</summary>
            Win8Point1 = 0x603,

            Win10 = 0xA00,
            Win10_Threshold1 = 10240,
            Win10_Threshold2 = 10586,
            Win10_Redstone1_AnniversaryUpdate,
            Win10_Redstone2_CreatorsUpdate = 15063,
            Win10_Redstone3_FallCreatorsUpdate = 16299,
            Win10_Redstone4 = 17134,
            Win10_Redstone5 = 17763,
            Win10_19H1 = 18362,
            Win10_19H2_Vanadium = 18363,
            Win10_20H1 = 19041,
            Win10_20H2 = 19042,
            Win10_21H1 = 19043,
            Win10_21H2 = 19044,
            Win11_21H2_SunValley = 22000,
        }

        #endregion

    }





    internal static class MSOffice
    {
        public enum OfficeApplications : int
        {
            Excel,
            Word
        }

        public static dynamic CreateOfficeAppInstance ( OfficeApplications oa )
        {
            string sCOMID = oa switch
            {
                OfficeApplications.Excel => "Excel.Application",
                OfficeApplications.Word => "Word.Application",
                _ => throw new ArgumentOutOfRangeException(oa.ToString())
            };


            Type? xlType = Type.GetTypeFromProgID(sCOMID , true);
            _ = xlType ?? throw new Exception(sCOMID + " is not installed!");

            dynamic? rCOMInstance = Activator.CreateInstance(xlType);
            _ = rCOMInstance ?? throw new Exception($"CreateInstance('{xlType}') FAILED!");
            return rCOMInstance!;
        }
    }



    /// <summary>MT safe FPS counter</summary>
    internal class FPSCounter ( string sTitle = "" )
    {
        private ulong _lLastSecondTimestamp = uom.OS.TickCount_64;

        private int _currentSecondFramesCounter = 0;
        private int _fps = 0;
        private readonly string _title = sTitle;
        private readonly EventArgs _lock = new();

        public int FPS { get { lock ( _lock ) { return _fps; } } }
        public string Title { get { lock ( _lock ) { return _title; } } }

        public void Reset ()
        {
            lock ( _lock )
            {
                _lLastSecondTimestamp = CurrentSecond;
                _currentSecondFramesCounter = 0;
                _fps = 0;
            }
        }


        private static ulong CurrentSecond { get => OS.TickCount_64 / 1000L; }



        public void CountFrame ()
        {
            lock ( _lock )
            {
                var lCurrentTimestamp = CurrentSecond;
                if ( lCurrentTimestamp == _lLastSecondTimestamp )
                {
                    _currentSecondFramesCounter++;
                }
                else
                {
                    _fps = _currentSecondFramesCounter;
                    _currentSecondFramesCounter = 1;
                    _lLastSecondTimestamp = lCurrentTimestamp;
                }
            }
        }

        public override string ToString ()
        {
            lock ( _lock )
            {
                return $"FPS{(_title == "" ? "" : $" ({_title})")}: {_fps}";
            }
        }
    }


    namespace AutoDisposable
    {


        /// <summary> Class than automaticaly disposes 1 attached COM value </summary>
        internal class AutoDisposableCOM : AutoDisposableUniversal
        {
            protected readonly Stack<object> COMObjectsToDispose = new();

            public AutoDisposableCOM () : base() => RegisterDisposeCallback(FreeCOMObjects , true);

            /// <summary>Register COM objects which need to be destroyed</summary>
            protected internal void RegisterDisposableCOMObject ( object COMObject )
            {
                _ = COMObject ?? throw new ArgumentNullException(nameof(COMObject));
                if ( COMObjectsToDispose.Contains(COMObject) )
                {
                    throw new ArgumentException($"'{COMObject}' already in dispose list!" , nameof(COMObject));
                }

                COMObjectsToDispose.Push(COMObject);


                //FreeUnmanagedObjects
            }


            protected virtual void FreeCOMObjects ()
            {
                OnBeforeFreeCOMObjects(COMObjectsToDispose);
                while ( COMObjectsToDispose.Count != 0 )
                {
                    var rObjectToKill = COMObjectsToDispose.Pop();
                    Marshal.ReleaseComObject(rObjectToKill);
                }
                OnAfterFreeCOMObjects(COMObjectsToDispose);
            }

            /// <summary>Just template, override if need</summary>            
            protected virtual void OnBeforeFreeCOMObjects ( Stack<object> rCOMObjectsToDispose ) { }

            /// <summary>Just template, override if need</summary>            
            protected virtual void OnAfterFreeCOMObjects ( Stack<object> rCOMObjectsToDispose ) { }
        }



    }



    /// <summary>FakeWindow to provide IWin32Window.Handle </summary>
    internal class Win32Window ( HWND hwnd ) : System.Windows.Forms.IWin32Window, IEqualityComparer<Win32Window>
    {

        public readonly HWND Handle = hwnd;


        public Win32Window ( IntPtr hwnd ) : this((HWND)hwnd) { }


        #region Operators


        public static bool operator == ( Win32Window w1 , Win32Window w2 ) => w1.Handle == w2.Handle;

        public static bool operator != ( Win32Window w1 , Win32Window w2 ) => !(w1 == w2);


        public static implicit operator IntPtr ( Win32Window wnd ) => (IntPtr)wnd.Handle;

        public static implicit operator HandleRef ( Win32Window wnd ) => new(wnd , (IntPtr)wnd.Handle);

        public static implicit operator HWND ( Win32Window wnd ) => wnd.Handle;

        public static implicit operator Win32Window ( HWND hwnd ) => new(hwnd.DangerousGetHandle());


        #endregion


        public bool Equals ( Win32Window? x , Win32Window? y )
        {
            return x is not null && (ReferenceEquals(x , y) || (y is not null && x.Handle == y.Handle));
        }

        public override bool Equals ( object? obj )
            => Equals(this , obj as Win32Window);


        public int GetHashCode ( [DisallowNull] Win32Window wnd )
            => wnd.Handle.GetHashCode();

        public override int GetHashCode ()
            => GetHashCode();

        nint IWin32Window.Handle => (IntPtr)Handle;

    }


    namespace Extensions
    {


        internal static partial class Extensions_Debug_Dump_Win
        {



            internal static string dumpHex ( this uom.WinAPI.memory.WinApiMemory mem ) => mem.DangerousGetHandle().dumpHex(mem.Lenght);



        }


        internal static partial class Extensions_Async_MT
        {


        }


        internal static partial class Extensions_Arrays_Win
        {

            /// <summary>PInvoke memcmp() from msvcrt.dll<br/>
            /// <c>! Most Fastest Method !</c>
            /// </summary>

            internal static bool eCompareArrays_MemCmp ( this byte[] a , byte[] b , ulong bytesToCompare )
                => uom.WinAPI.memory.memcmp(a , b , bytesToCompare) == 0;



            /// <inheritdoc cref="eCompareArrays_MemCmp" />

            internal static bool eCompareArrays_MemCmp ( this byte[] a , byte[] b )
                => a.eCompareArrays_MemCmp(b , (ulong)a.Length);

#if NET6_0_OR_GREATER




            /// <inheritdoc cref="eCompareArrays_MemCmp" />

            internal static bool eCompareArrays_MemCmp2 ( this byte[] a , byte[] b )
            {
                return false;

            }








            /// <summary>Проверяет заканчивается ли массив байт на указанные байты</summary>

            internal static bool eEndsWith ( this byte[] A , byte[] B )
            {
                // Validate buffers are the same length. This also ensures that the count does not exceed the length of either buffer.  
                int elementsToCompare = B.Length;
                if ( A.Length < elementsToCompare )
                {
                    return false;
                }

                if ( A.Length == 0 && elementsToCompare == 0 )
                {
                    return true;
                }

                A = A.TakeLast(elementsToCompare).ToArray();
                bool bEq = A.eCompareArrays_MemCmp(B);
                return bEq;
            }


            /// <inheritdoc cref="eEndsWith" />
            /// <param name="A"></param>
            /// <param name="abFinishHex">Строка байт вида 0D-0A-2B-43-53-51-3A</param>

            internal static bool eEndsWith ( this byte[] A , string abFinishHex )
            {
                var abFinish = abFinishHex.hexStringToBytes();
                bool bEq = A.eEndsWith(abFinish);
                return bEq;
            }
#endif

            /// <summary>Checks if a begins with b (!!! Uses WinAPI MemCmp !!!)</summary>

            internal static bool eStartsWith ( this byte[] A , byte[] B )
            {
                // Validate buffers length. This also ensures that the count does not exceed the length of either buffer.  
                int elementsToCompare = B.Length;
                if ( A.Length < elementsToCompare )
                {
                    return false;
                }

                if ( A.Length == 0 && elementsToCompare == 0 )
                {
                    return true;
                }

                //if (A.Length != elementsToCompare) A = [.. A.Take(elementsToCompare)];
                return A.eCompareArrays_MemCmp(B , (ulong)elementsToCompare);
            }


            /// <inheritdoc cref="eStartsWith" />
            /// <param name="hexString">bytes string like "0D-0A-2B-43-53-51-3A..."</param>

            internal static bool eStartsWith ( this byte[] A , string hexString )
                => A.eStartsWith(hexString.hexStringToBytes());


            /// <summary>Проверяет равна ли строка двоичному образцу (!!! Uses WinAPI MemCmp !!!)</summary>
            /// <param name="sTargetHexString">Строка байт вида 0D-0A-2B-43-53-51-3A</param>

            internal static bool eEqualToHex ( this byte[] A , string sTargetHexString )
            {
                var abTarge = sTargetHexString.hexStringToBytes();
                return A.Length == abTarge.Length && A.eStartsWith(abTarge);
            }



            // aa 12 30 30 30 30 00 ee                           Є.0000.о         
            /// <summary>
            /// 
            /// </summary>
            /// <param name="templateHexString">00-00-XX-01-aa</param>
            /// <returns></returns>

            [Obsolete("Must be rewrited with memoryspans" , true)]
            internal static byte[] eSearchTemplate ( this byte[] a , string templateHexString )
            {
                char C_SEP = constants.SystemDefaultHexByteSeparator.Value; // Получаем символ-разделитель
                templateHexString = templateHexString.normalizeHexString(null , C_SEP);
                string sSourceStringHex = a.eToStringHex();
                var sourceHexBytes = sSourceStringHex.Split(C_SEP);
                var templateHexBytes = templateHexString.Split(C_SEP);

                if ( sourceHexBytes.Length < templateHexBytes.Length )
                {
                    return [];
                }

                int iEnd = sourceHexBytes.Length - templateHexBytes.Length;
                for ( int iSourceCharPos = 0, loopTo = iEnd ; iSourceCharPos <= loopTo ; iSourceCharPos++ )
                {
                    string sourceChar = sourceHexBytes[ iSourceCharPos ];
                    if ( (sourceChar ?? "") == (templateHexBytes[ 0 ] ?? "") )
                    {
                        // Проверяем все оставшиеся символы на вхождение
                        int iTemplateBytesCount = templateHexBytes.Length;
                        int iFound = 0;
                        for ( int iSubScanChar = 0 ; iSubScanChar <= iTemplateBytesCount ; iSubScanChar++ )
                        {
                            int iFullCharIndex = iSourceCharPos + iSubScanChar;
                            string sTemplateCompareChar = templateHexBytes[ iSubScanChar ];
                            string sSourceCompareChar = sourceHexBytes[ iFullCharIndex ];
                            if ( sTemplateCompareChar == "XX" || sTemplateCompareChar == "X" )// Шаблонный символ - пропускаем
                            {
                                iFound++;
                            }
                            else if ( (sTemplateCompareChar ?? "") != (sSourceCompareChar ?? "") )// Сравниваем
                            {
                                break;
                            }
                            else
                            {
                                iFound++;
                            }
                        }

                        if ( iFound == iTemplateBytesCount ) // найдено!
                        {
                            var abFound = a.eTakeFrom(iSourceCharPos);
                            abFound = abFound.Take(iTemplateBytesCount).ToArray();
                            return abFound;
                        }
                    }
                }

                return [];
            }


        }


        internal static partial class Extensions_SystemCollections
        {

            extension<TKey, TValue> ( System.Collections.Concurrent.ConcurrentDictionary<TKey , TValue> source ) where TKey : notnull
            {
                /// <inheritdoc cref="System.Collections.Concurrent.ConcurrentDictionary(TKey, TValue).AddOrUpdate" />
                public void addOrUpdate ( TKey key , TValue newValue )
                    => source.AddOrUpdate(key , newValue , ( _ , oldValue ) => newValue);

                public bool remove ( TKey key )
                    => source.Remove(key , out _);
            }


        }



        internal static partial class Extensions_Serialize_Binary
        {


            #region Binary Serialization



            internal static void eSerializeBinary ( this object SerializableObject , string FileName )
            {
                using FileStream SM = FileName.eToFileInfo()!.OpenWrite();
                SerializableObject.eSerializeBinary(SM);
                SM.Flush();
            }


            internal static void eSerializeBinary ( this object SerializableObject , Stream SM )
            {
                byte[] abData = SerializableObject.eSerializeBinary();
                SM.Write(abData , 0 , abData.Length);
            }


            internal static byte[] eSerializeBinary ( this object SerializableObject )
            {
                using MemoryStream ms = new();
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(ms , SerializableObject);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                return ms.eReadAllBytes();
            }


            internal static T? eDeSerializeBinary<T> ( this Stream SM , T? defaultValue = default , bool ThrowExceptionOnError = false )
            {
                try
                {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    return (T)new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(SM);
#pragma warning restore SYSLIB0011 // Type or member is obsolete					
                }
                catch
                {
                    if ( ThrowExceptionOnError )
                    {
                        throw;
                    }

                    return defaultValue;
                }
            }



            internal static T? eDeSerializeBinary<T> ( this byte[] SerializedData , T? defaultValue = default , bool ThrowExceptionOnError = false )
            {
                using MemoryStream ms = new(SerializedData);
                return ms.eDeSerializeBinary(defaultValue , ThrowExceptionOnError);
            }



            internal static T? eDeSerializeBinary<T> ( this FileInfo File , T? defaultValue = default , bool ThrowExceptionOnError = false )
            {
                using FileStream fs = File.OpenRead();
                return fs.eDeSerializeBinary(defaultValue , ThrowExceptionOnError);
            }

            #endregion


        }


        internal static partial class Extensions_Math
        {

            internal static float CalculatePercentFrom ( this uint value , uint total )
                => (float)(value / (double)total);

            internal static float CalculatePercentFrom ( this int value , int total )
                => (float)(value / (double)total);

        }



        internal static partial class Extensions_Registry
        {



            public static RegistryKey eHiveToKey ( this RegistryHive Hive )
                => Hive switch
                {
                    RegistryHive.ClassesRoot => Registry.ClassesRoot,
                    RegistryHive.CurrentConfig => Registry.CurrentConfig,
                    RegistryHive.CurrentUser => Registry.CurrentUser,
#if NET6_0_WINDOWS
						case RegistryHive.DynData=> Registry.DynData,
#endif
                    RegistryHive.LocalMachine => Registry.LocalMachine,
                    RegistryHive.PerformanceData => Registry.PerformanceData,
                    RegistryHive.Users => Registry.Users,
                    _ => throw new ArgumentOutOfRangeException(nameof(Hive))
                };



            public static RegistryKey eOpenBaseKey ( this RegistryHive Hive , RegistryView View = RegistryView.Default )
                => RegistryKey.OpenBaseKey(Hive , View);






            public static (bool ValueFound, RegistryValueKind Kind, object? Value)
            eGetValueExt (
                this RegistryKey key ,
                string? valueName ,
                object? defaultValue ,
                RegistryValueOptions rvo = RegistryValueOptions.DoNotExpandEnvironmentNames )
            {
                valueName ??= "";
                try
                {
                    object? objValue = key?.GetValue(valueName , null , rvo);
                    _ = objValue ?? throw new Exception($"RegKey.GetValue('{valueName}') Failed! RegKey = '{key}'!");
                    return (true, key?.GetValueKind(valueName) ?? RegistryValueKind.Unknown, objValue);
                }
                catch { return (false, RegistryValueKind.Unknown, defaultValue); }
            }


            [DebuggerNonUserCode, DebuggerStepThrough]

            public static (bool ValueFound, RegistryValueKind Kind, T? Value) eGetValueT<T> ( this RegistryKey key , string ValueName , T? defaultValue )
            {
                try
                {
                    var (ValueFound, Kind, Value) = key.eGetValueExt(
                           ValueName ,
                           defaultValue ,
                            (typeof(T) == typeof(string))
                                ? RegistryValueOptions.DoNotExpandEnvironmentNames
                                : RegistryValueOptions.None
                            );

                    if ( ValueFound && (null != Value) )
                    {
                        return (true, Kind, (T)Value);
                    }
                }
                catch { }//ignore any errors
                return (false, RegistryValueKind.Unknown, defaultValue);
            }



            public static string? eGetValue_String ( this RegistryKey? key , string ValueName , string? defaultValue = null )
                => key?.eGetValueT<string>(ValueName , defaultValue).Value;





            public static string eGetValue_StringOrEmpty ( this RegistryKey? key , string ValueName , string? defaultValue = null )
                => key?.eGetValue_String(ValueName , defaultValue)
                ?? string.Empty;


            public enum RegOptions : int
            {
                None = 0,
                KEY_WOW64_64KEY = 0x100,
                KEY_WOW64_32KEY = 0x200,

                ReadKey = 131097,
                WriteKey = 131078
            }
            /*
	[DllImport(Lib.AdvApi32, SetLastError = true, ExactSpelling = false, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
	internal static extern Win32Error RegRenameKey (
	this IntPtr hKey,
	[MarshalAs(UnmanagedType.LPTStr)] string? lpSubKeyName,
	[MarshalAs(UnmanagedType.LPTStr)] string lpNewKeyName);
			 */

            /// <summary>Changes the name of the specified registry key.
            /// <br></br>
            /// https://learn.microsoft.com/en-us/windows/win32/api/winreg/nf-winreg-regrenamekey
            /// </summary>
            /// <param name="hKey">A handle to an open registry key. The key must have been opened with the KEY_WRITE 
            /// <br></br>
            /// access right. For more information, see Registry Key Security and Access Rights.
            /// <br></br>
            /// This handle is returned by the RegCreateKeyEx or RegOpenKeyEx function, or it
            ///  can be one of the predefined keys.
            /// </param>
            /// <param name="lpSubKeyName"></param>
            /// <param name="lpNewKeyName"></param>
            /// <returns></returns>
            [DllImport("advapi32.dll" , CharSet = CharSet.Auto)]
            public static extern Win32Error RegRenameKey ( this HKEY hKey , string lpSubKeyName , string lpNewKeyName );


            /// <summary>Work ONLY on local registry, dont work on remote connected registry!</summary>
            public static void eRenameSubKey ( this RegistryKey KeyParent , string name , string newName )
            {
                //RegistryKey _keyHKLM = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
                //KeyParent = _keyHKLM?.OpenSubKey("_uom_Test", true)!;
                if ( newName.isNullOrWhiteSpace ) throw new ArgumentNullException(nameof(newName));

                RegRenameKey(KeyParent.Handle.DangerousGetHandle() ,
                                name ,
                                newName).ThrowIfFailed();

                KeyParent.Flush();
            }


            public static void eRenameSubKeyViaCopy ( this RegistryKey KeyParent , string Name , string newName )
            {
                if ( newName.isNullOrWhiteSpace ) throw new ArgumentNullException(nameof(newName));

                newName = newName.Trim();
                Name = Name.Trim();

                if ( newName == Name )
                    return;//Already has this name

                if ( newName.isDifferOnlyByCase(Name) )
                    throw new ArgumentException("New key name differ only in case!" , nameof(newName));


                KeyParent.eCopySubkey(Name , newName , false);
                KeyParent.DeleteSubKeyTree(Name , false);
                KeyParent.Flush();
            }


            public static void eCopySubkey ( this RegistryKey KeyParent , string subKeyName , string newSubKeyName , bool owerwriteOnExist )
            {
                if ( string.IsNullOrWhiteSpace(subKeyName) )
                    throw new ArgumentNullException(nameof(subKeyName));

                if ( string.IsNullOrWhiteSpace(newSubKeyName) )
                    throw new ArgumentNullException(nameof(newSubKeyName));

                subKeyName = subKeyName.Trim();
                newSubKeyName = newSubKeyName.Trim();

                if ( newSubKeyName == subKeyName )
                    throw new ArgumentException($"{nameof(newSubKeyName)} = {nameof(subKeyName)}!");

                if ( subKeyName.isDifferOnlyByCase(newSubKeyName) )
                    throw new ArgumentException($"The {nameof(newSubKeyName)} and {nameof(subKeyName)} differ only in case!" , nameof(newSubKeyName));

                RegistryKey? keyOld = KeyParent.OpenSubKey(subKeyName , false);
                if ( keyOld == null )
                {
                    throw new Exception($"Failed to open key '{KeyParent}\\{subKeyName}'!");
                }

                if ( KeyParent.GetSubKeyNames().Contains(newSubKeyName) )
                {
                    if ( !owerwriteOnExist )
                    {
                        throw new Exception($"Key name '{newSubKeyName}' already exist!");
                    }

                    KeyParent.DeleteSubKeyTree(newSubKeyName , false);
                }
                using RegistryKey keyNew = KeyParent.CreateSubKey(newSubKeyName , true);
                keyOld.eCopyTo(keyNew);
            }


            public static void eCopyTo ( this RegistryKey keyOld , RegistryKey keyNew )
            {
                //Copy All Values
                var aValueNames = keyOld.GetValueNames();
                foreach ( var valueName in aValueNames )
                {
                    var kind = keyOld.GetValueKind(valueName);
                    RegistryValueOptions rvo = (kind == RegistryValueKind.ExpandString)
                        ? RegistryValueOptions.None
                        : RegistryValueOptions.DoNotExpandEnvironmentNames;

                    object? val = keyOld.GetValue(valueName , null , rvo);
                    if ( val != null )
                    {
                        keyNew.SetValue(valueName , val! , kind);
                    }
                }

                if ( !aValueNames.Where(s => s.Length < 1).Any() )//Default value not exist
                {
                    if ( keyNew.GetValueNames().Where(s => s.Length < 1).Any() )
                    {
                        keyNew.DeleteValue("" , false);
                    }
                }
                keyNew.Flush();

                //Copy Sub Keys
                var aKeyNames = keyOld.GetSubKeyNames();
                foreach ( var keyName in aKeyNames )
                {
                    using RegistryKey? keyChildOld = keyOld.OpenSubKey(keyName , false);
                    using RegistryKey keyChildNew = keyNew.CreateSubKey(keyName , true);
                    keyChildOld?.eCopyTo(keyChildNew);
                }
                keyNew.Flush();
            }


            public static void eExport ( this RegistryKey keyOld , string path )
            {
                SystemPrivilege.Backup.AdjustProcessPrivilegeAndCloseToken();

                AdvApi32.RegSaveKeyEx(
                    keyOld.Handle.DangerousGetHandle() ,
                    path ,
                    null ,
                    AdvApi32.REG_SAVE.REG_LATEST_FORMAT)
                    .ThrowIfFailed();
            }




            public static Int32? eGetValue_Int32 ( this RegistryKey? hRegKey , string ValueName , Int32 defaultValue = 0 )
                => hRegKey?.eGetValueT<Int32>(ValueName , defaultValue).Value;



            public static bool? eGetValue_Bool ( this RegistryKey? hRegKey , string ValueName , bool defaultValue = false )
                => 1 == eGetValue_Int32(hRegKey , ValueName , defaultValue ? 1 : 0);


            private static void eSetValueObject ( this RegistryKey hRegKey , string name , object? value , RegistryValueKind kind )
            {
                if ( null == value )
                {
                    hRegKey.DeleteValue(name , false);
                }
                else
                {
                    hRegKey.SetValue(name , value , kind);
                }
            }




            #region Set Value


            /// <summary>if value == null, reg value will be deleted!</summary>
            public static void eSetValue<T> ( this RegistryKey hRegKey , string name , T? value ) where T : struct
            {
                if ( null == value || !value.HasValue )
                {
                    hRegKey.eSetValueObject(name , null , RegistryValueKind.None);//Delete Value
                    return;
                }

                RegistryValueKind kind = RegistryValueKind.Unknown;
                object valueToWrite;

                //sbyte, byte, short, ushort, int, uint, long, ulong, char, float, double, decimal, or bool
                switch ( value )
                {
                    case Int32 i32: valueToWrite = i32; kind = RegistryValueKind.DWord; break;
                    case Int64 i64: valueToWrite = i64; kind = RegistryValueKind.QWord; break;
                    case bool b: valueToWrite = b ? 1 : 0; kind = RegistryValueKind.DWord; break;
                    case Enum e: valueToWrite = e; kind = RegistryValueKind.DWord; break;
                    case byte[] data: valueToWrite = data; kind = RegistryValueKind.Binary; break;
                    case byte data: valueToWrite = data; kind = RegistryValueKind.DWord; break;
                    case sbyte data: valueToWrite = data; kind = RegistryValueKind.DWord; break;
                    case short data: valueToWrite = data; kind = RegistryValueKind.DWord; break;
                    case ushort data: valueToWrite = data; kind = RegistryValueKind.DWord; break;
                    case uint data: valueToWrite = data; kind = RegistryValueKind.DWord; break;
                    case ulong data: valueToWrite = data; kind = RegistryValueKind.QWord; break;
                    case char data: valueToWrite = data; kind = RegistryValueKind.String; break;
                    case DateTime dt: valueToWrite = dt.ToBinary(); kind = RegistryValueKind.QWord; break;

                    // float, double, decimal
                    default: throw new ArgumentOutOfRangeException($"Unknown type of '{nameof(value)}' = '{typeof(T)}'");
                }
                hRegKey.eSetValueObject(name , valueToWrite , kind);
            }

            /// <summary>if value == null, value will be deleted!</summary>
            public static void eSetValueString ( this RegistryKey hRegKey , string name , string? value )
            {
                if ( null == value )
                {
                    hRegKey.eSetValueObject(name , null , RegistryValueKind.None);//Delete Value
                    return;
                }
                hRegKey.SetValue(name , value! , RegistryValueKind.String);
            }

            public static void eSetValueStrings ( this RegistryKey hRegKey , string name , string[]? value )
            {
                if ( null == value )
                {
                    hRegKey.eSetValueObject(name , null , RegistryValueKind.None);//Delete Value
                    return;
                }
                hRegKey.SetValue(name , value! , RegistryValueKind.MultiString);
            }


            #endregion


            /// <param name="fullPath">HKEY_CLASSES_ROOT/xxx/yyyy...</param>

            public static RegistryKey? eRegOpenByFullPath ( this string fullPath , bool writable )
            {
                string[] pathParts = fullPath.Split('\\');
                string rootKeyName = pathParts[ 0 ];


                RegistryKey keyRoot = rootKeyName switch
                {
                    "HKEY_CLASSES_ROOT" => Registry.ClassesRoot,
                    "HKEY_CURRENT_USER" => Registry.CurrentUser,
                    "HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
                    "HKEY_USERS" => Registry.Users,
                    _ => throw new ArgumentOutOfRangeException(nameof(rootKeyName))
                };

                fullPath = string.Join(@"\" , pathParts.eTakeFrom(1));
                return keyRoot.OpenSubKey(fullPath , writable);
            }


            #region https://www.codeproject.com/Articles/16343/Copy-and-Rename-Registry-Keys


            /// <summary>
            /// Renames a subkey of the passed in registry key since the Framework totally forgot to include such a handy feature.
            /// </summary>
            /// <param name="regKey">The RegistryKey that contains the subkey 
            /// you want to rename (must be writeable)</param>
            /// <param name="subKeyName">The name of the subkey that you want to rename
            /// </param>
            /// <param name="newSubKeyName">The new name of the RegistryKey</param>
            /// <returns>True if succeeds</returns>
            public static bool RenameSubKey ( this RegistryKey parentKey , string subKeyName , string newSubKeyName )
            {
                CopyKey(parentKey , subKeyName , newSubKeyName);
                parentKey.DeleteSubKeyTree(subKeyName);
                return true;
            }

            /// <summary>
            /// Copy a registry key.  The parentKey must be writeable.
            /// </summary>
            public static bool CopyKey ( this RegistryKey parentKey , string keyNameToCopy , string newKeyName )
            {
                //Create new key
                RegistryKey destinationKey = parentKey.CreateSubKey(newKeyName);

                //Open the sourceKey we are copying from
                RegistryKey sourceKey = parentKey.OpenSubKey(keyNameToCopy)!;

                RecurseCopyKey(sourceKey , destinationKey);

                return true;
            }

            private static void RecurseCopyKey ( this RegistryKey sourceKey , RegistryKey destinationKey )
            {
                //copy all the values
                foreach ( string valueName in sourceKey.GetValueNames() )
                {
                    var objValue = sourceKey.GetValue(valueName);
                    RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
#pragma warning disable CS8604 // Possible null reference argument.
                    destinationKey.SetValue(valueName , objValue , valKind);
#pragma warning restore CS8604 // Possible null reference argument.
                }

                //For Each subKey 
                //Create a new subKey in destinationKey 
                //Call myself 
                foreach ( string sourceSubKeyName in sourceKey.GetSubKeyNames() )
                {
                    var sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName)!;
                    var destSubKey = destinationKey.CreateSubKey(sourceSubKeyName)!;
                    RecurseCopyKey(sourceSubKey , destSubKey);
                }
            }


            #endregion

        }


        internal static partial class Extensions_COM
        {


            internal static T? createCOMInstance<T> ( this string ProgID ) => (T?)Activator.CreateInstance(Type.GetTypeFromProgID(ProgID)!);


        }


        internal static partial class Extensions_WinAPI_MemoryPtr
        {


            internal static bool CloseHandle ( this IntPtr H )
                => !H.isValid || Kernel32.CloseHandle(H);



            internal static Microsoft.Win32.SafeHandles.SafeFileHandle eToSafeFileHandle ( this IntPtr hFile , bool ownsHandle = true )
            {
                hFile.isValid.ThrowLastWin32ErrorIfFailed(true , "CreateFile");
                return new SafeFileHandle(hFile , ownsHandle);
            }



            /// <summary>Structure To MemoryBlock</summary>
            internal static uom.WinAPI.memory.WinApiMemory toMemoryBlock<T> ( this T rStructure ) where T : struct
            {
                uom.WinAPI.memory.WinApiMemory mem = new(Marshal.SizeOf(rStructure));
                rStructure.eStructureToPtr(mem.DangerousGetHandle()); // Записываем в буфер всю структуру
                return mem;
            }

            /// <summary>Structure To Bytes</summary>
            internal static byte[] toBytes<T> ( this T rStructure ) where T : struct
            {
                using var rMem = rStructure.toMemoryBlock();
                return rMem.ToBytes();
            }



            /// <summary>Последовательно читаем массив байт в массив одинаковых структур</summary>
            /// <param name="abData">Исходный массив</param>
            /// <param name="iCount">Количество структур для чтения</param>

            internal static T[] eToStructuresSequentially<T> ( this byte[] abData , int iCount , int iOffset = 0 ) where T : struct
            {
                // Размер одного элемента структуры
                int iSize1 = Marshal.SizeOf(typeof(T));
                // Полный размер всех структур
                int lTotalSize = iSize1 * iCount;
                if ( iOffset + lTotalSize > abData.Length )
                {
                    throw new IndexOutOfRangeException("(iOffset + lTotalSize) > abData.Length");
                }

                // Копируем весь блок памяти в массив
                var lFound = new List<T>();
                for ( int I = 1, loopTo = iCount ; I <= loopTo ; I++ )
                {
                    var rStruct = abData.eToStructure<T>(iOffset);
                    iOffset += iSize1;
                    lFound.Add(rStruct);
                }

                return lFound.ToArray();
            }


            /// <summary>Считать байты массива в структуру</summary>
            /// <param name="abData">Исходный массив</param>
            /// <param name="iOffset">Смещение в массиве, с которого начинаем читать</param>
            /// <returns></returns>

            internal static T eToStructure<T> ( this byte[] abData , int iOffset = 0 ) where T : struct
            {
                int iStructLen = Marshal.SizeOf(typeof(T));
                byte[] abArrayToCast;// = Array.Empty<byte>();
                if ( iOffset == 0 && abData.Length == iStructLen )
                {
                    // Копируем весь блок памяти в массив
                    abArrayToCast = abData;
                }
                else
                {
                    if ( iOffset + iStructLen > abData.Length )
                    {
                        throw new IndexOutOfRangeException("Структура выходит за пределы массива, с учётом сдвига!");
                    }

                    // Копируем часть массива
                    var abTmpBuffer = new byte[ iStructLen ];
                    Array.Copy(abData , iOffset , abTmpBuffer , 0 , iStructLen);
                    abArrayToCast = abTmpBuffer;
                }

                using var LMB = new uom.WinAPI.memory.WinApiMemory(abArrayToCast);
                return LMB.DangerousGetHandle().eToStructure<T>();
            }


#if NET

            public static unsafe T** AsPointer<T> ( ref T* value ) where T : unmanaged
            {
#pragma warning disable CS9198 // Reference kind modifier of parameter doesn't match the corresponding parameter in target.
                delegate*< ref byte , void* > d = &Unsafe.AsPointer;
#pragma warning restore CS9198 // Reference kind modifier of parameter doesn't match the corresponding parameter in target.
                return ((delegate*< ref T* , T** >)d)(ref value);
            }

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
            public static unsafe T* PointerTo<T> ( ReadOnlySpan<T> from ) where T : struct
                => *(T**)&from;

            public static unsafe T* PointerTo<T> ( Span<T> from ) where T : struct
                => *(T**)&from;
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

#endif

        }



        internal static partial class Extensions_WinAPI_Errors
        {



            #region ThrowLastWin32Exception




            public static void ThrowWin32Error ( this errors.Win32Errors error , string? errorDescription = null )
                => new Win32Exception((int)error , errorDescription).eThrow();



            public static void ThrowLastWin32ErrorIfFailed ( this errors.Win32Errors error )
            {
                var wex = errors.LastWin23Error();
                if ( wex.NativeErrorCode != errors.Win32Errors.ERROR_SUCCESS ) wex.eThrow();

                //$"ThrowLastWin23ErrorAssert skip: {wex.Message}\n\t{wex.StackTrace}".eDebugWriteLine ();
            }


            /// <summary>Вызывает ошибку, только если условие истинно</summary>
            /// <param name="b">Если условие верно - вызывается ошибка</param>

            internal static void ThrowLastWin32ErrorIfFailed ( this bool b , bool useCallerExpression = true , [CallerArgumentExpression(nameof(b))] string? failedAction = null )
            {
                if ( b ) return;
                if ( useCallerExpression )
                    Win32Error.ThrowLastError(failedAction);
                else
                    Win32Error.ThrowLastError();
            }


            internal static void ThrowLastWin32ErrorIfTrue ( this bool b , bool useCallerExpression = true , [CallerArgumentExpression(nameof(b))] string? failedAction = null )
                => (!b).ThrowLastWin32ErrorIfFailed(useCallerExpression , failedAction);



            /// <summary>Вызывает ошибку, только если код не равен указанному</summary>
            /// <param name="IgnoreErrorCode">Код игнорируемой ошибки</param>

            public static void ThrowLastWin32ErrorUnless ( this errors.Win32Errors ignoreErrorCode )
            {
                var wex = errors.LastWin23Error();
                if ( wex.NativeErrorCode != ignoreErrorCode ) wex.eThrow();
                //$"ThrowLastWin23ErrorAssert skip: {wex.Message}\n\t{wex.StackTrace}".eDebugWriteLine ();
            }


            /// <summary>Вызывает ошибку, только если условие истинно</summary>
            /// <param name="b">Если условие верно - вызывается ошибка</param>

            internal static void ThrowLastWin32ErrorIfFailedUnless ( this bool b , errors.Win32Errors ignoreError , out errors.Win32Errors actualError , bool useCallerExpression = true , [CallerArgumentExpression(nameof(b))] string? failedAction = null )
            {
                actualError = errors.Win32Errors.ERROR_SUCCESS;
                if ( b ) return;

                var w32Err = Win32Error.GetLastError();
                actualError = (errors.Win32Errors)(uint)w32Err;

                if ( useCallerExpression )
                    w32Err.ThrowUnless((uint)ignoreError , failedAction);
                else
                    w32Err.ThrowUnless((uint)ignoreError);

            }

            /// <summary>Вызывает ошибку, только если условие истинно</summary>
            /// <param name="b">Если условие верно - вызывается ошибка</param>

            internal static void ThrowLastWin32ErrorIfFailedUnless ( this bool b , errors.Win32Errors ignoreError , bool useCallerExpression = true , [CallerArgumentExpression(nameof(b))] string? failedAction = null )
                => b.ThrowLastWin32ErrorIfFailedUnless(ignoreError , out var actualError , useCallerExpression , failedAction);


            /// <summary>Вызывает ошибку если указатель пуст</summary>

            internal static IntPtr ThrowLastWin32ErrorIfNull ( this IntPtr ptr , bool useCallerExpression = true , [CallerArgumentExpression(nameof(ptr))] string? failedAction = null )
            {
                ptr.isValid.ThrowLastWin32ErrorIfFailed(useCallerExpression , failedAction);
                return ptr;
            }



            internal static T ThrowLastWin32ErrorIfInvalid<T> ( this T handle , bool useCallerExpression = true , [CallerArgumentExpression(nameof(handle))] string? failedAction = null ) where T : SafeHandleZeroOrMinusOneIsInvalid
            {
                handle.IsInvalid.ThrowLastWin32ErrorIfTrue(useCallerExpression , failedAction);
                return handle;
            }



            internal static T ThrowLastWin32ErrorIfInvalidUnless<T> ( this T handle , errors.Win32Errors ignoreError , out errors.Win32Errors actualError , bool useCallerExpression = true , [CallerArgumentExpression(nameof(handle))] string? failedAction = null ) where T : SafeHandleZeroOrMinusOneIsInvalid
            {
                (!handle.IsInvalid).ThrowLastWin32ErrorIfFailedUnless(ignoreError , out actualError , useCallerExpression , failedAction);
                return handle;
            }



            internal static T ThrowLastWin32ErrorIfInvalidUnless<T> ( this T handle , errors.Win32Errors ignoreError , bool useCallerExpression = true , [CallerArgumentExpression(nameof(handle))] string? failedAction = null ) where T : SafeHandleZeroOrMinusOneIsInvalid
            {
                (!handle.IsInvalid).ThrowLastWin32ErrorIfFailedUnless(ignoreError , out var actualError , useCallerExpression , failedAction);
                return handle;
            }


            #endregion





            #region 	   			 TryCatchWin32 



            public static void TryCatchWin32 ( Func<int> operation )
            {
                var funcResult = operation.Invoke();
                if ( funcResult == (int)Win32Error.ERROR_SUCCESS ) return;
                if ( funcResult >= 0 ) new Win32Error((uint)funcResult).ThrowIfFailed();
                throw new Win32Exception(funcResult);
            }



            public static void TryCatchWin32 ( Func<int> operation , string? messageTemplate = null , params object[] messageArgs )
            {
                var funcResult = operation.Invoke();
                if ( funcResult == (int)Win32Error.ERROR_SUCCESS ) return;

                string? err = null;
                if ( messageTemplate.isNotNullOrWhiteSpace )
                    err = messageArgs.Length != 0
                        ? messageTemplate
                        : messageTemplate!.format(messageArgs);

                if ( funcResult >= 0 ) new Win32Error((uint)funcResult).ThrowIfFailed();
                throw new Win32Exception(funcResult , err);
            }



            public static void TryCatchWin32 ( Func<bool> operation )
                => Win32Error.ThrowLastErrorIfFalse(operation.Invoke());



            public static void TryCatchWin32 ( Func<bool> operation , string messageTemplate , params object[] messageArgs )
                => Win32Error.ThrowLastErrorIfFalse(operation.Invoke() , messageTemplate.format(messageArgs));



            #endregion







            #region Win32Exception Throw

            internal static void eThrow ( this Win32Exception WEX )
                => throw WEX;


            /// <summary>Вызывает ошибку, если win32ErrorCode не равно 0 (ERROR_SUCCESS)</summary>
            /// <param name="win32ErrorCode">Проверяемый код ошибки</param>

            internal static void eThrowIfError ( this int win32ErrorCode )
            {
                var eResult = win32ErrorCode.eToWin32Error();
                if ( eResult != WinAPI.errors.Win32Errors.ERROR_SUCCESS )
                {
                    eResult.eToWin32Exception().eThrow();
                }
            }


            /// <summary>Вызывает ошибку, если win32ErrorCode не равно 0 (ERROR_SUCCESS)</summary>
            /// <param name="win32ErrorCode">Проверяемый код ошибки</param>

            internal static void eThrowIfError ( this WinAPI.errors.Win32Errors eResult )
            {
                if ( eResult != WinAPI.errors.Win32Errors.ERROR_SUCCESS )
                {
                    eResult.eToWin32Exception().eThrow();
                }
            }



            internal static void eThrowIf ( this Win32Exception WEX , WinAPI.errors.Win32Errors ThrowErrorCode )
            {
                if ( WEX.eToWin32Error() == ThrowErrorCode )
                {
                    throw WEX;
                }
            }



            internal static void eThrowIfNot ( this Win32Exception WEX , WinAPI.errors.Win32Errors DoNotThrowErrorCode )
            {
                if ( WEX.eToWin32Error() != DoNotThrowErrorCode )
                {
                    throw WEX;
                }
            }



            internal static void eThrowIfNot ( this WinAPI.errors.Win32Errors EX , WinAPI.errors.Win32Errors DoNotThrowErrorCode )
            {
                if ( EX == DoNotThrowErrorCode )
                {
                    return;
                }

                throw new Win32Exception((int)EX);
            }



            #endregion



            internal static string eToLocalizedMessage ( this WinAPI.errors.Win32Errors err )
                => WinAPI.errors.GetErrorMessage(err);



            internal static WinAPI.errors.Win32Errors eToWin32Error ( this Kernel32.WAIT_STATUS ws )
                => (WinAPI.errors.Win32Errors)ws;


            internal static WinAPI.errors.Win32Errors eToWin32Error ( this int iError )
                => (WinAPI.errors.Win32Errors)iError;


            internal static WinAPI.errors.Win32Errors eToWin32Error ( this Win32Exception WEX )
                => (WinAPI.errors.Win32Errors)WEX.NativeErrorCode;


            internal static Win32Exception eToWin32Exception ( this WinAPI.errors.Win32Errors eError )
                => new((int)eError);


            internal static Win32Exception eToWin32Exception ( this int iError )
                => new(iError);


            /*

internal static uom.WinAPI.errors.HResult eToHResult (this IntPtr i)
=> (uom.WinAPI.errors.HResult)((UInt64)i.ToInt64());
			 */


        }


        internal static partial class Extensions_WinAPI_Events
        {


            /// <inheritdoc cref="Kernel32.WaitForSingleObject" />

            public static Kernel32.WAIT_STATUS WaitForSingleObjectInfinite ( this IntPtr hHandle )
                => Kernel32.WaitForSingleObject((HEVENT)hHandle , Vanara.PInvoke.Kernel32.INFINITE);

        }


        internal static partial class Extensions_WinAPI_ProcessThread
        {


            /// <inheritdoc cref="Oleacc.GetProcessHandleFromHwnd" />

            public static Vanara.PInvoke.HPROCESS GetProcessHandleFromHwnd ( this IntPtr hwnd )
                => Oleacc.GetProcessHandleFromHwnd(hwnd);

            /// <inheritdoc cref="GetProcessHandleFromHwnd" />

            public static Vanara.PInvoke.HPROCESS GetProcessHandleFromHwnd ( this IWin32Window wnd )
                => wnd.Handle.GetProcessHandleFromHwnd();



            public static (uint processid, uint threadid) GetWindowThreadProcessId ( this IntPtr hWnd )
            {
                var threadid = User32.GetWindowThreadProcessId(hWnd , out var processid);
                return (processid, threadid);
            }


            #region IsProcedureExported - used to determine if an API function is exported.

            /// <summary>used to determine if an API function is exported.</summary>

            public static bool IsProcedureExported ( this Kernel32.SafeHINSTANCE hModule , string procName , bool noUnloadLib = false )
            {
                var lpProc = hModule.GetProcAddress(procName);
                return lpProc.isValid;
            }

            /// <inheritdoc cref="IsProcedureExported" />

            public static bool IsProcedureExported ( this string libFile , string procName , bool noUnloadLib = false )
            {
                bool needFreeLib = false;

                // check first to see if the module is already mapped into this process.
                var hModule = Kernel32.GetModuleHandle(libFile);

                // need to load module into this process.
                if ( !hModule.isValid )
                {
                    hModule = Kernel32.LoadLibrary(libFile);
                    if ( !hModule.isValid ) throw new Win32Exception();
                    needFreeLib = !noUnloadLib;
                }

                try { return hModule.IsProcedureExported(procName); }
                finally
                {
                    if ( needFreeLib ) hModule.Dispose();// unload library if we loaded it here.
                }

            }


            #endregion




        }


        internal static partial class Extensions_WinAPI_Security
        {


            #region TokenPrivileges



            /// <summary>The OpenProcessToken function opens the access token associated with a process.</summary>
            /// <param name="ProcessHandle">A handle to the process whose access token is opened. The process must have the PROCESS_QUERY_INFORMATION access permission.</param>
            /// <param name="DesiredAccess">Specifies an access mask that specifies the requested types of access to the access token. These requested access types are compared with the discretionary access control list (DACL) of the token to determine which accesses are granted or denied.</param>
            [SecurityCritical]

            public static AdvApi32.SafeHTOKEN OpenProcessToken ( this IntPtr hProcess , AdvApi32.TokenAccess desiredAccess ) //TokenAccessLevels 
            {
                AdvApi32.OpenProcessToken(hProcess , desiredAccess , out var processToken)
                    .ThrowLastWin32ErrorIfFailed();
                return processToken;
            }


            /// <summary>Open Token for Current Process</summary>
            [SecurityCritical]

            public static AdvApi32.SafeHTOKEN OpenCurrentProcessToken ( this AdvApi32.TokenAccess desiredAccess )
                => Kernel32.GetCurrentProcess().DangerousGetHandle().OpenProcessToken(desiredAccess);


            /// <summary>Returns privelege name like <c>SeBackupPrivilege</c> or <c>SeAssignPrimaryTokenPrivilege</c> etc...</summary>
            public static string GetPrivilegeName ( this Vanara.Security.AccessControl.SystemPrivilege pn )
                => $"Se{pn}Privilege";


            /// <summary>Gets localized name for privelege (use current OS UI lang)</summary>

            public static string LookupPrivilegeDisplayName ( this SystemPrivilege pn , out uint LanguageId , string? systemName = null )
            {
                //First get the buffer sizes for string
                uint cchDisplayName = 0;
                string spn = pn.GetPrivilegeName();

                AdvApi32.LookupPrivilegeDisplayName(systemName , spn , null , ref cchDisplayName , out _)
                    .ThrowLastWin32ErrorIfFailedUnless(errors.Win32Errors.ERROR_INSUFFICIENT_BUFFER);

                //Get the value to buffer
                StringBuilder sbDisplayName = new((int)cchDisplayName + 1);
                AdvApi32.LookupPrivilegeDisplayName(systemName , spn , sbDisplayName , ref cchDisplayName , out LanguageId)
                    .ThrowLastWin32ErrorIfFailed();

                return sbDisplayName.ToString();
            }

            /// <summary>Gets localized name for privelege (use current OS UI lang)</summary>

            public static string LookupPrivilegeDisplayName ( this SystemPrivilege pn )
                => pn.LookupPrivilegeDisplayName(out _);



            private static LUID LookupPrivilegeValue ( this string pn , string? systemName = null )
            {
                AdvApi32.LookupPrivilegeValue(systemName , pn , out var Luid)
                    .ThrowLastWin32ErrorIfFailed();

                return Luid;
            }



            public static LUID LookupPrivilegeValue ( this SystemPrivilege pn , string? systemName = null )
                => LookupPrivilegeValue(pn.GetPrivilegeName() , systemName);


            /// <returns>processToken</returns>
            [SecurityCritical]

            public static AdvApi32.SafeHTOKEN AdjustProcessPrivilege ( this string pn )
            {
                /*
				AdvApi32.LUID_AND_ATTRIBUTES[] P = new AdvApi32.LUID_AND_ATTRIBUTES[ 1 ];
				P[ 0 ].Luid = LookupPrivilegeValue (pn);
				P[ 0 ].Attributes = AdvApi32.PrivilegeAttributes.SE_PRIVILEGE_ENABLED;
				 */

                AdvApi32.TOKEN_PRIVILEGES tokenPrivileges = new(
                    LookupPrivilegeValue(pn) ,
                    AdvApi32.PrivilegeAttributes.SE_PRIVILEGE_ENABLED);

                var processToken = OpenCurrentProcessToken(AdvApi32.TokenAccess.TOKEN_QUERY | AdvApi32.TokenAccess.TOKEN_ADJUST_PRIVILEGES);
                AdvApi32.AdjustTokenPrivileges(
                    processToken.DangerousGetHandle() ,
                    false ,
                    tokenPrivileges ,
                    out _)
                    .ThrowIfFailed();

                return processToken;
            }


            /// <returns>processToken</returns>
            [SecurityCritical]

            public static AdvApi32.SafeHTOKEN AdjustProcessPrivilege ( this SystemPrivilege pn )
                => AdjustProcessPrivilege(pn.GetPrivilegeName());


            [SecurityCritical]

            public static void AdjustProcessPrivilegeAndCloseToken ( this SystemPrivilege pn )
            {
                using var token = AdjustProcessPrivilege(pn);
            }



            internal static string eGetPrivilegeDescriptionValue ( this SystemPrivilege pn )
                => pn.GetDescription()!;


            #endregion



            /// <summary>The WELL_KNOWN_SID_TYPE enumeration type is a list of commonly used security identifiers (SIDs). Programs can pass these values to the CreateWellKnownSid function to create a SID from this list.</summary>
            internal enum WELL_KNOWN_SID_TYPE
            {
                WinNullSid = 0,
                WinWorldSid = 1,
                WinLocalSid = 2,
                WinCreatorOwnerSid = 3,
                WinCreatorGroupSid = 4,
                WinCreatorOwnerServerSid = 5,
                WinCreatorGroupServerSid = 6,
                WinNtAuthoritySid = 7,
                WinDialupSid = 8,
                WinNetworkSid = 9,
                WinBatchSid = 10,
                WinInteractiveSid = 11,
                WinServiceSid = 12,
                WinAnonymousSid = 13,
                WinProxySid = 14,
                WinEnterpriseControllersSid = 15,
                WinSelfSid = 16,
                WinAuthenticatedUserSid = 17,
                WinRestrictedCodeSid = 18,
                WinTerminalServerSid = 19,
                WinRemoteLogonIdSid = 20,
                WinLogonIdsSid = 21,
                WinLocalSystemSid = 22,
                WinLocalServiceSid = 23,
                WinNetworkServiceSid = 24,
                WinBuiltinDomainSid = 25,
                WinBuiltinAdministratorsSid = 26,
                WinBuiltinUsersSid = 27,
                WinBuiltinGuestsSid = 28,
                WinBuiltinPowerUsersSid = 29,
                WinBuiltinAccountOperatorsSid = 30,
                WinBuiltinSystemOperatorsSid = 31,
                WinBuiltinPrintOperatorsSid = 32,
                WinBuiltinBackupOperatorsSid = 33,
                WinBuiltinReplicatorSid = 34,
                WinBuiltinPreWindows2000CompatibleAccessSid = 35,
                WinBuiltinRemoteDesktopUsersSid = 36,
                WinBuiltinNetworkConfigurationOperatorsSid = 37,
                WinAccountAdministratorSid = 38,
                WinAccountGuestSid = 39,
                WinAccountKrbtgtSid = 40,
                WinAccountDomainAdminsSid = 41,
                WinAccountDomainUsersSid = 42,
                WinAccountDomainGuestsSid = 43,
                WinAccountComputersSid = 44,
                WinAccountControllersSid = 45,
                WinAccountCertAdminsSid = 46,
                WinAccountSchemaAdminsSid = 47,
                WinAccountEnterpriseAdminsSid = 48,
                WinAccountPolicyAdminsSid = 49,
                WinAccountRasAndIasServersSid = 50,
                WinNTLMAuthenticationSid = 51,
                WinDigestAuthenticationSid = 52,
                WinSChannelAuthenticationSid = 53,
                WinThisOrganizationSid = 54,
                WinOtherOrganizationSid = 55,
                WinBuiltinIncomingForestTrustBuildersSid = 56,
                WinBuiltinPerfMonitoringUsersSid = 57,
                WinBuiltinPerfLoggingUsersSid = 58,
                WinBuiltinAuthorizationAccessSid = 59,
                WinBuiltinTerminalServerLicenseServersSid = 60,
                WinBuiltinDCOMUsersSid = 61,
                WinBuiltinIUsersSid = 62,
                WinIUserSid = 63,
                WinBuiltinCryptoOperatorsSid = 64,
                WinUntrustedLabelSid = 65,
                WinLowLabelSid = 66,
                WinMediumLabelSid = 67,
                WinHighLabelSid = 68,
                WinSystemLabelSid = 69,
                WinWriteRestrictedCodeSid = 70,
                WinCreatorOwnerRightsSid = 71,
                WinCacheablePrincipalsGroupSid = 72,
                WinNonCacheablePrincipalsGroupSid = 73,
                WinEnterpriseReadonlyControllersSid = 74,
                WinAccountReadonlyControllersSid = 75,
                WinBuiltinEventLogReadersGroup = 76,
                WinNewEnterpriseReadonlyControllersSid = 77,
                WinBuiltinCertSvcDComAccessGroup = 78
            }




            /// <summary>The structure represents a security identifier (SID) and its attributes.
            /// SIDs are used to uniquely identify users or groups.</summary>
            [StructLayout(LayoutKind.Sequential)]
            internal partial struct SID_AND_ATTRIBUTES
            {
                public IntPtr Sid;
                public int Attributes;
            }


            /// <summary>The structure indicates whether a token has elevated privileges.</summary>
            [StructLayout(LayoutKind.Sequential)]
            internal partial struct TOKEN_ELEVATION
            {
                public int TokenIsElevated;
            }





            /// <summary>The DuplicateToken function creates an impersonation token, 
            /// which you can use in functions such as SetThreadToken and ImpersonateLoggedOnUser. 
            /// The token created by DuplicateToken cannot be used in the CreateProcessAsUser function, which requires a primary token. 
            /// To create a token that you can pass to CreateProcessAsUser, use the DuplicateTokenEx function.</summary>
            /// <param name="ExistingTokenHandle">A handle to an access token opened with TOKEN_DUPLICATE access.</param>
            /// <param name="ImpersonationLevel">Specifies a SECURITY_IMPERSONATION_LEVEL enumerated type that supplies the impersonation level of the new token.</param>
            internal static AdvApi32.SafeHTOKEN DuplicateToken ( this HTOKEN existingToken , AdvApi32.SECURITY_IMPERSONATION_LEVEL level )
            {
                AdvApi32.DuplicateToken(existingToken , level , out var hToken2)
                    .ThrowLastWin32ErrorIfFailed();

                return hToken2;
            }


            /// <summary>Determine token type: limited, elevated, or default. </summary>
            internal static AdvApi32.TOKEN_ELEVATION_TYPE GetTokenInformation_Elevation ( this HTOKEN hToken )
                => AdvApi32.GetTokenInformation<AdvApi32.TOKEN_ELEVATION_TYPE>(hToken , AdvApi32.TOKEN_INFORMATION_CLASS.TokenElevationType);







            /// <summary>User name without domain</summary>

            internal static string eGetShortName ( this WindowsIdentity U ) => U.User?.LookupAccount().User!;




#if !UWP

#pragma warning disable SYSLIB0003 // Type or member is obsolete

            internal static void DemandFileIOPermission ( this string path , FileIOPermissionAccess eAccess = FileIOPermissionAccess.Read )
                => new FileIOPermission(eAccess , path).Demand();  // Запрос на доступ к файлу
#pragma warning restore SYSLIB0003 // Type or member is obsolete

#endif



            internal static SecurityIdentifier eToSID ( this string SIDString )
                => new(SIDString);



            internal static SecurityIdentifier eToSID ( this NTAccount AC )
                => (SecurityIdentifier)AC.Translate(typeof(SecurityIdentifier));



            internal static SecurityIdentifier eToSID ( this IdentityReference IR )
                => (SecurityIdentifier)IR.Translate(typeof(SecurityIdentifier));



            internal static NTAccount eToNTAccount ( this IdentityReference IR )
                => (NTAccount)IR.Translate(typeof(NTAccount));



            /// <summary>The SecurityReference object's Translate method does work on non-local SIDs but only for domain accounts.
            /// For accounts local To another machine Or In a non-domain setup you would need To PInvoke the Function LookupAccountSid specifying the specific machine name On which the look up needs To be performed.
            /// </summary>

            internal static NTAccount eToNTAccount ( this SecurityIdentifier SID )
                => (NTAccount)SID.Translate(typeof(NTAccount));


            internal enum SID_NAME_USE : int
            {
                SidTypeUser = 1,
                SidTypeGroup,
                SidTypeDomain,
                SidTypeAlias,
                SidTypeWellKnownGroup,
                SidTypeDeletedAccount,
                SidTypeInvalid,
                SidTypeUnknown,
                SidTypeComputer
            }


            /// <summary>Retrieves the account name for SID and the name of the first domain in which this SID is found.</summary>

            internal static (string User, string Domain, NTAccount FQDN) LookupAccount ( this SecurityIdentifier sid , string? systemName = null )
            {
                var abSID = new byte[ sid.BinaryLength ];
                sid.GetBinaryForm(abSID , 0);

                // First call, populate l_UserNameLength and l_DomainLength
                int userNameLength = 0, domainLength = 0;

                AdvApi32.LookupAccountSid(
                    systemName ,
                    abSID ,
                    null ,
                    ref userNameLength ,
                    null ,
                    ref domainLength ,
                    out _)
                    .ThrowLastWin32ErrorIfFailedUnless(WinAPI.errors.Win32Errors.ERROR_INSUFFICIENT_BUFFER);

                // Allocate space
                StringBuilder cbUser = new(userNameLength + 1);  // Need space for terminating chr(0)?
                StringBuilder cbDomain = new(domainLength + 1);

                AdvApi32.LookupAccountSid(
                    systemName ,
                    abSID ,
                    cbUser ,
                    ref userNameLength ,
                    cbDomain ,
                    ref domainLength ,
                    out _)
                    .ThrowLastWin32ErrorIfFailed();

                string user = cbUser.ToString(), domain = cbDomain.ToString();
                var nta = new NTAccount(user , domain);
                return (user, domain, nta);
            }



            internal static SecurityIdentifier eUserNameToSID ( this string user )
            {
                // The easy way, with .NET 2.0 and up, is this:
                var rNTAcc = new NTAccount(user);
                return rNTAcc.eToSID();


                // The hard way, which works when that won't, and works on .NET 1.1 also:

                // [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
                // public static extern bool LookupAccountName([In,MarshalAs(UnmanagedType.LPTStr)] string systemName, [In,MarshalAs(UnmanagedType.LPTStr)] string accountName, IntPtr sid, ref int cbSid, StringBuilder referencedDomainName, ref int cbReferencedDomainName, [Out ] int use);
                // [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
                // internal static extern bool ConvertSidToStringSid(IntPtr sid, [In,Out,MarshalAs(UnmanagedType.LPTStr)] ref string pStringSid);

                // /// <summary>The method converts object name (user, group) into SID string.</summary>
                // /// <param name="name">Object name in form domain\object_name.</param>
                // /// <returns>SID string.</returns>
                // public static string GetSid(string name) {
                // IntPtr _sid = IntPtr.Zero; //pointer to binary form of SID string.
                // int _sidLength = 0;   //size of SID buffer.
                // int _domainLength = 0;  //size of domain name buffer.
                // int _use;     //type of object.
                // StringBuilder _domain = new StringBuilder(); //stringBuilder for domain name.
                // int _error = 0;
                // string _sidString = "";

                // //first call of the function only returns the sizes of buffers (SDI, domain name)
                // LookupAccountName(null, name, _sid, ref _sidLength, _domain, ref _domainLength, [Out ] _use);
                // _error = Marshal.GetLastWin32Error();

                // if (_error != 122) //error 122 (The data area passed to a system call is too small) - normal behaviour.
                // {
                // throw (new Exception(new Win32Exception(_error).Message));
                // } else {
                // _domain = new StringBuilder(_domainLength); //allocates memory for domain name
                // _sid = Marshal.AllocHGlobal(_sidLength); //allocates memory for SID
                // bool _rc = LookupAccountName(null, name, _sid, ref _sidLength, _domain, ref _domainLength, [Out ] _use);

                // if (_rc == false) {
                // _error = Marshal.GetLastWin32Error();
                // Marshal.FreeHGlobal(_sid);
                // throw (new Exception(new Win32Exception(_error).Message));
                // } else {
                // // converts binary SID into string
                // _rc = ConvertSidToStringSid(_sid, ref _sidString);

                // if (_rc == false) {
                // _error = Marshal.GetLastWin32Error();
                // Marshal.FreeHGlobal(_sid);
                // throw (new Exception(new Win32Exception(_error).Message));
                // } else {
                // Marshal.FreeHGlobal(_sid);
                // return _sidString;
                // }
                // }
                // }
                // }
            }


        }




        internal static partial class Extensions_WinAPI_IO
        {

            public static DateTime eToDateTime ( this SYSTEMTIME st , bool Localize = false )
            {
                if ( Localize )
                {
                    // Convert to FILETIME, localize, then convert back to SYSTEMTIME.
                    _ = Kernel32.SystemTimeToFileTime(st , out var ft);
                    _ = Kernel32.FileTimeToLocalFileTime(ft , out ft);
                    _ = Kernel32.FileTimeToSystemTime(ft , out st);
                    return new DateTime(st.wYear , st.wMonth , st.wDay , st.wHour , st.wMinute , st.wSecond , st.wMilliseconds);
                }

                return new DateTime(st.wYear , st.wMonth , st.wDay , st.wHour , st.wMinute , st.wSecond , st.wMilliseconds);
            }


            internal static Kernel32.BY_HANDLE_FILE_INFORMATION GetFileInformationByHandle ( this IntPtr hFile )
            {

                var ss = Kernel32.GetFileAttributes("asdf");



                Kernel32.GetFileInformationByHandle(hFile , out var bhfi)
                    .ThrowLastWin32ErrorIfFailed();
                return bhfi;
            }









            [Obsolete("Устаревший класс! использовать надо многопоточный поисковик многоплатформенный" , true)]

            internal static FileInfo[] eSearchFile (
                this DirectoryInfo Dir ,
                string SearchPattern ,
                bool OnlyForFirstFile = true ,
                bool SkipNTFSJunctions = true ,
                Action<Exception>? ErrorHandler = null )
            {
                var leFound = new List<FileInfo>();
                try // Поиск в текущей папке
                {
                    var aFoundFiles = Dir.GetFiles(SearchPattern , SearchOption.TopDirectoryOnly);
                    if ( aFoundFiles.Length != 0 )
                    {
                        lock ( leFound )
                        {
                            if ( OnlyForFirstFile )
                            {
                                leFound.Add(aFoundFiles.First());
                                return [ .. leFound ];
                            }

                            leFound.AddRange(aFoundFiles);
                        }
                    }
                }
                catch ( Exception ex )
                { ErrorHandler?.Invoke(ex); }



                // Поиск в подкаталогах
                try
                {
                    var aSubDirs = Dir.GetDirectories();
                    var aFoundFiles = aSubDirs.eSearchFile(SearchPattern , OnlyForFirstFile , SkipNTFSJunctions , ErrorHandler);
                    if ( aFoundFiles.Length != 0 )
                    {
                        lock ( leFound )
                        {
                            if ( OnlyForFirstFile )
                            {
                                leFound.Add(aFoundFiles.First());
                                return leFound.ToArray();
                            }

                            leFound.AddRange(aFoundFiles);
                        }
                    }
                }
                catch ( Exception ex )
                { ErrorHandler?.Invoke(ex); }

                return leFound.ToArray();
            }



            internal static void eOpenExplorer ( this FileSystemInfo FI , uom.AppTools.WindowsExplorerPathModes PathMode = uom.AppTools.C_DEFAULT_EXPLORER_MODE )
                => FI.FullName.eOpenExplorer(PathMode);



            internal static void eOpenExplorer ( this string sPath , uom.AppTools.WindowsExplorerPathModes PathMode = uom.AppTools.C_DEFAULT_EXPLORER_MODE )
                => uom.AppTools.StartWinSysTool_Explorer(sPath , PathMode);



            internal static void eOpenURLInBrowser ( this string uri )
            {
                ProcessStartInfo psi = new()
                {
                    UseShellExecute = true ,
                    FileName = uri
                };
                Process.Start(psi);
            }


            extension( FileInfo source )
            {

                internal (string? Description, FileVersionInfo? Version) getFileTitle (
                    string? DefaultTitle = null )
                {
                    DefaultTitle ??= source.FullName;

                    if ( !source.Exists ) return (DefaultTitle, null);

                    string title = source.Name;
                    FileVersionInfo? Ver = FileVersionInfo.GetVersionInfo(source.FullName);
                    if ( !(Ver?.FileDescription?.isNullOrWhiteSpace ?? true) ) title = Ver!.FileDescription!;

                    return (title, Ver);
                }


            }

            internal static DirectoryInfo eToDirectoryInfoOrDefaultDir ( this string dir , Environment.SpecialFolder defaultDir = Environment.SpecialFolder.DesktopDirectory )
            {
                if ( dir.isNullOrWhiteSpace || !Directory.Exists(dir) )
                {
                    dir = Environment.GetFolderPath(defaultDir);
                }

                return new(dir.Trim());
            }




            internal static FileAttributes eToNETFileAttributes ( this FileFlagsAndAttributes winFileAttrs )
                => (FileAttributes)winFileAttrs;



            internal static Kernel32.FileAccess ToNative ( this System.IO.FileAccess access )
            {
                Kernel32.FileAccess result = 0;
                if ( access.HasFlag(System.IO.FileAccess.Read) ) result |= Kernel32.FileAccess.GENERIC_READ;
                if ( access.HasFlag(System.IO.FileAccess.Write) ) result |= Kernel32.FileAccess.GENERIC_WRITE;
                return result;
            }



            [Obsolete("Use new multithread scanner" , true)]

            internal static FileInfo[] eSearchFile (
                this DirectoryInfo[] aDirs ,
                string SearchPattern ,
                bool OnlyForFirstFile = true ,
                bool SkipNTFSJunctions = true ,
                Action<Exception>? ErrorHandler = null )
            {
                throw new NotImplementedException();


                //var leFound = new List<FileInfo>();
                //foreach (var rDir in aDirs)
                //{
                //    if (SkipNTFSJunctions && rDir.FullName.eIsNTFS_SymLink_Win32())
                //    {
                //        // Skip
                //    }
                //    else
                //    {
                //        var aFoundFiles = rDir.eSearchFile(SearchPattern, OnlyForFirstFile, SkipNTFSJunctions, ErrorHandler);
                //        if (aFoundFiles.Any())
                //        {
                //            lock (leFound)
                //            {
                //                if (OnlyForFirstFile)
                //                {
                //                    leFound.Add(aFoundFiles.First());
                //                    return leFound.ToArray();
                //                }

                //                leFound.AddRange(aFoundFiles);
                //            }
                //        }
                //    }
                //}

                //return leFound.ToArray();
            }


            /// <summary>WinAPI FILE_ATTRIBUTE_REPARSE_POINT</summary>

            internal static bool eIsNTFS_SymLink_WinAPI ( this string sPath )
                => Kernel32.GetFileAttributes(sPath).HasFlag(FileFlagsAndAttributes.FILE_ATTRIBUTE_REPARSE_POINT);


            /// <summary>Gets FriendlyTypeName from ClassesRoot by file extension</summary>
            internal static string eGetFileTypeDescriptionFromRegistry ( this FileInfo fi )
            {
                string ext = fi.Extension.ToLower().Trim();
                if ( ext.isNullOrWhiteSpace ) return string.Empty;

                string displayName = string.Empty;
                try
                {
                    const string CS_FRIENDLY_TYPE_NAME = "FriendlyTypeName";

                    using var keyExtensionClass = Registry.ClassesRoot.OpenSubKey(ext , RegistryKeyPermissionCheck.ReadSubTree); // HKEY_CLASSES_ROOT\.ani
                    if ( keyExtensionClass == null ) return displayName;

                    var typeKeyName = keyExtensionClass.GetValue(null , null)?.ToString() ?? string.Empty; // anifile
                    if ( typeKeyName.isNullOrWhiteSpace ) return displayName;

                    using var keyFileType = Registry.ClassesRoot.OpenSubKey(typeKeyName , false); // HKEY_CLASSES_ROOT\anifile
                    if ( keyFileType == null ) return string.Empty;

                    displayName = keyFileType.GetValue(null , string.Empty)?.ToString() ?? string.Empty;  //CorelDRAW 2020 Graphic


                    {
                        var friendlyTypeName = keyFileType.GetValue(CS_FRIENDLY_TYPE_NAME , string.Empty)?.ToString() ?? string.Empty; //@%SystemRoot%\system32\main.cpl,-2000
                        if ( friendlyTypeName.isNullOrWhiteSpace ) return displayName;
                        displayName = friendlyTypeName;
                    }


                    if ( displayName.StartsWith('@')
                        && displayName.TryParseWin32ResourcePath(out var resFile , out var resID)
                        && resFile != null
                        && resID.HasValue )
                    {
                        //friendlyTypeName = resFile.FullName;
                        using var hLibRes = resFile.GetOrLoadModuleHandle();
                        if ( hLibRes.isValid )
                        {
                            var loadedString = hLibRes.LoadString(Math.Abs(resID.Value));
                            if ( loadedString.isNotNullOrWhiteSpace ) displayName = loadedString!;
                        }
                    }
                }
                catch { }
                return displayName;
            }


            /// <summary>
            /// Convert local file path to full remote path: 'C:\Windows\explorer.exe' => '\\x.x.x.x\c$\Windows\explorer.exe'
            /// If 'local' path is like \\x.x.x.x\apps\var\any.exe, the path does not changes
            /// </summary>
            public static FileInfo eRemoteHostLocalPathToNetworkPath ( this string localPath , string remoteHost )
            {
                try
                {
                    FileInfo fiApp = new(localPath);   //'C:\Windows\explorer.exe' or '\\x.x.x.x\apps\var\app.exe'
                    DirectoryInfo diRemote = fiApp.Directory!;  //'C:\Windows' or '\\x.x.x.x\apps'
                    string root = diRemote.Root.FullName;      //'C:\' or '\\x.x.x.x\apps'
                    if ( !(root.Length == 3 && root.EndsWith(@":\")) )
                    {
                        return new(localPath);
                    }

                    char cDisk = root.First();
                    string dirPath = fiApp.Directory!.FullName.Substring(3);       //'Windows'
                    dirPath = remoteHost.formatAsWinSharePrefix() + @$"{cDisk}$\{dirPath}"; //'c$\Windows'
                    string fullRemoteFilePath = Path.Combine(dirPath , fiApp.Name);          //'c$\Windows\explorer.exe'
                    return new(fullRemoteFilePath);
                }
                catch
                {
                    return new(localPath);
                }
            }

#if NET6_0_OR_GREATER
            /// <summary>
            /// Convert full remote path to remote host local file path: '\\x.x.x.x\c$\Windows\explorer.exe' => 'C:\Windows\explorer.exe'
            /// If remote path is like \\x.x.x.x\apps\var\any.exe, the path does not changes!
            /// If remote path host name or IP does not equal to 'remoteHost' arg, the path does not changes!
            /// </summary>
            public static FileInfo eNetworkPathToRemoteHostLocalPath ( this string remotePath , string remoteHost )
            {
                string netPrefix = remoteHost.formatAsWinSharePrefix(); //'\\x.x.x.x/'
                if ( !remotePath.ToLower().StartsWith(netPrefix.ToLower()) )
                {
                    return new FileInfo(remotePath);// another remote host. do not modify path '\\y.y.y.y\aaaa\bbbb....'
                }

                //Path starts with '\\host\', sample: '\\x.x.x.x\c$\Windows\System32\Defrag.exe'
                string path = remotePath.Substring(netPrefix.Length);            //'c$\Windows\System32\Defrag.exe' or 'c$\Windows\System32\Eap3Host.exe'

                const string MIN_REMOTE_PATH_TO_PARSE = @"c$\a.b";
                if ( path.Length >= MIN_REMOTE_PATH_TO_PARSE.Length )
                {
                    string diskPrefix = path.Substring(0 , 3); //'c$\'
                    char diskChar = diskPrefix.First();                             //'c'
                    if ( char.IsLetter(diskChar) && diskPrefix.EndsWith(@"$\") )// path like 'c$\', not like 'share1\dir2...'
                    {
                        path = string.Concat($@"{diskChar}:\" , path.AsSpan(diskPrefix.Length));//'c:\Windows\System32\Defrag.exe'
                        return new FileInfo(path);
                    }
                }
                return new FileInfo(remotePath);//Dont change path
            }
#endif




            public static bool DeleteFileKernel32 ( this string name )
            {
                if ( name.isNullOrWhiteSpace ) throw new ArgumentNullException(nameof(name));
                if ( !Kernel32.DeleteFile(name) )
                {
                    var WEX = new errors.Win32ExceptionEx("DeleteFile");
                    WEX.eThrowIfNot(errors.Win32Errors.ERROR_FILE_NOT_FOUND);
                }
                return true;
            }


            /// <inheritdoc cref="Kernel32.CreateFile" />
            public static Kernel32.SafeHFILE CreateFileHandleNotDisk ( this
                string path ,
                Kernel32.FileAccess access ,
                FileShare share ,
                FileMode mode ,
                Vanara.PInvoke.FileFlagsAndAttributes flags ,
                SECURITY_ATTRIBUTES? security = null
                )
            {
                var hFile = Kernel32.CreateFile(path , access , share , security , mode , flags);
                if ( !hFile.IsInvalid && Kernel32.GetFileType(hFile) != Kernel32.FileType.FILE_TYPE_DISK )
                {
                    using ( hFile )
                        throw new NotSupportedException();
                }
                return hFile;
            }


        }


        internal static partial class Extensions_WinAPI_IO_IsolatedStorage
        {


            //List Isolated storages
            //VS CMD: StoreAdm /LIST

            /// <summary>Method to retrieve all directories, recursively, within a store.</summary>
            /// <param name="pattern"></param>
            internal static List<string> getAllDirectories ( this IsolatedStorageFile storeFile , string pattern = @"*" , bool recursive = false )
            {
                // Get the root of the search string.
                string root = Path.GetDirectoryName(pattern).valueOrEmpty;

                if ( root != "" ) root += "/";

                // Retrieve directories.
                List<string> directoryList = [ .. storeFile.GetDirectoryNames(pattern) ];

                if ( recursive )
                {
                    // Retrieve subdirectories of matches.
                    for ( int i = 0, max = directoryList.Count ; i < max ; i++ )
                    {
                        string directory = directoryList[ i ] + @"/";
                        List<string> more = storeFile.getAllDirectories(root + directory + @"*");

                        // For each subdirectory found, add in the base path.
                        for ( int j = 0 ; j < more.Count ; j++ )
                        {
                            more[ j ] = directory + more[ j ];
                        }

                        // Insert the subdirectories into the list and
                        // update the counter and upper bound.
                        directoryList.InsertRange(i + 1 , more);
                        i += more.Count;
                        max += more.Count;
                    }
                }
                return directoryList;
            }


            internal static List<string> getAllFiles ( this IsolatedStorageFile storeFile , string pattern = @"*" , bool recursive = false )
            {
                // Get the root and file portions of the search string.
                string fileString = Path.GetFileName(pattern);
                List<string> fileList = [ .. storeFile.GetFileNames(pattern) ];

                if ( recursive )
                {
                    // Loop through the subdirectories, collect matches,
                    // and make separators consistent.
                    foreach ( string directory in storeFile.getAllDirectories(@"*") )
                    {
                        foreach ( string file in storeFile.GetFileNames(directory + @"/" + fileString) )
                        {
                            fileList.Add(directory + @"/" + file);
                        }
                    }
                }

                return fileList;
            }


        }


        internal static partial class Extensions_WinAPI_Network
        {


            #region eSendARP


            /// <inheritdoc cref="IpHlpApi.SendARP" />
            public static PhysicalAddress SendARP ( this IPAddress ip , IPAddress? sendViaInterface = null )
            {
                if ( ip.Equals(IPAddress.None) || ip.Equals(IPAddress.Broadcast) || ip.Equals(IPAddress.Loopback) )
                    throw new ArgumentNullException(nameof(ip));

                sendViaInterface ??= IPAddress.Any;

                uint macLen = 6;
                byte[] abMAC = new byte[ macLen ];
                IpHlpApi.SendARP(ip.eToUInt32() , sendViaInterface!.eToUInt32() , abMAC , ref macLen)
                    .ThrowIfFailed();

                return new PhysicalAddress(abMAC);
            }

            /// <inheritdoc cref="IpHlpApi.SendARP" />
            public static async Task<Result<PhysicalAddress>> SendARPAsync ( this IPAddress ip , IPAddress? sendViaInterface = null , CancellationTokenSource? cancel = null )
            {
                var tskSendARP = Extensions_DebugAndErrors.tryCatchAsync(() => ip.SendARP(sendViaInterface) , cancel: cancel?.Token);
                var r = (cancel != null)
                    ? await tskSendARP.WaitAsync(cancel!.Token)
                    : await tskSendARP.WaitAsync(TimeSpan.FromSeconds(5));

                return r;
            }


            // Пытаемся определить NetBIOS имя хоста
            public static async Task<(
                Result<string> HostName,
                Result<PhysicalAddress> MAC
                )> QueryAsync ( this IPAddress IP , CancellationTokenSource? cancel = null )
            {
                var tskResolveDNSNAme = Extensions_DebugAndErrors.tryCatchAsync(() => Dns.GetHostEntry(IP) , cancel: cancel?.Token);
                var tskSendARP = Extensions_DebugAndErrors.tryCatchAsync(() => IP.SendARP() , cancel: cancel?.Token);
                try
                {
                    if ( cancel != null )
                    {
                        await Task
                            .WhenAll(tskResolveDNSNAme , tskSendARP)
                            .WaitAsync(cancel.Token);

                        if ( cancel.IsCancellationRequested )
                            //return (null, null);
                            return (
                                Result<string>.Canceled(),
                                Result<PhysicalAddress>.Canceled()
                            );
                    }
                    else
                    {
                        await Task.WhenAll(tskResolveDNSNAme , tskSendARP);
                    }


                    Result<string> hnr = tskResolveDNSNAme.IsCompletedSuccessfully && tskResolveDNSNAme.Result.IsSuccess
                        ? Result<string>.Success(tskResolveDNSNAme.Result.Value?.HostName ?? string.Empty)
                        : Result<string>.Failure(tskResolveDNSNAme.Result.Error);

                    return (hnr, tskSendARP.Result);

                }
                catch ( TaskCanceledException tce )
                {
                    return (
                        Result<string>.Failure(tce),
                        Result<PhysicalAddress>.Failure(tce)
                        );
                }

            }

            #endregion



            #region InetIsOffline

            internal static bool InetIsOffline () => Vanara.PInvoke.Url.InetIsOffline(0);


            #endregion


        }



        internal static partial class Extensions_WinAPI_Kernel
        {


            /// <inheritdoc cref="Kernel32.ExpandEnvironmentStrings" />

            public static string ExpandEnvironmentStrings ( this string src )
            {
                string expanded = src;

                //Detecting buffer size
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                var bufferSize = (int)Kernel32.ExpandEnvironmentStrings(src , null , 0);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                if ( bufferSize > 0 )
                {
                    //Expanding
                    StringBuilder sb = new(bufferSize);
                    var chars = Kernel32.ExpandEnvironmentStrings(expanded , sb , (uint)sb.Capacity);
                    if ( chars > 0 ) expanded = sb.ToString();
                }
                return expanded;
            }




            public static Version ToVersion ( this Kernel32.OSVERSIONINFOEX ovx )
                => new((int)ovx.dwMajorVersion , (int)ovx.dwMinorVersion , (int)ovx.dwBuildNumber);


            public static Version ToVersion ( this NtDll.OSVERSIONINFOW ovx )
                => new((int)ovx.dwMajorVersion , (int)ovx.dwMinorVersion , (int)ovx.dwBuildNumber);



            public static Version GetSPVersion ( this Kernel32.OSVERSIONINFOEX ovx )
                => new(ovx.wServicePackMajor , ovx.wServicePackMinor);



            public static OS.KnownVersionNames ToKnownName ( this Kernel32.OSVERSIONINFOEX ovx )
            {
                try
                {
                    if ( ovx.dwMajorVersion != 0 )
                    {
                        int ifullver = (int)((ovx.dwMajorVersion << 8) | ovx.dwMinorVersion);
                        var efullver = (OS.KnownVersionNames)ifullver;

                        switch ( efullver )
                        {
                            case OS.KnownVersionNames.Win10:
                                {
                                    var Win10_ = OS.KnownVersionNames.Win10.ToString() + "_";
                                    var Win11_ = OS.KnownVersionNames.Win11_21H2_SunValley.ToString().Split('_').First() + "_";


                                    var aW10Builds = (from OS.KnownVersionNames E in Enum.GetValues(typeof(OS.KnownVersionNames))
                                                      where E.ToString().StartsWith(Win10_) || E.ToString().StartsWith(Win11_)
                                                      orderby (int)E descending
                                                      select E).ToArray();


                                    var iBuild = ovx.dwBuildNumber;
                                    var eVerFound = aW10Builds.FirstOrDefault(e => iBuild >= (int)e);
                                    if ( eVerFound != default )
                                    {
                                        return eVerFound;
                                    }

                                    break;
                                }

                            default:
                                return efullver;
                        }
                    }
                }
                catch { }//ignore any errors
                return OS.KnownVersionNames.Unknown;
            }




            public static Vanara.PInvoke.Kernel32.SafeHINSTANCE GetOrLoadModuleHandle ( this string lib )
            {
                var hLibRes = Kernel32.GetModuleHandle(lib);
                if ( !hLibRes.isValid ) hLibRes = Kernel32.LoadLibrary(lib);//, Kernel32.LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE );
                return hLibRes;
            }




        }


        internal static partial class Extensions_WinAPI_Shell
        {






            #region StrFormatByteSize


            /// <inheritdoc cref="ShlwApi.StrFormatByteSize64A" />

            internal static string eFormatByteSize_Win32 ( this ulong fileLenght )
            {
                StringBuilder sb = new(120);
                _ = ShlwApi.StrFormatByteSize64A((Int64)fileLenght , sb , (uint)sb.Capacity);

                string s = sb.ToString();
                s.isNullOrWhiteSpace.ThrowLastWin32ErrorIfTrue(true , "ShlwApi.StrFormatByteSize64A");
                return s;
            }

            /// <inheritdoc cref="eFormatByteSize_Win32" />

            public static string eFormatByteSize_Win32 ( this Int64 FileLenght )
                => ((ulong)FileLenght).eFormatByteSize_Win32();

            /// <inheritdoc cref="eFormatByteSize_Win32" />

            public static string eFormatByteSize_Win32 ( this Int32 iFileLenght )
                => ((ulong)iFileLenght).eFormatByteSize_Win32();

            /// <inheritdoc cref="eFormatByteSize_Win32" />

            public static string eFormatByteSize_Win32 ( this uint iFileLenght )
                => ((ulong)iFileLenght).eFormatByteSize_Win32();

            /// <inheritdoc cref="eFormatByteSize_Win32" />

            public static string eFormatByteSize_Win32 ( this FileInfo FI )
                => FI.Length.eFormatByteSize_Win32();



            private const ShlwApi.SFBS_FLAGS FormatByteSizeExFlagsDefault = ShlwApi.SFBS_FLAGS.SFBS_FLAGS_TRUNCATE_UNDISPLAYED_DECIMAL_DIGITS;

            /// <inheritdoc cref="ShlwApi.StrFormatByteSizeEx" />

            internal static string eFormatByteSize_Win32Ex ( this ulong lenght , ShlwApi.SFBS_FLAGS flags = FormatByteSizeExFlagsDefault )
            {
                StringBuilder sb = new(120);
                _ = ShlwApi.StrFormatByteSizeEx(lenght , flags , sb , (uint)sb.Capacity);
                string s = sb.ToString();
                s.isNullOrWhiteSpace.ThrowLastWin32ErrorIfTrue(true , nameof(ShlwApi.StrFormatByteSizeEx));
                return s;
            }

            /// <inheritdoc cref="eFormatByteSize_Win32Ex" />

            public static string eFormatByteSize_Win32Ex ( this Int64 FileLenght , ShlwApi.SFBS_FLAGS flags = FormatByteSizeExFlagsDefault )
                => ((ulong)FileLenght).eFormatByteSize_Win32Ex(flags);

            /// <inheritdoc cref="eFormatByteSize_Win32Ex" />

            public static string eFormatByteSize_Win32Ex ( this Int32 iFileLenght , ShlwApi.SFBS_FLAGS flags = FormatByteSizeExFlagsDefault )
                => ((ulong)iFileLenght).eFormatByteSize_Win32Ex(flags);

            /// <inheritdoc cref="eFormatByteSize_Win32Ex" />

            public static string eFormatByteSize_Win32Ex ( this uint iFileLenght , ShlwApi.SFBS_FLAGS flags = FormatByteSizeExFlagsDefault )
                => ((ulong)iFileLenght).eFormatByteSize_Win32Ex(flags);

            /// <inheritdoc cref="eFormatByteSize_Win32Ex" />

            public static string eFormatByteSize_Win32Ex ( this FileInfo FI , ShlwApi.SFBS_FLAGS flags = FormatByteSizeExFlagsDefault )
                => FI.Length.eFormatByteSize_Win32Ex(flags);


            #endregion



            // <DllImport(UOM.Win32.WINDLL_SHLWAPI, _
            // SetLastError:=True, _
            // CharSet:=CharSet.Auto, _
            // ExactSpelling:=False, _
            // CallingConvention:=CallingConvention.Winapi)> _
            // Private Overloads Shared Function StrFormatKBSize( _
            // ByVal SizeInBytes As Long, _
            // <MarshalAs(UnmanagedType.LPTStr)> ByVal Buffer As System.Text.StringBuilder, _
            // ByVal BufferSize As Integer) As IntPtr
            // 
            // End Function 


            #region StrFromTimeInterval


            /// <inheritdoc cref="ShlwApi.StrFromTimeInterval " />
            /// <param name="Digits">
            /// <code>
            /// dwTimeMS	digits	pszOut 
            /// 34000	3	34 sec 
            /// 34000	2	34 sec 
            /// 34000	1	30 sec 
            /// 74000	3	1 min 14 sec 
            /// 74000	2	1 min 10 sec 
            /// 74000	1	1 min 
            /// </code>
            /// </param>
            /// <returns></returns>

            internal static string StrFromTimeInterval ( this uint dwTimeMS , int Digits = 3 )
            {
                StringBuilder? sb = null;
#pragma warning disable CS8604 // Possible null reference argument.
                var buffSize = ShlwApi.StrFromTimeInterval(sb , 0 , dwTimeMS , Digits);
#pragma warning restore CS8604 // Possible null reference argument.
                if ( buffSize > 0 )
                {
                    sb = new(buffSize + 1);
                    buffSize = ShlwApi.StrFromTimeInterval(sb , (uint)sb.Capacity , dwTimeMS , Digits);
                    if ( buffSize > 0 ) return sb.ToString().Trim();
                }
                return "";
            }

            /// <inheritdoc cref="StrFromTimeInterval" />

            public static string StrFromTimeInterval ( this TimeSpan TS , int Digits = 3 )
            {
                uint ms = (uint)Math.Round(TS.TotalSeconds * 1000d);
                return ms.StrFromTimeInterval(Digits);
            }


            #endregion


            #region SHMessageBoxCheck

            //// Public Enum MessageBoxCheckFlags As UInteger
            //// MB_OK = 0x0
            //// MB_OKCANCEL = 0x1
            //// MB_YESNO = 0x4
            //// MB_ICONHAND = 0x10
            //// MB_ICONQUESTION = 0x20
            //// MB_ICONEXCLAMATION = 0x30
            //// MB_ICONINFORMATION = 0x40
            //// End Enum

            //[DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
            //// The SHMessageBoxCheck Is exported by ordinal in shlwapi.dll
            //// SHMessageBoxCheckA: 185 
            //// SHMessageBoxCheckW: 191

            //// The Windows Shell (Explorer) stores your preference in the following registry key:
            //// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\DontShowMeThisDialogAgain
            //private static extern MsgBoxResult SHMessageBoxCheck(
            //    [In] IntPtr hwnd,
            //    [In,MarshalAs(UnmanagedType.LPTStr)] string pszText,
            //    [In,MarshalAs(UnmanagedType.LPTStr)] string pszTitle,
            //    [In] MsgBoxStyle uType,
            //    [In] MsgBoxResult iDefault,
            //    [In,MarshalAs(UnmanagedType.LPTStr)] string pszRegVal);

            //private static string BuildMessageID(string MessageID)
            //{
            //    if (string.IsNullOrWhiteSpace(MessageID)) throw new ArgumentNullException(nameof(MessageID));
            //    return Application.ProductName + "_" + MessageID; // This Is the value Of the registry key
            //}

            /////// <summary>Displays a message box that gives the user the option of suppressing further occurrences. If the user has already opted to suppress the message box, the function does not display a dialog box and instead simply returns the default value.</summary>
            /////// <param name="MessageID">ID под которым в реестре храниться результат установки галочки "не показывать более"</param>
            /////// <param name="iDefault">Это значение возвращается в конце функции, если в предыдущем вызове, была установлена галочка.</param>
            /////// <returns>В текущем вызове ВСЕГДА вернётся код нажатой кнопки (независимо от состояния галочки).
            /////// Если галочку установили сейчас, то в этом вызове вернётся код нажатой кнопки, а во всех последющих будет возвращаться iDefault и диалог не будет показываться.</returns>
            ////internal static MsgBoxResult SHMessageBoxCheck(
            //// IWin32Window WND,
            //// string pszText,
            //// string MessageID,
            //// string pszTitle = null,
            //// MsgBoxStyle.vbYesNo | MsgBoxStyle.Information, MsgBoxResult iDefault = MsgBoxResult.Abort)
            ////{
            //// IntPtr hwnd = WND.Handle;
            //// if (string.IsNullOrWhiteSpace(pszTitle)) pszTitle = Application.ProductName;
            //// string sID = BuildMessageID(MessageID); // This Is the value Of the registry key
            //// return SHMessageBoxCheck(hwnd, pszText, pszTitle, uType, iDefault, sID);
            ////}

            ///// <summary>SHMessageBoxCheckCleanup will reset the state of the message box (i.e. the users choice wether or not to see the message box again will be reset).</summary>
            ///// <param name="MessageID">ID под которым в реестре храниться результат установки галочки "не показывать более"</param>
            //internal static void SHMessageBoxCheckCleanup(string MessageID)
            //{
            //    // # delete the registry key that Is used To store the checkbox status so we can start fresh Next time
            //    // DeleteRegValue HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\DontShowMeThisDialogAgain" "${_PSZ_REG_VAL}"
            //    const string C_KEY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\DontShowMeThisDialogAgain";
            //    string sID = BuildMessageID(MessageID); // This Is the value Of the registry key
            //    try
            //    {
            //        using (var hKey = Registry.CurrentUser.OpenSubKey(C_KEY_PATH, true))
            //        {
            //            if (hKey?.GetValue(sID) != null)
            //            {
            //                hKey?.DeleteValue(sID);
            //                hKey?.Flush();
            //            }
            //        }
            //    }
            //    catch (Exception ex) { }                    // Ignore Any Errors
            //}

            #endregion


            #region SHFileOperation



            public static bool SHFO_Delete (
                this IntPtr hWndOwner ,
                IList<string> itemsToDelete ,
                string? title = null ,
                bool allowUndo = true ,
                Shell32.FILEOP_FLAGS? flagsModifers = null
                )
            {

                Shell32.FILEOP_FLAGS flg = Shell32.FILEOP_FLAGS.FOFX_NOMINIMIZEBOX;
                if ( allowUndo ) flg |= Shell32.FILEOP_FLAGS.FOF_ALLOWUNDO;
                if ( flagsModifers.HasValue ) flg |= flagsModifers.Value;

                string pathList = itemsToDelete.toAPIMultiStringZ();

                //Use RunWithGCPinned to avoid copy array of strings at memory
                return pathList.RunWithGCPinned(filesToDeletePtr =>
                {
                    Shell32.SHFILEOPSTRUCT fo = new()
                    {
                        hwnd = hWndOwner ,
                        lpszProgressTitle = title ,
                        wFunc = Shell32.ShellFileOperation.FO_DELETE ,
                        pFrom = filesToDeletePtr ,
                        fFlags = flg
                    };
                    return (Shell32.SHFileOperation(ref fo) == 0) && !fo.fAnyOperationsAborted;
                });
            }



            public static bool SHFO_Move (
                this IntPtr hWndOwner ,
                IEnumerable<string> sources ,
                IEnumerable<string> targets ,
                string? title = null ,
                bool allowUndo = true ,
                Shell32.FILEOP_FLAGS? flagsModifers = null
                )
            {

                Shell32.FILEOP_FLAGS flg = Shell32.FILEOP_FLAGS.FOFX_NOMINIMIZEBOX;
                if ( allowUndo ) flg |= Shell32.FILEOP_FLAGS.FOF_ALLOWUNDO;
                if ( flagsModifers.HasValue ) flg |= flagsModifers.Value;

                var a = new List<string> { sources.toAPIMultiStringZ() , targets.toAPIMultiStringZ() };
                //Use RunWithGCPinned to avoid copy array of strings at memory
                return a.RunWithGCPinned(ptrList =>
                {
                    Shell32.SHFILEOPSTRUCT fo = new()
                    {
                        hwnd = hWndOwner ,
                        lpszProgressTitle = title ,
                        wFunc = Shell32.ShellFileOperation.FO_MOVE ,
                        fFlags = flg ,
                        pFrom = ptrList[ 0 ] ,
                        pTo = ptrList[ 1 ] ,
                    };
                    return (Shell32.SHFileOperation(ref fo) == 0) && !fo.fAnyOperationsAborted;
                });
            }


            /// <summary>Копирование в заданную ОДНУ папку, без перенименования!</summary>

            public static bool SHFO_Copy (
                this IntPtr hWndOwner ,
                IEnumerable<string> sources ,
                string targetFolder ,
                string? title = null ,
                bool allowUndo = true ,
                Shell32.FILEOP_FLAGS? flagsModifers = null
                )
            {

                Shell32.FILEOP_FLAGS flg = Shell32.FILEOP_FLAGS.FOFX_NOMINIMIZEBOX;
                if ( allowUndo ) flg |= Shell32.FILEOP_FLAGS.FOF_ALLOWUNDO;
                if ( flagsModifers.HasValue ) flg |= flagsModifers.Value;

                var a = new List<string> { sources.toAPIMultiStringZ() , targetFolder + "\0\0" };
                //Use RunWithGCPinned to avoid copy array of strings at memory
                return a.RunWithGCPinned(ptrList =>
                {
                    Shell32.SHFILEOPSTRUCT fo = new()
                    {
                        hwnd = hWndOwner ,
                        lpszProgressTitle = title ,
                        wFunc = Shell32.ShellFileOperation.FO_COPY ,
                        fFlags = flg ,
                        pFrom = ptrList[ 0 ] ,
                        pTo = ptrList[ 1 ] ,
                    };
                    return (Shell32.SHFileOperation(ref fo) == 0) && !fo.fAnyOperationsAborted;
                });
            }


            /// <summary>Копирование массива исходных файлов с переименованием</summary>

            public static bool SHFO_Copy (
                this IntPtr hWndOwner ,
                IEnumerable<string> sources ,
                IEnumerable<string> targets ,
                string? title = null ,
                bool allowUndo = true ,
                Shell32.FILEOP_FLAGS? flagsModifers = null
                )
            {

                Shell32.FILEOP_FLAGS flg = Shell32.FILEOP_FLAGS.FOF_MULTIDESTFILES | Shell32.FILEOP_FLAGS.FOFX_NOMINIMIZEBOX;
                if ( allowUndo ) flg |= Shell32.FILEOP_FLAGS.FOF_ALLOWUNDO;
                if ( flagsModifers.HasValue ) flg |= flagsModifers.Value;

                var a = new List<string> { sources.toAPIMultiStringZ() , targets.toAPIMultiStringZ() };
                //Use RunWithGCPinned to avoid copy array of strings at memory
                return a.RunWithGCPinned(ptrList =>
                {
                    Shell32.SHFILEOPSTRUCT fo = new()
                    {
                        hwnd = IntPtr.Zero ,// HwndOwner,
                        lpszProgressTitle = title ,
                        wFunc = Shell32.ShellFileOperation.FO_COPY ,
                        fFlags = flg ,
                        pFrom = ptrList[ 0 ] ,
                        pTo = ptrList[ 1 ] ,
                    };
                    return (Shell32.SHFileOperation(ref fo) == 0) && !fo.fAnyOperationsAborted;
                });
            }

            #endregion




            // Function GetSysImageList(ByVal Flags As E_SHGFI_Flags, Optional ByVal Refresh As bool  = False) As Integer
            // UPGRADE_WARNING: Couldn't resolve default property of object lHSysImL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            // UPGRADE_WARNING: Arrays in structure FI may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1063"'
            // Dim FI As Win.SHFILEINFO
            // If Refresh Or lHSysImL = 0 Then
            // UPGRADE_ISSUE: COM expression not supported: Module methods of COM objects. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1060"'
            // UPGRADE_WARNING: Couldn't resolve default property of object lHSysImL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            // lHSysImL = Win.SHGetFileInfo("", 0, FI, Len(FI), E_SHGFI_Flags.SHGFI_SYSICONINDEX Or Flags)
            // End If
            // UPGRADE_WARNING: Couldn't resolve default property of object lHSysImL. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            // GetSysImageList = lHSysImL
            // End Function





            #region ShlwApi Path APIs...


            /// <inheritdoc cref="ShlwApi.PathCompactPath" />

            internal static string PathCompactPath ( this string path , IntPtr HDC , uint PixelWidth )
            {
                var sb = new StringBuilder(path , path.Length + Kernel32.MAX_PATH);
                ShlwApi.PathCompactPath(HDC , sb , PixelWidth);
                return sb.ToString();
            }


            /// <inheritdoc cref="ShlwApi.PathCompactPathEx" />

            internal static string PathCompactPathEx ( this string path , uint NeedCahrCount )
            {
                var SB = new StringBuilder((int)NeedCahrCount * 2);
                ShlwApi.PathCompactPathEx(SB , path , NeedCahrCount , 0).ThrowLastWin32ErrorIfFailed();
                return SB.ToString();
            }


            /// <inheritdoc cref="ShlwApi.PathRemoveFileSpec" />

            [Obsolete("This function is deprecated. We recommend the use of the PathCchRemoveFileSpec function in its place.")]
            internal static string PathRemoveFile ( this string path )
            {
                var sb = path.eToStringBuilder();
                ShlwApi.PathRemoveFileSpec(sb).ThrowLastWin32ErrorIfFailed();
                return sb.ToString();
            }


            /// <inheritdoc cref="ShlwApi.PathRelativePathTo" />

            internal static string GetRelativePathTo ( this string pathFrom , FileFlagsAndAttributes dwAttrFrom , string pathTo , FileFlagsAndAttributes dwAttrTo )
            {
                StringBuilder sb = new(Kernel32.MAX_PATH + 100);
                ShlwApi.PathRelativePathTo(sb , pathFrom , dwAttrFrom , pathTo , dwAttrTo)
                    .ThrowLastWin32ErrorIfFailed();

                return sb.ToString();
            }


            #endregion


            // <DllImport(UOM.Win32.WINDLL_SHELL, _
            // SetLastError:=True, _
            // CharSet:=CharSet.Auto, _
            // ExactSpelling:=False, _
            // CallingConvention:=CallingConvention.Winapi)> _
            // Friend Overloads shared Function SHGetDesktopFolder(ByRef ppshf As UOM.Shell.Interfaces.IShellFolder) As Integer
            // End Function
            // Friend Overloads shared Function SHGetDesktopFolder() As UOM.Shell.Interfaces.IShellFolder
            // Dim SH As UOM.Shell.Interfaces.IShellFolder
            // Call SHGetDesktopFolder(SH)
            // Return SH
            // End Function




            internal static (DialogResult DislogResult, string? IconFile, int IconIndex) PickIconDlg (
                this IWin32Window hWnd ,
                string? iconFile = null ,
                int iIconIndex = 0 )
            {
                const int C_DEFAULT_BUFFER_SIZE = 2000;
                StringBuilder sbPath = (null != iconFile)
                    ? new StringBuilder(iconFile , C_DEFAULT_BUFFER_SIZE)
                    : new StringBuilder(C_DEFAULT_BUFFER_SIZE);

                if ( !Shell32.PickIconDlg(hWnd.Handle , sbPath , (uint)sbPath.Capacity , ref iIconIndex) )
                    return (DialogResult.Cancel, null, -1);

                iconFile = Environment.ExpandEnvironmentVariables(sbPath.ToString());
                return (DialogResult.OK, iconFile, iIconIndex);
            }


        }


        internal static partial class Extensions_WinAPI_Windows
        {


            /// <summary>
            /// Used when system uses High DPI modes since Windows 8.1 <see href="https://learn.microsoft.com/en-us/windows/win32/hidpi/high-dpi-desktop-application-development-on-windows"/>
            /// <br/>
            /// For this to work correctly on Windows 10 anniversary, you need to add dpiAwareness to app.manifest of your project:
            /// </summary>
            /// <returns>
            /// 1.25 = 125%
            /// <br/>
            /// 1.5 = 150%
            /// </returns>

            public static float GetHighDPIScaleFactor ( this IntPtr hwnd )
            {
                try
                {
                    return User32.GetDpiForWindow(hwnd) / (float)uom.OS.DEFAULT_SCREEN_DPI;
                }
                catch { return 1f; }// Or fallback to GDI solutions above
            }

            /// <inheritdoc>GetHighDPIScaleFactor </inheritdoc> />

            public static float GetHighDPIScaleFactor ( this IWin32Window wnd )
                => wnd.Handle.GetHighDPIScaleFactor();



            #region AppCommand


            /// <summary>
            /// This commands mostly used by remote control apps for send commands from remote control to the system. Like Media players with remote or like the MediaCenter etc.
            /// https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-appcommand
            /// https://docs.microsoft.com/en-us/previous-versions/windows/desktop/windows-media-center-sdk/bb417079(v=msdn.10)
            /// </summary>
            public enum FAPPCOMMAND_CMD : int
            {
                APPCOMMAND_BROWSER_BACKWARD = 1,//Navigate backward.
                APPCOMMAND_BROWSER_FORWARD = 2,//Navigate forward.
                APPCOMMAND_BROWSER_REFRESH = 3,//Refresh page.
                APPCOMMAND_BROWSER_STOP = 4,//Stop download.
                APPCOMMAND_BROWSER_SEARCH = 5,//Open search.
                APPCOMMAND_BROWSER_FAVORITES = 6,//Open favorites.
                APPCOMMAND_BROWSER_HOME = 7,//Navigate home.

                /// <summary>Mute the volume.</summary>
                APPCOMMAND_VOLUME_MUTE = 8, //APPCOMMAND_VOLUME_MUTE = 0x8_00_00,
                /// <summary>Lower the volume.</summary>
                APPCOMMAND_VOLUME_DOWN = 9, //APPCOMMAND_VOLUME_DOWN = 0x9_00_00,
                /// <summary>Raise the volume.</summary>
                APPCOMMAND_VOLUME_UP = 10,  //APPCOMMAND_VOLUME_UP = 0xA_00_00,                


                APPCOMMAND_MEDIA_NEXTTRACK = 11,//Go to next track.
                APPCOMMAND_MEDIA_PREVIOUSTRACK = 12,//Go to previous track.
                APPCOMMAND_MEDIA_STOP = 13,//Stop playback.
                APPCOMMAND_MEDIA_PLAY_PAUSE = 14,//Play or pause playback.If there are discrete Play and Pause buttons, applications should take action on this command as well as APPCOMMAND_MEDIA_PLAY and APPCOMMAND_MEDIA_PAUSE.

                APPCOMMAND_LAUNCH_MAIL = 15,//Open mail.
                APPCOMMAND_LAUNCH_MEDIA_SELECT = 16,//Go to Media Select mode.
                APPCOMMAND_LAUNCH_APP1 = 17,//Start App1.
                APPCOMMAND_LAUNCH_APP2 = 18,//Start App2.


                APPCOMMAND_BASS_DOWN = 19,  //Decrease the bass.
                APPCOMMAND_BASS_BOOST = 20, //Toggle the bass boost on and off.
                APPCOMMAND_BASS_UP = 21,    //Increase the bass.
                APPCOMMAND_TREBLE_DOWN = 22,//Decrease the treble.
                APPCOMMAND_TREBLE_UP = 23,//Increase the treble.

                APPCOMMAND_MICROPHONE_VOLUME_MUTE = 24,//Mute the microphone.
                APPCOMMAND_MICROPHONE_VOLUME_DOWN = 25,//Decrease microphone volume.
                APPCOMMAND_MICROPHONE_VOLUME_UP = 26,//Increase microphone volume.

                APPCOMMAND_HELP = 27,//Open the Help dialog.
                APPCOMMAND_FIND = 28,//Open the Find dialog.
                APPCOMMAND_NEW = 29,//Create a new window.
                APPCOMMAND_OPEN = 30,//Open a window.
                APPCOMMAND_CLOSE = 31,//Close the window (not the application).

                APPCOMMAND_SAVE = 32,//Save current document.
                APPCOMMAND_PRINT = 33,//Print current document.

                APPCOMMAND_UNDO = 34,//Undo last action.
                APPCOMMAND_REDO = 35,//Redo last action.
                APPCOMMAND_COPY = 36,//Copy the selection.
                APPCOMMAND_CUT = 37,//Cut the selection.
                APPCOMMAND_PASTE = 38,//Paste

                APPCOMMAND_REPLY_TO_MAIL = 39,//Reply to a mail message.
                APPCOMMAND_FORWARD_MAIL = 40,//Forward a mail message.
                APPCOMMAND_SEND_MAIL = 41,//Send a mail message.
                APPCOMMAND_SPELL_CHECK = 42,//Initiate a spell check.
                APPCOMMAND_DICTATE_OR_COMMAND_CONTROL_TOGGLE = 43,//Toggles between two modes of speech input: dictation and command/control(giving commands to an application or accessing menus).
                APPCOMMAND_MIC_ON_OFF_TOGGLE = 44,//Toggle the microphone.
                APPCOMMAND_CORRECTION_LIST = 45,//Brings up the correction list when a word is incorrectly identified during speech input.

                APPCOMMAND_MEDIA_PLAY = 46,//Begin playing at the current position.If already paused, it will resume.This is a direct PLAY command that has no state.If there are discrete Play and Pause buttons, applications should take action on this command as well as APPCOMMAND_MEDIA_PLAY_PAUSE.
                APPCOMMAND_MEDIA_PAUSE = 47,//Pause.If already paused, take no further action.This is a direct PAUSE command that has no state.If there are discrete Play and Pause buttons, applications should take action on this command as well as APPCOMMAND_MEDIA_PLAY_PAUSE.
                APPCOMMAND_MEDIA_RECORD = 48,//Begin recording the current stream.
                APPCOMMAND_MEDIA_FAST_FORWARD = 49,//Increase the speed of stream playback. This can be implemented in many ways, for example, using a fixed speed or toggling through a series of increasing speeds.
                APPCOMMAND_MEDIA_REWIND = 50,//Go backward in a stream at a higher rate of speed.This can be implemented in many ways, for example, using a fixed speed or toggling through a series of increasing speeds.
                APPCOMMAND_MEDIA_CHANNEL_UP = 51,//Increment the channel value, for example, for a TV or radio tuner.
                APPCOMMAND_MEDIA_CHANNEL_DOWN = 52,//Decrement the channel value, for example, for a TV or radio tuner.

            }

            //The uDevice component indicates the input device that generated the input event, and can be one of the following values.
            public enum FAPPCOMMAND_SRC : int
            {
                /// <summary>User pressed a key.</summary>
                FAPPCOMMAND_KEY = 0,
                /// <summary>User clicked a mouse button.</summary>
                FAPPCOMMAND_MOUSE = 0x8000,
                /// <summary>An unidentified hardware source generated the event. It could be a mouse or a keyboard event.</summary>
                FAPPCOMMAND_OEM = 0x1000,
            }

            //The dwKeys component indicates whether various virtual keys are down, and can be one or more of the following values.
            public enum FAPPCOMMAND_VKEY : int
            {
                MK_NONE = 0,

                /// <summary>The CTRL key is down.</summary>
                MK_CONTROL = 0x0008,
                /// <summary>The left mouse button is down.</summary>
                MK_LBUTTON = 0x0001,
                /// <summary>The middle mouse button is down.</summary>
                MK_MBUTTON = 0x0010,
                /// <summary>The right mouse button is down.</summary>
                MK_RBUTTON = 0x0002,
                /// <summary>The SHIFT key is down.</summary>
                MK_SHIFT = 0x0004,
                /// <summary>The first X button is down.</summary>
                MK_XBUTTON1 = 0x0020,
                /// <summary>The second X button is down.</summary>
                MK_XBUTTON2 = 0x0040,
            }


            /// <summary>Send to the system an "app command", it is like pressing volume up / down / mute button or other like this.</summary>
            public static void SendAppCommand ( this
                IntPtr hwnd ,
                FAPPCOMMAND_CMD cmd ,
                FAPPCOMMAND_SRC source = FAPPCOMMAND_SRC.FAPPCOMMAND_KEY ,
                FAPPCOMMAND_VKEY vkey = FAPPCOMMAND_VKEY.MK_NONE )
            {
                int fullCode = ((int)cmd << 16) | (int)source | (int)vkey;
                User32.SendMessage(hwnd , User32.WindowMessage.WM_APPCOMMAND , hwnd , new IntPtr(fullCode));
            }


            #endregion




            /// <inheritdoc cref="User32.GetWindowText" />

            public static string GetWindowText ( this IntPtr hwnd )
            {
                int lenght = User32.GetWindowTextLength(hwnd);
                if ( lenght < 1 ) return string.Empty;

                StringBuilder sb = new(lenght + 2);
                lenght = User32.GetWindowText(hwnd , sb , sb.Capacity);
                return (lenght > 0)
                    ? sb.ToString()
                    : string.Empty;
            }

            /// <inheritdoc cref="GetWindowText" />

            public static string GetWindowText ( this IWin32Window wnd )
                => GetWindowText(wnd.Handle);


            /// <inheritdoc cref="User32.GetClassName" />

            public static string GetClassName ( this IntPtr hwnd )
            {
                StringBuilder sb = new(1024);
                int len = User32.GetClassName(hwnd , sb , sb.Capacity);
                return (len > 0)
                    ? sb.ToString()
                    : string.Empty;
            }

            /// <inheritdoc cref="GetClassName" />

            public static string GetClassName ( this IWin32Window wnd )
                => GetClassName(wnd.Handle);


            /// <inheritdoc cref="User32.GetWindowRect" />

            internal static Rectangle GetWindowRect ( this IntPtr hwnd )
                => User32.GetWindowRect(hwnd , out var rc)
                ? rc
                : throw new Win32Exception();

            /// <inheritdoc cref="GetWindowRect" />

            internal static Rectangle GetWindowRect ( this IWin32Window wind )
                => GetWindowRect(wind.Handle);

            /// <inheritdoc cref="GetWindowRect" />

            internal static Rectangle GetWindowRect ( this HWND hwnd )
                => GetWindowRect((IntPtr)hwnd);


            /// <inheritdoc cref="User32.GetClientRect" />

            internal static Rectangle GetClientRect ( this IntPtr hwnd )
                            => User32.GetClientRect(hwnd , out var rcClient)
                                ? rcClient
                                : throw new Win32Exception();

            /// <inheritdoc cref="GetClientRect" />

            internal static Rectangle GetClientRect ( this IWin32Window wind ) => GetClientRect(wind.Handle);


        }


        internal static partial class Extensions_WinAPI_Windows_DWM
        {


            #region DwmGetWindowAttribute / DwmSetWindowAttribute


            /// <summary>Gets window rect without shadow</summary>

            internal static Rectangle DwmGetWindowAttribute_DWMWA_EXTENDED_FRAME_BOUNDS ( this IntPtr hwnd )
            {
                var r = DwmApi.DwmGetWindowAttribute(hwnd , DwmApi.DWMWINDOWATTRIBUTE.DWMWA_EXTENDED_FRAME_BOUNDS , out RECT rect);
                if ( !r.Succeeded ) Marshal.ThrowExceptionForHR(r.Code);
                return rect;
            }




            internal static void DwmSetWindowAttribute ( IntPtr hwnd , DwmApi.DWMWINDOWATTRIBUTE dwAttribute , bool value )
            {
                var r = DwmApi.DwmSetWindowAttribute(hwnd , dwAttribute , value);
                if ( !r.Succeeded ) Marshal.ThrowExceptionForHR(r.Code);
            }


            internal static void DwmSetWindowAttribute_DWMWA_USE_IMMERSIVE_DARK_MODE ( IntPtr hwnd , bool value )
                => DwmSetWindowAttribute(hwnd , DwmApi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE , value);


            #endregion



            /// <summary>Gets window rect without shadow</summary>

            internal static Rectangle GetWindowRectWithoutShadow ( this IntPtr hwnd )
                => DwmGetWindowAttribute_DWMWA_EXTENDED_FRAME_BOUNDS(hwnd);


            internal static Rectangle GetWindowRectWithoutShadow ( this IWin32Window wnd )
                => wnd.Handle.GetWindowRectWithoutShadow();



            public static void SetWindowRectWithoutShadow ( this IntPtr handle , Rectangle window )
            {
                Rectangle excludeshadow = GetWindowRectWithoutShadow(handle);
                Rectangle includeshadow = handle.GetWindowRect();

                RECT shadow = new()
                {
                    Left = includeshadow.X - excludeshadow.X ,
                    Right = includeshadow.Right - excludeshadow.Right ,
                    Top = includeshadow.Top - excludeshadow.Top ,
                    Bottom = includeshadow.Bottom - excludeshadow.Bottom
                };

                int width = window.Right + shadow.Right - (window.Left + shadow.Left);
                int height = window.Bottom + shadow.Bottom - (window.Top - shadow.Top);

                User32.SetWindowPos(handle , IntPtr.Zero ,
                      window.Left + shadow.Left ,
                      window.Top + shadow.Top ,
                      width ,
                      height ,
                      0);
            }


            public static void SetWindowRectWithoutShadow ( this IWin32Window wnd , Rectangle rc )
                => wnd.Handle.SetWindowRectWithoutShadow(rc);



            //https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-messagebox
            public static User32.MB_RESULT MessageBox ( this string text , IntPtr hWndParent , User32.MB_FLAGS f = User32.MB_FLAGS.MB_OK )
                => User32.MessageBox(
                    hWndParent ,
                    text ,
                    uom.AppInfo.Title ?? uom.AppInfo.ProductName ?? string.Empty ,
                    f);


            //https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-messagebox
            public static User32.MB_RESULT MessageBox ( this IWin32Window wnd , string text , User32.MB_FLAGS f = User32.MB_FLAGS.MB_OK )
                => text.MessageBox(wnd.Handle , f);

        }


        internal static partial class Extensions_WinAPI_Console
        {


            /// <summary>
            /// https://stackoverflow.com/questions/1277563/how-do-i-get-the-handle-of-a-console-applications-window
            /// <br/>
            /// use this only for other process console!
            /// <br/>
            /// for own console use 'uom.AppTools.GetConsoleWindowForCurrentProcess ()'!
            /// </summary>
            [Obsolete("Didn't work with Windows Terminal!")]

            public static HWND GetConsoleWindowForProcess ( this int processID )
            {
                bool is_attached = false;

                Kernel32.HandlerRoutine consoleCtrlDelegateDetach = new(ct =>
                {
                    if ( is_attached = !Kernel32.FreeConsole() )
                    {
                        var wex = new Win32Exception();
                        Trace.TraceError($"FreeConsole on {ct}: {wex}");
                    }
                    return true;
                });

                Kernel32.SetConsoleCtrlHandler(consoleCtrlDelegateDetach , true);

                is_attached = Kernel32.AttachConsole((uint)processID);
                if ( !is_attached ) throw new Win32Exception();

                var hwnd = uom.AppTools.GetConsoleWindowForCurrentProcess_Net();
                if ( is_attached ) Kernel32.FreeConsole();

                return hwnd;
            }





        }


        internal static class Extensions_WinAPI_Resources
        {



            public static Vanara.PInvoke.ResourceId[] EnumResourceNames ( this Kernel32.SafeHINSTANCE hLib , ResourceType resType = ResourceType.RT_STRING )
            {
                List<Vanara.PInvoke.ResourceId> names = [];
                var result = Kernel32.EnumResourceNames(
                    hLib ,
                    new SafeResourceId(resType) ,
                    ( hInst , resType , resID , ptr ) =>
                {
                    names.Add(resID);
                    return true;
                });
                return [ .. names ];
            }


            [DllImport(Lib.User32 , CharSet = CharSet.Auto , SetLastError = true)]
            [System.Security.SecurityCritical]
            private static extern int LoadString ( HINSTANCE hInstance , int uID , ref IntPtr lpBuffer , int nBufferMax );

            public static string? LoadString ( this Kernel32.SafeHINSTANCE hLib , int id )
            {
                IntPtr p = default;
                var length = LoadString(hLib , id , ref p , 0);
                if ( length == 0 ) return null;// Win32Error.ThrowLastError();
                return StringHelper.GetString(p , CharSet.Auto , length * Marshal.SystemDefaultCharSize)!;
            }




















            /// <summary>Разбирает строку с указанием ресурса, которая обычно используется в реестре, или ещё где...</summary>
            ///<param name="win32ResPath">@%systemroot%\system32\mstsc.exe или %systemroot%\system32\mstsc.exe,-4004 или shell32.dll, 2</param>
            public static bool TryParseWin32ResourcePath ( this string win32ResPath , out string? file , out int? resID )
            {
                file = null;
                resID = null;
                if ( win32ResPath.isNullOrWhiteSpace ) return false;

                if ( win32ResPath.StartsWith('@') ) win32ResPath = win32ResPath.Substring(1);

                file = win32ResPath.Trim();
                if ( file.Contains(',') ) // Есть запятая-разделитель - делим
                {
                    (file, string resIndex, int idx) = file.eSplitByChar(',' , true);
                    file = file.Trim();
                    if ( int.TryParse(resIndex.Trim() , out var parsedResIndex) )
                        resID = parsedResIndex;
                }

                if ( file.Contains('%') ) // Path contains '%' looks like environment string...
                    file = Environment.ExpandEnvironmentVariables(file).Trim();
                //file = new FileInfo( path );
                return true;
            }


            //public static string MAKEINTRESOURCE(this int uID)
            //{
            //    return UOM.Win32.Resources.mResourcesAPI.MAKEINTRESOURCE(uID);
            //}


            //public static string MAKEINTRESOURCE(this ushort uID)
            //{
            //    return UOM.Win32.Resources.mResourcesAPI.MAKEINTRESOURCE(uID);
            //}



            //public static string MAKEINTRESOURCE(this uint uID)
            //{
            //    return UOM.Win32.Resources.mResourcesAPI.MAKEINTRESOURCE((int)uID);
            //}



            /// <summary>Expands a Microsoft indirect string, such as '@vmms.exe,-279', which can be mapped
            /// to a normal human-readable string.  Also supports indirect strings similar to this:
            /// '@{Microsoft.Cortana?ms-resource://Microsoft.Cortana/resources/ProductDescription}'.
            /// Microsoft indirect strings begin with an "@" symbol and are common in the registry. See https://msdn.microsoft.com/en-us/library/windows/desktop/bb759919(v=vs.85).aspx
            /// </summary>
            /// <param name="indirectString">
            /// Win32: '@{Filepath?resource}'
            /// AppX: '@{PackageFullName?resource-id}'
            /// SAMPLE:
            /// @{C:\Program Files\WindowsApps\Microsoft.BingMaps_2.1.3230.2048_x64__8wekyb3d8bbwe\resources.pri?ms-resource://Microsoft.BingMaps/Resources/AppShortDisplayName};
            /// </param>			

            public static string SHLoadIndirectString ( this string indirectString , int bufferSize = 1024 )
            {
                //@{s}
                if ( indirectString.Length < 5 || indirectString[ 0 ] != '@' )
                    throw new ArgumentException($"'{nameof(indirectString)}' should begin with the '@' symbol!");


                StringBuilder sb = new(bufferSize);
                var result = ShlwApi.SHLoadIndirectString(indirectString , sb , (uint)sb.Capacity);
                result.ThrowIfFailed();

                string s = sb.ToString();
                if ( s != indirectString ) return s;//Resource loaded OK

                throw new ArgumentException($"'{nameof(indirectString)}' is not an indirect string!");
            }




            /// <param name="ResID">ms-resource:AppxManifest_DisplayName</param>
            /// <returns>@{C:\Program Files\WindowsApps\Microsoft.BingMaps_2.1.3230.2048_x64__8wekyb3d8bbwe\resources.pri?ms-resource://Microsoft.BingMaps/Resources/AppShortDisplayName};</returns>

            public static string SHLoadIndirectppPackageResourceString ( this string ResID , string PackageName , string PackageFullName , string InstallLocation )
            {
                if ( !ResID.StartsWith(core.MS_RESX_PREFIX , StringComparison.InvariantCultureIgnoreCase) ) throw new ArgumentOutOfRangeException(nameof(ResID));

                //@{PackageFullName?resource-id}
                //ms-resource://PackageName/Resources/Id
                /*
                 * https://stackoverflow.com/questions/18219915/get-localized-friendly-names-for-all-winrt-metro-apps-installed-from-wpf-applica
                 Я обнаружил, что путь к PRI является лучшим решением, чем указание имени пакета. Много раз SHLoadIndirectString не разрешает ресурс, когда указано только имя пакета. Путь к PRI — это место установки пакета + resources.pri . Пример: C:\Program Files\WindowsApps\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\Resources.pri.
                 */

                string resID2 = ResID[ core.MS_RESX_PREFIX.Length.. ];

                {
                    string pack = PackageFullName;

                    try
                    {
                        string input = @$"{pack}? {core.MS_RESX_PREFIX}//{PackageName}/Resources/{resID2}";
                        input = @"@{" + input + @"}";

                        string parsedString = input.SHLoadIndirectString();
                        if ( !string.IsNullOrWhiteSpace(parsedString) )
                        {
                            return parsedString;
                        }
                    }
                    catch ( Exception ex ) { var ttt = ex; }

                    try
                    {
                        string input = @$"{pack}? {core.MS_RESX_PREFIX}//{PackageName}/{resID2}";
                        input = @"@{" + input + @"}";

                        string parsedString = input.SHLoadIndirectString();
                        if ( !string.IsNullOrWhiteSpace(parsedString) )
                        {
                            return parsedString;
                        }
                    }
                    catch ( Exception ex ) { var ttt = ex; }
                }

                {
                    string pack = InstallLocation;
                    try
                    {
                        string input = @$"{pack}? {core.MS_RESX_PREFIX}//{PackageName}/Resources/{resID2}";
                        input = @"@{" + input + @"}";

                        string parsedString = input.SHLoadIndirectString();
                        if ( !string.IsNullOrWhiteSpace(parsedString) )
                        {
                            return parsedString;
                        }
                    }
                    catch ( Exception ex ) { var ttt = ex; }

                    try
                    {
                        string input = @$"{pack}? {core.MS_RESX_PREFIX}//{PackageName}/{resID2}";
                        input = @"@{" + input + @"}";

                        string parsedString = input.SHLoadIndirectString();
                        if ( !string.IsNullOrWhiteSpace(parsedString) )
                        {
                            return parsedString;
                        }
                    }
                    catch ( Exception ex ) { var ttt = ex; }
                }

                {
                    string pack = InstallLocation;
                    try
                    {
                        string input = @$"{pack}\resources.pri? {core.MS_RESX_PREFIX}//{PackageName}/Resources/{resID2}";
                        input = @"@{" + input + @"}";

                        string parsedString = input.SHLoadIndirectString();
                        if ( !parsedString.isNullOrWhiteSpace ) return parsedString;
                    }
                    catch ( Exception ex ) { var ttt = ex; }

                    try
                    {
                        string input = @$"{pack}\resources.pri? {core.MS_RESX_PREFIX}//{PackageName}/{resID2}";
                        input = @"@{" + input + @"}";

                        string parsedString = input.SHLoadIndirectString();
                        if ( !parsedString.isNullOrWhiteSpace ) return parsedString;
                    }
                    catch ( Exception ex ) { var ttt = ex; }

                    try
                    {
                        string input = @$"{pack}\resources.pri? {core.MS_RESX_PREFIX}//{resID2}";
                        input = @"@{" + input + @"}";

                        string parsedString = input.SHLoadIndirectString();
                        if ( !parsedString.isNullOrWhiteSpace ) return parsedString;
                    }
                    catch ( Exception ex ) { var ttt = ex; }
                }

                return string.Empty;
            }



        }


        internal static partial class Extensions_InternalClasses
        {


            extension( Win32ResourcePathParser source )
            {

                public (System.Drawing.Icon? Large, System.Drawing.Icon? Small) ExtractIconsEx ()
                   => uom.WinAPI.gdi.icon.ExtractIconEx(source.File , source.ResourceIndex);


                public Icon? ExtractIconEx ( bool small = true )
                {
                    var r = source.ExtractIconsEx();
                    if ( small )
                        r.Large?.Dispose();
                    else
                        r.Small?.Dispose();

                    return small
                        ? r.Small
                        : r.Large;
                }


                ///<summary>Загружает строковый ресурс из DLL по пути типа '@%SystemRoot%\System32\shell32.dll,-30596'</summary>
                public string? ExtractResourceString ()
                    => uom.WinAPI.win32Resources.LoadString(source.File , source.ResourceIndex);


            }

        }

        internal static partial class Extensions_UI_Localization
        {

            internal const string LOCALIZED_RES_NAME_TEMPLATE_L_UI = "L_UI_{0}";
            internal const string LOCALIZED_RES_NAME_TEMPLATE_L_ENUM = "L_ENUM_{0}";



            internal static string? eGetLocalizedText (
                this string? resName ,
                System.Resources.ResourceManager? rm = null ,
                string resNameTemplate = LOCALIZED_RES_NAME_TEMPLATE_L_UI )
            {
                if ( resName.isNullOrWhiteSpace ) return null;

                resName = resNameTemplate.format(resName!);
                rm ??= uom.AppTools.LocalizedStringsManager.Value;
                string? resStringValue = rm.GetString(resName);
                return resStringValue.isNotNullOrWhiteSpace ? resStringValue : null;
            }



            private static string? eGetLocalizedTextByPropertyName ( this Component c , string propertyName = "Name" )
            {
                string? componentName = c.eGetPropertyValueAs<string>(propertyName , null , BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                return componentName.isNotNullOrWhiteSpace ? componentName.eGetLocalizedText() : null;
            }



            internal static string eLocalize ( this Enum value , System.Resources.ResourceManager? rm = null )
            {
                string enumResSuffix = value.eGetFullName(false);
                string? localizedString = enumResSuffix.eGetLocalizedText(rm , LOCALIZED_RES_NAME_TEMPLATE_L_ENUM);
                return localizedString.isNotNullOrWhiteSpace ? localizedString! : value.ToString().eInsertSpacesBeforeUpperCaseChars();
            }


        }


    }


    namespace WinAPI
    {




        /// <summary>Win32 Safe Handles for diferent API</summary>
        internal static partial class safeHandles
        {



            public static SafeHINSTANCE LoadLibraryEx ( string LibFileName , Kernel32.LoadLibraryExFlags Flags )
                => Kernel32.LoadLibraryEx(LibFileName , IntPtr.Zero , Flags);


            public static SafeHINSTANCE LoadLibraryEx_AsDataFile ( string LibFileName )
                => LoadLibraryEx(LibFileName , Kernel32.LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE);


            internal static SafeHINSTANCE GetRunningLibraryOrLoad ( string LibFileName , Kernel32.LoadLibraryExFlags Flags )
            {
                // check first to see if the module is already mapped into this process.
                var hModule = Kernel32.GetModuleHandle(LibFileName);
                if ( hModule.isValid ) return hModule;

                // need to load module into this process.
                return LoadLibraryEx(LibFileName , Flags);
            }


            internal static SafeHINSTANCE GetRunningLibraryOrLoad_AsResource ( string LibFileName )
                => GetRunningLibraryOrLoad(LibFileName , Kernel32.LoadLibraryExFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE);



            /*

            //[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            [DebuggerDisplay("Win32LibHandle={handle}")]
            internal partial class Win32LibHandle : Kernel32.SafeHINSTANCE
            {


                /// <summary>Только для информации. НИГДЕ НЕ ПРИМЕНЯЕТСЯ!</summary>
                public readonly string? LibFile;


                #region Constructor/destructor


                public Win32LibHandle ( IntPtr hLib , bool ownsHandle = true , string? sLibFileName = null ) : base(hLib , ownsHandle)
                {
                    LibFile = sLibFileName;
                }

                public Win32LibHandle ( Kernel32.SafeHINSTANCE hLib , bool ownsHandle = true , string? sLibFileName = null ) : base(hLib.DangerousGetHandle() , ownsHandle)
                {
                    LibFile = sLibFileName;
                }


                #endregion
                 

                #region Operator
                // Shared Operator =(ByVal Mem1 As LocalMemory, ByVal LMB2 As LocalMemory) As Boolean
                // If (Mem1.IsValid AndAlso LMB2.IsValid) Then Return (Mem1.DangerousGetHandle = LMB2.DangerousGetHandle)
                // Return False
                // End Operator
                // Shared Operator <>(ByVal LMB1 As LocalMemory, ByVal LMB2 As LocalMemory) As Boolean
                // Return (Not (LMB1 = LMB2))
                // End Operator

                public static implicit operator IntPtr ( Win32LibHandle WLH )
                {
                    return WLH.DangerousGetHandle();
                }


                // Public Shared Widening Operator CType(ByVal LMB1 As LocalMemory) As string
                // Return LMB1.ToString
                // End Operator
                // Public Shared Widening Operator CType(ByVal LMB1 As LocalMemory) As Integer()
                // Return LMB1.ToIntegers
                // End Operator
                // Public Shared Widening Operator CType(ByVal LMB1 As LocalMemory) As System.Drawing.Point()
                // Return LMB1.ToPoints
                // End Operator





                #endregion

            }
             */


        }


        /// <summary>Win32 Errors API</summary>
        internal static partial class errors
        {

            //  Values are 32 bit values layed [Out ] as follows:

            //   3 3 2 2 2 2 2 2 2 2 2 2 1 1 1 1 1 1 1 1 1 1
            //   1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0 9 8 7 6 5 4 3 2 1 0
            //  +---+-+-+-----------------------+-------------------------------+
            //  |Sev|C|R|     Facility          |               Code            |
            //  +---+-+-+-----------------------+-------------------------------+

            //  where

            //      Sev - is the severity code

            //          00 - Success
            //          01 - Informational
            //          10 - Warning
            //          11 - Error

            //      C - is the Customer code flag

            //      R - is a reserved bit

            //      Facility - is the facility code

            //      Code - is the facility's status code


            /// <summary>Define the facility codes</summary>
            internal enum FACILITY_CODES : ushort
            {
                FACILITY_NULL = 0,
                FACILITY_RPC = 1,
                FACILITY_DISPATCH = 2,
                FACILITY_STORAGE = 3,
                FACILITY_ITF = 4,
                FACILITY_WIN32 = 7,
                FACILITY_WINDOWS = 8,
                FACILITY_SECURITY = 9,
                FACILITY_SSPI = 9,
                FACILITY_CONTROL = 10,
                FACILITY_CERT = 11,
                FACILITY_INTERNET = 12,
                FACILITY_MEDIASERVER = 13,
                FACILITY_MSMQ = 14,
                FACILITY_SETUPAPI = 15,
                FACILITY_SCARD = 16,
                FACILITY_COMPLUS = 17,
                FACILITY_AAF = 18,
                FACILITY_URT = 19,
                FACILITY_ACS = 20,
                FACILITY_DPLAY = 21,
                FACILITY_UMI = 22,
                FACILITY_SXS = 23,
                FACILITY_WINDOWS_CE = 24,
                FACILITY_HTTP = 25,
                FACILITY_BACKGROUNDCOPY = 32,
                FACILITY_CONFIGURATION = 33,
                FACILITY_STATE_MANAGEMENT = 34,
                FACILITY_METADIRECTORY = 35
            }


            /*
		   // HRESULT Wrapper
		   internal enum HResult : Int64
		   {
			   //
			   // Summary:
			   //     S_OK
			   Ok = 0,
			   //
			   // Summary:
			   //     S_FALSE
			   False = 1,
			   //
			   // Summary:
			   //     E_INVALIDARG
			   InvalidArguments = -2147024809,
			   //
			   // Summary:
			   //     E_OUTOFMEMORY
			   OutOfMemory = -2147024882,
			   //
			   // Summary:
			   //     E_NOINTERFACE
			   NoInterface = -2147467262,
			   //
			   // Summary:
			   //     E_FAIL
			   Fail = -2147467259,
			   //
			   // Summary:
			   //     E_ELEMENTNOTFOUND
			   ElementNotFound = -2147023728,
			   //
			   // Summary:
			   //     TYPE_E_ELEMENTNOTFOUND
			   TypeElementNotFound = -2147319765,
			   //
			   // Summary:
			   //     NO_OBJECT
			   NoObject = -2147221019,
			   //
			   // Summary:
			   //     Win32 Error code: ERROR_CANCELLED
			   Win32ErrorCanceled = 1223,
			   //
			   // Summary:
			   //     ERROR_CANCELLED
			   Canceled = -2147023673,
			   //
			   // Summary:
			   //     The requested resource is in use
			   ResourceInUse = -2147024726,
			   //
			   // Summary:
			   //     The requested resources is read-only.
			   AccessDenied = -2147287035
		   }
			 */


            internal enum Win32Errors : int
            {
                /// <summary>The operation completed successfully.</summary>
                ERROR_SUCCESS = 0,

                NO_ERROR = ERROR_SUCCESS,

                /// <summary> Incorrect function.</summary>
                ERROR_INVALID_FUNCTION = 1,
                /// <summary>The system cannot find the file specified.</summary>
                ERROR_FILE_NOT_FOUND = 2,
                /// <summary>The system cannot find the path specified.</summary>
                ERROR_PATH_NOT_FOUND = 3,
                /// <summary>The system cannot open the file.</summary>
                ERROR_TOO_MANY_OPEN_FILES = 4,
                /// <summary>Access is denied.</summary>
                ERROR_ACCESS_DENIED = 5,
                /// <summary>The handle is invalid.</summary>
                ERROR_INVALID_HANDLE = 6,
                /// <summary>The storage control blocks were destroyed.</summary>
                ERROR_ARENA_TRASHED = 7,
                /// <summary>Not enough storage is available to process this command.</summary>
                ERROR_NOT_ENOUGH_MEMORY = 8,
                /// <summary>The storage control block address is invalid.</summary>
                ERROR_INVALID_BLOCK = 9,
                /// <summary>The environment is incorrect.</summary>
                ERROR_BAD_ENVIRONMENT = 10,
                /// <summary>An attempt was made to load a program with an incorrect format.</summary>
                ERROR_BAD_FORMAT = 11,
                /// <summary>The access code is invalid.</summary>
                ERROR_INVALID_ACCESS = 12,
                /// <summary>The data is invalid.</summary>
                ERROR_INVALID_DATA = 13,
                /// <summary>Not enough storage is available to complete this operation.</summary>
                ERROR_OUTOFMEMORY = 14,
                /// <summary>The system cannot find the drive specified.</summary>
                ERROR_INVALID_DRIVE = 15,
                /// <summary>The directory cannot be removed.</summary>
                ERROR_CURRENT_DIRECTORY = 16,
                /// <summary>The system cannot move the file to a different disk drive.</summary>
                ERROR_NOT_SAME_DEVICE = 17,
                /// <summary>There are no more files.</summary>
                ERROR_NO_MORE_FILES = 18,
                /// <summary>The media is write protected.</summary>
                ERROR_WRITE_PROTECT = 19,
                /// <summary>The system cannot find the device specified.</summary>
                ERROR_BAD_UNIT = 20,
                /// <summary>The device is not ready.</summary>
                ERROR_NOT_READY = 21,
                /// <summary>The device does not recognize the command.</summary>
                ERROR_BAD_COMMAND = 22,
                /// <summary>Data error (cyclic redundancy check).</summary>
                ERROR_CRC = 23,
                /// <summary>The program issued a command but the command length is incorrect.</summary>
                ERROR_BAD_LENGTH = 24,
                /// <summary>The drive cannot locate a specific area or track on the disk.</summary>
                ERROR_SEEK = 25,
                /// <summary>The specified disk or diskette cannot be accessed.</summary>
                ERROR_NOT_DOS_DISK = 26,
                /// <summary>The drive cannot find the sector requested.</summary>
                ERROR_SECTOR_NOT_FOUND = 27,
                /// <summary>The printer is Out of paper.</summary>
                ERROR_OUT_OF_PAPER = 28,
                /// <summary>The system cannot write to the specified device.</summary>
                ERROR_WRITE_FAULT = 29,
                /// <summary>The system cannot read from the specified device.</summary>
                ERROR_READ_FAULT = 30,
                /// <summary>A device attached to the system is not functioning.</summary>
                ERROR_GEN_FAILURE = 31,
                /// <summary>The process cannot access the file because it is being used by another process.</summary>
                ERROR_SHARING_VIOLATION = 32,
                /// <summary>The process cannot access the file because another process has locked a portion of the file.</summary>
                ERROR_LOCK_VIOLATION = 33,
                /// <summary>The wrong diskette is in the drive. Insert %2 (Volume Serial Number: %3) into drive %1.</summary>
                ERROR_WRONG_DISK = 34,
                /// <summary>Too many files opened for sharing.</summary>
                ERROR_SHARING_BUFFER_EXCEEDED = 36,
                /// <summary>Reached the end of the file.</summary>
                ERROR_HANDLE_EOF = 38,
                /// <summary>The disk is full.</summary>
                ERROR_HANDLE_DISK_FULL = 39,
                /// <summary>The request is not supported.</summary>
                ERROR_NOT_SUPPORTED = 50,
                /// <summary>Windows cannot find the network path. Verify that the network path is correct and the destination computer is not busy or turned off. If Windows still cannot find the network path, contact your network administrator.</summary>
                ERROR_REM_NOT_LIST = 51,
                /// <summary>You were not connected because a duplicate name exists on the network. Go to System in Control Panel to change the computer name and try again.</summary>
                ERROR_DUP_NAME = 52,
                /// <summary>The network path was not found.</summary>
                ERROR_BAD_NETPATH = 53,
                /// <summary>The network is busy.</summary>
                ERROR_NETWORK_BUSY = 54,
                /// <summary>The specified network resource or device is no longer available.</summary>
                ERROR_DEV_NOT_EXIST = 55, // dderror
                /// <summary>The network BIOS command limit has been reached.</summary>
                ERROR_TOO_MANY_CMDS = 56,
                /// <summary>A network adapter hardware error occurred.</summary>
                ERROR_ADAP_HDW_ERR = 57,
                /// <summary>The specified server cannot perform the requested operation.</summary>
                ERROR_BAD_NET_RESP = 58,
                /// <summary>An unexpected network error occurred.</summary>
                ERROR_UNEXP_NET_ERR = 59,
                /// <summary>The remote adapter is not compatible.</summary>
                ERROR_BAD_REM_ADAP = 60,
                /// <summary>The printer queue is full.</summary>
                ERROR_PRINTQ_FULL = 61,
                /// <summary>Space to store the file waiting to be printed is not available on the server.</summary>
                ERROR_NO_SPOOL_SPACE = 62,
                /// <summary>Your file waiting to be printed was deleted.</summary>
                ERROR_PRINT_CANCELLED = 63,
                /// <summary>The specified network name is no longer available.</summary>
                ERROR_NETNAME_DELETED = 64,
                /// <summary>Network access is denied.</summary>
                ERROR_NETWORK_ACCESS_DENIED = 65,
                /// <summary>The network resource type is not correct.</summary>
                ERROR_BAD_DEV_TYPE = 66,
                /// <summary>The network name cannot be found.</summary>
                ERROR_BAD_NET_NAME = 67,
                /// <summary>The name limit for the local computer network adapter card was exceeded.</summary>
                ERROR_TOO_MANY_NAMES = 68,
                /// <summary>The network BIOS session limit was exceeded.</summary>
                ERROR_TOO_MANY_SESS = 69,
                /// <summary>The remote server has been paused or is in the process of being started.</summary>
                ERROR_SHARING_PAUSED = 70,
                /// <summary>No more connections can be made to this remote computer at this time because there are already as many connections as the computer can accept.</summary>
                ERROR_REQ_NOT_ACCEP = 71,
                /// <summary>The specified printer or disk device has been paused.</summary>
                ERROR_REDIR_PAUSED = 72,
                /// <summary>The file exists.</summary>
                ERROR_FILE_EXISTS = 80,
                /// <summary>The directory or file cannot be created.</summary>
                ERROR_CANNOT_MAKE = 82,
                /// <summary>Fail on INT 24.</summary>
                ERROR_FAIL_I24 = 83,
                /// <summary>Storage to process this request is not available.</summary>
                ERROR_OUT_OF_STRUCTURES = 84,
                /// <summary>The local device name is already in use.</summary>
                ERROR_ALREADY_ASSIGNED = 85,
                /// <summary>The specified network password is not correct.</summary>
                ERROR_INVALID_PASSWORD = 86,
                /// <summary>The parameter is incorrect.</summary>
                ERROR_INVALID_PARAMETER = 87,
                /// <summary>A write fault occurred on the network.</summary>
                ERROR_NET_WRITE_FAULT = 88,
                /// <summary>The system cannot start another process at this time.</summary>
                ERROR_NO_PROC_SLOTS = 89,
                /// <summary>Cannot create another system semaphore.</summary>
                ERROR_TOO_MANY_SEMAPHORES = 100,
                /// <summary>The exclusive semaphore is owned by another process.</summary>
                ERROR_EXCL_SEM_ALREADY_OWNED = 101,
                /// <summary>The semaphore is set and cannot be closed.</summary>
                ERROR_SEM_IS_SET = 102,
                /// <summary>The semaphore cannot be set again.</summary>
                ERROR_TOO_MANY_SEM_REQUESTS = 103,
                /// <summary>Cannot request exclusive semaphores at interrupt time.</summary>
                ERROR_INVALID_AT_INTERRUPT_TIME = 104,
                /// <summary>The previous ownership of this semaphore has ended.</summary>
                ERROR_SEM_OWNER_DIED = 105,
                /// <summary>Insert the diskette for drive %1.</summary>
                ERROR_SEM_USER_LIMIT = 106,
                /// <summary>The program stopped because an alternate diskette was not inserted.</summary>
                ERROR_DISK_CHANGE = 107,
                /// <summary>The disk is in use or locked by another process.</summary>
                ERROR_DRIVE_LOCKED = 108,
                /// <summary>The pipe has been ended.</summary>
                ERROR_BROKEN_PIPE = 109,
                /// <summary>The system cannot open the device or file specified.</summary>
                ERROR_OPEN_FAILED = 110,
                /// <summary>The file name is too long.</summary>
                ERROR_BUFFER_OVERFLOW = 111,
                /// <summary>There is not enough space on the disk.</summary>
                ERROR_DISK_FULL = 112,
                /// <summary>No more internal file identifiers available.</summary>
                ERROR_NO_MORE_SEARCH_HANDLES = 113,
                /// <summary>The target internal file identifier is incorrect.</summary>
                ERROR_INVALID_TARGET_HANDLE = 114,
                /// <summary>The IOCTL call made by the application program is not correct.</summary>
                ERROR_INVALID_CATEGORY = 117,
                /// <summary>The verify-on-write switch parameter value is not correct.</summary>
                ERROR_INVALID_VERIFY_SWITCH = 118,
                /// <summary>The system does not support the command requested.</summary>
                ERROR_BAD_DRIVER_LEVEL = 119,
                /// <summary>This function is not supported on this system.</summary>
                ERROR_CALL_NOT_IMPLEMENTED = 120,
                /// <summary>The semaphore timeout period has expired.</summary>
                ERROR_SEM_TIMEOUT = 121,
                /// <summary>The data area passed to a system call is too small.</summary>
                ERROR_INSUFFICIENT_BUFFER = 122,
                /// <summary>The filename, directory name, or volume label syntax is incorrect.</summary>
                ERROR_INVALID_NAME = 123, // dderror
                /// <summary>The system call level is not correct.</summary>
                ERROR_INVALID_LEVEL = 124,
                /// <summary>The disk has no volume label.</summary>
                ERROR_NO_VOLUME_LABEL = 125,
                /// <summary>The specified module could not be found.</summary>
                ERROR_MOD_NOT_FOUND = 126,
                /// <summary>The specified procedure could not be found.</summary>
                ERROR_PROC_NOT_FOUND = 127,
                /// <summary>There are no child processes to wait for.</summary>
                ERROR_WAIT_NO_CHILDREN = 128,
                /// <summary>The %1 application cannot be run in Win32 mode.</summary>
                ERROR_CHILD_NOT_COMPLETE = 129,
                /// <summary>Attempt to use a file handle to an open disk partition for an operation other than raw disk I/O.</summary>
                ERROR_DIRECT_ACCESS_HANDLE = 130,
                /// <summary>An attempt was made to move the file pointer before the beginning of the file.</summary>
                ERROR_NEGATIVE_SEEK = 131,
                /// <summary>The file pointer cannot be set on the specified device or file.</summary>
                ERROR_SEEK_ON_DEVICE = 132,
                /// <summary>A JOIN or SUBST command cannot be used for a drive that contains previously joined drives.</summary>
                ERROR_IS_JOIN_TARGET = 133,
                /// <summary>An attempt was made to use a JOIN or SUBST command on a drive that has already been joined.</summary>
                ERROR_IS_JOINED = 134,
                /// <summary>An attempt was made to use a JOIN or SUBST command on a drive that has already been substituted.</summary>
                ERROR_IS_SUBSTED = 135,
                /// <summary>The system tried to delete the JOIN of a drive that is not joined.</summary>
                ERROR_NOT_JOINED = 136,
                /// <summary>The system tried to delete the substitution of a drive that is not substituted.</summary>
                ERROR_NOT_SUBSTED = 137,
                /// <summary>The system tried to join a drive to a directory on a joined drive.</summary>
                ERROR_JOIN_TO_JOIN = 138,
                /// <summary>The system tried to substitute a drive to a directory on a substituted drive.</summary>
                ERROR_SUBST_TO_SUBST = 139,
                /// <summary>The system tried to join a drive to a directory on a substituted drive.</summary>
                ERROR_JOIN_TO_SUBST = 140,
                /// <summary>The system tried to SUBST a drive to a directory on a joined drive.</summary>
                ERROR_SUBST_TO_JOIN = 141,
                /// <summary>The system cannot perform a JOIN or SUBST at this time.</summary>
                ERROR_BUSY_DRIVE = 142,
                /// <summary>The system cannot join or substitute a drive to or for a directory on the same drive.</summary>
                ERROR_SAME_DRIVE = 143,
                /// <summary>The directory is not a subdirectory of the root directory.</summary>
                ERROR_DIR_NOT_ROOT = 144,
                /// <summary>The directory is not empty.</summary>
                ERROR_DIR_NOT_EMPTY = 145,
                /// <summary>The path specified is being used in a substitute.</summary>
                ERROR_IS_SUBST_PATH = 146,
                /// <summary>Not enough resources are available to process this command.</summary>
                ERROR_IS_JOIN_PATH = 147,
                /// <summary>The path specified cannot be used at this time.</summary>
                ERROR_PATH_BUSY = 148,
                /// <summary>An attempt was made to join or substitute a drive for which a directory on the drive is the target of a previous substitute.</summary>
                ERROR_IS_SUBST_TARGET = 149,
                /// <summary>System trace information was not specified in your CONFIG. SYS file, or tracing is disallowed.</summary>
                ERROR_SYSTEM_TRACE = 150,
                /// <summary>The number of specified semaphore events for DosMuxSemWait is not correct.</summary>
                ERROR_INVALID_EVENT_COUNT = 151,
                /// <summary>DosMuxSemWait did not erunute; too many semaphores are already set.</summary>
                ERROR_TOO_MANY_MUXWAITERS = 152,
                /// <summary>The DosMuxSemWait list is not correct.</summary>
                ERROR_INVALID_LIST_FORMAT = 153,
                /// <summary>The volume label you entered exceeds the label character limit of the target file system.</summary>
                ERROR_LABEL_TOO_LONG = 154,
                /// <summary>Cannot create another thread.</summary>
                ERROR_TOO_MANY_TCBS = 155,
                /// <summary>The recipient process has refused the signal.</summary>
                ERROR_SIGNAL_REFUSED = 156,
                /// <summary>The segment is already discarded and cannot be locked.</summary>
                ERROR_DISCARDED = 157,
                /// <summary>The segment is already unlocked.</summary>
                ERROR_NOT_LOCKED = 158,
                /// <summary>The address for the thread ID is not correct.</summary>
                ERROR_BAD_THREADID_ADDR = 159,
                /// <summary>One or more arguments are not correct.</summary>
                ERROR_BAD_ARGUMENTS = 160,
                /// <summary>The specified path is invalid.</summary>
                ERROR_BAD_PATHNAME = 161,
                /// <summary>A signal is already pending.</summary>
                ERROR_SIGNAL_PENDING = 162,
                /// <summary>No more threads can be created in the system.</summary>
                ERROR_MAX_THRDS_REACHED = 164,
                /// <summary>Unable to lock a region of a file.</summary>
                ERROR_LOCK_FAILED = 167,
                /// <summary>The requested resource is in use.</summary>
                ERROR_BUSY = 170, // dderror
                /// <summary>A lock request was not outstanding for the supplied cancel region.</summary>
                ERROR_CANCEL_VIOLATION = 173,
                /// <summary>The file system does not support atomic changes to the lock type.</summary>
                ERROR_ATOMIC_LOCKS_NOT_SUPPORTED = 174,
                /// <summary>The system detected a segment number that was not correct.</summary>
                ERROR_INVALID_SEGMENT_NUMBER = 180,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_INVALID_ORDINAL = 182,
                /// <summary>Cannot create a file when that file already exists.</summary>
                ERROR_ALREADY_EXISTS = 183,
                /// <summary>The flag passed is not correct.</summary>
                ERROR_INVALID_FLAG_NUMBER = 186,
                /// <summary>The specified system semaphore name was not found.</summary>
                ERROR_SEM_NOT_FOUND = 187,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_INVALID_STARTING_CODESEG = 188,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_INVALID_STACKSEG = 189,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_INVALID_MODULETYPE = 190,
                /// <summary>Cannot run %1 in Win32 mode.</summary>
                ERROR_INVALID_EXE_SIGNATURE = 191,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_EXE_MARKED_INVALID = 192,
                /// <summary>%1 is not a valid Win32 System.Windows.Forms.Application.</summary>
                ERROR_BAD_EXE_FORMAT = 193,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_ITERATED_DATA_EXCEEDS_64k = 194,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_INVALID_MINALLOCSIZE = 195,
                /// <summary>The operating system cannot run this application program.</summary>
                ERROR_DYNLINK_FROM_INVALID_RING = 196,
                /// <summary>The operating system is not presently configured to run this System.Windows.Forms.Application.</summary>
                ERROR_IOPL_NOT_ENABLED = 197,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_INVALID_SEGDPL = 198,
                /// <summary>The operating system cannot run this application program.</summary>
                ERROR_AUTODATASEG_EXCEEDS_64k = 199,
                /// <summary>The code segment cannot be greater than or equal to 64K.</summary>
                ERROR_RING2SEG_MUST_BE_MOVABLE = 200,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_RELOC_CHAIN_XEEDS_SEGLIM = 201,
                /// <summary>The operating system cannot run %1.</summary>
                ERROR_INFLOOP_IN_RELOC_CHAIN = 202,
                /// <summary>The system could not find the environment option that was entered.</summary>
                ERROR_ENVVAR_NOT_FOUND = 203,
                /// <summary>No process in the command subtree has a signal handler.</summary>
                ERROR_NO_SIGNAL_SENT = 205,
                /// <summary>The filename or extension is too long.</summary>
                ERROR_FILENAME_EXCED_RANGE = 206,
                /// <summary>The ring 2 stack is in use.</summary>
                ERROR_RING2_STACK_IN_USE = 207,
                /// <summary>The global filename characters, * or ?, are entered incorrectly or too many global filename characters are specified.</summary>
                ERROR_META_EXPANSION_TOO_LONG = 208,
                /// <summary>The signal being posted is not correct.</summary>
                ERROR_INVALID_SIGNAL_NUMBER = 209,
                /// <summary>The signal handler cannot be set.</summary>
                ERROR_THREAD_1_INACTIVE = 210,
                /// <summary>The segment is locked and cannot be reallocated.</summary>
                ERROR_LOCKED = 212,
                /// <summary>Too many dynamic-link modules are attached to this program or dynamic-link module.</summary>
                ERROR_TOO_MANY_MODULES = 214,
                /// <summary>Cannot nest calls to LoadModule.</summary>
                ERROR_NESTING_NOT_ALLOWED = 215,
                /// <summary>The image file %1 is valid, but is for a machine type other than the current machine.</summary>
                ERROR_EXE_MACHINE_TYPE_MISMATCH = 216,
                /// <summary>The image file %1 is signed, unable to modify.</summary>
                ERROR_EXE_CANNOT_MODIFY_SIGNED_BINARY = 217,
                /// <summary>The image file %1 is strong signed, unable to modify.</summary>
                ERROR_EXE_CANNOT_MODIFY_STRONG_SIGNED_BINARY = 218,
                /// <summary>The pipe state is invalid.</summary>
                ERROR_BAD_PIPE = 230,
                /// <summary>All pipe instances are busy.</summary>
                ERROR_PIPE_BUSY = 231,
                /// <summary>The pipe is being closed.</summary>
                ERROR_NO_DATA = 232,
                /// <summary>No process is on the other end of the pipe.</summary>
                ERROR_PIPE_NOT_CONNECTED = 233,
                /// <summary>More data is available.</summary>
                ERROR_MORE_DATA = 234, // dderror
                /// <summary>The session was canceled.</summary>
                ERROR_VC_DISCONNECTED = 240,
                /// <summary>The specified extended attribute name was invalid.</summary>
                ERROR_INVALID_EA_NAME = 254,
                /// <summary>The extended attributes are inconsistent.</summary>
                ERROR_EA_LIST_INCONSISTENT = 255,
                /// <summary>The wait operation timed out.</summary>
                WAIT_TIMEOUT = 258, // dderror
                /// <summary>No more data is available.</summary>
                ERROR_NO_MORE_ITEMS = 259,
                /// <summary>The copy functions cannot be used.</summary>
                ERROR_CANNOT_COPY = 266,
                /// <summary>The directory name is invalid.</summary>
                ERROR_DIRECTORY = 267,
                /// <summary>The extended attributes did not fit in the buffer.</summary>
                ERROR_EAS_DIDNT_FIT = 275,
                /// <summary>The extended attribute file on the mounted file system is corrupt.</summary>
                ERROR_EA_FILE_CORRUPT = 276,
                /// <summary>The extended attribute table file is full.</summary>
                ERROR_EA_TABLE_FULL = 277,
                /// <summary>The specified extended attribute handle is invalid.</summary>
                ERROR_INVALID_EA_HANDLE = 278,
                /// <summary>The mounted file system does not support extended attributes.</summary>
                ERROR_EAS_NOT_SUPPORTED = 282,
                /// <summary>Attempt to release mutex not owned by caller.</summary>
                ERROR_NOT_OWNER = 288,
                /// <summary>Too many posts were made to a semaphore.</summary>
                ERROR_TOO_MANY_POSTS = 298,
                /// <summary>Only part of a ReadProcessMemory or WriteProcessMemory request was completed.</summary>
                ERROR_PARTIAL_COPY = 299,
                /// <summary>The oplock request is denied.</summary>
                ERROR_OPLOCK_NOT_GRANTED = 300,
                /// <summary>An invalid oplock acknowledgment was received by the system.</summary>
                ERROR_INVALID_OPLOCK_PROTOCOL = 301,
                /// <summary>The volume is too fragmented to complete this operation.</summary>
                ERROR_DISK_TOO_FRAGMENTED = 302,
                /// <summary>The file cannot be opened because it is in the process of being deleted.</summary>
                ERROR_DELETE_PENDING = 303,
                /// <summary>The system cannot find message text for message number = %1 in the message file for %2.</summary>
                ERROR_MR_MID_NOT_FOUND = 317,
                /// <summary>The scope specified was not found.</summary>
                ERROR_SCOPE_NOT_FOUND = 318,
                /// <summary>Attempt to access invalid address.</summary>
                ERROR_INVALID_ADDRESS = 487,
                /// <summary>Arithmetic result exceeded 32 bits.</summary>
                ERROR_ARITHMETIC_OVERFLOW = 534,
                /// <summary>There is a process on other end of the pipe.</summary>
                ERROR_PIPE_CONNECTED = 535,
                /// <summary>Waiting for a process to open the other end of the pipe.</summary>
                ERROR_PIPE_LISTENING = 536,
                ERROR_ELEVATION_REQUIRED = 740, // (0x2E4)

                /// <summary>Access to the extended attribute was denied.</summary>
                ERROR_EA_ACCESS_DENIED = 994,
                /// <summary>The I/O operation has been aborted because of either a thread exit or an application request.</summary>
                ERROR_OPERATION_ABORTED = 995,
                /// <summary>Overlapped I/O event is not in a signaled state.</summary>
                ERROR_IO_INCOMPLETE = 996,
                /// <summary>Overlapped I/O operation is in progress.</summary>
                ERROR_IO_PENDING = 997, // dderror
                /// <summary>Invalid access to memory location.</summary>
                ERROR_NOACCESS = 998,
                /// <summary>Error performing inpage operation.</summary>
                ERROR_SWAPERROR = 999,
                /// <summary>Recursion too deep; the stack overflowed.</summary>
                ERROR_STACK_OVERFLOW = 1001,
                /// <summary>The window cannot act on the sent message.</summary>
                ERROR_INVALID_MESSAGE = 1002,
                /// <summary>Cannot complete this function.</summary>
                ERROR_CAN_NOT_COMPLETE = 1003,
                /// <summary>Invalid flags.</summary>
                ERROR_INVALID_FLAGS = 1004,
                /// <summary>The volume does not contain a recognized file system. Please make sure that all required file system drivers are loaded and that the volume is not corrupted.</summary>
                ERROR_UNRECOGNIZED_VOLUME = 1005,
                /// <summary>The volume for a file has been externally altered so that the opened file is no longer valid.</summary>
                ERROR_FILE_INVALID = 1006,
                /// <summary>The requested operation cannot be performed in full-screen mode.</summary>
                ERROR_FULLSCREEN_MODE = 1007,
                /// <summary>An attempt was made to reference a token that does not exist.</summary>
                ERROR_NO_TOKEN = 1008,
                /// <summary>The configuration registry database is corrupt.</summary>
                ERROR_BADDB = 1009,
                /// <summary>The configuration registry key is invalid.</summary>
                ERROR_BADKEY = 1010,
                /// <summary>The configuration registry key could not be opened.</summary>
                ERROR_CANTOPEN = 1011,
                /// <summary>The configuration registry key could not be read.</summary>
                ERROR_CANTREAD = 1012,
                /// <summary>The configuration registry key could not be written.</summary>
                ERROR_CANTWRITE = 1013,
                /// <summary>One of the files in the registry database had to be recovered by use of a log or alternate copy. The recovery was successful.</summary>
                ERROR_REGISTRY_RECOVERED = 1014,
                /// <summary>The registry is corrupted. The structure of one of the files containing registry data is corrupted, or the system's memory image of the file is corrupted, or the file could not be recovered because the alternate copy or log was absent or corrupted.</summary>
                ERROR_REGISTRY_CORRUPT = 1015,
                /// <summary>An I/O operation initiated by the registry failed unrecoverably. The registry could not read in, or write out, or flush, one of the files that contain the system's image of the registry.</summary>
                ERROR_REGISTRY_IO_FAILED = 1016,
                /// <summary>The system has attempted to load or restore a file into the registry, but the specified file is not in a registry file format.</summary>
                ERROR_NOT_REGISTRY_FILE = 1017,
                /// <summary>Illegal operation attempted on a registry key that has been marked for deletion.</summary>
                ERROR_KEY_DELETED = 1018,
                /// <summary>System could not allocate the required space in a registry log.</summary>
                ERROR_NO_LOG_SPACE = 1019,
                /// <summary>Cannot create a symbolic link in a registry key that already has subkeys or values.</summary>
                ERROR_KEY_HAS_CHILDREN = 1020,
                /// <summary>Cannot create a stable subkey under a volatile parent key.</summary>
                ERROR_CHILD_MUST_BE_VOLATILE = 1021,
                /// <summary>A notify change request is being completed and the information is not being returned in the caller's buffer. The caller now needs to enumerate the files to find the changes.</summary>
                ERROR_NOTIFY_ENUM_DIR = 1022,
                /// <summary>A stop control has been sent to a service that other running services are dependent on.</summary>
                ERROR_DEPENDENT_SERVICES_RUNNING = 1051,
                /// <summary>The requested control is not valid for this service.</summary>
                ERROR_INVALID_SERVICE_CONTROL = 1052,
                /// <summary>The service did not respond to the start or control request in a timely fashion.</summary>
                ERROR_SERVICE_REQUEST_TIMEOUT = 1053,
                /// <summary>A thread could not be created for the service.</summary>
                ERROR_SERVICE_NO_THREAD = 1054,
                /// <summary>The service database is locked.</summary>
                ERROR_SERVICE_DATABASE_LOCKED = 1055,
                /// <summary>An instance of the service is already running.</summary>
                ERROR_SERVICE_ALREADY_RUNNING = 1056,
                /// <summary>The account name is invalid or does not exist, or the password is invalid for the account name specified.</summary>
                ERROR_INVALID_SERVICE_ACCOUNT = 1057,
                /// <summary>The service cannot be started, either because it is disabled or because it has no enabled devices associated with it.</summary>
                ERROR_SERVICE_DISABLED = 1058,
                /// <summary>Circular service dependency was specified.</summary>
                ERROR_CIRCULAR_DEPENDENCY = 1059,
                /// <summary>The specified service does not exist as an installed service.</summary>
                ERROR_SERVICE_DOES_NOT_EXIST = 1060,
                /// <summary>The service cannot accept control messages at this time.</summary>
                ERROR_SERVICE_CANNOT_ACCEPT_CTRL = 1061,
                /// <summary>The service has not been started.</summary>
                ERROR_SERVICE_NOT_ACTIVE = 1062,
                /// <summary>The service process could not connect to the service controller.</summary>
                ERROR_FAILED_SERVICE_CONTROLLER_CONNECT = 1063,
                /// <summary>An exception occurred in the service when handling the control request.</summary>
                ERROR_EXCEPTION_IN_SERVICE = 1064,
                /// <summary>The database specified does not exist.</summary>
                ERROR_DATABASE_DOES_NOT_EXIST = 1065,
                /// <summary>The service has returned a service-specific error code.</summary>
                ERROR_SERVICE_SPECIFIC_ERROR = 1066,
                /// <summary>The process terminated unexpectedly.</summary>
                ERROR_PROCESS_ABORTED = 1067,
                /// <summary>The dependency service or group failed to start.</summary>
                ERROR_SERVICE_DEPENDENCY_FAIL = 1068,
                /// <summary>The service did not start due to a logon failure.</summary>
                ERROR_SERVICE_LOGON_FAILED = 1069,
                /// <summary>After starting, the service hung in a start-pending state.</summary>
                ERROR_SERVICE_START_HANG = 1070,
                /// <summary>The specified service database lock is invalid.</summary>
                ERROR_INVALID_SERVICE_LOCK = 1071,
                /// <summary>The specified service has been marked for deletion.</summary>
                ERROR_SERVICE_MARKED_FOR_DELETE = 1072,
                /// <summary>The specified service already exists.</summary>
                ERROR_SERVICE_EXISTS = 1073,
                /// <summary>The system is currently running with the last-known-good configuration.</summary>
                ERROR_ALREADY_RUNNING_LKG = 1074,
                /// <summary>The dependency service does not exist or has been marked for deletion.</summary>
                ERROR_SERVICE_DEPENDENCY_DELETED = 1075,
                /// <summary>The current boot has already been accepted for use as the last-known-good control set.</summary>
                ERROR_BOOT_ALREADY_ACCEPTED = 1076,
                /// <summary>No attempts to start the service have been made since the last boot.</summary>
                ERROR_SERVICE_NEVER_STARTED = 1077,
                /// <summary>The name is already in use as either a service name or a service display name.</summary>
                ERROR_DUPLICATE_SERVICE_NAME = 1078,
                /// <summary>The account specified for this service is different from the account specified for other services running in the same process.</summary>
                ERROR_DIFFERENT_SERVICE_ACCOUNT = 1079,
                /// <summary>Failure actions can only be set for Win32 services, not for drivers.</summary>
                ERROR_CANNOT_DETECT_DRIVER_FAILURE = 1080,
                /// <summary>This service runs in the same process as the service control manager. Therefore, the service control manager cannot take action if this service's process terminates unexpectedly.</summary>
                ERROR_CANNOT_DETECT_PROCESS_ABORT = 1081,
                /// <summary>No recovery program has been configured for this service.</summary>
                ERROR_NO_RECOVERY_PROGRAM = 1082,
                /// <summary>The erunutable program that this service is configured to run in does not implement the service.</summary>
                ERROR_SERVICE_NOT_IN_EXE = 1083,
                /// <summary>This service cannot be started in Safe Mode.</summary>
                ERROR_NOT_SAFEBOOT_SERVICE = 1084,
                /// <summary>The physical end of the tape has been reached.</summary>
                ERROR_END_OF_MEDIA = 1100,
                /// <summary>A tape access reached a filemark.</summary>
                ERROR_FILEMARK_DETECTED = 1101,
                /// <summary>The beginning of the tape or a partition was encountered.</summary>
                ERROR_BEGINNING_OF_MEDIA = 1102,
                /// <summary>A tape access reached the end of a set of files.</summary>
                ERROR_SETMARK_DETECTED = 1103,
                /// <summary>No more data is on the tape.</summary>
                ERROR_NO_DATA_DETECTED = 1104,
                /// <summary>Tape could not be partitioned.</summary>
                ERROR_PARTITION_FAILURE = 1105,
                /// <summary>When accessing a new tape of a multivolume partition, the current block size is incorrect.</summary>
                ERROR_INVALID_BLOCK_LENGTH = 1106,
                /// <summary>Tape partition information could not be found when loading a tape.</summary>
                ERROR_DEVICE_NOT_PARTITIONED = 1107,
                /// <summary>Unable to lock the media eject mechanism.</summary>
                ERROR_UNABLE_TO_LOCK_MEDIA = 1108,
                /// <summary>Unable to unload the media.</summary>
                ERROR_UNABLE_TO_UNLOAD_MEDIA = 1109,
                /// <summary>The media in the drive may have changed.</summary>
                ERROR_MEDIA_CHANGED = 1110,
                /// <summary>The I/O bus was reset.</summary>
                ERROR_BUS_RESET = 1111,
                /// <summary>No media in drive.</summary>
                ERROR_NO_MEDIA_IN_DRIVE = 1112,
                /// <summary>No mapping for the Unicode character exists in the target multi-byte code page.</summary>
                ERROR_NO_UNICODE_TRANSLATION = 1113,
                /// <summary>A dynamic link library (DLL) initialization routine failed.</summary>
                ERROR_DLL_INIT_FAILED = 1114,
                /// <summary>A system shutdown is in progress.</summary>
                ERROR_SHUTDOWN_IN_PROGRESS = 1115,
                /// <summary>Unable to abort the system shutdown because no shutdown was in progress.</summary>
                ERROR_NO_SHUTDOWN_IN_PROGRESS = 1116,
                /// <summary>The request could not be performed because of an I/O device error.</summary>
                ERROR_IO_DEVICE = 1117,
                /// <summary>No serial device was successfully initialized. The serial driver will unload.</summary>
                ERROR_SERIAL_NO_DEVICE = 1118,
                /// <summary>Unable to open a device that was sharing an interrupt request (IRQ) with other devices. At least one other device that uses that IRQ was already opened.</summary>
                ERROR_IRQ_BUSY = 1119,
                /// <summary>A serial I/O operation was completed by another write to the serial port. (The IOCTL_SERIAL_XOFF_COUNTER reached zero.</summary>)
                ERROR_MORE_WRITES = 1120,
                /// <summary>A serial I/O operation completed because the timeout period expired. (The IOCTL_SERIAL_XOFF_COUNTER did not reach zero.</summary>)
                ERROR_COUNTER_TIMEOUT = 1121,
                /// <summary>No ID address mark was found on the floppy disk.</summary>
                ERROR_FLOPPY_ID_MARK_NOT_FOUND = 1122,
                /// <summary>Mismatch between the floppy disk sector ID field and the floppy disk controller track address.</summary>
                ERROR_FLOPPY_WRONG_CYLINDER = 1123,
                /// <summary>The floppy disk controller reported an error that is not recognized by the floppy disk driver.</summary>
                ERROR_FLOPPY_UNKNOWN_ERROR = 1124,
                /// <summary>The floppy disk controller returned inconsistent results in its registers.</summary>
                ERROR_FLOPPY_BAD_REGISTERS = 1125,
                /// <summary>While accessing the hard disk, a recalibrate operation failed, even after retries.</summary>
                ERROR_DISK_RECALIBRATE_FAILED = 1126,
                /// <summary>While accessing the hard disk, a disk operation failed even after retries.</summary>
                ERROR_DISK_OPERATION_FAILED = 1127,
                /// <summary>While accessing the hard disk, a disk controller reset was needed, but even that failed.</summary>
                ERROR_DISK_RESET_FAILED = 1128,
                /// <summary>Physical end of tape encountered.</summary>
                ERROR_EOM_OVERFLOW = 1129,
                /// <summary>Not enough server storage is available to process this command.</summary>
                ERROR_NOT_ENOUGH_SERVER_MEMORY = 1130,
                /// <summary>A potential deadlock condition has been detected.</summary>
                ERROR_POSSIBLE_DEADLOCK = 1131,
                /// <summary>The base address or the file offset specified does not have the proper alignment.</summary>
                ERROR_MAPPED_ALIGNMENT = 1132,
                /// <summary>An attempt to change the system power state was vetoed by another application or driver.</summary>
                ERROR_SET_POWER_STATE_VETOED = 1140,
                /// <summary>The system BIOS failed an attempt to change the system power state.</summary>
                ERROR_SET_POWER_STATE_FAILED = 1141,
                /// <summary>An attempt was made to create more links on a file than the file system supports.</summary>
                ERROR_TOO_MANY_LINKS = 1142,
                /// <summary>The specified program requires a newer version of Windows.</summary>
                ERROR_OLD_WIN_VERSION = 1150,
                /// <summary>The specified program is not a Windows or MS-DOS program.</summary>
                ERROR_APP_WRONG_OS = 1151,
                /// <summary>Cannot start more than one instance of the specified program.</summary>
                ERROR_SINGLE_INSTANCE_APP = 1152,
                /// <summary>The specified program was written for an earlier version of Windows.</summary>
                ERROR_RMODE_APP = 1153,
                /// <summary>One of the library files needed to run this application is damaged.</summary>
                ERROR_INVALID_DLL = 1154,
                /// <summary>No application is associated with the specified file for this operation.</summary>
                ERROR_NO_ASSOCIATION = 1155,
                /// <summary>An error occurred in sending the command to the System.Windows.Forms.Application.</summary>
                ERROR_DDE_FAIL = 1156,
                /// <summary>One of the library files needed to run this application cannot be found.</summary>
                ERROR_DLL_NOT_FOUND = 1157,
                /// <summary>The current process has used all of its system allowance of handles for Window Manager objects.</summary>
                ERROR_NO_MORE_USER_HANDLES = 1158,
                /// <summary>The message can be used only with synchronous operations.</summary>
                ERROR_MESSAGE_SYNC_ONLY = 1159,
                /// <summary>The indicated source element has no media.</summary>
                ERROR_SOURCE_ELEMENT_EMPTY = 1160,
                /// <summary>The indicated destination element already contains media.</summary>
                ERROR_DESTINATION_ELEMENT_FULL = 1161,
                /// <summary>The indicated element does not exist.</summary>
                ERROR_ILLEGAL_ELEMENT_ADDRESS = 1162,
                /// <summary>The indicated element is part of a magazine that is not present.</summary>
                ERROR_MAGAZINE_NOT_PRESENT = 1163,
                /// <summary>The indicated device requires reinitialization due to hardware errors.</summary>
                ERROR_DEVICE_REINITIALIZATION_NEEDED = 1164, // dderror
                /// <summary>The device has indicated that cleaning is required before further operations are attempted.</summary>
                ERROR_DEVICE_REQUIRES_CLEANING = 1165,
                /// <summary>The device has indicated that its door is open.</summary>
                ERROR_DEVICE_DOOR_OPEN = 1166,
                /// <summary>The device is not connected.</summary>
                ERROR_DEVICE_NOT_CONNECTED = 1167,
                /// <summary>Element not found.</summary>
                ERROR_NOT_FOUND = 1168,
                /// <summary>There was no match for the specified key in the index.</summary>
                ERROR_NO_MATCH = 1169,
                /// <summary>The property set specified does not exist on the object.</summary>
                ERROR_SET_NOT_FOUND = 1170,
                /// <summary>The point passed to GetMouseMovePoints is not in the buffer.</summary>
                ERROR_POINT_NOT_FOUND = 1171,
                /// <summary>The tracking (workstation) service is not running.</summary>
                ERROR_NO_TRACKING_SERVICE = 1172,
                /// <summary>The Volume ID could not be found.</summary>
                ERROR_NO_VOLUME_ID = 1173,
                /// <summary>Unable to remove the file to be replaced.</summary>
                ERROR_UNABLE_TO_REMOVE_REPLACED = 1175,
                /// <summary>Unable to move the replacement file to the file to be replaced. The file to be replaced has retained its original name.</summary>
                ERROR_UNABLE_TO_MOVE_REPLACEMENT = 1176,
                /// <summary>Unable to move the replacement file to the file to be replaced. The file to be replaced has been renamed using the backup name.</summary>
                ERROR_UNABLE_TO_MOVE_REPLACEMENT_2 = 1177,
                /// <summary>The volume change journal is being deleted.</summary>
                ERROR_JOURNAL_DELETE_IN_PROGRESS = 1178,
                /// <summary>The volume change journal is not active.</summary>
                ERROR_JOURNAL_NOT_ACTIVE = 1179,
                /// <summary>A file was found, but it may not be the correct file.</summary>
                ERROR_POTENTIAL_FILE_FOUND = 1180,
                /// <summary>The journal entry has been deleted from the journal.</summary>
                ERROR_JOURNAL_ENTRY_DELETED = 1181,
                /// <summary>The specified device name is invalid.</summary>
                ERROR_BAD_DEVICE = 1200,
                /// <summary>The device is not currently connected but it is a remembered connection.</summary>
                ERROR_CONNECTION_UNAVAIL = 1201,
                /// <summary>The local device name has a remembered connection to another network resource.</summary>
                ERROR_DEVICE_ALREADY_REMEMBERED = 1202,
                /// <summary>No network provider accepted the given network path.</summary>
                ERROR_NO_NET_OR_BAD_PATH = 1203,
                /// <summary>The specified network provider name is invalid.</summary>
                ERROR_BAD_PROVIDER = 1204,
                /// <summary>Unable to open the network connection profile.</summary>
                ERROR_CANNOT_OPEN_PROFILE = 1205,
                /// <summary>The network connection profile is corrupted.</summary>
                ERROR_BAD_PROFILE = 1206,
                /// <summary>Cannot enumerate a noncontainer.</summary>
                ERROR_NOT_CONTAINER = 1207,
                /// <summary>An extended error has occurred.</summary>
                ERROR_EXTENDED_ERROR = 1208,
                /// <summary>The format of the specified group name is invalid.</summary>
                ERROR_INVALID_GROUPNAME = 1209,
                /// <summary>The format of the specified computer name is invalid.</summary>
                ERROR_INVALID_COMPUTERNAME = 1210,
                /// <summary>The format of the specified event name is invalid.</summary>
                ERROR_INVALID_EVENTNAME = 1211,
                /// <summary>The format of the specified domain name is invalid.</summary>
                ERROR_INVALID_DOMAINNAME = 1212,
                /// <summary>The format of the specified service name is invalid.</summary>
                ERROR_INVALID_SERVICENAME = 1213,
                /// <summary>The format of the specified network name is invalid.</summary>
                ERROR_INVALID_NETNAME = 1214,
                /// <summary>The format of the specified share name is invalid.</summary>
                ERROR_INVALID_SHARENAME = 1215,
                /// <summary>The format of the specified password is invalid.</summary>
                ERROR_INVALID_PASSWORDNAME = 1216,
                /// <summary>The format of the specified message name is invalid.</summary>
                ERROR_INVALID_MESSAGENAME = 1217,
                /// <summary>The format of the specified message destination is invalid.</summary>
                ERROR_INVALID_MESSAGEDEST = 1218,
                /// <summary>Multiple connections to a server or shared resource by the same user, using more than one user name, are not allowed. Disconnect all previous connections to the server or shared resource and try again.</summary>
                ERROR_SESSION_CREDENTIAL_CONFLICT = 1219,
                /// <summary>An attempt was made to establish a session to a network server, but there are already too many sessions established to that server.</summary>
                ERROR_REMOTE_SESSION_LIMIT_EXCEEDED = 1220,
                /// <summary>The workgroup or domain name is already in use by another computer on the network.</summary>
                ERROR_DUP_DOMAINNAME = 1221,
                /// <summary>The network is not present or not started.</summary>
                ERROR_NO_NETWORK = 1222,
                /// <summary>The operation was canceled by the user.</summary>
                ERROR_CANCELLED = 1223,
                /// <summary>The requested operation cannot be performed on a file with a user-mapped section open.</summary>
                ERROR_USER_MAPPED_FILE = 1224,
                /// <summary>The remote system refused the network connection.</summary>
                ERROR_CONNECTION_REFUSED = 1225,
                /// <summary>The network connection was gracefully closed.</summary>
                ERROR_GRACEFUL_DISCONNECT = 1226,
                /// <summary>The network transport endpoint already has an address associated with it.</summary>
                ERROR_ADDRESS_ALREADY_ASSOCIATED = 1227,
                /// <summary>An address has not yet been associated with the network endpoint.</summary>
                ERROR_ADDRESS_NOT_ASSOCIATED = 1228,
                /// <summary>An operation was attempted on a nonexistent network connection.</summary>
                ERROR_CONNECTION_INVALID = 1229,
                /// <summary>An invalid operation was attempted on an active network connection.</summary>
                ERROR_CONNECTION_ACTIVE = 1230,
                /// <summary>The network location cannot be reached. For information about network troubleshooting, see Windows Help.</summary>
                ERROR_NETWORK_UNREACHABLE = 1231,
                /// <summary>The network location cannot be reached. For information about network troubleshooting, see Windows Help.</summary>
                ERROR_HOST_UNREACHABLE = 1232,
                /// <summary>The network location cannot be reached. For information about network troubleshooting, see Windows Help.</summary>
                ERROR_PROTOCOL_UNREACHABLE = 1233,
                /// <summary>No service is operating at the destination network endpoint on the remote system.</summary>
                ERROR_PORT_UNREACHABLE = 1234,
                /// <summary>The request was aborted.</summary>
                ERROR_REQUEST_ABORTED = 1235,
                /// <summary>The network connection was aborted by the local system.</summary>
                ERROR_CONNECTION_ABORTED = 1236,
                /// <summary>The operation could not be completed. A retry should be performed.</summary>
                ERROR_RETRY = 1237,
                /// <summary>A connection to the server could not be made because the limit on the number of concurrent connections for this account has been reached.</summary>
                ERROR_CONNECTION_COUNT_LIMIT = 1238,
                /// <summary>Attempting to log in during an unauthorized time of day for this account.</summary>
                ERROR_LOGIN_TIME_RESTRICTION = 1239,
                /// <summary>The account is not authorized to log in from this station.</summary>
                ERROR_LOGIN_WKSTA_RESTRICTION = 1240,
                /// <summary>The network address could not be used for the operation requested.</summary>
                ERROR_INCORRECT_ADDRESS = 1241,
                /// <summary>The service is already registered.</summary>
                ERROR_ALREADY_REGISTERED = 1242,
                /// <summary>The specified service does not exist.</summary>
                ERROR_SERVICE_NOT_FOUND = 1243,
                /// <summary>The operation being requested was not performed because the user has not been authenticated.</summary>
                ERROR_NOT_AUTHENTICATED = 1244,
                /// <summary>The operation being requested was not performed because the user has not logged on to the network. The specified service does not exist.</summary>
                ERROR_NOT_LOGGED_ON = 1245,
                /// <summary>Continue with work in progress.</summary>
                ERROR_CONTINUE = 1246, // dderror
                /// <summary>An attempt was made to perform an initialization operation when initialization has already been completed.</summary>
                ERROR_ALREADY_INITIALIZED = 1247,
                /// <summary>No more local devices.</summary>
                ERROR_NO_MORE_DEVICES = 1248, // dderror
                /// <summary>The specified site does not exist.</summary>
                ERROR_NO_SUCH_SITE = 1249,
                /// <summary>A domain controller with the specified name already exists.</summary>
                ERROR_DOMAIN_CONTROLLER_EXISTS = 1250,
                /// <summary>This operation is supported only when you are connected to the server.</summary>
                ERROR_ONLY_IF_CONNECTED = 1251,
                /// <summary>The group policy framework should call the extension even if there are no changes.</summary>
                ERROR_OVERRIDE_NOCHANGES = 1252,
                /// <summary>The specified user does not have a valid profile.</summary>
                ERROR_BAD_USER_PROFILE = 1253,
                /// <summary>This operation is not supported on a Microsoft Small Business Server.</summary>
                ERROR_NOT_SUPPORTED_ON_SBS = 1254,
                /// <summary>The server machine is shutting down.</summary>
                ERROR_SERVER_SHUTDOWN_IN_PROGRESS = 1255,
                /// <summary>The remote system is not available. For information about network troubleshooting, see Windows Help.</summary>
                ERROR_HOST_DOWN = 1256,
                /// <summary>The security identifier provided is not from an account domain.</summary>
                ERROR_NON_ACCOUNT_SID = 1257,
                /// <summary>The security identifier provided does not have a domain component.</summary>
                ERROR_NON_DOMAIN_SID = 1258,
                /// <summary>AppHelp dialog canceled thus preventing the application from starting.</summary>
                ERROR_APPHELP_BLOCK = 1259,
                /// <summary>Windows cannot open this program because it has been prevented by a software restriction policy. For more information, open Event Viewer or contact your system administrator.</summary>
                ERROR_ACCESS_DISABLED_BY_POLICY = 1260,
                /// <summary>A program attempt to use an invalid register value. = Normally caused by an uninitialized register. This error is Itanium specific.</summary>
                ERROR_REG_NAT_CONSUMPTION = 1261,
                /// <summary>The share is currently offline or does not exist.</summary>
                ERROR_CSCSHARE_OFFLINE = 1262,
                /// <summary>The kerberos protocol encountered an error while validating the KDC certificate during smartcard logon.</summary>
                ERROR_PKINIT_FAILURE = 1263,
                /// <summary>The kerberos protocol encountered an error while attempting to utilize the smartcard subsystem.</summary>
                ERROR_SMARTCARD_SUBSYSTEM_FAILURE = 1264,
                /// <summary>The system detected a possible attempt to compromise security. Please ensure that you can contact the server that authenticated you.</summary>
                ERROR_DOWNGRADE_DETECTED = 1265,

                // Do not use ID's 1266 - 1270 as the symbolicNames have been moved to SEC_E_*

                /// <summary>The machine is locked and can not be shut down without the force option.</summary>
                ERROR_MACHINE_LOCKED = 1271,
                /// <summary>An application-defined callback gave invalid data when called.</summary>
                ERROR_CALLBACK_SUPPLIED_INVALID_DATA = 1273,
                /// <summary>The group policy framework should call the extension in the synchronous foreground policy refresh.</summary>
                ERROR_SYNC_FOREGROUND_REFRESH_REQUIRED = 1274,
                /// <summary>This driver has been blocked from loading.</summary>
                ERROR_DRIVER_BLOCKED = 1275,
                /// <summary>A dynamic link library (DLL) referenced a module that was neither a DLL nor the process's erunutable image.</summary>
                ERROR_INVALID_IMPORT_OF_NON_DLL = 1276,
                /// <summary>Windows cannot open this program since it has been disabled.</summary>
                ERROR_ACCESS_DISABLED_WEBBLADE = 1277,
                /// <summary>Windows cannot open this program because the license enforcement system has been tampered with or become corrupted.</summary>
                ERROR_ACCESS_DISABLED_WEBBLADE_TAMPER = 1278,
                /// <summary>A transaction recover failed.</summary>
                ERROR_RECOVERY_FAILURE = 1279,
                /// <summary>The current thread has already been converted to a fiber.</summary>
                ERROR_ALREADY_FIBER = 1280,
                /// <summary>The current thread has already been converted from a fiber.</summary>
                ERROR_ALREADY_THREAD = 1281,
                /// <summary>The system detected an overrun of a stack-based buffer in this System.Windows.Forms.Application. This overrun could potentially allow a malicious user to gain control of this System.Windows.Forms.Application.</summary>
                ERROR_STACK_BUFFER_OVERRUN = 1282,
                /// <summary>Data present in one of the parameters is more than the function can operate on.</summary>
                ERROR_PARAMETER_QUOTA_EXCEEDED = 1283,
                /// <summary>An attempt to do an operation on a debug object failed because the object is in the process of being deleted.</summary>
                ERROR_DEBUGGER_INACTIVE = 1284,
                /// <summary>An attempt to delay-load a .dll or get a function address in a delay-loaded .dll failed.</summary>
                ERROR_DELAY_LOAD_FAILED = 1285,
                /// <summary>%1 is a 16-bit System.Windows.Forms.Application. You do not have permissions to erunute 16-bit applications. Check your permissions with your system administrator.</summary>
                ERROR_VDM_DISALLOWED = 1286,


                // Add new status codes before this point unless there is a component specific section below.


                #region Security Status Codes
                /// <summary>Not all privileges referenced are assigned to the caller.</summary>
                ERROR_NOT_ALL_ASSIGNED = 1300,
                /// <summary>Some mapping between account names and security IDs was not done.</summary>
                ERROR_SOME_NOT_MAPPED = 1301,
                /// <summary>No system quota limits are specifically set for this account.</summary>
                ERROR_NO_QUOTAS_FOR_ACCOUNT = 1302,
                /// <summary>No encryption key is available. A well-known encryption key was returned.</summary>
                ERROR_LOCAL_USER_SESSION_KEY = 1303,
                /// <summary>The password is too complex to be converted to a LAN Manager password. The LAN Manager password returned is a NULL string.</summary>
                ERROR_NULL_LM_PASSWORD = 1304,
                /// <summary>The revision level is unknown.</summary>
                ERROR_UNKNOWN_REVISION = 1305,
                /// <summary>Indicates two revision levels are incompatible.</summary>
                ERROR_REVISION_MISMATCH = 1306,
                /// <summary>This security ID may not be assigned as the owner of this object.</summary>
                ERROR_INVALID_OWNER = 1307,
                /// <summary>This security ID may not be assigned as the primary group of an object.</summary>
                ERROR_INVALID_PRIMARY_GROUP = 1308,
                /// <summary>An attempt has been made to operate on an impersonation token by a thread that is not currently impersonating a client.</summary>
                ERROR_NO_IMPERSONATION_TOKEN = 1309,
                /// <summary>The group may not be disabled.</summary>
                ERROR_CANT_DISABLE_MANDATORY = 1310,
                /// <summary>There are currently no logon servers available to service the logon request.</summary>
                ERROR_NO_LOGON_SERVERS = 1311,
                /// <summary>A specified logon session does not exist. It may already have been terminated.</summary>
                ERROR_NO_SUCH_LOGON_SESSION = 1312,
                /// <summary>A specified privilege does not exist.</summary>
                ERROR_NO_SUCH_PRIVILEGE = 1313,
                /// <summary>A required privilege is not held by the client.</summary>
                ERROR_PRIVILEGE_NOT_HELD = 1314,
                /// <summary>The name provided is not a properly formed account name.</summary>
                ERROR_INVALID_ACCOUNT_NAME = 1315,
                /// <summary>The specified user already exists.</summary>
                ERROR_USER_EXISTS = 1316,
                /// <summary>The specified user does not exist.</summary>
                ERROR_NO_SUCH_USER = 1317,
                /// <summary>The specified group already exists.</summary>
                ERROR_GROUP_EXISTS = 1318,
                /// <summary>The specified group does not exist.</summary>
                ERROR_NO_SUCH_GROUP = 1319,
                /// <summary>Either the specified user account is already a member of the specified group, or the specified group cannot be deleted because it contains a member.</summary>
                ERROR_MEMBER_IN_GROUP = 1320,
                /// <summary>The specified user account is not a member of the specified group account.</summary>
                ERROR_MEMBER_NOT_IN_GROUP = 1321,
                /// <summary>The last remaining administration account cannot be disabled or deleted.</summary>
                ERROR_LAST_ADMIN = 1322,
                /// <summary>Unable to update the password. The value provided as the current password is incorrect.</summary>
                ERROR_WRONG_PASSWORD = 1323,
                /// <summary>Unable to update the password. The value provided for the new password contains values that are not allowed in passwords.</summary>
                ERROR_ILL_FORMED_PASSWORD = 1324,
                /// <summary>Unable to update the password. The value provided for the new password does not meet the length, complexity, or history requirement of the domain.</summary>
                ERROR_PASSWORD_RESTRICTION = 1325,
                /// <summary>Logon failure: unknown user name or bad password.</summary>
                ERROR_LOGON_FAILURE = 1326,
                /// <summary>Logon failure: user account restriction. = Possible reasons are blank passwords not allowed, logon hour restrictions, or a policy restriction has been enforced.</summary>
                ERROR_ACCOUNT_RESTRICTION = 1327,
                /// <summary>Logon failure: account logon time restriction violation.</summary>
                ERROR_INVALID_LOGON_HOURS = 1328,
                /// <summary>Logon failure: user not allowed to log on to this computer.</summary>
                ERROR_INVALID_WORKSTATION = 1329,
                /// <summary>Logon failure: the specified account password has expired.</summary>
                ERROR_PASSWORD_EXPIRED = 1330,
                /// <summary>Logon failure: account currently disabled.</summary>
                ERROR_ACCOUNT_DISABLED = 1331,
                /// <summary>No mapping between account names and security IDs was done.</summary>
                ERROR_NONE_MAPPED = 1332,
                /// <summary>Too many local user identifiers (LUIDs) were requested at one time.</summary>
                ERROR_TOO_MANY_LUIDS_REQUESTED = 1333,
                /// <summary>No more local user identifiers (LUIDs) are available.</summary>
                ERROR_LUIDS_EXHAUSTED = 1334,
                /// <summary>The subauthority part of a security ID is invalid for this particular use.</summary>
                ERROR_INVALID_SUB_AUTHORITY = 1335,
                /// <summary>The access control list (ACL) structure is invalid.</summary>
                ERROR_INVALID_ACL = 1336,
                /// <summary>The security ID structure is invalid.</summary>
                ERROR_INVALID_SID = 1337,
                /// <summary>The security descriptor structure is invalid.</summary>
                ERROR_INVALID_SECURITY_DESCR = 1338,
                /// <summary>The inherited access control list (ACL) or access control entry (ACE) could not be built.</summary>
                ERROR_BAD_INHERITANCE_ACL = 1340,
                /// <summary>The server is currently disabled.</summary>
                ERROR_SERVER_DISABLED = 1341,
                /// <summary>The server is currently enabled.</summary>
                ERROR_SERVER_NOT_DISABLED = 1342,
                /// <summary>The value provided was an invalid value for an identifier authority.</summary>
                ERROR_INVALID_ID_AUTHORITY = 1343,
                /// <summary>No more memory is available for security information updates.</summary>
                ERROR_ALLOTTED_SPACE_EXCEEDED = 1344,
                /// <summary>The specified attributes are invalid, or incompatible with the attributes for the group as a whole.</summary>
                ERROR_INVALID_GROUP_ATTRIBUTES = 1345,
                /// <summary>Either a required impersonation level was not provided, or the provided impersonation level is invalid.</summary>
                ERROR_BAD_IMPERSONATION_LEVEL = 1346,
                /// <summary>Cannot open an anonymous level security token.</summary>
                ERROR_CANT_OPEN_ANONYMOUS = 1347,
                /// <summary>The validation information class requested was invalid.</summary>
                ERROR_BAD_VALIDATION_CLASS = 1348,
                /// <summary>The type of the token is inappropriate for its attempted use.</summary>
                ERROR_BAD_TOKEN_TYPE = 1349,
                /// <summary>Unable to perform a security operation on an object that has no associated security.</summary>
                ERROR_NO_SECURITY_ON_OBJECT = 1350,
                /// <summary>Configuration information could not be read from the domain controller, either because the machine is unavailable, or access has been denied.</summary>
                ERROR_CANT_ACCESS_DOMAIN_INFO = 1351,
                /// <summary>The security account manager (SAM) or local security authority (LSA) server was in the wrong state to perform the security operation.</summary>
                ERROR_INVALID_SERVER_STATE = 1352,
                /// <summary>The domain was in the wrong state to perform the security operation.</summary>
                ERROR_INVALID_DOMAIN_STATE = 1353,
                /// <summary>This operation is only allowed for the Primary Domain Controller of the domain.</summary>
                ERROR_INVALID_DOMAIN_ROLE = 1354,
                /// <summary>The specified domain either does not exist or could not be contacted.</summary>
                ERROR_NO_SUCH_DOMAIN = 1355,
                /// <summary>The specified domain already exists.</summary>
                ERROR_DOMAIN_EXISTS = 1356,
                /// <summary>An attempt was made to exceed the limit on the number of domains per server.</summary>
                ERROR_DOMAIN_LIMIT_EXCEEDED = 1357,
                /// <summary>Unable to complete the requested operation because of either a catastrophic media failure or a data structure corruption on the disk.</summary>
                ERROR_INTERNAL_DB_CORRUPTION = 1358,
                /// <summary>An internal error occurred.</summary>
                ERROR_INTERNAL_ERROR = 1359,
                /// <summary>Generic access types were contained in an access mask which should already be mapped to nongeneric types.</summary>
                ERROR_GENERIC_NOT_MAPPED = 1360,
                /// <summary>A security descriptor is not in the right format (absolute or self-relative).</summary>
                ERROR_BAD_DESCRIPTOR_FORMAT = 1361,
                /// <summary>The requested action is restricted for use by logon processes only. The calling process has not registered as a logon process.</summary>
                ERROR_NOT_LOGON_PROCESS = 1362,
                /// <summary>Cannot start a new logon session with an ID that is already in use.</summary>
                ERROR_LOGON_SESSION_EXISTS = 1363,
                /// <summary>A specified authentication package is unknown.</summary>
                ERROR_NO_SUCH_PACKAGE = 1364,
                /// <summary>The logon session is not in a state that is consistent with the requested operation.</summary>
                ERROR_BAD_LOGON_SESSION_STATE = 1365,
                /// <summary>The logon session ID is already in use.</summary>
                ERROR_LOGON_SESSION_COLLISION = 1366,
                /// <summary>A logon request contained an invalid logon type value.</summary>
                ERROR_INVALID_LOGON_TYPE = 1367,
                /// <summary>Unable to impersonate using a named pipe until data has been read from that pipe.</summary>
                ERROR_CANNOT_IMPERSONATE = 1368,
                /// <summary>The transaction state of a registry subtree is incompatible with the requested operation.</summary>
                ERROR_RXACT_INVALID_STATE = 1369,
                /// <summary>An internal security database corruption has been encountered.</summary>
                ERROR_RXACT_COMMIT_FAILURE = 1370,
                /// <summary>Cannot perform this operation on built-in accounts.</summary>
                ERROR_SPECIAL_ACCOUNT = 1371,
                /// <summary>Cannot perform this operation on this built-in special group.</summary>
                ERROR_SPECIAL_GROUP = 1372,
                /// <summary>Cannot perform this operation on this built-in special user.</summary>
                ERROR_SPECIAL_USER = 1373,
                /// <summary>The user cannot be removed from a group because the group is currently the user's primary group.</summary>
                ERROR_MEMBERS_PRIMARY_GROUP = 1374,
                /// <summary>The token is already in use as a primary token.</summary>
                ERROR_TOKEN_ALREADY_IN_USE = 1375,
                /// <summary>The specified local group does not exist.</summary>
                ERROR_NO_SUCH_ALIAS = 1376,
                /// <summary>The specified account name is not a member of the local group.</summary>
                ERROR_MEMBER_NOT_IN_ALIAS = 1377,
                /// <summary>The specified account name is already a member of the local group.</summary>
                ERROR_MEMBER_IN_ALIAS = 1378,
                /// <summary>The specified local group already exists.</summary>
                ERROR_ALIAS_EXISTS = 1379,
                /// <summary>Logon failure: the user has not been granted the requested logon type at this computer.</summary>
                ERROR_LOGON_NOT_GRANTED = 1380,
                /// <summary>The maximum number of secrets that may be stored in a single system has been exceeded.</summary>
                ERROR_TOO_MANY_SECRETS = 1381,
                /// <summary>The length of a secret exceeds the maximum length allowed.</summary>
                ERROR_SECRET_TOO_LONG = 1382,
                /// <summary>The local security authority database contains an internal inconsistency.</summary>
                ERROR_INTERNAL_DB_ERROR = 1383,
                /// <summary>During a logon attempt, the user's security context accumulated too many security IDs.</summary>
                ERROR_TOO_MANY_CONTEXT_IDS = 1384,
                /// <summary>Logon failure: the user has not been granted the requested logon type at this computer.</summary>
                ERROR_LOGON_TYPE_NOT_GRANTED = 1385,
                /// <summary>A cross-encrypted password is necessary to change a user password.</summary>
                ERROR_NT_CROSS_ENCRYPTION_REQUIRED = 1386,
                /// <summary>A member could not be added to or removed from the local group because the member does not exist.</summary>
                ERROR_NO_SUCH_MEMBER = 1387,
                /// <summary>A new member could not be added to a local group because the member has the wrong account type.</summary>
                ERROR_INVALID_MEMBER = 1388,
                /// <summary>Too many security IDs have been specified.</summary>
                ERROR_TOO_MANY_SIDS = 1389,
                /// <summary>A cross-encrypted password is necessary to change this user password.</summary>
                ERROR_LM_CROSS_ENCRYPTION_REQUIRED = 1390,
                /// <summary>Indicates an ACL contains no inheritable components.</summary>
                ERROR_NO_INHERITANCE = 1391,
                /// <summary>The file or directory is corrupted and unreadable.</summary>
                ERROR_FILE_CORRUPT = 1392,
                /// <summary>The disk structure is corrupted and unreadable.</summary>
                ERROR_DISK_CORRUPT = 1393,
                /// <summary>There is no user session key for the specified logon session.</summary>
                ERROR_NO_USER_SESSION_KEY = 1394,
                /// <summary>The service being accessed is licensed for a particular number of connections. No more connections can be made to the service at this time because there are already as many connections as the service can accept.</summary>
                ERROR_LICENSE_QUOTA_EXCEEDED = 1395,
                /// <summary>Logon Failure: The target account name is incorrect.</summary>
                ERROR_WRONG_TARGET_NAME = 1396,
                /// <summary>Mutual Authentication failed. The server's password is [Out ] of date at the domain controller.</summary>
                ERROR_MUTUAL_AUTH_FAILED = 1397,
                /// <summary>There is a time and/or date difference between the client and server.</summary>
                ERROR_TIME_SKEW = 1398,
                /// <summary>This operation can not be performed on the current domain.</summary>
                ERROR_CURRENT_DOMAIN_NOT_ALLOWED = 1399,
                #endregion


                #region WinUser Error Codes
                /// <summary>Invalid window handle.</summary>
                ERROR_INVALID_WINDOW_HANDLE = 1400,
                /// <summary>Invalid menu handle.</summary>
                ERROR_INVALID_MENU_HANDLE = 1401,
                /// <summary>Invalid cursor handle.</summary>
                ERROR_INVALID_CURSOR_HANDLE = 1402,
                /// <summary>Invalid accelerator table handle.</summary>
                ERROR_INVALID_ACCEL_HANDLE = 1403,
                /// <summary>Invalid hook handle.</summary>
                ERROR_INVALID_HOOK_HANDLE = 1404,
                /// <summary>Invalid handle to a multiple-window position structure.</summary>
                ERROR_INVALID_DWP_HANDLE = 1405,
                /// <summary>Cannot create a top-level child window.</summary>
                ERROR_TLW_WITH_WSCHILD = 1406,
                /// <summary>Cannot find window class.</summary>
                ERROR_CANNOT_FIND_WND_CLASS = 1407,
                /// <summary>Invalid window; it belongs to other thread.</summary>
                ERROR_WINDOW_OF_OTHER_THREAD = 1408,
                /// <summary>Hot key is already registered.</summary>
                ERROR_HOTKEY_ALREADY_REGISTERED = 1409,
                /// <summary>Class already exists.</summary>
                ERROR_CLASS_ALREADY_EXISTS = 1410,
                /// <summary>Class does not exist.</summary>
                ERROR_CLASS_DOES_NOT_EXIST = 1411,
                /// <summary>Class still has open windows.</summary>
                ERROR_CLASS_HAS_WINDOWS = 1412,
                /// <summary>Invalid index.</summary>
                ERROR_INVALID_INDEX = 1413,
                /// <summary>Invalid icon handle.</summary>
                ERROR_INVALID_ICON_HANDLE = 1414,
                /// <summary>Using private DIALOG window words.</summary>
                ERROR_PRIVATE_DIALOG_INDEX = 1415,
                /// <summary>The list box identifier was not found.</summary>
                ERROR_LISTBOX_ID_NOT_FOUND = 1416,
                /// <summary>No wildcards were found.</summary>
                ERROR_NO_WILDCARD_CHARACTERS = 1417,
                /// <summary>Thread does not have a clipboard open.</summary>
                ERROR_CLIPBOARD_NOT_OPEN = 1418,
                /// <summary>Hot key is not registered.</summary>
                ERROR_HOTKEY_NOT_REGISTERED = 1419,
                /// <summary>The window is not a valid dialog window.</summary>
                ERROR_WINDOW_NOT_DIALOG = 1420,
                /// <summary>Control ID not found.</summary>
                ERROR_CONTROL_ID_NOT_FOUND = 1421,
                /// <summary>Invalid message for a combo box because it does not have an edit control.</summary>
                ERROR_INVALID_COMBOBOX_MESSAGE = 1422,
                /// <summary>The window is not a combo box.</summary>
                ERROR_WINDOW_NOT_COMBOBOX = 1423,
                /// <summary>Height must be less than 256.</summary>
                ERROR_INVALID_EDIT_HEIGHT = 1424,
                /// <summary>Invalid device context (DC) handle.</summary>
                ERROR_DC_NOT_FOUND = 1425,
                /// <summary>Invalid hook procedure type.</summary>
                ERROR_INVALID_HOOK_FILTER = 1426,
                /// <summary>Invalid hook procedure.</summary>
                ERROR_INVALID_FILTER_PROC = 1427,
                /// <summary>Cannot set nonlocal hook without a module handle.</summary>
                ERROR_HOOK_NEEDS_HMOD = 1428,
                /// <summary>This hook procedure can only be set globally.</summary>
                ERROR_GLOBAL_ONLY_HOOK = 1429,
                /// <summary>The journal hook procedure is already installed.</summary>
                ERROR_JOURNAL_HOOK_SET = 1430,
                /// <summary>The hook procedure is not installed.</summary>
                ERROR_HOOK_NOT_INSTALLED = 1431,
                /// <summary>Invalid message for single-selection list box.</summary>
                ERROR_INVALID_LB_MESSAGE = 1432,
                /// <summary>LB_SETCOUNT sent to non-lazy list box.</summary>
                ERROR_SETCOUNT_ON_BAD_LB = 1433,
                /// <summary>This list box does not support tab stops.</summary>
                ERROR_LB_WITHOUT_TABSTOPS = 1434,
                /// <summary>Cannot destroy object created by another thread.</summary>
                ERROR_DESTROY_OBJECT_OF_OTHER_THREAD = 1435,
                /// <summary>Child windows cannot have menus.</summary>
                ERROR_CHILD_WINDOW_MENU = 1436,
                /// <summary>The window does not have a system menu.</summary>
                ERROR_NO_SYSTEM_MENU = 1437,
                /// <summary>Invalid message box style.</summary>
                ERROR_INVALID_MSGBOX_STYLE = 1438,
                /// <summary>Invalid system-wide (SPI_*) parameter.</summary>
                ERROR_INVALID_SPI_VALUE = 1439,
                /// <summary>Screen already locked.</summary>
                ERROR_SCREEN_ALREADY_LOCKED = 1440,
                /// <summary>All handles to windows in a multiple-window position structure must have the same parent.</summary>
                ERROR_HWNDS_HAVE_DIFF_PARENT = 1441,
                /// <summary>The window is not a child window.</summary>
                ERROR_NOT_CHILD_WINDOW = 1442,
                /// <summary>Invalid GW_* command.</summary>
                ERROR_INVALID_GW_COMMAND = 1443,
                /// <summary>Invalid thread identifier.</summary>
                ERROR_INVALID_THREAD_ID = 1444,
                /// <summary>Cannot process a message from a window that is not a multiple document interface (MDI) window.</summary>
                ERROR_NON_MDICHILD_WINDOW = 1445,
                /// <summary>Popup menu already active.</summary>
                ERROR_POPUP_ALREADY_ACTIVE = 1446,
                /// <summary>The window does not have scroll bars.</summary>
                ERROR_NO_SCROLLBARS = 1447,
                /// <summary>Scroll bar range cannot be greater than MAXLONG.</summary>
                ERROR_INVALID_SCROLLBAR_RANGE = 1448,
                /// <summary>Cannot show or remove the window in the way specified.</summary>
                ERROR_INVALID_SHOWWIN_COMMAND = 1449,
                /// <summary>Insufficient system resources exist to complete the requested service.</summary>
                ERROR_NO_SYSTEM_RESOURCES = 1450,
                /// <summary>Insufficient system resources exist to complete the requested service.</summary>
                ERROR_NONPAGED_SYSTEM_RESOURCES = 1451,
                /// <summary>Insufficient system resources exist to complete the requested service.</summary>
                ERROR_PAGED_SYSTEM_RESOURCES = 1452,
                /// <summary>Insufficient quota to complete the requested service.</summary>
                ERROR_WORKING_SET_QUOTA = 1453,
                /// <summary>Insufficient quota to complete the requested service.</summary>
                ERROR_PAGEFILE_QUOTA = 1454,
                /// <summary>The paging file is too small for this operation to complete.</summary>
                ERROR_COMMITMENT_LIMIT = 1455,
                /// <summary>A menu item was not found.</summary>
                ERROR_MENU_ITEM_NOT_FOUND = 1456,
                /// <summary>Invalid keyboard layout handle.</summary>
                ERROR_INVALID_KEYBOARD_HANDLE = 1457,
                /// <summary>Hook type not allowed.</summary>
                ERROR_HOOK_TYPE_NOT_ALLOWED = 1458,
                /// <summary>This operation requires an interactive window station.</summary>
                ERROR_REQUIRES_INTERACTIVE_WINDOWSTATION = 1459,
                /// <summary>This operation returned because the timeout period expired.</summary>
                ERROR_TIMEOUT = 1460,
                /// <summary>Invalid monitor handle.</summary>
                ERROR_INVALID_MONITOR_HANDLE = 1461,
                #endregion


                #region Eventlog Status Codes 
                /// <summary>The event log file is corrupted.</summary>
                ERROR_EVENTLOG_FILE_CORRUPT = 1500,
                /// <summary>No event log file could be opened, so the event logging service did not start.</summary>
                ERROR_EVENTLOG_CANT_START = 1501,
                /// <summary>The event log file is full.</summary>
                ERROR_LOG_FILE_FULL = 1502,
                /// <summary>The event log file has changed between read operations.</summary>
                ERROR_EVENTLOG_FILE_CHANGED = 1503,
                #endregion


                #region MSI Error Codes
                /// <summary>The Windows Installer Service could not be accessed. This can occur if you are running Windows in safe mode, or if the Windows Installer is not correctly installed. Contact your support personnel for assistance.</summary>
                ERROR_INSTALL_SERVICE_FAILURE = 1601,
                /// <summary>User cancelled installation.</summary>
                ERROR_INSTALL_USEREXIT = 1602,
                /// <summary>Fatal error during installation.</summary>
                ERROR_INSTALL_FAILURE = 1603,
                /// <summary>Installation suspended, incomplete.</summary>
                ERROR_INSTALL_SUSPEND = 1604,
                /// <summary>This action is only valid for products that are currently installed.</summary>
                ERROR_UNKNOWN_PRODUCT = 1605,
                /// <summary>Feature ID not registered.</summary>
                ERROR_UNKNOWN_FEATURE = 1606,
                /// <summary>Component ID not registered.</summary>
                ERROR_UNKNOWN_COMPONENT = 1607,
                /// <summary>Unknown property.</summary>
                ERROR_UNKNOWN_PROPERTY = 1608,
                /// <summary>Handle is in an invalid state.</summary>
                ERROR_INVALID_HANDLE_STATE = 1609,
                /// <summary>The configuration data for this product is corrupt. = Contact your support personnel.</summary>
                ERROR_BAD_CONFIGURATION = 1610,
                /// <summary>Component qualifier not present.</summary>
                ERROR_INDEX_ABSENT = 1611,
                /// <summary>The installation source for this product is not available. = Verify that the source exists and that you can access it.</summary>
                ERROR_INSTALL_SOURCE_ABSENT = 1612,
                /// <summary>This installation package cannot be installed by the Windows Installer service. = You must install a Windows service pack that contains a newer version of the Windows Installer service.</summary>
                ERROR_INSTALL_PACKAGE_VERSION = 1613,
                /// <summary>Product is uninstalled.</summary>
                ERROR_PRODUCT_UNINSTALLED = 1614,
                /// <summary>SQL query syntax invalid or unsupported.</summary>
                ERROR_BAD_QUERY_SYNTAX = 1615,
                /// <summary>Record field does not exist.</summary>
                ERROR_INVALID_FIELD = 1616,
                /// <summary>The device has been removed.</summary>
                ERROR_DEVICE_REMOVED = 1617,
                /// <summary>Another installation is already in progress. = Complete that installation before proceeding with this install.</summary>
                ERROR_INSTALL_ALREADY_RUNNING = 1618,
                /// <summary>This installation package could not be opened. = Verify that the package exists and that you can access it, or contact the application vendor to verify that this is a valid Windows Installer package.</summary>
                ERROR_INSTALL_PACKAGE_OPEN_FAILED = 1619,
                /// <summary>This installation package could not be opened. = Contact the application vendor to verify that this is a valid Windows Installer package.</summary>
                ERROR_INSTALL_PACKAGE_INVALID = 1620,
                /// <summary>There was an error starting the Windows Installer service user interface. = Contact your support personnel.</summary>
                ERROR_INSTALL_UI_FAILURE = 1621,
                /// <summary>Error opening installation log file. Verify that the specified log file location exists and that you can write to it.</summary>
                ERROR_INSTALL_LOG_FAILURE = 1622,
                /// <summary>The language of this installation package is not supported by your system.</summary>
                ERROR_INSTALL_LANGUAGE_UNSUPPORTED = 1623,
                /// <summary>Error applying transforms. = Verify that the specified transform paths are valid.</summary>
                ERROR_INSTALL_TRANSFORM_FAILURE = 1624,
                /// <summary>This installation is forbidden by system policy. = Contact your system administrator.</summary>
                ERROR_INSTALL_PACKAGE_REJECTED = 1625,
                /// <summary>Function could not be erunuted.</summary>
                ERROR_FUNCTION_NOT_CALLED = 1626,
                /// <summary>Function failed during erunution.</summary>
                ERROR_FUNCTION_FAILED = 1627,
                /// <summary>Invalid or unknown table specified.</summary>
                ERROR_INVALID_TABLE = 1628,
                /// <summary>Data supplied is of wrong type.</summary>
                ERROR_DATATYPE_MISMATCH = 1629,
                /// <summary>Data of this type is not supported.</summary>
                ERROR_UNSUPPORTED_TYPE = 1630,
                /// <summary>The Windows Installer service failed to start. = Contact your support personnel.</summary>
                ERROR_CREATE_FAILED = 1631,
                /// <summary>The Temp folder is on a drive that is full or is inaccessible. Free up space on the drive or verify that you have write permission on the Temp folder.</summary>
                ERROR_INSTALL_TEMP_UNWRITABLE = 1632,
                /// <summary>This installation package is not supported by this processor type. Contact your product vendor.</summary>
                ERROR_INSTALL_PLATFORM_UNSUPPORTED = 1633,
                /// <summary>Component not used on this computer.</summary>
                ERROR_INSTALL_NOTUSED = 1634,
                /// <summary>This patch package could not be opened. = Verify that the patch package exists and that you can access it, or contact the application vendor to verify that this is a valid Windows Installer patch package.</summary>
                ERROR_PATCH_PACKAGE_OPEN_FAILED = 1635,
                /// <summary>This patch package could not be opened. = Contact the application vendor to verify that this is a valid Windows Installer patch package.</summary>
                ERROR_PATCH_PACKAGE_INVALID = 1636,
                /// <summary>This patch package cannot be processed by the Windows Installer service. = You must install a Windows service pack that contains a newer version of the Windows Installer service.</summary>
                ERROR_PATCH_PACKAGE_UNSUPPORTED = 1637,
                /// <summary>Another version of this product is already installed. = Installation of this version cannot continue. = To configure or remove the existing version of this product, use Add/Remove Programs on the Control Panel.</summary>
                ERROR_PRODUCT_VERSION = 1638,
                /// <summary>Invalid command line argument. = Consult the Windows Installer SDK for detailed command line help.</summary>
                ERROR_INVALID_COMMAND_LINE = 1639,
                /// <summary>Only administrators have permission to add, remove, or configure server software during a Terminal services remote session. If you want to install or configure software on the server, contact your network administrator.</summary>
                ERROR_INSTALL_REMOTE_DISALLOWED = 1640,
                /// <summary>The requested operation completed successfully. = The system will be restarted so the changes can take effect.</summary>
                ERROR_SUCCESS_REBOOT_INITIATED = 1641,
                /// <summary>The upgrade patch cannot be installed by the Windows Installer service because the program to be upgraded may be missing, or the upgrade patch may update a different version of the program. Verify that the program to be upgraded exists on your computer and that you have the correct upgrade patch.</summary>
                ERROR_PATCH_TARGET_NOT_FOUND = 1642,
                /// <summary>The patch package is not permitted by software restriction policy.</summary>
                ERROR_PATCH_PACKAGE_REJECTED = 1643,
                /// <summary>One or more customizations are not permitted by software restriction policy.</summary>
                ERROR_INSTALL_TRANSFORM_REJECTED = 1644,
                /// <summary>The Windows Installer does not permit installation from a Remote Desktop Connection.</summary>
                ERROR_INSTALL_REMOTE_PROHIBITED = 1645,

                #endregion


                #region RPC Status Codes

                ///<summary> The string binding is invalid.</summary> 
                RPC_S_INVALID_STRING_BINDING = 1700,
                ///<summary> The binding handle is not the correct type.</summary> 
                RPC_S_WRONG_KIND_OF_BINDING = 1701,
                ///<summary> The binding handle is invalid.</summary> 
                RPC_S_INVALID_BINDING = 1702,
                ///<summary> The RPC protocol sequence is not supported.</summary> 
                RPC_S_PROTSEQ_NOT_SUPPORTED = 1703,
                ///<summary> The RPC protocol sequence is invalid.</summary> 
                RPC_S_INVALID_RPC_PROTSEQ = 1704,
                ///<summary> The string universal unique identifier (UUID) is invalid.</summary> 
                RPC_S_INVALID_STRING_UUID = 1705,
                ///<summary> The endpoint format is invalid.</summary> 
                RPC_S_INVALID_ENDPOINT_FORMAT = 1706,
                ///<summary> The network address is invalid.</summary> 
                RPC_S_INVALID_NET_ADDR = 1707,
                ///<summary> No endpoint was found.</summary> 
                RPC_S_NO_ENDPOINT_FOUND = 1708,
                ///<summary> The timeout value is invalid.</summary> 
                RPC_S_INVALID_TIMEOUT = 1709,
                ///<summary> The object universal unique identifier (UUID) was not found.</summary> 
                RPC_S_OBJECT_NOT_FOUND = 1710,
                ///<summary> The object universal unique identifier (UUID) has already been registered.</summary> 
                RPC_S_ALREADY_REGISTERED = 1711,
                ///<summary> The type universal unique identifier (UUID) has already been registered.</summary> 
                RPC_S_TYPE_ALREADY_REGISTERED = 1712,
                ///<summary> The RPC server is already listening.</summary> 
                RPC_S_ALREADY_LISTENING = 1713,
                ///<summary> No protocol sequences have been registered.</summary> 
                RPC_S_NO_PROTSEQS_REGISTERED = 1714,
                ///<summary> The RPC server is not listening.</summary> 
                RPC_S_NOT_LISTENING = 1715,
                ///<summary> The manager type is unknown.</summary> 
                RPC_S_UNKNOWN_MGR_TYPE = 1716,
                ///<summary> The interface is unknown.</summary> 
                RPC_S_UNKNOWN_IF = 1717,
                ///<summary> There are no bindings.</summary> 
                RPC_S_NO_BINDINGS = 1718,
                ///<summary> There are no protocol sequences.</summary> 
                RPC_S_NO_PROTSEQS = 1719,
                ///<summary> The endpoint cannot be created.</summary> 
                RPC_S_CANT_CREATE_ENDPOINT = 1720,
                ///<summary> Not enough resources are available to complete this operation.</summary> 
                RPC_S_OUT_OF_RESOURCES = 1721,
                ///<summary> The RPC server is unavailable.</summary> 
                RPC_S_SERVER_UNAVAILABLE = 1722,
                ///<summary> The RPC server is too busy to complete this operation.</summary> 
                RPC_S_SERVER_TOO_BUSY = 1723,
                ///<summary> The network options are invalid.</summary> 
                RPC_S_INVALID_NETWORK_OPTIONS = 1724,
                ///<summary> There are no remote procedure calls active on this thread.</summary> 
                RPC_S_NO_CALL_ACTIVE = 1725,
                ///<summary> The remote procedure call failed.</summary> 
                RPC_S_CALL_FAILED = 1726,
                ///<summary> The remote procedure call failed and did not erunute.</summary> 
                RPC_S_CALL_FAILED_DNE = 1727,
                ///<summary> A remote procedure call (RPC) protocol error occurred.</summary> 
                RPC_S_PROTOCOL_ERROR = 1728,
                ///<summary> The transfer syntax is not supported by the RPC server.</summary> 
                RPC_S_UNSUPPORTED_TRANS_SYN = 1730,
                ///<summary> The universal unique identifier (UUID) type is not supported.</summary> 
                RPC_S_UNSUPPORTED_TYPE = 1732,
                ///<summary> The tag is invalid.</summary> 
                RPC_S_INVALID_TAG = 1733,
                ///<summary> The array bounds are invalid.</summary> 
                RPC_S_INVALID_BOUND = 1734,
                ///<summary> The binding does not contain an entry name.</summary> 
                RPC_S_NO_ENTRY_NAME = 1735,
                ///<summary> The name syntax is invalid.</summary> 
                RPC_S_INVALID_NAME_SYNTAX = 1736,
                ///<summary> The name syntax is not supported.</summary> 
                RPC_S_UNSUPPORTED_NAME_SYNTAX = 1737,
                ///<summary> No network address is available to use to construct a universal unique identifier (UUID).</summary> 
                RPC_S_UUID_NO_ADDRESS = 1739,
                ///<summary> The endpoint is a duplicate.</summary> 
                RPC_S_DUPLICATE_ENDPOINT = 1740,
                ///<summary> The authentication type is unknown.</summary> 
                RPC_S_UNKNOWN_AUTHN_TYPE = 1741,
                ///<summary> The maximum number of calls is too small.</summary> 
                RPC_S_MAX_CALLS_TOO_SMALL = 1742,
                ///<summary> The string is too long.</summary> 
                RPC_S_STRING_TOO_LONG = 1743,
                ///<summary> The RPC protocol sequence was not found.</summary> 
                RPC_S_PROTSEQ_NOT_FOUND = 1744,
                ///<summary> The procedure number is [Out ] of range.</summary> 
                RPC_S_PROCNUM_OUT_OF_RANGE = 1745,
                ///<summary> The binding does not contain any authentication information.</summary> 
                RPC_S_BINDING_HAS_NO_AUTH = 1746,
                ///<summary> The authentication service is unknown.</summary> 
                RPC_S_UNKNOWN_AUTHN_SERVICE = 1747,
                ///<summary> The authentication level is unknown.</summary> 
                RPC_S_UNKNOWN_AUTHN_LEVEL = 1748,
                ///<summary> The security context is invalid.</summary> 
                RPC_S_INVALID_AUTH_IDENTITY = 1749,
                ///<summary> The authorization service is unknown.</summary> 
                RPC_S_UNKNOWN_AUTHZ_SERVICE = 1750,
                ///<summary> The entry is invalid.</summary> 
                EPT_S_INVALID_ENTRY = 1751,
                ///<summary> The server endpoint cannot perform the operation.</summary> 
                EPT_S_CANT_PERFORM_OP = 1752,
                ///<summary> There are no more endpoints available from the endpoint mapper.</summary> 
                EPT_S_NOT_REGISTERED = 1753,
                ///<summary> No interfaces have been exported.</summary> 
                RPC_S_NOTHING_TO_EXPORT = 1754,
                ///<summary> The entry name is incomplete.</summary> 
                RPC_S_INCOMPLETE_NAME = 1755,
                ///<summary> The version option is invalid.</summary> 
                RPC_S_INVALID_VERS_OPTION = 1756,
                ///<summary> There are no more members.</summary> 
                RPC_S_NO_MORE_MEMBERS = 1757,
                ///<summary> There is nothing to unexport.</summary> 
                RPC_S_NOT_ALL_OBJS_UNEXPORTED = 1758,
                ///<summary> The interface was not found.</summary> 
                RPC_S_INTERFACE_NOT_FOUND = 1759,
                ///<summary> The entry already exists.</summary> 
                RPC_S_ENTRY_ALREADY_EXISTS = 1760,
                ///<summary> The entry is not found.</summary> 
                RPC_S_ENTRY_NOT_FOUND = 1761,
                ///<summary> The name service is unavailable.</summary> 
                RPC_S_NAME_SERVICE_UNAVAILABLE = 1762,
                ///<summary> The network address family is invalid.</summary> 
                RPC_S_INVALID_NAF_ID = 1763,
                ///<summary> The requested operation is not supported.</summary> 
                RPC_S_CANNOT_SUPPORT = 1764,
                ///<summary> No security context is available to allow impersonation.</summary> 
                RPC_S_NO_CONTEXT_AVAILABLE = 1765,
                ///<summary> An internal error occurred in a remote procedure call (RPC).</summary> 
                RPC_S_INTERNAL_ERROR = 1766,
                ///<summary> The RPC server attempted an integer division by zero.</summary> 
                RPC_S_ZERO_DIVIDE = 1767,
                ///<summary> An addressing error occurred in the RPC server.</summary> 
                RPC_S_ADDRESS_ERROR = 1768,
                ///<summary> A floating-point operation at the RPC server caused a division by zero.</summary> 
                RPC_S_FP_DIV_ZERO = 1769,
                ///<summary> A floating-point underflow occurred at the RPC server.</summary> 
                RPC_S_FP_UNDERFLOW = 1770,
                ///<summary> A floating-point overflow occurred at the RPC server.</summary> 
                RPC_S_FP_OVERFLOW = 1771,
                ///<summary> The list of RPC servers available for the binding of auto handles has been exhausted.</summary> 
                RPC_X_NO_MORE_ENTRIES = 1772,
                ///<summary> Unable to open the character translation table file.</summary> 
                RPC_X_SS_CHAR_TRANS_OPEN_FAIL = 1773,
                ///<summary> The file containing the character translation table has fewer than 512 bytes.</summary> 
                RPC_X_SS_CHAR_TRANS_SHORT_FILE = 1774,
                ///<summary> A null context handle was passed from the client to the host during a remote procedure call.</summary> 
                RPC_X_SS_IN_NULL_CONTEXT = 1775,
                ///<summary> The context handle changed during a remote procedure call.</summary> 
                RPC_X_SS_CONTEXT_DAMAGED = 1777,
                ///<summary> The binding handles passed to a remote procedure call do not match.</summary> 
                RPC_X_SS_HANDLES_MISMATCH = 1778,
                ///<summary> The stub is unable to get the remote procedure call handle.</summary> 
                RPC_X_SS_CANNOT_GET_CALL_HANDLE = 1779,
                ///<summary> A null reference pointer was passed to the stub.</summary> 
                RPC_X_NULL_REF_POINTER = 1780,
                ///<summary> The enumeration value is [Out ] of range.</summary> 
                RPC_X_ENUM_VALUE_OUT_OF_RANGE = 1781,               // MessageId: RPC_X_BYTE_COUNT_TOO_SMAL ,
                ///<summary> The byte count is too small.</summary> 
                RPC_X_BYTE_COUNT_TOO_SMALL = 1782,
                ///<summary> The stub received bad data.</summary> 
                RPC_X_BAD_STUB_DATA = 1783,


                #endregion



                //#region API Error Codes

                ///<summary> The supplied user buffer is not valid for the requested operation.</summary>
                ERROR_INVALID_USER_BUFFER = 1784,
                ///<summary> The disk media is not recognized. It may not be formatted.</summary>
                ERROR_UNRECOGNIZED_MEDIA = 1785,
                ///<summary> The workstation does not have a trust secret.</summary>
                ERROR_NO_TRUST_LSA_SECRET = 1786,
                ///<summary> The security database on the server does not have a computer account for this workstation trust relationship.</summary>
                ERROR_NO_TRUST_SAM_ACCOUNT = 1787,
                ///<summary> The trust relationship between the primary domain and the trusted domain failed.</summary>
                ERROR_TRUSTED_DOMAIN_FAILURE = 1788,
                ///<summary> The trust relationship between this workstation and the primary domain failed.</summary>
                ERROR_TRUSTED_RELATIONSHIP_FAILURE = 1789,
                ///<summary> The network logon failed.</summary>
                ERROR_TRUST_FAILURE = 1790,
                ///<summary> A remote procedure call is already in progress for this thread.</summary>
                RPC_S_CALL_IN_PROGRESS = 1791,
                ///<summary> An attempt was made to logon, but the network logon service was not started.</summary>
                ERROR_NETLOGON_NOT_STARTED = 1792,
                ///<summary> The user's account has expired.</summary>
                ERROR_ACCOUNT_EXPIRED = 1793,
                ///<summary> The redirector is in use and cannot be unloaded.</summary>
                ERROR_REDIRECTOR_HAS_OPEN_HANDLES = 1794,
                ///<summary> The specified printer driver is already installed.</summary>
                ERROR_PRINTER_DRIVER_ALREADY_INSTALLED = 1795,
                ///<summary> The specified port is unknown.</summary>
                ERROR_UNKNOWN_PORT = 1796,
                ///<summary> The printer driver is unknown.</summary>
                ERROR_UNKNOWN_PRINTER_DRIVER = 1797,
                ///<summary> The print processor is unknown.</summary>
                ERROR_UNKNOWN_PRINTPROCESSOR = 1798,
                ///<summary> The specified separator file is invalid.</summary>
                ERROR_INVALID_SEPARATOR_FILE = 1799,
                ///<summary> The specified priority is invalid.</summary>
                ERROR_INVALID_PRIORITY = 1800,
                ///<summary> The printer name is invalid.</summary>
                ERROR_INVALID_PRINTER_NAME = 1801,
                ///<summary> The printer already exists.</summary>
                ERROR_PRINTER_ALREADY_EXISTS = 1802,
                ///<summary> The printer command is invalid.</summary>
                ERROR_INVALID_PRINTER_COMMAND = 1803,
                ///<summary> The specified datatype is invalid.</summary>
                ERROR_INVALID_DATATYPE = 1804,
                ///<summary> The environment specified is invalid.</summary>
                ERROR_INVALID_ENVIRONMENT = 1805,
                ///<summary> There are no more bindings.</summary>
                RPC_S_NO_MORE_BINDINGS = 1806,
                ///<summary> The account used is an interdomain trust account. Use your global user account or local user account to access this server.</summary>
                ERROR_NOLOGON_INTERDOMAIN_TRUST_ACCOUNT = 1807,
                ///<summary> The account used is a computer account. Use your global user account or local user account to access this server.</summary>
                ERROR_NOLOGON_WORKSTATION_TRUST_ACCOUNT = 1808,
                ///<summary> The account used is a server trust account. Use your global user account or local user account to access this server.</summary>
                ERROR_NOLOGON_SERVER_TRUST_ACCOUNT = 1809,
                ///<summary> The name or security ID (SID) of the domain specified is inconsistent with the trust information for that domain.</summary>
                ERROR_DOMAIN_TRUST_INCONSISTENT = 1810,
                ///<summary> The server is in use and cannot be unloaded.</summary>
                ERROR_SERVER_HAS_OPEN_HANDLES = 1811,
                ///<summary> The specified image file did not contain a resource section.</summary>
                ERROR_RESOURCE_DATA_NOT_FOUND = 1812,
                ///<summary> The specified resource type cannot be found in the image file.</summary>
                ERROR_RESOURCE_TYPE_NOT_FOUND = 1813,
                ///<summary> The specified resource name cannot be found in the image file.</summary>
                ERROR_RESOURCE_NAME_NOT_FOUND = 1814,
                ///<summary> The specified resource language ID cannot be found in the image file.</summary>
                ERROR_RESOURCE_LANG_NOT_FOUND = 1815,
                ///<summary> Not enough quota is available to process this command.</summary>
                ERROR_NOT_ENOUGH_QUOTA = 1816,
                ///<summary> No interfaces have been registered.</summary>
                RPC_S_NO_INTERFACES = 1817,
                ///<summary> The remote procedure call was cancelled.</summary>
                RPC_S_CALL_CANCELLED = 1818,
                ///<summary> The binding handle does not contain all required information.</summary>
                RPC_S_BINDING_INCOMPLETE = 1819,
                ///<summary> A communications failure occurred during a remote procedure call.</summary>
                RPC_S_COMM_FAILURE = 1820,
                ///<summary> The requested authentication level is not supported.</summary>
                RPC_S_UNSUPPORTED_AUTHN_LEVEL = 1821,
                ///<summary> No principal name registered.</summary>
                RPC_S_NO_PRINC_NAME = 1822,
                ///<summary> The error specified is not a valid Windows RPC error code.</summary>
                RPC_S_NOT_RPC_ERROR = 1823,
                ///<summary> A UUID that is valid only on this computer has been allocated.</summary>
                RPC_S_UUID_LOCAL_ONLY = 1824,
                ///<summary> A security package specific error occurred.</summary>
                RPC_S_SEC_PKG_ERROR = 1825,
                ///<summary> Thread is not canceled.</summary>
                RPC_S_NOT_CANCELLED = 1826,
                ///<summary> Invalid operation on the encoding/decoding handle.</summary>
                RPC_X_INVALID_ES_ACTION = 1827,
                ///<summary> Incompatible version of the serializing package.</summary>
                RPC_X_WRONG_ES_VERSION = 1828,
                ///<summary> Incompatible version of the RPC stub.</summary>
                RPC_X_WRONG_STUB_VERSION = 1829,
                ///<summary> The RPC pipe object is invalid or corrupted.</summary>
                RPC_X_INVALID_PIPE_OBJECT = 1830,
                ///<summary> An invalid operation was attempted on an RPC pipe object.</summary>
                RPC_X_WRONG_PIPE_ORDER = 1831,
                ///<summary> Unsupported RPC pipe version.</summary>
                RPC_X_WRONG_PIPE_VERSION = 1832,
                ///<summary> The group member was not found.</summary>
                RPC_S_GROUP_MEMBER_NOT_FOUND = 1898,
                ///<summary> The endpoint mapper database entry could not be created.</summary>
                EPT_S_CANT_CREATE = 1899,
                ///<summary> The object universal unique identifier (UUID) is the nil UUID.</summary>
                RPC_S_INVALID_OBJECT = 1900,
                ///<summary> The specified time is invalid.</summary>
                ERROR_INVALID_TIME = 1901,
                ///<summary> The specified form name is invalid.</summary>
                ERROR_INVALID_FORM_NAME = 1902,
                ///<summary> The specified form size is invalid.</summary>
                ERROR_INVALID_FORM_SIZE = 1903,
                ///<summary> The specified printer handle is already being waited on</summary>
                ERROR_ALREADY_WAITING = 1904,
                ///<summary> The specified printer has been deleted.</summary>
                ERROR_PRINTER_DELETED = 1905,
                ///<summary> The state of the printer is invalid.</summary>
                ERROR_INVALID_PRINTER_STATE = 1906,
                ///<summary> The user's password must be changed before logging on the first time.</summary>
                ERROR_PASSWORD_MUST_CHANGE = 1907,
                ///<summary> Could not find the domain controller for this domain.</summary>
                ERROR_DOMAIN_CONTROLLER_NOT_FOUND = 1908,
                ///<summary> The referenced account is currently locked [Out ] and may not be logged on to.</summary>
                ERROR_ACCOUNT_LOCKED_OUT = 1909,
                ///<summary> The object exporter specified was not found.</summary>
                OR_INVALID_OXID = 1910,
                ///<summary> The object specified was not found.</summary>
                OR_INVALID_OID = 1911,
                ///<summary> The object resolver set specified was not found.</summary>
                OR_INVALID_SET = 1912,
                ///<summary> Some data remains to be sent in the request buffer.</summary>
                RPC_S_SEND_INCOMPLETE = 1913,
                ///<summary> Invalid asynchronous remote procedure call handle.</summary>
                RPC_S_INVALID_ASYNC_HANDLE = 1914,
                ///<summary> Invalid asynchronous RPC call handle for this operation.</summary>
                RPC_S_INVALID_ASYNC_CALL = 1915,
                ///<summary> The RPC pipe object has already been closed.</summary>
                RPC_X_PIPE_CLOSED = 1916,
                ///<summary> The RPC call completed before all pipes were processed.</summary>
                RPC_X_PIPE_DISCIPLINE_ERROR = 1917,
                ///<summary> No more data is available from the RPC pipe.</summary>
                RPC_X_PIPE_EMPTY = 1918,
                ///<summary> No site name is available for this machine.</summary>
                ERROR_NO_SITENAME = 1919,
                ///<summary> The file can not be accessed by the system.</summary>
                ERROR_CANT_ACCESS_FILE = 1920,
                ///<summary> The name of the file cannot be resolved by the system.</summary>
                ERROR_CANT_RESOLVE_FILENAME = 1921,
                ///<summary> The entry is not of the expected type.</summary>
                RPC_S_ENTRY_TYPE_MISMATCH = 1922,
                ///<summary> Not all object UUIDs could be exported to the specified entry.</summary>
                RPC_S_NOT_ALL_OBJS_EXPORTED = 1923,
                ///<summary> Interface could not be exported to the specified entry.</summary>
                RPC_S_INTERFACE_NOT_EXPORTED = 1924,
                ///<summary> The specified profile entry could not be added.</summary>
                RPC_S_PROFILE_NOT_ADDED = 1925,
                ///<summary> The specified profile element could not be added.</summary>
                RPC_S_PRF_ELT_NOT_ADDED = 1926,
                ///<summary> The specified profile element could not be removed.</summary>
                RPC_S_PRF_ELT_NOT_REMOVED = 1927,
                ///<summary> The group element could not be added.</summary>
                RPC_S_GRP_ELT_NOT_ADDED = 1928,
                ///<summary> The group element could not be removed.</summary>
                RPC_S_GRP_ELT_NOT_REMOVED = 1929,
                ///<summary> The printer driver is not compatible with a policy enabled on your computer that blocks NT 4.0 drivers.</summary>
                ERROR_KM_DRIVER_BLOCKED = 1930,
                ///<summary> The context has expired and can no longer be used.</summary>
                ERROR_CONTEXT_EXPIRED = 1931,
                ///<summary> The current user's delegated trust creation quota has been exceeded.</summary>
                ERROR_PER_USER_TRUST_QUOTA_EXCEEDED = 1932,
                ///<summary> The total delegated trust creation quota has been exceeded.</summary>
                ERROR_ALL_USER_TRUST_QUOTA_EXCEEDED = 1933,
                ///<summary> The current user's delegated trust deletion quota has been exceeded.</summary>
                ERROR_USER_DELETE_TRUST_QUOTA_EXCEEDED = 1934,
                ///<summary> Logon Failure: The machine you are logging onto is protected by an authentication firewall.  The specified account is not allowed to authenticate to the machine.</summary>
                ERROR_AUTHENTICATION_FIREWALL_FAILED = 1935,
                ///<summary> Remote connections to the Print Spooler are blocked by a policy set on your machine.</summary>
                ERROR_REMOTE_PRINT_CONNECTIONS_BLOCKED = 1936,


                #region OpenGL Error Code

                ///<summary> The pixel format is invalid.</summary>
                ERROR_INVALID_PIXEL_FORMAT = 2000,
                ///<summary> The specified driver is invalid.</summary>
                ERROR_BAD_DRIVER = 2001,
                ///<summary> The window style or class attribute is invalid for this operation.</summary>
                ERROR_INVALID_WINDOW_STYLE = 2002,
                ///<summary> The requested metafile operation is not supported.</summary>
                ERROR_METAFILE_NOT_SUPPORTED = 2003,
                ///<summary> The requested transformation operation is not supported.</summary>
                ERROR_TRANSFORM_NOT_SUPPORTED = 2004,
                ///<summary> The requested clipping operation is not supported.</summary>
                ERROR_CLIPPING_NOT_SUPPORTED = 2005,

                #endregion


                #region Image Color Management Error Code"
                ///<summary> The specified color management module is invalid.</summary>
                ERROR_INVALID_CMM = 2010,
                ///<summary> The specified color profile is invalid.</summary>
                ERROR_INVALID_PROFILE = 2011,
                ///<summary> The specified tag was not found.</summary>
                ERROR_TAG_NOT_FOUND = 2012,
                ///<summary> A required tag is not present.</summary>
                ERROR_TAG_NOT_PRESENT = 2013,
                ///<summary> The specified tag is already present.</summary>
                ERROR_DUPLICATE_TAG = 2014,
                ///<summary> The specified color profile is not associated with any device.</summary>
                ERROR_PROFILE_NOT_ASSOCIATED_WITH_DEVICE = 2015,
                ///<summary> The specified color profile was not found.</summary>
                ERROR_PROFILE_NOT_FOUND = 2016,
                ///<summary> The specified color space is invalid.</summary>
                ERROR_INVALID_COLORSPACE = 2017,
                ///<summary> Image Color Management is not enabled.</summary>
                ERROR_ICM_NOT_ENABLED = 2018,
                ///<summary> There was an error while deleting the color transform.</summary>
                ERROR_DELETING_ICM_XFORM = 2019,
                ///<summary> The specified color transform is invalid.</summary>
                ERROR_INVALID_TRANSFORM = 2020,
                ///<summary> The specified transform does not match the bitmap's color space.</summary>
                ERROR_COLORSPACE_MISMATCH = 2021,
                ///<summary> The specified named color index is not present in the profile.</summary>
                ERROR_INVALID_COLORINDEX = 2022,
                #endregion


                #region Winnet32 Status Codes. (The range 2100 through 2999 is reserved for network status codes. See lmerr.h for a complete listing)

                ///<summary> The network connection was made successfully, but the user had to be prompted for a password other than the one originally specified.</summary>
                ERROR_CONNECTED_OTHER_PASSWORD = 2108,
                ///<summary> The network connection was made successfully using default credentials.</summary>
                ERROR_CONNECTED_OTHER_PASSWORD_DEFAULT = 2109,
                ///<summary> The specified username is invalid.</summary>
                ERROR_BAD_USERNAME = 2202,
                ///<summary> This network connection does not exist.</summary>
                ERROR_NOT_CONNECTED = 2250,
                ///<summary> This network connection has files open or requests pending.</summary>
                ERROR_OPEN_FILES = 2401,
                ///<summary> Active connections still exist.</summary>
                ERROR_ACTIVE_CONNECTIONS = 2402,
                ///<summary> The device is in use by an active process and cannot be disconnected.</summary>
                ERROR_DEVICE_IN_USE = 2404,
                #endregion


                #region Win32 Spooler Error Codes

                ///<summary> The specified print monitor is unknown.</summary>
                ERROR_UNKNOWN_PRINT_MONITOR = 3000,
                ///<summary> The specified printer driver is currently in use.</summary>
                ERROR_PRINTER_DRIVER_IN_USE = 3001,
                ///<summary> The spool file was not found.</summary>
                ERROR_SPOOL_FILE_NOT_FOUND = 3002,
                ///<summary> A StartDocPrinter call was not issued.</summary>
                ERROR_SPL_NO_STARTDOC = 3003,
                ///<summary> An AddJob call was not issued.</summary>
                ERROR_SPL_NO_ADDJOB = 3004,
                ///<summary> The specified print processor has already been installed.</summary>
                ERROR_PRINT_PROCESSOR_ALREADY_INSTALLED = 3005,
                ///<summary> The specified print monitor has already been installed.</summary>
                ERROR_PRINT_MONITOR_ALREADY_INSTALLED = 3006,
                ///<summary> The specified print monitor does not have the required functions.</summary>
                ERROR_INVALID_PRINT_MONITOR = 3007,
                ///<summary> The specified print monitor is currently in use.</summary>
                ERROR_PRINT_MONITOR_IN_USE = 3008,
                ///<summary> The requested operation is not allowed when there are jobs queued to the printer.</summary>
                ERROR_PRINTER_HAS_JOBS_QUEUED = 3009,
                ///<summary> The requested operation is successful. Changes will not be effective until the system is rebooted.</summary>
                ERROR_SUCCESS_REBOOT_REQUIRED = 3010,
                ///<summary> The requested operation is successful. Changes will not be effective until the service is restarted.</summary>
                ERROR_SUCCESS_RESTART_REQUIRED = 3011,
                ///<summary> No printers were found.</summary>
                ERROR_PRINTER_NOT_FOUND = 3012,
                ///<summary> The printer driver is known to be unreliable.</summary>
                ERROR_PRINTER_DRIVER_WARNED = 3013,
                ///<summary> The printer driver is known to harm the system.</summary>
                ERROR_PRINTER_DRIVER_BLOCKED = 3014,

                #endregion


                #region Wins Error Codes 

                ///<summary> WINS encountered an error while processing the command.</summary>
                ERROR_WINS_INTERNAL = 4000,
                ///<summary> The local WINS can not be deleted.</summary>
                ERROR_CAN_NOT_DEL_LOCAL_WINS = 4001,
                ///<summary> The importation from the file failed.</summary>
                ERROR_STATIC_INIT = 4002,
                ///<summary> The backup failed. Was a full backup done before?</summary>
                ERROR_INC_BACKUP = 4003,
                ///<summary> The backup failed. Check the directory to which you are backing the database.</summary>
                ERROR_FULL_BACKUP = 4004,
                ///<summary> The name does not exist in the WINS database.</summary>
                ERROR_REC_NON_EXISTENT = 4005,
                ///<summary> Replication with a nonconfigured partner is not allowed.</summary>
                ERROR_RPL_NOT_ALLOWED = 4006,

                #endregion


                #region DHCP Error Codes

                ///<summary> The DHCP client has obtained an IP address that is already in use on the network. The local interface will be disabled until the DHCP client can obtain a new address.</summary>
                ERROR_DHCP_ADDRESS_CONFLICT = 4100,

                #endregion


                #region WMI Error Codes

                ///<summary> The GUID passed was not recognized as valid by a WMI data provider.</summary>
                ERROR_WMI_GUID_NOT_FOUND = 4200,
                ///<summary> The instance name passed was not recognized as valid by a WMI data provider.</summary>
                ERROR_WMI_INSTANCE_NOT_FOUND = 4201,
                ///<summary> The data item ID passed was not recognized as valid by a WMI data provider.</summary>
                ERROR_WMI_ITEMID_NOT_FOUND = 4202,
                ///<summary> The WMI request could not be completed and should be retried.</summary>
                ERROR_WMI_TRY_AGAIN = 4203,
                ///<summary> The WMI data provider could not be located.</summary>
                ERROR_WMI_DP_NOT_FOUND = 4204,
                ///<summary> The WMI data provider references an instance set that has not been registered.</summary>
                ERROR_WMI_UNRESOLVED_INSTANCE_REF = 4205,
                ///<summary> The WMI data block or event notification has already been enabled.</summary>
                ERROR_WMI_ALREADY_ENABLED = 4206,
                ///<summary> The WMI data block is no longer available.</summary>
                ERROR_WMI_GUID_DISCONNECTED = 4207,
                ///<summary> The WMI data service is not available.</summary>
                ERROR_WMI_SERVER_UNAVAILABLE = 4208,
                ///<summary> The WMI data provider failed to carry [Out ] the request.</summary>
                ERROR_WMI_DP_FAILED = 4209,
                ///<summary> The WMI MOF information is not valid.</summary>
                ERROR_WMI_INVALID_MOF = 4210,
                ///<summary> The WMI registration information is not valid.</summary>
                ERROR_WMI_INVALID_REGINFO = 4211,
                ///<summary> The WMI data block or event notification has already been disabled.</summary>
                ERROR_WMI_ALREADY_DISABLED = 4212,
                ///<summary> The WMI data item or data block is read only.</summary>
                ERROR_WMI_READ_ONLY = 4213,
                ///<summary> The WMI data item or data block could not be changed.</summary>
                ERROR_WMI_SET_FAILURE = 4214,
                #endregion


                #region NT Media Services (RSM) Error Codes

                ///<summary> The media identifier does not represent a valid medium.</summary>
                ERROR_INVALID_MEDIA = 4300,
                ///<summary> The library identifier does not represent a valid library.</summary>
                ERROR_INVALID_LIBRARY = 4301,
                ///<summary> The media pool identifier does not represent a valid media pool.</summary>
                ERROR_INVALID_MEDIA_POOL = 4302,
                ///<summary> The drive and medium are not compatible or exist in different libraries.</summary>
                ERROR_DRIVE_MEDIA_MISMATCH = 4303,
                ///<summary> The medium currently exists in an offline library and must be online to perform this operation.</summary>
                ERROR_MEDIA_OFFLINE = 4304,
                ///<summary> The operation cannot be performed on an offline library.</summary>
                ERROR_LIBRARY_OFFLINE = 4305,
                ///<summary> The library, drive, or media pool is empty.</summary>
                ERROR_EMPTY = 4306,
                ///<summary> The library, drive, or media pool must be empty to perform this operation.</summary>
                ERROR_NOT_EMPTY = 4307,
                ///<summary> No media is currently available in this media pool or library.</summary>
                ERROR_MEDIA_UNAVAILABLE = 4308,
                ///<summary> A resource required for this operation is disabled.</summary>
                ERROR_RESOURCE_DISABLED = 4309,
                ///<summary> The media identifier does not represent a valid cleaner.</summary>
                ERROR_INVALID_CLEANER = 4310,
                ///<summary> The drive cannot be cleaned or does not support cleaning.</summary>
                ERROR_UNABLE_TO_CLEAN = 4311,
                ///<summary> The object identifier does not represent a valid object.</summary>
                ERROR_OBJECT_NOT_FOUND = 4312,
                ///<summary> Unable to read from or write to the database.</summary>
                ERROR_DATABASE_FAILURE = 4313,
                ///<summary> The database is full.</summary>
                ERROR_DATABASE_FULL = 4314,
                ///<summary> The medium is not compatible with the device or media pool.</summary>
                ERROR_MEDIA_INCOMPATIBLE = 4315,
                ///<summary> The resource required for this operation does not exist.</summary>
                ERROR_RESOURCE_NOT_PRESENT = 4316,
                ///<summary> The operation identifier is not valid.</summary>
                ERROR_INVALID_OPERATION = 4317,
                ///<summary> The media is not mounted or ready for use.</summary>
                ERROR_MEDIA_NOT_AVAILABLE = 4318,
                ///<summary> The device is not ready for use.</summary>
                ERROR_DEVICE_NOT_AVAILABLE = 4319,
                ///<summary> The operator or administrator has refused the request.</summary>
                ERROR_REQUEST_REFUSED = 4320,
                ///<summary> The drive identifier does not represent a valid drive.</summary>
                ERROR_INVALID_DRIVE_OBJECT = 4321,
                ///<summary> Library is full.  No slot is available for use.</summary>
                ERROR_LIBRARY_FULL = 4322,
                ///<summary> The transport cannot access the medium.</summary>
                ERROR_MEDIUM_NOT_ACCESSIBLE = 4323,
                ///<summary> Unable to load the medium into the drive.</summary>
                ERROR_UNABLE_TO_LOAD_MEDIUM = 4324,
                ///<summary> Unable to retrieve the drive status.</summary>
                ERROR_UNABLE_TO_INVENTORY_DRIVE = 4325,
                ///<summary> Unable to retrieve the slot status.</summary>
                ERROR_UNABLE_TO_INVENTORY_SLOT = 4326,
                ///<summary> Unable to retrieve status about the transport.</summary>
                ERROR_UNABLE_TO_INVENTORY_TRANSPORT = 4327,
                ///<summary> Cannot use the transport because it is already in use.</summary>
                ERROR_TRANSPORT_FULL = 4328,
                ///<summary> Unable to open or close the inject/eject port.</summary>
                ERROR_CONTROLLING_IEPORT = 4329,
                ///<summary> Unable to eject the medium because it is in a drive.</summary>
                ERROR_UNABLE_TO_EJECT_MOUNTED_MEDIA = 4330,
                ///<summary> A cleaner slot is already reserved.</summary>
                ERROR_CLEANER_SLOT_SET = 4331,
                ///<summary> A cleaner slot is not reserved.</summary>
                ERROR_CLEANER_SLOT_NOT_SET = 4332,
                ///<summary> The cleaner cartridge has performed the maximum number of drive cleanings.</summary>
                ERROR_CLEANER_CARTRIDGE_SPENT = 4333,
                ///<summary> Unexpected on-medium identifier.</summary>
                ERROR_UNEXPECTED_OMID = 4334,
                ///<summary> The last remaining item in this group or resource cannot be deleted.</summary>
                ERROR_CANT_DELETE_LAST_ITEM = 4335,
                ///<summary> The message provided exceeds the maximum size allowed for this parameter.</summary>
                ERROR_MESSAGE_EXCEEDS_MAX_SIZE = 4336,
                ///<summary> The volume contains system or paging files.</summary>
                ERROR_VOLUME_CONTAINS_SYS_FILES = 4337,
                ///<summary> The media type cannot be removed from this library since at least one drive in the library reports it can support this media type.</summary>
                ERROR_INDIGENOUS_TYPE = 4338,
                ///<summary> This offline media cannot be mounted on this system since no enabled drives are present which can be used.</summary>
                ERROR_NO_SUPPORTING_DRIVES = 4339,
                ///<summary> A cleaner cartridge is present in the tape library.</summary>
                ERROR_CLEANER_CARTRIDGE_INSTALLED = 4340,

                #endregion


                #region NT Remote Storage Service Error Codes  

                ///<summary> The remote storage service was not able to recall the file.</summary>
                ERROR_FILE_OFFLINE = 4350,
                ///<summary> The remote storage service is not operational at this time.</summary>
                ERROR_REMOTE_STORAGE_NOT_ACTIVE = 4351,
                ///<summary> The remote storage service encountered a media error.</summary>
                ERROR_REMOTE_STORAGE_MEDIA_ERROR = 4352,

                #endregion


                #region NT Reparse Points Error Codes

                ///<summary> The file or directory is not a reparse point.</summary>
                ERROR_NOT_A_REPARSE_POINT = 4390,
                ///<summary> The reparse point attribute cannot be set because it conflicts with an existing attribute.</summary>
                ERROR_REPARSE_ATTRIBUTE_CONFLICT = 4391,
                ///<summary> The data present in the reparse point buffer is invalid.</summary>
                ERROR_INVALID_REPARSE_DATA = 4392,
                ///<summary> The tag present in the reparse point buffer is invalid.</summary>
                ERROR_REPARSE_TAG_INVALID = 4393,
                ///<summary> There is a mismatch between the tag specified in the request and the tag present in the reparse point.</summary>
                ERROR_REPARSE_TAG_MISMATCH = 4394,

                #endregion


                #region NT Single Instance Store Error Codes

                ///<summary> Single Instance Storage is not available on this volume.</summary>
                ERROR_VOLUME_NOT_SIS_ENABLED = 4500,

                #endregion


                #region Cluster Error Codes

                ///<summary> The cluster resource cannot be moved to another group because other resources are dependent on it.</summary>
                ERROR_DEPENDENT_RESOURCE_EXISTS = 5001,
                ///<summary> The cluster resource dependency cannot be found.</summary>
                ERROR_DEPENDENCY_NOT_FOUND = 5002,
                ///<summary> The cluster resource cannot be made dependent on the specified resource because it is already dependent.</summary>
                ERROR_DEPENDENCY_ALREADY_EXISTS = 5003,
                ///<summary> The cluster resource is not online.</summary>
                ERROR_RESOURCE_NOT_ONLINE = 5004,
                ///<summary> A cluster node is not available for this operation.</summary>
                ERROR_HOST_NODE_NOT_AVAILABLE = 5005,
                ///<summary> The cluster resource is not available.</summary>
                ERROR_RESOURCE_NOT_AVAILABLE = 5006,
                ///<summary> The cluster resource could not be found.</summary>
                ERROR_RESOURCE_NOT_FOUND = 5007,
                ///<summary> The cluster is being shut down.</summary>
                ERROR_SHUTDOWN_CLUSTER = 5008,
                ///<summary> A cluster node cannot be evicted from the cluster unless the node is down or it is the last node.</summary>
                ERROR_CANT_EVICT_ACTIVE_NODE = 5009,
                ///<summary> The object already exists.</summary>
                ERROR_OBJECT_ALREADY_EXISTS = 5010,
                ///<summary> The object is already in the list.</summary>
                ERROR_OBJECT_IN_LIST = 5011,
                ///<summary> The cluster group is not available for any new requests.</summary>
                ERROR_GROUP_NOT_AVAILABLE = 5012,
                ///<summary> The cluster group could not be found.</summary>
                ERROR_GROUP_NOT_FOUND = 5013,
                ///<summary> The operation could not be completed because the cluster group is not online.</summary>
                ERROR_GROUP_NOT_ONLINE = 5014,
                ///<summary> The cluster node is not the owner of the resource.</summary>
                ERROR_HOST_NODE_NOT_RESOURCE_OWNER = 5015,
                ///<summary> The cluster node is not the owner of the group.</summary>
                ERROR_HOST_NODE_NOT_GROUP_OWNER = 5016,
                ///<summary> The cluster resource could not be created in the specified resource monitor.</summary>
                ERROR_RESMON_CREATE_FAILED = 5017,
                ///<summary> The cluster resource could not be brought online by the resource monitor.</summary>
                ERROR_RESMON_ONLINE_FAILED = 5018,
                ///<summary> The operation could not be completed because the cluster resource is online.</summary>
                ERROR_RESOURCE_ONLINE = 5019,
                ///<summary> The cluster resource could not be deleted or brought offline because it is the quorum resource.</summary>
                ERROR_QUORUM_RESOURCE = 5020,
                ///<summary> The cluster could not make the specified resource a quorum resource because it is not capable of being a quorum resource.</summary>
                ERROR_NOT_QUORUM_CAPABLE = 5021,
                ///<summary> The cluster software is shutting down.</summary>
                ERROR_CLUSTER_SHUTTING_DOWN = 5022,
                ///<summary> The group or resource is not in the correct state to perform the requested operation.</summary>
                ERROR_INVALID_STATE = 5023,
                ///<summary> The properties were stored but not all changes will take effect until the next time the resource is brought online.</summary>
                ERROR_RESOURCE_PROPERTIES_STORED = 5024,
                ///<summary> The cluster could not make the specified resource a quorum resource because it does not belong to a shared storage class.</summary>
                ERROR_NOT_QUORUM_CLASS = 5025,
                ///<summary> The cluster resource could not be deleted since it is a core resource.</summary>
                ERROR_CORE_RESOURCE = 5026,
                ///<summary> The quorum resource failed to come online.</summary>
                ERROR_QUORUM_RESOURCE_ONLINE_FAILED = 5027,
                ///<summary> The quorum log could not be created or mounted successfully.</summary>
                ERROR_QUORUMLOG_OPEN_FAILED = 5028,
                ///<summary> The cluster log is corrupt.</summary>
                ERROR_CLUSTERLOG_CORRUPT = 5029,
                ///<summary> The record could not be written to the cluster log since it exceeds the maximum size.</summary>
                ERROR_CLUSTERLOG_RECORD_EXCEEDS_MAXSIZE = 5030,
                ///<summary> The cluster log exceeds its maximum size.</summary>
                ERROR_CLUSTERLOG_EXCEEDS_MAXSIZE = 5031,
                ///<summary> No checkpoint record was found in the cluster log.</summary>
                ERROR_CLUSTERLOG_CHKPOINT_NOT_FOUND = 5032,
                ///<summary> The minimum required disk space needed for logging is not available.</summary>
                ERROR_CLUSTERLOG_NOT_ENOUGH_SPACE = 5033,
                ///<summary> The cluster node failed to take control of the quorum resource because the resource is owned by another active node.</summary>
                ERROR_QUORUM_OWNER_ALIVE = 5034,
                ///<summary> A cluster network is not available for this operation.</summary>
                ERROR_NETWORK_NOT_AVAILABLE = 5035,
                ///<summary> A cluster node is not available for this operation.</summary>
                ERROR_NODE_NOT_AVAILABLE = 5036,
                ///<summary> All cluster nodes must be running to perform this operation.</summary>
                ERROR_ALL_NODES_NOT_AVAILABLE = 5037,
                ///<summary> A cluster resource failed.</summary>
                ERROR_RESOURCE_FAILED = 5038,
                ///<summary> The cluster node is not valid.</summary>
                ERROR_CLUSTER_INVALID_NODE = 5039,
                ///<summary> The cluster node already exists.</summary>
                ERROR_CLUSTER_NODE_EXISTS = 5040,
                ///<summary> A node is in the process of joining the cluster.</summary>
                ERROR_CLUSTER_JOIN_IN_PROGRESS = 5041,
                ///<summary> The cluster node was not found.</summary>
                ERROR_CLUSTER_NODE_NOT_FOUND = 5042,
                ///<summary> The cluster local node information was not found.</summary>
                ERROR_CLUSTER_LOCAL_NODE_NOT_FOUND = 5043,
                ///<summary> The cluster network already exists.</summary>
                ERROR_CLUSTER_NETWORK_EXISTS = 5044,
                ///<summary> The cluster network was not found.</summary>
                ERROR_CLUSTER_NETWORK_NOT_FOUND = 5045,
                ///<summary> The cluster network interface already exists.</summary>
                ERROR_CLUSTER_NETINTERFACE_EXISTS = 5046,
                ///<summary> The cluster network interface was not found.</summary>
                ERROR_CLUSTER_NETINTERFACE_NOT_FOUND = 5047,
                ///<summary> The cluster request is not valid for this object.</summary>
                ERROR_CLUSTER_INVALID_REQUEST = 5048,
                ///<summary> The cluster network provider is not valid.</summary>
                ERROR_CLUSTER_INVALID_NETWORK_PROVIDER = 5049,
                ///<summary> The cluster node is down.</summary>
                ERROR_CLUSTER_NODE_DOWN = 5050,
                ///<summary> The cluster node is not reachable.</summary>
                ERROR_CLUSTER_NODE_UNREACHABLE = 5051,
                ///<summary> The cluster node is not a member of the cluster.</summary>
                ERROR_CLUSTER_NODE_NOT_MEMBER = 5052,
                ///<summary> A cluster join operation is not in progress.</summary>
                ERROR_CLUSTER_JOIN_NOT_IN_PROGRESS = 5053,
                ///<summary> The cluster network is not valid.</summary>
                ERROR_CLUSTER_INVALID_NETWORK = 5054,
                ///<summary> The cluster node is up.</summary>
                ERROR_CLUSTER_NODE_UP = 5056,
                ///<summary> The cluster IP address is already in use.</summary>
                ERROR_CLUSTER_IPADDR_IN_USE = 5057,
                ///<summary> The cluster node is not paused.</summary>
                ERROR_CLUSTER_NODE_NOT_PAUSED = 5058,
                ///<summary> No cluster security context is available.</summary>
                ERROR_CLUSTER_NO_SECURITY_CONTEXT = 5059,
                ///<summary> The cluster network is not configured for internal cluster communication.</summary>
                ERROR_CLUSTER_NETWORK_NOT_INTERNAL = 5060,
                ///<summary> The cluster node is already up.</summary>
                ERROR_CLUSTER_NODE_ALREADY_UP = 5061,
                ///<summary> The cluster node is already down.</summary>
                ERROR_CLUSTER_NODE_ALREADY_DOWN = 5062,
                ///<summary> The cluster network is already online.</summary>
                ERROR_CLUSTER_NETWORK_ALREADY_ONLINE = 5063,
                ///<summary> The cluster network is already offline.</summary>
                ERROR_CLUSTER_NETWORK_ALREADY_OFFLINE = 5064,
                ///<summary> The cluster node is already a member of the cluster.</summary>
                ERROR_CLUSTER_NODE_ALREADY_MEMBER = 5065,
                ///<summary> The cluster network is the only one configured for internal cluster communication between two or more active cluster nodes. The internal communication capability cannot be removed from the network.</summary>
                ERROR_CLUSTER_LAST_INTERNAL_NETWORK = 5066,
                ///<summary> One or more cluster resources depend on the network to provide service to clients. The client access capability cannot be removed from the network.</summary>
                ERROR_CLUSTER_NETWORK_HAS_DEPENDENTS = 5067,
                ///<summary> This operation cannot be performed on the cluster resource as it the quorum resource. You may not bring the quorum resource offline or modify its possible owners list.</summary>
                ERROR_INVALID_OPERATION_ON_QUORUM = 5068,
                ///<summary> The cluster quorum resource is not allowed to have any dependencies.</summary>
                ERROR_DEPENDENCY_NOT_ALLOWED = 5069,
                ///<summary> The cluster node is paused.</summary>
                ERROR_CLUSTER_NODE_PAUSED = 5070,
                ///<summary> The cluster resource cannot be brought online. The owner node cannot run this resource.</summary>
                ERROR_NODE_CANT_HOST_RESOURCE = 5071,
                ///<summary> The cluster node is not ready to perform the requested operation.</summary>
                ERROR_CLUSTER_NODE_NOT_READY = 5072,
                ///<summary> The cluster node is shutting down.</summary>
                ERROR_CLUSTER_NODE_SHUTTING_DOWN = 5073,
                ///<summary> The cluster join operation was aborted.</summary>
                ERROR_CLUSTER_JOIN_ABORTED = 5074,
                ///<summary> The cluster join operation failed due to incompatible software versions between the joining node and its sponsor.</summary>
                ERROR_CLUSTER_INCOMPATIBLE_VERSIONS = 5075,
                ///<summary> This resource cannot be created because the cluster has reached the limit on the number of resources it can monitor.</summary>
                ERROR_CLUSTER_MAXNUM_OF_RESOURCES_EXCEEDED = 5076,
                ///<summary> The system configuration changed during the cluster join or form operation. The join or form operation was aborted.</summary>
                ERROR_CLUSTER_SYSTEM_CONFIG_CHANGED = 5077,
                ///<summary> The specified resource type was not found.</summary>
                ERROR_CLUSTER_RESOURCE_TYPE_NOT_FOUND = 5078,
                ///<summary> The specified node does not support a resource of this type.  This may be due to version inconsistencies or due to the absence of the resource DLL on this node.</summary>
                ERROR_CLUSTER_RESTYPE_NOT_SUPPORTED = 5079,
                ///<summary> The specified resource name is not supported by this resource DLL. This may be due to a bad (or changed) name supplied to the resource DLL.</summary>
                ERROR_CLUSTER_RESNAME_NOT_FOUND = 5080,
                ///<summary> No authentication package could be registered with the RPC server.</summary>
                ERROR_CLUSTER_NO_RPC_PACKAGES_REGISTERED = 5081,
                ///<summary> You cannot bring the group online because the owner of the group is not in the preferred list for the group. To change the owner node for the group, move the group.</summary>
                ERROR_CLUSTER_OWNER_NOT_IN_PREFLIST = 5082,
                ///<summary> The join operation failed because the cluster database sequence number has changed or is incompatible with the locker node. This may happen during a join operation if the cluster database was changing during the join.</summary>
                ERROR_CLUSTER_DATABASE_SEQMISMATCH = 5083,
                ///<summary> The resource monitor will not allow the fail operation to be performed while the resource is in its current state. This may happen if the resource is in a pending state.</summary>
                ERROR_RESMON_INVALID_STATE = 5084,
                ///<summary> A non locker code got a request to reserve the lock for making global updates.</summary>
                ERROR_CLUSTER_GUM_NOT_LOCKER = 5085,
                ///<summary> The quorum disk could not be located by the cluster service.</summary>
                ERROR_QUORUM_DISK_NOT_FOUND = 5086,
                ///<summary> The backed up cluster database is possibly corrupt.</summary>
                ERROR_DATABASE_BACKUP_CORRUPT = 5087,
                ///<summary> A DFS root already exists in this cluster node.</summary>
                ERROR_CLUSTER_NODE_ALREADY_HAS_DFS_ROOT = 5088,
                ///<summary> An attempt to modify a resource property failed because it conflicts with another existing property.</summary>
                ERROR_RESOURCE_PROPERTY_UNCHANGEABLE = 5089,



                //
                //  Codes from 4300 through 5889 overlap with codes in ds\published\inc\apperr2.w.
                //  Do not add any more error codes in that range.
                //


                ///<summary> An operation was attempted that is incompatible with the current membership state of the node.</summary>
                ERROR_CLUSTER_MEMBERSHIP_INVALID_STATE = 5890,
                ///<summary> The quorum resource does not contain the quorum log.</summary>
                ERROR_CLUSTER_QUORUMLOG_NOT_FOUND = 5891,
                ///<summary> The membership engine requested shutdown of the cluster service on this node.</summary>
                ERROR_CLUSTER_MEMBERSHIP_HALT = 5892,
                ///<summary> The join operation failed because the cluster instance ID of the joining node does not match the cluster instance ID of the sponsor node.</summary>
                ERROR_CLUSTER_INSTANCE_ID_MISMATCH = 5893,
                ///<summary> A matching network for the specified IP address could not be found. Please also specify a subnet mask and a cluster network.</summary>
                ERROR_CLUSTER_NETWORK_NOT_FOUND_FOR_IP = 5894,
                ///<summary> The actual data type of the property did not match the expected data type of the property.</summary>
                ERROR_CLUSTER_PROPERTY_DATA_TYPE_MISMATCH = 5895,
                ///<summary> The cluster node was evicted from the cluster successfully, but the node was not cleaned up.  Extended status information explaining why the node was not cleaned up is available.</summary>
                ERROR_CLUSTER_EVICT_WITHOUT_CLEANUP = 5896,
                ///<summary> Two or more parameter values specified for a resource's properties are in conflict.</summary>
                ERROR_CLUSTER_PARAMETER_MISMATCH = 5897,
                ///<summary> This computer cannot be made a member of a cluster.</summary>
                ERROR_NODE_CANNOT_BE_CLUSTERED = 5898,
                ///<summary> This computer cannot be made a member of a cluster because it does not have the correct version of Windows installed.</summary>
                ERROR_CLUSTER_WRONG_OS_VERSION = 5899,
                ///<summary> A cluster cannot be created with the specified cluster name because that cluster name is already in use. Specify a different name for the cluster.</summary>
                ERROR_CLUSTER_CANT_CREATE_DUP_CLUSTER_NAME = 5900,
                ///<summary> The cluster configuration action has already been committed.</summary>
                ERROR_CLUSCFG_ALREADY_COMMITTED = 5901,
                ///<summary> The cluster configuration action could not be rolled back.</summary>
                ERROR_CLUSCFG_ROLLBACK_FAILED = 5902,
                ///<summary> The drive letter assigned to a system disk on one node conflicted with the drive letter assigned to a disk on another node.</summary>
                ERROR_CLUSCFG_SYSTEM_DISK_DRIVE_LETTER_CONFLICT = 5903,
                ///<summary> One or more nodes in the cluster are running a version of Windows that does not support this operation.</summary>
                ERROR_CLUSTER_OLD_VERSION = 5904,
                ///<summary> The name of the corresponding computer account doesn't match the Network Name for this resource.</summary>
                ERROR_CLUSTER_MISMATCHED_COMPUTER_ACCT_NAME = 5905,


                #endregion

                #region EFS Error Codes

                ///<summary> The specified file could not be encrypted.</summary>
                ERROR_ENCRYPTION_FAILED = 6000,
                ///<summary> The specified file could not be decrypted.</summary>
                ERROR_DECRYPTION_FAILED = 6001,
                ///<summary> The specified file is encrypted and the user does not have the ability to decrypt it.</summary>
                ERROR_FILE_ENCRYPTED = 6002,
                ///<summary> There is no valid encryption recovery policy configured for this system.</summary>
                ERROR_NO_RECOVERY_POLICY = 6003,
                ///<summary> The required encryption driver is not loaded for this system.</summary>
                ERROR_NO_EFS = 6004,
                ///<summary> The file was encrypted with a different encryption driver than is currently loaded.</summary>
                ERROR_WRONG_EFS = 6005,
                ///<summary> There are no EFS keys defined for the user.</summary>
                ERROR_NO_USER_KEYS = 6006,
                ///<summary> The specified file is not encrypted.</summary>
                ERROR_FILE_NOT_ENCRYPTED = 6007,
                ///<summary> The specified file is not in the defined EFS export format.</summary>
                ERROR_NOT_EXPORT_FORMAT = 6008,
                ///<summary> The specified file is read only.</summary>
                ERROR_FILE_READ_ONLY = 6009,
                ///<summary> The directory has been disabled for encryption.</summary>
                ERROR_DIR_EFS_DISALLOWED = 6010,
                ///<summary> The server is not trusted for remote encryption operation.</summary>
                ERROR_EFS_SERVER_NOT_TRUSTED = 6011,
                ///<summary> Recovery policy configured for this system contains invalid recovery certificate.</summary>
                ERROR_BAD_RECOVERY_POLICY = 6012,
                ///<summary> The encryption algorithm used on the source file needs a bigger key buffer than the one on the destination file.</summary>
                ERROR_EFS_ALG_BLOB_TOO_BIG = 6013,
                ///<summary> The disk partition does not support file encryption.</summary>
                ERROR_VOLUME_NOT_SUPPORT_EFS = 6014,
                ///<summary> This machine is disabled for file encryption.</summary>
                ERROR_EFS_DISABLED = 6015,
                ///<summary> A newer system is required to decrypt this encrypted file.</summary>
                ERROR_EFS_VERSION_NOT_SUPPORT = 6016,

                #endregion

                ///<summary> This message number is for historical purposes and cannot be changed or re-used.
                ///The list of servers for this workgroup is not currently available</summary>
                ERROR_NO_BROWSER_SERVERS_FOUND = 6118,


                #region Task Scheduler Error Codes that NET START must understand

                ///<summary> The Task Scheduler service must be configured to run in the System account to function properly.  Individual tasks may be configured to run in other accounts.</summary>
                SCHED_E_SERVICE_NOT_LOCALSYSTEM = 6200,
                #endregion


                #region Terminal Server Error Codes

                ///<summary> The specified session name is invalid.</summary>
                ERROR_CTX_WINSTATION_NAME_INVALID = 7001,
                ///<summary> The specified protocol driver is invalid.</summary>
                ERROR_CTX_INVALID_PD = 7002,
                ///<summary> The specified protocol driver was not found in the system path.</summary>
                ERROR_CTX_PD_NOT_FOUND = 7003,
                ///<summary> The specified terminal connection driver was not found in the system path.</summary>
                ERROR_CTX_WD_NOT_FOUND = 7004,
                ///<summary> A registry key for event logging could not be created for this session.</summary>
                ERROR_CTX_CANNOT_MAKE_EVENTLOG_ENTRY = 7005,
                ///<summary> A service with the same name already exists on the system.</summary>
                ERROR_CTX_SERVICE_NAME_COLLISION = 7006,
                ///<summary> A close operation is pending on the session.</summary>
                ERROR_CTX_CLOSE_PENDING = 7007,
                ///<summary> There are no free output buffers available.</summary>
                ERROR_CTX_NO_OUTBUF = 7008,
                ///<summary> The MODEM.INF file was not found.</summary>
                ERROR_CTX_MODEM_INF_NOT_FOUND = 7009,
                ///<summary> The modem name was not found in MODEM.INF.</summary>
                ERROR_CTX_INVALID_MODEMNAME = 7010,
                ///<summary> The modem did not accept the command sent to it. Verify that the configured modem name matches the attached modem.</summary>
                ERROR_CTX_MODEM_RESPONSE_ERROR = 7011,
                ///<summary> The modem did not respond to the command sent to it. Verify that the modem is properly cabled and powered on.</summary>
                ERROR_CTX_MODEM_RESPONSE_TIMEOUT = 7012,
                ///<summary> Carrier detect has failed or carrier has been dropped due to disconnect.</summary>
                ERROR_CTX_MODEM_RESPONSE_NO_CARRIER = 7013,
                ///<summary> Dial tone not detected within the required time. Verify that the phone cable is properly attached and functional.</summary>
                ERROR_CTX_MODEM_RESPONSE_NO_DIALTONE = 7014,
                ///<summary> Busy signal detected at remote site on callback.</summary>
                ERROR_CTX_MODEM_RESPONSE_BUSY = 7015,
                ///<summary> Voice detected at remote site on callback.</summary>
                ERROR_CTX_MODEM_RESPONSE_VOICE = 7016,
                ///<summary> Transport driver error</summary>
                ERROR_CTX_TD_ERROR = 7017,
                ///<summary> The specified session cannot be found.</summary>
                ERROR_CTX_WINSTATION_NOT_FOUND = 7022,
                ///<summary> The specified session name is already in use.</summary>
                ERROR_CTX_WINSTATION_ALREADY_EXISTS = 7023,
                ///<summary> The requested operation cannot be completed because the terminal connection is currently busy processing a connect, disconnect, reset, or delete operation.</summary>
                ERROR_CTX_WINSTATION_BUSY = 7024,
                ///<summary> An attempt has been made to connect to a session whose video mode is not supported by the current client.</summary>
                ERROR_CTX_BAD_VIDEO_MODE = 7025,
                ///<summary> The application attempted to enable DOS graphics mode.
                ///DOS graphics mode is not supported.</summary>
                ERROR_CTX_GRAPHICS_INVALID = 7035,
                ///<summary> Your interactive logon privilege has been disabled.
                ///Please contact your administrator.</summary>
                ERROR_CTX_LOGON_DISABLED = 7037,
                ///<summary> The requested operation can be performed only on the system console.
                ///This is most often the result of a driver or system DLL requiring direct console access.</summary>
                ERROR_CTX_NOT_CONSOLE = 7038,
                ///<summary> The client failed to respond to the server connect message.</summary>
                ERROR_CTX_CLIENT_QUERY_TIMEOUT = 7040,
                ///<summary> Disconnecting the console session is not supported.</summary>
                ERROR_CTX_CONSOLE_DISCONNECT = 7041,
                ///<summary> Reconnecting a disconnected session to the console is not supported.</summary>
                ERROR_CTX_CONSOLE_CONNECT = 7042,
                ///<summary> The request to control another session remotely was denied.</summary>
                ERROR_CTX_SHADOW_DENIED = 7044,
                ///<summary> The requested session access is denied.</summary>
                ERROR_CTX_WINSTATION_ACCESS_DENIED = 7045,
                ///<summary> The specified terminal connection driver is invalid.</summary>
                ERROR_CTX_INVALID_WD = 7049,
                ///<summary> The requested session cannot be controlled remotely.
                ///This may be because the session is disconnected or does not currently have a user logged on.</summary>
                ERROR_CTX_SHADOW_INVALID = 7050,
                ///<summary> The requested session is not configured to allow remote control.</summary>
                ERROR_CTX_SHADOW_DISABLED = 7051,
                ///<summary> Your request to connect to this Terminal Server has been rejected. Your Terminal Server client license number is currently being used by another user.
                ///Please call your system administrator to obtain a unique license number.</summary>
                ERROR_CTX_CLIENT_LICENSE_IN_USE = 7052,
                ///<summary> Your request to connect to this Terminal Server has been rejected. Your Terminal Server client license number has not been entered for this copy of the Terminal Server client.
                ///Please contact your system administrator.</summary>
                ERROR_CTX_CLIENT_LICENSE_NOT_SET = 7053,
                ///<summary> The system has reached its licensed logon limit.
                ///Please try again later.</summary>
                ERROR_CTX_LICENSE_NOT_AVAILABLE = 7054,
                ///<summary> The client you are using is not licensed to use this system.  Your logon request is denied.</summary>
                ERROR_CTX_LICENSE_CLIENT_INVALID = 7055,
                ///<summary> The system license has expired.  Your logon request is denied.</summary>
                ERROR_CTX_LICENSE_EXPIRED = 7056,
                ///<summary> Remote control could not be terminated because the specified session is not currently being remotely controlled.</summary>
                ERROR_CTX_SHADOW_NOT_RUNNING = 7057,
                ///<summary> The remote control of the console was terminated because the display mode was changed. Changing the display mode in a remote control session is not supported.</summary>
                ERROR_CTX_SHADOW_ENDED_BY_MODE_CHANGE = 7058,
                ///<summary> Activation has already been reset the maximum number of times for this installation. Your activation timer will not be cleared.</summary>
                ERROR_ACTIVATION_COUNT_EXCEEDED = 7059,

                #endregion


                #region Traffic Control Error Codes
                // 7500 to  7999 defined in: tcerror.h 
                #endregion


                #region Active Directory Error Codes (8000 to  8999)

                #region FACILITY_FILE_REPLICATION_SERVICE

                ///<summary> The file replication service API was called incorrectly.</summary>
                FRS_ERR_INVALID_API_SEQUENCE = 8001,
                ///<summary> The file replication service cannot be started.</summary>
                FRS_ERR_STARTING_SERVICE = 8002,
                ///<summary> The file replication service cannot be stopped.</summary>
                FRS_ERR_STOPPING_SERVICE = 8003,
                ///<summary> The file replication service API terminated the request.
                //  The event log may have more information.</summary>
                FRS_ERR_INTERNAL_API = 8004,
                ///<summary> The file replication service terminated the request.
                //  The event log may have more information.</summary>
                FRS_ERR_INTERNAL = 8005,
                ///<summary> The file replication service cannot be contacted.
                //  The event log may have more information.</summary>
                FRS_ERR_SERVICE_COMM = 8006,
                ///<summary> The file replication service cannot satisfy the request because the user has insufficient privileges.
                //  The event log may have more information.</summary>
                FRS_ERR_INSUFFICIENT_PRIV = 8007,
                ///<summary> The file replication service cannot satisfy the request because authenticated RPC is not available.
                //  The event log may have more information.</summary>
                FRS_ERR_AUTHENTICATION = 8008,
                ///<summary> The file replication service cannot satisfy the request because the user has insufficient privileges on the domain controller.
                //  The event log may have more information.</summary>
                FRS_ERR_PARENT_INSUFFICIENT_PRIV = 8009,
                ///<summary> The file replication service cannot satisfy the request because authenticated RPC is not available on the domain controller.
                //  The event log may have more information.</summary>
                FRS_ERR_PARENT_AUTHENTICATION = 8010,
                ///<summary> The file replication service cannot communicate with the file replication service on the domain controller.
                //  The event log may have more information.</summary>
                FRS_ERR_CHILD_TO_PARENT_COMM = 8011,
                ///<summary> The file replication service on the domain controller cannot communicate with the file replication service on this computer.
                //  The event log may have more information.</summary>
                FRS_ERR_PARENT_TO_CHILD_COMM = 8012,
                ///<summary> The file replication service cannot populate the system volume because of an internal error.
                //  The event log may have more information.</summary>
                FRS_ERR_SYSVOL_POPULATE = 8013,
                ///<summary> The file replication service cannot populate the system volume because of an internal timeout.
                //  The event log may have more information.</summary>
                FRS_ERR_SYSVOL_POPULATE_TIMEOUT = 8014,
                ///<summary> The file replication service cannot process the request. The system volume is busy with a previous request.</summary>
                FRS_ERR_SYSVOL_IS_BUSY = 8015,
                ///<summary> The file replication service cannot stop replicating the system volume because of an internal error.
                //  The event log may have more information.</summary>
                FRS_ERR_SYSVOL_DEMOTE = 8016,
                ///<summary> The file replication service detected an invalid parameter.</summary>
                FRS_ERR_INVALID_SERVICE_PARAMETER = 8017,
                #endregion

                #region FACILITY DIRECTORY SERVICE

                DS_S_SUCCESS = NO_ERROR,

                ///<summary> An error occurred while installing the directory service. For more information, see the event log.</summary>
                ERROR_DS_NOT_INSTALLED = 8200,
                ///<summary> The directory service evaluated group memberships locally.</summary>
                ERROR_DS_MEMBERSHIP_EVALUATED_LOCALLY = 8201,
                ///<summary> The specified directory service attribute or value does not exist.</summary>
                ERROR_DS_NO_ATTRIBUTE_OR_VALUE = 8202,
                ///<summary> The attribute syntax specified to the directory service is invalid.</summary>
                ERROR_DS_INVALID_ATTRIBUTE_SYNTAX = 8203,
                ///<summary> The attribute type specified to the directory service is not defined.</summary>
                ERROR_DS_ATTRIBUTE_TYPE_UNDEFINED = 8204,
                ///<summary> The specified directory service attribute or value already exists.</summary>
                ERROR_DS_ATTRIBUTE_OR_VALUE_EXISTS = 8205,
                ///<summary> The directory service is busy.</summary>
                ERROR_DS_BUSY = 8206,
                ///<summary> The directory service is unavailable.</summary>
                ERROR_DS_UNAVAILABLE = 8207,
                ///<summary> The directory service was unable to allocate a relative identifier.</summary>
                ERROR_DS_NO_RIDS_ALLOCATED = 8208,
                ///<summary> The directory service has exhausted the pool of relative identifiers.</summary>
                ERROR_DS_NO_MORE_RIDS = 8209,
                ///<summary> The requested operation could not be performed because the directory service is not the master for that type of operation.</summary>
                ERROR_DS_INCORRECT_ROLE_OWNER = 8210,
                ///<summary> The directory service was unable to initialize the subsystem that allocates relative identifiers.</summary>
                ERROR_DS_RIDMGR_INIT_ERROR = 8211,
                ///<summary> The requested operation did not satisfy one or more constraints associated with the class of the object.</summary>
                ERROR_DS_OBJ_CLASS_VIOLATION = 8212,
                ///<summary> The directory service can perform the requested operation only on a leaf object.</summary>
                ERROR_DS_CANT_ON_NON_LEAF = 8213,
                ///<summary> The directory service cannot perform the requested operation on the RDN attribute of an object.</summary>
                ERROR_DS_CANT_ON_RDN = 8214,
                ///<summary> The directory service detected an attempt to modify the object class of an object.</summary>
                ERROR_DS_CANT_MOD_OBJ_CLASS = 8215,
                ///<summary> The requested cross-domain move operation could not be performed.</summary>
                ERROR_DS_CROSS_DOM_MOVE_ERROR = 8216,
                ///<summary> Unable to contact the global catalog server.</summary>
                ERROR_DS_GC_NOT_AVAILABLE = 8217,
                ///<summary> The policy object is shared and can only be modified at the root.</summary>
                ERROR_SHARED_POLICY = 8218,
                ///<summary> The policy object does not exist.</summary>
                ERROR_POLICY_OBJECT_NOT_FOUND = 8219,
                ///<summary> The requested policy information is only in the directory service.</summary>
                ERROR_POLICY_ONLY_IN_DS = 8220,
                ///<summary> A domain controller promotion is currently active.</summary>
                ERROR_PROMOTION_ACTIVE = 8221,
                ///<summary> A domain controller promotion is not currently active</summary>
                ERROR_NO_PROMOTION_ACTIVE = 8222,


                // 8223 unused

                ///<summary> An operations error occurred.</summary>
                ERROR_DS_OPERATIONS_ERROR = 8224,
                ///<summary> A protocol error occurred.</summary>
                ERROR_DS_PROTOCOL_ERROR = 8225,
                ///<summary> The time limit for this request was exceeded.</summary>
                ERROR_DS_TIMELIMIT_EXCEEDED = 8226,
                ///<summary> The size limit for this request was exceeded.</summary>
                ERROR_DS_SIZELIMIT_EXCEEDED = 8227,
                ///<summary> The administrative limit for this request was exceeded.</summary>
                ERROR_DS_ADMIN_LIMIT_EXCEEDED = 8228,
                ///<summary> The compare response was false.</summary>
                ERROR_DS_COMPARE_FALSE = 8229,
                ///<summary> The compare response was true.</summary>
                ERROR_DS_COMPARE_TRUE = 8230,
                ///<summary> The requested authentication method is not supported by the server.</summary>
                ERROR_DS_AUTH_METHOD_NOT_SUPPORTED = 8231,
                ///<summary> A more secure authentication method is required for this server.</summary>
                ERROR_DS_STRONG_AUTH_REQUIRED = 8232,
                ///<summary> Inappropriate authentication.</summary>
                ERROR_DS_INAPPROPRIATE_AUTH = 8233,
                ///<summary> The authentication mechanism is unknown.</summary>
                ERROR_DS_AUTH_UNKNOWN = 8234,
                ///<summary> A referral was returned from the server.</summary>
                ERROR_DS_REFERRAL = 8235,
                ///<summary> The server does not support the requested critical extension.</summary>
                ERROR_DS_UNAVAILABLE_CRIT_EXTENSION = 8236,
                ///<summary> This request requires a secure connection.</summary>
                ERROR_DS_CONFIDENTIALITY_REQUIRED = 8237,
                ///<summary> Inappropriate matching.</summary>
                ERROR_DS_INAPPROPRIATE_MATCHING = 8238,
                ///<summary> A constraint violation occurred.</summary>
                ERROR_DS_CONSTRAINT_VIOLATION = 8239,
                ///<summary> There is no such object on the server.</summary>
                ERROR_DS_NO_SUCH_OBJECT = 8240,
                ///<summary> There is an alias problem.</summary>
                ERROR_DS_ALIAS_PROBLEM = 8241,
                ///<summary> An invalid dn syntax has been specified.</summary>
                ERROR_DS_INVALID_DN_SYNTAX = 8242,
                ///<summary> The object is a leaf object.</summary>
                ERROR_DS_IS_LEAF = 8243,
                ///<summary> There is an alias dereferencing problem.</summary>
                ERROR_DS_ALIAS_DEREF_PROBLEM = 8244,
                ///<summary> The server is unwilling to process the request.</summary>
                ERROR_DS_UNWILLING_TO_PERFORM = 8245,
                ///<summary> A loop has been detected.</summary>
                ERROR_DS_LOOP_DETECT = 8246,
                ///<summary> There is a naming violation.</summary>
                ERROR_DS_NAMING_VIOLATION = 8247,
                ///<summary> The result set is too large.</summary>
                ERROR_DS_OBJECT_RESULTS_TOO_LARGE = 8248,
                ///<summary> The operation affects multiple DSAs</summary>
                ERROR_DS_AFFECTS_MULTIPLE_DSAS = 8249,
                ///<summary> The server is not operational.</summary>
                ERROR_DS_SERVER_DOWN = 8250,
                ///<summary> A local error has occurred.</summary>
                ERROR_DS_LOCAL_ERROR = 8251,
                ///<summary> An encoding error has occurred.</summary>
                ERROR_DS_ENCODING_ERROR = 8252,
                ///<summary> A decoding error has occurred.</summary>
                ERROR_DS_DECODING_ERROR = 8253,
                ///<summary> The search filter cannot be recognized.</summary>
                ERROR_DS_FILTER_UNKNOWN = 8254,
                ///<summary> One or more parameters are illegal.</summary>
                ERROR_DS_PARAM_ERROR = 8255,
                ///<summary> The specified method is not supported.</summary>
                ERROR_DS_NOT_SUPPORTED = 8256,
                ///<summary> No results were returned.</summary>
                ERROR_DS_NO_RESULTS_RETURNED = 8257,
                ///<summary> The specified control is not supported by the server.</summary>
                ERROR_DS_CONTROL_NOT_FOUND = 8258,
                ///<summary> A referral loop was detected by the client.</summary>
                ERROR_DS_CLIENT_LOOP = 8259,
                ///<summary> The preset referral limit was exceeded.</summary>
                ERROR_DS_REFERRAL_LIMIT_EXCEEDED = 8260,
                ///<summary> The search requires a SORT control.</summary>
                ERROR_DS_SORT_CONTROL_MISSING = 8261,
                ///<summary> The search results exceed the offset range specified.</summary>
                ERROR_DS_OFFSET_RANGE_ERROR = 8262,
                ///<summary> The root object must be the head of a naming context. The root object cannot have an instantiated parent.</summary>
                ERROR_DS_ROOT_MUST_BE_NC = 8301,
                ///<summary> The add replica operation cannot be performed. The naming context must be writeable in order to create the replica.</summary>
                ERROR_DS_ADD_REPLICA_INHIBITED = 8302,
                ///<summary> A reference to an attribute that is not defined in the schema occurred.</summary>
                ERROR_DS_ATT_NOT_DEF_IN_SCHEMA = 8303,
                ///<summary> The maximum size of an object has been exceeded.</summary>
                ERROR_DS_MAX_OBJ_SIZE_EXCEEDED = 8304,
                ///<summary> An attempt was made to add an object to the directory with a name that is already in use.</summary>
                ERROR_DS_OBJ_STRING_NAME_EXISTS = 8305,
                ///<summary> An attempt was made to add an object of a class that does not have an RDN defined in the schema.</summary>
                ERROR_DS_NO_RDN_DEFINED_IN_SCHEMA = 8306,
                ///<summary> An attempt was made to add an object using an RDN that is not the RDN defined in the schema.</summary>
                ERROR_DS_RDN_DOESNT_MATCH_SCHEMA = 8307,
                ///<summary> None of the requested attributes were found on the objects.</summary>
                ERROR_DS_NO_REQUESTED_ATTS_FOUND = 8308,
                ///<summary> The user buffer is too small.</summary>
                ERROR_DS_USER_BUFFER_TO_SMALL = 8309,
                ///<summary> The attribute specified in the operation is not present on the object.</summary>
                ERROR_DS_ATT_IS_NOT_ON_OBJ = 8310,
                ///<summary> Illegal modify operation. Some aspect of the modification is not permitted.</summary>
                ERROR_DS_ILLEGAL_MOD_OPERATION = 8311,
                ///<summary> The specified object is too large.</summary>
                ERROR_DS_OBJ_TOO_LARGE = 8312,
                ///<summary> The specified instance type is not valid.</summary>
                ERROR_DS_BAD_INSTANCE_TYPE = 8313,
                ///<summary> The operation must be performed at a master DSA.</summary>
                ERROR_DS_MASTERDSA_REQUIRED = 8314,
                ///<summary> The object class attribute must be specified.</summary>
                ERROR_DS_OBJECT_CLASS_REQUIRED = 8315,
                ///<summary> A required attribute is missing.</summary>
                ERROR_DS_MISSING_REQUIRED_ATT = 8316,
                ///<summary> An attempt was made to modify an object to include an attribute that is not legal for its class.</summary>
                ERROR_DS_ATT_NOT_DEF_FOR_CLASS = 8317,
                ///<summary> The specified attribute is already present on the object.</summary>
                ERROR_DS_ATT_ALREADY_EXISTS = 8318,


                // 8319 unused

                ///<summary> The specified attribute is not present, or has no values.</summary>
                ERROR_DS_CANT_ADD_ATT_VALUES = 8320,
                ///<summary> Multiple values were specified for an attribute that can have only one value.</summary>
                ERROR_DS_SINGLE_VALUE_CONSTRAINT = 8321,
                ///<summary> A value for the attribute was not in the acceptable range of values.</summary>
                ERROR_DS_RANGE_CONSTRAINT = 8322,
                ///<summary> The specified value already exists.</summary>
                ERROR_DS_ATT_VAL_ALREADY_EXISTS = 8323,
                ///<summary> The attribute cannot be removed because it is not present on the object.</summary>
                ERROR_DS_CANT_REM_MISSING_ATT = 8324,
                ///<summary> The attribute value cannot be removed because it is not present on the object.</summary>
                ERROR_DS_CANT_REM_MISSING_ATT_VAL = 8325,
                ///<summary> The specified root object cannot be a subref.</summary>
                ERROR_DS_ROOT_CANT_BE_SUBREF = 8326,
                ///<summary> Chaining is not permitted.</summary>
                ERROR_DS_NO_CHAINING = 8327,
                ///<summary> Chained evaluation is not permitted.</summary>
                ERROR_DS_NO_CHAINED_EVAL = 8328,
                ///<summary> The operation could not be performed because the object's parent is either uninstantiated or deleted.</summary>
                ERROR_DS_NO_PARENT_OBJECT = 8329,
                ///<summary> Having a parent that is an alias is not permitted. Aliases are leaf objects.</summary>
                ERROR_DS_PARENT_IS_AN_ALIAS = 8330,
                ///<summary> The object and parent must be of the same type, either both masters or both replicas.</summary>
                ERROR_DS_CANT_MIX_MASTER_AND_REPS = 8331,
                ///<summary> The operation cannot be performed because child objects exist. This operation can only be performed on a leaf object.</summary>
                ERROR_DS_CHILDREN_EXIST = 8332,
                ///<summary> Directory object not found.</summary>
                ERROR_DS_OBJ_NOT_FOUND = 8333,
                ///<summary> The aliased object is missing.</summary>
                ERROR_DS_ALIASED_OBJ_MISSING = 8334,
                ///<summary> The object name has bad syntax.</summary>
                ERROR_DS_BAD_NAME_SYNTAX = 8335,
                ///<summary> It is not permitted for an alias to refer to another alias.</summary>
                ERROR_DS_ALIAS_POINTS_TO_ALIAS = 8336,
                ///<summary> The alias cannot be dereferenced.</summary>
                ERROR_DS_CANT_DEREF_ALIAS = 8337,
                ///<summary> The operation is [Out ] of scope.</summary>
                ERROR_DS_OUT_OF_SCOPE = 8338,
                ///<summary> The operation cannot continue because the object is in the process of being removed.</summary>
                ERROR_DS_OBJECT_BEING_REMOVED = 8339,
                ///<summary> The DSA object cannot be deleted.</summary>
                ERROR_DS_CANT_DELETE_DSA_OBJ = 8340,
                ///<summary> A directory service error has occurred.</summary>
                ERROR_DS_GENERIC_ERROR = 8341,
                ///<summary> The operation can only be performed on an internal master DSA object.</summary>
                ERROR_DS_DSA_MUST_BE_INT_MASTER = 8342,
                ///<summary> The object must be of class DSA.</summary>
                ERROR_DS_CLASS_NOT_DSA = 8343,
                ///<summary> Insufficient access rights to perform the operation.</summary>
                ERROR_DS_INSUFF_ACCESS_RIGHTS = 8344,
                ///<summary> The object cannot be added because the parent is not on the list of possible superiors.</summary>
                ERROR_DS_ILLEGAL_SUPERIOR = 8345,
                ///<summary> Access to the attribute is not permitted because the attribute is owned by the Security Accounts Manager (SAM).</summary>
                ERROR_DS_ATTRIBUTE_OWNED_BY_SAM = 8346,
                ///<summary> The name has too many parts.</summary>
                ERROR_DS_NAME_TOO_MANY_PARTS = 8347,
                ///<summary> The name is too long.</summary>
                ERROR_DS_NAME_TOO_LONG = 8348,
                ///<summary> The name value is too long.</summary>
                ERROR_DS_NAME_VALUE_TOO_LONG = 8349,
                ///<summary> The directory service encountered an error parsing a name.</summary>
                ERROR_DS_NAME_UNPARSEABLE = 8350,
                ///<summary> The directory service cannot get the attribute type for a name.</summary>
                ERROR_DS_NAME_TYPE_UNKNOWN = 8351,
                ///<summary> The name does not identify an object; the name identifies a phantom.</summary>
                ERROR_DS_NOT_AN_OBJECT = 8352,
                ///<summary> The security descriptor is too short.</summary>
                ERROR_DS_SEC_DESC_TOO_SHORT = 8353,
                ///<summary> The security descriptor is invalid.</summary>
                ERROR_DS_SEC_DESC_INVALID = 8354,
                ///<summary> Failed to create name for deleted object.</summary>
                ERROR_DS_NO_DELETED_NAME = 8355,
                ///<summary> The parent of a new subref must exist.</summary>
                ERROR_DS_SUBREF_MUST_HAVE_PARENT = 8356,
                ///<summary> The object must be a naming context.</summary>
                ERROR_DS_NCNAME_MUST_BE_NC = 8357,
                ///<summary> It is not permitted to add an attribute which is owned by the system.</summary>
                ERROR_DS_CANT_ADD_SYSTEM_ONLY = 8358,
                ///<summary> The class of the object must be structural; you cannot instantiate an abstract class.</summary>
                ERROR_DS_CLASS_MUST_BE_CONCRETE = 8359,
                ///<summary> The schema object could not be found.</summary>
                ERROR_DS_INVALID_DMD = 8360,
                ///<summary> A local object with this GUID (dead or alive) already exists.</summary>
                ERROR_DS_OBJ_GUID_EXISTS = 8361,
                ///<summary> The operation cannot be performed on a back link.</summary>
                ERROR_DS_NOT_ON_BACKLINK = 8362,
                ///<summary> The cross reference for the specified naming context could not be found.</summary>
                ERROR_DS_NO_CROSSREF_FOR_NC = 8363,
                ///<summary> The operation could not be performed because the directory service is shutting down.</summary>
                ERROR_DS_SHUTTING_DOWN = 8364,
                ///<summary> The directory service request is invalid.</summary>
                ERROR_DS_UNKNOWN_OPERATION = 8365,
                ///<summary> The role owner attribute could not be read.</summary>
                ERROR_DS_INVALID_ROLE_OWNER = 8366,
                ///<summary> The requested FSMO operation failed. The current FSMO holder could not be contacted.</summary>
                ERROR_DS_COULDNT_CONTACT_FSMO = 8367,
                ///<summary> Modification of a DN across a naming context is not permitted.</summary>
                ERROR_DS_CROSS_NC_DN_RENAME = 8368,
                ///<summary> The attribute cannot be modified because it is owned by the system.</summary>
                ERROR_DS_CANT_MOD_SYSTEM_ONLY = 8369,
                ///<summary> Only the replicator can perform this function.</summary>
                ERROR_DS_REPLICATOR_ONLY = 8370,
                ///<summary> The specified class is not defined.</summary>
                ERROR_DS_OBJ_CLASS_NOT_DEFINED = 8371,
                ///<summary> The specified class is not a subclass.</summary>
                ERROR_DS_OBJ_CLASS_NOT_SUBCLASS = 8372,
                ///<summary> The name reference is invalid.</summary>
                ERROR_DS_NAME_REFERENCE_INVALID = 8373,
                ///<summary> A cross reference already exists.</summary>
                ERROR_DS_CROSS_REF_EXISTS = 8374,
                ///<summary> It is not permitted to delete a master cross reference.</summary>
                ERROR_DS_CANT_DEL_MASTER_CROSSREF = 8375,
                ///<summary> Subtree notifications are only supported on NC heads.</summary>
                ERROR_DS_SUBTREE_NOTIFY_NOT_NC_HEAD = 8376,
                ///<summary> Notification filter is too complex.</summary>
                ERROR_DS_NOTIFY_FILTER_TOO_COMPLEX = 8377,
                ///<summary> Schema update failed: duplicate RDN.</summary>
                ERROR_DS_DUP_RDN = 8378,
                ///<summary> Schema update failed: duplicate OID.</summary>
                ERROR_DS_DUP_OID = 8379,
                ///<summary> Schema update failed: duplicate MAPI identifier.</summary>
                ERROR_DS_DUP_MAPI_ID = 8380,
                ///<summary> Schema update failed: duplicate schema-id GUID.</summary>
                ERROR_DS_DUP_SCHEMA_ID_GUID = 8381,
                ///<summary> Schema update failed: duplicate LDAP display name.</summary>
                ERROR_DS_DUP_LDAP_DISPLAY_NAME = 8382,
                ///<summary> Schema update failed: range-lower less than range upper.</summary>
                ERROR_DS_SEMANTIC_ATT_TEST = 8383,
                ///<summary> Schema update failed: syntax mismatch.</summary>
                ERROR_DS_SYNTAX_MISMATCH = 8384,
                ///<summary> Schema deletion failed: attribute is used in must-contain.</summary>
                ERROR_DS_EXISTS_IN_MUST_HAVE = 8385,
                ///<summary> Schema deletion failed: attribute is used in may-contain.</summary>
                ERROR_DS_EXISTS_IN_MAY_HAVE = 8386,
                ///<summary> Schema update failed: attribute in may-contain does not exist.</summary>
                ERROR_DS_NONEXISTENT_MAY_HAVE = 8387,
                ///<summary> Schema update failed: attribute in must-contain does not exist.</summary>
                ERROR_DS_NONEXISTENT_MUST_HAVE = 8388,
                ///<summary> Schema update failed: class in aux-class list does not exist or is not an auxiliary class.</summary>
                ERROR_DS_AUX_CLS_TEST_FAIL = 8389,
                ///<summary> Schema update failed: class in poss-superiors does not exist.</summary>
                ERROR_DS_NONEXISTENT_POSS_SUP = 8390,
                ///<summary> Schema update failed: class in subclassof list does not exist or does not satisfy hierarchy rules.</summary>
                ERROR_DS_SUB_CLS_TEST_FAIL = 8391,
                ///<summary> Schema update failed: Rdn-Att-Id has wrong syntax.</summary>
                ERROR_DS_BAD_RDN_ATT_ID_SYNTAX = 8392,
                ///<summary> Schema deletion failed: class is used as auxiliary class.</summary>
                ERROR_DS_EXISTS_IN_AUX_CLS = 8393,
                ///<summary> Schema deletion failed: class is used as sub class.</summary>
                ERROR_DS_EXISTS_IN_SUB_CLS = 8394,
                ///<summary> Schema deletion failed: class is used as poss superior.</summary>
                ERROR_DS_EXISTS_IN_POSS_SUP = 8395,
                ///<summary> Schema update failed in recalculating validation cache.</summary>
                ERROR_DS_RECALCSCHEMA_FAILED = 8396,
                ///<summary> The tree deletion is not finished.  The request must be made again to continue deleting the tree.</summary>
                ERROR_DS_TREE_DELETE_NOT_FINISHED = 8397,
                ///<summary> The requested delete operation could not be performed.</summary>
                ERROR_DS_CANT_DELETE = 8398,
                ///<summary> Cannot read the governs class identifier for the schema record.</summary>
                ERROR_DS_ATT_SCHEMA_REQ_ID = 8399,
                ///<summary> The attribute schema has bad syntax.</summary>
                ERROR_DS_BAD_ATT_SCHEMA_SYNTAX = 8400,
                ///<summary> The attribute could not be cached.</summary>
                ERROR_DS_CANT_CACHE_ATT = 8401,
                ///<summary> The class could not be cached.</summary>
                ERROR_DS_CANT_CACHE_CLASS = 8402,
                ///<summary> The attribute could not be removed from the cache.</summary>
                ERROR_DS_CANT_REMOVE_ATT_CACHE = 8403,
                ///<summary> The class could not be removed from the cache.</summary>
                ERROR_DS_CANT_REMOVE_CLASS_CACHE = 8404,
                ///<summary> The distinguished name attribute could not be read.</summary>
                ERROR_DS_CANT_RETRIEVE_DN = 8405,
                ///<summary> No superior reference has been configured for the directory service. The directory service is therefore unable to issue referrals to objects outside this forest.</summary>
                ERROR_DS_MISSING_SUPREF = 8406,
                ///<summary> The instance type attribute could not be retrieved.</summary>
                ERROR_DS_CANT_RETRIEVE_INSTANCE = 8407,
                ///<summary> An internal error has occurred.</summary>
                ERROR_DS_CODE_INCONSISTENCY = 8408,
                ///<summary> A database error has occurred.</summary>
                ERROR_DS_DATABASE_ERROR = 8409,
                ///<summary> The attribute GOVERNSID is missing.</summary>
                ERROR_DS_GOVERNSID_MISSING = 8410,
                ///<summary> An expected attribute is missing.</summary>
                ERROR_DS_MISSING_EXPECTED_ATT = 8411,
                ///<summary> The specified naming context is missing a cross reference.</summary>
                ERROR_DS_NCNAME_MISSING_CR_REF = 8412,
                ///<summary> A security checking error has occurred.</summary>
                ERROR_DS_SECURITY_CHECKING_ERROR = 8413,
                ///<summary> The schema is not loaded.</summary>
                ERROR_DS_SCHEMA_NOT_LOADED = 8414,
                ///<summary> Schema allocation failed. Please check if the machine is running low on memory.</summary>
                ERROR_DS_SCHEMA_ALLOC_FAILED = 8415,
                ///<summary> Failed to obtain the required syntax for the attribute schema.</summary>
                ERROR_DS_ATT_SCHEMA_REQ_SYNTAX = 8416,
                ///<summary> The global catalog verification failed. The global catalog is not available or does not support the operation. Some part of the directory is currently not available.</summary>
                ERROR_DS_GCVERIFY_ERROR = 8417,
                ///<summary> The replication operation failed because of a schema mismatch between the servers involved.</summary>
                ERROR_DS_DRA_SCHEMA_MISMATCH = 8418,
                ///<summary> The DSA object could not be found.</summary>
                ERROR_DS_CANT_FIND_DSA_OBJ = 8419,
                ///<summary> The naming context could not be found.</summary>
                ERROR_DS_CANT_FIND_EXPECTED_NC = 8420,
                ///<summary> The naming context could not be found in the cache.</summary>
                ERROR_DS_CANT_FIND_NC_IN_CACHE = 8421,
                ///<summary> The child object could not be retrieved.</summary>
                ERROR_DS_CANT_RETRIEVE_CHILD = 8422,
                ///<summary> The modification was not permitted for security reasons.</summary>
                ERROR_DS_SECURITY_ILLEGAL_MODIFY = 8423,
                ///<summary> The operation cannot replace the hidden record.</summary>
                ERROR_DS_CANT_REPLACE_HIDDEN_REC = 8424,
                ///<summary> The hierarchy file is invalid.</summary>
                ERROR_DS_BAD_HIERARCHY_FILE = 8425,
                ///<summary> The attempt to build the hierarchy table failed.</summary>
                ERROR_DS_BUILD_HIERARCHY_TABLE_FAILED = 8426,
                ///<summary> The directory configuration parameter is missing from the registry.</summary>
                ERROR_DS_CONFIG_PARAM_MISSING = 8427,
                ///<summary> The attempt to count the address book indices failed.</summary>
                ERROR_DS_COUNTING_AB_INDICES_FAILED = 8428,
                ///<summary> The allocation of the hierarchy table failed.</summary>
                ERROR_DS_HIERARCHY_TABLE_MALLOC_FAILED = 8429,
                ///<summary> The directory service encountered an internal failure.</summary>
                ERROR_DS_INTERNAL_FAILURE = 8430,
                ///<summary> The directory service encountered an unknown failure.</summary>
                ERROR_DS_UNKNOWN_ERROR = 8431,
                ///<summary> A root object requires a class of 'top'.</summary>
                ERROR_DS_ROOT_REQUIRES_CLASS_TOP = 8432,
                ///<summary> This directory server is shutting down, and cannot take ownership of new floating single-master operation roles.</summary>
                ERROR_DS_REFUSING_FSMO_ROLES = 8433,
                ///<summary> The directory service is missing mandatory configuration information, and is unable to determine the ownership of floating single-master operation roles.</summary>
                ERROR_DS_MISSING_FSMO_SETTINGS = 8434,
                ///<summary> The directory service was unable to transfer ownership of one or more floating single-master operation roles to other servers.</summary>
                ERROR_DS_UNABLE_TO_SURRENDER_ROLES = 8435,
                ///<summary> The replication operation failed.</summary>
                ERROR_DS_DRA_GENERIC = 8436,
                ///<summary> An invalid parameter was specified for this replication operation.</summary>
                ERROR_DS_DRA_INVALID_PARAMETER = 8437,
                ///<summary> The directory service is too busy to complete the replication operation at this time.</summary>
                ERROR_DS_DRA_BUSY = 8438,
                ///<summary> The distinguished name specified for this replication operation is invalid.</summary>
                ERROR_DS_DRA_BAD_DN = 8439,
                ///<summary> The naming context specified for this replication operation is invalid.</summary>
                ERROR_DS_DRA_BAD_NC = 8440,
                ///<summary> The distinguished name specified for this replication operation already exists.</summary>
                ERROR_DS_DRA_DN_EXISTS = 8441,
                ///<summary> The replication system encountered an internal error.</summary>
                ERROR_DS_DRA_INTERNAL_ERROR = 8442,
                ///<summary> The replication operation encountered a database inconsistency.</summary>
                ERROR_DS_DRA_INCONSISTENT_DIT = 8443,
                ///<summary> The server specified for this replication operation could not be contacted.</summary>
                ERROR_DS_DRA_CONNECTION_FAILED = 8444,
                ///<summary> The replication operation encountered an object with an invalid instance type.</summary>
                ERROR_DS_DRA_BAD_INSTANCE_TYPE = 8445,
                ///<summary> The replication operation failed to allocate memory.</summary>
                ERROR_DS_DRA_OUT_OF_MEM = 8446,
                ///<summary> The replication operation encountered an error with the mail system.</summary>
                ERROR_DS_DRA_MAIL_PROBLEM = 8447,
                ///<summary> The replication reference information for the target server already exists.</summary>
                ERROR_DS_DRA_REF_ALREADY_EXISTS = 8448,
                ///<summary> The replication reference information for the target server does not exist.</summary>
                ERROR_DS_DRA_REF_NOT_FOUND = 8449,
                ///<summary> The naming context cannot be removed because it is replicated to another server.</summary>
                ERROR_DS_DRA_OBJ_IS_REP_SOURCE = 8450,
                ///<summary> The replication operation encountered a database error.</summary>
                ERROR_DS_DRA_DB_ERROR = 8451,
                ///<summary> The naming context is in the process of being removed or is not replicated from the specified server.</summary>
                ERROR_DS_DRA_NO_REPLICA = 8452,
                ///<summary> Replication access was denied.</summary>
                ERROR_DS_DRA_ACCESS_DENIED = 8453,
                ///<summary> The requested operation is not supported by this version of the directory service.</summary>
                ERROR_DS_DRA_NOT_SUPPORTED = 8454,
                ///<summary> The replication remote procedure call was cancelled.</summary>
                ERROR_DS_DRA_RPC_CANCELLED = 8455,
                ///<summary> The source server is currently rejecting replication requests.</summary>
                ERROR_DS_DRA_SOURCE_DISABLED = 8456,
                ///<summary> The destination server is currently rejecting replication requests.</summary>
                ERROR_DS_DRA_SINK_DISABLED = 8457,
                ///<summary> The replication operation failed due to a collision of object names.</summary>
                ERROR_DS_DRA_NAME_COLLISION = 8458,
                ///<summary> The replication source has been reinstalled.</summary>
                ERROR_DS_DRA_SOURCE_REINSTALLED = 8459,
                ///<summary> The replication operation failed because a required parent object is missing.</summary>
                ERROR_DS_DRA_MISSING_PARENT = 8460,
                ///<summary> The replication operation was preempted.</summary>
                ERROR_DS_DRA_PREEMPTED = 8461,
                ///<summary> The replication synchronization attempt was abandoned because of a lack of updates.</summary>
                ERROR_DS_DRA_ABANDON_SYNC = 8462,
                ///<summary> The replication operation was terminated because the system is shutting down.</summary>
                ERROR_DS_DRA_SHUTDOWN = 8463,
                ///<summary> The replication synchronization attempt failed as the destination partial attribute set is not a subset of source partial attribute set.</summary>
                ERROR_DS_DRA_INCOMPATIBLE_PARTIAL_SET = 8464,
                ///<summary> The replication synchronization attempt failed because a master replica attempted to sync from a partial replica.</summary>
                ERROR_DS_DRA_SOURCE_IS_PARTIAL_REPLICA = 8465,
                ///<summary> The server specified for this replication operation was contacted, but that server was unable to contact an additional server needed to complete the operation.</summary>
                ERROR_DS_DRA_EXTN_CONNECTION_FAILED = 8466,
                ///<summary> The version of the Active Directory schema of the source forest is not compatible with the version of Active Directory on this computer.</summary>
                ERROR_DS_INSTALL_SCHEMA_MISMATCH = 8467,
                ///<summary> Schema update failed: An attribute with the same link identifier already exists.</summary>
                ERROR_DS_DUP_LINK_ID = 8468,
                ///<summary> Name translation: Generic processing error.</summary>
                ERROR_DS_NAME_ERROR_RESOLVING = 8469,
                ///<summary> Name translation: Could not find the name or insufficient right to see name.</summary>
                ERROR_DS_NAME_ERROR_NOT_FOUND = 8470,
                ///<summary> Name translation: Input name mapped to more than one output name.</summary>
                ERROR_DS_NAME_ERROR_NOT_UNIQUE = 8471,
                ///<summary> Name translation: Input name found, but not the associated output format.</summary>
                ERROR_DS_NAME_ERROR_NO_MAPPING = 8472,
                ///<summary> Name translation: Unable to resolve completely, only the domain was found.</summary>
                ERROR_DS_NAME_ERROR_DOMAIN_ONLY = 8473,
                ///<summary> Name translation: Unable to perform purely syntactical mapping at the client without going [Out ] to the wire.</summary>
                ERROR_DS_NAME_ERROR_NO_SYNTACTICAL_MAPPING = 8474,
                ///<summary> Modification of a constructed attribute is not allowed.</summary>
                ERROR_DS_CONSTRUCTED_ATT_MOD = 8475,
                ///<summary> The OM-Object-Class specified is incorrect for an attribute with the specified syntax.</summary>
                ERROR_DS_WRONG_OM_OBJ_CLASS = 8476,
                ///<summary> The replication request has been posted; waiting for reply.</summary>
                ERROR_DS_DRA_REPL_PENDING = 8477,
                ///<summary> The requested operation requires a directory service, and none was available.</summary>
                ERROR_DS_DS_REQUIRED = 8478,
                ///<summary> The LDAP display name of the class or attribute contains non-ASCII characters.</summary>
                ERROR_DS_INVALID_LDAP_DISPLAY_NAME = 8479,
                ///<summary> The requested search operation is only supported for base searches.</summary>
                ERROR_DS_NON_BASE_SEARCH = 8480,
                ///<summary> The search failed to retrieve attributes from the database.</summary>
                ERROR_DS_CANT_RETRIEVE_ATTS = 8481,
                ///<summary> The schema update operation tried to add a backward link attribute that has no corresponding forward link.</summary>
                ERROR_DS_BACKLINK_WITHOUT_LINK = 8482,
                ///<summary> Source and destination of a cross-domain move do not agree on the object's epoch number.  Either source or destination does not have the latest version of the object.</summary>
                ERROR_DS_EPOCH_MISMATCH = 8483,
                ///<summary> Source and destination of a cross-domain move do not agree on the object's current name.  Either source or destination does not have the latest version of the object.</summary>
                ERROR_DS_SRC_NAME_MISMATCH = 8484,
                ///<summary> Source and destination for the cross-domain move operation are identical.  Caller should use local move operation instead of cross-domain move operation.</summary>
                ERROR_DS_SRC_AND_DST_NC_IDENTICAL = 8485,
                ///<summary> Source and destination for a cross-domain move are not in agreement on the naming contexts in the forest.  Either source or destination does not have the latest version of the Partitions container.</summary>
                ERROR_DS_DST_NC_MISMATCH = 8486,
                ///<summary> Destination of a cross-domain move is not authoritative for the destination naming context.</summary>
                ERROR_DS_NOT_AUTHORITIVE_FOR_DST_NC = 8487,
                ///<summary> Source and destination of a cross-domain move do not agree on the identity of the source object.  Either source or destination does not have the latest version of the source object.</summary>
                ERROR_DS_SRC_GUID_MISMATCH = 8488,
                ///<summary> Object being moved across-domains is already known to be deleted by the destination server.  The source server does not have the latest version of the source object.</summary>
                ERROR_DS_CANT_MOVE_DELETED_OBJECT = 8489,
                ///<summary> Another operation which requires exclusive access to the PDC FSMO is already in progress.</summary>
                ERROR_DS_PDC_OPERATION_IN_PROGRESS = 8490,
                ///<summary> A cross-domain move operation failed such that two versions of the moved object exist - one each in the source and destination domains.  The destination object needs to be removed to restore the system to a consistent state.</summary>
                ERROR_DS_CROSS_DOMAIN_CLEANUP_REQD = 8491,
                ///<summary> This object may not be moved across domain boundaries either because cross-domain moves for this class are disallowed, or the object has some special characteristics, e.g.: trust account or restricted RID, which prevent its move.</summary>
                ERROR_DS_ILLEGAL_XDOM_MOVE_OPERATION = 8492,
                ///<summary> Can't move objects with memberships across domain boundaries as once moved, this would violate the membership conditions of the account group.  Remove the object from any account group memberships and retry.</summary>
                ERROR_DS_CANT_WITH_ACCT_GROUP_MEMBERSHPS = 8493,
                ///<summary> A naming context head must be the immediate child of another naming context head, not of an interior node.</summary>
                ERROR_DS_NC_MUST_HAVE_NC_PARENT = 8494,
                ///<summary> The directory cannot validate the proposed naming context name because it does not hold a replica of the naming context above the proposed naming context.  Please ensure that the domain naming master role is held by a server that is configured as a global catalog server, and that the server is up to date with its replication partners. (Applies only to Windows 2000 Domain Naming masters)</summary>
                ERROR_DS_CR_IMPOSSIBLE_TO_VALIDATE = 8495,
                ///<summary> Destination domain must be in native mode.</summary>
                ERROR_DS_DST_DOMAIN_NOT_NATIVE = 8496,
                ///<summary> The operation can not be performed because the server does not have an infrastructure container in the domain of interest.</summary>
                ERROR_DS_MISSING_INFRASTRUCTURE_CONTAINER = 8497,
                ///<summary> Cross-domain move of non-empty account groups is not allowed.</summary>
                ERROR_DS_CANT_MOVE_ACCOUNT_GROUP = 8498,
                ///<summary> Cross-domain move of non-empty resource groups is not allowed.</summary>
                ERROR_DS_CANT_MOVE_RESOURCE_GROUP = 8499,
                ///<summary> The search flags for the attribute are invalid. The ANR bit is valid only on attributes of Unicode or Teletex strings.</summary>
                ERROR_DS_INVALID_SEARCH_FLAG = 8500,
                ///<summary> Tree deletions starting at an object which has an NC head as a descendant are not allowed.</summary>
                ERROR_DS_NO_TREE_DELETE_ABOVE_NC = 8501,
                ///<summary> The directory service failed to lock a tree in preparation for a tree deletion because the tree was in use.</summary>
                ERROR_DS_COULDNT_LOCK_TREE_FOR_DELETE = 8502,
                ///<summary> The directory service failed to identify the list of objects to delete while attempting a tree deletion.</summary>
                ERROR_DS_COULDNT_IDENTIFY_OBJECTS_FOR_TREE_DELETE = 8503,
                ///<summary> Security Accounts Manager initialization failed because of the following error: %1.
                ///Error Status: = 0x%2. Click OK to shut down the system and reboot into Directory Services Restore Mode. Check the event log for detailed information.</summary>
                ERROR_DS_SAM_INIT_FAILURE = 8504,
                ///<summary> Only an administrator can modify the membership list of an administrative group.</summary>
                ERROR_DS_SENSITIVE_GROUP_VIOLATION = 8505,
                ///<summary> Cannot change the primary group ID of a domain controller account.</summary>
                ERROR_DS_CANT_MOD_PRIMARYGROUPID = 8506,
                ///<summary> An attempt is made to modify the base schema.</summary>
                ERROR_DS_ILLEGAL_BASE_SCHEMA_MOD = 8507,
                ///<summary> Adding a new mandatory attribute to an existing class, deleting a mandatory attribute from an existing class, or adding an optional attribute to the special class Top that is not a backlink attribute (directly or through inheritance, for example, by adding or deleting an auxiliary class) is not allowed.</summary>
                ERROR_DS_NONSAFE_SCHEMA_CHANGE = 8508,
                ///<summary> Schema update is not allowed on this DC because the DC is not the schema FSMO Role Owner.</summary>
                ERROR_DS_SCHEMA_UPDATE_DISALLOWED = 8509,
                ///<summary> An object of this class cannot be created under the schema container. You can only create attribute-schema and class-schema objects under the schema container.</summary>
                ERROR_DS_CANT_CREATE_UNDER_SCHEMA = 8510,
                ///<summary> The replica/child install failed to get the objectVersion attribute on the schema container on the source DC. Either the attribute is missing on the schema container or the credentials supplied do not have permission to read it.</summary>
                ERROR_DS_INSTALL_NO_SRC_SCH_VERSION = 8511,
                ///<summary> The replica/child install failed to read the objectVersion attribute in the SCHEMA section of the file schema.ini in the system32 directory.</summary>
                ERROR_DS_INSTALL_NO_SCH_VERSION_IN_INIFILE = 8512,
                ///<summary> The specified group type is invalid.</summary>
                ERROR_DS_INVALID_GROUP_TYPE = 8513,
                ///<summary> You cannot nest global groups in a mixed domain if the group is security-enabled.</summary>
                ERROR_DS_NO_NEST_GLOBALGROUP_IN_MIXEDDOMAIN = 8514,
                ///<summary> You cannot nest local groups in a mixed domain if the group is security-enabled.</summary>
                ERROR_DS_NO_NEST_LOCALGROUP_IN_MIXEDDOMAIN = 8515,
                ///<summary> A global group cannot have a local group as a member.</summary>
                ERROR_DS_GLOBAL_CANT_HAVE_LOCAL_MEMBER = 8516,
                ///<summary> A global group cannot have a universal group as a member.</summary>
                ERROR_DS_GLOBAL_CANT_HAVE_UNIVERSAL_MEMBER = 8517,
                ///<summary> A universal group cannot have a local group as a member.</summary>
                ERROR_DS_UNIVERSAL_CANT_HAVE_LOCAL_MEMBER = 8518,
                ///<summary> A global group cannot have a cross-domain member.</summary>
                ERROR_DS_GLOBAL_CANT_HAVE_CROSSDOMAIN_MEMBER = 8519,
                ///<summary> A local group cannot have another cross domain local group as a member.</summary>
                ERROR_DS_LOCAL_CANT_HAVE_CROSSDOMAIN_LOCAL_MEMBER = 8520,
                ///<summary> A group with primary members cannot change to a security-disabled group.</summary>
                ERROR_DS_HAVE_PRIMARY_MEMBERS = 8521,
                ///<summary> The schema cache load failed to convert the string default SD on a class-schema object.</summary>
                ERROR_DS_STRING_SD_CONVERSION_FAILED = 8522,
                ///<summary> Only DSAs configured to be Global Catalog servers should be allowed to hold the Domain Naming Master FSMO role. (Applies only to Windows 2000 servers)</summary>
                ERROR_DS_NAMING_MASTER_GC = 8523,
                ///<summary> The DSA operation is unable to proceed because of a DNS lookup failure.</summary>
                ERROR_DS_DNS_LOOKUP_FAILURE = 8524,
                ///<summary> While processing a change to the DNS Host Name for an object, the Service Principal Name values could not be kept in sync.</summary>
                ERROR_DS_COULDNT_UPDATE_SPNS = 8525,
                ///<summary> The Security Descriptor attribute could not be read.</summary>
                ERROR_DS_CANT_RETRIEVE_SD = 8526,
                ///<summary> The object requested was not found, but an object with that key was found.</summary>
                ERROR_DS_KEY_NOT_UNIQUE = 8527,
                ///<summary> The syntax of the linked attribute being added is incorrect. Forward links can only have syntax 2.5.5.1, 2.5.5.7, and 2.5.5.14, and backlinks can only have syntax 2.5.5.1</summary>
                ERROR_DS_WRONG_LINKED_ATT_SYNTAX = 8528,
                ///<summary> Security Account Manager needs to get the boot password.</summary>
                ERROR_DS_SAM_NEED_BOOTKEY_PASSWORD = 8529,
                ///<summary> Security Account Manager needs to get the boot key from floppy disk.</summary>
                ERROR_DS_SAM_NEED_BOOTKEY_FLOPPY = 8530,
                ///<summary> Directory Service cannot start.</summary>
                ERROR_DS_CANT_START = 8531,
                ///<summary> Directory Services could not start.</summary>
                ERROR_DS_INIT_FAILURE = 8532,
                ///<summary> The connection between client and server requires packet privacy or better.</summary>
                ERROR_DS_NO_PKT_PRIVACY_ON_CONNECTION = 8533,
                ///<summary> The source domain may not be in the same forest as destination.</summary>
                ERROR_DS_SOURCE_DOMAIN_IN_FOREST = 8534,
                ///<summary> The destination domain must be in the forest.</summary>
                ERROR_DS_DESTINATION_DOMAIN_NOT_IN_FOREST = 8535,
                ///<summary> The operation requires that destination domain auditing be enabled.</summary>
                ERROR_DS_DESTINATION_AUDITING_NOT_ENABLED = 8536,
                ///<summary> The operation couldn't locate a DC for the source domain.</summary>
                ERROR_DS_CANT_FIND_DC_FOR_SRC_DOMAIN = 8537,
                ///<summary> The source object must be a group or user.</summary>
                ERROR_DS_SRC_OBJ_NOT_GROUP_OR_USER = 8538,
                ///<summary> The source object's SID already exists in destination forest.</summary>
                ERROR_DS_SRC_SID_EXISTS_IN_FOREST = 8539,
                ///<summary> The source and destination object must be of the same type.</summary>
                ERROR_DS_SRC_AND_DST_OBJECT_CLASS_MISMATCH = 8540,
                ///<summary> Security Accounts Manager initialization failed because of the following error: %1.
                ///Error Status: = 0x%2. Click OK to shut down the system and reboot into Safe Mode. Check the event log for detailed information.</summary>
                ERROR_SAM_INIT_FAILURE = 8541,
                ///<summary> Schema information could not be included in the replication request.</summary>
                ERROR_DS_DRA_SCHEMA_INFO_SHIP = 8542,
                ///<summary> The replication operation could not be completed due to a schema incompatibility.</summary>
                ERROR_DS_DRA_SCHEMA_CONFLICT = 8543,
                ///<summary> The replication operation could not be completed due to a previous schema incompatibility.</summary>
                ERROR_DS_DRA_EARLIER_SCHEMA_CONFLICT = 8544,
                ///<summary> The replication update could not be applied because either the source or the destination has not yet received information regarding a recent cross-domain move operation.</summary>
                ERROR_DS_DRA_OBJ_NC_MISMATCH = 8545,
                ///<summary> The requested domain could not be deleted because there exist domain controllers that still host this domain.</summary>
                ERROR_DS_NC_STILL_HAS_DSAS = 8546,
                ///<summary> The requested operation can be performed only on a global catalog server.</summary>
                ERROR_DS_GC_REQUIRED = 8547,
                ///<summary> A local group can only be a member of other local groups in the same domain.</summary>
                ERROR_DS_LOCAL_MEMBER_OF_LOCAL_ONLY = 8548,
                ///<summary> Foreign security principals cannot be members of universal groups.</summary>
                ERROR_DS_NO_FPO_IN_UNIVERSAL_GROUPS = 8549,
                ///<summary> The attribute is not allowed to be replicated to the GC because of security reasons.</summary>
                ERROR_DS_CANT_ADD_TO_GC = 8550,
                ///<summary> The checkpoint with the PDC could not be taken because there too many modifications being processed currently.</summary>
                ERROR_DS_NO_CHECKPOINT_WITH_PDC = 8551,
                ///<summary> The operation requires that source domain auditing be enabled.</summary>
                ERROR_DS_SOURCE_AUDITING_NOT_ENABLED = 8552,
                ///<summary> Security principal objects can only be created inside domain naming contexts.</summary>
                ERROR_DS_CANT_CREATE_IN_NONDOMAIN_NC = 8553,
                ///<summary> A Service Principal Name (SPN) could not be constructed because the provided hostname is not in the necessary format.</summary>
                ERROR_DS_INVALID_NAME_FOR_SPN = 8554,
                ///<summary> A Filter was passed that uses constructed attributes.</summary>
                ERROR_DS_FILTER_USES_CONTRUCTED_ATTRS = 8555,
                ///<summary> The unicodePwd attribute value must be enclosed in double quotes.</summary>
                ERROR_DS_UNICODEPWD_NOT_IN_QUOTES = 8556,
                ///<summary> Your computer could not be joined to the domain. You have exceeded the maximum number of computer accounts you are allowed to create in this domain. Contact your system administrator to have this limit reset or increased.</summary>
                ERROR_DS_MACHINE_ACCOUNT_QUOTA_EXCEEDED = 8557,
                ///<summary> For security reasons, the operation must be run on the destination DC.</summary>
                ERROR_DS_MUST_BE_RUN_ON_DST_DC = 8558,
                ///<summary> For security reasons, the source DC must be NT4SP4 or greater.</summary>
                ERROR_DS_SRC_DC_MUST_BE_SP4_OR_GREATER = 8559,
                ///<summary> Critical Directory Service System objects cannot be deleted during tree delete operations.  The tree delete may have been partially performed.</summary>
                ERROR_DS_CANT_TREE_DELETE_CRITICAL_OBJ = 8560,
                ///<summary> Directory Services could not start because of the following error: %1.
                ///Error Status: = 0x%2. Please click OK to shutdown the system. You can use the recovery console to diagnose the system further.</summary>
                ERROR_DS_INIT_FAILURE_CONSOLE = 8561,
                ///<summary> Security Accounts Manager initialization failed because of the following error: %1.
                ///Error Status: = 0x%2. Please click OK to shutdown the system. You can use the recovery console to diagnose the system further.</summary>
                ERROR_DS_SAM_INIT_FAILURE_CONSOLE = 8562,
                ///<summary> The version of the operating system installed is incompatible with the current forest functional level. You must upgrade to a new version of the operating system before this server can become a domain controller in this forest.</summary>
                ERROR_DS_FOREST_VERSION_TOO_HIGH = 8563,
                ///<summary> The version of the operating system installed is incompatible with the current domain functional level. You must upgrade to a new version of the operating system before this server can become a domain controller in this domain.</summary>
                ERROR_DS_DOMAIN_VERSION_TOO_HIGH = 8564,
                ///<summary> The version of the operating system installed on this server no longer supports the current forest functional level. You must raise the forest functional level before this server can become a domain controller in this forest.</summary>
                ERROR_DS_FOREST_VERSION_TOO_LOW = 8565,
                ///<summary> The version of the operating system installed on this server no longer supports the current domain functional level. You must raise the domain functional level before this server can become a domain controller in this domain.</summary>
                ERROR_DS_DOMAIN_VERSION_TOO_LOW = 8566,
                ///<summary> The version of the operating system installed on this server is incompatible with the functional level of the domain or forest.</summary>
                ERROR_DS_INCOMPATIBLE_VERSION = 8567,
                ///<summary> The functional level of the domain (or forest) cannot be raised to the requested value, because there exist one or more domain controllers in the domain (or forest) that are at a lower incompatible functional level.</summary>
                ERROR_DS_LOW_DSA_VERSION = 8568,
                ///<summary> The forest functional level cannot be raised to the requested value since one or more domains are still in mixed domain mode. All domains in the forest must be in native mode, for you to raise the forest functional level.</summary>
                ERROR_DS_NO_BEHAVIOR_VERSION_IN_MIXEDDOMAIN = 8569,
                ///<summary> The sort order requested is not supported.</summary>
                ERROR_DS_NOT_SUPPORTED_SORT_ORDER = 8570,
                ///<summary> The requested name already exists as a unique identifier.</summary>
                ERROR_DS_NAME_NOT_UNIQUE = 8571,
                ///<summary> The machine account was created pre-NT4.  The account needs to be recreated.</summary>
                ERROR_DS_MACHINE_ACCOUNT_CREATED_PRENT4 = 8572,
                ///<summary> The database is [Out ] of version store.</summary>
                ERROR_DS_OUT_OF_VERSION_STORE = 8573,
                ///<summary> Unable to continue operation because multiple conflicting controls were used.</summary>
                ERROR_DS_INCOMPATIBLE_CONTROLS_USED = 8574,
                ///<summary> Unable to find a valid security descriptor reference domain for this partition.</summary>
                ERROR_DS_NO_REF_DOMAIN = 8575,
                ///<summary> Schema update failed: The link identifier is reserved.</summary>
                ERROR_DS_RESERVED_LINK_ID = 8576,
                ///<summary> Schema update failed: There are no link identifiers available.</summary>
                ERROR_DS_LINK_ID_NOT_AVAILABLE = 8577,
                ///<summary> An account group can not have a universal group as a member.</summary>
                ERROR_DS_AG_CANT_HAVE_UNIVERSAL_MEMBER = 8578,
                ///<summary> Rename or move operations on naming context heads or read-only objects are not allowed.</summary>
                ERROR_DS_MODIFYDN_DISALLOWED_BY_INSTANCE_TYPE = 8579,
                ///<summary> Move operations on objects in the schema naming context are not allowed.</summary>
                ERROR_DS_NO_OBJECT_MOVE_IN_SCHEMA_NC = 8580,
                ///<summary> A system flag has been set on the object and does not allow the object to be moved or renamed.</summary>
                ERROR_DS_MODIFYDN_DISALLOWED_BY_FLAG = 8581,
                ///<summary> This object is not allowed to change its grandparent container. Moves are not forbidden on this object, but are restricted to sibling containers.</summary>
                ERROR_DS_MODIFYDN_WRONG_GRANDPARENT = 8582,
                ///<summary> Unable to resolve completely, a referral to another forest is generated.</summary>
                ERROR_DS_NAME_ERROR_TRUST_REFERRAL = 8583,
                ///<summary> The requested action is not supported on standard server.</summary>
                ERROR_NOT_SUPPORTED_ON_STANDARD_SERVER = 8584,
                ///<summary> Could not access a partition of the Active Directory located on a remote server.  Make sure at least one server is running for the partition in question.</summary>
                ERROR_DS_CANT_ACCESS_REMOTE_PART_OF_AD = 8585,
                ///<summary> The directory cannot validate the proposed naming context (or partition) name because it does not hold a replica nor can it contact a replica of the naming context above the proposed naming context.  Please ensure that the parent naming context is properly registered in DNS, and at least one replica of this naming context is reachable by the Domain Naming master.</summary>
                ERROR_DS_CR_IMPOSSIBLE_TO_VALIDATE_V2 = 8586,
                ///<summary> The thread limit for this request was exceeded.</summary>
                ERROR_DS_THREAD_LIMIT_EXCEEDED = 8587,
                ///<summary> The Global catalog server is not in the closest site.</summary>
                ERROR_DS_NOT_CLOSEST = 8588,
                ///<summary> The DS cannot derive a service principal name (SPN) with which to mutually authenticate the target server because the corresponding server object in the local DS database has no serverReference attribute.</summary>
                ERROR_DS_CANT_DERIVE_SPN_WITHOUT_SERVER_REF = 8589,
                ///<summary> The Directory Service failed to enter single user mode.</summary>
                ERROR_DS_SINGLE_USER_MODE_FAILED = 8590,
                ///<summary> The Directory Service cannot parse the script because of a syntax error.</summary>
                ERROR_DS_NTDSCRIPT_SYNTAX_ERROR = 8591,
                ///<summary> The Directory Service cannot process the script because of an error.</summary>
                ERROR_DS_NTDSCRIPT_PROCESS_ERROR = 8592,
                ///<summary> The directory service cannot perform the requested operation because the servers involved are of different replication epochs (which is usually related to a domain rename that is in progress).</summary>
                ERROR_DS_DIFFERENT_REPL_EPOCHS = 8593,
                ///<summary> The directory service binding must be renegotiated due to a change in the server extensions information.</summary>
                ERROR_DS_DRS_EXTENSIONS_CHANGED = 8594,
                ///<summary> Operation not allowed on a disabled cross ref.</summary>
                ERROR_DS_REPLICA_SET_CHANGE_NOT_ALLOWED_ON_DISABLED_CR = 8595,
                ///<summary> Schema update failed: No values for msDS-IntId are available.</summary>
                ERROR_DS_NO_MSDS_INTID = 8596,
                ///<summary> Schema update failed: Duplicate msDS-INtId. Retry the operation.</summary>
                ERROR_DS_DUP_MSDS_INTID = 8597,
                ///<summary> Schema deletion failed: attribute is used in rDNAttID.</summary>
                ERROR_DS_EXISTS_IN_RDNATTID = 8598,
                ///<summary> The directory service failed to authorize the request.</summary>
                ERROR_DS_AUTHORIZATION_FAILED = 8599,
                ///<summary> The Directory Service cannot process the script because it is invalid.</summary>
                ERROR_DS_INVALID_SCRIPT = 8600,
                ///<summary> The remote create cross reference operation failed on the Domain Naming Master FSMO.  The operation's error is in the extended data.</summary>
                ERROR_DS_REMOTE_CROSSREF_OP_FAILED = 8601,
                ///<summary> A cross reference is in use locally with the same name.</summary>
                ERROR_DS_CROSS_REF_BUSY = 8602,
                ///<summary> The DS cannot derive a service principal name (SPN) with which to mutually authenticate the target server because the server's domain has been deleted from the forest.</summary>
                ERROR_DS_CANT_DERIVE_SPN_FOR_DELETED_DOMAIN = 8603,
                ///<summary> Writeable NCs prevent this DC from demoting.</summary>
                ERROR_DS_CANT_DEMOTE_WITH_WRITEABLE_NC = 8604,
                ///<summary> The requested object has a non-unique identifier and cannot be retrieved.</summary>
                ERROR_DS_DUPLICATE_ID_FOUND = 8605,
                ///<summary> Insufficient attributes were given to create an object.  This object may not exist because it may have been deleted and already garbage collected.</summary>
                ERROR_DS_INSUFFICIENT_ATTR_TO_CREATE_OBJECT = 8606,
                ///<summary> The group cannot be converted due to attribute restrictions on the requested group type.</summary>
                ERROR_DS_GROUP_CONVERSION_ERROR = 8607,
                ///<summary> Cross-domain move of non-empty basic application groups is not allowed.</summary>
                ERROR_DS_CANT_MOVE_APP_BASIC_GROUP = 8608,
                ///<summary> Cross-domain move of non-empty query based application groups is not allowed.</summary>
                ERROR_DS_CANT_MOVE_APP_QUERY_GROUP = 8609,
                ///<summary> The FSMO role ownership could not be verified because its directory partition has not replicated successfully with atleast one replication partner. </summary>
                ERROR_DS_ROLE_NOT_VERIFIED = 8610,
                ///<summary> The target container for a redirection of a well known object container cannot already be a special container.</summary>
                ERROR_DS_WKO_CONTAINER_CANNOT_BE_SPECIAL = 8611,
                ///<summary> The Directory Service cannot perform the requested operation because a domain rename operation is in progress.</summary>
                ERROR_DS_DOMAIN_RENAME_IN_PROGRESS = 8612,
                ///<summary> The Active Directory detected an Active Directory child partition below the requested new partition name.  The Active Directory's partition heiarchy must be created in a top down method.</summary>
                ERROR_DS_EXISTING_AD_CHILD_NC = 8613,
                ///<summary> The Active Directory cannot replicate with this server because the time since the last replication with this server has exceeded the tombstone lifetime.</summary>
                ERROR_DS_REPL_LIFETIME_EXCEEDED = 8614,
                ///<summary> The requested operation is not allowed on an object under the system container.</summary>
                ERROR_DS_DISALLOWED_IN_SYSTEM_CONTAINER = 8615,
                ///<summary> The LDAP servers network send queue has filled up because the client is not processing the results of it's requests fast enough.
                ///No more requests will be processed until the client catches up.  If the client does not catch up then it will be disconnected.</summary>
                ERROR_DS_LDAP_SEND_QUEUE_FULL = 8616,

                #endregion


                #endregion

                #region DNS Error Codes (9000 to 9999)


                // Facility DNS Error Messages



                /*


			DNS_ERROR_RESPONSE_CODES_BASE =9000,

			DNS_ERROR_RCODE_NO_ERROR =NO_ERROR	,

	DNS_ERROR_MASK = 0x00002328 ,
				//' 9000 or DNS_ERROR_RESPONSE_CODES_BASE
			// DNS_ERROR_RCODE_FORMAT_ERROR =  0x00002329

			///<summary> DNS server unable to interpret format.</summary>
	DNS_ERROR_RCODE_FORMAT_ERROR = 9001L
			// DNS_ERROR_RCODE_SERVER_FAILURE = = 0x0000232a

			///<summary> DNS server failure.</summary>
	DNS_ERROR_RCODE_SERVER_FAILURE = 9002,
			 DNS_ERROR_RCODE_NAME_ERROR =  0x0000232b,

			///<summary> DNS name does not exist.</summary>
	DNS_ERROR_RCODE_NAME_ERROR = 9003L
			// DNS_ERROR_RCODE_NOT_IMPLEMENTED = = 0x0000232c

			///<summary> DNS request not supported by name server.</summary>
	DNS_ERROR_RCODE_NOT_IMPLEMENTED  9004L
			// DNS_ERROR_RCODE_REFUSED = = 0x0000232d

			///<summary> DNS operation refused.</summary>
	DNS_ERROR_RCODE_REFUSED = 9005L
			// DNS_ERROR_RCODE_YXDOMAIN = = 0x0000232e

			///<summary> DNS name that ought not exist, does exist.</summary>
	DNS_ERROR_RCODE_YXDOMAIN = 9006L
			// DNS_ERROR_RCODE_YXRRSET = = 0x0000232f

			///<summary> DNS RR set that ought not exist, does exist.</summary>
	DNS_ERROR_RCODE_YXRRSET = 9007L
			// DNS_ERROR_RCODE_NXRRSET = = 0x00002330

			///<summary> DNS RR set that ought to exist, does not exist.</summary>
	DNS_ERROR_RCODE_NXRRSET = 9008L
			// DNS_ERROR_RCODE_NOTAUTH = = 0x00002331

			///<summary> DNS server not authoritative for zone.</summary>
	DNS_ERROR_RCODE_NOTAUTH = 9009L
			// DNS_ERROR_RCODE_NOTZONE = = 0x00002332

			///<summary> DNS name in update or prereq is not in zone.</summary>
	DNS_ERROR_RCODE_NOTZONE = 9010L
			// DNS_ERROR_RCODE_BADSIG = = 0x00002338

			///<summary> DNS signature failed to verify.</summary>
	DNS_ERROR_RCODE_BADSIG = 9016L
			// DNS_ERROR_RCODE_BADKEY = = 0x00002339

			///<summary> DNS bad key.</summary>
	DNS_ERROR_RCODE_BADKEY = 9017L
			// DNS_ERROR_RCODE_BADTIME = = 0x0000233a

			///<summary> DNS signature validity expired.</summary>
	DNS_ERROR_RCODE_BADTIME = 9018L </ summary >
	DNS_ERROR_RCODE_LAST DNS_ERROR_RCODE_BADTIME




			//  Packet format
	</summary>
	DNS_ERROR_PACKET_FMT_BASE 9500
			// DNS_INFO_NO_RECORDS = = 0x0000251d

			///<summary> No records found for given DNS query.</summary>
	DNS_INFO_NO_RECORDS = 9501L
			// DNS_ERROR_BAD_PACKET = = 0x0000251e

			///<summary> Bad DNS packet.</summary>
	DNS_ERROR_BAD_PACKET = 9502L
			// DNS_ERROR_NO_PACKET = = 0x0000251f

			///<summary> No DNS packet.</summary>
	DNS_ERROR_NO_PACKET = 9503L
			// DNS_ERROR_RCODE = = 0x00002520

			///<summary> DNS error, check rcode.</summary>
	DNS_ERROR_RCODE = 9504L
			// DNS_ERROR_UNSECURE_PACKET = = 0x00002521

			///<summary> Unsecured DNS packet.</summary>
	DNS_ERROR_UNSECURE_PACKET = 9505L </ summary >
	DNS_STATUS_PACKET_UNSECURE DNS_ERROR_UNSECURE_PACKET

			//  General API errors
	</summary>
	DNS_ERROR_NO_MEMORY = ERROR_OUTOFMEMORY
			//</summary>
	DNS_ERROR_INVALID_NAME = ERROR_INVALID_NAME
			//</summary>
	DNS_ERROR_INVALID_DATA = ERROR_INVALID_DATA </ summary >
	DNS_ERROR_GENERAL_API_BASE 9550
			// DNS_ERROR_INVALID_TYPE = = 0x0000254f

			///<summary> Invalid DNS type.</summary>
	DNS_ERROR_INVALID_TYPE = 9551L
			// DNS_ERROR_INVALID_IP_ADDRESS = = 0x00002550

			///<summary> Invalid IP address.</summary>
	DNS_ERROR_INVALID_IP_ADDRESS = 9552L
			// DNS_ERROR_INVALID_PROPERTY = = 0x00002551

			///<summary> Invalid property.</summary>
	DNS_ERROR_INVALID_PROPERTY = 9553L
			// DNS_ERROR_TRY_AGAIN_LATER = = 0x00002552

			///<summary> Try DNS operation again later.</summary>
	DNS_ERROR_TRY_AGAIN_LATER = 9554L
			// DNS_ERROR_NOT_UNIQUE = = 0x00002553

			///<summary> Record for given name and type is not unique.</summary>
	DNS_ERROR_NOT_UNIQUE = 9555L
			// DNS_ERROR_NON_RFC_NAME = = 0x00002554

			///<summary> DNS name does not comply with RFC specifications.</summary>
	DNS_ERROR_NON_RFC_NAME = 9556L
			// DNS_STATUS_FQDN = = 0x00002555

			///<summary> DNS name is a fully-qualified DNS name.</summary>
	DNS_STATUS_FQDN = 9557L
			// DNS_STATUS_DOTTED_NAME = = 0x00002556

			///<summary> DNS name is dotted (multi-label).</summary>
	DNS_STATUS_DOTTED_NAME = 9558L
			// DNS_STATUS_SINGLE_PART_NAME = = 0x00002557

			///<summary> DNS name is a single-part name.</summary>
	DNS_STATUS_SINGLE_PART_NAME = 9559L
			// DNS_ERROR_INVALID_NAME_CHAR = = 0x00002558

			///<summary> DNS name contains an invalid character.</summary>
	DNS_ERROR_INVALID_NAME_CHAR = 9560L
			// DNS_ERROR_NUMERIC_NAME = = 0x00002559

			///<summary> DNS name is entirely numeric.</summary>
	DNS_ERROR_NUMERIC_NAME = 9561L
			// DNS_ERROR_NOT_ALLOWED_ON_ROOT_SERVER  = 0x0000255A

			///<summary> The operation requested is not permitted on a DNS root server.</summary>
	DNS_ERROR_NOT_ALLOWED_ON_ROOT_SERVER 9562L
			// DNS_ERROR_NOT_ALLOWED_UNDER_DELEGATION  = 0x0000255B

			///<summary> The record could not be created because this part of the DNS namespace has
			//  been delegated to another server.</summary>
	DNS_ERROR_NOT_ALLOWED_UNDER_DELEGATION 9563L
			// DNS_ERROR_CANNOT_FIND_ROOT_HINTS  = 0x0000255C

			///<summary> The DNS server could not find a set of root hints.</summary>
	DNS_ERROR_CANNOT_FIND_ROOT_HINTS 9564L
			// DNS_ERROR_INCONSISTENT_ROOT_HINTS  = 0x0000255D

			///<summary> The DNS server found root hints but they were not consistent across
			//  all adapters.</summary>
	DNS_ERROR_INCONSISTENT_ROOT_HINTS 9565 ,
			//  Zone errors
	</summary>
	DNS_ERROR_ZONE_BASE 9600
			// DNS_ERROR_ZONE_DOES_NOT_EXIST = = 0x00002581

			///<summary> DNS zone does not exist.</summary>
	DNS_ERROR_ZONE_DOES_NOT_EXIST = 9601L
			// DNS_ERROR_NO_ZONE_INFO = = 0x00002582

			///<summary> DNS zone information not available.</summary>
	DNS_ERROR_NO_ZONE_INFO = 9602L
			// DNS_ERROR_INVALID_ZONE_OPERATION = = 0x00002583

			///<summary> Invalid operation for DNS zone.</summary>
	DNS_ERROR_INVALID_ZONE_OPERATION 9603L
			// DNS_ERROR_ZONE_CONFIGURATION_ERROR = = 0x00002584

			///<summary> Invalid DNS zone configuration.</summary>
	DNS_ERROR_ZONE_CONFIGURATION_ERROR 9604L
			// DNS_ERROR_ZONE_HAS_NO_SOA_RECORD = = 0x00002585

			///<summary> DNS zone has no start of authority (SOA) record.</summary>
	DNS_ERROR_ZONE_HAS_NO_SOA_RECORD 9605L
			// DNS_ERROR_ZONE_HAS_NO_NS_RECORDS = = 0x00002586

			///<summary> DNS zone has no Name Server (NS) record.</summary>
	DNS_ERROR_ZONE_HAS_NO_NS_RECORDS 9606L
			// DNS_ERROR_ZONE_LOCKED = = 0x00002587

			///<summary> DNS zone is locked.</summary>
	DNS_ERROR_ZONE_LOCKED = 9607L
			// DNS_ERROR_ZONE_CREATION_FAILED = = 0x00002588

			///<summary> DNS zone creation failed.</summary>
	DNS_ERROR_ZONE_CREATION_FAILED = 9608L
			// DNS_ERROR_ZONE_ALREADY_EXISTS = = 0x00002589

			///<summary> DNS zone already exists.</summary>
	DNS_ERROR_ZONE_ALREADY_EXISTS = 9609L
			// DNS_ERROR_AUTOZONE_ALREADY_EXISTS = = 0x0000258a

			///<summary> DNS automatic zone already exists.</summary>
	DNS_ERROR_AUTOZONE_ALREADY_EXISTS 9610L
			// DNS_ERROR_INVALID_ZONE_TYPE = = 0x0000258b

			///<summary> Invalid DNS zone type.</summary>
	DNS_ERROR_INVALID_ZONE_TYPE = 9611L
			// DNS_ERROR_SECONDARY_REQUIRES_MASTER_IP = 0x0000258c

			///<summary> Secondary DNS zone requires master IP address.</summary>
	DNS_ERROR_SECONDARY_REQUIRES_MASTER_IP 9612L
			// DNS_ERROR_ZONE_NOT_SECONDARY = = 0x0000258d

			///<summary> DNS zone not secondary.</summary>
	DNS_ERROR_ZONE_NOT_SECONDARY = 9613L
			// DNS_ERROR_NEED_SECONDARY_ADDRESSES = = 0x0000258e

			///<summary> Need secondary IP address.</summary>
	DNS_ERROR_NEED_SECONDARY_ADDRESSES 9614L
			// DNS_ERROR_WINS_INIT_FAILED = = 0x0000258f

			///<summary> WINS initialization failed.</summary>
	DNS_ERROR_WINS_INIT_FAILED = 9615L
			// DNS_ERROR_NEED_WINS_SERVERS = = 0x00002590

			///<summary> Need WINS servers.</summary>
	DNS_ERROR_NEED_WINS_SERVERS = 9616L
			// DNS_ERROR_NBSTAT_INIT_FAILED = = 0x00002591

			///<summary> NBTSTAT initialization call failed.</summary>
	DNS_ERROR_NBSTAT_INIT_FAILED = 9617L
			// DNS_ERROR_SOA_DELETE_INVALID = = 0x00002592

			///<summary> Invalid delete of start of authority (SOA)</summary>
	DNS_ERROR_SOA_DELETE_INVALID = 9618L
			// DNS_ERROR_FORWARDER_ALREADY_EXISTS = = 0x00002593

			///<summary> A conditional forwarding zone already exists for that name.</summary>
	DNS_ERROR_FORWARDER_ALREADY_EXISTS 9619L
			// DNS_ERROR_ZONE_REQUIRES_MASTER_IP = = 0x00002594

			///<summary> This zone must be configured with one or more master DNS server IP addresses.</summary>
	DNS_ERROR_ZONE_REQUIRES_MASTER_IP 9620L
			// DNS_ERROR_ZONE_IS_SHUTDOWN = = 0x00002595

			///<summary> The operation cannot be performed because this zone is shutdown.</summary>
	DNS_ERROR_ZONE_IS_SHUTDOWN = 9621,
			//  Datafile errors
	</summary>
	DNS_ERROR_DATAFILE_BASE 9650
			// DNS = = 0x000025b3

			///<summary> Primary DNS zone requires datafile.</summary>
	DNS_ERROR_PRIMARY_REQUIRES_DATAFILE 9651L
			// DNS = = 0x000025b4

			///<summary> Invalid datafile name for DNS zone.</summary>
	DNS_ERROR_INVALID_DATAFILE_NAME  9652L
			// DNS = = 0x000025b5

			///<summary> Failed to open datafile for DNS zone.</summary>
	DNS_ERROR_DATAFILE_OPEN_FAILURE  9653L
			// DNS = = 0x000025b6

			///<summary> Failed to write datafile for DNS zone.</summary>
	DNS_ERROR_FILE_WRITEBACK_FAILED  9654L
			// DNS = = 0x000025b7

			///<summary> Failure while reading datafile for DNS zone.</summary>
	DNS_ERROR_DATAFILE_PARSING = 9655,
			//  Database errors
	</summary>
	DNS_ERROR_DATABASE_BASE 9700
			// DNS_ERROR_RECORD_DOES_NOT_EXIST = = 0x000025e5

			///<summary> DNS record does not exist.</summary>
	DNS_ERROR_RECORD_DOES_NOT_EXIST  9701L
			// DNS_ERROR_RECORD_FORMAT = = 0x000025e6

			///<summary> DNS record format error.</summary>
	DNS_ERROR_RECORD_FORMAT = 9702L
			// DNS_ERROR_NODE_CREATION_FAILED = = 0x000025e7

			///<summary> Node creation failure in DNS.</summary>
	DNS_ERROR_NODE_CREATION_FAILED = 9703L
			// DNS_ERROR_UNKNOWN_RECORD_TYPE = = 0x000025e8

			///<summary> Unknown DNS record type.</summary>
	DNS_ERROR_UNKNOWN_RECORD_TYPE = 9704L
			// DNS_ERROR_RECORD_TIMED_OUT = = 0x000025e9

			///<summary> DNS record timed out.</summary>
	DNS_ERROR_RECORD_TIMED_OUT = 9705L
			// DNS_ERROR_NAME_NOT_IN_ZONE = = 0x000025ea

			///<summary> Name not in DNS zone.</summary>
	DNS_ERROR_NAME_NOT_IN_ZONE = 9706L
			// DNS_ERROR_CNAME_LOOP = = 0x000025eb

			///<summary> CNAME loop detected.</summary>
	DNS_ERROR_CNAME_LOOP = 9707L
			// DNS_ERROR_NODE_IS_CNAME = = 0x000025ec

			///<summary> Node is a CNAME DNS record.</summary>
	DNS_ERROR_NODE_IS_CNAME = 9708L
			// DNS_ERROR_CNAME_COLLISION = = 0x000025ed

			///<summary> A CNAME record already exists for given name.</summary>
	DNS_ERROR_CNAME_COLLISION = 9709L
			// DNS_ERROR_RECORD_ONLY_AT_ZONE_ROOT = = 0x000025ee

			///<summary> Record only at DNS zone root.</summary>
	DNS_ERROR_RECORD_ONLY_AT_ZONE_ROOT 9710L
			// DNS_ERROR_RECORD_ALREADY_EXISTS = = 0x000025ef

			///<summary> DNS record already exists.</summary>
	DNS_ERROR_RECORD_ALREADY_EXISTS  9711L
			// DNS_ERROR_SECONDARY_DATA = = 0x000025f0

			///<summary> Secondary DNS zone data error.</summary>
	DNS_ERROR_SECONDARY_DATA = 9712L
			// DNS_ERROR_NO_CREATE_CACHE_DATA = = 0x000025f1

			///<summary> Could not create DNS cache data.</summary>
	DNS_ERROR_NO_CREATE_CACHE_DATA = 9713L
			// DNS_ERROR_NAME_DOES_NOT_EXIST = = 0x000025f2

			///<summary> DNS name does not exist.</summary>
	DNS_ERROR_NAME_DOES_NOT_EXIST = 9714L
			// DNS_WARNING_PTR_CREATE_FAILED = = 0x000025f3

			///<summary> Could not create pointer (PTR) record.</summary>
	DNS_WARNING_PTR_CREATE_FAILED = 9715L
			// DNS_WARNING_DOMAIN_UNDELETED = = 0x000025f4

			///<summary> DNS domain was undeleted.</summary>
	DNS_WARNING_DOMAIN_UNDELETED = 9716L
			// DNS_ERROR_DS_UNAVAILABLE = = 0x000025f5

			///<summary> The directory service is unavailable.</summary>
	DNS_ERROR_DS_UNAVAILABLE = 9717L
			// DNS_ERROR_DS_ZONE_ALREADY_EXISTS = = 0x000025f6

			///<summary> DNS zone already exists in the directory service.</summary>
	DNS_ERROR_DS_ZONE_ALREADY_EXISTS 9718L
			// DNS_ERROR_NO_BOOTFILE_IF_DS_ZONE = = 0x000025f7

			///<summary> DNS server not creating or reading the boot file for the directory service integrated DNS zone.</summary>
	DNS_ERROR_NO_BOOTFILE_IF_DS_ZONE 9719 ,
			//  Operation errors
	</summary>
	DNS_ERROR_OPERATION_BASE 9750
			// DNS_INFO_AXFR_COMPLETE = = 0x00002617

			///<summary> DNS AXFR (zone transfer) complete.</summary>
	DNS_INFO_AXFR_COMPLETE = 9751L
			// DNS_ERROR_AXFR = = 0x00002618

			///<summary> DNS zone transfer failed.</summary>
	DNS_ERROR_AXFR = 9752L
			// DNS_INFO_ADDED_LOCAL_WINS = = 0x00002619

			///<summary> Added local WINS server.</summary>
	DNS_INFO_ADDED_LOCAL_WINS = 9753,
			//  Secure update
	</summary>
	DNS_ERROR_SECURE_BASE 9800
			// DNS_STATUS_CONTINUE_NEEDED = = 0x00002649

			///<summary> Secure update call needs to continue update request.</summary>
	DNS_STATUS_CONTINUE_NEEDED = 9801,
			//  Setup errors
	</summary>
	DNS_ERROR_SETUP_BASE 9850
			// DNS_ERROR_NO_TCPIP = = 0x0000267b

			///<summary> TCP/IP network protocol not installed.</summary>
	DNS_ERROR_NO_TCPIP = 9851L
			// DNS_ERROR_NO_DNS_SERVERS = = 0x0000267c

			///<summary> No DNS servers configured for local system.</summary>
	DNS_ERROR_NO_DNS_SERVERS = 9852,
			//  Directory partition (DP) errors
	</summary>
	DNS_ERROR_DP_BASE 9900
			// DNS_ERROR_DP_DOES_NOT_EXIST = = 0x000026ad

			///<summary> The specified directory partition does not exist.</summary>
	DNS_ERROR_DP_DOES_NOT_EXIST = 9901L
			// DNS_ERROR_DP_ALREADY_EXISTS = = 0x000026ae

			///<summary> The specified directory partition already exists.</summary>
	DNS_ERROR_DP_ALREADY_EXISTS = 9902L
			// DNS_ERROR_DP_NOT_ENLISTED = = 0x000026af

			///<summary> This DNS server is not enlisted in the specified directory partition.</summary>
	DNS_ERROR_DP_NOT_ENLISTED = 9903L
			// DNS_ERROR_DP_ALREADY_ENLISTED = = 0x000026b0

			///<summary> This DNS server is already enlisted in the specified directory partition.</summary>
	DNS_ERROR_DP_ALREADY_ENLISTED = 9904L
			// DNS_ERROR_DP_NOT_AVAILABLE = = 0x000026b1

			///<summary> The directory partition is not available at this time. Please wait
			//  a few minutes and try again.</summary>
	DNS_ERROR_DP_NOT_AVAILABLE = 9905L
			// '/
			// = '
			// = End of DNS Error Codes = '
			// = '
			// = 9000 to 9999 = '
			// '/

				*/

                #endregion

                #region WinSock Error Codes (10000 to 11999) (WinSock error codes are also defined in WinSock.h and WinSock2.h, hence the IFDEF)

                WSABASEERR = 10000,

                ///<summary> A blocking operation was interrupted by a call to WSACancelBlockingCall.</summary>
                WSAEINTR = 10004,
                ///<summary> The file handle supplied is not valid.</summary>
                WSAEBADF = 10009,
                ///<summary> An attempt was made to access a socket in a way forbidden by its access permissions.</summary>
                WSAEACCES = 10013,
                ///<summary> The system detected an invalid pointer address in attempting to use a pointer argument in a call.</summary>
                WSAEFAULT = 10014,
                ///<summary> An invalid argument was supplied.</summary>
                WSAEINVAL = 10022,
                ///<summary> Too many open sockets.</summary>
                WSAEMFILE = 10024,
                ///<summary> A non-blocking socket operation could not be completed immediately.</summary>
                WSAEWOULDBLOCK = 10035,
                ///<summary> A blocking operation is currently erunuting.</summary>
                WSAEINPROGRESS = 10036,
                ///<summary> An operation was attempted on a non-blocking socket that already had an operation in progress.</summary>
                WSAEALREADY = 10037,
                ///<summary> An operation was attempted on something that is not a socket.</summary>
                WSAENOTSOCK = 10038,
                ///<summary> A required address was omitted from an operation on a socket.</summary>
                WSAEDESTADDRREQ = 10039,
                ///<summary> A message sent on a datagram socket was larger than the internal message buffer or some other network limit, or the buffer used to receive a datagram into was smaller than the datagram itself.</summary>
                WSAEMSGSIZE = 10040,
                ///<summary> A protocol was specified in the socket function call that does not support the semantics of the socket type requested.</summary>
                WSAEPROTOTYPE = 10041,
                ///<summary> An unknown, invalid, or unsupported option or level was specified in a getsockopt or setsockopt call.</summary>
                WSAENOPROTOOPT = 10042,
                ///<summary> The requested protocol has not been configured into the system, or no implementation for it exists.</summary>
                WSAEPROTONOSUPPORT = 10043,
                ///<summary> The support for the specified socket type does not exist in this address family.</summary>
                WSAESOCKTNOSUPPORT = 10044,
                ///<summary> The attempted operation is not supported for the type of object referenced.</summary>
                WSAEOPNOTSUPP = 10045,
                ///<summary> The protocol family has not been configured into the system or no implementation for it exists.</summary>
                WSAEPFNOSUPPORT = 10046,
                ///<summary> An address incompatible with the requested protocol was used.</summary>
                WSAEAFNOSUPPORT = 10047,
                ///<summary> Only one usage of each socket address (protocol/network address/port) is normally permitted.</summary>
                WSAEADDRINUSE = 10048,
                ///<summary> The requested address is not valid in its context.</summary>
                WSAEADDRNOTAVAIL = 10049,
                ///<summary> A socket operation encountered a dead network.</summary>
                WSAENETDOWN = 10050,
                ///<summary> A socket operation was attempted to an unreachable network.</summary>
                WSAENETUNREACH = 10051,
                ///<summary> The connection has been broken due to keep-alive activity detecting a failure while the operation was in progress.</summary>
                WSAENETRESET = 10052,
                ///<summary> An established connection was aborted by the software in your host machine.</summary>
                WSAECONNABORTED = 10053,
                ///<summary> An existing connection was forcibly closed by the remote host.</summary>
                WSAECONNRESET = 10054,
                ///<summary> An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full.</summary>
                WSAENOBUFS = 10055,
                ///<summary> A connect request was made on an already connected socket.</summary>
                WSAEISCONN = 10056,
                ///<summary> A request to send or receive data was disallowed because the socket is not connected and (when sending on a datagram socket using a sendto call) no address was supplied.</summary>
                WSAENOTCONN = 10057,
                ///<summary> A request to send or receive data was disallowed because the socket had already been shut down in that direction with a previous shutdown call.</summary>
                WSAESHUTDOWN = 10058,
                ///<summary> Too many references to some kernel object.</summary>
                WSAETOOMANYREFS = 10059,
                ///<summary> A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.</summary>
                WSAETIMEDOUT = 10060,
                ///<summary> No connection could be made because the target machine actively refused it.</summary>
                WSAECONNREFUSED = 10061,
                ///<summary> Cannot translate name.</summary>
                WSAELOOP = 10062,
                ///<summary> Name component or name was too long.</summary>
                WSAENAMETOOLONG = 10063,
                ///<summary> A socket operation failed because the destination host was down.</summary>
                WSAEHOSTDOWN = 10064,
                ///<summary> A socket operation was attempted to an unreachable host.</summary>
                WSAEHOSTUNREACH = 10065,
                ///<summary> Cannot remove a directory that is not empty.</summary>
                WSAENOTEMPTY = 10066,
                ///<summary> A Windows Sockets implementation may have a limit on the number of applications that may use it simultaneously.</summary>
                WSAEPROCLIM = 10067,
                ///<summary> Ran [Out ] of quota.</summary>
                WSAEUSERS = 10068,
                ///<summary> Ran [Out ] of disk quota.</summary>
                WSAEDQUOT = 10069,
                ///<summary> File handle reference is no longer available.</summary>
                WSAESTALE = 10070,
                ///<summary> Item is not available locally.</summary>
                WSAEREMOTE = 10071,
                ///<summary> WSAStartup cannot function at this time because the underlying system it uses to provide network services is currently unavailable.</summary>
                WSASYSNOTREADY = 10091,
                ///<summary> The Windows Sockets version requested is not supported.</summary>
                WSAVERNOTSUPPORTED = 10092,
                ///<summary> Either the application has not called WSAStartup, or WSAStartup failed.</summary>
                WSANOTINITIALISED = 10093,
                ///<summary> Returned by WSARecv or WSARecvFrom to indicate the remote party has initiated a graceful shutdown sequence.</summary>
                WSAEDISCON = 10101,
                ///<summary> No more results can be returned by WSALookupServiceNext.</summary>
                WSAENOMORE = 10102,
                ///<summary> A call to WSALookupServiceEnd was made while this call was still processing. The call has been canceled.</summary>
                WSAECANCELLED = 10103,
                ///<summary> The procedure call table is invalid.</summary>
                WSAEINVALIDPROCTABLE = 10104,
                ///<summary> The requested service provider is invalid.</summary>
                WSAEINVALIDPROVIDER = 10105,
                ///<summary> The requested service provider could not be loaded or initialized.</summary>
                WSAEPROVIDERFAILEDINIT = 10106,
                ///<summary> A system call that should never fail has failed.</summary>
                WSASYSCALLFAILURE = 10107,
                ///<summary> No such service is known. The service cannot be found in the specified name space.</summary>
                WSASERVICE_NOT_FOUND = 10108,
                ///<summary> The specified class was not found.</summary>
                WSATYPE_NOT_FOUND = 10109,
                ///<summary> No more results can be returned by WSALookupServiceNext.</summary>
                WSA_E_NO_MORE = 10110,
                ///<summary> A call to WSALookupServiceEnd was made while this call was still processing. The call has been canceled.</summary>
                WSA_E_CANCELLED = 10111,
                ///<summary> A database query failed because it was actively refused.</summary>
                WSAEREFUSED = 10112,
                ///<summary> No such host is known.</summary>
                WSAHOST_NOT_FOUND = 11001,
                ///<summary> This is usually a temporary error during hostname resolution and means that the local server did not receive a response from an authoritative server.</summary>
                WSATRY_AGAIN = 11002,
                ///<summary> A non-recoverable error occurred during a database lookup.</summary>
                WSANO_RECOVERY = 11003,
                ///<summary> The requested name is valid, but no data of the requested type was found.</summary>
                WSANO_DATA = 11004,
                ///<summary> At least one reserve has arrived.</summary>
                WSA_QOS_RECEIVERS = 11005,
                ///<summary> At least one path has arrived.</summary>
                WSA_QOS_SENDERS = 11006,
                ///<summary> There are no senders.</summary>
                WSA_QOS_NO_SENDERS = 11007,
                ///<summary> There are no receivers.</summary>
                WSA_QOS_NO_RECEIVERS = 11008,
                ///<summary> Reserve has been confirmed.</summary>
                WSA_QOS_REQUEST_CONFIRMED = 11009,
                ///<summary> Error due to lack of resources.</summary>
                WSA_QOS_ADMISSION_FAILURE = 11010,
                ///<summary> Rejected for administrative reasons - bad credentials.</summary>
                WSA_QOS_POLICY_FAILURE = 11011,
                ///<summary> Unknown or conflicting style.</summary>
                WSA_QOS_BAD_STYLE = 11012,
                ///<summary> Problem with some part of the filterspec or providerspecific buffer in general.</summary>
                WSA_QOS_BAD_OBJECT = 11013,
                ///<summary> Problem with some part of the flowspec.</summary>
                WSA_QOS_TRAFFIC_CTRL_ERROR = 11014,
                ///<summary> General QOS error.</summary>
                WSA_QOS_GENERIC_ERROR = 11015,
                ///<summary> An invalid or unrecognized service type was found in the flowspec.</summary>
                WSA_QOS_ESERVICETYPE = 11016,
                ///<summary> An invalid or inconsistent flowspec was found in the QOS structure.</summary>
                WSA_QOS_EFLOWSPEC = 11017,
                ///<summary> Invalid QOS provider-specific buffer.</summary>
                WSA_QOS_EPROVSPECBUF = 11018,
                ///<summary> An invalid QOS filter style was used.</summary>
                WSA_QOS_EFILTERSTYLE = 11019,
                ///<summary> An invalid QOS filter type was used.</summary>
                WSA_QOS_EFILTERTYPE = 11020,
                ///<summary> An incorrect number of QOS FILTERSPECs were specified in the FLOWDESCRIPTOR.</summary>
                WSA_QOS_EFILTERCOUNT = 11021,
                ///<summary> An object with an invalid ObjectLength field was specified in the QOS provider-specific buffer.</summary>
                WSA_QOS_EOBJLENGTH = 11022,
                ///<summary> An incorrect number of flow descriptors was specified in the QOS structure.</summary>
                WSA_QOS_EFLOWCOUNT = 11023,
                ///<summary> An unrecognized object was found in the QOS provider-specific buffer.</summary>
                WSA_QOS_EUNKOWNPSOBJ = 11024,
                ///<summary> An invalid policy object was found in the QOS provider-specific buffer.</summary>
                WSA_QOS_EPOLICYOBJ = 11025,
                ///<summary> An invalid QOS flow descriptor was found in the flow descriptor list.</summary>
                WSA_QOS_EFLOWDESC = 11026,
                ///<summary> An invalid or inconsistent flowspec was found in the QOS provider specific buffer.</summary>
                WSA_QOS_EPSFLOWSPEC = 11027,
                ///<summary> An invalid FILTERSPEC was found in the QOS provider-specific buffer.</summary>
                WSA_QOS_EPSFILTERSPEC = 11028,
                ///<summary> An invalid shape discard mode object was found in the QOS provider specific buffer.</summary>
                WSA_QOS_ESDMODEOBJ = 11029,
                ///<summary> An invalid shaping rate object was found in the QOS provider-specific buffer.</summary>
                WSA_QOS_ESHAPERATEOBJ = 11030,
                ///<summary> A reserved policy element was found in the QOS provider-specific buffer.</summary>
                WSA_QOS_RESERVED_PETYPE = 11031,

                #endregion


                #region Side By Side Error Codes (14000 to 14999)


                ///<summary> The requested section was not present in the activation context.</summary>
                ERROR_SXS_SECTION_NOT_FOUND = 14000,
                ///<summary> This application has failed to start because the application configuration is incorrect. Reinstalling the application may fix this problem.</summary>
                ERROR_SXS_CANT_GEN_ACTCTX = 14001,
                ///<summary> The application binding data format is invalid.</summary>
                ERROR_SXS_INVALID_ACTCTXDATA_FORMAT = 14002,
                ///<summary> The referenced assembly is not installed on your system.</summary>
                ERROR_SXS_ASSEMBLY_NOT_FOUND = 14003,
                ///<summary> The manifest file does not begin with the required tag and format information.</summary>
                ERROR_SXS_MANIFEST_FORMAT_ERROR = 14004,
                ///<summary> The manifest file contains one or more syntax errors.</summary>
                ERROR_SXS_MANIFEST_PARSE_ERROR = 14005,
                ///<summary> The application attempted to activate a disabled activation context.</summary>
                ERROR_SXS_ACTIVATION_CONTEXT_DISABLED = 14006,
                ///<summary> The requested lookup key was not found in any active activation context.</summary>
                ERROR_SXS_KEY_NOT_FOUND = 14007,
                ///<summary> A component version required by the application conflicts with another component version already active.</summary>
                ERROR_SXS_VERSION_CONFLICT = 14008,
                ///<summary> The type requested activation context section does not match the query API used.</summary>
                ERROR_SXS_WRONG_SECTION_TYPE = 14009,
                ///<summary> Lack of system resources has required isolated activation to be disabled for the current thread of erunution.</summary>
                ERROR_SXS_THREAD_QUERIES_DISABLED = 14010,
                ///<summary> An attempt to set the process default activation context failed because the process default activation context was already set.</summary>
                ERROR_SXS_PROCESS_DEFAULT_ALREADY_SET = 14011,
                ///<summary> The encoding group identifier specified is not recognized.</summary>
                ERROR_SXS_UNKNOWN_ENCODING_GROUP = 14012,
                ///<summary> The encoding requested is not recognized.</summary>
                ERROR_SXS_UNKNOWN_ENCODING = 14013,
                ///<summary> The manifest contains a reference to an invalid URI.</summary>
                ERROR_SXS_INVALID_XML_NAMESPACE_URI = 14014,
                ///<summary> The application manifest contains a reference to a dependent assembly which is not installed</summary>
                ERROR_SXS_ROOT_MANIFEST_DEPENDENCY_NOT_INSTALLED = 14015,
                ///<summary> The manifest for an assembly used by the application has a reference to a dependent assembly which is not installed</summary>
                ERROR_SXS_LEAF_MANIFEST_DEPENDENCY_NOT_INSTALLED = 14016,
                ///<summary> The manifest contains an attribute for the assembly identity which is not valid.</summary>
                ERROR_SXS_INVALID_ASSEMBLY_IDENTITY_ATTRIBUTE = 14017,
                ///<summary> The manifest is missing the required default namespace specification on the assembly element.</summary>
                ERROR_SXS_MANIFEST_MISSING_REQUIRED_DEFAULT_NAMESPACE = 14018,
                ///<summary> The manifest has a default namespace specified on the assembly element but its value is not "urn:schemas-microsoft-com:asm.v1".</summary>
                ERROR_SXS_MANIFEST_INVALID_REQUIRED_DEFAULT_NAMESPACE = 14019,
                ///<summary> The private manifest probed has crossed reparse-point-associated path</summary>
                ERROR_SXS_PRIVATE_MANIFEST_CROSS_PATH_WITH_REPARSE_POINT = 14020,
                ///<summary> Two or more components referenced directly or indirectly by the application manifest have files by the same name.</summary>
                ERROR_SXS_DUPLICATE_DLL_NAME = 14021,
                ///<summary> Two or more components referenced directly or indirectly by the application manifest have window classes with the same name.</summary>
                ERROR_SXS_DUPLICATE_WINDOWCLASS_NAME = 14022,
                ///<summary> Two or more components referenced directly or indirectly by the application manifest have the same COM server CLSIDs.</summary>
                ERROR_SXS_DUPLICATE_CLSID = 14023,
                ///<summary> Two or more components referenced directly or indirectly by the application manifest have proxies for the same COM interface IIDs.</summary>
                ERROR_SXS_DUPLICATE_IID = 14024,
                ///<summary> Two or more components referenced directly or indirectly by the application manifest have the same COM type library TLBIDs.</summary>
                ERROR_SXS_DUPLICATE_TLBID = 14025,
                ///<summary> Two or more components referenced directly or indirectly by the application manifest have the same COM ProgIDs.</summary>
                ERROR_SXS_DUPLICATE_PROGID = 14026,
                ///<summary> Two or more components referenced directly or indirectly by the application manifest are different versions of the same component which is not permitted.</summary>
                ERROR_SXS_DUPLICATE_ASSEMBLY_NAME = 14027,
                ///<summary> A component's file does not match the verification information present in the component manifest.</summary>
                ERROR_SXS_FILE_HASH_MISMATCH = 14028,
                ///<summary> The policy manifest contains one or more syntax errors.</summary>
                ERROR_SXS_POLICY_PARSE_ERROR = 14029,
                ///<summary> Manifest Parse Error : A string literal was expected, but no opening quote character was found.</summary>
                ERROR_SXS_XML_E_MISSINGQUOTE = 14030,
                ///<summary> Manifest Parse Error : Incorrect syntax was used in a comment.</summary>
                ERROR_SXS_XML_E_COMMENTSYNTAX = 14031,
                ///<summary> Manifest Parse Error : A name was started with an invalid character.</summary>
                ERROR_SXS_XML_E_BADSTARTNAMECHAR = 14032,
                ///<summary> Manifest Parse Error : A name contained an invalid character.</summary>
                ERROR_SXS_XML_E_BADNAMECHAR = 14033,
                ///<summary> Manifest Parse Error : A string literal contained an invalid character.</summary>
                ERROR_SXS_XML_E_BADCHARINSTRING = 14034,
                ///<summary> Manifest Parse Error : Invalid syntax for an xml declaration.</summary>
                ERROR_SXS_XML_E_XMLDECLSYNTAX = 14035,
                ///<summary> Manifest Parse Error : An Invalid character was found in text content.</summary>
                ERROR_SXS_XML_E_BADCHARDATA = 14036,
                ///<summary> Manifest Parse Error : Required white space was missing.</summary>
                ERROR_SXS_XML_E_MISSINGWHITESPACE = 14037,
                ///<summary> Manifest Parse Error : The character '>' was expected.</summary>
                ERROR_SXS_XML_E_EXPECTINGTAGEND = 14038,
                ///<summary> Manifest Parse Error : A semi colon character was expected.</summary>
                ERROR_SXS_XML_E_MISSINGSEMICOLON = 14039,
                ///<summary> Manifest Parse Error : Unbalanced parentheses.</summary>
                ERROR_SXS_XML_E_UNBALANCEDPAREN = 14040,
                ///<summary> Manifest Parse Error : Internal error.</summary>
                ERROR_SXS_XML_E_INTERNALERROR = 14041,
                ///<summary> Manifest Parse Error : Whitespace is not allowed at this location.</summary>
                ERROR_SXS_XML_E_UNEXPECTED_WHITESPACE = 14042,
                ///<summary> Manifest Parse Error : End of file reached in invalid state for current encoding.</summary>
                ERROR_SXS_XML_E_INCOMPLETE_ENCODING = 14043,
                ///<summary> Manifest Parse Error : Missing parenthesis.</summary>
                ERROR_SXS_XML_E_MISSING_PAREN = 14044,
                ///<summary> Manifest Parse Error : A single or double closing quote character (\' or \") is missing.</summary>
                ERROR_SXS_XML_E_EXPECTINGCLOSEQUOTE = 14045,
                ///<summary> Manifest Parse Error : Multiple colons are not allowed in a name.</summary>
                ERROR_SXS_XML_E_MULTIPLE_COLONS = 14046,
                ///<summary> Manifest Parse Error : Invalid character for decimal digit.</summary>
                ERROR_SXS_XML_E_INVALID_DECIMAL = 14047,
                ///<summary> Manifest Parse Error : Invalid character for hexidecimal digit.</summary>
                ERROR_SXS_XML_E_INVALID_HEXIDECIMAL = 14048,
                ///<summary> Manifest Parse Error : Invalid unicode character value for this platform.</summary>
                ERROR_SXS_XML_E_INVALID_UNICODE = 14049,
                ///<summary> Manifest Parse Error : Expecting whitespace or '?'.</summary>
                ERROR_SXS_XML_E_WHITESPACEORQUESTIONMARK = 14050,
                ///<summary> Manifest Parse Error : End tag was not expected at this location.</summary>
                ERROR_SXS_XML_E_UNEXPECTEDENDTAG = 14051,
                ///<summary> Manifest Parse Error : The following tags were not closed: %1.</summary>
                ERROR_SXS_XML_E_UNCLOSEDTAG = 14052,
                ///<summary> Manifest Parse Error : Duplicate attribute.</summary>
                ERROR_SXS_XML_E_DUPLICATEATTRIBUTE = 14053,
                ///<summary> Manifest Parse Error : Only one top level element is allowed in an XML document.</summary>
                ERROR_SXS_XML_E_MULTIPLEROOTS = 14054,
                ///<summary> Manifest Parse Error : Invalid at the top level of the document.</summary>
                ERROR_SXS_XML_E_INVALIDATROOTLEVEL = 14055,
                ///<summary> Manifest Parse Error : Invalid xml declaration.</summary>
                ERROR_SXS_XML_E_BADXMLDECL = 14056,
                ///<summary> Manifest Parse Error : XML document must have a top level element.</summary>
                ERROR_SXS_XML_E_MISSINGROOT = 14057,
                ///<summary> Manifest Parse Error : Unexpected end of file.</summary>
                ERROR_SXS_XML_E_UNEXPECTEDEOF = 14058,
                ///<summary> Manifest Parse Error : Parameter entities cannot be used inside markup declarations in an internal subset.</summary>
                ERROR_SXS_XML_E_BADPEREFINSUBSET = 14059,
                ///<summary> Manifest Parse Error : Element was not closed.</summary>
                ERROR_SXS_XML_E_UNCLOSEDSTARTTAG = 14060,
                ///<summary> Manifest Parse Error : End element was missing the character '>'.</summary>
                ERROR_SXS_XML_E_UNCLOSEDENDTAG = 14061,
                ///<summary> Manifest Parse Error : A string literal was not closed.</summary>
                ERROR_SXS_XML_E_UNCLOSEDSTRING = 14062,
                ///<summary> Manifest Parse Error : A comment was not closed.</summary>
                ERROR_SXS_XML_E_UNCLOSEDCOMMENT = 14063,
                ///<summary> Manifest Parse Error : A declaration was not closed.</summary>
                ERROR_SXS_XML_E_UNCLOSEDDECL = 14064,
                ///<summary> Manifest Parse Error : A CDATA section was not closed.</summary>
                ERROR_SXS_XML_E_UNCLOSEDCDATA = 14065,
                ///<summary> Manifest Parse Error : The namespace prefix is not allowed to start with the reserved string "xml".</summary>
                ERROR_SXS_XML_E_RESERVEDNAMESPACE = 14066,
                ///<summary> Manifest Parse Error : System does not support the specified encoding.</summary>
                ERROR_SXS_XML_E_INVALIDENCODING = 14067,
                ///<summary> Manifest Parse Error : Switch from current encoding to specified encoding not supported.</summary>
                ERROR_SXS_XML_E_INVALIDSWITCH = 14068,
                ///<summary> Manifest Parse Error : The name 'xml' is reserved and must be lower case.</summary>
                ERROR_SXS_XML_E_BADXMLCASE = 14069,
                ///<summary> Manifest Parse Error : The standalone attribute must have the value 'yes' or 'no'.</summary>
                ERROR_SXS_XML_E_INVALID_STANDALONE = 14070,
                ///<summary> Manifest Parse Error : The standalone attribute cannot be used in external entities.</summary>
                ERROR_SXS_XML_E_UNEXPECTED_STANDALONE = 14071,
                ///<summary> Manifest Parse Error : Invalid version number.</summary>
                ERROR_SXS_XML_E_INVALID_VERSION = 14072,
                ///<summary> Manifest Parse Error : Missing equals sign between attribute and attribute value.</summary>
                ERROR_SXS_XML_E_MISSINGEQUALS = 14073,
                ///<summary> Assembly Protection Error : Unable to recover the specified assembly.</summary>
                ERROR_SXS_PROTECTION_RECOVERY_FAILED = 14074,
                ///<summary> Assembly Protection Error : The public key for an assembly was too short to be allowed.</summary>
                ERROR_SXS_PROTECTION_PUBLIC_KEY_TOO_SHORT = 14075,
                ///<summary> Assembly Protection Error : The catalog for an assembly is not valid, or does not match the assembly's manifest.</summary>
                ERROR_SXS_PROTECTION_CATALOG_NOT_VALID = 14076,
                ///<summary> An HRESULT could not be translated to a corresponding Win32 error code.</summary>
                ERROR_SXS_UNTRANSLATABLE_HRESULT = 14077,
                ///<summary> Assembly Protection Error : The catalog for an assembly is missing.</summary>
                ERROR_SXS_PROTECTION_CATALOG_FILE_MISSING = 14078,
                ///<summary> The supplied assembly identity is missing one or more attributes which must be present in this context.</summary>
                ERROR_SXS_MISSING_ASSEMBLY_IDENTITY_ATTRIBUTE = 14079,
                ///<summary> The supplied assembly identity has one or more attribute names that contain characters not permitted in XML names.</summary>
                ERROR_SXS_INVALID_ASSEMBLY_IDENTITY_ATTRIBUTE_NAME = 14080,

                #endregion


                #region Start of IPSec Error codes (13000 to 13999)

                ///<summary> The specified quick mode policy already exists.</summary>
                ERROR_IPSEC_QM_POLICY_EXISTS = 13000,
                ///<summary> The specified quick mode policy was not found.</summary>
                ERROR_IPSEC_QM_POLICY_NOT_FOUND = 13001,
                ///<summary> The specified quick mode policy is being used.</summary>
                ERROR_IPSEC_QM_POLICY_IN_USE = 13002,
                ///<summary> The specified main mode policy already exists.</summary>
                ERROR_IPSEC_MM_POLICY_EXISTS = 13003,
                ///<summary> The specified main mode policy was not found</summary>
                ERROR_IPSEC_MM_POLICY_NOT_FOUND = 13004,
                ///<summary> The specified main mode policy is being used.</summary>
                ERROR_IPSEC_MM_POLICY_IN_USE = 13005,
                ///<summary> The specified main mode filter already exists.</summary>
                ERROR_IPSEC_MM_FILTER_EXISTS = 13006,
                ///<summary> The specified main mode filter was not found.</summary>
                ERROR_IPSEC_MM_FILTER_NOT_FOUND = 13007,
                ///<summary> The specified transport mode filter already exists.</summary>
                ERROR_IPSEC_TRANSPORT_FILTER_EXISTS = 13008,
                ///<summary> The specified transport mode filter does not exist.</summary>
                ERROR_IPSEC_TRANSPORT_FILTER_NOT_FOUND = 13009,
                ///<summary> The specified main mode authentication list exists.</summary>
                ERROR_IPSEC_MM_AUTH_EXISTS = 13010,
                ///<summary> The specified main mode authentication list was not found.</summary>
                ERROR_IPSEC_MM_AUTH_NOT_FOUND = 13011,
                ///<summary> The specified quick mode policy is being used.</summary>
                ERROR_IPSEC_MM_AUTH_IN_USE = 13012,
                ///<summary> The specified main mode policy was not found.</summary>
                ERROR_IPSEC_DEFAULT_MM_POLICY_NOT_FOUND = 13013,
                ///<summary> The specified quick mode policy was not found</summary>
                ERROR_IPSEC_DEFAULT_MM_AUTH_NOT_FOUND = 13014,
                ///<summary> The manifest file contains one or more syntax errors.</summary>
                ERROR_IPSEC_DEFAULT_QM_POLICY_NOT_FOUND = 13015,
                ///<summary> The application attempted to activate a disabled activation context.</summary>
                ERROR_IPSEC_TUNNEL_FILTER_EXISTS = 13016,
                ///<summary> The requested lookup key was not found in any active activation context.</summary>
                ERROR_IPSEC_TUNNEL_FILTER_NOT_FOUND = 13017,
                ///<summary> The Main Mode filter is pending deletion.</summary>
                ERROR_IPSEC_MM_FILTER_PENDING_DELETION = 13018,
                ///<summary> The transport filter is pending deletion.</summary>
                ERROR_IPSEC_TRANSPORT_FILTER_PENDING_DELETION = 13019,
                ///<summary> The tunnel filter is pending deletion.</summary>
                ERROR_IPSEC_TUNNEL_FILTER_PENDING_DELETION = 13020,
                ///<summary> The Main Mode policy is pending deletion.</summary>
                ERROR_IPSEC_MM_POLICY_PENDING_DELETION = 13021,
                ///<summary> The Main Mode authentication bundle is pending deletion.</summary>
                ERROR_IPSEC_MM_AUTH_PENDING_DELETION = 13022,
                ///<summary> The Quick Mode policy is pending deletion.</summary>
                ERROR_IPSEC_QM_POLICY_PENDING_DELETION = 13023,
                ///<summary> The Main Mode policy was successfully added, but some of the requested offers are not supported.</summary>
                WARNING_IPSEC_MM_POLICY_PRUNED = 13024,
                ///<summary> The Quick Mode policy was successfully added, but some of the requested offers are not supported.</summary>
                WARNING_IPSEC_QM_POLICY_PRUNED = 13025,
                ///<summary> ERROR_IPSEC_IKE_NEG_STATUS_BEGIN</summary>
                ERROR_IPSEC_IKE_NEG_STATUS_BEGIN = 13800,
                ///<summary> IKE authentication credentials are unacceptable</summary>
                ERROR_IPSEC_IKE_AUTH_FAIL = 13801,
                ///<summary> IKE security attributes are unacceptable</summary>
                ERROR_IPSEC_IKE_ATTRIB_FAIL = 13802,
                ///<summary> IKE Negotiation in progress</summary>
                ERROR_IPSEC_IKE_NEGOTIATION_PENDING = 13803,
                ///<summary> General processing error</summary>
                ERROR_IPSEC_IKE_GENERAL_PROCESSING_ERROR = 13804,
                ///<summary> Negotiation timed out</summary>
                ERROR_IPSEC_IKE_TIMED_OUT = 13805,
                ///<summary> IKE failed to find valid machine certificate</summary>
                ERROR_IPSEC_IKE_NO_CERT = 13806,
                ///<summary> IKE SA deleted by peer before establishment completed</summary>
                ERROR_IPSEC_IKE_SA_DELETED = 13807,
                ///<summary> IKE SA deleted before establishment completed</summary>
                ERROR_IPSEC_IKE_SA_REAPED = 13808,
                ///<summary> Negotiation request sat in Queue too long</summary>
                ERROR_IPSEC_IKE_MM_ACQUIRE_DROP = 13809,
                ///<summary> Negotiation request sat in Queue too long</summary>
                ERROR_IPSEC_IKE_QM_ACQUIRE_DROP = 13810,
                ///<summary> Negotiation request sat in Queue too long</summary>
                ERROR_IPSEC_IKE_QUEUE_DROP_MM = 13811,
                ///<summary> Negotiation request sat in Queue too long</summary>
                ERROR_IPSEC_IKE_QUEUE_DROP_NO_MM = 13812,
                ///<summary> No response from peer</summary>
                ERROR_IPSEC_IKE_DROP_NO_RESPONSE = 13813,
                ///<summary> Negotiation took too long</summary>
                ERROR_IPSEC_IKE_MM_DELAY_DROP = 13814,
                ///<summary> Negotiation took too long</summary>
                ERROR_IPSEC_IKE_QM_DELAY_DROP = 13815,
                ///<summary> Unknown error occurred</summary>
                ERROR_IPSEC_IKE_ERROR = 13816,
                ///<summary> Certificate Revocation Check failed</summary>
                ERROR_IPSEC_IKE_CRL_FAILED = 13817,
                ///<summary> Invalid certificate key usage</summary>
                ERROR_IPSEC_IKE_INVALID_KEY_USAGE = 13818,
                ///<summary> Invalid certificate type</summary>
                ERROR_IPSEC_IKE_INVALID_CERT_TYPE = 13819,
                ///<summary> No private key associated with machine certificate</summary>
                ERROR_IPSEC_IKE_NO_PRIVATE_KEY = 13820,
                ///<summary> Failure in Diffie-Helman computation</summary>
                ERROR_IPSEC_IKE_DH_FAIL = 13822,
                ///<summary> Invalid header</summary>
                ERROR_IPSEC_IKE_INVALID_HEADER = 13824,
                ///<summary> No policy configured</summary>
                ERROR_IPSEC_IKE_NO_POLICY = 13825,
                ///<summary> Failed to verify signature</summary>
                ERROR_IPSEC_IKE_INVALID_SIGNATURE = 13826,
                ///<summary> Failed to authenticate using kerberos</summary>
                ERROR_IPSEC_IKE_KERBEROS_ERROR = 13827,
                ///<summary> Peer's certificate did not have a public key</summary>
                ERROR_IPSEC_IKE_NO_PUBLIC_KEY = 13828,

                // These must stay as a unit.

                ///<summary> Error processing error payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR = 13829,
                ///<summary> Error processing SA payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_SA = 13830,
                ///<summary> Error processing Proposal payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_PROP = 13831,
                ///<summary> Error processing Transform payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_TRANS = 13832,
                ///<summary> Error processing KE payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_KE = 13833,
                ///<summary> Error processing ID payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_ID = 13834,
                ///<summary> Error processing Cert payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_CERT = 13835,
                ///<summary> Error processing Certificate Request payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_CERT_REQ = 13836,
                ///<summary> Error processing Hash payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_HASH = 13837,
                ///<summary> Error processing Signature payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_SIG = 13838,
                ///<summary> Error processing Nonce payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_NONCE = 13839,
                ///<summary> Error processing Notify payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_NOTIFY = 13840,
                ///<summary> Error processing Delete Payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_DELETE = 13841,
                ///<summary> Error processing VendorId payload</summary>
                ERROR_IPSEC_IKE_PROCESS_ERR_VENDOR = 13842,
                ///<summary> Invalid payload received</summary>
                ERROR_IPSEC_IKE_INVALID_PAYLOAD = 13843,
                ///<summary> Soft SA loaded</summary>
                ERROR_IPSEC_IKE_LOAD_SOFT_SA = 13844,
                ///<summary> Soft SA torn down</summary>
                ERROR_IPSEC_IKE_SOFT_SA_TORN_DOWN = 13845,
                ///<summary> Invalid cookie received.</summary>
                ERROR_IPSEC_IKE_INVALID_COOKIE = 13846,
                ///<summary> Peer failed to send valid machine certificate</summary>
                ERROR_IPSEC_IKE_NO_PEER_CERT = 13847,
                ///<summary> Certification Revocation check of peer's certificate failed</summary>
                ERROR_IPSEC_IKE_PEER_CRL_FAILED = 13848,
                ///<summary> New policy invalidated SAs formed with old policy</summary>
                ERROR_IPSEC_IKE_POLICY_CHANGE = 13849,
                ///<summary> There is no available Main Mode IKE policy.</summary>
                ERROR_IPSEC_IKE_NO_MM_POLICY = 13850,
                ///<summary> Failed to enabled TCB privilege.</summary>
                ERROR_IPSEC_IKE_NOTCBPRIV = 13851,
                ///<summary> Failed to load SECURITY.DLL.</summary>
                ERROR_IPSEC_IKE_SECLOADFAIL = 13852,
                ///<summary> Failed to obtain security function table dispatch address from SSPI.</summary>
                ERROR_IPSEC_IKE_FAILSSPINIT = 13853,
                ///<summary> Failed to query Kerberos package to obtain max token size.</summary>
                ERROR_IPSEC_IKE_FAILQUERYSSP = 13854,
                ///<summary> Failed to obtain Kerberos server credentials for ISAKMP/ERROR_IPSEC_IKE service.  Kerberos authentication will not function.  The most likely reason for this is lack of domain membership.  This is normal if your computer is a member of a workgroup.</summary>
                ERROR_IPSEC_IKE_SRVACQFAIL = 13855,
                ///<summary> Failed to determine SSPI principal name for ISAKMP/ERROR_IPSEC_IKE service (QueryCredentialsAttributes).</summary>
                ERROR_IPSEC_IKE_SRVQUERYCRED = 13856,
                ///<summary> Failed to obtain new SPI for the inbound SA from Ipsec driver.  The most common cause for this is that the driver does not have the correct filter.  Check your policy to verify the filters.</summary>
                ERROR_IPSEC_IKE_GETSPIFAIL = 13857,
                ///<summary> Given filter is invalid</summary>
                ERROR_IPSEC_IKE_INVALID_FILTER = 13858,
                ///<summary> Memory allocation failed.</summary>
                ERROR_IPSEC_IKE_OUT_OF_MEMORY = 13859,
                ///<summary> Failed to add Security Association to IPSec Driver.  The most common cause for this is if the IKE negotiation took too long to complete.  If the problem persists, reduce the load on the faulting machine.</summary>
                ERROR_IPSEC_IKE_ADD_UPDATE_KEY_FAILED = 13860,
                ///<summary> Invalid policy</summary>
                ERROR_IPSEC_IKE_INVALID_POLICY = 13861,
                ///<summary> Invalid DOI</summary>
                ERROR_IPSEC_IKE_UNKNOWN_DOI = 13862,
                ///<summary> Invalid situation</summary>
                ERROR_IPSEC_IKE_INVALID_SITUATION = 13863,
                ///<summary> Diffie-Hellman failure</summary>
                ERROR_IPSEC_IKE_DH_FAILURE = 13864,
                ///<summary> Invalid Diffie-Hellman group</summary>
                ERROR_IPSEC_IKE_INVALID_GROUP = 13865,
                ///<summary> Error encrypting payload</summary>
                ERROR_IPSEC_IKE_ENCRYPT = 13866,
                ///<summary> Error decrypting payload</summary>
                ERROR_IPSEC_IKE_DECRYPT = 13867,
                ///<summary> Policy match error</summary>
                ERROR_IPSEC_IKE_POLICY_MATCH = 13868,
                ///<summary> Unsupported ID</summary>
                ERROR_IPSEC_IKE_UNSUPPORTED_ID = 13869,
                ///<summary> Hash verification failed</summary>
                ERROR_IPSEC_IKE_INVALID_HASH = 13870,
                ///<summary> Invalid hash algorithm</summary>
                ERROR_IPSEC_IKE_INVALID_HASH_ALG = 13871,
                ///<summary> Invalid hash size</summary>
                ERROR_IPSEC_IKE_INVALID_HASH_SIZE = 13872,
                ///<summary> Invalid encryption algorithm</summary>
                ERROR_IPSEC_IKE_INVALID_ENCRYPT_ALG = 13873,
                ///<summary> Invalid authentication algorithm</summary>
                ERROR_IPSEC_IKE_INVALID_AUTH_ALG = 13874,
                ///<summary> Invalid certificate signature</summary>
                ERROR_IPSEC_IKE_INVALID_SIG = 13875,
                ///<summary> Load failed</summary>
                ERROR_IPSEC_IKE_LOAD_FAILED = 13876,
                ///<summary> Deleted via RPC call</summary>
                ERROR_IPSEC_IKE_RPC_DELETE = 13877,
                ///<summary> Temporary state created to perform reinit. This is not a real failure.</summary>
                ERROR_IPSEC_IKE_BENIGN_REINIT = 13878,
                ///<summary> The lifetime value received in the Responder Lifetime Notify is below the Windows 2000 configured minimum value.  Please fix the policy on the peer machine.</summary>
                ERROR_IPSEC_IKE_INVALID_RESPONDER_LIFETIME_NOTIFY = 13879,
                ///<summary> Key length in certificate is too small for configured security requirements.</summary>
                ERROR_IPSEC_IKE_INVALID_CERT_KEYLEN = 13881,
                ///<summary> Max number of established MM SAs to peer exceeded.</summary>
                ERROR_IPSEC_IKE_MM_LIMIT = 13882,
                ///<summary> IKE received a policy that disables negotiation.</summary>
                ERROR_IPSEC_IKE_NEGOTIATION_DISABLED = 13883,
                ///<summary> ERROR_IPSEC_IKE_NEG_STATUS_END</summary>
                ERROR_IPSEC_IKE_NEG_STATUS_END = 13884
                #endregion


            }


            internal sealed partial class Win32ExceptionEx : System.ComponentModel.Win32Exception
            {

                #region Constructor

                private Win32ExceptionEx ( int iError , string? ErrorAction = null ) : base(iError , ErrorAction) { }

                public Win32ExceptionEx ( Win32Exception wex ) : base(wex.NativeErrorCode) { }

                public Win32ExceptionEx ( string? ErrorAction = null ) : this(Marshal.GetLastWin32Error() , ErrorAction) { }

                public Win32ExceptionEx ( Win32Errors err , string? ErrorAction = null ) : this(
                    (int)err ,
                    ($"[{(int)err} - {err}]: {GetErrorMessage(err)}" + (ErrorAction.isNullOrWhiteSpace ? string.Empty : $"\nError source: {ErrorAction}")).Trim()
                    )
                { }

                #endregion

                public new Win32Errors NativeErrorCode => (Win32Errors)base.NativeErrorCode;

                public string FullMessage => string.Format($"[{NativeErrorCode}] {Message}");

                public override string ToString () => FullMessage;
            }


            #region API FormatMessage/ GetLastError/SetLastError


            internal static string GetErrorMessage ( int code )
            {
                return ErrorHelper.GetErrorMessage<Win32Error , int>(code);

                /*
					StringBuilder? sb = null;
					try
					{
						var strLen = Kernel32.FormatMessage (
							Kernel32.FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM |
							Kernel32.FormatMessageFlags.FORMAT_MESSAGE_IGNORE_INSERTS |
							Kernel32.FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER,
							IntPtr.Zero,
							iCode,
							0,
							ref sb,
							0,
							IntPtr.Zero);

						if (strLen > 0 && sb != null)
						{
							string err = sb!.ToString ().eTrimEnd (constants.vbCrLf);
							return (err, true);
						}
						//Can't read error message!
					}
					catch { }
					finally
					{
						sb = null;//Free mem;						
					}
									return ($"Unknown Error: {iCode}", false);
				 */
            }

            internal static string GetErrorMessage ( Win32Errors eCode )
                => GetErrorMessage((int)eCode) ?? eCode.ToString();


            #endregion



            internal static class WinsockErrors
            {
                [DllImport(Lib.Ws2_32 , CharSet = CharSet.Auto , SetLastError = true)]
                internal static extern int WSAGetLastError ();

                public static string WSAGetError ( int? code = null )
                    => GetErrorMessage(code ??= WSAGetLastError()) ?? $"Unknown WSA Error: {code}";

            }


            #region Фиксация ошибки в журнале и в DEBUG MODE вывод сообщения

#if !UWP
            /// <summary>Wrires message into NTLog and _ERRORS.LOG file in AppData Local dir</summary>
            internal static void ErrorLogWrite ( string message , EventLogEntryType et = EventLogEntryType.Error , ushort eventID = 1001 , string NT_LOG_NAME = "UOM" )
            {
                StringBuilder sb = new(4096);
                sb.AppendLine($"{DateTime.Now.eToFileName()} | {message}");
#if DEBUG
                sb.AppendLine("mode: DEBUG");
#else
                sb.AppendLine("mode: RELEASE");
#endif

                var loc = uom.AppInfo.AppAssembly.Location;

                sb.AppendLine($"Product Name: {Application.ProductName} (Version: {Application.ProductVersion})");
                sb.AppendLine($"ExecutablePath: {Application.ExecutablePath}");
                sb.AppendLine(uom.AppInfo.AppAssembly.dumpObjectToString(excludeMembers: [ "EscapedCodeBase" , "CustomAttributes" , "DefinedTypes" , "ExportedTypes" , "IsCollectible" , "Modules" , "ReflectionOnly" , "SecurityRuleSet" ]));
                //sb.AppendLine($"FileVersionInfo: {uom.AppInfo.AssemblyFileVersionInfo.eDumpObjectToString()}");

                string[] exl = [ "DefaultThreadCurrentCulture" , "DefaultThreadCurrentUICulture" , "IetfLanguageTag" , "InvariantCulture" , "NumberFormat" , "OptionalCalendars" , "UseUserOverride" , "IsReadOnly" , "CultureTypes" , "DateTimeFormat" , "EnglishName" , "IsNeutralCulture" , "InstalledUICulture" , "DisplayName" ];
                //sb.AppendLine($"CurrentCulture: {System.Globalization.CultureInfo.CurrentCulture.eDumpObjectToString(excludeMembers: exl)}");
                sb.AppendLine($"CurrentThread.CurrentUICulture: {Thread.CurrentThread.CurrentUICulture.dumpObjectToString(excludeMembers: exl)}");
                sb.AppendLine(typeof(System.Environment).dumpStaticMembers("ExitCode" , "NewLine" , "StackTrace"));

                message = sb.ToString();

                void WriteNTLog ()
                {

                    lock ( _eaNTLogLockObject.Value )
                    {
                        //Trying Write To Standart Application Event Log...
                        static void WriteToApplicationLog ( string msg , EventLogEntryType et , ushort eventID )
                        {
                            var a = delegate ()
                            {
                                const string src = "Application";
                                using EventLog elApplication = new(src);
                                elApplication.Source = src;
                                elApplication.WriteEntry(msg , et , eventID);
                            };
                            a.tryCatch();
                        }

                        var fiApp = new FileInfo(Application.ExecutablePath);
                        string src = fiApp.Name;
                        try
                        {
                            // searching the source throws a security exception ONLY if not exists!
                            if ( !EventLog.SourceExists(src) )
                            {
                                EventLog.CreateEventSource(src , NT_LOG_NAME);
                                using System.Diagnostics.Eventing.Reader.EventLogConfiguration elcUOM = new(NT_LOG_NAME);
                                elcUOM.LogMode = System.Diagnostics.Eventing.Reader.EventLogMode.Circular;
                                elcUOM.MaximumSizeInBytes = (long)10u.fileSize_MBToBytes();
                                elcUOM.SaveChanges();
                            }
                            using EventLog elUOM = new(NT_LOG_NAME);
                            elUOM.Source = src;
                            elUOM.WriteEntry(message , et , eventID);
                        }
                        catch ( SecurityException )
                        {
                            WriteToApplicationLog($"{src}:\nEventlog source '{src}' is not registered for log {NT_LOG_NAME}!\nApp '{fiApp.FullName}' must be run with admin priveleges just once, to allow this source registration." , EventLogEntryType.Warning , eventID);
                            WriteToApplicationLog(message , et , eventID);
                        }
                        catch ( Exception )
                        {
                            //Some shit happens...
                        }
                    }
                }

                void WriteFileLog ()
                {
                    const uint MAX_ERR_LOG_SIZE_MB = 10u;
                    const int MAX_ARHIVE_FILES_COUNT = 5;

                    try
                    {
                        lock ( _eaFileLogLockObject.Value )
                        {
                            var fiErrorLog = uom.AppTools.GetFileIn_AppData("_Errors.log" , false).eToFileInfo();
                            if ( fiErrorLog != null )
                            {
                                if ( fiErrorLog.Exists && fiErrorLog.Length >= (long)MAX_ERR_LOG_SIZE_MB.fileSize_MBToBytes() )
                                {
                                    try
                                    {
                                        fiErrorLog.eMoveToArhive(MAX_ARHIVE_FILES_COUNT);
                                    }
                                    catch { }
                                }

                                using var sw = fiErrorLog.AppendText();
                                sw.WriteLine('*'.repeat());
                                sw.WriteLine(message);
                                sw.Flush();
                            }
                        }
                    }
                    catch { }
                }

                Task tskWriteNTLog = new(WriteNTLog , TaskCreationOptions.LongRunning);
                Task tskWriteFileLog = new(WriteFileLog , TaskCreationOptions.LongRunning);
                Task[] allTasks = [ tskWriteNTLog , tskWriteFileLog ];
                allTasks.forEach(t => t.Start());
                Task.WaitAll(allTasks);
            }


#endif


            private static readonly Lazy<EventArgs> _eaFileLogLockObject = new(() => new EventArgs());
            private static readonly Lazy<EventArgs> _eaNTLogLockObject = new(() => new EventArgs());

            #endregion



            /// <summary>
            /// Obsolete! use <see cref="Vanara.PInvoke.Win32Error.ThrowLastError(string?)"/> instead!
            /// </summary>
            [Obsolete("Use Vanara.PInvoke.Win32Error.ThrowLastError (); instead!" , true)]

            public static void ThrowLastWin23Error ( [CallerMemberName] string? errorDescription = null )
                => new Win32Exception(errorDescription).eThrow();



            public static Win32ExceptionEx LastWin23Error ()
                => new();


        }


        /// <summary>Win32 core API</summary>
        internal static partial class core
        {
            internal const int ANYSIZE_ARRAY = 1;
            internal const uint MAXDWORD = 0xFFFFFFFFu;
            internal const int MINCHAR = 0x80;
            internal const int MAXCHAR = 0x7F;
            internal const int MINSHORT = 0x8000;
            internal const int MAXSHORT = 0x7FFF;
            internal const uint MINLONG = 0x80000000u;
            internal const int MAXLONG = 0x7FFFFFFF;
            internal const uint MAXBYTE = 0xFFU;
            internal const int MAXWORD = 0xFFFF;
            internal const int S_OK = 0;

            public const uint INFINITE = 0xFFFFFFFF;
            //public const UInt32 WAIT_FAILED = 0xFFFFFFFF;


            //internal const uint STANDARD_RIGHTS_REQUIRED = 0xF0000;
            //internal const uint GENERIC_READ = (uint) io.NativeFileAccess.GenericRead;
            //public const Int32 GENERIC_ALL_ACCESS = 0x10000000;



            internal const string DISPLAY = "DISPLAY";
            internal const string WINDLL_WINSPOOL = "winspool.drv";


            //Lib.Kernel32
            internal const string WINDLL_MSVCRT = "msvcrt.dll";
            //internal const string WINDLL_TOOLHELP = WINDLL_KERNEL;
            //internal const string WINDLL_USERENV = "Userenv.dll";
            //internal const string WINDLL_COMCTL = "comctl32.dll";
            //internal const string WINDLL_SHLWAPI = "SHLWAPI.dll";
            //internal const string WINDLL_NETAPI = "Netapi32.dll";
            //internal const string WINDLL_MPR = "Mpr.dll";
            //internal const string WINDLL_THEME = "uxtheme.dll";
            //internal const string WINDLL_DWMAPI = "dwmapi.dll";
            //internal const string WINDLL_avicap32 = "avicap32.dll";
            //internal const string WINDLL_POWRPROF = "powrprof.dll";
            //internal const string WINDLL_NTDLL = "ntdll.dll";
            //internal const string WINDLL_wlanapi = "wlanapi.dll";
            //internal const string WINDLL_iphlpapi = "iphlpapi.dll";
            //internal const string WINDLL_URL = "url.dll";
            //internal const string WINDLL_WS232 = "Ws2_32.dll";


            /// <summary>This prefix indicates to NTFS that the path is to be treated as a non-interpretedpath in the virtual file system.</summary>
            internal const string UNC_PATH_PREFIX = @"\\?\";
            // Public Const LONG_PATH_PREFIX As string = "\??\"
            public static readonly IntPtr HWND_DESKTOP = new(0);

            public const string MS_RESX_PREFIX = @"ms-resource:";

        }


        /// <summary>Win32 Memory API</summary>
        internal static partial class memory
        {


            #region Universal way of getting WinAPI structures from Ptr					 


            internal delegate void OnStructureCreated<T> ( ref T rStrucr ) where T : struct;


            /// <summary>
            /// Универсальный метод получения структур WinAPI через буфер в памяти
            /// <br/>
            /// Sample:
            /// <code>
            /// Dim A = UniversalGetWin32Structure(Of SPI_NONCLIENTMETRICS)(Sub(ByRef R1)
            /// R1.cbSize = Marshal.SizeOf(R1)
            /// End Sub,
            /// Sub(Ptr)
            /// If (Not SystemParametersInfo(SPI.SPI_GETNONCLIENTMETRICS, 0, Ptr)) Then Throw New ComponentModel.Win32Exception
            /// End Sub)
            /// 
            /// Or:
            /// Return UOM.WinAPI.UniversalGetWin32Structure(Of SPI_NONCLIENTMETRICS)(
            /// Sub(ByRef R1) R1.cbSize = Marshal.SizeOf(R1),
            /// Sub(Ptr) If (Not SystemParametersInfo(SPI.SPI_GETNONCLIENTMETRICS, 0, Ptr)) Then Throw New ComponentModel.Win32Exception)
            /// </code>
            /// </summary>
            /// <typeparam name="T">Тип структуры</typeparam>
            /// <param name="structInitializer">Действие после создания нового экземпляра структуры (инициализация полей, размеров и т.д.). Структура передаётся по-ссылке</param>
            /// <param name="structGetterBody">Метод главного вызова API, с передачей ему указателя на буфер, который потом будет преобразован в структуру</param>

            internal static T GetWinApiStruct<T> (
                OnStructureCreated<T> structInitializer ,
                Action<IntPtr> structGetterBody
                ) where T : struct
            {
                var rStruct = new T();
                structInitializer?.Invoke(ref rStruct);                // Вызываем метод инициализации новой структуры - заполение полей, указание размеров и т.д., что требует конкретная API
                using var mem = rStruct.toMemoryBlock(); // Создаём буффер в памяти размером со структуру и копируем содержимое структуры в новый буфер																	
                structGetterBody.Invoke(mem.DangerousGetHandle());          // Главный вызов API, с указателем на буфер в памяти																														
                return mem.DangerousGetHandle().eToStructure<T>();           // Преобразование буфера обратно в структуру
            }


            #endregion


            /// <summary>PInvoke, для CRT функции memcmp()</summary>
            [DllImport(core.WINDLL_MSVCRT , CallingConvention = CallingConvention.Cdecl)]
            public static extern int memcmp ( byte[] a , byte[] b , ulong p_Count );

#if NET






            #region Only for stackalloc'ed arrays!

            [DllImport(core.WINDLL_MSVCRT , CallingConvention = CallingConvention.Cdecl)]
            internal static extern int memcmp ( in byte a , in byte b , ulong p_Count );

            /// <summary>This faster than memcmp(byte[] a, byte[] b) 
            /// ONY when A & B is located in stack (stackalloc'ed)!
            /// When Arrays located in RAM - this not faster.</summary>

            public static unsafe bool memcmp ( this ReadOnlySpan<byte> a , ReadOnlySpan<byte> b )
            {
                fixed (
                    byte*
                    ptrA = &MemoryMarshal.GetReference(a),
                    ptrB = &MemoryMarshal.GetReference(b)
                    )
                {
                    return memcmp(
                        Unsafe.AsRef<byte>(ptrA) ,
                        Unsafe.AsRef<byte>(ptrB) ,
                        (ulong)a.Length) == 0;
                }
            }


            public static unsafe bool memcmp ( this Span<byte> a , Span<byte> b )
                => a.memcmp((ReadOnlySpan<byte>)b);

            /*
/// <summary>PInvoke, для CRT функции memcmp()</summary>
[DllImport (uom.WinAPI.core.WINDLL_MSVCRT, CallingConvention = CallingConvention.Cdecl)]
public static extern int memcmp ( byte[] a, byte[] b, UInt64 p_Count );

/// <summary>PInvoke, для CRT функции memcmp()</summary>
[DllImport (uom.WinAPI.core.WINDLL_MSVCRT, CallingConvention = CallingConvention.Cdecl)]
public static extern int memcmp ( Span<byte> a, Span<byte> b, UInt64 p_Count );
*/
            /*

			/// <summary>PInvoke, для CRT функции memcmp()</summary>
			[DllImport (core.WINDLL_MSVCRT, CallingConvention = CallingConvention.Cdecl)]
			public unsafe static extern int memcmp ( byte* a, byte* b, UInt64 p_Count );

			 	fixed (byte*
						data1ptr = &d1.DangerousGetReference (),
						data2ptr = &d2.DangerousGetReference ()
						)
					{
						var result = memcmp (data1ptr, data2ptr, (ulong) readBlockSize);
						bool equal2 = result == 0;
					}
			 
			 */



            #endregion
#endif



            #region CopyMemory ZeroMemory FillMemory

            /// <summary>Note that if you declare the lpData parameter as string, you must pass it By Value.</summary>
            [DllImport(Lib.Kernel32 , EntryPoint = "RtlMoveMemory" , SetLastError = true , CharSet = CharSet.Auto , CallingConvention = CallingConvention.Winapi)]
            internal static extern void CopyMemory ( IntPtr target , IntPtr source , int length );



            internal static void ZeroMemory ( ref byte[] A , int? nBytes = null )
            {
                nBytes ??= A.Length;

                var GCH = GCHandle.Alloc(A);
                try
                {
                    var ptrAP1 = Marshal.UnsafeAddrOfPinnedArrayElement(A , 0);
                    Kernel32.RtlZeroMemory(ptrAP1 , (int)nBytes);
                }
                finally { GCH.Free(); }
            }


            #endregion




            /// <summary>Memory Block uses Marshal.AllocHGlobal</summary>
            //[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            [DebuggerDisplay("WinApiMemory: hMem:{handle}, Lenght={Lenght}")]
            internal sealed partial class WinApiMemory : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
            {

                private HandleRef _HandleRef = default;
                private ulong _Size = 0UL;

                public static readonly WinApiMemory INVALID = new(0);


                /// <summary>If true - uses LocalFree for release memory. If False - uses FreeHGlobal</summary>
                public readonly bool IsLocal = false;


                #region Constructor\Destructor


                /// <summary>HGlobal</summary>
                internal WinApiMemory ( ulong lSize ) : base(true) => ReAlloc(lSize);

                internal WinApiMemory ( int iSize ) : this((ulong)iSize) { }

                internal WinApiMemory ( byte[] abData ) : base(true)
                {
                    if ( abData!.Length < 1 ) throw new ArgumentNullException(nameof(abData));

                    ReAlloc((ulong)abData!.Length);
                    Marshal.Copy(abData , 0 , DangerousGetHandle() , abData.Length);
                }

                /// <summary>Init from Memory Handle</summary>
                /// <param name="bIsLocal">If true - uses LocalFree for release memory. If False - uses FreeHGlobal</param>
                internal WinApiMemory ( IntPtr hMem , bool OwnsHandle = true , bool bIsLocal = false ) : base(OwnsHandle)
                {
                    IsLocal = bIsLocal;
                    SetHandle(hMem);
                    IsInvalid.ThrowLastWin32ErrorIfTrue();

                    _HandleRef = new HandleRef(this , hMem);
                    _Size = IsLocal
                        ? Kernel32.LocalSize(hMem)
                        : Kernel32.GlobalSize(hMem);
                }


                /// <summary>Init from string (create copy of the string)</summary>
                internal WinApiMemory ( string source , bool unicode = true ) : this(
                    unicode
                    ? Marshal.StringToHGlobalUni(source)
                    : Marshal.StringToHGlobalAnsi(source)
                    , true , false)
                { }


                public void Zero ( int? bytesCount = null )
                {
                    if ( !IsValid ) return;
                    Kernel32.RtlZeroMemory(DangerousGetHandle() , bytesCount ?? Lenght);
                }

                internal void FillMemory ( byte Data , int? nBytes = null )
                {
                    if ( !IsValid ) return;
                    Kernel32.RtlFillMemory(DangerousGetHandle() , nBytes ?? Lenght , Data);
                }

                public void ReAlloc ( ulong lSize )
                {
                    if ( IsLocal ) throw new NotSupportedException($"ReAlloc is not supported with {nameof(IsLocal)} flag!");


                    ReleaseHandle();
                    if ( lSize > 0 )
                    {
                        int iSize = (int)lSize;
                        var hMem = Marshal.AllocHGlobal(iSize);
                        SetHandle(hMem);
                        IsInvalid.ThrowLastWin32ErrorIfTrue();
                        _HandleRef = new HandleRef(this , hMem);
                    }
                    _Size = lSize;
                }


                protected override bool ReleaseHandle ()
                {
                    _HandleRef = default;
                    if ( IsValid )
                    {
                        if ( IsLocal )
                            Kernel32.LocalFree(DangerousGetHandle());
                        else
                            Marshal.FreeHGlobal(DangerousGetHandle());

                        SetHandle(IntPtr.Zero);
                        _Size = 0UL;
                    }
                    return true;
                }


                #endregion


                #region Properties


                public int Lenght => (int)_Size;

                public ulong Lenght64 => _Size;

                public bool IsValid => !IsInvalid;


                #endregion



                #region Public Methods


                [DebuggerNonUserCode, DebuggerStepThrough]
                public void CopyFrom ( IntPtr Source , int CopySize )
                {
                    if ( CopySize > Lenght ) throw new OutOfMemoryException();
                    CopyMemory(DangerousGetHandle() , Source , CopySize);
                }

                [DebuggerNonUserCode, DebuggerStepThrough]
                public void CopyTo ( IntPtr Target , int CopySize )
                {
                    if ( CopySize > Lenght )
                    {
                        throw new OutOfMemoryException();
                    }

                    CopyMemory(Target , DangerousGetHandle() , CopySize);
                }

                [DebuggerNonUserCode, DebuggerStepThrough]
                public byte[] ToBytes () => DangerousGetHandle().ePtrToBytes(Lenght);


                public short[] ToShorts ()
                {
                    int VAL_SIZE = Marshal.SizeOf(typeof(short));
                    var Numbers = Array.Empty<short>();
                    int iCount = (int)Math.Round(Lenght / (double)VAL_SIZE);
                    if ( iCount > 0 )
                    {
                        Numbers = new short[ iCount ];
                        int N, iOffset = default;
                        var loopTo = iCount - 1;
                        for ( N = 0 ; N <= loopTo ; N++ )
                        {
                            Numbers[ N ] = Marshal.ReadInt16(DangerousGetHandle() , iOffset);
                            iOffset += VAL_SIZE;
                        }
                    }

                    return Numbers;
                }

                public int[] ToIntegers ()
                {
                    int VAL_SIZE = Marshal.SizeOf(typeof(int));
                    var Numbers = Array.Empty<int>();
                    int iCount = (int)Math.Round(Lenght / (double)VAL_SIZE);
                    if ( iCount > 0 )
                    {
                        Numbers = new int[ iCount ];
                        int N, iOffset = default;
                        var loopTo = iCount - 1;
                        for ( N = 0 ; N <= loopTo ; N++ )
                        {
                            Numbers[ N ] = Marshal.ReadInt32(DangerousGetHandle() , iOffset);
                            iOffset += VAL_SIZE;
                        }
                    }

                    return Numbers;
                }

                public System.Drawing.Point[] ToPoints ()
                {
                    int VAL_SIZE = Marshal.SizeOf(typeof(System.Drawing.Point));
                    var Numbers = Array.Empty<System.Drawing.Point>();
                    int iCount = (int)Math.Round(Lenght / (double)VAL_SIZE);
                    if ( iCount > 0 )
                    {
                        Numbers = new System.Drawing.Point[ iCount ];
                        int N, iOffset = default;
                        var loopTo = iCount - 1;
                        for ( N = 0 ; N <= loopTo ; N++ )
                        {
                            Numbers[ N ].X = Marshal.ReadInt32(DangerousGetHandle() , iOffset);
                            Numbers[ N ].Y = Marshal.ReadInt32(DangerousGetHandle() , iOffset + (int)Math.Round(VAL_SIZE / 2d));
                            iOffset += VAL_SIZE;
                        }
                    }
                    return Numbers;
                }

                [Obsolete("Need Refactor inner call!" , true)]
                public string[] ToStrings ( int StringSize ) => ToStrings(StringSize , Marshal.SystemDefaultCharSize);

                [Obsolete("Need Refactor!" , true)]
                public string[] ToStrings ( int stringSizeBytes , int CharSize )
                {
                    List<string> lResult = [];
                    int sringSizeChars = stringSizeBytes * CharSize;
                    int iStringCount = (int)Math.Round(Lenght / (double)sringSizeChars);
                    if ( iStringCount > 0 )
                    {
                        IntPtr ptr = DangerousGetHandle();
                        for ( int i = 1 ; i <= iStringCount ; i++ )
                        {
                            //TODO: NEED Refactor!
                            string sValue = Marshal.PtrToStringAuto(ptr , stringSizeBytes)?.eNString() ?? string.Empty;

                            lResult.Add(sValue);
                            ptr += sringSizeChars;
                        }
                    }
                    return [ .. lResult ];
                }

                public string DumpHex () => DangerousGetHandle().dumpHex(Lenght);

                public override string ToString () => DumpHex();

                public byte[] RAWData => ToBytes();

                public string RAWData_AsUnicode => Encoding.Unicode.GetString(RAWData , 0 , RAWData.Length);

                public string RAWData_AsASCII => Encoding.ASCII.GetString(RAWData , 0 , RAWData.Length);

                #endregion


                #region Operator
                public static bool operator == ( WinApiMemory Mem1 , WinApiMemory LMB2 ) => Mem1.IsValid && LMB2.IsValid && (Mem1.DangerousGetHandle() == LMB2.DangerousGetHandle());

                public static bool operator != ( WinApiMemory LMB1 , WinApiMemory LMB2 ) => !(LMB1 == LMB2);

                public static implicit operator IntPtr ( WinApiMemory LMB1 ) => LMB1.DangerousGetHandle();







                // Public Shared Widening Operator CType(ByVal LMB1 As LocalMemory) As string
                // Return LMB1.ToString
                // End Operator
                // Public Shared Widening Operator CType(ByVal LMB1 As LocalMemory) As Integer()
                // Return LMB1.ToIntegers
                // End Operator
                // Public Shared Widening Operator CType(ByVal LMB1 As LocalMemory) As System.Drawing.Point()
                // Return LMB1.ToPoints
                // End Operator





                #endregion


                public MemoryLock Lock () => new(this);

                public override bool Equals ( object? obj )
                    => obj is WinApiMemory memory && EqualityComparer<IntPtr>.Default.Equals(handle , memory.handle);

                public override int GetHashCode () => handle.GetHashCode();


                #region Class MemoryLock

                public partial class MemoryLock : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
                {
                    protected readonly WinApiMemory? _OwnerMemory = null;

                    protected internal MemoryLock ( WinApiMemory M ) : base(true)
                    {
                        _OwnerMemory = M;
                        var hMem = _OwnerMemory.IsLocal
                            ? Kernel32.LocalLock(_OwnerMemory.DangerousGetHandle())
                            : Kernel32.GlobalLock(_OwnerMemory.DangerousGetHandle());

                        SetHandle(hMem);
                        IsInvalid.ThrowLastWin32ErrorIfTrue();
                    }

                    protected override bool ReleaseHandle ()
                    {
                        var hMem = DangerousGetHandle();
                        if ( hMem.isValid )
                        {
                            hMem = _OwnerMemory!.DangerousGetHandle();

                            if ( _OwnerMemory.IsLocal )
                                Kernel32.LocalUnlock(hMem).ThrowLastWin32ErrorIfFailed();
                            else
                                Kernel32.GlobalUnlock(hMem).ThrowLastWin32ErrorIfFailed();

                            SetHandle(IntPtr.Zero);
                        }
                        return true;
                    }
                }
                #endregion


            }

        }


        /// <summary>Win32 Multithreading and syncronisation API</summary>
        internal static partial class sync
        {

            #region Event APIs

            /*
			//[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			[DebuggerDisplay ("Win32Event: hEvent:{handle}")]
			internal sealed partial class Win32Event : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
			{

                #region Constructor\Destructor

				public Win32Event ( IntPtr hEvent, bool OwnHandle ) : base (OwnHandle)
				{
					ReleaseHandle ();

					SetHandle (hEvent);
					if (IsInvalid) errors.ThrowLastWin23Error ();
				}

				public Win32Event ( bool ManualReset, bool InitialState, string? Name = null ) : this (IntPtr.Zero, ManualReset, InitialState, Name) { }


				public Win32Event ( SECURITY_ATTRIBUTES sa, bool ManualReset, bool InitialState, string? Name = null )
					: this (Kernel32.CreateEvent (sa, ManualReset, InitialState, Name).DangerousGetHandle (), true)
				{ }




				protected override bool ReleaseHandle ()
				{
					if (!IsInvalid)
					{
						if (!Kernel32.CloseHandle (DangerousGetHandle ()))
							return false;

						SetHandle (IntPtr.Zero);
					}
					return true;
				}
                #endregion

				internal Kernel32.WAIT_STATUS WaitForSingleObject ( int Milliseconds = Timeout.Infinite )
					=> DangerousGetHandle ().WaitForSingleObject (Milliseconds);

			}

			 */

            #endregion

        }


        /// <summary>Win32 Process and Threads API</summary>
        internal static partial class process
        {



            #region Process API


            /// <summary>c# Analog for VB.Interaction.Shell</summary>
            /// <param name="cmdWithArgs">Sample: "cmd /c powershell"</param>
            /// <returns></returns>
            /// <exception cref="Win32Exception"></exception>
            internal static Kernel32.SafePROCESS_INFORMATION InteractionShell ( string cmdWithArgs , bool waitForExit = false , Kernel32.CREATE_PROCESS flags = 0 )
            {
                Kernel32.STARTUPINFO si = new();
                if ( !Kernel32.CreateProcess(
                    null ,
                    new StringBuilder(cmdWithArgs) ,
                    null ,
                    null ,
                    false ,
                    flags ,
                    null ,
                    null ,
                    si ,
                    out var pi) )
                {
                    throw new Win32Exception();
                }

                // Wait until child process exits.
                if ( waitForExit )
                {
                    pi.hProcess.DangerousGetHandle().WaitForSingleObjectInfinite();
                    pi.hThread.Close();
                    pi.hProcess.Close();
                }
                return pi;
            }


            #endregion








            #region ADVAPI32 User, Logon and Impersonation API




            internal sealed class SafeUserTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
            {
                public SafeUserTokenHandle () : base(true) { }

                public SafeUserTokenHandle ( IntPtr existingHandle ) : base(true)
                    => base.SetHandle(existingHandle);


                public static explicit operator IntPtr ( SafeUserTokenHandle userTokenHandle )
                    => userTokenHandle.DangerousGetHandle();


                protected override bool ReleaseHandle ()
                    => Kernel32.CloseHandle(handle);

            }



            #endregion




            /*

			public static bool CreateProcess(int parentProcessId)
			{
				const uint EXTENDED_STARTUPINFO_PRESENT = 0x00080000;
				const int PROC_THREAD_ATTRIBUTE_PARENT_PROCESS = 0x00020000;

				PROCESS_INFORMATION pInfo = new();
				STARTUPINFOEX sInfoEx = new();

				sInfoEx.StartupInfo.cb = Marshal.SizeOf(sInfoEx);
				IntPtr lpValue = IntPtr.Zero;

				try
				{
					if (parentProcessId > 0)
					{
						var lpSize = IntPtr.Zero;
						var success = InitializeProcThreadAttributeList(IntPtr.Zero, 1, 0, ref lpSize);
						if (success || lpSize == IntPtr.Zero)
						{
							return false;
						}

						sInfoEx.lpAttributeList = Marshal.AllocHGlobal(lpSize);
						success = InitializeProcThreadAttributeList(sInfoEx.lpAttributeList, 1, 0, ref lpSize);
						if (!success)
						{
							return false;
						}

						var parentHandle = Process.GetProcessById(parentProcessId).Handle;
						// This value should persist until the attribute list is destroyed using the DeleteProcThreadAttributeList function
						lpValue = Marshal.AllocHGlobal(IntPtr.Size);
						Marshal.WriteIntPtr(lpValue, parentHandle);

						success = UpdateProcThreadAttribute(
							sInfoEx.lpAttributeList,
							0,
							(IntPtr)PROC_THREAD_ATTRIBUTE_PARENT_PROCESS,
							lpValue,
							(IntPtr)IntPtr.Size,
							IntPtr.Zero,
							IntPtr.Zero);

						if (!success) return false;
					}

					var pSec = new SECURITY_ATTRIBUTES();
					var tSec = new SECURITY_ATTRIBUTES();
					pSec.nLength = Marshal.SizeOf(pSec);
					tSec.nLength = Marshal.SizeOf(tSec);
					var lpApplicationName = Path.Combine(Environment.SystemDirectory, "notepad.exe");
					return CreateProcess(lpApplicationName, null, ref pSec, ref tSec, false, EXTENDED_STARTUPINFO_PRESENT, IntPtr.Zero, null, ref sInfoEx, out pInfo);

				}
				finally
				{
					// Free the attribute list
					if (sInfoEx.lpAttributeList != IntPtr.Zero)
					{
						DeleteProcThreadAttributeList(sInfoEx.lpAttributeList);
						Marshal.FreeHGlobal(sInfoEx.lpAttributeList);
					}
					Marshal.FreeHGlobal(lpValue);

					// Close process and thread handles
					if (pInfo.hProcess != IntPtr.Zero) uom.WinAPI.core.CloseHandle(pInfo.hProcess);
					if (pInfo.hThread != IntPtr.Zero) uom.WinAPI.core.CloseHandle(pInfo.hThread);
				}
			}

			 */

        }


        /// <summary>Win32 Resources API</summary>
        internal static partial class win32Resources
        {

            /// <summary>
            /// <summary>Префикс Win32 строкового ресурса '@' обычно используется в строках вида '@%SystemRoot%\System32\shell32.dll,-30596'</summary
            /// </summary>
            public const char WIN32_RESOURCE_PREFIX = '@';


            /*
            [System.Diagnostics.DebuggerDisplayAttribute("Win32ResourceHandle={handle}")]
            internal class Win32ResourceHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
            {

                public Win32ResourceHandle ( IntPtr hInstance , IntPtr hResInfo ) : base(true)
                {
                    //'Call MyBase.SetHandle(LoadResource(hInstance, hResInfo))
                    IsInvalid.ThrowLastWin32ErrorIfTrue();
                }

                protected override bool ReleaseHandle ()
                {
                    
            //        If(Not Me.IsInvalid) Then
            //    '    Dim bResult As Boolean = FreeResource(Me.handle)
            //    '   If bResult Then Call MyBase.SetHandle(IntPtr.Zero)
            //    '   Return bResult
            //End If
                    
                    return true;
                }
            }
             */


            internal class Win32ResourcePathParser
            {
                public readonly string File;
                public readonly int ResourceIndex;


                /// <summary>Разбирает строку с указанием ресурса, которая обычно используется в реестре, или ещё где...</summary>
                ///<param name="win32ResString">%systemroot%\system32\mstsc.exe или %systemroot%\system32\mstsc.exe,-4004 или shell32.dll, 2</param>
                public Win32ResourcePathParser ( string win32ResString ) : base()
                {
                    if ( win32ResString.isNullOrWhiteSpace ) throw new ArgumentNullException(nameof(win32ResString));

                    string file = win32ResString.Trim();

                    if ( file.StartsWith('@') ) file = file[ 1.. ];

                    int resIndex = 0;

                    if ( file.Contains(',') ) // Есть запятая-разделитель - делим
                    {
                        var s = file.eSplitByChar(',' , true);
                        file = s!.LeftPart.Trim();
                        resIndex = int.Parse(s!.RightPart.Trim());
                    }
                    ResourceIndex = resIndex;

                    if ( file.Contains('%') ) // Path contains '%' looks like environment string...
                        file = System.Environment.ExpandEnvironmentVariables(file);

                    File = file;
                }

            }

            #region String resources


            internal const int DEFAULT_LOAD_STRING_RES_BUFFER_SIZE = 1024 * 10;//' 2048



            //'Friend Function LoadStringFromDLL( hInst As IntPtr,
            //'                                   ID As Integer,
            //'                                  Optional  DefaultBufferLenght As Integer = DEFAULT_LOAD_STRING_RES_BUFFER_SIZE) As String

            //'    '                inline CString LoadStringFromDLL ( HINSTANCE hInst , int ids )
            //'    '	// (c) 1998-2000 Golden Crater Software, All rights reserved.
            //'    '	// You may freely use this code
            //'    '	// You may freely redistribute this code, provided this comment block remains 
            //'    '        // intact.
            //'    '	// The body of this function Is from the epAssist EAX SDK:
            //'    '	//   Use plain English to control your PC over email, AppBar, And soon 
            //'    '        //   voice telephone. 
            //'    '	//   Check out this free SDK that allows you to interface to epAssist. 
            //'    '	//   Easily add natural language functionality to your application And 
            //'    '	//   provide your users with access to your program from anywhere they 
            //'    '	//   can send And receive email (2-way pagers, public terminals And email 
            //'    '	//   enabled telephones included.) MFC AppWizard And HTML Help, And sample
            //'    '	//   source included.             http://www.goldencrater.com/software

            //'    '	CString str;
            //'    '	TCHAR szTemp[ 256 ];		// Raise limit Of 255 Char If needed.
            //'    '	int nLen;



            //'    Dim lpszName = MAKEINTRESOURCE((ID >> 4) + 1)
            //'    Dim hResInfo = FindResource(hInst, lpszName, RESOURCE_TYPES.RT_STRING)
            //'    '  Dim hMem = LoadResource(hInst , hResInfo)
            //'    ' Dim HHH = LockResource(hMem)


            //'    'of the resource data, call the LockResource function; to obtain the size of the resource, call SizeofResource.



            //'    '	If (: FindResource(hInst, lpszName, RT_STRING) != NULL &&
            //'    '		(nLen = :LoadString(hInst, ids, szTemp, 255) != 0)) {
            //'    '		str = szTemp;
            //'    '	}
            //'    '	Return str;
            //'End Function



            //            <DllImport( WINDLL_SHLWAPI, BestFitMapping:=False, CharSet:=CharSet.Unicode, ExactSpelling:=True, SetLastError:=False, ThrowOnUnmappableChar:=True)>
            //Friend Function SHLoadIndirectString ( pszSource As String,
            //									 <[In] (), [Out] ()> <MarshalAs(UnmanagedType.LPTStr)> pszOutBuf As Global.System.Text.StringBuilder,
            //									 <[In] (), [Out] ()> cchOutBuf As Integer,
            //                                                 ppvReserved As IntPtr) As Integer

            //            End Function



            [Obsolete("VANARA User32.LoadString must be updated !!!")]
            public static string? LoadString ( SafeHINSTANCE hinstance , int id , int defaultBufferLenght = DEFAULT_LOAD_STRING_RES_BUFFER_SIZE )
            {
                id = Math.Abs(id);

                //must use:
                //var len2 = User32.LoadString(hinstance , id);

                var sb = new StringBuilder(defaultBufferLenght);
                var len = User32.LoadString(hinstance , id , sb , sb.Capacity);
                return len > 0
                    ? sb.ToString()
                    : null;
            }


            ///<summary>Загружает строковый ресурс из DLL по пути типа '@%SystemRoot%\System32\shell32.dll,-30596'</summary>
            [Obsolete("VANARA User32.LoadString must be updated !!!")]
            public static string? LoadString ( string file , int id , int defaultBufferLenght = DEFAULT_LOAD_STRING_RES_BUFFER_SIZE )
            {
                var hModule = uom.WinAPI.safeHandles.GetRunningLibraryOrLoad_AsResource(file);
                return LoadString(hModule , id , defaultBufferLenght);
            }


            #endregion


            #region Icon resources




            /*
        [DllImport(core.WINDLL_KERNEL, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr LoadResource(
          [in, optional] IntPtr hModule,
         [in] HRSRC hResInfo
        );
             */


            internal enum LRI_METHOD : int
            {
                ExtractIconExByIndex,
                //LoadIconByResID
                Load_RT_ICON,
                Load_RT_ICON_GROUP
            }
            //Friend Const C_DEFAULT_LRI_METHOD As LRI_METHOD = LRI_METHOD.Load_RT_ICON

            internal enum LRI_ICON_SIZE : int
            {
                Small = 0,
                Large = 1
                //'Any other size will be processed to APIo
            }

            //Friend Const C_LRI_ICON_SIZE As LRI_ICON_SIZE = LRI_ICON_SIZE.Large
            /*
        private Icon LoadResWin32Icon_Safe(
          int IconIndexOrResID,
          string sLib,
          LRI_METHOD eMethod = LRI_METHOD.Load_RT_ICON,
          LRI_ICON_SIZE eIconSize = LRI_ICON_SIZE.Small,
          bool UseDefultIconOnError = true)
        {



                          Try
                              Select Case eMethod
                          Case LRI_METHOD.ExtractIconExByIndex
                              Dim iIconIndex = IconIndexOrResID
                              Dim LR = uomvb.Win32.GDI.GDIObjects.Icon.ExtractIconEx(sLib, iIconIndex)

                              Dim rIcon = LR.Large
                              Select Case eIconSize
                                  Case LRI_ICON_SIZE.Small : rIcon = LR.Small
                                  Case LRI_ICON_SIZE.Large: rIcon = LR.Large
                                      'Case Else
                              End Select
                              If(rIcon Is Nothing) Then Throw New Exception("ExtractIconEx({0}, {1}) Failed!".format(sLib, iIconIndex))
                              Return rIcon




                          'Case LRI_METHOD.LoadIconByResID
                              'Dim hIcon = uomvb.Win32.GDI.GDIObjects.Icon.LoadIcon(hLib.DangerousGetHandle, resID)




                          Case LRI_METHOD.Load_RT_ICON
                              'Dim resID = LI.Value.ID.MAKEINTRESOURCE
                              'Dim hIcon = uomvb.Win32.GDI.GDIObjects.Icon.LoadImage(hLib.DangerousGetHandle, resID, 1, iIconSize, iIconSize, 0)

                              Dim iIconResID = IconIndexOrResID
                              Dim hLib = uomvb.Win32.SafeHandles.Win32LibHandle.GetRunningLibraryOrLoad_AsResource(sLib, True)
                              If(Not hLib.IsValid) Then Throw New Exception("Failed to load '{0}' as resource lib!".format(sLib))

                              Dim iIconSize = CInt(eIconSize)
                              Select Case eIconSize
                                  Case LRI_ICON_SIZE.Small : iIconSize = System.Windows.Forms.SystemInformation.SmallIconSize.Width
                                  Case LRI_ICON_SIZE.Large: iIconSize = System.Windows.Forms.SystemInformation.IconSize.Width
                              End Select
                              Dim hIcon = uomvb.Win32.GDI.Imaging.LoadImage(hLib,
                                                                          iIconResID,
                                                                          uomvb.Win32.GDI.Imaging.LoadImageuType.IMAGE_ICON,
                                                                          iIconSize,
                                                                          iIconSize)

                              If(Not hIcon.IsValid) Then Throw New Exception("LoadImage() Failed to load resource with ID = {0} from '{1}'!".format(iIconResID, sLib))
                              Dim ricon = Icon.FromHandle(hIcon)
                              Return ricon




                          Case LRI_METHOD.Load_RT_ICON_GROUP

                              Dim iIconResID = IconIndexOrResID
                              Dim IDR = uomvb.Win32.Resources.GetIconDirectory(sLib, iIconResID)
                              Dim aIconDirs = IDR.IconDirectory
                              If(Not aIconDirs.Any) Then Throw New Exception("GetIconDirectory({0}, {1}) Failed!".format(sLib, iIconResID))


                              Dim iIconSize = CInt(eIconSize)
                              Select Case eIconSize
                                  Case LRI_ICON_SIZE.Small : iIconSize = System.Windows.Forms.SystemInformation.SmallIconSize.Width
                                  Case LRI_ICON_SIZE.Large: iIconSize = System.Windows.Forms.SystemInformation.IconSize.Width
                              End Select

                              Dim szTarget As New Size(iIconSize, iIconSize)

                              aIconDirs = uomvb.Win32.Resources.GRPICONDIRENTRY.SortByQuality(aIconDirs)

                              Dim rDir = aIconDirs.Last

                              aIconDirs = (From R In aIconDirs
                                           Where(R.bWidth >= iIconSize) AndAlso(R.bHeight >= iIconSize)).ToArray

                              If aIconDirs.Any Then
                                  aIconDirs = (From R In aIconDirs
                                               Order By R.bWidth Descending, R.bHeight Descending, R.wBitCount, R.wPlanes).ToArray

                                  rDir = aIconDirs.First
                              End If

                              'Dim aFound = (From R In aIconsDirectory
                              'Order By R.wBitCount, R.wPlanes, R.bWidth, R.bHeight).ToArray
                              'Return aFound
                              Dim rIcon = rDir.CreateIconFromResourceEx(IDR.hLib)
                              If(rIcon Is Nothing) Then Throw New Exception("CreateIconFromResource({0}, RT_GROUP_ICON, {1}) Failed!".format(sLib, iIconResID))
                              Return rIcon

                          Case Else : Throw New ArgumentOutOfRangeException("eMethod")
                      End Select



        }

                             */



            /*



# Region "LoadResIcon_Safe"


        Friend Enum LRI_Libs As Integer
        shell32
        imageres
        End Enum
        Friend Const C_DEFAULT_LRI_Libs As LRI_Libs = LRI_Libs.shell32
        Friend Function LoadResWin32Icon_Safe(IconIndexOrResID As Integer,
                                          Optional eLib As LRI_Libs = C_DEFAULT_LRI_Libs,
                                          Optional eMethod As LRI_METHOD = C_DEFAULT_LRI_METHOD,
                                          Optional eIconSize As LRI_ICON_SIZE = C_LRI_ICON_SIZE,
                                          Optional UseDefultIconOnError As Boolean = True) As Global.System.Drawing.Icon

        Dim sLib = vbNullString
        Select Case eLib
            Case LRI_Libs.shell32 : sLib = uomvb.Win32.WINDLL_SHELL
            Case LRI_Libs.imageres : sLib = "imageres.dll"
            Case Else : Throw New ArgumentOutOfRangeException("eLib")
        End Select

        Return LoadResWin32Icon_Safe(IconIndexOrResID, sLib, eMethod, eIconSize, UseDefultIconOnError)
        End Function


        Public Enum LRI_ID As Integer
        shell32_FileBlank = 1
        shell32_FileDocument = 2
        shell32_App = 3
        shell32_Lupa = 24
        shell32_PowerShell = 25

        shell32_Folder_Open = 4
        shell32_Folder_Fonts = 39

        shell32_RecycleEmpty = 32
        shell32_RecycleFull = 33

        'shell32_Shield = 224
        shell32_Rename = 242
        shell32_Question = 24 ' 263
        shell32_Question3D = 324

        shell32_Camera = 309
        shell32_Cellphone = 310
        shell32_PDA = 314
        shell32_Scanner = 315
        shell32_ScannerPhoto = 316
        shell32_VideoCamera = 317


        shell32_Disk_SSD = 306
        shell32_Disk_SDCardBlue = 307
        shell32_Disk_SDCardBlack = 308

        End Enum
        Friend Function LoadResWin32Icon_Safe(ResID As LRI_ID,
                                          Optional eLib As LRI_Libs = C_DEFAULT_LRI_Libs,
                                          Optional eMethod As LRI_METHOD = C_DEFAULT_LRI_METHOD,
                                          Optional eIconSize As LRI_ICON_SIZE = C_LRI_ICON_SIZE,
                                          Optional UseDefultIconOnError As Boolean = True) As Global.System.Drawing.Icon

        Return LoadResWin32Icon_Safe(CInt(ResID), eLib, eMethod, eIconSize, UseDefultIconOnError)
        End Function












        Public Enum RT_ICON_PICK_MODES As Integer
        GetFirst

         <summary>GetMaxQality</summary>
        GetLast
        End Enum


        'Function LoadResIcon_RT_GROUP_ICON_Safe(sLib As String,
        '                                        ID As Integer,
        '                                        Optional Modes As RT_ICON_PICK_MODES = RT_ICON_PICK_MODES.GetLast,
        '                                        Optional UseDefultIconOnError As Boolean = True) As Global.System.Drawing.Icon

        '    Try
        '        Dim R = uomvb.Win32.Resources.GetIconDirectory(sLib, ID)
        '        If (Not R.IconDirectory.Any) Then Throw New Exception("GetIconDirectory({0}, {1}) Failed!".format(sLib, ID))

        '        Dim rIconDirectory = uomvb.Win32.Resources.GRPICONDIRENTRY.SortByQuality(R.IconDirectory)

        '        Dim rDir = rIconDirectory.First
        '        Select Case Modes
        '            Case RT_ICON_PICK_MODES.GetLast : rDir = rIconDirectory.Last
        '            Case Else : Throw New ArgumentOutOfRangeException("Modes")
        '        End Select

        '        Dim rIcon = rDir.CreateIcon(R.hLib.DangerousGetHandle)
        '        If (rIcon Is Nothing) Then Throw New Exception("CreateIconFromResource({0}, RT_GROUP_ICON, {1}) Failed!".format(sLib, ID))

        '        Return rIcon

        '    Catch ex As Exception
        '        If (UseDefultIconOnError) Then
        '            Call ex.eFIX_ERROR(False)
        '            Return uomvb.DEFAULT_APP_ICON
        '        End If

        '        Throw ex
        '    End Try
        'End Function

        'Function LoadResIcon_RT_GROUP_ICON_Safe_Shell32(ID As Integer,
        '                                        Optional Modes As RT_ICON_PICK_MODES = RT_ICON_PICK_MODES.GetLast,
        '                                        Optional UseDefultIconOnError As Boolean = True) As Global.System.Drawing.Icon

        '    Return LoadResIcon_RT_GROUP_ICON_Safe(uomvb.Win32.WINDLL_SHELL, ID, Modes, UseDefultIconOnError)
        'End Function






# End Region



             */

            #endregion




        }






        internal static class hooks
        {


            internal class CbtEventArgs ( IntPtr hwnd , string title , string className , bool isDialog ) : EventArgs()
            {
                public readonly IntPtr Handle = hwnd;
                public readonly string Title = title;
                public readonly string ClassName = className;
                public readonly bool IsDialogWindow = isDialog;
            }

            internal class HookEventArgs ( int code , IntPtr wp , IntPtr lp ) : EventArgs()
            {
                public readonly int HookCode = code;
                public readonly IntPtr wParam = wp;
                public readonly IntPtr lParam = lp;
            }

            internal class LocalWindowsHook : AutoDisposableUniversal
            {


                protected User32.SafeHHOOK? _hhook = null;
                protected readonly User32.HookProc _filterFunc;
                protected readonly User32.HookType _hookType;


                public event EventHandler<HookEventArgs> HookInvoked = delegate { };


                private LocalWindowsHook ( User32.HookProc? func ) : base()
                {
                    _filterFunc = func ?? CoreHookProc;
                    RegisterDisposeCallback(Uninstall , false);
                }

                public LocalWindowsHook ( User32.HookType hook ) : this(null) { _hookType = hook; }

                public LocalWindowsHook ( User32.HookType hook , User32.HookProc func ) : this(func) { _hookType = hook; }


                protected void OnHookInvoked ( HookEventArgs e )
                    => HookInvoked?.Invoke(this , e);


                protected IntPtr CoreHookProc ( int code , IntPtr wParam , IntPtr lParam )
                {
                    if ( code < 0 ) return User32.CallNextHookEx(_hhook! , code , wParam , lParam);

                    HookEventArgs hookEventArgs = new(code , wParam , lParam);
                    OnHookInvoked(hookEventArgs);
                    return User32.CallNextHookEx(_hhook! , code , wParam , lParam);
                }


                public void Install ()
                {
                    //Don't Work: m_hhook = SetWindowsHookEx(m_hookType, m_filterFunc, IntPtr.Zero, System.Threading.Thread.CurrentThread.ManagedThreadId);
                    _hhook = User32.SetWindowsHookEx(_hookType , _filterFunc , IntPtr.Zero , (int)Kernel32.GetCurrentThreadId());
                }


                public void Uninstall ()
                {
                    _hhook?.Dispose();// User32.UnhookWindowsHookEx (_hhook);
                    _hhook = null;
                }

            }


            internal class LocalCbtHook : LocalWindowsHook
            {

                protected IntPtr _hwnd = IntPtr.Zero;
                protected string _title = string.Empty;
                protected string _class = string.Empty;
                protected bool _isDialog = false;
                public event EventHandler<CbtEventArgs> WindowCreated = delegate { };
                public event EventHandler<CbtEventArgs> WindowDestroyed = delegate { };
                public event EventHandler<CbtEventArgs> WindowActivated = delegate { };

                public LocalCbtHook () : base(User32.HookType.WH_CBT) { base.HookInvoked += CbtHookInvoked!; }

                public LocalCbtHook ( User32.HookProc func ) : base(User32.HookType.WH_CBT , func) { base.HookInvoked += CbtHookInvoked!; }

                private void CbtHookInvoked ( object sender , HookEventArgs e )
                {
                    User32.HCBT hookCode = (User32.HCBT)e.HookCode;
                    IntPtr wParam = e.wParam;
                    IntPtr lParam = e.lParam;
                    switch ( hookCode )
                    {
                        case User32.HCBT.HCBT_CREATEWND:
                            HandleCreateWndEvent(wParam , lParam);
                            break;
                        case User32.HCBT.HCBT_DESTROYWND:
                            HandleDestroyWndEvent(wParam , lParam);
                            break;
                        case User32.HCBT.HCBT_ACTIVATE:
                            HandleActivateEvent(wParam , lParam);
                            break;
                    }
                }

                private void HandleCreateWndEvent ( IntPtr wParam , IntPtr lParam )
                {
                    UpdateWindowData(wParam);
                    OnWindowCreated();
                }

                private void HandleDestroyWndEvent ( IntPtr wParam , IntPtr lParam )
                {
                    UpdateWindowData(wParam);
                    OnWindowDestroyed();
                }

                private void HandleActivateEvent ( IntPtr wParam , IntPtr lParam )
                {
                    UpdateWindowData(wParam);
                    OnWindowActivated();
                }


                private void UpdateWindowData ( IntPtr wParam )
                {
                    _hwnd = wParam;
                    _class = _hwnd.GetClassName();
                    _title = _hwnd.GetWindowText();
                    _isDialog = _class == "#32770";
                }

                protected virtual void OnWindowCreated () => WindowCreated?.Invoke(this , PrepareEventData());

                protected virtual void OnWindowDestroyed () => WindowDestroyed?.Invoke(this , PrepareEventData());

                protected virtual void OnWindowActivated () => WindowActivated?.Invoke(this , PrepareEventData());

                private CbtEventArgs PrepareEventData () => new(_hwnd , _title , _class , _isDialog);

            }


        }





    }




}



#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning restore IDE1006 // Naming Styles

#endif