#nullable enable

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using System.Windows.Forms;
using uom.AutoDisposable;
using uom.AutoDisposable.SafeContainers;
using uom.controls;
using Vanara.Extensions;

//https://github.com/dahall/Vanara
using Vanara.PInvoke;
using static System.Object;
using static uom.Extensions.Extensions_WinForms_Controls;
using static Vanara.PInvoke.Gdi32;
using static Vanara.PInvoke.User32;
using ComboBoxStyle = System.Windows.Forms.ComboBoxStyle;


//using static System.Runtime.InteropServices.JavaScript.JSType;
//using static System.Windows.Forms.Design.AxImporter;
using Con = uom.Extensions.Extensions_Console;
using Object = System.Object;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.

namespace uom
{

    /// <summary>Constants</summary>
    internal static partial class constants
    {

        internal static readonly System.Drawing.Icon ICON_SYSTEM_APP = SystemIcons.Application;
        internal static readonly Icon ICON_SYSTEM_SHIELD = SystemIcons.Shield;
        //internal static readonly   Icon ICON_SYSTEM_SHIELD_SMALL = ICON_SYSTEM_SHIELD.Create_Small;
    }


    internal static partial class AppInfo
    {

        /// <summary>WinForms + WPF</summary>
        public static bool isInDesignerMode ( Control? ctl = null )
        {
            if ( IsInDesignerMode_WPF ) return true;

            while ( ctl != null )
            {
                if ( null != ctl.Site && ctl.Site.DesignMode ) return true;
                ctl = ctl.Parent;
            }
            return IsInDesignerMode_WinForms;
        }
    }


    internal static partial class AppTools
    {

#if !ANDROID


        public static string FormatFormTitle ( string? title = null )
        {
            var asm = uom.AppInfo.AppAssembly;


            title ??= uom.AppInfo.AssemblyTitle!;

            var el = uom.AppInfo.IsElevated();
            return $"{title} ({uom.AppInfo.AssemblyFileVersionAttribute})" + (el ? " [Admin]" : string.Empty);
        }


        internal static string ConsoleAppHeader ()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Con.CreateHSplitter());
            sb.AppendLine($"{System.Windows.Forms.Application.ProductName} v{System.Windows.Forms.Application.ProductVersion}");
            sb.AppendLine();
            sb.AppendLine(uom.AppInfo.Description);
            sb.Append(Con.CreateHSplitter());
            return sb.ToString();
        }


        /// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>

        public static Image GetEmbeddedResourceAsImage ( string embeddedResourceFileSuffix )
        {
            using Stream stream = GetEmbeddedResourceStream(embeddedResourceFileSuffix);
            return Image.FromStream(stream);
        }

        /// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>

        public static async Task<Image> GetEmbeddedResourceAsImageAsync ( string embeddedResourceFileSuffix )
            => await Task.Factory.StartNew(() => GetEmbeddedResourceAsImage(embeddedResourceFileSuffix));



        /// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>

        public static Icon GetEmbeddedResourceAsIcon ( string embeddedResourceFileSuffix )
        {
            using Stream stream = GetEmbeddedResourceStream(embeddedResourceFileSuffix);
            return new(stream);
        }

        /// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>

        public static async Task<Icon> GetEmbeddedResourceAsIconAsync ( string embeddedResourceFileSuffix )
            => await Task.Factory.StartNew(() => GetEmbeddedResourceAsIcon(embeddedResourceFileSuffix));



#endif




    }


    internal static partial class OS
    {

        #region SystemUpTime


        /// <summary>
        /// Task Manager 's Performance tab (CPU section) shows the Up time information of the system, but you may be wondering why your boot up time doesn’t match the Up time data reported.<br/>
        /// This Is because Task Manager Or WMI wouldn't deduct the sleep/hibernation time when calculating up time, 
        /// and with Fast Startup introduced and enabled by default in Windows 8 (and higher), the Up time reported may not match with your actual last boot up time.
        /// <c>
        /// Fast startup Is a hybrid Of cold startup + hibernate; 
        /// </c><br/>
        /// When you shutdown the computer With fast startup enabled, the user accounts are logged off completely.<br/>
        /// Then the system goes To hibernate mode (instead Of traditional cold shutdown), so that the Next boot up till the logon screen will be quicker (30-70 % faster). <br/>
        /// If you have old hardware you may Not see much difference In startup times.
        /// </summary>
        /// <remarks>
        /// Windows 8 and old, hibernation reset this counter to zerro.
        /// <br/>		
        /// Windows 10: Incorrect Uptime Reported by Task Manager and WMI
        /// </remarks>
        public static TimeSpan GetSystemUpTime_FromSystemCounters ()
        {
            using var rUpTime = new PerformanceCounter(
                "System" ,
                "System Up Time");
            rUpTime.NextValue(); // Call this an extra time before reading its value
            return TimeSpan.FromSeconds(rUpTime.NextValue());

            // Windows 10: Incorrect Uptime Reported by Task Manager and WMI
            // Using OS As New ROOT.CIMV2.OperatingSystem
            // Dim dtBoot = OS.LastBootUpTime
            // Return dtBoot
            // End Using
        }

        public static async Task<TimeSpan> GetSystemUpTime_FromSystemCountersAsync ()
            => await Task.Factory.StartNew(() => GetSystemUpTime_FromSystemCounters() , TaskCreationOptions.LongRunning);


        #endregion


        internal static DateTime GetOSBootDate_FromSystemCounters ()
            => DateTime.Now.Subtract(GetSystemUpTime_FromSystemCounters());


        /// <param name="onCapture">Return true if do not dispose image</param>

        public static void GetScreenshots ( Func<Bitmap , bool> onCapture )
            => System.Windows.Forms.Screen.AllScreens
            .forEach(scr =>
            {
                // capture
                Bitmap bmCapt = new(scr.Bounds.Width , scr.Bounds.Height , PixelFormat.Format32bppArgb);
                var rcCapt = scr.Bounds;
                using ( Graphics g = Graphics.FromImage(bmCapt) )
                {
                    g.CopyFromScreen(rcCapt.Left , rcCapt.Top , 0 , 0 , rcCapt.Size);
                }

                // process result
                bool doNotDispose = false;
                try
                {
                    doNotDispose = onCapture.Invoke(bmCapt);
                }
                finally
                {
                    if ( !doNotDispose ) bmCapt.Dispose();
                }
            });



        public static Bitmap[] GetScreenshotsAsImage ()
        {
            List<Bitmap> images = [];
            GetScreenshots(frame =>
            {
                images.Add(frame);
                return true;
            });
            return [ .. images ];
        }



        public static System.IO.FileInfo[] GetScreenshotsAsFiles ( ImageFormat fmt , string fileExt = "jpg" )
        {
            List<System.IO.FileInfo> fileNames = [];
            GetScreenshots(frame =>
            {
                var filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath() , Guid.NewGuid().ToString() + '.'.ToString() + fileExt);
                frame.Save(filePath , fmt);
                fileNames.Add(new(filePath));
                return false;
            });
            return [ .. fileNames ];
        }


        internal static partial class Shell
        {

            #region RegisterAutorun
            internal enum APP_STARTUP_MODES
            {
                Registry,
                AutoStartFolder
            }


            public static void RegisterAutorun ( APP_STARTUP_MODES Mode , bool forAllUsers , bool unregister = false )
            {
                switch ( Mode )
                {
                    case APP_STARTUP_MODES.AutoStartFolder:
                        {
                            // TODO: Создаем ярлык в папке автозагрузки
                            throw new NotImplementedException();
                        }

                    case APP_STARTUP_MODES.Registry:
                        {
                            const string KEY_RUN_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

                            var fiEXE = System.Windows.Forms.Application.ExecutablePath.eToFileInfo();
                            _ = fiEXE ?? throw new Exception("Application.ExecutablePath = NULL!");

                            using var keyAutoRun = (forAllUsers ? Registry.LocalMachine : Registry.CurrentUser)
                                .OpenSubKey(KEY_RUN_PATH , true);

                            if ( keyAutoRun == null )
                            {
                                throw new Exception($"Failed to open '{KEY_RUN_PATH}' key!");
                            }

                            if ( unregister )
                            {
                                keyAutoRun
                                    .GetValueNames()?
                                    .Where(s => s.ToLower() == fiEXE.Name.ToLower())?
                                    .forEach(foundFile => keyAutoRun.DeleteValue(foundFile));
                            }
                            else
                            {
                                keyAutoRun
                                    .SetValue(fiEXE.Name , fiEXE.FullName.eEnclose());
                            }

                            keyAutoRun.Flush();
                            break;
                        }
                }

            }
            #endregion

            #region Shell_RegisterContextMenu

            internal const string CS_DEFAULT_CMDLINE_ARG = "\"%1\"";
            private const string CS_REG_KEY_SHELL = "shell";
            private const string CS_REG_KEY_COMMAND = "Command";
            private const string C_REG_CLASS_DIRECTORY = "Directory";


            /// <summary>Register ShellContectMenu for specifed class</summary>
            /// <param name="HCCRClass">Имя класса в реестре, например 'Directory' 'file' '.exe' 'CorelDraw.Graphic.19' 'brmFile')</param>
            /// <param name="shellActionName">Внутреннее имя ключа операции в реестре, например 'Open'</param>
            /// <param name="shellActionDisplayName">То, что видно в контекстном меню проводника</param>
            /// <param name="ExecutablePath">Можно не указывать, если эта же программа</param>
            /// <param name="CmdLineArgument"></param>
            /// <remarks></remarks>
            internal static string ContextMenu_RegisterForClass (
                string HCCRClass ,
                string shellActionName ,
                string shellActionDisplayName ,
                string? executablePath = null ,
                string? cmdLineArgsPrefix = "" ,
                string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG )
            {
                if ( HCCRClass.isNullOrWhiteSpace )
                    throw new ArgumentNullException(nameof(HCCRClass));

                if ( shellActionName.isNullOrWhiteSpace )
                    throw new ArgumentNullException(nameof(shellActionName));

                if ( shellActionDisplayName.isNullOrWhiteSpace )
                    throw new ArgumentNullException(nameof(shellActionDisplayName));


                executablePath ??= System.Windows.Forms.Application.ExecutablePath;

                string keyPath = string.Join(@"\" , new[] { HCCRClass , CS_REG_KEY_SHELL , shellActionName });
                using var keyClass = Registry.ClassesRoot.CreateSubKey(keyPath , RegistryKeyPermissionCheck.ReadWriteSubTree);
                keyClass.SetValue("" , shellActionDisplayName);
                keyClass.Flush();

                using var hkCommand = keyClass.CreateSubKey(CS_REG_KEY_COMMAND , RegistryKeyPermissionCheck.ReadWriteSubTree);
                string commandString = ContextMenu_CreateRegCommandString(executablePath , cmdLineArgsPrefix , cmdLineArgs);
                hkCommand.SetValue("" , commandString);
                hkCommand.Flush();
                return commandString;
            }

            private static string ContextMenu_CreateRegCommandString (
               string executablePath ,
               string? cmdLineArgsPrefix = "" ,
               string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG )
                => $"\"{executablePath.Trim()}\" {(cmdLineArgsPrefix ?? "").Trim()} {cmdLineArgs.Trim()}".Trim();


            internal static bool ContextMenu_IsRegisteredForClass (
                string HCCRClass ,
                string RegistryActionName ,
                string ActionDisplayName ,
                string? executablePath = null ,
                string? cmdLineArgsPrefix = "" ,
                string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG )
            {
                if ( HCCRClass.isNullOrWhiteSpace )
                    throw new ArgumentNullException(nameof(HCCRClass));

                if ( RegistryActionName.isNullOrWhiteSpace )
                    throw new ArgumentNullException(nameof(RegistryActionName));

                if ( ActionDisplayName.isNullOrWhiteSpace )
                    throw new ArgumentNullException(nameof(ActionDisplayName));

                executablePath ??= System.Windows.Forms.Application.ExecutablePath;

                string sKey = string.Join(@"\" , new[] { HCCRClass , CS_REG_KEY_SHELL , RegistryActionName });
                using RegistryKey? keyClass = Registry.ClassesRoot.OpenSubKey(sKey , false);
                string? defVlue = keyClass?.eGetValue_StringOrEmpty("");
                if ( defVlue != ActionDisplayName )
                {
                    return false;
                }

                using RegistryKey? hkCommand = keyClass?.OpenSubKey(CS_REG_KEY_COMMAND , false);
                string commandString = ContextMenu_CreateRegCommandString(executablePath , cmdLineArgsPrefix , cmdLineArgs);
                string? regCommandValue = hkCommand?.eGetValue_StringOrEmpty("");
                return commandString == regCommandValue;
            }


            #region ContextMenu_RegisterForDirectory

            internal static bool ContextMenu_IsRegisteredForDirectory (
                string RegistryActionName ,
                string ActionDisplayName ,
                string? executablePath = null ,
                string cmdLineArgsPrefix = "" ,
                string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG )
                => ContextMenu_IsRegisteredForClass(C_REG_CLASS_DIRECTORY , RegistryActionName , ActionDisplayName , executablePath , cmdLineArgsPrefix , cmdLineArgs);


            internal static void ContextMenu_RegisterForDirectory (
                string RegistryActionName ,
                string ActionDisplayName ,
                string? executablePath = null ,
                string cmdLineArgsPrefix = "" ,
                string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG )
                => ContextMenu_RegisterForClass(C_REG_CLASS_DIRECTORY , RegistryActionName , ActionDisplayName , executablePath , cmdLineArgsPrefix , cmdLineArgs);


            internal static void ContextMenu_UnRegisterForDirectory ( string registryActionName )
                => ContextMenu_UnRegister(C_REG_CLASS_DIRECTORY , registryActionName);

            #endregion


            #region ContextMenu_RegisterForFile(s)

            /// <param name="RegistryActionName">
            /// !!! Registry has some unknown small limit to ActionKeyName in Shell key, so make it max shorter!!!
            /// </param>
            internal static string ContextMenu_RegisterForAllFiles (
                string RegistryActionName ,
                string ActionDisplayName ,
                string? executablePath = null ,
                string cmdLineArgsPrefix = "" ,
                string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG )
                => ContextMenu_RegisterForClass("*" , RegistryActionName , ActionDisplayName , executablePath , cmdLineArgsPrefix , cmdLineArgs);


            /// <summary>Регистрация для заданного разрешения !!!НЕ КОАССА!!!</summary>
            /// <param name="filesExtensions">Разрешение файла, например '.exe' '.png')</param>
            /// <param name="RegistryActionName">Внутреннее имя ключа операции в реестре, например 'Open'
            /// !!! Registry has some unknown small limit to ActionKeyName in Shell key, so make it max shorter!!!
            /// </param>
            /// <param name="ActionDisplayName">То, что видно в контекстном меню проводника</param>
            /// <param name="ExecutablePath">Можно не указывать, если эта же программа</param>
            /// <param name="CmdLineArgument"></param>
            internal static string[] ContextMenu_RegisterForFileExt (
                string[] filesExtensions ,
                string RegistryActionName ,
                string ActionDisplayName ,
                string? executablePath = null ,
                string cmdLineArgsPrefix = "" ,
                string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG )
                => filesExtensions
                    .Select(ext => ContextMenu_RegisterForFileExt(ext , RegistryActionName , ActionDisplayName , executablePath , cmdLineArgsPrefix , cmdLineArgs))
                    .ToArray()
                    ;


            /// <summary>Регистрация для заданного разрешения !!!НЕ КОАССА!!!</summary>
            /// <param name="fileExtensionWithDot">Разрешение файла, например '.exe' '.png')</param>
            /// <param name="RegistryActionName">Внутреннее имя ключа операции в реестре, например 'Open'</param>
            /// <param name="ActionDisplayName">То, что видно в контекстном меню проводника
            /// !!! Registry has some unknown small limit to ActionKeyName in Shell key, so make it max shorter!!!
            /// </param>
            /// <param name="ExecutablePath">Можно не указывать, если эта же программа</param>
            /// <param name="CmdLineArgument"></param>
            internal static string ContextMenu_RegisterForFileExt (
                string fileExtensionWithDot ,
                string RegistryActionName ,
                string ActionDisplayName ,
                string? executablePath = null ,
                string cmdLineArgsPrefix = "" ,
                string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG )
            {
                if ( fileExtensionWithDot.isNullOrWhiteSpace )
                {
                    throw new ArgumentNullException(nameof(fileExtensionWithDot));
                }

                // Читаем класс файла из HKEY_CLASSES_ROOT\.pdf

                string readFileClass ()
                {
                    using var hkeyFileExtension = Registry.ClassesRoot.OpenSubKey(fileExtensionWithDot);
                    return hkeyFileExtension!.eGetValue_StringOrEmpty("" , null);
                }

                Func<string> ff = readFileClass;
                var result = ff.tryCatch();

                var fileClass = result.IsSuccess
                    ? result.Value
                    : string.Empty;

                if ( IsWin10OrLater )
                {
                    // в W10 контекстные команды работают по другому: 
                    // HKEY_CLASSES_ROOT\SystemFileAssociations\.EXT\Shell\Action\Command = ""
                    fileClass = @"SystemFileAssociations\" + fileExtensionWithDot;

                    #region То работает то нет
                    // Dim sW10SystemFileAssociationsClass = erunOn_TryCatch_Func(Of string)(Function()
                    // Using hkeySystemFileAssociations = Global.Microsoft.WinAPI.Registry.ClassesRoot.OpenSubKey()
                    // Return CStr(hkeyFileExtension.GetValue("", vbNullString, Microsoft.WinAPI.RegistryValueOptions.DoNotExpandEnvironmentNames))
                    // End Using
                    // End Function, vbNullString, False)

                    // в W10 контекстные команды почему-то работают по другому...
                    // есть раздел HKEY_CLASSES_ROOT\.pdf\OpenWithProgids
                    // где есть список "обработчиков"

                    // Using hkOpenProgIDs = Global.Microsoft.WinAPI.Registry.ClassesRoot.OpenSubKey(FileExt & "\OpenWithProgids")
                    // If (hkOpenProgIDs IsNot Nothing) Then
                    // Dim aOpenProgIDsValues = hkOpenProgIDs.ExtReg_GetAllValues

                    // aOpenProgIDsValues = (From T In aOpenProgIDsValues
                    // Where ((T.Kind = RegistryValueKind.string) AndAlso T.ValueName. IsNotNullOrWhiteSpace)).ToArray

                    // If aOpenProgIDsValues.Any Then
                    // Dim FirstValue = aOpenProgIDsValues.First
                    // sFileClass = FirstValue.ValueName
                    // End If
                    // End If
                    // End Using
                    #endregion
                }

                if ( fileClass.isNullOrWhiteSpace )
                {
                    fileClass = fileExtensionWithDot; // В разделе не указан Класс файла. Сделаем раздел прямо на самом расширении
                }

                return ContextMenu_RegisterForClass(
                    fileClass! ,
                    RegistryActionName ,
                    ActionDisplayName ,
                    executablePath ,
                    cmdLineArgsPrefix ,
                    cmdLineArgs);
            }

            #endregion

            /// <summary>Unregister this action on every registry class</summary>
            internal static void ContextMenu_UnRegisterAction ( string[]? registryActionNames )
                => registryActionNames?.forEach(actionName => ContextMenu_UnRegisterAction(actionName));

            /// <inheritdoc cref="ContextMenu_UnRegisterAction" />
            internal static int ContextMenu_UnRegisterAction ( string RegistryActionName , bool useActionNameAsPrefix = false )
            {
                int totalUnregistered = Registry
                    .ClassesRoot?
                    .GetSubKeyNames()?
                    .Select(key => ContextMenu_UnRegister(key , RegistryActionName , useActionNameAsPrefix))?
                    .Sum() ?? 0;

                if ( !IsWin10OrLater )
                {
                    return totalUnregistered;
                }

                try
                {
                    // HKEY_CLASSES_ROOT\SystemFileAssociations\.EXT\Shell\Action\Command = ""
                    const string C_W10_SystemFileAssociations = "SystemFileAssociations";

                    using var hkeyW10SystemFileAssociations = Registry
                        .ClassesRoot?
                        .OpenSubKey(C_W10_SystemFileAssociations , RegistryKeyPermissionCheck.ReadSubTree);

                    totalUnregistered += hkeyW10SystemFileAssociations?
                        .GetSubKeyNames()?
                        .Select(key => ContextMenu_UnRegister(
                            @$"{C_W10_SystemFileAssociations}\{key}" ,
                            RegistryActionName ,
                            useActionNameAsPrefix))?
                            .Sum() ?? 0;
                }
                catch { }
                return totalUnregistered;
            }


            internal static int ContextMenu_UnRegister (
                string registryClass ,
                string registryActionName ,
                bool useActionNameAsPrefix = false )
            {
                if ( registryClass.isNullOrWhiteSpace )
                {
                    throw new ArgumentNullException(nameof(registryClass));
                }

                string sShellKey = @$"{registryClass}\{CS_REG_KEY_SHELL}";

                string[]? keyNamesToKill = null;

                using ( var keyShell = Registry
                    .ClassesRoot
                    .OpenSubKey(sShellKey , RegistryKeyPermissionCheck.ReadSubTree) )
                {
                    string actionL = registryActionName.ToLower().Trim();
                    keyNamesToKill = keyShell?
                        .GetSubKeyNames()?
                        .Where(keyName => useActionNameAsPrefix
                        ? keyName.ToLower().Trim().StartsWith(actionL)
                        : (keyName.ToLower().Trim() == actionL))?
                        .ToArray();

                }

                if ( keyNamesToKill != null && keyNamesToKill.Length != 0 )
                {
                    using var keyShell = Registry
                    .ClassesRoot
                    .OpenSubKey(sShellKey , RegistryKeyPermissionCheck.ReadWriteSubTree);

                    keyNamesToKill?
                        .ToList()?
                        .ForEach(keyNameToKill => keyShell?.DeleteSubKeyTree(keyNameToKill)
                        );
                    keyShell?.Flush();
                }
                return keyNamesToKill?.Length ?? 0;
            }


            public static void AssociateFileWithApp (
                string FileExtWithoutDot ,
                string FileDecription ,
                string? iconFile = null ,
                int IconIndex = 0 )
            {
                string newRegKey = "." + FileExtWithoutDot;
                using var keyExt = Registry.ClassesRoot.CreateSubKey(newRegKey);
                if ( null == keyExt )
                {
                    throw new Exception($"Failed to create '{newRegKey}' registry key!");
                }

                string sAppKeyName = string.Format("{0} {1}" , System.Windows.Forms.Application.CompanyName , System.Windows.Forms.Application.ProductName).Trim();
                keyExt.SetValue("" , sAppKeyName);
                keyExt.Flush();
                using var keyAppRoot = Registry.ClassesRoot.CreateSubKey(sAppKeyName);
                if ( null == keyAppRoot )
                {
                    throw new Exception($"Failed to create '{sAppKeyName}' registry key!");
                }

                keyAppRoot.SetValue("" , FileDecription);
                keyAppRoot.Flush();

                iconFile ??= System.Windows.Forms.Application.ExecutablePath;
                using var keyIcon = keyAppRoot.CreateSubKey("DefaultIcon");
                keyIcon!.SetValue("" , $"\"{iconFile}\", {IconIndex}");
                keyIcon!.Flush();
            }


            #endregion


            public static void RegisterSoundSchemeEvent ( string eventName , string soundFilePath )
            {
                string appName = System.Windows.Forms.Application.ProductName!;
                using var keySoundEvent = Registry.CurrentUser.CreateSubKey(@"AppEvents\Schemes\Apps\" + appName)!;
                keySoundEvent.SetValue("" , appName);
                keySoundEvent.Flush();

                string keyPath = eventName + @"\.current";
                var keyCurrent = keySoundEvent.OpenSubKey(keyPath);
                if ( null == keyCurrent ) keyCurrent = keySoundEvent.CreateSubKey(keyPath);
                using ( keyCurrent )
                {
                    keyCurrent.SetValue("" , soundFilePath);
                    keyCurrent.Flush();
                }
            }
        }



        internal static partial class UserAccounts
        {

            #region Список скрытых в реестре пользователей


            private const string CS_KEY_WINLOGON = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";
            private const string CS_KEY_HIDDEN_USERS_LIST = @"SpecialAccounts\UserList";
            public const string CS_KEY_HIDDEN_USERS_LIST_FULL = CS_KEY_WINLOGON + @"\" + CS_KEY_HIDDEN_USERS_LIST;


            /// <summary>Список скрытых в реестре пользователей</summary>
            public static string[] RegistryHiddenUsers_GetUsers ()
            {
                if ( !IsRegistryHiddenUsersKeyExist() )
                {
                    throw new FileNotFoundException(CS_KEY_HIDDEN_USERS_LIST_FULL);
                }

                using var keyFULL = Registry.LocalMachine.OpenSubKey(CS_KEY_HIDDEN_USERS_LIST_FULL , false);
                return keyFULL?
                    .GetValueNames().Where(
                        sUser =>
                        (keyFULL!.GetValueKind(sUser) == RegistryValueKind.DWord) && (keyFULL!.eGetValue_Int32(sUser , -1)!.Value! == 0)
                    )
                    .ToArray()!;
            }


            /// <summary>Checks for existing 'HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList'</summary>
            public static bool IsRegistryHiddenUsersKeyExist ()
            {
                using var keyFULL = Registry.LocalMachine.OpenSubKey(CS_KEY_HIDDEN_USERS_LIST_FULL , false);
                return null != keyFULL;
            }


            #region Open Keys
            private static RegistryKey OpenWinLogonKey ( bool bWritable )
                => Registry.LocalMachine.OpenSubKey(CS_KEY_WINLOGON , bWritable)!;


            private static RegistryKey OpenUserListKey ( RegistryKey keyWINLOGON , bool bWritable , bool bCreateIfNotExist )
            {
                RegistryKey? keyUserList = keyWINLOGON.OpenSubKey(CS_KEY_HIDDEN_USERS_LIST , bWritable);
                return keyUserList
?? (!bCreateIfNotExist
                    ? throw new Exception("No hidden user profiles exist!")
                    : keyWINLOGON.CreateSubKey(CS_KEY_HIDDEN_USERS_LIST , RegistryKeyPermissionCheck.ReadWriteSubTree)
                    ?? throw new Exception($"Failed to create registry key '{CS_KEY_HIDDEN_USERS_LIST}'!"));
            }
            #endregion

            #region ModifyVisibility

            public static void RegistryHiddenUsers_SetVisibility ( string UserName , bool bVisible )
            {
                // Чтобы скрыть отдельных пользователей в окне приветствия, 
                // запустите редактор реестра и перейдите в раздел 
                // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsNT\CurrentVersion\Winlogon\SpecialAccounts\UserList
                // Теперь создайте новый параметр DWORD, назовите его так же, как имя пользователя, и укажите в качестве значения ноль.
                // Например, если нужно скрыть пользователя Petr, создайте параметр Petr со значением 0.
                // После этого имя Petr не будет появляться в окне приветствия Windows.


                // чтобы скрыть учетную запись в экране приветствия необходимо в
                // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon
                // создать подраздел с именем SpecialAccounts, а в нем еще один с именем UserList.
                // Затем в разделе UserList создайте параметр типа REG_DWORD с именем равным имени учетной записи, которую необходимо скрыть
                // и со значением равным 0 (ноль),
                // соответственно для отображения этой учетной записи в экране приветствия значение параметра нужно будет установить 1 (один) или удалить параметр.
                // Также можете текст кода скопировать в текстовый файл, исправить имя параметра ("User") на имя учетной записи, которую хотите скрыть, сохранить файл, присвоить ему расширение *.reg и запустить полученный файл, согласившись с внесением изменений в реестр.
                // Windows Registry Editor Version 5.00
                // [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList]
                // "Blast"=dword:00000000
                // Учтите, что вместе со скрытием учетной записи в экране приветствия также эта учетная запись будет скрыта и из апплета "Учетные записи пользователей" в Панели управления.
                // Конечно управлять учетной записью (настраивать) вы сможете из оснастки "Локальные пользователи и группы", которую можно открыть через Пуск - Выполнить - lusrmgr.msc, а так же в классическом управлении учетными записями пользователей: Пуск - Выполнить - control
                // Чтобы скрыть отдельных пользователей в окне приветствия, запустите редактор реестра и перейдите в раздел HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsNT\CurrentVersion\ Winlogon\SpecialAccounts\UserList. Теперь создайте новый параметр DWORD, назовите его так же, как имя пользователя, и укажите в качестве значения ноль. Например, если нужно скрыть пользователя Petr, создайте параметр Petr со значением 0. После этого имя Petr не будет появляться в окне приветствия Windows.


                using var keyWINLOGON = OpenWinLogonKey(true);
                using var keyUserList = OpenUserListKey(keyWINLOGON! , true , !bVisible);
                int iCurrentValue = keyUserList!.eGetValue_Int32(UserName , int.MaxValue)!.Value;
                if ( (iCurrentValue == int.MaxValue) && bVisible )
                {
                    return; //'HIDE' Registry value is not exist, and bVisible=true
                }

                int iTargetVisible = bVisible.toInt32ABS();
                if ( iCurrentValue == iTargetVisible )
                {
                    return;
                }
                // Call OutString("Меняем значение видимости...")
                keyUserList!.SetValue(UserName , iTargetVisible , RegistryValueKind.DWord);
            }
            #endregion

            /// <summary>Удаляем из реестра раздел со скрытыми пользователями</summary>
            public static void RegistryHiddenUsers_DeleteRegKey ()
            {
                string killSubKeyName = CS_KEY_HIDDEN_USERS_LIST.Split('\\').First();
                using RegistryKey keyWINLOGON = OpenWinLogonKey(true);
                keyWINLOGON?
                    .GetSubKeyNames()?
                    .forEach(sSubKey =>
                    {
                        if ( sSubKey.ToLower() == killSubKeyName.ToLower() )
                        {
                            keyWINLOGON!.DeleteSubKeyTree(sSubKey);
                        }
                    });
            }


            #endregion


            /// <summary>Кэш для GetCurrentUser</summary>
            private static readonly EventArgs _CurrentUserSyncLock = new();
            private static WindowsIdentity? _CurrentUser = null;
            private static SecurityIdentifier? _CurrentSID = null;


            /// <summary>Возвращает данные о текущем пользователе (из под которого вызывается) ПО-УМОЛЧАНИЮ ИСПОЛЬЗУЕТ КЭШИРОВАНИЕ МЕЖДУ ВЫЗОВАМИ!!!</summary>
            /// <param name="notUseCache">Не использовать ранее кэшированные данные, а запросить новые</param>
            /// <returns>Global.System.Security.Principal.WindowsIdentity</returns>
            internal static WindowsIdentity GetCurrentUser ( bool notUseCache = false )
            {
                if ( notUseCache )
                {
                    return WindowsIdentity.GetCurrent();
                }

                lock ( _CurrentUserSyncLock )
                {
                    _CurrentUser ??= WindowsIdentity.GetCurrent();
                    return _CurrentUser;
                }
            }


            internal static SecurityIdentifier GetCurrentUserSID ()
            {
                _CurrentSID ??= GetCurrentUser(false).User;
                return _CurrentSID!;
            }


            internal static WindowsPrincipal GetCurrentUserPrincipal () => new(GetCurrentUser());


            internal static bool UserInAdminGroup () => GetCurrentUserPrincipal().IsInRole(WindowsBuiltInRole.Administrator);


            /// <summary>domain/user</summary>
            internal static string GetCurrentUserName () => GetCurrentUser().Name;


            ///// <summary>Имя пользователя без домена</summary>
            //internal static string GetCurrentUserShortName() => GetCurrentUserSID().LookupAccountSidUserName();




            #region User Tile Image

            [DllImport(Lib.Shell32 , EntryPoint = "#261" , CharSet = CharSet.Unicode , PreserveSig = false)]
            private static extern void GetUserTilePath (
                [In, MarshalAs(UnmanagedType.LPWStr)] string? username ,
                [In] uint whatever ,
                [In, Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder picpath ,
                [In] int maxLength );


            /// <summary>When called, OS create Userxxx.bmp file in 'C:\Users\xxx\AppData\Local\temp\' (for caller user) and returns path to that file.</summary>
            /// <param name="UserName">username: use null For current user</param>
            /// <remarks>User account pictures a placed at 'C:\Users\xxx\AppData\Roaming\Microsoft\Windows\AccountPictures</remarks>
            /// <completionlist cref=""/>
            public static string GetUserTilePath ( string? UserName = null )
            {
                StringBuilder sb = new(1000);
                GetUserTilePath(UserName , 0x80000000 , sb , sb.Capacity);
                return sb.ToString();
            }


            /// <inheritdoc cref="GetUserTilePath(string?)" />
            public static Image? GetUserTileImage ( string? UserName = null )
            {
                string sUserImagePath = GetUserTilePath(UserName);
                if ( File.Exists(sUserImagePath) )
                {
                    using var imgFile = Image.FromFile(sUserImagePath);
                    // Надо использовать клонирование, чтобы не занимать файл изображения, а освободить его сразу после чтения
                    return imgFile!.CloneT();
                }

                return null;

                // ********************** Domain UserAccountPicture stored in Outlook:
                // Newer versions of Office (2010+) use Active Directory to retrieve And display user photos. 
                // It 's a useful feature that also adds visual interest.
                // I can look quickly at the thumbnails at the bottom of an email or meeting request in Outlook to see who’s invited;
                // this is much faster than reading through the semi-colon delimited list of email addresses.
                /*                 
				Private void GetUserPicture(string userName)
				{
					var directoryEntry = New DirectoryEntry("LDAP://YourDomain");
					var directorySearcher = New DirectorySearcher(directoryEntry);
					directorySearcher.Filter = string.Format("(&(SAMAccountName={0}))", UserName);
					var user = directorySearcher.FindOne();

					var bytes = user.Properties["thumbnailPhoto"][0] as byte[];

					Using(var ms = New MemoryStream(bytes))
				 {
						var imageSource = New BitmapImage();
						imageSource.BeginInit();
						imageSource.StreamSource = MS;
						imageSource.EndInit();

						uxPhoto.Source = imageSource;
					}
				}
				*/
            }

            // [DllImport("shell32.dll", EntryPoint = "#262", CharSet = CharSet.Unicode, PreserveSig = false)]
            // Public Static extern void SetUserTile(string username, int whatever, string picpath);

            // [STAThread]
            // Static void Main(string[] args)
            // {
            // SetUserTile(args[0], 0, args[1]);
            // }


            // I have found relevant information at \HKEY_CURRENT_USER\Volatile Envirnment, but Not the exact path.
            // My guess Is that the avatar Is always at C: \Users\UserName\AppData\Local\Temp\ And the file name itself can be found from this algorithm:

            // // Note that $XYZ$ means \HKEY_CURRENT_USER\Volatile Envirnment\XYZ
            // If $USERDOMAIN$ = "" Then
            // Return $USERNAME$.Substring(0, $USERNAME$.IndexOf('.'));
            // Else
            // Return $USERDOMAIN$ + "+" + $USERNAME$.Substring(0, $USERNAME$.IndexOf('.'));
            // Again, just a guess.

            // P.S.: There Is Volatile Environment For all users, If you look at \HKEY_USERS. If you want a specific user, iterate over all users And check In the Volatile Environment For the user name (the Sub-keys Of \HKEY_USERS are just random strings, so you must look inside).


            //<summary>Работает некорректно!!!</summary>
            // Public Shared Function GetUserTileImage() As Image
            // Dim sFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\Temp\" & Environment.UserName & ".bmp"
            // If (File.Exists(sFile)) Then Return Image.FromFile(sFile)
            // Return Nothing
            // End Function
            #endregion



            internal static SecurityIdentifier GetSID ( WellKnownSidType esid , SecurityIdentifier? domain = null )
                => new(esid , domain);

            internal static SecurityIdentifier GetSID_Everyone ()
                => GetSID(WellKnownSidType.WorldSid);

        }












    }


    /// <summary>Creates mutex like 'AppTitle[_UserSID][_Suffix]'
    /// <example>
    /// <code>
    /// "UOM Network Center_S-1-5-21-3677865666-450531355-3649671759-1002_UOMNetworkCenter.Tools.Apps.WOL.ToolLauncher"
    /// </code>
    /// </example>
    /// </summary>
    internal class AppMutex : AutoDisposable1T<Mutex>
    {
        private const char C_MUTEX_PARTS_SEPARATOR = '_';

        public readonly Mutex Mutex;

        /// <summary>Signal that mutex is created there. If false - mutex was already exist.</summary>
        public readonly bool IsMutexCreated;

        /// <summary>If <see langword="true"/> - Mutex name is diferent for each logged on user. (Session ID is not used)</summary>
        public readonly bool ForCurrentUser;

        public readonly string? Suffix;
        public readonly string MutexName;

        /// <inheritdoc cref="AppMutex" />
        /// <param name="suffix">Any string or null</param>
        /// <param name="currentUser">If <see langword="true"/> - Mutex name will be diferent for each logged on user. (Session ID is not used)</param>
        /// <param name="throwAlreadyExist">If <see langword="true"/> - throws <see cref="AppMutexAlreadyExistException"/> if mutex already exist</param>
        /// <exception cref="AppMutexAlreadyExistException"> Thrown when mutex already exist and throwAlreadyExist = true.</exception>
        public AppMutex ( string? suffix = null , bool currentUser = true , bool throwAlreadyExist = false ) : base()
        {
            (Mutex, IsMutexCreated, MutexName) = CreateAppMutex(currentUser , suffix);
            ForCurrentUser = currentUser;
            Suffix = suffix;

            if ( IsMutexCreated )
            {
                RegisterDisposableObject(Mutex , true);
            }
            else if ( throwAlreadyExist )
            {
                throw new AppMutexAlreadyExistException();
            }
        }

        /// <inheritdoc cref="AppMutex" />

        private static (Mutex Mutex, bool MutexCreated, string MutexID) CreateAppMutex (
            bool currentUser ,
            string? suffix = null )
        {
            string mutexID = CreateAppIDString(currentUser , suffix);
            var mtx = new Mutex(
                true ,
                mutexID ,
                out bool bMutexCreated);

            return (mtx, bMutexCreated, mutexID);
        }

        /// <summary>Create MutexID string with name like 'AppTitle[_UserSID][_Suffix]'
        /// <example>
        /// <code>
        /// "App1_S-1-5-21-3677865666-450531355-3649671759-1002_suffix"
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="ForCurrentUser">Mutex name will be diferent for each logged on user !!! Session ID is not used!!!</param>
        /// <param name="suffix">Mutex ID suffix - any string or null</param>

        internal static string CreateAppIDString ( bool currentUser , string? suffix = null )
        {
            string appID = AppInfo.Title ?? string.Empty;
            if ( appID.isNullOrWhiteSpace )
            {
                throw new Exception("Application.Title = null!");
            }

            if ( currentUser )
            {
                appID += C_MUTEX_PARTS_SEPARATOR + OS.UserAccounts.GetCurrentUserSID().ToString();
            }

            if ( suffix.isNotNullOrWhiteSpace )
            {
                appID += C_MUTEX_PARTS_SEPARATOR + suffix;
            }

            return appID;
        }

        public override string ToString () => MutexName;


        internal partial class AppMutexAlreadyExistException () : Exception(uom.WinAPI.errors.Win32Errors.ERROR_SERVICE_ALREADY_RUNNING.eToWin32Exception().Message) { }

    }


    namespace AutoDisposable
    {



        namespace SafeContainers
        {

            internal abstract partial class MTSafeContainerBase<T> : AutoDisposableUniversal
            {

                #region AttachToUI

                /// <summary>Прицепляет обработку изменения значения, к потоку заданного элемента управления</summary>
                /// <param name="ValueDisplayControl">Элемент управления, в потоке которого будет выполнен обработчик изменения значения</param>
                /// <param name="OnValueChanged">Будет вызываться в потоке ValueDisplayControl</param>
                /// <returns>При CTL.HandleDestroyed отслеживание изменения значения автоматически прекращается</returns>
                public ValueChangedUINotifer AttachToUI ( Control ValueDisplayControl , Action<T?> OnValueChanged )
                    => new(this , ValueDisplayControl , OnValueChanged);

                /// <summary>Прицепляет обработку изменения значения, к отображению его в текстовом поле, с возможностью форматирования</summary>
                /// <param name="ValueDisplayControl">Элемент управления, в потоке которого будет выполнен обработчик изменения значения</param>
                /// <param name="TemplateFormatString">Шаблон строки для отображения</param>
                /// <returns>При CTL.HandleDestroyed отслеживание изменения значения автоматически прекращается</returns>
                public ValueChangedUINotifer AttachToUI ( TextBox ValueDisplayControl , string? TemplateFormatString = null )
                {
                    //var VT = Value.GetType();
                    switch ( Value! )
                    {
                        case string sV:
                        case int iV:
                        case long lV:
                        case short srtV:
                        case StringBuilder sb:
                        case DateTime dt:
                        case Guid g:
                            {
                                Action<T?> CB = new(NewVal =>
                                {
                                    if ( TemplateFormatString.isNotNullOrWhiteSpace )
                                    {
                                        string S = TemplateFormatString!.format(NewVal?.ToString()!);
                                        ValueDisplayControl.Text = S;
                                    }
                                    else
                                    {
                                        ValueDisplayControl.Text = NewVal!.ToString();
                                    }
                                });
                                return AttachToUI(ValueDisplayControl , CB);
                            }

                        default:
                            throw new ArgumentOutOfRangeException($"Элемент управления {ValueDisplayControl.GetType()}, не может отобразить тип значения {Value!.GetType()}");
                    }
                }

                /// <summary>Прицепляет обработку изменения значения, к установке ProgressBar.Value</summary>
                /// <param name="pb">Элемент управления, в потоке которого будет выполнен обработчик изменения значения</param>
                /// <returns>При CTL.HandleDestroyed отслеживание изменения значения автоматически прекращается</returns>
                public ValueChangedUINotifer AttachToUI ( ProgressBar pb )
                    => Value! switch
                    {
                        int iVal => AttachToUI(pb , NewVal => pb.Value = iVal),
                        _ => throw new ArgumentOutOfRangeException($"Элемент управления {pb.GetType()}, не может отобразить тип значения {Value!.GetType()}"),
                    };

                public partial class ValueChangedUINotifer : AutoDisposableUniversal
                {
                    private MTSafeContainerBase<T>? __changedNotifer = null;

                    private MTSafeContainerBase<T>? _ChangedNotifer
                    {
                        [MethodImpl(MethodImplOptions.Synchronized)]
                        get => __changedNotifer;

                        [MethodImpl(MethodImplOptions.Synchronized)]
                        set
                        {
                            if ( __changedNotifer != null )
                            {
                                __changedNotifer.AfterValueChanged -= _VCN_OnAfterValueChanged!;
                            }

                            __changedNotifer = value;
                            if ( __changedNotifer != null )
                            {
                                __changedNotifer.AfterValueChanged += _VCN_OnAfterValueChanged!;
                            }
                        }
                    }

                    public readonly Control control;
                    public readonly Action<T?> OnValueChangedCallBack;//{ get; private set; } = null;

                    protected internal ValueChangedUINotifer ( MTSafeContainerBase<T> MTSC , Control ValueDisplayControl , Action<T?> cbValueChangedCallBack ) : base()
                    {
                        _ChangedNotifer = MTSC;
                        control = ValueDisplayControl;
                        OnValueChangedCallBack = cbValueChangedCallBack;

                        // Отключаем слежение за изменением значения для этого элемента управления, и освобождаем ресурсы
                        control.HandleDestroyed += ( _ , _ ) => Dispose();
                        RegisterDisposeCallback(Destroy);
                    }

                    /// <summary> IDisposable</summary>
                    private void Destroy ()
                    {
                        _ChangedNotifer = null;
                        //control = null;
                        //OnValueChangedCallBack = null;
                    }
                    /// <summary>Обновляем показания в UI</summary>
                    private void _VCN_OnAfterValueChanged ( object sender , ValueChangedEventArgs e )
                    {
                        if ( null == OnValueChangedCallBack )
                        {
                            return;
                        }

                        control?.runInUIThread(() => OnValueChangedCallBack?.Invoke(e.NewValue!));
                    }

                    public MTSafeContainerBase<T> ChangedNotifer { get => ChangedNotifer; }
                }
                #endregion

            }
        }

    }


    namespace controls
    {


        [DefaultProperty("Value")]
        internal class ListItemContainer<T> ( T wrappedValue ) : object()
        {
            public readonly T @Value = wrappedValue ?? throw new ArgumentNullException(nameof(wrappedValue));
            private readonly string _displayName = string.Empty;
            private readonly Func<T , string>? _dynamicDisplayNameProvider = null;

            public ListItemContainer ( T wrappedValue , string displayName ) : this(wrappedValue)
                => _displayName = displayName;

            public ListItemContainer ( T wrappedValue , Func<T , string> dynamicDisplayNameProvider ) : this(wrappedValue)
                => _dynamicDisplayNameProvider = dynamicDisplayNameProvider;

            public string DisplayName
            {
                get
                {
                    return _dynamicDisplayNameProvider != null
                        ? _dynamicDisplayNameProvider.Invoke(Value)
                        : !_displayName.isNullOrEmpty
                        ? _displayName
                        : Value?.ToString() ?? string.Empty;
                }
            }

            public override string ToString () => DisplayName;
        }


        /// <summary>The wrapper class for any objects which need to be displayed in Comboboxes with custom text</summary>
        [DefaultProperty("Value")]
        internal class EnumListItemContainer<T> : ListItemContainer<T> where T : notnull, Enum
        {
            public EnumListItemContainer ( T val , string displayName ) : base(val , displayName) { }

            public EnumListItemContainer ( T val ) : base(val , e => e.GetDescriptionEx() ?? string.Empty) { }
        }



        /// <summary>Typed Writable ListViewItem</summary>
        internal class ListViewItemT<T> ( T? value ) : ListViewItem()
        {
            public T? Value = value;

            /// <summary>Value Not Null</summary>
            public T Value2 = value!;
        }

        /// <summary>Typed Read-Only ListViewItem</summary>
        internal class ListViewItemTRO<T> ( T value ) : ListViewItem()
        {
            public readonly T Value = value;
        }

    }


    namespace UI
    {


        ///<summary>Saves From Position and size. Process Moved/Sized Events</summary>
        [Serializable]
        public class FormPositionInfo ()
        {
            private const string FORMS_SETTTINGS = "Windows";

            /// <summary>Internal storage for avoid GC</summary>
            private static readonly Lazy<Dictionary<Form , FormPositionInfo>> _lll = new(() => []);

            [NonSerialized] private Form? _form;
            [NonSerialized] private bool _canSave = false;

            public DateTime Timestamp = DateTime.Now;
            public FormWindowState State;
            public FormStartPosition StartPosition;
            public System.Drawing.Rectangle RectOnDesktop;
            public System.Drawing.Rectangle RectOnCurrentDisplay;
            public System.Drawing.Rectangle CurrentDisplayBounds;


            //public System.Drawing.Rectangle Restore;
            public string Display = string.Empty;



            public static void Attach ( Form f , [CallerMemberName] string caller = "" )
            {
                if ( caller != ".ctor" )
                {
                    throw new Exception("e_AttachFormPositionSaver() must be called ONLY FROM form Constructor!");
                }

                string id = GetID(f);
                var fpi = Load(f);
                if ( fpi != null )
                {
                    fpi._form = f;

                    //Found previous saved settings - load and apply it to the form
                    f.SuspendLayout();
                    try
                    {
                        fpi.Apply(f);
                    }
                    finally
                    {
                        f.ResumeLayout();
                    }
                }
                fpi ??= new FormPositionInfo(f);
                fpi!.AttachEvents();

                lock ( _lll.Value )
                {
                    var dic = _lll.Value;
                    if ( dic.ContainsKey(f) )
                    {
                        dic[ f ] = fpi!;
                    }
                    else
                    {
                        dic.Add(f , fpi!);
                    }
                }
            }



            private static FormPositionInfo? Load ( Form f )
            {
                string id = GetID(f);
                try
                {
                    var lines = uom.AppTools.AppSettings.Get_stringsAsText(id , string.Empty , FORMS_SETTTINGS);
                    if ( lines != null && lines.isNotNullOrWhiteSpace )
                    {
                        return lines!.eDeSerializeXML<FormPositionInfo>();
                    }
                }
                catch { }

                return null;
            }


            private FormPositionInfo ( Form f ) : this() { _form = f; }



            private void AttachEvents ()
            {
                _form!.ResizeEnd += ( _ , _ ) => Save();
                _form!.LocationChanged += ( _ , _ ) => Save();

                _form!.Shown += ( _ , _ ) => { _canSave = true; };
                _form!.FormClosed += ( _ , _ ) =>
                {
                    _canSave = false;

                    //Removing from temp storage
                    lock ( _lll.Value )
                    {
                        var dic = _lll.Value;
                        if ( dic.ContainsKey(_form) )
                        {
                            dic.Remove(_form);
                        }
                    }
                    _form = null;
                };
            }



            private static string GetID ( Form f ) => f.GetType().FullName!;


            private string GetID () => GetID(_form!);



            private void FromUI ()
            {
                if ( !_canSave || !_form!.IsHandleCreated || _form!.IsDisposed || _form.WindowState == FormWindowState.Minimized || Screen.AllScreens.Length < 1 )
                {
                    return;
                }

                //Debug.WriteLine("");

                Timestamp = DateTime.Now;

                StartPosition = _form.StartPosition;
                State = _form.WindowState;

                RectOnDesktop = _form.GetWindowRectWithoutShadow();
                RectOnCurrentDisplay = RectOnDesktop;
                CurrentDisplayBounds = Screen.PrimaryScreen!.Bounds;
                Display = string.Empty;
                if ( Screen.AllScreens.Length > 1 )
                {
                    Screen scr = Screen.FromHandle(_form.Handle);
                    Display = scr.DeviceName;

                    CurrentDisplayBounds = scr.Bounds;
                    RectOnCurrentDisplay.Offset(-CurrentDisplayBounds.Left , -CurrentDisplayBounds.Top);

                    /*

					Debug.WriteLine($"CurrentDisplay Bounds: {scr.Bounds}");
					Debug.WriteLine($"CurrentDisplay WorkingArea: {scr.WorkingArea}");
					Debug.WriteLine($"RectOnCurrentDisplay: {RectOnCurrentDisplay}");

					//var rr = uom.WinAPI.windows.GetWindowRectWithoutShadow(_f.Handle);
					//Debug.WriteLine($"GetWindowRect: {rr}");

					 */
                }
            }



            private void Apply ( Form f )
            {
                if ( f.IsDisposed || State == FormWindowState.Minimized )
                {
                    return;
                }

                f.StartPosition = FormStartPosition.Manual;

                Screen targetDisplay = Screen
                    .AllScreens
                    .Where(d => d.DeviceName.Equals(Display , StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault() ?? Screen.PrimaryScreen!;

                if ( State != FormWindowState.Maximized )
                {
                    //Checking that bounds is not out of screen
                    {
                        int minX = Screen.AllScreens.Select(d => d.WorkingArea.Left).Min();
                        int minY = Screen.AllScreens.Select(d => d.WorkingArea.Top).Min();
                        int maxX = Screen.AllScreens.Select(d => d.WorkingArea.Right).Max();
                        int maxY = Screen.AllScreens.Select(d => d.WorkingArea.Bottom).Max();

                        if ( RectOnDesktop.X < minX )
                        {
                            RectOnDesktop.X = minX;
                        }

                        if ( RectOnDesktop.Y < minY )
                        {
                            RectOnDesktop.Y = minY;
                        }

                        //Ensuring window Size in not more than Current display Size
                        {
                            if ( RectOnDesktop.Width > targetDisplay.WorkingArea.Width )
                            {
                                RectOnDesktop.Width = targetDisplay.WorkingArea.Width;
                            }

                            if ( RectOnDesktop.Height > targetDisplay.WorkingArea.Height )
                            {
                                RectOnDesktop.Height = targetDisplay.WorkingArea.Height;
                            }
                        }


                        //Slide window left and top if out of right and bonttom bounds
                        if ( RectOnDesktop.Right > maxX )
                        {
                            RectOnDesktop.X = maxX - RectOnDesktop.Width;
                        }

                        if ( RectOnDesktop.Bottom > maxY )
                        {
                            RectOnDesktop.Y = maxY - RectOnDesktop.Height;
                        }
                    }

                    f.SetWindowRectWithoutShadow(RectOnDesktop);
                    var bb = targetDisplay.Bounds;

                    if ( Screen.AllScreens.Length > 1 )
                    {
                        //
                    }
                }
                else
                {
                    //Maximized
                    if ( Screen.AllScreens.Length > 1 && !targetDisplay.Primary )
                    {
                        //need maximize on proper display!
                        var r = _form!.Bounds;
                        r = r.eCenterTo(targetDisplay.WorkingArea.eGetCenter().eToPoint());
                        _form!.Bounds = r;
                    }
                }
                f.WindowState = State;
            }



            private void Save ()
            {
                if ( !_canSave || !_form!.IsHandleCreated || _form!.IsDisposed || _form.WindowState == FormWindowState.Minimized )
                {
                    return;
                }

                FromUI();
                string xml = this.eSerializeAsXML();
                uom.AppTools.AppSettings.SaveMultiString(GetID() , xml , FORMS_SETTTINGS);
                //Debug.WriteLine($"*********** Save\n{xml}");
            }


            /*

	Public Sub Load()
	If (Me._Form?.IsDisposed) Then Return

	Dim sRecordName = Me.GetID
	If(sRecordName.IsNullOrWhiteSpace) Then Return

	Dim aRows = uomvb.Settings.GetSetting_Strings(sRecordName,,,, CS_SETTTINGS_FOLDER).Value
	If(aRows Is Nothing) OrElse(Not aRows.Any) Then Return

	Dim sRows = aRows.join(vbCrLf)
	Dim fps As FormPositionSaver = Nothing
	Try
	fps = sRows.eDeSerializeXML(Of FormPositionSaver)
	Catch ex As Exception 'Any error - ignore
	'Debug.WriteLine("*********** Load ERROR")
	'Debug.WriteLine(ex.Message)
	Return
	End Try
	If(fps Is Nothing) Then Return

	'Debug.WriteLine("*********** Load ")
	'Debug.WriteLine(rFP.ToString)

	With Me._Form
	Call.SuspendLayout()
	Try

	'Ищем монитор, на котором последний раз было окно (могли отключить или удалить - тогда используем основной)
	Dim scrnDisplay = Screen.PrimaryScreen
	If (fps.Display.isNotNullOrWhiteSpace ) Then
		Dim lastDisplay = (From SC In Screen.AllScreens
						   Where(SC.DeviceName.isNotNullOrWhiteSpace AndAlso (SC.DeviceName.Equals(fps.Display, StringComparison.OrdinalIgnoreCase)))).FirstOrDefault()

		If(lastDisplay IsNot Nothing) Then scrnDisplay = lastDisplay 'Найден!
	End If
	Dim rcDisplay = scrnDisplay.Bounds 'Размеры экрана монитора


	Select Case fps.State
		Case FormWindowState.Maximized
			If(.WindowState<> FormWindowState.Normal) Then.WindowState = FormWindowState.Normal
			'разворачиваем на том мониторе, что был последним
			.Location = rcDisplay.Location
			.WindowState = FormWindowState.Maximized

		Case FormWindowState.Minimized
			'.WindowState = FormWindowState.Normal
	'
		Case FormWindowState.Normal
			Dim rcBounds As Rectangle = fps.Bounds

			'Проверяем чтобы окно не выходило за размеры экрана
			'//rcDisplay
			rcBounds = rcBounds.eEnsureInRect(rcDisplay)
			If (rcBounds.Width< 100) Then
				rcBounds.Width = 100
				rcBounds.X = rcBounds.Right - rcBounds.Width
			End If
			If (rcBounds.Height< 100) Then
				rcBounds.Height = 100
				rcBounds.Y = rcBounds.Bottom - rcBounds.Height
			End If

			.Bounds = rcBounds
			If (.WindowState<> FormWindowState.Normal) Then.WindowState = FormWindowState.Normal
			'.Location = rcBounds.Location
			'.Size = rcBounds.Size
	End Select

	Catch ex As Exception
	'
	Finally
	Call.ResumeLayout()
	End Try
	End With
	End Sub


	Public Overrides Function ToString() As string
	Dim sData = Me.eSerializeXML()
	Return sData
	End Function

	*/


        }


        internal class MessageBoxWithCheckbox : AutoDisposable1T<uom.WinAPI.hooks.LocalCbtHook>
        {
            protected uom.WinAPI.hooks.LocalCbtHook _apiHook;
            protected User32.SafeHWND? _hwndDialogWindow;
            protected User32.SafeHWND? _hwndCheckBox;
            protected bool _bInit = false;
            protected bool _dialogCheckBoxValue = false;
            protected string? _checkBoxText;

            public MessageBoxWithCheckbox () : base()
            {
                _apiHook = new();
                _apiHook.WindowCreated += OnWndCreated!;
                _apiHook.WindowDestroyed += OnWndDestroyed!;
                _apiHook.WindowActivated += OnWndActivated!;

                RegisterDisposableObject(_apiHook , false);
            }

            public static void ClearLastUserAnswer ( string dialogID )
            {
                try { uom.AppTools.AppSettings.Delete(dialogID); }
                catch
                {
                    // No processing needed...the convert might throw an exception,
                    // but if so we proceed as if the value was false.
                }
            }

            public const string DEFAULT_CHECKBOX_TEXT = "Don't ask me this again";

            private DialogResult Show (
                string dialogID ,
                string text ,
                string? title = null ,
                string? checkBoxText = DEFAULT_CHECKBOX_TEXT ,
                MessageBoxButtons buttons = MessageBoxButtons.OK ,
                MessageBoxIcon icon = MessageBoxIcon.Information ,
                MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1 )
            {

                if ( string.IsNullOrWhiteSpace(dialogID) ) throw new ArgumentNullException(nameof(dialogID));
                if ( string.IsNullOrWhiteSpace(checkBoxText) ) checkBoxText = DEFAULT_CHECKBOX_TEXT;
                if ( string.IsNullOrWhiteSpace(title) ) title = Application.ProductName;

                try
                {
                    const int VALUE_INVALID = -1;
                    int? regOldCheckBoxValue = uom.AppTools.AppSettings.Get_Int32(dialogID , VALUE_INVALID , "MessageBoxWithCheckbox");

                    if ( regOldCheckBoxValue.HasValue
                        && regOldCheckBoxValue.Value != VALUE_INVALID
                        && System.Enum.IsDefined(typeof(DialogResult) , regOldCheckBoxValue.Value) )
                    {
                        //Registry contains some old value & This is valid DialogResult enum
                        DialogResult drReg = (DialogResult)regOldCheckBoxValue;
                        return drReg;
                    }
                }
                catch
                {
                    // No processing needed...the convert might throw an exception,
                    // but if so we proceed as if the value was false.
                }

                _checkBoxText = checkBoxText;
                _apiHook.Install();
                try
                {
                    DialogResult dr = System.Windows.Forms.MessageBox.Show(text , title , buttons , icon , defbtn);
                    //Save User Answer to registry
                    if ( _dialogCheckBoxValue )
                    {
                        uom.AppTools.AppSettings.Save<int>(dialogID , (int)dr , "MessageBoxWithCheckbox");
                    }

                    return dr;
                }
                finally
                {
                    _apiHook.Uninstall();
                }
            }


            public static DialogResult ShowDialog (
                string dialogID ,
                string text ,
                string? title = null ,
                string? checkBoxText = DEFAULT_CHECKBOX_TEXT ,
                MessageBoxButtons buttons = MessageBoxButtons.OK ,
                MessageBoxIcon icon = MessageBoxIcon.Information ,
                MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1 )
            {
                using MessageBoxWithCheckbox dlg = new();
                return dlg.Show(dialogID , text , title , checkBoxText ?? DEFAULT_CHECKBOX_TEXT , buttons , icon , defbtn);
            }


            private void OnWndCreated ( object sender , uom.WinAPI.hooks.CbtEventArgs e )
            {
                if ( e.IsDialogWindow )
                {
                    _bInit = false;
                    _hwndDialogWindow = new(e.Handle , false);
                }
            }

            private void OnWndDestroyed ( object sender , uom.WinAPI.hooks.CbtEventArgs e )
            {
                if ( e.Handle == _hwndDialogWindow!.DangerousGetHandle() )
                {
                    _bInit = false;
                    _hwndDialogWindow = null;

                    if ( User32.ButtonStateFlags.BST_CHECKED == (User32.ButtonStateFlags)User32.SendMessage(_hwndCheckBox! , User32.ButtonMessage.BM_GETCHECK , IntPtr.Zero , IntPtr.Zero) )
                    {
                        _dialogCheckBoxValue = true;
                    }
                }
            }

            private void OnWndActivated ( object sender , uom.WinAPI.hooks.CbtEventArgs e )
            {
                if ( _hwndDialogWindow == null || _hwndDialogWindow != e.Handle || _bInit ) return;

                _bInit = true;
                // Get the current font, either from the static text window or the message box itself
                var hwndText = User32.GetDlgItem(_hwndDialogWindow , 0xFFFF);

                IntPtr hFont = User32.SendMessage(
                    ((IntPtr)hwndText).isValid ? hwndText : _hwndDialogWindow ,
                    User32.WindowMessage.WM_GETFONT , IntPtr.Zero , IntPtr.Zero);

                Font fCur = Font.FromHfont(hFont);

                // Get the x coordinate for the check box.  Align it with the icon if possible, or one character height in
                Point ptCheckBoxLocation = new();
                var hwndIcon = User32.GetDlgItem(_hwndDialogWindow , 0x0014);
                if ( hwndIcon != IntPtr.Zero )
                {
                    var rcIcon = ((IntPtr)hwndIcon).GetWindowRect();
                    POINT pt = rcIcon.Location;
                    User32.ScreenToClient(_hwndDialogWindow , ref pt);
                    ptCheckBoxLocation.X = pt.X + (rcIcon.Width / 2) - 4;
                }
                else
                {
                    ptCheckBoxLocation.X = (int)fCur.GetHeight();
                }

                // Get the y coordinate for the check box, which is the bottom of the current message box client area
                var rcLicent = _hwndDialogWindow.DangerousGetHandle().GetClientRect();
                int fontHeight = (int)fCur.GetHeight();
                ptCheckBoxLocation.Y = rcLicent.Height + fontHeight;


                // Resize the message box with room for the check box
                var rc = _hwndDialogWindow.DangerousGetHandle().GetWindowRect();
                User32.MoveWindow(_hwndDialogWindow ,
                    rc.Left ,
                    rc.Top ,
                    rc.Width ,
                    rc.Height + (fontHeight * 3) ,
                    true);


                _hwndCheckBox = User32.CreateWindowEx(
                    0 ,
                    "button" ,
                    _checkBoxText! ,
                    (User32.WindowStyles)User32.ButtonStyle.BS_AUTOCHECKBOX |
                    User32.WindowStyles.WS_CHILD |
                    User32.WindowStyles.WS_VISIBLE |
                    User32.WindowStyles.WS_TABSTOP ,

                    ptCheckBoxLocation.X ,
                    ptCheckBoxLocation.Y ,
                    rc.Width - ptCheckBoxLocation.X ,
                    fontHeight ,
                    _hwndDialogWindow ,
                    IntPtr.Zero , IntPtr.Zero , IntPtr.Zero);


                User32.SendMessage(_hwndCheckBox , User32.WindowMessage.WM_SETFONT , hFont , new IntPtr(1));
            }

            #region Win32 Imports

            //private const int WM_SETFONT = 0x00000030;
            //private const int WM_GETFONT = 0x00000031;
            //private const int BM_GETCHECK = 0x00F0;
            //private const int BST_CHECKED = 0x0001;

            #endregion
        }


    }



    namespace Extensions
    {






        internal static class Extensions_Debug_Dump_WinForms
        {



            private static readonly Size _defaultPropertyGridFormSize = new(600 , 800);


            internal static void ePropertyGrid_DisplayInUI ( this object o )
            {
                using Form f = new()
                {
                    StartPosition = FormStartPosition.CenterScreen ,
                    Text = $"{o.GetType()}" ,
                    Size = _defaultPropertyGridFormSize ,
                    closeOnEsc = true
                };

                using PropertyGrid pg = new()
                {
                    Dock = DockStyle.Fill ,
                    SelectedObject = o
                };

                f.Controls.Add(pg);
                f.ShowDialog();
            }


            internal static void ePropertyGrid_DisplayInUI<T> ( this T[] o ) where T : class
            {
                if ( o.Length < 1 )
                {
                    return;
                }

                using Form f = new()
                {
                    StartPosition = FormStartPosition.CenterScreen ,
                    Text = $"{o.First().GetType()} ({o.Length})" ,
                    Size = _defaultPropertyGridFormSize ,
                    closeOnEsc = true
                };

                using PropertyGrid pg = new()
                {
                    Dock = DockStyle.Fill ,
                    SelectedObjects = o
                };

                f.Controls.Add(pg);
                f.ShowDialog();
            }

        }






        internal static class Extensions_WinForms_MessageBox
        {
            [Flags]
            public enum MsgBoxFlags : uint
            {
                Btn_OK = 0,
                Btn_OKCancel = 0b0001,
                Btn_AbortRetryIgnore = 0b0010,
                Btn_YesNoCancel = 0b0011,
                Btn_YesNo = 0b0100,
                Btn_RetryCancel = 0b0101,
#if NET6_0_OR_GREATER
                Btn_CancelTryContinue = 0b0110,
#endif

                Icn_None = 0b0001 << 8,
                Icn_Hand = 0b0010 << 8,
                Icn_Stop = 0b0011 << 8,
                Icn_Error = 0b0100 << 8,
                Icn_Question = 0b0101 << 8,
                Icn_Exclamation = 0b0110 << 8,
                Icn_Warning = 0b0111 << 8,
                Icn_Asterisk = 0b1000 << 8,
                Icn_Information = 0b1001 << 8,

                DefBtn_1 = 0b0001 << 16,
                DefBtn_2 = 0b0010 << 16,
                DefBtn_3 = 0b0011 << 16,
#if NET6_0_OR_GREATER
                DefBtn_4 = 0b0100 << 16,
#endif
            }


            [DebuggerNonUserCode, DebuggerStepThrough]

            private static (
                MessageBoxButtons Buttons,
                MessageBoxIcon Icon,
                MessageBoxDefaultButton DefaultButton
                ) ParseMsgboxFlags ( MsgBoxFlags flags )
            {


                MsgBoxFlags btnFlags = (MsgBoxFlags)((uint)flags & 0x00_00_00_ff);
                MessageBoxButtons btn = btnFlags switch
                {
                    MsgBoxFlags.Btn_OKCancel => MessageBoxButtons.OKCancel,
                    MsgBoxFlags.Btn_AbortRetryIgnore => MessageBoxButtons.AbortRetryIgnore,
                    MsgBoxFlags.Btn_YesNoCancel => MessageBoxButtons.YesNoCancel,
                    MsgBoxFlags.Btn_YesNo => MessageBoxButtons.YesNo,
                    MsgBoxFlags.Btn_RetryCancel => MessageBoxButtons.RetryCancel,
#if NET6_0_OR_GREATER
                    MsgBoxFlags.Btn_CancelTryContinue => MessageBoxButtons.CancelTryContinue,
#endif
                    _ => MessageBoxButtons.OK
                };

                MsgBoxFlags iconFlags = (MsgBoxFlags)((uint)flags & 0x00_00_ff_00);
                MessageBoxIcon icn = iconFlags switch
                {
                    MsgBoxFlags.Icn_Hand => MessageBoxIcon.Hand,
                    MsgBoxFlags.Icn_Stop => MessageBoxIcon.Stop,
                    MsgBoxFlags.Icn_Error => MessageBoxIcon.Error,
                    MsgBoxFlags.Icn_Question => MessageBoxIcon.Question,
                    MsgBoxFlags.Icn_Exclamation => MessageBoxIcon.Exclamation,
                    MsgBoxFlags.Icn_Warning => MessageBoxIcon.Warning,
                    MsgBoxFlags.Icn_Asterisk => MessageBoxIcon.Asterisk,
                    MsgBoxFlags.Icn_Information => MessageBoxIcon.Information,
                    _ => MessageBoxIcon.None
                };



                MsgBoxFlags defBtnFlags = (MsgBoxFlags)((uint)flags & 0x00_ff_00_00);
                MessageBoxDefaultButton dbtn = defBtnFlags switch
                {
                    MsgBoxFlags.DefBtn_2 => MessageBoxDefaultButton.Button2,
                    MsgBoxFlags.DefBtn_3 => MessageBoxDefaultButton.Button3,
#if NET6_0_OR_GREATER
                    MsgBoxFlags.DefBtn_4 => MessageBoxDefaultButton.Button4,
#endif
                    _ => MessageBoxDefaultButton.Button1
                };

                return (btn, icn, dbtn);
            }


            [DebuggerNonUserCode, DebuggerStepThrough]

            public static DialogResult eMsgboxShow (
                this string msg ,
                MsgBoxFlags? flags = MsgBoxFlags.Btn_OK | MsgBoxFlags.Icn_Information | MsgBoxFlags.DefBtn_1 ,
                string? title = null )
            {
                flags ??= (MsgBoxFlags.Btn_OK | MsgBoxFlags.Icn_Information | MsgBoxFlags.DefBtn_1);
                title ??= Application.ProductName;

                var ff = ParseMsgboxFlags(flags.Value);
                DialogResult dr = System.Windows.Forms.MessageBox.Show(msg , title! , ff.Buttons , ff.Icon , ff.DefaultButton);
                return dr;
            }


            [DebuggerNonUserCode, DebuggerStepThrough]

            public static void eMsgboxError ( this Exception ex , string? title = null )
            {
                ex.Message.eMsgboxShow(MsgBoxFlags.Btn_OK | MsgBoxFlags.Icn_Error);
            }


            [DebuggerNonUserCode, DebuggerStepThrough]

            public static DialogResult eMsgboxAsk (
                this string question ,
                bool defButtonYes = true ,
                string? title = null )
            {
                MsgBoxFlags flg = MsgBoxFlags.Btn_YesNo | MsgBoxFlags.Icn_Question
                    | (defButtonYes
                    ? MsgBoxFlags.DefBtn_1
                    : MsgBoxFlags.DefBtn_2);

                return question.eMsgboxShow(flg , title);
            }

            [DebuggerNonUserCode, DebuggerStepThrough]

            public static bool eMsgboxAskIsYes (
                this string question ,
                bool defButtonYes = true ,
                string? title = null )
                => question.eMsgboxAsk(defButtonYes , title) == DialogResult.Yes;


            [DebuggerNonUserCode, DebuggerStepThrough]

            public static DialogResult eMsgboxWithCheckboxAsk (
                this string question ,
                string dialogID ,
                bool defButtonYes = true ,
                string? checkBoxText = uom.UI.MessageBoxWithCheckbox.DEFAULT_CHECKBOX_TEXT ,
                string? title = null )
            {
                MsgBoxFlags flg = MsgBoxFlags.Btn_YesNo | MsgBoxFlags.Icn_Question
                    | (defButtonYes
                    ? MsgBoxFlags.DefBtn_1
                    : MsgBoxFlags.DefBtn_2);

                var ff = ParseMsgboxFlags(flg);

                DialogResult dr = uom.UI.MessageBoxWithCheckbox.ShowDialog(
                    dialogID ,
                    question ,
                    title ,
                    checkBoxText ,
                    ff.Buttons ,
                    ff.Icon ,
                    ff.DefaultButton);

                return dr;
            }


            [DebuggerNonUserCode, DebuggerStepThrough]

            public static void eMsgboxWithCheckboxClearLastUserAnswer ( this string dialogID )
                => uom.UI.MessageBoxWithCheckbox.ClearLastUserAnswer(dialogID);


        }


        internal static partial class Extensions_SystemStruct
        {

            extension( bool source )
            {

                public void toEnabled ( params Component[] cmpnents )
                {
                    cmpnents?.forEach(cmpnent =>
                    {
                        switch ( cmpnent )
                        {
                            case Control ctl: ctl.Enabled = source; break;
                            case ToolStripItem tsi: tsi.Enabled = source; break;
                            default: throw new Exception($"toEnabled failed for '{cmpnent.GetType()}'");
                        }
                    });
                }

                public void toVisible ( params Component[] cmpnents )
                {
                    cmpnents?.forEach(cmpnent =>
                    {
                        switch ( cmpnent )
                        {
                            case Control ctl: ctl.Visible = source; break;
                            case ToolStripItem tsi: tsi.Visible = source; break;
                            default: throw new Exception($"toVisible failed for '{cmpnent.GetType()}'");
                        }
                    });
                }

            }

        }




        internal static partial class Extensions_WinForms_Controls
        {

            private const int DEFAULT_FORM_SHOWN_DELAY = 500;
            internal const int DEFAULT_TEXT_EDIT_DELAY = 1000;
            private const bool APPEND_NEW_LINE_DEFAULT = false;
            internal const string DEFAULT_FILTER_CUEBANNER = "Filter";
            internal const int DEFAULT_TASK_RUN_DELAY = 100;

            internal const string C_DEFAULT_XML_EXT = "xml";
            internal const string C_DEFAULT_XML_EXPORT_FILTER = "XML-files|*." + C_DEFAULT_XML_EXT;

            internal enum SelectionMode : uint
            {
                None, First, Last
            }



            public delegate Task OnFilterChangedDelegate ( string filter );



            [GeneratedRegex(@"^(?<Prefix>.*)\[(?<LinkText>.*)\](?<Suffix>.*)" ,
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace , "ru-RU")]
            private static partial Regex _rxSimpleLink ();

            #region ControlsTimersStorage


            //private  static readonly Lazy<ConcurrentDictionary<System.Guid, System.Windows.Forms.Timer>> _timersStorage = new(  []);
            private static readonly Lazy<ConcurrentDictionary<Control , System.Windows.Forms.Timer>> _controlsTimersStorage0 = new([]);
            private static void ControlsTimersStorageAddRecord ( Control ctl , System.Windows.Forms.Timer tmr )
            {
                _controlsTimersStorage0.Value.addOrUpdate(ctl , tmr);
                ctl.Disposed += ( sender , _ ) =>
                {
                    Control ctl = (Control)sender!;
                    //ControlsTimersStorageDeleteRecord( ctl );
                    //if ( _controlsTimersStorage0.Value.TryGetValue( ctl , out var tmr ) && tmr != null )
                    if ( _controlsTimersStorage0.Value.Remove(ctl , out var tmr) && tmr != null )
                    {
                        //Has stored timer
                        tmr?.Stop();
                        //if ( tmr.IsDisposed ?? true ) tmr?.Dispose();
                    }
                };
            }
            private static void ControlsTimersStorageDeleteRecord ( Control ctl )
                => _controlsTimersStorage0.Value.remove(ctl);

            private static bool ControlsTimersStorageHasRecord ( Control ctl )
                => _controlsTimersStorage0.Value.TryGetValue(ctl , out _);



            #endregion



            public static T eBuildFromFlags<T> ( this T initialValue , params (CheckBox flagCondition, T flagToSet)[] flags ) where T : Enum
            {
                Int64 val = Convert.ToInt64(initialValue);
                foreach ( var item in flags )
                {
                    if ( item.flagCondition.Checked )
                    {
                        val |= Convert.ToInt64(item.flagToSet);
                    }
                }
                T TResult = (T)Enum.ToObject(typeof(T) , val);
                return TResult;
            }




            #region ShortcutKeys to/from string

            /// <summary>Converts Keys value to shortcut keys string like 'Ctrl+I' </summary>

            public static string eToShortcutKeysString ( this System.Windows.Forms.Keys eKey )
                => (string)new KeysConverter().ConvertTo(eKey , typeof(string))!;


            /// <summary>Converts shortcut keys string to Keys value</summary>
            /// <param name="sKeys">Sample 'Ctrl+I'</param>

            public static System.Windows.Forms.Keys eToShortcutKeys ( this string sKeys )
                => (Keys)new KeysConverter().ConvertFrom(sKeys)!;

            #endregion





            #region FORM


            #region runDelayedTask 





            internal static System.Windows.Forms.Timer runDelayedTask ( this Func<Task> delayedAction , int delay = DEFAULT_TASK_RUN_DELAY )
            {
                delayedAction.ThrowIfNull();

                //Use 'System.Windows.Forms.Timer' that uses some thread with caller to raise events
                System.Windows.Forms.Timer tmrDelay = new()
                {
                    Interval = delay ,
                    Enabled = false //do not start timer untill we finish it's setup
                };

                tmrDelay.Tick += async ( s , e ) =>
                {
                    System.Windows.Forms.Timer tmr = (System.Windows.Forms.Timer)s!;
                    //first stop and dispose our timer, to avoid double erunution
                    tmr.Stop();

                    //Now start action incontrols UI thread
                    await delayedAction.Invoke();
                    tmr.Dispose();
                };

                tmrDelay.Start();//Start delay timer

                //We need to avoid dispose timer after exit this proc
                return tmrDelay;
            }


            /// <summary>
            /// Usually used when you need to do an action with a slight delay after exiting the current method. 
            /// For example, if some data will be ready only after exiting the control event handler processing branch
            /// </summary>

            internal static System.Windows.Forms.Timer runDelayed ( this Action delayedAction , int delay = DEFAULT_TASK_RUN_DELAY )
            {
                delayedAction.ThrowIfNull();

                //Use 'System.Windows.Forms.Timer' that uses some thread with caller to raise events
                System.Windows.Forms.Timer tmrDelay = new()
                {
                    Interval = delay ,
                    Enabled = false //do not start timer untill we finish it's setup
                };

                tmrDelay.Tick += ( _ , _ ) =>
                {
                    //first stop and dispose our timer, to avoid double erunution
                    tmrDelay.Stop();
                    tmrDelay.Dispose();

                    //Now start action
                    delayedAction.Invoke();
                };

                //Start delay timer
                tmrDelay.Start();

                //We need to avoid dispose timer after exit this proc
                return tmrDelay;
            }


            internal static System.Windows.Forms.Timer runDelayedOnTimer ( this Task delayedTask , int delay = DEFAULT_TASK_RUN_DELAY )
            {
                delayedTask.ThrowIfNull();

                //Use 'System.Windows.Forms.Timer' that uses some thread with caller to raise events
                System.Windows.Forms.Timer tmrDelay = new()
                {
                    Interval = delay ,
                    Enabled = false //do not start timer untill we finish it's setup
                };

                tmrDelay.Tick += async ( _ , _ ) =>
                {
                    //first stop and dispose our timer, to avoid double erunution
                    tmrDelay.Stop();
                    tmrDelay.Dispose();

                    //Now start action
                    await delayedTask;
                };

                //Start delay timer
                tmrDelay.Start();

                //We need to avoid dispose timer after exit this proc
                return tmrDelay;
            }



            /*
			public static async Task eInvokeAsync(this Control ctl,
			Func<Task> invokeDelegate,
			TimeSpan timeOutSpan = default,
			CancellationToken cancellationToken = default,
			params object[] args)
			{
			var tokenRegistration = default(CancellationTokenRegistration);
			RegisteredWaitHandle? registeredWaitHandle = null;

			try
			{
			TaskCompletionSource<bool> taskCompletionSource = new();
			IAsyncResult? asyncResult = ctl.BeginInvoke(invokeDelegate, args);

			registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(
			 asyncResult.AsyncWaitHandle,
			 new WaitOrTimerCallback(InvokeAsyncCallBack),
			 taskCompletionSource,
			 timeOutSpan.Milliseconds,
			 true);

			tokenRegistration = cancellationToken.Register(
			 CancellationTokenRegistrationCallBack,
			 taskCompletionSource);

			await taskCompletionSource.Task;

			object? returnObject = ctl.EndInvoke(asyncResult);
			return;
			}
			finally
			{
			registeredWaitHandle?.Unregister(null);
			tokenRegistration.Dispose();
			}

			static void CancellationTokenRegistrationCallBack(object? state)
			{
			if (state is TaskCompletionSource<bool> source)
			 source.TrySetCanceled();
			}

			static void InvokeAsyncCallBack(object? state, bool timeOut)
			{
			if (state is TaskCompletionSource<bool> source)
			 source.TrySetResult(timeOut);
			}
			}

			public static async Task<T> eInvokeAsync<T>(this Control ctl,
			Func<Task> invokeDelegate,
			TimeSpan timeOutSpan = default,
			CancellationToken cancellationToken = default,
			params object[] args)
			{
			var tokenRegistration = default(CancellationTokenRegistration);
			RegisteredWaitHandle? registeredWaitHandle = null;

			try
			{
			TaskCompletionSource<bool> taskCompletionSource = new();
			IAsyncResult? asyncResult = ctl.BeginInvoke(invokeDelegate, args);

			registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(
			 asyncResult.AsyncWaitHandle,
			 new WaitOrTimerCallback(InvokeAsyncCallBack),
			 taskCompletionSource,
			 timeOutSpan.Milliseconds,
			 true);

			tokenRegistration = cancellationToken.Register(
			 CancellationTokenRegistrationCallBack,
			 taskCompletionSource);

			await taskCompletionSource.Task;

			object? returnObject = ctl.EndInvoke(asyncResult);
			return (T)returnObject;
			}
			finally
			{
			registeredWaitHandle?.Unregister(null);
			tokenRegistration.Dispose();
			}

			static void CancellationTokenRegistrationCallBack(object? state)
			{
			if (state is TaskCompletionSource<bool> source)
			 source.TrySetCanceled();
			}

			static void InvokeAsyncCallBack(object? state, bool timeOut)
			{
			if (state is TaskCompletionSource<bool> source)
			 source.TrySetResult(timeOut);
			}
			}

			*/


            #endregion


            #region SetIcon_

#if NETFRAMEWORK

			
			private static void eSetIcon_Core<t> (this t Ctl, System.Drawing.Icon rIcon) where t : class
			{
				switch (Ctl)
				{
					case Form frm: frm.Icon = rIcon; break;
					case ToolStripItem tsb: tsb.Image = rIcon.ToBitmap(); break;
					default: throw new NotImplementedException($"Can't set icon for {Ctl.GetType()}");

						//case ToolStripButton tsb: tsb.Image = rIcon.ToBitmap(); break;
						//case ToolStripMenuItem tsmi: tsmi.Image = rIcon.ToBitmap(); break;
				}

			}


			
			//internal static void SetIconsSafe<t>(this IEnumerable<t> eCtl, int IconID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = LRI_ICON_SIZE.Small) where t : class
			//{
			//    foreach (var rCtl in eCtl) rCtl.SetIconSafe(IconID, eLib, eMethod, eIconSize);
			//}

			
			//internal static void SetIconSafe<t>(this t rCtl, int IconID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = LRI_ICON_SIZE.Small) where t : class
			//{
			//    var rImg = LoadResWin32Icon_Safe(IconID, eLib, eMethod, eIconSize);
			//    rCtl.SetIcon_Core(rImg);
			//}

			
			//internal static void SetIconsSafe<t>(this IEnumerable<t> eCtl, LRI_ID IconID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = LRI_ICON_SIZE.Small) where t : class
			//{
			//    eCtl.SetIconsSafe(IconID, eLib, eMethod, eIconSize);
			//}

			
			//internal static void SetIconSafe<t>(this t rCtl, LRI_ID IconID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = LRI_ICON_SIZE.Small) where t : class
			//{
			//    rCtl.SetIconSafe(IconID, eLib, eMethod, eIconSize);
			//}

#endif

            #endregion





            #endregion






            internal static HandleRef createHandleReff ( this IHandle obj )
                => new(obj! , obj!.DangerousGetHandle());


            internal static T? getLParamOf<T> ( this System.Windows.Forms.Message m )
                => (T?)m.GetLParam(typeof(T));


            ///<summary>Wait event with disabled UI</summary>
            internal static async Task waitOnLockedUIAsync ( this WaitHandle evt , Control[] controlsToBlock , int timeout = Timeout.Infinite )
            {
                //Return Await eExecOn_TryFinally(Async Function() Await EVT.waitAsync(rStopFlagToSet, iWaitTimeout),                                       Sub() Call aUI.eEnable(False),                                       Sub() Call aUI.eEnable(True))
                try
                {
                    //Disable UI
                    controlsToBlock.enable(false);

                    await evt.waitAsync(timeout);
                }
                finally
                {
                    //Enable UI
                    controlsToBlock.enable(true);
                }
            }


            #region ItemsAndTags


            public static T? tagAs<T> ( this ListViewGroup lvg ) => (T?)lvg.Tag;
            public static T? tagAs<T> ( this ListViewItem li ) => (T?)li.Tag;
            public static T? tagAs<T> ( this TreeNode nd ) => (T?)nd.Tag;
            public static T? tagAs<T> ( this Control ctl ) => (T?)ctl.Tag;


            /*           

            #region ItemsAndTags2

            public sealed class ListViewItemAndTag<T>
            {
                public ListViewItem Item { get; set; }

                public ListViewItemAndTag ( ListViewItem li ) : base() { Item = li; }

                public T? Tag => Item.eTagAs<T>();
            }



            public static IEnumerable<T?> eTags<T> ( this IEnumerable<ListViewItemAndTag<T>> A )
                => A.Select( li => li.Tag );


            public static IEnumerable<ListViewItem> eItems<T> ( this IEnumerable<ListViewItemAndTag<T>> A )
                => A.Select( li => li.Item );


            public static IEnumerable<ListViewItemAndTag<T>> eItemsAndTags<T> ( this IEnumerable<ListViewItem> A )
                => A.Select( li => new ListViewItemAndTag<T>( li ) );


            public static IEnumerable<ListViewItemAndTag<T>> eItemsAndTags<T> ( this ListViewGroup G )
                => eItemsAndTags<T>( G.itemsAsIEnumerable() );


            public static IEnumerable<ListViewItemAndTag<T>> eItemsAndTags<T> ( this ListView lvw )
                => eItemsAndTags<T>( lvw.itemsAsIEnumerable() );


            public static IEnumerable<ListViewItemAndTag<T>> eSelectedItemsAndTags<T> ( this ListView lvw )
                => eItemsAndTags<T>( lvw.eSelectedItemsAsIEnumerable() );


            public static IEnumerable<ListViewItemAndTag<T>> eCheckedItemsAndTags<T> ( this ListView lvw )
                => eItemsAndTags<T>( lvw.eCheckedItemsAsIEnumerable() );

            #endregion


             */
            #endregion


            extension<T> ( T? source ) where T : Enum
            {


                public controls.EnumListItemContainer<T>[] getAllValuesAsEnumContainers ()
                {
                    T[] enumValues = [ .. Enum.GetValues(typeof(T)).OfType<T>() ];
                    EnumListItemContainer<T>[]? items = [ .. enumValues.Select(v => new EnumListItemContainer<T>(v)) ];
                    return items;
                }



            }


            extension( Control source )
            {



                #region Control.SetProperty


                /// <summary>
                /// Compares the current and new values for a given property. If the value has changed, updates the property with the new value, 
                /// then call <see cref="Control.Invalidate()"/>.
                /// </summary>
                /// <typeparam name="T">The type of the property that changed.</typeparam>
                /// <param name="field">The field storing the property's value.</param>
                /// <param name="newValue">The property's value after the change occurred.</param>
                /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> instance to use to compare the input values.</param>
                /// <param name="propertyName">(optional) The name of the property that changed.</param>
                /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
                /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="comparer"/> is <see langword="null"/>.</exception>
                //
                internal bool setProperty<T> (
                    ref T field ,
                    T newValue ,
                    IEqualityComparer<T>? comparer = null ,
                    Action<T>? valueChangeCallback = null ,
                    bool invalidate = true ,
                    [CallerMemberName] string? propertyName = null )
                {
                    comparer ??= EqualityComparer<T>.Default;
                    if ( comparer!.Equals(field , newValue) ) return false;

                    //OnPropertyChanging(propertyName);
                    if ( null == valueChangeCallback )
                    {
                        field = newValue;
                    }
                    else
                    {
                        valueChangeCallback.Invoke(newValue);
                    }
                    //OnPropertyChanged(propertyName);
                    if ( invalidate ) source?.Invalidate();
                    return true;
                }


                #endregion



                /*
                 public static void runWhenHandleReady<T> ( this T ctl , Action<Control> HandleReadyAction ) where T : Control
                {
                    _ = ctl ?? throw new ArgumentNullException( nameof( ctl ) );

                    if ( ctl.Disposing || ctl.IsDisposed )
                    {
                        return;
                    }

                    if ( ctl.IsHandleCreated )
                    {
                        HandleReadyAction?.Invoke( ctl );//Control handle already Exist, run immediate
                    }
                    else
                    {
                        //Delay action when handle will be ready...
                        ctl.HandleCreated += ( s , e ) => HandleReadyAction?.Invoke( ( T )s! );
                    }
                }                 
                 */

                public void runWhenHandleReady ( Action<Control> HandleReadyAction )
                {
                    if ( (source?.Disposing ?? true) || (source?.IsDisposed ?? true) ) return;

                    if ( source?.IsHandleCreated ?? false )
                    {
                        HandleReadyAction?.Invoke(source);//Control handle already Exist, run immediate
                    }
                    else
                    {
                        //Delay action when handle will be ready...
                        source?.HandleCreated += ( s , e ) => HandleReadyAction?.Invoke((Control)s!);
                    }
                }




                /// <summary>
                /// Usually used when you need to do an action with a slight delay after exiting the current method. 
                /// For example, if some data will be ready only after exiting the control event handler processing branch
                /// </summary>
                internal void runDelayedInUIThread ( Action delayedAction , int delay = DEFAULT_TASK_RUN_DELAY )
                {
                    if ( source == null ) return;

                    var tmr = runDelayed(() =>
                    {
                        ControlsTimersStorageDeleteRecord(source);
                        source.Invoke(delayedAction);
                    } , delay);

                    ControlsTimersStorageAddRecord(source , tmr);
                }


                internal void runDelayedOnTimer ( Task delayedTask , int delay = DEFAULT_TASK_RUN_DELAY )
                {
                    if ( source == null ) return;

                    Task tsk2 = new(async () =>
                    {
                        delayedTask.Start();
                        await delayedTask;
                        ControlsTimersStorageDeleteRecord(source);
                    });

                    var tmr = tsk2.runDelayedOnTimer(delay);
                    ControlsTimersStorageAddRecord(source , tmr);
                }


                /// <summary>MT Safe</summary>
                public void runInUIThread_SetText ( string Text )
                    => source?.runInUIThread(() =>
                    {
                        source?.Text = Text;
                        source?.Update();
                    });



                public void setStyleInternal ( ControlStyles st , bool bset )
                {
                    MethodInfo objMethodInfo = typeof(Control).GetMethod("SetStyle" , BindingFlags.NonPublic | BindingFlags.Instance)!;
                    object[] args = [ st , bset ];
                    objMethodInfo.Invoke(source! , args);
                }


                /// <summary>ThreadSafe excute action in control UI thread
                /// <param name="context">Control in which UI thread action erunutes</param>
                /// <param name="asyncInvoke">If true - used BeginInvoke, else used Invoke</param>
                /// <param name="onError">Any action when Error occurs</param>
                public void runInUIThread ( Action a , bool asyncInvoke = false , Action<Exception>? onError = null )
                {
                    _ = a ?? throw new ArgumentNullException(nameof(a));
                    if ( !(source?.IsHandleCreated ?? false) || (source?.IsDisposed ?? true) ) return;

                    try
                    {
                        if ( asyncInvoke )
                        {
                            source?.BeginInvoke(a);
                        }
                        else if ( source?.InvokeRequired ?? false )
                        {
                            source?.Invoke(a);
                        }
                        else
                        {
                            a.Invoke();
                        }
                    }
                    catch ( Exception EX ) { onError?.Invoke(EX); }
                }


                /// <summary>ThreadSafe excute action in control UI thread
                /// <param name="context">Control in which UI thread action erunutes</param>
                /// <param name="useBeginInvoke">If true - used BeginInvoke, else used Invoke</param>
                /// <param name="onError">Any action when Error occurs</param>
                public T? runInUIThread<T> ( Func<T> a , Action<Exception>? onError = null )
                {
                    _ = a ?? throw new ArgumentNullException(nameof(a));
                    if ( !(source?.IsHandleCreated ?? false) || (source?.IsDisposed ?? true) ) return default;

                    try
                    {
                        return (source?.InvokeRequired ?? false)
                            ? (source!.Invoke(a) ?? default)!
                            : a.Invoke();
                    }
                    catch ( Exception EX )
                    {
                        onError?.Invoke(EX);
                        return default;
                    }
                }

                /// <summary>ThreadSafe excute action in control UI thread
                /// <param name="context">Control in which UI thread action erunutes</param>
                /// <param name="onError">Any action when Error occurs</param>
                public async Task runInUIThreadAsync ( Func<Task> a , Action<Exception>? onError = null )
                {
                    _ = a ?? throw new ArgumentNullException(nameof(a));

                    if ( !(source?.IsHandleCreated ?? false) || (source?.IsDisposed ?? true) ) return;

                    try
                    {
                        if ( source?.InvokeRequired ?? false )
                        {
#if NET
                            await source!.Invoke(a);
#else
						await Task.Delay(1);
						context.Invoke(a);
#endif
                        }
                        else
                        {
                            await a.Invoke();
                        }
                    }
                    catch ( Exception EX ) { onError?.Invoke(EX); }
                }


                public async Task runOnDisabledAsync (
                    Func<Task> a ,
                    Form? waitCursorForm = null )
                    => await source!.toArrayFromSingleElement().runOnDisabledAsync(a , waitCursorForm);


                public void runOnDisabled ( Action a )
                {
                    bool oldEnabledStatus = source?.Enabled ?? false;
                    source?.Enabled = false;
                    try
                    {
                        a?.Invoke();
                    }
                    finally
                    {
                        if ( oldEnabledStatus ) source?.Enabled = true;
                    }
                }


                /*

                public static T[] GetChildrensOfType<T> ( this Control ctl, bool recurse = false ) where T : Control
                    => ctl.GetChildrens (recurse)
                    .Where (c => c.GetType () == typeof (T))
                    .OfType<T> ()
                    .ToArray ();
                 



                public static T[] eChildControlsOf<T> ( this Control ctl ) where T : Control
                    => ctl.Controls.GetChildrensOf<T> ();

                 */


                public Control[] getChildrens ( bool recurse )
                {
                    if ( !source?.HasChildren ?? false ) return [];

                    List<Control> ctlList = [];
                    var childrens = source?.Controls.toArray() ?? [];
                    ctlList.AddRange(childrens);

                    if ( recurse )
                    {
                        foreach ( var ctlChild in childrens )
                        {
                            var cc = ctlChild.getChildrens(true);
                            if ( cc.Length != 0 ) ctlList.AddRange(cc);
                        }
                    }

                    return [ .. ctlList ];
                }



                public T[] getChildrensOf<T> ( bool recurse = false ) where T : Control
                    => [ ..
                    source?.getChildrens (recurse)
                    .OfType<T> ()
                        ??[]
                        ];



                internal HandleRef createHandleReff ()
                    => new(source! , source!.Handle);


            }



            extension( Control[] source )
            {



                public void enable ( bool e )
                    => source?.ToList()?.ForEach(ctl => ctl.Enabled = e);


                public void runOnDisabled ( Action a )
                {
                    _ = a ?? throw new ArgumentNullException(nameof(a));

                    var L = source?
                        .Select(ctl => new { Ctl = ctl , EnabledStatus = ctl.Enabled })
                        .ToList();

                    L?.ForEach(ctl => ctl.Ctl.Enabled = false);
                    try
                    {
                        a.Invoke();
                    }
                    finally
                    {
                        L?.ForEach(ctl => ctl.Ctl.Enabled = ctl.EnabledStatus);
                    }
                }


                public async Task runOnDisabledAsync ( Func<Task> a , Form? waitCursorForm = null )
                {
                    waitCursorForm?.UseWaitCursor = true;

                    //Remember enabled status
                    var L = source?
                        .Select(ctl => new { Ctl = ctl , OldEnabledStatus = ctl.Enabled })
                        .ToList();

                    try
                    {
                        //Change enabled status
                        L?.ForEach(cs =>
                        {
                            cs.Ctl.Enabled = false;
                            cs.Ctl.Update();
                        });

                        await a!.Invoke();
                    }
                    finally
                    {
                        //Restoring old known enabled status
                        L?.ForEach(cs =>
                        {
                            if ( cs.OldEnabledStatus ) cs.Ctl.Enabled = true;
                        });

                        waitCursorForm?.UseWaitCursor = false;
                    }

                }



            }



            extension( Control.ControlCollection source )
            {


                public Control[] toArray () => [ .. source?.OfType<Control>() ?? [] ];



                /*

                public static Control[] GetChildrens ( this Control.ControlCollection cc, bool recurse = false )
                {
                    List<Control> ctlList = [];
                    var childrens = cc.AsArray ();
                    ctlList.AddRange (childrens);

                    if (recurse)
                    {
                        foreach (var ctlChild in childrens)
                        {
                            var cc2 = ctlChild.GetChildrens (true);
                            if (cc2.Any ()) ctlList.AddRange (cc2);
                        }
                    }

                    return [ .. ctlList ];
                }
    */


            }





            #region IIf









            extension( Form source )
            {


                public bool showDialog_OK ( Form? ParentForm = null )
                    => ((null == ParentForm)
                    ? source?.ShowDialog()
                    : source?.ShowDialog(ParentForm)) == System.Windows.Forms.DialogResult.OK;


                public bool showDialog_NotOK ( Form? ParentForm = null ) =>
                    !source.showDialog_OK(ParentForm);



                public void setTrayBackgroundWindowAttributes ()
                {
                    source?.Text = string.Empty;
                    source?.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    source?.Visible = false;
                    source?.WindowState = FormWindowState.Minimized;
                    source?.ControlBox = false;
                    source?.Opacity = 0f;
                    source?.ShowIcon = false;
                    source?.ShowInTaskbar = false;
                }


                /// <summary>Executes after form shown on screen, with specifed delay.
                /// Delay starts after 'Form.Shown' ebent</summary>
                internal void runOnClosed ( Action action , bool ignoreExceptions = false )
                {
                    source?.FormClosed += ( ctx , ea ) =>
                    {
                        try { action.Invoke(); }
                        catch
                        {
                            if ( !ignoreExceptions ) throw;
                        }
                    };
                }



                /// <summary>Executes 'delayedAction' after form shown on screen, with specifed delay in UI Thread.
                /// Delay starts after 'Form.Shown' event</summary>
                internal void runDelayed_OnShown (
                    Action delayedAction ,
                    int delay = DEFAULT_FORM_SHOWN_DELAY ,
                    bool uswaitCursor = true ,
                    bool onErrorShowUI = true ,
                    Action<Exception>? onError = null ,
                    bool onErrorCloseForm = true
                    )
                {
                    async Task t ()
                    {
                        await Task.Delay(1);
                        delayedAction.Invoke();
                    }

                    source?.runDelayed_OnShown(t , delay , uswaitCursor , onErrorShowUI , onError , onErrorCloseForm);
                }


                /// <inheritdoc cref="runDelayed_OnShown" />
                internal void runDelayed_OnShown (
                    IEnumerable<Func<Task>> delayedTasks ,
                    int delay = DEFAULT_FORM_SHOWN_DELAY ,
                    bool uswaitCursor = true ,
                    bool onErrorShowUI = true ,
                    Action<Exception>? onError = null ,
                    bool onErrorCloseForm = true )
                {
                    async Task onShown ()
                    {
                        await Task.Delay(delay);

                        if ( uswaitCursor && !(source?.UseWaitCursor ?? false) )
                            source?.UseWaitCursor = true;

                        try
                        {
                            foreach ( var tsk in delayedTasks )
                            {
                                await tsk.Invoke();
                            }
                        }
                        catch ( OperationCanceledException ) { }                 //catch (TaskCanceledException tcex) { }
                        catch ( Exception ex )
                        {
                            ex.eLogError(onErrorShowUI);
                            onError?.Invoke(ex);
                            if ( onErrorCloseForm )
                            {
                                source?.Close();
                            }
                        }
                        finally
                        {
                            if ( uswaitCursor && (source?.UseWaitCursor ?? false) )
                                source.UseWaitCursor = false;
                        }

                    }
                    source?.Shown += async ( _ , _ ) => await onShown();
                }


                /// <inheritdoc cref="runDelayed_OnShown" />
                internal void runDelayed_OnShown (
                    Func<Task> delayedTask ,
                    int delay = DEFAULT_FORM_SHOWN_DELAY ,
                    bool uswaitCursor = true ,
                    bool onErrorShowUI = true ,
                    Action<Exception>? onError = null ,
                    bool onErrorCloseForm = true )
                        => source?.runDelayed_OnShown([ delayedTask ] , delay , uswaitCursor , onErrorShowUI , onError , onErrorCloseForm);


                internal void runDelayed_OnShown_SetFocus ( int delay = DEFAULT_FORM_SHOWN_DELAY )
                    => source?.runDelayed_OnShown(delegate
                    {
                        source?.Focus();
                        source?.Activate();
                        source?.BringToFront();
                    } , delay);


                public bool tryCatch_OnWaitCursor ( Action a , bool showError = true , string errorTitle = "Error" )
                {
                    try
                    {
                        source?.Cursor = Cursors.WaitCursor;
                        try { a.Invoke(); }
                        finally { source?.Cursor = Cursors.Default; }

                        return true;
                    }
                    catch ( Exception ex )
                    {
                        if ( showError )
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message.ToString() , errorTitle , MessageBoxButtons.OK , MessageBoxIcon.Error);
                        }

                        return false;
                    }
                }


                public async Task<bool> tryCatch_OnWaitCursorAsync ( Task tsk , bool showError = true , string errorTitle = "Error" )
                {
                    try
                    {
                        source?.Cursor = Cursors.WaitCursor;
                        try
                        {
                            await tsk;
                        }
                        finally { source?.Cursor = Cursors.Default; }

                        return true;
                    }
                    catch ( Exception ex )
                    {
                        if ( showError )
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message.ToString() , errorTitle , MessageBoxButtons.OK , MessageBoxIcon.Error);
                        }

                        return false;
                    }
                }





                /// <summary>
                /// Enables close this form when ESC key pressed
                /// </summary>
                public void enableCloseOnEsc ()
                {
                    source?.KeyPreview = true;
                    source?.KeyDown += ( s , e ) =>
                    {
                        Form f2 = (Form)s!;
                        if ( e.KeyCode == Keys.Escape )
                        {
                            f2.DialogResult = DialogResult.Cancel;
                        }
                    };

                }

                /// <summary>
                /// Enables close this form when ESC key pressed
                /// </summary>
                [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
                public bool closeOnEsc
                {
                    set
                    {
                        if ( !value ) throw new Exception("Property 'closeOnEsc' is Write-only!");
                        source?.enableCloseOnEsc();
                    }

                }


                public void attach_PositionAndStateSaver ( [CallerMemberName] string caller = "" )
                    => UI.FormPositionInfo.Attach(source! , caller);


                #region closeOnClick


                public void closeOnClick ( System.Windows.Forms.ToolStripMenuItem MI )
                    => MI.Click += ( _ , _ ) => source?.closeSafe();


                public void closeOnClick ( Button Ctl )
                    => Ctl.Click += ( _ , _ ) => source?.closeSafe();


                private void closeSafe ()
                    => source?.runInUIThread(source.Close);


                #endregion



                public string[] openLoadFilesDialog (
                    string defaultExt = "" ,
                    string filter = C_DEFAULT_XML_EXPORT_FILTER ,
                    bool multiselect = true ,
                    string initialFile = "" ,
                    string initialDirectory = "" )
                {
                    using OpenFileDialog ofd = new();
                    ofd.initDefault(defaultExt , filter , multiselect);
                    ofd.Filter = filter;

                    if ( initialFile.isNotNullOrWhiteSpace ) ofd.FileName = initialFile;
                    if ( initialDirectory.isNotNullOrWhiteSpace ) ofd.InitialDirectory = initialDirectory;

                    return (ofd.ShowDialog(source!) != DialogResult.OK)
                        ? []
                        : ofd.FileNames;
                }

                public string openLoadFileDialog (
                    string defaultExt = "" ,
                    string filter = C_DEFAULT_XML_EXPORT_FILTER ,
                    string initialFile = "" ,
                    string initialDirectory = "" )
                    => openLoadFilesDialog(source ,
                        defaultExt ,
                        filter ,
                        false ,
                        initialFile ,
                        initialDirectory).FirstOrDefault() ?? string.Empty;

                public string openLoadFileDialog (
                    string defaultExt = "" ,
                    string filter = C_DEFAULT_XML_EXPORT_FILTER ,
                    string initialFile = "" ,
                    Environment.SpecialFolder initialDirectory = Environment.SpecialFolder.DesktopDirectory )
                    => openLoadFileDialog(source ,
                        defaultExt ,
                        filter ,
                        initialFile ,
                        System.Environment.GetFolderPath(initialDirectory));


            }


            extension( TextBox source )
            {


                /// <summary>MT Safe</summary>

                public void runInUIThread_AppendText ( string Text )
                    => source?.runInUIThread(() =>
                    {
                        source?.AppendText(Text);
                        source?.Update();
                    });


#if NET6_0_OR_GREATER

                /// <summary>MT Safe</summary>
                public void runInUIThread_AppendLine ( string Text , int limitLinesCount = 0 )
                    => source?.runInUIThread(() =>
                    {
                        if ( limitLinesCount > 0 )
                        {
                            source?.Lines = [ .. source?.Lines?.TakeLast(limitLinesCount) ?? [] ];
                        }
                        source?.AppendText($"{Text}{Environment.NewLine}");
                        source?.Update();
                    }
                    );
#endif



                [Obsolete("Use Vanara SetCueBanner instead!" , true)]
                public void setVistaCueBanner ( string? banner = null )
                    => source?
                        .runWhenHandleReady(tb =>
                        User32.SendMessage<User32.EditMessage>(tb.Handle , User32.EditMessage.EM_SETCUEBANNER , 0 , banner.valueOrEmpty));


                #region AttachDelayedFilter

                /// <summary>
                /// Attaches a deferred text change event handler that makes it possible to react to text changes with some delay, 
                /// allowing the user to correct erroneous input, 
                /// or complete input, rather than reacting immediately to each letter.
                /// </summary>
                /// <param name="onTextChanged">TextChanged Handler</param>
                /// <param name="delay">Delay (ms.) during which repeated input will not call the handler</param>
                /// <param name="cueBanner">Vista cueabanner text</param>
                /// <param name="changeBackColor">Sets the background color for textbox to Systemcolors.Info</param>
                public void attach_DelayedFilter (
                    Action<string> onTextChanged ,
                    int delay = DEFAULT_TEXT_EDIT_DELAY ,
                    string? cueBanner = DEFAULT_FILTER_CUEBANNER ,
                    bool changeBackColor = true )
                {
                    if ( source == null ) return;


                    if ( ControlsTimersStorageHasRecord(source) )
                        throw new ArgumentException("This TextBox already has attached DelayedFilter!" , nameof(source));

                    //Create new delay timer
                    var tmr = new System.Windows.Forms.Timer() { Interval = delay };
                    //Сохраняем ссылку на таймер хоть где-то, чтобы GC его не грохнул.
                    ControlsTimersStorageAddRecord(source! , tmr);

                    source?.SetCueBanner(cueBanner);
                    if ( changeBackColor ) source?.BackColor = SystemColors.Info;


                    tmr.Tick += delegate
                    {
                        tmr.Stop(); //Останавливаем таймер
                        onTextChanged.Invoke(source?.Text?.valueOrEmpty ?? string.Empty);
                    };

                    source?.TextChanged += delegate
                    {
                        //Restart timer...
                        tmr.Stop();
                        tmr.Start();
                    };
                }


                public void attach_DelayedFilterTask (
                  Task onTextChanged ,
                  int delay = DEFAULT_TEXT_EDIT_DELAY ,
                  string? cueBanner = DEFAULT_FILTER_CUEBANNER ,
                  bool changeBackColor = true )
                {
                    if ( source == null ) return;


                    if ( ControlsTimersStorageHasRecord(source) )
                        throw new ArgumentException("This TextBox already has attached DelayedFilter!" , nameof(source));

                    //Create new delay timer
                    var tmr = new System.Windows.Forms.Timer() { Interval = delay };
                    //Сохраняем ссылку на таймер хоть где-то, чтобы GC его не грохнул.
                    ControlsTimersStorageAddRecord(source! , tmr);

                    source?.SetCueBanner(cueBanner);
                    if ( changeBackColor ) source?.BackColor = SystemColors.Info;


                    tmr.Tick += async ( _ , _ ) =>
                    {
                        tmr.Stop(); //Останавливаем таймер
                        await onTextChanged;
                    };

                    source?.TextChanged += delegate
                    {
                        //Restart timer...
                        tmr.Stop();
                        tmr.Start();
                    };
                }

                public void attach_DelayedFilterTask (
                   OnFilterChangedDelegate onTextChanged ,
                   int delay = DEFAULT_TEXT_EDIT_DELAY ,
                   string? cueBanner = DEFAULT_FILTER_CUEBANNER ,
                   bool changeBackColor = true )
                {
                    if ( source == null ) return;


                    if ( ControlsTimersStorageHasRecord(source) )
                        throw new ArgumentException("This TextBox already has attached DelayedFilter!" , nameof(source));

                    //Create new delay timer
                    var tmr = new System.Windows.Forms.Timer() { Interval = delay };
                    //Сохраняем ссылку на таймер хоть где-то, чтобы GC его не грохнул.
                    ControlsTimersStorageAddRecord(source! , tmr);

                    source?.SetCueBanner(cueBanner);
                    if ( changeBackColor ) source?.BackColor = SystemColors.Info;


                    tmr.Tick += async ( _ , _ ) =>
                    {
                        tmr.Stop(); //Останавливаем таймер
                        await onTextChanged.Invoke(source?.Text?.valueOrEmpty ?? string.Empty);
                    };

                    source?.TextChanged += delegate
                    {
                        //Restart timer...
                        tmr.Stop();
                        tmr.Start();
                    };
                }



                /*
                /// <inheritdoc cref="AttachDelayedFilter" />
                public void attach_DelayedFilter (
                    System.Windows.Forms.MethodInvoker onTextChanged ,
                    int delay = DEFAULT_TEXT_EDIT_DELAY ,
                    string cueBanner = DEFAULT_FILTER_CUEBANNER ,
                    bool changeBackColor = true )
                        => source?.attach_DelayedFilter(
                            new Action<string>(( _ ) => onTextChanged.Invoke()) ,
                            delay , cueBanner , changeBackColor);
                 */



                #endregion




                public void setAutoCompleteSource ( string[] list , AutoCompleteMode mode = AutoCompleteMode.SuggestAppend )
                {
                    source?.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    source?.AutoCompleteMode = mode;
                    AutoCompleteStringCollection cc = [];
                    cc.AddRange(list);
                    source?.AutoCompleteCustomSource = cc;
                }




            }





            extension( RichTextBox source )
            {


                public void appendTextWithActionOnSelection ( string text , Action a , bool appendNewLine = APPEND_NEW_LINE_DEFAULT )
                {
                    if ( source == null ) return;

                    int selStart = source.Text.Length;
                    source.AppendText(text);
                    int selLen = source.Text.Length - selStart;
                    source.Select(selStart , selLen);
                    a.Invoke();
                    source.Select(source.TextLength , 0);
                    if ( appendNewLine ) source.AppendText("\n");
                }



                public void appendTextFormatted (
                    string text ,
                    Color? clrFore = null ,
                    Color? clrBack = null ,
                    Font? fnt = null ,
                    bool appendNewLine = APPEND_NEW_LINE_DEFAULT )
                {
                    source?.appendTextWithActionOnSelection(text ,
                         delegate
                         {
                             if ( clrFore != null ) source?.SelectionColor = clrFore.Value;
                             if ( clrBack != null ) source?.SelectionBackColor = clrBack.Value;
                             if ( fnt != null ) source?.SelectionFont = fnt;
                         } ,
                         appendNewLine);
                }
            }



            extension( LinkLabel source )
            {
                //<a href="mailto:mail@htmlacademy.ru">Напишите нам</a>


                public void clearAllLinks ()
                    => source?.LinkArea = new LinkArea(0 , 0);


                /// <summary>Sets Text and clears all links</summary>
                public void setTextNoLink ( string text )
                {
                    source?.Text = text;
                    source?.clearAllLinks();
                }

                /// <summary>Append Text. Links does not changes</summary>

                public void appendText ( string text )
                    => source?.Text += text;


                public void appendTextWithLinkSimple ( string text , Action<LinkLabel.Link>? clickHandler = null , bool clearOldLinks = false )
                {
                    //"Текущий MAC-адрес: ['{qr.MAC.eToStringHex()}']"


                    if ( source == null ) return;

                    string oldText = clearOldLinks
                        ? string.Empty
                        : source.Text;

                    int initLen = oldText.Length;
                    if ( clearOldLinks ) source.clearAllLinks();

                    MatchCollection result = _rxSimpleLink().Matches(text);
                    if ( result.Count < 1 ) throw new ArgumentException($"Not found any Links in the string '{text}'" , nameof(text));

                    if ( result.Count > 1 ) throw new ArgumentException($"Found more than one Link in the string '{text}'" , nameof(text));


                    Match m = result.OfType<Match>().First();
                    string prefix = m.Groups[ "Prefix" ].Value;
                    string suffix = m.Groups[ "Suffix" ].Value;
                    Group link = m.Groups[ "LinkText" ];
                    string linkText = link.Value;

                    //string ss = text.Substring(link.Index, link.Length);

                    Guid linkID = Guid.NewGuid();
                    int startPos = prefix.Length;
                    int LinkLen = linkText.Length;
                    LinkLabel.Link lll = new(initLen + startPos , LinkLen , linkID);
                    text = $"{prefix}{linkText}{suffix}";

                    source.Text = oldText + text;
                    source.Links.Add(lll);

                    if ( clickHandler == null ) return;

                    source.LinkClicked += ( _ , e ) =>
                    {
                        LinkLabel.Link? lllClicked = e.Link;
                        if ( lllClicked == null || lllClicked.LinkData is not Guid || !linkID.Equals((Guid)lllClicked.LinkData) )
                            return;

                        //This is our Link Clicked!
                        clickHandler?.Invoke(lllClicked);
                    };

                }

            }


            extension( ToolStripStatusLabel source )
            {


                /// <summary>Sets Text and link</summary>
                public void setTextAsLink ( string linkText , Action clickAction )
                {
                    source?.Text = linkText;
                    source?.IsLink = true;
                    source?.Click += ( _ , _ ) => clickAction.Invoke();
                }

            }





            extension( ListBox.ObjectCollection source )
            {
                public int countOrZerro => source?.Count ?? 0;
            }
            extension( CheckedListBox.ObjectCollection source )
            {
                public int countOrZerro => source?.Count ?? 0;
            }
            extension( ComboBox.ObjectCollection source )
            {
                public int countOrZerro => source?.Count ?? 0;
            }



            extension( ListBox source )
            {


                public IEnumerable<T> itemsAsEnumerableOfType<T> ()
                   => source?.Items.OfType<T>() ?? [];

                public T[] itemsOfType<T> ()
                    => [ .. source?.itemsAsEnumerableOfType<T>() ?? [] ];

                public ListItemContainer<T>[] itemsAs_ContainerOf<T> ()
                    => [ .. source?.itemsOfType<ListItemContainer<T>>() ?? [] ];

                public ListItemContainer<T>? selectedItemAs_ContainerOf<T> ()
                    => source?.SelectedItem as ListItemContainer<T>;

                public T? selectedItemAs_ContainerValueOf<T> ()
                {
                    //=> selectedItemAs_ContainerOf<T>(source)?.Value ?? default;
                    var cnt = selectedItemAs_ContainerOf<T>(source);
                    return cnt == null
                        ? default
                        : cnt.Value ?? default;

                }



                /// <summary>/Limit count of lines to value</summary>
                public void limitMaxRowsCount ( int maxRows )
                {
                    if ( source == null ) return;

                    source.runOnLockedUpdateInUIThread(delegate
                    {
                        while ( source.Items.Count >= maxRows )
                        {
                            source.Items.RemoveAt(0);
                        }
                    });
                }


                public int lastItemIndex
                    => (source?.Items?.Count ?? 0) - 1;


                public Object? lastItem
                    => (source?.Items?.any ?? false)
                    ? source!.Items[ source!.lastItemIndex ]
                    : null;



                public Object? selectLastRow ()
                {
                    if ( source == null ) return null;

                    source.SelectedIndex = source.lastItemIndex;
                    return source.SelectedItem;
                }


                public void setLastItem ( object newValue )
                {
                    if ( source == null || !source.Items.any ) return;
                    source.Items[ source.lastItemIndex ] = newValue;
                }


                /// <returns>Returns number of removed items</returns>
                public int removeSelectedItems ( bool selectFirstPreviousItem = true )
                {
                    if ( source == null || !source.Items.any ) return 0;

                    var itemIndexesToRemove = source.SelectedIndices.OfType<int>().Reverse().ToArray(); //Removing items from end to start to avoid change items indexes during remove process
                    foreach ( var itemIndex in itemIndexesToRemove )
                    {
                        source.Items.RemoveAt(itemIndex);
                    }
                    if ( selectFirstPreviousItem && source.Items.any )
                    {
                        var firstDeletedIndex = (itemIndexesToRemove[ 0 ] - 1).checkRange(0 , source.Items.Count);
                        source.SelectedIndex = firstDeletedIndex;
                    }
                    return itemIndexesToRemove.Length;
                }


            }


            extension( ComboBox source )
            {

                #region Get Items From CBO

                public IEnumerable<T> itemsAsEnumerableOfType<T> ()
                    => source?.Items?.OfType<T>() ?? [];

                public T[] itemsOfType<T> ()
                    => [ .. source.itemsAsEnumerableOfType<T>() ];

                public ListItemContainer<T>[] itemsAs_ContainerOf<T> ()
                    => [ .. source?.itemsOfType<ListItemContainer<T>>() ?? [] ];

                public ListItemContainer<T>? selectedItemAs_ContainerOf<T> ()
                    => source?.SelectedItem as ListItemContainer<T>;

                public T? selectedItemAs_ContainerValueOf<T> ()
                {
                    //=> selectedItemAs_ContainerOf<T>(source)?.Value ?? default;
                    var cnt = selectedItemAs_ContainerOf<T>(source);
                    return cnt == null
                        ? default
                        : cnt.Value ?? default;
                }


                #endregion


                public void disabeAndShowBanner ( string banner )
                {
                    source?.Enabled = false;
                    source?.Items.Clear();
                    source?.Items.Add(banner);
                    source?.SelectedIndex = 0;
                }


                public void disabeAndShowBanner ( Exception ex )
                    => source?.disabeAndShowBanner(ex.Message);


                /// <returns>Returns index of the removed item or -1</returns>
                public int removeSelectedItem ()
                {
                    if ( source == null ) return -1;

                    var itemIndexeToRemove = source.SelectedIndex;
                    if ( itemIndexeToRemove < 0 ) return -1;
                    source.Items.RemoveAt(itemIndexeToRemove);
                    return itemIndexeToRemove;
                }


            }




            extension<TListCtl> ( TListCtl source ) where TListCtl : ListControl
            {



                public void clearItems ( bool resetDataSource = false )
                {
                    if ( source is ComboBox cbo )
                    {
                        if ( resetDataSource ) cbo.DataSource = null;
                        cbo.Items.Clear();
                    }
                    else if ( source is ListBox lst )
                    {
                        if ( resetDataSource ) lst.DataSource = null;
                        lst.Items.Clear();
                    }
                    else throw new ArgumentOutOfRangeException(nameof(source));
                }


                ///<summary>MT Safe!!!</summary>
                public void runOnLockedUpdateInUIThread ( Action action )
                {
                    if ( source is not ComboBox && source is not ListBox ) throw new ArgumentOutOfRangeException(nameof(source));

                    void a ()
                    {
                        if ( source is ComboBox cbo )
                            cbo.BeginUpdate();
                        else if ( source is ListBox lst )
                            lst.BeginUpdate();

                        try
                        {
                            action!.Invoke();
                        }
                        finally
                        {
                            if ( source is ComboBox cbo2 )
                                cbo2.EndUpdate();
                            else if ( source is ListBox lst2 )
                                lst2.EndUpdate();
                        }
                    }

                    if ( source != null && source.InvokeRequired )
                    {
                        source.runInUIThread(a);
                    }
                    else
                    {
                        a();
                    }
                }


                public void fill<T> ( IEnumerable<T> items , T? itemToSelect )
                {
                    var arr = items.ToArray();

                    void a ()
                    {
                        source.clearItems();
                        if ( arr.Length == 0 ) return;

                        if ( source is ComboBox cbo2 )
                            foreach ( var item in arr )
                                cbo2.Items.Add(item!);
                        else if ( source is ListBox lst2 )
                            foreach ( var item in arr )
                                lst2.Items.Add(item!);

                        if ( itemToSelect != null )
                        {
                            if ( source is ComboBox cbo3 )
                                cbo3.SelectedItem = itemToSelect;
                            else if ( source is ListBox lst3 )
                                lst3.SelectedItem = itemToSelect;
                        }
                    }

                    source?.runOnLockedUpdateInUIThread(a);
                }



                public void fill<T> ( IEnumerable<T> items , SelectionMode selectionMode )
                {

                    T? sel = selectionMode switch
                    {
                        SelectionMode.First => items.First(),
                        SelectionMode.Last => items.Last(),
                        _ => default
                    };
                    source?.fill<TListCtl , T>(items , sel);
                }



                ///<summary>Fills ComboBox with <see cref="ListItemContainer"/> wrappers and returns selected index</summary>
                ///<remarks>Sample: cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING)</remarks>
                public void fill<T> ( IEnumerable<ListItemContainer<T>> items , ListItemContainer<T>? itemToSelect , ComboBoxStyle style = ComboBoxStyle.DropDownList )
                {

                    void a ()
                    {
                        source?.clearItems(true);
                        if ( source is ComboBox cbo ) cbo.DropDownStyle = style;

                        if ( !items.Any() ) return;

                        var a = items.ToArray();

                        source?.fill(a , selectionMode: SelectionMode.None);

                        if ( itemToSelect == null ) return;
                        //itemToSelect ??= a[ 0 ];

                        if ( source is ComboBox cbo2 )
                            cbo2.SelectedItem = itemToSelect!;
                        else if ( source is ListBox lst2 )
                            lst2.SelectedItem = itemToSelect!;
                    }

                    source?.runOnLockedUpdateInUIThread(a);
                }


                ///<summary>Fills ComboBox with <see cref="ListItemContainer"/> wrappers and returns selected index</summary>
                ///<remarks>Sample: cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING)</remarks>

                public void fill<T> ( IEnumerable<ListItemContainer<T>> items , T? itemToSelect = default , ComboBoxStyle style = ComboBoxStyle.DropDownList )
                {
                    ListItemContainer<T>[]? itemsArray = [ .. items ];

                    var containerToSelect = itemsArray
                    .FirstOrDefault(ii => ii.Value.eEqualsUniversal(itemToSelect));

                    if ( containerToSelect == null )
                        throw new ArgumentOutOfRangeException(nameof(itemToSelect) , $"Not Found {typeof(ListItemContainer<T>)} with value='{itemToSelect}' in list to select!");

                    source?.fill<TListCtl , T>(itemsArray , containerToSelect , style);
                }


                ///<summary>Fills ComboBox with <see cref="ListItemContainer"/> wrappers and returns selected index</summary>
                ///<remarks>Sample: cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING)</remarks>
                public void fillWithContainersOf<T> ( IEnumerable<T> items , T? itemToSelect = default , ComboBoxStyle style = ComboBoxStyle.DropDownList )
                {

                    ListItemContainer<T>? containerToSelect = null;

                    var containers = items
                    .Select(obj =>
                    {
                        ListItemContainer<T> cc = new(obj);
                        if ( itemToSelect != null && obj.eEqualsUniversal(itemToSelect) )
                        {
                            containerToSelect = cc;
                        }
                        return cc;
                    })
                    .ToArray();

                    source?.fill<TListCtl , T>(containers , containerToSelect , style);
                }


                ///<summary>Fills ComboBox with <see cref="EnumContainer"/> wrappers. ONLY FOR ENUM!</summary>
                ///<remarks>Sample: cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING.ByCaller)</remarks>

                public void fillWithContainersOfEnum<TEnum> ( TEnum selectedItem ) where TEnum : Enum
                    => source?.fill<TListCtl , TEnum>(selectedItem.getAllValuesAsEnumContainers() , selectedItem);



                public ListItemContainer<T>? SelectContainerOf<T> ( T? itemToSelect , bool selectFirstItemIfNull = true )
                {
                    if ( source is ComboBox cbo )
                    {
                        if ( cbo.Items.Count < 1 ) return null;
                        var containerToSelect = cbo
                        .itemsAs_ContainerOf<T>()
                        .FirstOrDefault(c => c.Value.eEqualsUniversal(itemToSelect));

                        if ( containerToSelect != null )
                            cbo.SelectedItem = containerToSelect;
                        else if ( selectFirstItemIfNull )
                            cbo.SelectedIndex = 0;

                        return containerToSelect;
                    }
                    else if ( source is ListBox lst )
                    {
                        if ( lst.Items.Count < 1 ) return null;
                        var containerToSelect = lst
                        .itemsAs_ContainerOf<T>()
                        .FirstOrDefault(c => c.Value.eEqualsUniversal(itemToSelect));

                        if ( containerToSelect != null )
                            lst.SelectedItem = containerToSelect;
                        else if ( selectFirstItemIfNull )
                            lst.SelectedIndex = 0;

                        return containerToSelect;
                    }
                    else throw new ArgumentOutOfRangeException(nameof(source));
                }



                public bool AppendStringIfNotExist ( string newItem , StringComparer? comparer = null )
                {
                    comparer ??= StringComparer.CurrentCultureIgnoreCase;

                    if ( source is ComboBox cbo )
                    {
                        if ( !cbo.itemsAsEnumerableOfType<string>().Contains(newItem , comparer) )
                        {
                            cbo.Items.Add(newItem);
                            return true;
                        }
                        return false;
                    }
                    else if ( source is ListBox lst )
                    {
                        if ( !lst.itemsAsEnumerableOfType<string>().Contains(newItem , comparer) )
                        {
                            lst.Items.Add(newItem);
                            return true;
                        }
                        return false;
                    }
                    else throw new ArgumentOutOfRangeException(nameof(source));
                }

            }







            extension( ProgressBar source )
            {

                internal void setState ( ComCtl32.ProgressState state )
                    => User32.SendMessage(source!.Handle , ComCtl32.ProgressMessage.PBM_SETSTATE , state);


                internal void setValues ( int iMin = 0 , int iMax = 100 , int iValue = 0 )
                {
                    source?.SuspendLayout();
                    try
                    {
                        source?.Minimum = iMin;
                        source?.Maximum = iMax;
                        source?.Value = iValue;
                    }
                    finally
                    {
                        source?.ResumeLayout();
                        source?.Update();
                    }
                }


            }



            extension( ImageList source )
            {

                internal void setKeyNames ( int startIndex , params string[] keys )
                {
                    if ( source == null ) return;

                    for ( int i = 0 ; i < keys.Length ; i++ )
                    {
                        source?.Images.SetKeyName(startIndex + i , keys[ i ]);
                    }
                }

            }

        }


        internal static class Extensions_WinForms_FileDialogs
        {


            extension( SaveFileDialog source )
            {

                public void initDefault ( string defaultExt , string filter )
                {
                    source?.ShowHelp = false;
                    source?.AddExtension = true;
                    source?.AutoUpgradeEnabled = true;
                    source?.CheckPathExists = true;
                    source?.DereferenceLinks = true;
                    source?.DefaultExt = defaultExt;
                    source?.SupportMultiDottedExtensions = true;
                    source?.ValidateNames = true;
                    source?.Filter = filter;
                    source?.OverwritePrompt = true;
                }

            }


            /* 
					<DebuggerNonUserCode, DebuggerStepThrough>
					<MethodImpl(MethodImplOptions.AggressiveInlining), Extension()>
					Public Function eOpenSaveFileDialog(ParentForm As Form,
															  Optional sDefaultFileName As string = vbNullString,
															  Optional sDefaultExt As string = C_DEFAULT_XML_EXT,
															  Optional sFilter As string = C_DEFAULT_XML_EXPORT_FILTER,
															  Optional sAutoFileNameSuffix As string = "Данные от",
															  Optional neInitialDirectory As Nullable(Of Environment.SpecialFolder) = Environment.SpecialFolder.DesktopDirectory) As string
						Using SFD As New SaveFileDialog
							With SFD
								If (sDefaultFileName.IsNullOrWhiteSpace) Then
									sDefaultFileName = Now.ToFileName
									If (sAutoFileNameSuffix.IsNotNullOrWhiteSpace) Then sDefaultFileName = sAutoFileNameSuffix & " " & sDefaultFileName
								End If
								.FileName = sDefaultFileName

								.DefaultExt = sDefaultExt
								.Filter = sFilter

								.AddExtension = True
								.AutoUpgradeEnabled = True
								.CheckPathExists = True
								.DereferenceLinks = True
								.ShowHelp = False
								.SupportMultiDottedExtensions = True
								.ValidateNames = True


								If neInitialDirectory.HasValue Then
									Dim eInitialDirectory = neInitialDirectory.Value
									.InitialDirectory = Environment.GetFolderPath(eInitialDirectory)
								End If

								If (.ShowDialog(ParentForm) <> Forms.DialogResult.OK) Then Return vbNullString

								Return .FileName
							End With
						End Using
					End Function





		# End Region


							 */



            extension( OpenFileDialog source )
            {

                public void initDefault ( string defaultExt , string filter , bool Multiselect = false )
                {
                    source?.ShowHelp = false;
                    source?.ShowReadOnly = false;
                    source?.AutoUpgradeEnabled = true;
                    source?.AddExtension = true;
                    source?.CheckPathExists = true;
                    source?.CheckFileExists = true;
                    source?.DereferenceLinks = true;
                    source?.SupportMultiDottedExtensions = true;
                    source?.ValidateNames = true;
                    source?.Multiselect = Multiselect;
                    source?.Filter = filter;
                    if ( defaultExt.isNotNullOrWhiteSpace ) source?.DefaultExt = defaultExt;
                }

            }

        }



        internal static class Extensions_WinForms_MenuAndToolbar
        {

            extension( ToolStrip source )
            {

                public void enableItems ( bool e )
                {
                    var cc = source?.Items?.Cast<ToolStripItem>() ?? [];
                    foreach ( ToolStripItem ctl in cc )
                    {
                        ctl.Enabled = e;
                    }
                }

            }


            extension( ToolStripItem source )
            {

                /// <summary>MT Safe</summary>
                public void runInUIThread_SetText ( string Text , Form? frmUI = null )
                {
                    frmUI ??= source?.GetCurrentParent()?.FindForm();//If form not specifed = search form...
                    frmUI?.runInUIThread(() =>
                    {
                        if ( (frmUI?.IsHandleCreated ?? false) && !(frmUI?.IsDisposed ?? true) && !(source?.IsDisposed ?? true) )
                        {
                            source?.Text = Text;
                        }
                    });
                }


            }



            extension( ToolStripTextBox source )
            {

                //// <inheritdoc cref="AttachDelayedFilter" />
                public void attach_DelayedFilter (
                Action<string> onTextChanged ,
                int delay = Extensions.Extensions_WinForms_Controls.DEFAULT_TEXT_EDIT_DELAY ,
                string cueBanner = Extensions.Extensions_WinForms_Controls.DEFAULT_FILTER_CUEBANNER ,
                bool changeBackColor = true )
                    => source!.TextBox!.attach_DelayedFilter(onTextChanged , delay , cueBanner , changeBackColor);


                //// <inheritdoc cref="AttachDelayedFilter" />
                public void attach_DelayedFilterTask (
                    Task onTextChanged ,
                    int delay = Extensions.Extensions_WinForms_Controls.DEFAULT_TEXT_EDIT_DELAY ,
                    string cueBanner = Extensions.Extensions_WinForms_Controls.DEFAULT_FILTER_CUEBANNER ,
                    bool changeBackColor = true )
                    => source!.TextBox!.attach_DelayedFilterTask(onTextChanged , delay , cueBanner , changeBackColor);

                //// <inheritdoc cref="AttachDelayedFilter" />
                public void attach_DelayedFilterTask (
                    OnFilterChangedDelegate onTextChanged ,
                    int delay = Extensions.Extensions_WinForms_Controls.DEFAULT_TEXT_EDIT_DELAY ,
                    string cueBanner = Extensions.Extensions_WinForms_Controls.DEFAULT_FILTER_CUEBANNER ,
                    bool changeBackColor = true )
                    => source!.TextBox!.attach_DelayedFilterTask(onTextChanged , delay , cueBanner , changeBackColor);

            }

            extension( IEnumerable<ToolStripItem>? source )
            {

                internal ToolStripMenuItem? searchMenuItemTree ( Func<ToolStripMenuItem , bool> predicate )
                {
                    foreach ( ToolStripItem tsi in source! )
                    {
                        switch ( tsi )
                        {
                            case ToolStripMenuItem mi:
                                {
                                    if ( predicate(mi) ) return mi;

                                    IEnumerable<ToolStripItem> childItems = mi.DropDownItems.OfType<ToolStripItem>();
                                    if ( childItems.Any() )
                                    {
                                        ToolStripMenuItem? miChild = searchMenuItemTree(childItems , predicate);
                                        if ( miChild != null ) return miChild;
                                    }
                                    break;
                                }

                            default: break;
                        }
                    }

                    return null;
                }

            }

            extension( ToolStripMenuItem source )
            {

                internal ToolStripMenuItem? searchMenuItemTree ( Func<ToolStripMenuItem , bool> predicate )
                    => source?.DropDownItems?.OfType<ToolStripItem>()?.searchMenuItemTree(predicate);


                internal void remove ()
                {
                    ToolStripItem? o = source?.OwnerItem;
                    if ( o == null ) return;

                    switch ( o )
                    {
                        case ToolStripMenuItem tsmi:
                            {
                                tsmi.DropDownItems.Remove(source!);
                                break;
                            }
                        default: throw new ArgumentException($"Unknown {o.GetType()}");
                    }
                }

            }

            extension( ContextMenuStrip source )
            {
                internal ToolStripMenuItem? searchMenuItemTree ( Func<ToolStripMenuItem , bool> predicate )
                    => source?.Items?.OfType<ToolStripItem>()?.searchMenuItemTree(predicate);

            }

        }



        internal static class Extensions_WinForms_ListView
        {




            extension( ListView source )
            {


                public void selectAllItems ()
                {
                    if ( source == null ) return;
                    try
                    {
                        source.BeginUpdate();
                        {
                            foreach ( ListViewItem li in source.Items )
                            {
                                li.Selected = true;
                            }
                        }
                    }
                    finally { source.EndUpdate(); }
                }


                /// <summary>
                /// if (Items.Count > 0) used AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
                /// else used AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                /// </summary>
                public void autoSizeColumnsAuto ()
                    => source?.AutoResizeColumns(
                        source.Items.any
                        ? ColumnHeaderAutoResizeStyle.ColumnContent
                        : ColumnHeaderAutoResizeStyle.HeaderSize
                        );



                internal IEnumerable<ListViewGroup> addGroups ( IEnumerable<ListViewGroup> groups , bool autoSizeColumns = true , bool clearGroupsBefore = false , bool lockedUpdate = true )
                {
                    if ( source != null )
                    {
                        Action a = new(() =>
                        {
                            if ( clearGroupsBefore ) source.Groups.Clear();
                            if ( groups.Any() ) source?.Groups.AddRange([ .. groups ]);
                        });

                        if ( lockedUpdate )
                        {
                            source.runOnLockedUpdate(a , autoSizeColumns);
                        }
                        else
                        {
                            a.Invoke();
                            source.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        }
                    }
                    return groups;
                }



                #region eAddItems 



                internal IEnumerable<ListViewItem> addItems ( IEnumerable<ListViewItem> items , bool autoSizeColumns = true , bool clearItemsBefore = false , bool clearGroupsBefore = false , bool lockedUpdate = true )
                {
                    if ( source != null )
                    {
                        Action a = new(() =>
                        {
                            if ( clearItemsBefore ) source.Items.Clear();
                            if ( clearGroupsBefore ) source.Groups.Clear();
                            if ( items.Any() ) source.Items.AddRange([ .. items ]);
                        });

                        if ( lockedUpdate )
                        {
                            source.runOnLockedUpdate(a , autoSizeColumns);
                        }
                        else
                        {
                            a.Invoke();
                            source.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        }
                    }
                    return items;
                }


                #endregion



                internal void clear ( bool autoSizeColumns = true , bool clearItems = true , bool clearGroups = true , bool clearColumns = false )
                {
                    source?.runOnLockedUpdate(() =>
                    {
                        if ( clearItems ) source!.Items.Clear();
                        if ( clearGroups ) source!.Groups.Clear();
                        if ( clearColumns ) source!.Columns.Clear();

                    } , autoSizeColumns);
                }

                public void clearItems ( bool autoSizeColumns = false , bool disposeDisposableItems = false )
                {

                    IDisposable[] itemsToDispose = [.. disposeDisposableItems
                        ? (source?.itemsAsIEnumerable()?.OfType<IDisposable>() ?? [])
                        : []
                        ];

                    source?.clear(autoSizeColumns , clearItems: true , clearGroups: false , clearColumns: false);


                    if ( disposeDisposableItems && itemsToDispose != null && itemsToDispose.any )
                    {
                        itemsToDispose.forEach(li => li.Dispose());
                    }
                }


                public IEnumerable<ListViewGroup> groupsAsIEnumerable ()
                    => source?
                    .Groups
                    .OfType<ListViewGroup>() ?? [];


#if NET


                public (ListViewGroup Group, bool Created) groupsCreateGroupByKey (
                    string key ,
                    string? header = null ,
                    Action<ListViewGroup>? onNewGroup = null ,
                    ListViewGroupCollapsedState newGroupState = ListViewGroupCollapsedState.Collapsed
                    )
                {
                    ListViewGroup? grp = source?.groupsAsIEnumerable()?.FirstOrDefault(g => g.Name == key);
                    bool exist = grp != null;
                    if ( !exist )
                    {
                        grp = new ListViewGroup(key , header ?? key);
                        source?.Groups.Add(grp);
                        onNewGroup?.Invoke(grp);
                        grp.CollapsedState = newGroupState;
                    }
                    return (grp!, !exist);
                }
#else
			// Defined in ListViewEx extensions...
			
			public static (ListViewGroup Group, bool Created) eGroupsCreateGroupByKey (
				this ListView lvw,
				string key,
				string? header = null,
				Action<ListViewGroup>? onNewGroup = null
			)
			{
				ListViewGroup? foundGroup = lvw!.eGroupsAsIEnumerable().FirstOrDefault(g => (g.Name == key));
				if (foundGroup != null) return (foundGroup, false);

				ListViewGroup grp = new(key, header ?? key);
				lvw.Groups.Add(grp);
				onNewGroup?.Invoke(grp);
				return (grp, true);
			}
#endif






                /*




                internal static void eSetAllGroupsTitlesBy_TagAndCount(this ListView lvw)
                {
                var GNP = ListViewGroup grp =>
                {
                var sTitle = G.Tag.ToString;
                return sTitle;
                };

                lvw.eSetAllGroupsTitlesBy_Count(GNP);
                }

                */



                public void setGroupsTitlesFast (
                    Func<ListViewGroup , string>? getGroupHeader = null )
                    => source?
                    .groupsAsIEnumerable()
                    .forEach(g =>
                    {
                        string sTitle = g.Name ?? "";
                        sTitle = getGroupHeader != null ? getGroupHeader.Invoke(g) : $"{sTitle} ({g.Items.Count:N0})".Trim();

                        if ( !string.IsNullOrWhiteSpace(sTitle) )
                        {
                            g.Header = sTitle;
                        }
                    });


                public void setGroupsState ( ListViewGroupCollapsedState state = ListViewGroupCollapsedState.Collapsed )
                        => source?
                    .groupsAsIEnumerable()
                    .forEach(g => g.CollapsedState = state);


                ///<summary>MT Safe!!!</summary>
                public void runOnLockedUpdate ( Action a , bool autoSizeColumns = false , bool fastUpdateGroupHeaders = false )
                {
                    _ = a ?? throw new ArgumentNullException(nameof(a));

                    Action a2 = delegate
                    {
                        source?.BeginUpdate();
                        try
                        {
                            a!.Invoke();
                        }
                        finally
                        {
                            if ( autoSizeColumns ) source?.autoSizeColumnsAuto();
                            if ( fastUpdateGroupHeaders ) source?.setGroupsTitlesFast();
                            source?.EndUpdate();
                        }
                    };

                    if ( source != null && source.InvokeRequired )
                    {
                        source.runInUIThread(a2);
                    }
                    else
                    {
                        a2.Invoke();
                    }
                }

                ///<summary>MT Safe!!!</summary>
                public async Task runOnLockedUpdateAsync (
                    Func<Task> a ,
                    bool autoSizeColumns = false ,
                    bool fastUpdateGroupHeaders = false )
                {
                    _ = a ?? throw new ArgumentNullException(nameof(a));

                    async Task SafeUpdateCore ()
                    {
                        source?.BeginUpdate();
                        try
                        {
                            await a!.Invoke();
                        }
                        finally
                        {
                            if ( autoSizeColumns )
                            {
                                source?.autoSizeColumnsAuto();
                            }

                            if ( fastUpdateGroupHeaders )
                            {
                                source?.setGroupsTitlesFast();
                            }

                            source?.EndUpdate();
                        }
                    }

                    if ( source != null && source.InvokeRequired )
                    {
                        await source.runInUIThreadAsync(SafeUpdateCore)!;
                    }
                    else
                    {
                        await SafeUpdateCore();
                    }
                }


                ///<summary>MT Safe!!!</summary>

                public T? runOnLockedUpdate<T> (
                    Func<T> a ,
                    bool autoSizeColumns = false ,
                    bool fastUpdateGroupHeaders = false )
                {
                    _ = a ?? throw new ArgumentNullException(nameof(a));

                    T? SafeUpdateCore ()
                    {
                        source?.BeginUpdate();
                        try { return a!.Invoke(); }
                        finally
                        {
                            if ( autoSizeColumns ) source?.autoSizeColumnsAuto();

                            if ( fastUpdateGroupHeaders ) source?.setGroupsTitlesFast();

                            source?.EndUpdate();
                        }
                    }

                    return source != null && source.InvokeRequired ? source.runInUIThread(SafeUpdateCore) : SafeUpdateCore();
                }



                public async Task<T?> runOnLockedUpdateAsync<T> (
                    Func<Task<T?>> tsk ,
                    bool autoSizeColumns = false ,
                    bool fastUpdateGroupHeaders = false ) where T : class
                {
                    //ArgumentNullException.ThrowIfNull(tsk);
                    tsk.ThrowIfNull();// = tsk ?? throw new ArgumentNullException(nameof(tsk));

                    source?.BeginUpdate();
                    try
                    {
                        //tsk.Start();
                        T? ret = await tsk.Invoke();
                        return ret;
                    }
                    finally
                    {
                        if ( autoSizeColumns ) source?.autoSizeColumnsAuto();
                        if ( fastUpdateGroupHeaders ) source?.setGroupsTitlesFast();
                        source?.EndUpdate();
                    }
                }




                /*

                public static async Task<T?> erunOnLockedUpdateAsync<T>(
                    this ListView lvw,
                    Task<T?> tsk,
                    bool autoSizeColumns = false,
                    bool fastUpdateGroupHeaders = false) where T : class
                {
                    //ArgumentNullException.ThrowIfNull(tsk);
                    tsk.ThrowIfNull();// = tsk ?? throw new ArgumentNullException(nameof(tsk));

                    lvw?.BeginUpdate();
                    try
                    {
                        tsk.Start();
                        T? ret = await tsk;
                        return ret;
                    }
                    finally
                    {
                        if (autoSizeColumns) lvw?.eAutoSizeColumns();
                        if (fastUpdateGroupHeaders) lvw?.eSetGroupsTitlesFast();
                        lvw?.EndUpdate();
                    }
                }
                             */

                //************************************** OLD



                public int items_Count => source?.Items?.Count ?? 0;
                public int items_SelectedCount => source?.SelectedItems?.Count ?? 0;
                public int items_CheckedCount => source?.CheckedItems?.Count ?? 0;


                public void selectFirstItem ()
                {
                    var first = source?.itemsAsIEnumerable().FirstOrDefault();
                    if ( first == default ) return;
                    first.Selected = true;
                    first.Focused = true;
                    first.EnsureVisible();
                }




                public void itemsRemoveRange (
                    IEnumerable<ListViewItem> ItemsToRemove ,
                    bool aAutoSizeColumnsAtFinish = false )
                    => source?.runOnLockedUpdate(()
                        => source?.Items?.itemsRemoveRange(ItemsToRemove) , aAutoSizeColumnsAtFinish);



                #region ItemsAsIEnumerable


                public IEnumerable<ListViewItem> itemsAsIEnumerable ()
                    => source?.Items?.OfType<ListViewItem>() ?? [];

                public IEnumerable<ListViewItem> selectedItemsAsIEnumerable ()
                    => source?.SelectedItems?.OfType<ListViewItem>() ?? [];

                public IEnumerable<ListViewItem> checkedItemsAsIEnumerable ()
                    => source?.CheckedItems?.OfType<ListViewItem>() ?? [];


                #endregion



                #region ItemsAs


                public IEnumerable<T> itemsAs<T> () where T : ListViewItem => source?.Items?.OfType<T>() ?? [];
                public IEnumerable<T> selectedItemsAs<T> () where T : ListViewItem => source?.SelectedItems?.OfType<T>() ?? [];
                public IEnumerable<T> checkedItemsAs<T> () where T : ListViewItem => source?.CheckedItems?.OfType<T>() ?? [];


                #endregion


                public IEnumerable<ColumnHeader> columnsAsIEnumerable () => source?.Columns?.OfType<ColumnHeader>() ?? [];


                public ListViewGroup? groupsGetByKey ( string key , StringComparison stringComparison = StringComparison.OrdinalIgnoreCase )
                    => source?.groupsAsIEnumerable()?.FirstOrDefault(grp => (grp.Name ?? string.Empty).Equals(key , stringComparison));


                /// <summary>Counts distance from cursor to the nearest item in the list.</summary>
                public (ListViewItem? Item, Size? VectorToItemCenter, double? Distance) getNearestItem ( Point pt )
                {
                    if ( source == null || source.Items.Count < 1 ) return (null, null, null);

                    var nearest = source
                        .itemsAsIEnumerable()
                        .Select(item =>
                        {
                            Rectangle rcItem = item.GetBounds(ItemBoundsPortion.Entire);
                            var ptItemCenter = rcItem.eGetCenter().eRoundToInt();

                            var szDistace = pt.eGetVectorTo(ptItemCenter);
                            var hyp = szDistace.eGetHypotenuse();
                            var hypA = Math.Abs(hyp);
                            return (Item: item, VectorToItemCenter: szDistace, Distace: hyp, DistaceAbs: hypA, Center: ptItemCenter);
                        })
                        .OrderBy(r => r.Distace)
                        .FirstOrDefault();

                    return (nearest.Item, nearest.VectorToItemCenter, nearest.Distace);
                }



                public int columnsCount
                    => source?.Columns?.Count ?? 0;


                public Dictionary<string , ColumnHeader> addColumns ( string[] columnNames )
                {
                    if ( source == null ) return [];

                    //cols.Clear();
                    var dic = columnNames
                        .Select(cn => new { Key = cn , Header = source.Columns.Add(cn) })
                        .ToDictionary(tuple => tuple.Key , tuple => tuple.Header);

                    return dic;
                }

                public Dictionary<string , ColumnHeader> addColumns ( bool clearColumnsBefore , string[] columnNames )
                {
                    if ( source == null ) return [];

                    if ( clearColumnsBefore ) source.Columns.Clear();
                    var dic = source.addColumns(columnNames);
                    return dic;
                }



            }


            extension( ListViewItem source )
            {


                public void runOnLockedUpdateIfNeed ( Action a )
                {
                    if ( source.ListView == null )
                    {
                        a.Invoke();
                    }
                    else
                    {
                        source.ListView.runOnLockedUpdate(a);
                    }
                }



                public void addFakeSubitems ( int columnsCount , string fakeText = "" )
                {
                    for ( int i = 0 ; i < columnsCount ; i++ )
                    {
                        source.SubItems.Add(fakeText);
                    }
                }



                public void addFakeSubitems ( ListView? lvw = null , string fakeText = "" )
                {
                    lvw ??= source.ListView;
                    lvw.ThrowIfNull();//ArgumentNullException.ThrowIfNull(lvw);
                    source.addFakeSubitems(lvw!.Columns.Count , fakeText);
                }


                public void updateTexts ( int startIndex , [AllowNullAttribute] params string?[] subItemTexts )
                {
                    if ( subItemTexts == null || subItemTexts.Length < 1 ) return;

                    void updateCore ()
                    {
                        int i = 0;
                        subItemTexts.forEach(text =>
                        {
                            if ( null != text )
                            {
                                source.SubItems[ startIndex + i ].Text = text;
                            }
                            i++;
                        });
                    }

                    source.runOnLockedUpdateIfNeed(updateCore);
                }

                public void updateTexts ( [AllowNullAttribute] params string?[] subItemTexts )
                    => source.updateTexts(0 , subItemTexts);


                public void activate ( bool deactivateOther = true )
                {
                    void activateCore ()
                    {
                        if ( deactivateOther )
                        {
                            source
                                .ListView?
                                .itemsAsIEnumerable()?
                                .ToList()?
                                .ForEach(li => li.Selected = false);
                        }

                        source.Selected = true;
                        source.Focused = true;
                        source.EnsureVisible();
                    }
                    source.runOnLockedUpdateIfNeed(activateCore);
                }



                public void addSubitems ( params string[] subitems )
                    => subitems?.forEach(s => source?.SubItems.Add(s));


                public async void flashAsync ( int flashCount = 10 )
                {
                    Color clrFore = source.ForeColor, clrBack = source.BackColor; //Remember source color values
                    try
                    {
                        // swap colors
                        for ( int i = 1, loopTo = flashCount * 2 ; i <= loopTo ; i++ )
                        {
                            (source.BackColor, source.ForeColor) = (source.ForeColor, source.BackColor);
                            await Task.Delay(100);
                        }
                    }
                    finally
                    {
                        //Restore original colors
                        source.ForeColor = clrFore;
                        source.BackColor = clrBack;
                    }
                }



                /// <summary>Предыдущий элемент (Index меньше на 1)</summary>
                public ListViewItem? previous => (source!.Index <= 0)
                    ? null
                    : source.ListView!.Items[ source.Index - 1 ];

                /// <summary>Предыдущий элемент в той же группе (Index меньше на 1)</summary>
                public ListViewItem? previousInGroup
                {
                    get
                    {
                        var pp = source.previous;
                        return pp != null && object.ReferenceEquals(pp.Group , source.Group)
                            ? pp
                            : null;
                    }
                }


                /// <summary>Next элемент (Index +1)</summary>
                public ListViewItem? next
                    => (source.ListView == null)
                    ? null
                    : (source.Index >= (source.ListView.items_Count - 1))
                        ? null
                        : source.ListView.Items[ source.Index + 1 ];


                /// <summary>Next элемент в той же группе (Index +1)</summary>
                public ListViewItem? nextInGroup
                {
                    get
                    {
                        var nn = source.next;
                        return nn != null && object.ReferenceEquals(nn.Group , source!.Group)
                            ? nn
                            : null;
                    }
                }



            }


            extension( ListView.ListViewItemCollection source )
            {

                public void itemsRemoveRange ( IEnumerable<ListViewItem> ItemsToRemove )
                        => ItemsToRemove.forEach(li => source!.Remove(li));

                public IEnumerable<T> selectedItemsAs<T> () where T : ListViewItem
                    => source?.OfType<T>()?.selectedItems() ?? [];

                public IEnumerable<T> checkedItemsAs<T> () where T : ListViewItem
                    => source?.OfType<T>()?.checkedItems() ?? [];

            }


            extension<T> ( IEnumerable<T> rows ) where T : ListViewItem
            {
                public IEnumerable<T> selectedItems () => rows?.Where(li => li.Selected) ?? [];
                public IEnumerable<T> checkedItems () => rows?.Where(li => li.Checked) ?? [];

            }


            extension( ListViewGroup lvg )
            {

                public IEnumerable<ListViewItem> itemsAsIEnumerable ()
                        => lvg?.Items?.OfType<ListViewItem>() ?? [];

                public IEnumerable<T> itemsAs<T> () where T : ListViewItem
                    => lvg?.Items?.OfType<T>() ?? [];

                public IEnumerable<ListViewItem> selectedItems ()
                    => lvg?.Items?.selectedItemsAs<ListViewItem>() ?? [];

                public IEnumerable<ListViewItem> checkedItems ()
                    => lvg?.Items?.checkedItemsAs<ListViewItem>() ?? [];


                public IEnumerable<T> selectedItemsOfType<T> () where T : ListViewItem
                    => lvg?.Items?.selectedItemsAs<T>() ?? [];

                public IEnumerable<T> checkedItemsOfType<T> () where T : ListViewItem
                    => lvg?.Items?.checkedItemsAs<T>() ?? [];


            }



        }

        internal static class Extensions_WinForms_Clipboard
        {


            internal static bool eSetClipboardSafe ( this string text , bool clearBefore = false )
            {
                if ( clearBefore )
                {
                    try { System.Windows.Forms.Clipboard.Clear(); } catch { }
                }

                try
                {
                    System.Windows.Forms.Clipboard.SetText(text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }


            public static (ListViewGroup Group, bool Created) createGroupByKey (
                   this Dictionary<string , ListViewGroup> dicGroups ,
                   string key ,
                   string? header = null ,
                   Action<ListViewGroup>? onNewGroup = null )
            {
                bool exist = dicGroups.TryGetValue(key , out ListViewGroup? grp);
                if ( !exist )
                {
                    header ??= key;
                    grp = new ListViewGroup(key , header);
                    dicGroups.Add(key , grp);
                    onNewGroup?.Invoke(grp);
                }
                return (grp!, !exist);
            }

        }


        internal static partial class Extensions_WinForms_Drawing
        {





            internal static StringAlignment eGetXAlignment ( this ContentAlignment ca )
                => ca switch
                {
                    var ca2
                    when ca2 == ContentAlignment.TopLeft || ca2 == ContentAlignment.MiddleLeft || ca2 == ContentAlignment.BottomLeft
                    => StringAlignment.Near,

                    var ca2
                    when ca2 == ContentAlignment.TopCenter || ca2 == ContentAlignment.MiddleCenter || ca2 == ContentAlignment.BottomCenter
                    => StringAlignment.Center,

                    var ca2
                    when ca2 == ContentAlignment.TopRight || ca2 == ContentAlignment.MiddleRight || ca2 == ContentAlignment.BottomRight
                        => StringAlignment.Far,

                    _ => StringAlignment.Center,
                };




            internal static StringAlignment eGetYAlignment ( this ContentAlignment ca )
                => ca switch
                {
                    var ca2
                    when ca2 == ContentAlignment.TopLeft || ca2 == ContentAlignment.TopCenter || ca2 == ContentAlignment.TopRight
                    => StringAlignment.Near,

                    var ca2
                    when ca2 == ContentAlignment.MiddleLeft || ca2 == ContentAlignment.MiddleCenter || ca2 == ContentAlignment.MiddleRight
                    => StringAlignment.Center,

                    var ca2
                    when ca2 == ContentAlignment.BottomLeft || ca2 == ContentAlignment.BottomCenter || ca2 == ContentAlignment.BottomRight
                        => StringAlignment.Far,

                    _ => StringAlignment.Center,
                };




            [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static GraphicsPath eCreateRoundRect ( this Rectangle rc , int radius )
            {
                Rectangle rcCorner = new(rc.X , rc.Y , radius , radius);

                GraphicsPath path = new();
                path.AddArc(rcCorner , 180 , 90);

                rcCorner.X = rc.X + rc.Width - radius;
                path.AddArc(rcCorner , 270 , 90);

                rcCorner.Y = rc.Y + rc.Height - radius;
                path.AddArc(rcCorner , 0 , 90);

                rcCorner.X = rc.X;
                path.AddArc(rcCorner , 90 , 90);

                path.CloseFigure();
                return path;
            }


            [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void eDrawPath (
                   this Graphics g ,
                   GraphicsPath path ,
                   Pen? pen = null ,
                   Brush? brush = null )
            {
                if ( brush != null ) g?.FillPath(brush , path);
                if ( pen != null ) g?.DrawPath(pen , path);
            }


            [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void eDrawRoundRect (
                   this Graphics g ,
                   Rectangle rc ,
                   int radius ,
                   Pen? pen = null ,
                   Brush? brush = null )
            {
                using var path = rc.eCreateRoundRect(radius);
                g.eDrawPath(path , pen , brush);
            }


            [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void eDeflateByPadding ( this ref Rectangle rect , Padding pd )
            {
                //if (null == pd) return;

                if ( pd.Left > 0 )
                {
                    rect.Offset(pd.Left , 0);
                    rect.Width -= pd.Left;
                }
                if ( pd.Right > 0 ) rect.Width -= pd.Right;

                if ( pd.Top > 0 )
                {
                    rect.Offset(0 , pd.Top);
                    rect.Height -= pd.Top;
                }
                if ( pd.Bottom > 0 ) rect.Height -= pd.Bottom;
            }



            [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Icon eToIcon ( this Bitmap img )
                => Icon.FromHandle(img.GetHicon());





















            /// <summary>Расчёт точки вывода текста относительно заданной точки</summary>

            internal static (PointF TextPos, SizeF MeasuredTextSize) eMeasureStringLocation (
                this Graphics g ,
                string Text ,
                Font Font ,
                PointF TargetPoint ,
                System.Drawing.ContentAlignment alignment )
            {
                SizeF retMeasuredTextSize = g.MeasureString(Text , Font);
                var szfMeasuredTextSize2 = new SizeF(retMeasuredTextSize.Width / 2 , retMeasuredTextSize.Height / 2);
                var rcLabel = default(PointF);

                switch ( alignment )
                {
                    case System.Drawing.ContentAlignment.MiddleLeft: // Слева посередине
                        rcLabel = new PointF(TargetPoint.X - retMeasuredTextSize.Width , TargetPoint.Y - szfMeasuredTextSize2.Height); break;

                    case System.Drawing.ContentAlignment.BottomCenter: // Снизу по центру
                        rcLabel = new PointF(TargetPoint.X - szfMeasuredTextSize2.Width , TargetPoint.Y); break;

                    case System.Drawing.ContentAlignment.TopCenter: // Сверху по центру
                        rcLabel = new PointF(TargetPoint.X - szfMeasuredTextSize2.Width , TargetPoint.Y - retMeasuredTextSize.Height); break;

                    case System.Drawing.ContentAlignment.MiddleCenter:  // Центр
                        rcLabel = new PointF(TargetPoint.X - szfMeasuredTextSize2.Width , TargetPoint.Y - szfMeasuredTextSize2.Height); break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(alignment));
                }
                return (rcLabel, retMeasuredTextSize);
            }







            #region System.Drawing.ContentAlignment->System.Drawing.StringFormat, System.Windows.Forms.HorizontalAlignment->System.Drawing.StringAlignment



            internal static System.Drawing.StringFormat eToDrawingStringFormat (
                this System.Drawing.ContentAlignment CA ,
                StringFormatFlags FormatFlags = 0 )
            {
                StringAlignment VA = StringAlignment.Center;
                StringAlignment HA = StringAlignment.Center;
                switch ( CA )
                {
                    case System.Drawing.ContentAlignment.TopLeft:
                        {
                            HA = StringAlignment.Near;
                            VA = StringAlignment.Near;
                            break;
                        }

                    case System.Drawing.ContentAlignment.TopCenter:
                        {
                            HA = StringAlignment.Center;
                            VA = StringAlignment.Near;
                            break;
                        }

                    case System.Drawing.ContentAlignment.TopRight:
                        {
                            HA = StringAlignment.Far;
                            VA = StringAlignment.Near;
                            break;
                        }

                    case System.Drawing.ContentAlignment.MiddleLeft:
                        {
                            HA = StringAlignment.Near;
                            VA = StringAlignment.Center;
                            break;
                        }

                    case System.Drawing.ContentAlignment.MiddleCenter:
                        {
                            HA = StringAlignment.Center;
                            VA = StringAlignment.Center;
                            break;
                        }

                    case System.Drawing.ContentAlignment.MiddleRight:
                        {
                            HA = StringAlignment.Far;
                            VA = StringAlignment.Center;
                            break;
                        }

                    case System.Drawing.ContentAlignment.BottomLeft:
                        {
                            HA = StringAlignment.Near;
                            VA = StringAlignment.Far;
                            break;
                        }

                    case System.Drawing.ContentAlignment.BottomCenter:
                        {
                            HA = StringAlignment.Center;
                            VA = StringAlignment.Far;
                            break;
                        }

                    case System.Drawing.ContentAlignment.BottomRight:
                        {
                            HA = StringAlignment.Far;
                            VA = StringAlignment.Far;
                            break;
                        }
                }

                var SF = new StringFormat()
                {
                    Alignment = HA ,
                    LineAlignment = VA ,
                    FormatFlags = FormatFlags
                };
                return SF;
            }



            internal static System.Drawing.StringFormat eToDrawingStringFormat (
                this HorizontalAlignment A ,
                StringAlignment LineAlignment = StringAlignment.Center ,
                StringFormatFlags FormatFlags = 0 )
            {
                var SF = new StringFormat();
                switch ( A )
                {
                    case HorizontalAlignment.Center: SF.Alignment = StringAlignment.Center; break;
                    case HorizontalAlignment.Right: SF.Alignment = StringAlignment.Far; break;
                }

                SF.LineAlignment = LineAlignment;
                SF.FormatFlags = FormatFlags;
                return SF;
            }



            internal static System.Windows.Forms.TextFormatFlags eToTextFormatFlags ( this System.Drawing.StringFormat SF )
            {
                TextFormatFlags FF = 0;
                switch ( SF.Alignment )
                {
                    case StringAlignment.Near: FF |= TextFormatFlags.Left; break;
                    case StringAlignment.Center: FF |= TextFormatFlags.HorizontalCenter; break;
                    case StringAlignment.Far: FF |= TextFormatFlags.Right; break;
                }
                switch ( SF.LineAlignment )
                {
                    case StringAlignment.Near: FF |= TextFormatFlags.Top; break;
                    case StringAlignment.Center: FF |= TextFormatFlags.VerticalCenter; break;
                    case StringAlignment.Far: FF |= TextFormatFlags.Bottom; break;
                }
                return FF;
            }



            internal static System.Drawing.StringAlignment eToDrawingStringAlignment ( this System.Windows.Forms.HorizontalAlignment Align )
                => Align switch
                {
                    HorizontalAlignment.Center => StringAlignment.Center,
                    HorizontalAlignment.Left => StringAlignment.Near,
                    HorizontalAlignment.Right => StringAlignment.Far,
                    _ => StringAlignment.Near,
                };

            #endregion







            internal static string eGetExtensionForRawFormat ( this Bitmap IMG )
                => IMG.RawFormat.eGetExtension();





            internal static string eGetExtension ( this System.Drawing.Imaging.ImageFormat fmt )
            {
                string sExt = fmt.Equals(System.Drawing.Imaging.ImageFormat.Jpeg)
                    ? "jpg"
                    : fmt.Equals(System.Drawing.Imaging.ImageFormat.Png)
                        ? "png"
                        : fmt.Equals(System.Drawing.Imaging.ImageFormat.Bmp)
                        ? "Bmp"
                        : fmt.Equals(System.Drawing.Imaging.ImageFormat.Emf)
                        ? "Emf"
                        : fmt.Equals(System.Drawing.Imaging.ImageFormat.Exif)
                        ? "Exif"
                        : fmt.Equals(System.Drawing.Imaging.ImageFormat.Gif)
                        ? "Gif"
                        : fmt.Equals(System.Drawing.Imaging.ImageFormat.Icon)
                        ? "Ico"
                        : fmt.Equals(System.Drawing.Imaging.ImageFormat.Tiff)
                        ? "Tif"
                        : fmt.Equals(System.Drawing.Imaging.ImageFormat.Wmf) ? "Wmf" : throw new Exception($"Unknown image format: {fmt}");
                return "." + sExt.ToLower();
            }







            internal static Icon eCreateIcon ( this Image IMG , System.Drawing.Size IconSize )
            {
                var bmSmall = (Bitmap)IMG.GetThumbnailImage(IconSize.Width , IconSize.Height , default , default);
                return Icon.FromHandle(bmSmall.GetHicon());
            }







#if NETFRAMEWORK
			
			internal static string? eExtractAssociatedIcon (
				this FileInfo App,
				ImageList IML,
				bool TryLoadFileAsImage = false) => App!.FullName!.eExtractAssociatedIcon(IML, TryLoadFileAsImage);


			
			internal static string? eExtractAssociatedIcon (this string sPath, ImageList IML, bool TryLoadFileAsImage = false)
			{
				lock (IML)
				{
					sPath = sPath.ToLower();
					if (IML.Images.ContainsKey(sPath)) return sPath; // Для этого файла уже была извлечена иконка - используем её

					// Иконка ещё не извлекалась - Пытаемся извлечь её 
					if (TryLoadFileAsImage)
					{
						try
						{
							var BM = new Bitmap(sPath);
							// Image loaeded OK! Creating Icon
							var rFileIcon = BM.eCreateIcon(IML.ImageSize);
							if (rFileIcon != null)
							{
								var FileIcon_16 = new Icon(rFileIcon, IML.ImageSize);
								IML.Images.Add(sPath, FileIcon_16);
								return sPath;
							}
						}
						catch
						{
						}
						// Failed to load file as image!
					}

					// Use System Default Icon Extractor
					try
					{
						System.Drawing.Icon rFileIcon = System.Drawing.Icon.ExtractAssociatedIcon(sPath)!;
						System.Drawing.Icon FileIcon_16 = new Icon(rFileIcon, IML.ImageSize);
						IML.Images.Add(sPath, FileIcon_16);
						return sPath;
					}
					catch
					{
					}

					return null;
				}
			}



#endif




            internal static System.Drawing.Icon? eExtractAssociatedIcon ( this string file )
                => System.Drawing.Icon.ExtractAssociatedIcon(file);



            internal static System.Drawing.Icon? eExtractAssociatedIcon ( this FileSystemInfo FI )
                => FI.FullName.eExtractAssociatedIcon();














            /// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение.
            /// Уменьшает большие картинки
            /// </summary>

            internal static Bitmap eGetThumbnailBitmap ( this System.Drawing.Icon rIcon , System.Drawing.Size szCanvas )
                => rIcon.ToBitmap().eGetThumbnailBitmap(szCanvas , System.Windows.Forms.SystemInformation.IconSize , true);

            /// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение.
            /// Уменьшает большие картинки
            /// </summary>

            internal static Bitmap eGetThumbnailBitmap ( this Icon rIcon , ImageList rImageList )
            {
                return null == rImageList ? throw new ArgumentNullException(nameof(rImageList)) : rIcon.eGetThumbnailBitmap(rImageList.ImageSize);
            }



            /// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение. Уменьшает большие картинки</summary>
            /// <param name="CanvasSize">Выходной размер получаемого изображения</param>
            /// <param name="DrawImageSize">Размер рисуемого изображения на холсте. Если не указан, автоматически маленькие как есть, а большие уменьшались</param>

            internal static Bitmap eGetThumbnailBitmap ( this Icon rIcon , System.Drawing.Size CanvasSize , System.Drawing.Size? DrawImageSize = default , bool UpScaleSmallImgaes = false )
                => eGetThumbnailBitmap_CORE(rIcon.Size ,
                    ( G , RC ) => G.DrawIcon(rIcon , RC) ,
                    CanvasSize , DrawImageSize , UpScaleSmallImgaes);

            /// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение. Уменьшает большие картинки</summary>
            /// <param name="CanvasSize">Выходной размер получаемого изображения</param>
            /// <param name="DrawImageSize">Размер рисуемого изображения на холсте. Если не указан, автоматически маленькие как есть, а большие уменьшались</param>

            internal static Bitmap eGetThumbnailBitmap ( this Image rBitmap , System.Drawing.Size CanvasSize , System.Drawing.Size? DrawImageSize = default , bool UpScaleSmallImgaes = false )
                => eGetThumbnailBitmap_CORE(rBitmap.Size ,
                    ( G , RC ) => G.DrawImage(rBitmap , RC) ,
                    CanvasSize , DrawImageSize , UpScaleSmallImgaes);

            /// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение.
            /// Уменьшает большие картинки</summary>
            /// <param name="CanvasSize">Размер холста</param>
            /// <param name="DrawImageSize">Размер рисуемого изображения на холсте. Если не указан, автоматически маленькие как есть, а большие уменьшались</param>
            private static Bitmap eGetThumbnailBitmap_CORE (
                System.Drawing.Size ImageSize ,
                Action<Graphics , System.Drawing.Rectangle> DrawImageProc ,
                System.Drawing.Size CanvasSize ,
                System.Drawing.Size? DrawImageSize = null ,
                bool UpScaleSmallImgaes = false )
            {
                var szfCanvas = CanvasSize.eToSizeF();
                Bitmap bmCanvas = new(CanvasSize.Width , CanvasSize.Height , System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using Graphics g = Graphics.FromImage(bmCanvas);
                g.PageUnit = GraphicsUnit.Pixel;
                g.Clear(Color.Transparent);
                // Call G.DrawRectangle(Pens.Blue, rcFileBitmap)

                var szfDraw = (DrawImageSize ?? ImageSize).eToSizeF();
                if ( szfDraw.Width > szfCanvas.Width || szfDraw.Height > szfCanvas.Height )
                {
                    // Выводимое изображение превышает размеры холста, надо вписать
                    szfDraw = szfDraw.eВписатьВ(szfCanvas).TargetSize;
                }
                else if ( UpScaleSmallImgaes )
                {
                    // Мелкое изображание надо увеличить ?
                    var szfUpscaleTo = (DrawImageSize ?? CanvasSize).eToSizeF();
                    if ( ImageSize.Width < szfUpscaleTo.Width && ImageSize.Height > szfUpscaleTo.Height )
                    {
                        // Мелкое изображание надо увеличить!
                        szfDraw = szfDraw.eВписатьВ(szfUpscaleTo).TargetSize;
                    }
                }

                var ptfCenter = szfCanvas.eToRectangleF().eGetCenter();
                var rcDraw = szfDraw.eToRectangleF().eCenterTo(ptfCenter).eToRectangle();
                DrawImageProc.Invoke(g , rcDraw);
                return bmCanvas;




                #region OLD
                // Dim szfCanvas = szCanvas. eToSizeF
                // Dim bmCanvas = New Bitmap(szCanvas.Width, szCanvas.Height, Imaging.PixelFormat.Format32bppArgb)
                // Dim ptfCanvasCanter = szfCanvas. eToRectangle. eGetCenter
                // Using G = Graphics.FromImage(bmCanvas)
                // G.PageUnit = GraphicsUnit.Pixel
                // Call G.Clear(Color.Transparent)
                //  Call G.DrawRectangle(Pens.Blue, rcFileBitmap)

                // If (TargetImageSize IsNot Nothing) AndAlso (TargetImageSize.HasValue) AndAlso (TargetImageSize.Value <> Size.Empty) Then
                // Указан размер изображения на эскизе (сам размер эскиза задан другими размерами!) Всегда масштабируем
                // Dim szImageToDraw = TargetImageSize.Value

                // Dim rcfImageToDraw = szImageToDraw. eToRectangle. eToRectangleF. eCenterTo(ptfCanvasCanter)
                // Call G.DrawImage(imgImageToDraw, rcfImageToDraw)

                // Else
                // Принудательный размер не указан Маленькие рисуем как есть, большие уменьшаем
                // Dim szfImageToDraw = imgImageToDraw.Size. eToSizeF

                // If (Not ScaleSmallImgaes) AndAlso ((szfImageToDraw.Width <= szCanvas.Width) AndAlso (szfImageToDraw.Height <= szCanvas.Height)) Then
                // Изображание не превышает размеров холста
                // Dim rcfDrawTo = szfImageToDraw. eToRectangle. eCenterTo(ptfCanvasCanter)
                // Call G.DrawImage(imgImageToDraw, rcfDrawTo)

                // Else
                // Изображение больше холста - вписываем с сохранением пропорций
                // Dim fScaled = imgImageToDraw.Size. eToSizeF. eВписатьВ(szfCanvas)
                // Dim rcfScaled = fScaled.TargetSize. eToRectangle
                // rcfScaled = rcfScaled. eCenterTo(ptfCanvasCanter)
                // Call G.DrawImage(imgImageToDraw, rcfScaled)
                // End If
                // End If
                // Return bmCanvas
                // End Using
                #endregion

            }











            public static StringAlignment eGetAlignment ( this ContentAlignment ca )
                => ca switch
                {
                    var ca2
                    when ca2 == ContentAlignment.TopLeft || ca2 == ContentAlignment.MiddleLeft || ca2 == ContentAlignment.BottomLeft
                    => StringAlignment.Near,

                    var ca2
                    when ca2 == ContentAlignment.TopCenter || ca2 == ContentAlignment.MiddleCenter || ca2 == ContentAlignment.BottomCenter
                    => StringAlignment.Center,

                    var ca2
                    when ca2 == ContentAlignment.TopRight || ca2 == ContentAlignment.MiddleRight || ca2 == ContentAlignment.BottomRight
                        => StringAlignment.Far,

                    _ => StringAlignment.Center,
                };


            public static StringAlignment eGetLineAlignment ( this ContentAlignment ca )
                => ca switch
                {
                    var ca2
                    when ca2 == ContentAlignment.TopLeft || ca2 == ContentAlignment.TopCenter || ca2 == ContentAlignment.TopRight
                    => StringAlignment.Near,

                    var ca2
                    when ca2 == ContentAlignment.MiddleLeft || ca2 == ContentAlignment.MiddleCenter || ca2 == ContentAlignment.MiddleRight
                    => StringAlignment.Center,

                    var ca2
                    when ca2 == ContentAlignment.BottomLeft || ca2 == ContentAlignment.BottomCenter || ca2 == ContentAlignment.BottomRight
                        => StringAlignment.Far,

                    _ => StringAlignment.Center,
                };



            public static TextFormatFlags eToTextFormatFlags ( this ContentAlignment ca )
            => ca switch
            {
                ContentAlignment.TopLeft => TextFormatFlags.Left | TextFormatFlags.Top,
                ContentAlignment.TopCenter => TextFormatFlags.HorizontalCenter | TextFormatFlags.Top,
                ContentAlignment.TopRight => TextFormatFlags.Right | TextFormatFlags.Top,

                ContentAlignment.MiddleLeft => TextFormatFlags.Left | TextFormatFlags.VerticalCenter,
                ContentAlignment.MiddleCenter => TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                ContentAlignment.MiddleRight => TextFormatFlags.Right | TextFormatFlags.VerticalCenter,

                ContentAlignment.BottomLeft => TextFormatFlags.Left | TextFormatFlags.Bottom,
                ContentAlignment.BottomCenter => TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom,
                ContentAlignment.BottomRight => TextFormatFlags.Right | TextFormatFlags.Bottom,
                _ => TextFormatFlags.Default
            };




            public static StringFormat eToStringFormat ( this ContentAlignment ca )
            {
                var sf = new StringFormat()
                {
                    Alignment = ca.eGetAlignment() ,
                    LineAlignment = ca.eGetLineAlignment()
                };
                return sf;
            }




            #region DrawTextEx





            /// <returns>measured text size</returns>

            public static SizeF eDrawTextEx (
                this Graphics g ,
                string text ,
                Font font ,
                Color textcolor ,
                RectangleF rect ,
                ContentAlignment textAlign = ContentAlignment.MiddleCenter ,
                StringTrimming trimming = StringTrimming.Character ,
                bool autoHeightRect = false )
            {
                using Brush brText = new SolidBrush(textcolor);
                return g.eDrawTextEx(text , font , brText , rect , textAlign , trimming , autoHeightRect);
            }


            /// <returns>measured text size</returns>

            public static SizeF eDrawTextEx (
                this Graphics g ,
                string text ,
                Font font ,
                Brush textbrush ,
                RectangleF rc ,
                ContentAlignment textAlign = ContentAlignment.MiddleCenter ,
                StringTrimming trimming = StringTrimming.Character ,
                bool autoHeightRect = false
                )
            {
                using var sf = textAlign.eToStringFormat();
                sf.Trimming = trimming;

                var textSize = g.MeasureString(text , font , rc.Size , sf);
                if ( autoHeightRect )
                {
                    rc.Height = textSize.Height;
                }

                g.DrawString(text , font , textbrush , rc , sf);

                return textSize;
            }


            /// <returns>measured text size</returns>

            public static SizeF eDrawTextEx (
                this Graphics g ,
                string text ,
                Font font ,
                Brush textbrush ,
                PointF location ,
                ContentAlignment textAlign = ContentAlignment.MiddleCenter
                )
            {
                var msl = g.eMeasureStringLocation(text , font , location , textAlign);
                g.DrawString(text , font , textbrush , msl.TextPos);
                return msl.MeasuredTextSize;
            }





            internal static void eDrawTextEx (
                this Graphics g ,
                string text ,
                Font font ,
                Color textcolor ,
                Rectangle rect ,
                ContentAlignment textAlign = ContentAlignment.MiddleCenter ,
                StringFormatFlags? additionalFF = null )
            {
                using Brush brText = new SolidBrush(textcolor);
                g.eDrawTextEx(text , font , brText , rect , textAlign , additionalFF);
            }



            internal static void eDrawTextEx (
                this Graphics g ,
                string text ,
                Font font ,
                Brush textbrush ,
                Rectangle rect ,
                ContentAlignment textAlign = ContentAlignment.MiddleCenter ,
                StringFormatFlags? additionalFormatFlags = null )
            {
                using var sf = textAlign.eToStringFormat();
                if ( additionalFormatFlags != null && additionalFormatFlags.HasValue ) sf.FormatFlags |= additionalFormatFlags.Value;
                g.DrawString(text , font , textbrush , rect , sf);
            }


            #endregion






            public enum Direction { Up, Down, Right, Left }
            public enum GeoOrientation { North, South, East, West }
            public static GeoOrientation ToGeoOrientation ( Direction direction ) => direction switch
            {
                Direction.Up => GeoOrientation.North,
                Direction.Right => GeoOrientation.East,
                Direction.Down => GeoOrientation.South,
                Direction.Left => GeoOrientation.West,
                _ => throw new ArgumentOutOfRangeException(nameof(direction) , $"Not expected direction value: {direction}"),
            };



            internal enum PaperOrientations : int { Portrait, Landscape, Square }

            internal static PaperOrientations eGetOrientation ( this System.Drawing.Size szScreen )
                => (szScreen.Width == szScreen.Height)
                ? PaperOrientations.Square
                : (szScreen.Width > szScreen.Height)
                    ? PaperOrientations.Landscape
                    : PaperOrientations.Portrait;



            internal static bool eOrientationIsLandscape ( this System.Drawing.Size szScreen )
            {
                return szScreen.eGetOrientation() == PaperOrientations.Landscape;
            }


            internal static bool eOrientationIsPortrait ( this System.Drawing.Size szScreen )
            {
                return szScreen.eGetOrientation() == PaperOrientations.Portrait;
            }




            #region Inches / centimeters / pixels



            internal static SizeF eInches_ToPixels ( this Graphics g , SizeF SizeInInches )
                => new(
                    SizeInInches.Width * g.DpiX ,
                    SizeInInches.Height * g.DpiY
                    );



            internal static Size eMM_ToPixels ( this Graphics g , SizeF SizeInMM )
                => new(
                    (int)(SizeInMM.Width.distance_MMToInches() * g.DpiX) ,
                    (int)(SizeInMM.Height.distance_MMToInches() * g.DpiY)
                    );



            internal static SizeF eInchesToCM ( this SizeF valueInInches )
                => new(
                    valueInInches.Width.distance_InchesToCM() ,
                    valueInInches.Height.distance_InchesToCM());



            internal static RectangleF eInchesToCM ( this RectangleF valueInInches )
                => new(
                    valueInInches.Left.distance_InchesToCM() ,
                    valueInInches.Top.distance_InchesToCM() ,
                    valueInInches.Width.distance_InchesToCM() ,
                    valueInInches.Height.distance_InchesToCM()
                    );



            internal static RectangleF eCMToInches ( this RectangleF CM )
                => new(
                    CM.Left.distance_CMToInches() ,
                    CM.Top.distance_CMToInches() ,
                    CM.Width.distance_CMToInches() ,
                    CM.Height.distance_CMToInches()
                    );



            internal static System.Drawing.SizeF ePixels_To_Inches ( this Size pixels , System.Drawing.Point dpi )
                => new(
                    pixels.Width / (float)dpi.X ,
                    pixels.Height / (float)dpi.Y);



            internal static System.Drawing.Point eInches_To_Pixels ( this PointF Inches , System.Drawing.Point DPI )
                => new(
                    (int)(Inches.X * DPI.X) ,
                    (int)(Inches.Y * DPI.Y)
                    );



            internal static System.Drawing.Size eInches_To_Pixels ( this SizeF Inches , System.Drawing.Point DPI )
                => new(
                    (int)(Inches.Width * DPI.X) ,
                    (int)(Inches.Height * DPI.Y)
                    );


            internal static Rectangle eInches_To_Pixels ( this System.Drawing.RectangleF rcfДюймы , System.Drawing.Point DPI )
                => new(
                    rcfДюймы.Location.eInches_To_Pixels(DPI) ,
                    rcfДюймы.Size.eInches_To_Pixels(DPI)
                    );

            #endregion






            #region ВписатьРазмеры

            /// <summary>Вписывает одни размеры в другие, сохраняя пропорции</summary>
            /// <param name="source">Исходный размер, КОТОРЫЙ надо изменить</param>
            /// <param name="target">Целевой размер, В КОТОРЫЙ надо вписать</param>
            /// <returns>Zoom = коэффициент масштабирования</returns>

            internal static (System.Drawing.Size TargetSize, float Zoom) eВписатьВ ( this System.Drawing.Size source , System.Drawing.Size target
                )
            {
                //float sngZoom = 0f;
                var SourceF = new SizeF(source.Width , source.Height);
                var LimitF = new SizeF(target.Width , target.Height);
                var (TargetSize, Zoom) = SourceF.eВписатьВ(LimitF);
                return (TargetSize.ToSize(), Zoom);
            }

            /// <summary>Вписывает одни размеры в другие, сохраняя пропорции</summary>
            /// <param name="source">Исходный размер, КОТОРЫЙ надо изменить</param>
            /// <param name="target">Целевой размер, В КОТОРЫЙ надо вписать</param>
            /// <returns>Zoom = коэффициент масштабирования</returns>

            internal static (System.Drawing.SizeF TargetSize, float Zoom) eВписатьВ ( this SizeF source , SizeF target )
            {
                float zoom, w, h;

                if ( source.Width > source.Height )
                {
                    if ( target.Width > target.Height )
                    {
                        zoom = source.Height / source.Width;
                        w = target.Width;
                        h = target.Width * zoom;
                        if ( h > target.Height )
                        {
                            h = target.Height;
                            w = target.Height / zoom;
                        }
                    }
                    else // Limit.Height > Limit.Width
                    {
                        zoom = source.Height / source.Width;
                        w = target.Width;
                        h = target.Width * zoom;
                    }
                }
                else if ( target.Height > target.Width ) // Source.Height > Source.Width
                {
                    zoom = source.Width / source.Height;
                    h = target.Height;
                    w = target.Height * zoom;
                    if ( w > target.Width )
                    {
                        w = target.Width;
                        h = target.Width / zoom;
                    }
                }
                else // Limit.Width > Limit.Height
                {
                    zoom = source.Width / source.Height;
                    h = target.Height;
                    w = target.Height * zoom;
                }

                var szf = new SizeF(w , h);

                // Рассчитываем коэффициент Zoom'а, по большему из размеров, для большей точночти
                zoom = (w > h)
                    ? (w / source.Width)
                    : (h / source.Height);

                return (szf, zoom);
            }

            #endregion


            /// <summary>Размер = текущему разрешению экрана? Наверное будет гнать в многомониторной системе</summary>

            internal static bool eIsLikeScreen ( this System.Drawing.Size szTarget )
            {
                throw new NotImplementedException();

                //var szScreen = My.Computer.Screen.Bounds.Size;
                //bool IsLikeScr = szTarget == szScreen;
                //return IsLikeScr;
            }


            internal static bool eIsFullHD ( this System.Drawing.Size szTarget )
            {
                var szFullHD = new Size(1920 , 1080);
                return szTarget == szFullHD;
            }





            #region Point / Size / Rectangle - Туда / сюда преобразования



            internal static SizeF eMultiply ( this SizeF source , float Zoom ) => new(source.Width * Zoom , source.Height * Zoom);



            internal static PointF eMultiply ( this PointF source , float Zoom ) => new(source.X * Zoom , source.Y * Zoom);



            internal static Point eToPoint ( this PointF source ) => Point.Round(source);



            internal static PointF eToPoint ( this SizeF source ) => new(source.Width , source.Height);


            internal static Point eToPoint ( this Size source ) => new(source.Width , source.Height);



            internal static PointF eToPointF ( this Size source ) => new(source.Width , source.Height);



            internal static SizeF eToSize ( this PointF source ) => new(source.X , source.Y);



            internal static SizeF eToSizeF ( this Size source ) => new(source.Width , source.Height);



            internal static RectangleF eToRectangleF ( this System.Drawing.SizeF source , PointF? location = null )
                => new(
                    location.HasValue
                        ? location.Value
                        : new PointF(0 , 0) ,
                     source);



            internal static Rectangle eToRectangle ( this System.Drawing.Size source , Point location )
                => new(location , source);


            internal static Rectangle eToRectangle ( this System.Drawing.Size source )
                => new(new Point(0 , 0) , source);




            internal static RectangleF eToRectangleF ( this Rectangle source ) => new(source.Left , source.Top , source.Width , source.Height);



            internal static Rectangle eToRectangle ( this RectangleF source ) => System.Drawing.Rectangle.Round(source);


            /// <summary>Округляет, используя Round(Precission)</summary>

            internal static RectangleF eRound ( this RectangleF source , int precission )
            {
                float X, Y, W, H;
                X = source.Left.round(precission);
                Y = source.Top.round(precission);
                W = source.Width.round(precission);
                H = source.Height.round(precission);
                var RC = new RectangleF(X , Y , W , H);
                return RC;
            }



            // 	Function RectCenterRect(ByRef RcParent As Win.RECT, ByRef RcClient As Win.RECT) As Win.RECT
            // 		Dim WP, Xp, Yp, Hp As Integer
            // 		Dim Wc, Xc, Yc, Hc As Integer
            // 		Dim RR As Win.RECT

            // 		Xp = RcParent.Left
            // 		Yp = RcParent.Top
            // 		WP = RcParent.Right - RcParent.Left
            // 		Hp = RcParent.bottom - RcParent.Top
            // 		Xc = RcClient.Left
            // 		Yc = RcClient.Top
            // 		Wc = RcClient.Right - RcClient.Left
            // 		Hc = RcClient.bottom - RcClient.Top

            // 		RR.Left = (Xp + (WP \ 2)) - (Wc \ 2)
            // 		RR.Top = (Yp + (Hp \ 2)) - (Hc \ 2)

            // 		RR.Right = RR.Left + Wc
            // 		RR.bottom = RR.Top + Hc
            // 		'UPGRADE_WARNING: Couldn't resolve default property of object RectCenterRect. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            // 		RectCenterRect = RR
            // 	End Function
            // #endregion


            #endregion

            #region Тригонометрия, 3D

            /// <summary>Расчёт координат новой точки, повернутой вокруг заданной на угол</summary>
            /// <param name="ptfCenter">Точка, относительно оторой вращаемся</param>
            /// <param name="AngleRad">Угол в радианах</param>
            /// <param name="sngRadius">Радиус</param>

            internal static PointF eRotateAround ( this PointF ptfCenter , float AngleRad , float sngRadius )
            {
                var ptfOffset = AngleRad.eRotateAround(sngRadius);
                return new PointF(ptfCenter.X + ptfOffset.X , ptfCenter.Y + ptfOffset.Y);
            }

            /// <summary>Расчёт координат новой точки, повернутой вокруг заданной на угол (с разными X/Y радиусами - для овалов))</summary>
            /// <param name="ptfCenter">Точка, относительно оторой вращаемся</param>
            /// <param name="AngleRad">Угол в радианах</param>
            /// <param name="Radius">Радиус X,Y</param>

            internal static System.Drawing.PointF eRotateAround ( this PointF ptfCenter , float AngleRad , System.Drawing.SizeF Radius )
            {
                var ptfOffset = AngleRad.eRotateAround(Radius);
                return new PointF(ptfCenter.X + ptfOffset.X , ptfCenter.Y + ptfOffset.Y);
            }

            /// <summary>Расчёт координат новой точки, повернутой вокруг заданной на угол (с разными X/Y радиусами - для овалов))</summary>
            /// <param name="AngleRad">Угол в радианах</param>
            /// <param name="Radius">Радиус X,Y</param>

            internal static System.Drawing.PointF eRotateAround ( this float AngleRad , System.Drawing.SizeF Radius )
            {
                return new PointF((float)(Math.Cos(AngleRad) * Radius.Width) , (float)(Math.Sin(AngleRad) * Radius.Height));
            }

            /// <summary>Расчёт координат новой точки, повернутой вокруг заданной на угол</summary>
            /// <param name="AngleRad">Угол в радианах</param>
            /// <param name="sngRadius">Радиус</param>

            internal static PointF eRotateAround ( this float AngleRad , float sngRadius )
            {
                return AngleRad.eRotateAround(new SizeF(sngRadius , sngRadius));
            }



            #endregion


            /// <summary>Делит каждое значение на заданное</summary>

            internal static RectangleF eDivideTo ( this Rectangle RC , float Делитель ) => new(RC.Left / Делитель , RC.Top / Делитель , RC.Width / Делитель , RC.Height / Делитель);


            /// <summary>Делит каждое значение на заданное</summary>

            internal static RectangleF eMultipleTo ( this RectangleF RC , float Множитель ) => new(RC.Left * Множитель , RC.Top * Множитель , RC.Width * Множитель , RC.Height * Множитель);


            /// <summary>Находит центр прямоугольника</summary>

            internal static PointF eGetCenter ( this RectangleF RC ) => new(RC.Left + (RC.Width / 2) , RC.Top + (RC.Height / 2));

            internal static PointF eRound ( this PointF pt , int digits = 0 ) => new(
                (float)Math.Round(pt.X , digits) ,
                (float)Math.Round(pt.Y , digits));

            internal static Point eRoundToInt ( this PointF pt )
            {
                pt = pt.eRound(0);
                return new((int)pt.X , (int)pt.Y);
            }

            internal static Size eGetVectorTo ( this Point source , Point target )
            {
                int dx = source.X - target.X;
                int dy = source.Y - target.Y;
                return new(dx , dy);
            }

            internal static double eGetHypotenuse ( this Size vector )
            {
                var dx = Math.Pow(vector.Width , 2d);
                var dy = Math.Pow(vector.Height , 2d);
                return Math.Sqrt(dx + dy);
            }


            /// <summary>Находит центр прямоугольника</summary>

            internal static PointF eGetCenter ( this Rectangle RC ) => RC.eToRectangleF().eGetCenter();


            /// <summary>Центрирует Короткую линию по более длинной</summary>

            internal static float eCenterTo ( this float ShortLineWidth , float LongLineWidth ) => (LongLineWidth - ShortLineWidth) / 2f;


            /// <summary>Центрирует Короткую линию по более длинной</summary>

            internal static int eCenterTo ( this int ShortLineWidth , int LongLineWidth ) => (int)Math.Round((LongLineWidth - ShortLineWidth) / 2f);


            /// <summary>Центрирует прямоугольник относительно заданной точки</summary>

            internal static RectangleF eCenterTo ( this RectangleF RC , PointF ptCenter )
                => new(
                    ptCenter.X - (RC.Width / 2) ,
                    ptCenter.Y - (RC.Height / 2) ,
                    RC.Width ,
                    RC.Height);



            /// <summary>Центрирует прямоугольник относительно заданной точки</summary>

            internal static Rectangle eCenterTo ( this Rectangle RC , Point ptCenter )
                => new(
                    ptCenter.X - (RC.Width / 2) ,
                    ptCenter.Y - (RC.Height / 2) ,
                    RC.Width ,
                    RC.Height);





            internal static RectangleF eScale ( this RectangleF source , PointF zoom )
                => new(
                    source.Left ,
                    source.Top ,
                    source.Size.Width * zoom.X ,
                    source.Size.Height * zoom.Y
                    );


            internal static RectangleF eScale ( this Rectangle source , PointF zoom )
                => source.eToRectangleF().eScale(zoom);


            internal static Rectangle eScaleToInt ( this Rectangle source , PointF zoom )
                => source.eScale(zoom).eToRectangle();





            internal static Rectangle eMoveLeftEdge ( this Rectangle source , int delthaX )
            {
                var rc = source;
                source.Offset(delthaX , 0);
                source.Width -= delthaX;
                return source;
            }



            internal static Rectangle eMoveTopEdge ( this Rectangle source , int delthaY )
            {
                var rc = source;
                source.Offset(0 , delthaY);
                source.Height -= delthaY;
                return source;
            }



            internal static Rectangle eMoveLeftTopCorner ( this Rectangle source , Size deltha )
            {
                var rc = source;
                source.Offset(deltha.eToPoint());
                source.Width -= deltha.Width;
                source.Height -= deltha.Height;
                return source;
            }




            /// <summary>Центрирует прямоугольник относительно заданной точки</summary>

            internal static Rectangle eAlignTo ( this Rectangle src , Rectangle trg , ContentAlignment alignment , Point? offset = null )
            {
                Point ptLocation = src.Location;

                {
                    //Calculate Y
                    switch ( alignment )
                    {
                        case ContentAlignment.TopLeft:
                        case ContentAlignment.TopCenter:
                        case ContentAlignment.TopRight:
                            {
                                ptLocation.Y = trg.Y;
                                if ( offset.HasValue )
                                {
                                    ptLocation.Y += offset.Value.Y;
                                }

                                break;
                            }
                    }

                    switch ( alignment )
                    {
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.MiddleCenter:
                        case ContentAlignment.MiddleRight:
                            {
                                Rectangle rcCentered = src.eCenterTo(trg.eGetCenter().eToPoint());
                                ptLocation.Y = rcCentered.Y;
                                break;
                            }
                    }

                    switch ( alignment )
                    {
                        case ContentAlignment.BottomLeft:
                        case ContentAlignment.BottomCenter:
                        case ContentAlignment.BottomRight:
                            {
                                ptLocation.Y = trg.Bottom - src.Height;
                                if ( offset.HasValue )
                                {
                                    ptLocation.Y -= offset.Value.Y;
                                }

                                break;
                            }
                    }
                }

                {
                    //Calculate X

                    switch ( alignment )
                    {
                        case ContentAlignment.TopLeft:
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.BottomLeft:
                            {
                                ptLocation.X = trg.X;
                                if ( offset.HasValue )
                                {
                                    ptLocation.X += offset.Value.X;
                                }

                                break;
                            }
                    }

                    switch ( alignment )
                    {
                        case ContentAlignment.TopCenter:
                        case ContentAlignment.MiddleCenter:
                        case ContentAlignment.BottomCenter:
                            {
                                Rectangle rcCentered = src.eCenterTo(trg.eGetCenter().eToPoint());
                                ptLocation.X = rcCentered.X;
                                break;
                            }
                    }

                    switch ( alignment )
                    {
                        case ContentAlignment.TopRight:
                        case ContentAlignment.MiddleRight:
                        case ContentAlignment.BottomRight:
                            {
                                ptLocation.X = trg.Right - src.Width;
                                if ( offset.HasValue )
                                {
                                    ptLocation.X -= offset.Value.X;
                                }

                                break;
                            }
                    }
                }
                return new(ptLocation , src.Size);
            }

            /// <summary>Центрирует прямоугольник относительно заданной точки</summary>

            internal static RectangleF eAlignTo ( this RectangleF src , RectangleF trg , ContentAlignment alignment , Point? offset = null )
            {
                PointF ptLocation = src.Location;

                {
                    //Calculate Y
                    switch ( alignment )
                    {
                        case ContentAlignment.TopLeft:
                        case ContentAlignment.TopCenter:
                        case ContentAlignment.TopRight:
                            {
                                ptLocation.Y = trg.Y;
                                if ( offset.HasValue )
                                {
                                    ptLocation.Y += offset.Value.Y;
                                }

                                break;
                            }
                    }

                    switch ( alignment )
                    {
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.MiddleCenter:
                        case ContentAlignment.MiddleRight:
                            {
                                RectangleF rcCentered = src.eCenterTo(trg.eGetCenter());
                                ptLocation.Y = rcCentered.Y;
                                break;
                            }
                    }

                    switch ( alignment )
                    {
                        case ContentAlignment.BottomLeft:
                        case ContentAlignment.BottomCenter:
                        case ContentAlignment.BottomRight:
                            {
                                ptLocation.Y = trg.Bottom - src.Height;
                                if ( offset.HasValue )
                                {
                                    ptLocation.Y -= offset.Value.Y;
                                }

                                break;
                            }
                    }
                }

                {
                    //Calculate X

                    switch ( alignment )
                    {
                        case ContentAlignment.TopLeft:
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.BottomLeft:
                            {
                                ptLocation.X = trg.X;
                                if ( offset.HasValue )
                                {
                                    ptLocation.X += offset.Value.X;
                                }

                                break;
                            }
                    }

                    switch ( alignment )
                    {
                        case ContentAlignment.TopCenter:
                        case ContentAlignment.MiddleCenter:
                        case ContentAlignment.BottomCenter:
                            {
                                RectangleF rcCentered = src.eCenterTo(trg.eGetCenter());
                                ptLocation.X = rcCentered.X;
                                break;
                            }
                    }

                    switch ( alignment )
                    {
                        case ContentAlignment.TopRight:
                        case ContentAlignment.MiddleRight:
                        case ContentAlignment.BottomRight:
                            {
                                ptLocation.X = trg.Right - src.Width;
                                if ( offset.HasValue )
                                {
                                    ptLocation.X -= offset.Value.X;
                                }

                                break;
                            }
                    }
                }
                return new(ptLocation , src.Size);
            }












            /// <summary>Точка внутри?</summary>

            internal static bool eIsInRect ( this System.Drawing.Rectangle RC , System.Drawing.Point PT )
            {
                return PT.X >= RC.Left && PT.X <= RC.Right && PT.Y >= RC.Top && PT.Y <= RC.Bottom;
            }

            /// <summary>Сделать, чтобы точка не выходила за пределы</summary>

            internal static System.Drawing.RectangleF eEnsureInRect ( this System.Drawing.RectangleF src , System.Drawing.RectangleF trg )
            {
                if ( src.Left > trg.Right )
                {
                    src.X = trg.Right;
                }

                if ( src.Top > trg.Bottom )
                {
                    src.Y = trg.Bottom;
                }

                if ( src.Left < trg.Left )
                {
                    src.X = trg.Left;
                }

                if ( src.Top < trg.Top )
                {
                    src.Y = trg.Top;
                }

                if ( src.Right > trg.Right )
                {
                    src.Width = trg.Right - src.Left;
                }

                if ( src.Bottom > trg.Bottom )
                {
                    src.Height = trg.Bottom - src.Top;
                }

                return src;
            }




            /// <inheritdoc cref="eEnsureInRect"/>

            internal static System.Drawing.Rectangle eEnsureInRect ( this System.Drawing.Rectangle src , System.Drawing.Rectangle trg )
            {
                if ( src.Left > trg.Right )
                {
                    src.X = trg.Right;
                }

                if ( src.Top > trg.Bottom )
                {
                    src.Y = trg.Bottom;
                }

                if ( src.Left < trg.Left )
                {
                    src.X = trg.Left;
                }

                if ( src.Top < trg.Top )
                {
                    src.Y = trg.Top;
                }

                if ( src.Right > trg.Right )
                {
                    src.Width = trg.Right - src.Left;
                }

                if ( src.Bottom > trg.Bottom )
                {
                    src.Height = trg.Bottom - src.Top;
                }

                return src;
            }

            /// <summary>Normalizing rectangle</summary>

            internal static System.Drawing.Rectangle eNormalize ( this System.Drawing.Rectangle src )
            {
                int l = new int[] { src.Left , src.Right }.Min();
                int r = new int[] { src.Left , src.Right }.Max();
                int t = new int[] { src.Top , src.Bottom }.Min();
                int b = new int[] { src.Top , src.Bottom }.Max();
                return System.Drawing.Rectangle.FromLTRB(l , t , r , b);
            }

            /// <summary>Normalizing rectangle</summary>

            internal static System.Drawing.RectangleF eNormalize ( this System.Drawing.RectangleF src )
            {
                float l = new float[] { src.Left , src.Right }.Min();
                float r = new float[] { src.Left , src.Right }.Max();
                float t = new float[] { src.Top , src.Bottom }.Min();
                float b = new float[] { src.Top , src.Bottom }.Max();
                return RectangleF.FromLTRB(l , t , r , b);
            }





            internal static void SetBottom ( this ref RectangleF src , float newBottom )
                => src.Height = newBottom - src.Top;



            internal static void SetRight ( this ref RectangleF src , float newRight )
                => src.Width = newRight - src.Left;






            internal static string ToString_WxH ( this Point PT ) => $"{PT.X}x{PT.Y}";



            internal static string ToString_WxH ( this PointF PT ) => $"{PT.X}x{PT.Y}";



            internal static string ToString_WxH ( this PointF PT , int iRound ) => $"{PT.X.round(iRound)}x{PT.Y.round(iRound)}";



            internal static string ToString_WxH ( this Size PT ) => $"{PT.Width}x{PT.Height}";



            internal static string ToString_WxH ( this SizeF PT ) => $"{PT.Width}x{PT.Height}";



            internal static string ToString_WxH ( this SizeF PT , int iRound ) => $"{PT.Width.round(iRound)}x{PT.Height.round(iRound)}";





            #region StockIcon



            public static Icon GetStockIcon ( this Shell32.SHSTOCKICONID id , Shell32.SHGSI f )
            {

                Shell32.SHSTOCKICONINFO sii = new()
                {
                    cbSize = (uint)Marshal.SizeOf(typeof(Shell32.SHSTOCKICONINFO))
                };

                Shell32.SHGetStockIconInfo(id , f , ref sii)
                    .ThrowIfFailed();

                try
                {
                    return Icon.FromHandle(sii.hIcon.DangerousGetHandle()).CloneT();
                }
                finally
                {
                    User32.DestroyIcon(sii.hIcon.DangerousGetHandle());
                }
            }



            public static Icon GetStockIcon ( this Shell32.SHSTOCKICONID id , bool small = true )
            {
                Shell32.SHGSI iconSize = small
                    ? Shell32.SHGSI.SHGSI_SMALLICON
                    : Shell32.SHGSI.SHGSI_LARGEICON;

                return GetStockIcon(id , Shell32.SHGSI.SHGSI_ICON | iconSize);
            }



            /*
            public static BitmapSource GetShieldIcon()
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    SHSTOCKICONINFO sii = new SHSTOCKICONINFO();
                    sii.cbSize = (UInt32)Marshal.SizeOf(typeof(SHSTOCKICONINFO));

                    Marshal.ThrowExceptionForHR(SHGetStockIconInfo(SHSTOCKICONID.SIID_SHIELD,
                        SHGSI.SHGSI_ICON | SHGSI.SHGSI_SMALLICON,
                        ref sii));
                    try
                    {
                        return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                            sii.hIcon,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                    }
                    finally
                    {
                        DestroyIcon(sii.hIcon);
                    }
                }
                else
                {
                    return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        System.Drawing.SystemIcons.Shield.Handle,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
             */


            #endregion









        }




        internal static class Extensions_Imaging
        {






            internal static Bitmap eTransformColor ( this Bitmap original , ColorMatrix cm )
            {
                // create a blank bitmap the same size as original
                Bitmap newBitmap = new(original.Width , original.Height , PixelFormat.Format32bppArgb);

                // create some image attributes
                using ImageAttributes attributes = new();
                // set the color matrix attribute
                attributes.SetColorMatrix(cm);

                // get a graphics object from the new image
                using Graphics g = Graphics.FromImage(newBitmap);
                // draw the original image on the new image using the grayscale color matrix
                g.DrawImage(
                     original ,
                     original.Size.eToRectangle() ,
                     0 , 0 , original.Width , original.Height ,
                     GraphicsUnit.Pixel ,
                     attributes);

                return newBitmap;
            }




            internal static Bitmap eMakeTransparent ( this Bitmap original , float alpha )
            {
                ColorMatrix cm = new(
                       [
                                   [1.0f, 0.0f, 0.0f, 0.0f, 0.0f],
                                   [0.0f, 1.0f, 0.0f, 0.0f, 0.0f],
                                   [0.0f, 0.0f, 1.0f, 0.0f, 0.0f],
                                   [0.0f, 0.0f, 0.0f, alpha, 0.0f],
                                   [0.0f, 0.0f, 0.0f, 0.0f, 1.0f],
                                   ]
                                   );

                return original.eTransformColor(cm);

            }




            internal static Bitmap eToGrayscaled_Matrix ( this Bitmap original )
            {
                // create the grayscale ColorMatrix
                ColorMatrix cm = new(
                       [
                                   [0.30f, 0.30f, 0.30f, 0.00f, 0.00f],
                                   [0.59f, 0.59f, 0.59f, 0.00f, 0.00f],
                                   [0.11f, 0.11f, 0.11f, 0.00f, 0.00f],
                                   [0.00f, 0.00f, 0.00f, 1.00f, 0.00f],
                                   [0.00f, 0.00f, 0.00f, 0.00f, 1.00f]
                                   ]
                                   );

                return original.eTransformColor(cm);
            }




            internal static Image eToGrayscaled_ToolStripRenderer ( this Bitmap src ) => ToolStripRenderer.CreateDisabledImage(src);



            /// <summary>Saves the specified <see cref="Bitmap"/> objects as a single icon into the output stream.</summary>
            /// <param name="iconFrames">The bitmaps to save as an icon frames.<br/>
            /// The expected input image size is less than or equal to 256 and the height is less than or equal to 256</param>
            internal static Stream eSaveAsMultisizedIconStream ( this IEnumerable<Bitmap> iconFrames )
            {
                const int MAX_ICON_SIZE = 256;
                const ushort ICON_HEADER_RESERVED = 0;
                const ushort ICON_HEADER_ICON_TYPE = 1;
                const byte HEADER_LENGTH = 6;
                const byte ENTRY_RESERVED = 0;
                const byte ENTRY_LENGTH = 16;
                const byte PNG_COLORS_IN_PALETTE = 0;
                const ushort PNG_COLOR_PLANES = 1;


                _ = iconFrames ?? throw new ArgumentNullException(nameof(iconFrames));


                Bitmap[] orderedImages = [..
                    iconFrames
                    .OrderBy(i => i.Width)
                    .ThenBy(i => i.Height)
                    ];

                MemoryStream msOutput = new();

                using ( BinaryWriter bw = new(msOutput , Encoding.ASCII , true) )
                {
                    // write the header
                    bw.Write(ICON_HEADER_RESERVED);
                    bw.Write(ICON_HEADER_ICON_TYPE);
                    bw.Write((ushort)orderedImages.Length);

                    // save the image buffers and offsets
                    Dictionary<uint , byte[]> buffers = [];

                    // tracks the length of the buffers as the iterations occur
                    // and adds that to the offset of the entries
                    uint lengthSum = 0;
                    uint baseOffset = (uint)(HEADER_LENGTH +
                                             (ENTRY_LENGTH * orderedImages.Length));

                    for ( uint i = 0 ; i < orderedImages.Length ; i++ )
                    {
                        Bitmap image = orderedImages[ i ];

                        if ( image.PixelFormat != PixelFormat.Format32bppArgb )
                        {
                            throw new InvalidOperationException($"Required pixel format is PixelFormat.{PixelFormat.Format32bppArgb}.");
                        }

                        if ( image.Width > MAX_ICON_SIZE || image.Height > MAX_ICON_SIZE )
                        {
                            throw new InvalidOperationException($"Dimensions must be less than or equal to {MAX_ICON_SIZE}x{MAX_ICON_SIZE}");
                        }

                        if ( image.RawFormat.Guid != ImageFormat.Png.Guid )
                        {
                            //Converting image to png
                            using MemoryStream msPngTemp = new();
                            image.Save(msPngTemp , ImageFormat.Png);
                            msPngTemp.Seek(0 , SeekOrigin.Begin);
                            image = (Bitmap)Bitmap.FromStream(msPngTemp);
                        }

                        using var msBuffer = new MemoryStream();
                        image.Save(msBuffer , image.RawFormat);
                        // creates a byte array from an image
                        byte[] buffer = msBuffer.ToArray();

                        // calculates what the offset of this image will be
                        // in the stream
                        uint offset = baseOffset + lengthSum;

                        byte iconHeight = (image.Height == MAX_ICON_SIZE)
                            ? (byte)0
                            : (byte)image.Height;

                        byte iconWidth = (image.Width == MAX_ICON_SIZE)
                            ? (byte)0
                            : (byte)image.Width;

                        // writes the image entry
                        bw.Write(iconWidth);
                        bw.Write(iconHeight);
                        bw.Write(PNG_COLORS_IN_PALETTE);
                        bw.Write(ENTRY_RESERVED);
                        bw.Write(PNG_COLOR_PLANES);
                        bw.Write((ushort)Image.GetPixelFormatSize(image.PixelFormat));
                        bw.Write((uint)buffer.Length);
                        bw.Write(offset);

                        lengthSum += (uint)buffer.Length;

                        // adds the buffer to be written at the offset
                        buffers.Add(offset , buffer);
                    }

                    // writes the buffers for each image
                    foreach ( var kvp in buffers )
                    {

                        // seeks to the specified offset required for the image buffer
                        bw.BaseStream.Seek(kvp.Key , SeekOrigin.Begin);

                        // writes the buffer
                        bw.Write(kvp.Value);
                    }
                }

                msOutput.Seek(0 , SeekOrigin.Begin);
                return msOutput;
            }








            /// <summary>Клонирует объект системным методом CLONE, возвращая объект такого-же типа, что и исходный</summary>

            public static Icon eCloneViaGDICopyIcon ( this Icon rSource )
            {
                throw new NotImplementedException();

                //var hIconNew = UOM.Win32.GDI.GDIObjects.Icon.CopyIcon(rSource.Handle);
                //if (hIconNew.IsInValid)
                //{
                //    Win32.Errors.ThrowLastWin23ErrorAssert(Win32.Errors.Win32Errors.ERROR_SUCCESS);
                //    throw new Win32.Errors.Win32Exception("CopyIcon Failed with unknown error!");
                //}

                //var rNewicon = System.Drawing.Icon.FromHandle(hIconNew);
                //if (rNewicon null == )
                //throw new Exception("System.Drawing.Icon.FromHandle Failed!");
                //return rNewicon;
            }


            /// <summary>Клонирует Image через GDI+.DrawImage</summary>

            public static Bitmap eCloneViaDrawImage ( this System.Drawing.Image imgSrc )
            {
                Bitmap bmNew = new(imgSrc.Width , imgSrc.Height);
                using Graphics g = Graphics.FromImage(bmNew);
                g.DrawImage(imgSrc , 0 , 0 , imgSrc.Width , imgSrc.Height);
                return bmNew;
            }


            /// <summary>Клонирует объект системным методом CLONE, возвращая объект такого-же типа, что и исходный</summary>

            public static Icon eCloneViaMemStream ( this Icon rSource )
            {
                using MemoryStream ms = new();
                rSource.Save(ms);
                ms.Seek(0L , SeekOrigin.Begin);
                return new Icon(ms);
            }


            /// <summary>Клонирует объект системным методом CLONE, возвращая объект такого-же типа, что и исходный</summary>

            public static Bitmap eCloneViaMemStream ( this Bitmap rSource )
            {
                using MemoryStream ms = new();
                rSource.Save(ms , System.Drawing.Imaging.ImageFormat.MemoryBmp);
                ms.Seek(0L , SeekOrigin.Begin);
                return new Bitmap(ms);
            }








        }





        internal static partial class Extensions_UI_Localization
        {



            #region Menus


            internal static void eLocalizeUITree ( this MenuStrip mnuRoot )
            {
                ToolStripItem[] actl = [ .. mnuRoot.Items.OfType<ToolStripItem>() ];
                actl.eLocalizeUI(true);
            }


            #endregion



            internal static void eLocalizeUI ( this Component c , bool recurse = true )
            {

                switch ( c )
                {
                    case ColumnHeader col:
                        {
                            var s = col.eGetLocalizedTextByPropertyName();
                            if ( s != null )
                            {
                                col.Text = s;
                            }
                            else
                            {
                                s = col.Text.eGetLocalizedText();
                                if ( s != null )
                                {
                                    col.Text = s;
                                }
                            }
                            break;
                        }

                    case MenuStrip mnu:
                        {
                            mnu.Items.OfType<ToolStripItem>().eLocalizeUI(recurse);
                            break;
                        }

                    case ToolStripItem tsi:
                        {
                            switch ( tsi )
                            {
                                case ToolStripSeparator sep: break;

                                case ToolStripButton:
                                case ToolStripDropDownButton:
                                case ToolStripLabel:
                                case ToolStripMenuItem:
                                    {
                                        var s = tsi.Name.eGetLocalizedText();
                                        if ( s != null )
                                        {
                                            tsi.Text = s;
                                        }
                                        else
                                        {
                                            s = tsi.Text.eGetLocalizedText();
                                            if ( s != null )
                                            {
                                                tsi.Text = s;
                                            }
                                        }

                                        if ( recurse && tsi is ToolStripMenuItem mnu )
                                        {
                                            //ToolStripItem[] actl = [.. mnu.DropDownItems.OfType<ToolStripItem>()];
                                            mnu.DropDownItems.OfType<ToolStripItem>().eLocalizeUI(recurse);
                                        }
                                        break;
                                    }

                            }
                            break;
                        }

                    case Control ctl:
                        {
                            var s = ctl.eGetLocalizedTextByPropertyName();
                            if ( s != null )
                            {
                                ctl.Text = s;
                            }
                            else
                            {
                                s = ctl.Text.eGetLocalizedText();
                                if ( s != null )
                                {
                                    ctl.Text = s;
                                }
                            }
                            break;
                        }

                    default:
                        throw new NotImplementedException($"Localization of Component '{c.GetType()}' is not supported!");
                }
            }




            internal static void eLocalizeUI ( this IEnumerable<Component> cmpList , bool recurse = true )
                => cmpList.forEach(ctl => ctl.eLocalizeUI(recurse));


        }


        internal static partial class Extensions_DebugAndErrors
        {

            #region TryCatchWin






#if WINDOWS && !UWP


            /// <summary>Вызывает Callback внутри Try/Catch и при ошибке автоматом вызывает ex.eLogError(ShowModalMessageBox)</summary>

            internal static void tryCatchUI ( this Action a ,
                        bool errorUI = true ,
                        string uiTitle = C_FAILED_TO_RUN ,
                        MessageBoxIcon icon = MessageBoxIcon.Error ,
                        MessageBoxButtons btn = MessageBoxButtons.OK ,
                        bool debugErrorUI = false )
                => a.tryCatch(e => e.eLogError(errorUI , uiTitle , icon , btn , debugErrorUI));


            /*
            /// <summary>Вызывает Callback внутри Try/Catch и при ошибке автоматом вызывает ex.eLogError(ShowModalMessageBox)</summary>

            internal static void tryCatchUI ( this System.Windows.Forms.MethodInvoker mi,
                        bool errorUI = true,
                        string uiTitle = C_FAILED_TO_RUN,
                        MessageBoxIcon icon = MessageBoxIcon.Error,
                        MessageBoxButtons btn = MessageBoxButtons.OK,
                        bool debugErrorUI = false )
                => mi.tryCatch (e => e.eLogError (errorUI, uiTitle, icon, btn, debugErrorUI));
             */



            #region tryCatchUIAsync


            /// <inheritdoc cref="tryCatch" />
            internal static async Task<(bool Result, ExceptionEventArgs? Error)> tryCatchUIAsync (
                this Task tsk ,
                bool errorUI = true ,
                string uiTitle = C_FAILED_TO_RUN ,
                MessageBoxIcon icon = MessageBoxIcon.Error ,
                MessageBoxButtons btn = MessageBoxButtons.OK ,
                bool debugErrorUI = false )
            {
                ExceptionEventArgs? ex2 = null;
                var r = await tsk.tryCatchAsync(ex =>
                {
                    ex2 = ex;
                    ex.eLogError(errorUI , uiTitle , icon , btn , debugErrorUI);
                }
                );
                return (r, ex2);
            }



            /// <inheritdoc cref="tryCatch" />

            internal static async Task<Result<T>> tryCatchUIAsync<T> (
                this Task<T> tsk ,
                T? defaultValue = default ,
                bool errorUI = true ,
                string uiTitle = C_FAILED_TO_RUN ,
                MessageBoxIcon icon = MessageBoxIcon.Error ,
                MessageBoxButtons btn = MessageBoxButtons.OK ,
                bool debugErrorUI = false )
            {
                var r = await tsk.tryCatchAsync(defaultValue);
                if ( !r.IsSuccess && !r.IsCanceled )
                {
                    r.Error?.Exception?.eLogError(errorUI , uiTitle , icon , btn , debugErrorUI);
                }
                return r;

            }


            //#if ( !ANDROID && !UWP )



            /// <inheritdoc cref="tryCatch" />

            internal static async Task<Result<T>> tryCatchUIAsync<T> (
                this Func<T> func ,
                //T? defaultValue = default ,
                CancellationToken? cancel = null ,
                bool errorUI = true ,
                string uiTitle = C_FAILED_TO_RUN ,
                MessageBoxIcon icon = MessageBoxIcon.Error ,
                MessageBoxButtons btn = MessageBoxButtons.OK ,
                bool debugErrorUI = false )
            {

                var r = await func.tryCatchAsync(cancel);
                if ( r.IsSuccess ) return r;

                if ( !r.IsCanceled )
                {
                    r.Error?.Exception?.eLogError(errorUI , uiTitle , icon , btn , debugErrorUI);
                }
                return r;
                //, e => e.eLogError( errorUI , uiTitle , icon , btn , debugErrorUI)
            }


            #endregion

#endif

            #endregion



            #region eLogError


            /// <summary>Фиксация ошибки в журнале, в DEBUG, вывод сообщения</summary>

            internal static void eLogError ( this Exception ex ,
                bool errorUI ,
                string uiTitle = C_FAILED_TO_RUN ,
                MessageBoxIcon icon = MessageBoxIcon.Error ,
                MessageBoxButtons btn = MessageBoxButtons.OK ,
                bool debugErrorUI = false ,
                [CallerMemberName] string callerName = "" , [CallerFilePath] string callerFile = "" , [CallerLineNumber] int callerLine = 0
                )
            {
                try
                {
                    string errorDump = ex.eFullDump(callerName , callerFile , callerLine);

                    ushort eventID = 1001;
                    try
                    {
                        if ( ex is Win32Exception wex )
                        {
                            eventID = (ushort)wex.ErrorCode.checkRange(ushort.MinValue , ushort.MaxValue);
                        }
                    }
                    catch { }

                    WinAPI.errors.ErrorLogWrite(errorDump , eventID: eventID);
                    string msg = ex.Message;
#if DEBUG
                    $"{CS_CONSOLE_SEPARATOR}\n{errorDump}\n{CS_CONSOLE_SEPARATOR}".eDebugWriteLine();

                    //Показываем расширенные данные в DEBUG режиме
                    msg += $"\n{CS_CONSOLE_SEPARATOR}\nUOM DEBUG-MODE DETAILED ERROR INFO:\n{errorDump}";
#endif

                    if ( errorUI ) // Надо показать на экране модальное Сообщение об ошибке
                    {
                        System.Windows.Forms.MessageBox.Show(msg , uiTitle , btn , icon);
                    }
                    else
                    {
#if DEBUG
                        if ( debugErrorUI )
                        {
                            //В DEBUG режиме показываем модальное окно с ошибкой, если прямо не запрещено!
                            System.Windows.Forms.MessageBox.Show(msg , uiTitle , btn , icon);
                        }
#endif
                    }
                }
                catch ( Exception ex2 )
                {
                    if ( errorUI )
                    {
                        System.Windows.Forms.MessageBox.Show(ex2.Message , "Error when journaling previous error!" , MessageBoxButtons.OK , MessageBoxIcon.Error);
                    }
                }
            }


            /// <summary>Фиксация ошибки в журнале, в DEBUG, вывод сообщения</summary>

            internal static void eLogError ( this ExceptionEventArgs ex ,
                bool errorUI ,
                string uiTitle = C_FAILED_TO_RUN ,
                MessageBoxIcon icon = MessageBoxIcon.Error ,
                MessageBoxButtons btn = MessageBoxButtons.OK ,
                bool debugErrorUI = false )
                => ex.Exception.eLogError(errorUI , uiTitle , icon , btn , debugErrorUI , ex.CallerMemberName , ex.CallerFilePath , ex.CallerLineNumber);





#if NETFRAMEWORK


			private partial class FORM_ATTACHMENT
			{
				private static readonly Size C_DEFAULT_ERROR_FORM_MINIMUMSIZE = new(300, 200);
				private static readonly Size C_DEFAULT_ERROR_FORM_SIZE = new(
					(int)Math.Round(C_DEFAULT_ERROR_FORM_MINIMUMSIZE.Width * 2d),
					(int)Math.Round(C_DEFAULT_ERROR_FORM_MINIMUMSIZE.Height * 2d));

				public readonly Form? parentForm = null;
				private volatile Form? __frmError = null;

				private Form? _frmError
				{
					[MethodImpl(MethodImplOptions.Synchronized)]
					get => __frmError;

					[MethodImpl(MethodImplOptions.Synchronized)]
					set
					{
						if (__frmError != null) __frmError.FormClosing -= OnErrorFormClosing!;//Unsubscribe from form events

						__frmError = value;
						if (__frmError != null) __frmError.FormClosing += OnErrorFormClosing!;
					}
				}

				private volatile bool _isFomErrorClosing = false;
				private readonly TextBox _ctlTextBox;
				private int _iTotalErrorsCount = 0;

				//private readonly EventArgs _ErrorsQueueSyncObject = new EventArgs();
				private readonly Queue<Exception> _qErrorsToShow = new(); // Очередь сообщений об ошибках для показа

				public FORM_ATTACHMENT (Form PF) : base()
				{
					parentForm = PF;
					_frmError = new Form();
					{
						var withBlock = _frmError;
						withBlock.Text = "Ошибки";
						withBlock.Icon = SystemIcons.Error;
						withBlock.Owner = PF;
						withBlock.FormBorderStyle = FormBorderStyle.SizableToolWindow;
						withBlock.StartPosition = FormStartPosition.CenterParent;
						withBlock.ShowInTaskbar = false;
						withBlock.ShowIcon = true;
						withBlock.SizeGripStyle = SizeGripStyle.Show;
						withBlock.ControlBox = true;
						withBlock.Padding = new Padding(8);
						withBlock.MinimumSize = C_DEFAULT_ERROR_FORM_MINIMUMSIZE;
						var szParent = parentForm.Size;
						int iErrorWindowsWidth = (int)(0.7F * (float)szParent.Width);
						var szError = new Size(iErrorWindowsWidth, C_DEFAULT_ERROR_FORM_SIZE.Height);
						withBlock.Size = szError;
						_ctlTextBox = new TextBox()
						{
							Multiline = true,
							ReadOnly = true,
							ScrollBars = ScrollBars.Both,
							Dock = DockStyle.Fill,
							Text = ""
						};

						withBlock.Controls.Add(_ctlTextBox);
					}

					UpdateFormTitle();
					_isFomErrorClosing = false;
					StartWatchTimer();
				}

				private void UpdateFormTitle () => _frmError!.Text = "Ошибки: {0}".format((object)_iTotalErrorsCount);

				//private FormClosingEventHandler _OnErrorFormClosing2233 = new(OnErrorFormClosing);

				/// <summary>При попытке зукрыть окно не закрываем его, а просто скрываем</summary>
				private void OnErrorFormClosing (object sender, System.Windows.Forms.FormClosingEventArgs e)
				{
					if (CloseReason.UserClosing == e.CloseReason)
					{
						e.Cancel = true;
						_frmError?.Hide();
						return;
					}

					_isFomErrorClosing = true; // Ставим флаг, что форма начала закрываться, и больше ничего показывать нельзя.
				}

				private const int C_WATCH_TIMER = 2000;
				private System.Windows.Forms.Timer? __tmrWatchErrors = null;

				//private System.Windows.Forms.Timer _tmrWatchErrors
				//{
				//    [MethodImpl(MethodImplOptions.Synchronized)]
				//    get => __tmrWatchErrors;

				//    [MethodImpl(MethodImplOptions.Synchronized)]
				//    set
				//    {
				//        if (__tmrWatchErrors != null) __tmrWatchErrors.Tick -= (_, __) => WatchNewErrorrs();

				//        __tmrWatchErrors = value;
				//        if (__tmrWatchErrors != null) __tmrWatchErrors.Tick += (_, __) => WatchNewErrorrs();
				//    }
				//}

				private void StartWatchTimer ()
				{
					__tmrWatchErrors = new System.Windows.Forms.Timer()
					{
						Interval = C_WATCH_TIMER,
						Enabled = false
					};
					__tmrWatchErrors.Tick += (s, e) => WatchNewErrorrs();
					__tmrWatchErrors.Start();
				}


				// Работает в потоке формы _frmError, смотрит очередь на наличие новых ошибок
				private async void WatchNewErrorrs ()
				{
					__tmrWatchErrors?.Stop(); // Останавливаем таймер, чтобы из-за длительного выполнения не накладывались обработчики таймера
					if (_isFomErrorClosing == true) return; // форма начала закрываться, и больше ничего показывать нельзя. Таймер перезапускать не обязательно.

					try
					{
						if (null != _frmError && !_frmError.Handle.isValid) return; // Негде показать. Таймер перезапускать не обязательно.
					}
					catch { }

					try
					{
						// Формируем простыню с ошибками неблокируя UI
						int iErrorsFound = 0;
						var PrepareAllErrorsInfoCallBack = new Func<string?>(() =>
						 {
							 var aErrorsToShow = Array.Empty<Exception>();
							 lock (_qErrorsToShow) // Занимаем очередь, начинаем ждать освобождения занятой очереди
							 {
								 {
									 //var withBlock = _qErrorsToShow;
									 if (!_qErrorsToShow.Any()) return null;  // Очередь пуста - выходим

									 // В списке на показ есть ошибки, забираем их и показываем.
									 aErrorsToShow = [ .. _qErrorsToShow ];
									 _qErrorsToShow.Clear();
								 }
							 } // освобождаем очередь

							 iErrorsFound = aErrorsToShow.Count();
							 if (iErrorsFound < 1) return null;

							 var sbAllErrors = new StringBuilder();
							 foreach (var EX in aErrorsToShow)
							 {
								 try
								 {
#if DEBUG
									 var sMSG = EX.eFullDump() + constants.vbCrLf;
#else
									 var sMSG = EX.Message + constants.vbCrLf;
#endif
									 sbAllErrors.Append(sMSG);
								 }
								 catch { }
							 }
							 return sbAllErrors.ToString();
						 });

						string? sAllErrors = await Task.Run(PrepareAllErrorsInfoCallBack);
						if (sAllErrors.isNullOrWhiteSpace) return;

						// Показываем все ошибки, полученные из очереди.
						_iTotalErrorsCount += iErrorsFound;
						{
							// Показываем ошибки в TextBox
							try { _ctlTextBox!.AppendText(sAllErrors); }
							catch
							{
								// Может быть косяк, если текста ООЧЕНЬ много
								try
								{
									// Пытаемся очистить и снова добавить
									_ctlTextBox!.Clear();
									_ctlTextBox!.AppendText(sAllErrors);
								}
								catch { }
							}

							UpdateFormTitle();

							// Если форма скрыта - показываем её
							if (!_frmError!.Visible)
							{
								var szChild = _frmError.Size;
								var szParent = parentForm!.Size;
								int X = (szParent.Width - szChild.Width) / 2;
								int Y = (szParent.Height - szChild.Height) / 2;
								var ptLocation = parentForm.Location;
								ptLocation.X += X;
								ptLocation.Y += Y;
								_frmError.Location = ptLocation;
								_frmError.Show(parentForm);
							}
						}
					}
					catch { }
					finally { __tmrWatchErrors?.Start(); }// Перезапускаем таймер
				}

				/// <summary>Выполняется в чужом потоке, помещает ошибку в очередь на показ</summary>
				public void AddErrorToQueue (Exception EX)
				{
					try { EX.eLogError(false, debugErrorUI: false); }// Пишем ошибку в журнал
					catch { }
					lock (_qErrorsToShow) _qErrorsToShow.Enqueue(EX);
				}




				// Public Sub ShowError(EX As Exception)
				// Try
				// Call EX.eLogError(False,,, True) 'Пишем ошибку в журнал

				// If (_frmError Is Nothing) OrElse (Not _frmError.Handle.IsValid) Then Return 'Негде показать
				// Catch : End Try

				// _iTotalErrorsCount += 1

				// With _frmError
				// If (Not .Visible) Then
				// .Location = New Point(0, 0)
				// Dim szChild = .Size
				// Dim szParent = ParentForm.Size

				// Dim X = CInt((szParent.Width - szChild.Width) / 2)
				// Dim Y = CInt((szParent.Height - szChild.Height) / 2)
				// Dim ptLocation = ParentForm.Location

				// ptLocation.X += X
				// ptLocation.Y += Y

				// .Location = ptLocation

				// Call .Show(ParentForm)
				// End If

				// Dim sMSG = EX.Message & vbCrLf
				// #if DEBUG
				// sMSG = EX.eLogError_FullErrorDump & vbCrLf
				// #endif
				// Call _ctlTextBox.AppendText(sMSG)

				// Call UpdateFormTitle()

				// Call .Update()
				// End With
				// End Sub



			}

			/// <summary>Внутренний список родительских форм и сопоставленных ERROR-окон</summary>
			private static readonly List<FORM_ATTACHMENT> _lListOfAttachments = [];

			//<summary>Выполняется в потоке UI формы frmErrorWindowParent</summary>
			// Private Sub ShowError_CORE(EX As Exception, frmErrorWindowParent As Form)
			// Dim FA As FORM_ATTACHMENT = Nothing

			// Ищем среди ранее зарегистрированных для формы окон 
			// SyncLock _lListOfAttachments
			// Dim aFound = (From A In _lListOfAttachments Where A.ParentForm Is frmErrorWindowParent).ToArray
			// If (Not aFound.Any) Then
			// Для этой родительской формы ещё не создавалось окна сообщений об ошибках!
			// FA = New FORM_ATTACHMENT(frmErrorWindowParent) 'Создаём новое
			// Call _lListOfAttachments.Add(FA)
			// Else
			// FA = aFound.First
			// End If
			// End SyncLock

			// Call FA.ShowError(EX)
			// End Sub
			// <DebuggerNonUserCode, DebuggerStepThrough>  Friend Sub eLogError_NONMODAL(ByVal EX As System.Exception, frmErrorWindowParent As Form)
			// Try
			// Call frmErrorWindowParent.runInUIThread(Sub() ShowError_CORE(EX, frmErrorWindowParent))

			// Catch ex2 As Exception
			// Неудалось показать ошибку в окне!
			// End Try
			// End Sub


			/// <summary>Выполняется в вызывающем потоке</summary>
			
			internal static void eLogError_NONMODAL (this Exception EX, Form frmErrorWindowParent)
			{
				try
				{
					FORM_ATTACHMENT? FA = null;

					// Ищем среди ранее зарегистрированных для формы окон 
					lock (_lListOfAttachments)
					{
						var aFound = (from A in _lListOfAttachments
									  where object.ReferenceEquals(A.parentForm, frmErrorWindowParent)
									  select A).ToArray();
						if (!aFound.Any())
						{
							// Для этой родительской формы ещё не создавалось окна сообщений об ошибках!
							void CreateNewAttachment (Form FP)
							{
								FA = new FORM_ATTACHMENT(FP); // Создаём новое
								_lListOfAttachments.Add(FA);
							}
							;

							// Временно переходим в поток этой формы
							frmErrorWindowParent.runInUIThread(() => CreateNewAttachment(frmErrorWindowParent));
						}
						else
						{
							FA = aFound.First();
						}
					}

					// Добавляем сообщение об ошибке в очередь на показ.
					FA?.AddErrorToQueue(EX);
				}
				catch { }// Неудалось показать ошибку в окне!
			}
#endif



            #endregion


        }



    }



}


#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning restore IDE1006 // Naming Styles


#endregion