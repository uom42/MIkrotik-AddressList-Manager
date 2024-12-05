#nullable enable

using System.Drawing;
using System.Windows.Forms;

using uom.AutoDisposable;
using uom.ComboboxItems;

using Con = uom.Extensions.Extensions_Console;


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
		public static bool isInDesignerMode(Control? ctl = null)
		{
			if (IsInDesignerMode_WPF)
			{
				return true;
			}

			while (ctl != null)
			{
				if (null != ctl.Site && ctl.Site.DesignMode)
				{
					return true;
				}

				ctl = ctl.Parent;
			}
			return IsInDesignerMode_WinForms;
		}


		internal static string ConsoleAppHeader()
		{
			var sb = new StringBuilder();
			sb.AppendLine(Con.CreateHSplitter());
			sb.AppendLine($"{System.Windows.Forms.Application.ProductName} v{System.Windows.Forms.Application.ProductVersion}");
			sb.AppendLine();
			sb.AppendLine(Description);
			sb.Append(Con.CreateHSplitter());
			return sb.ToString();
		}





#if !ANDROID

		/// <param name="fileNameEndsWith">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Image LoadResourceFileAsImage(string fileNameEndsWith)
		{
			using Stream stream = GetResourceStreamForFile(fileNameEndsWith);
			return Image.FromStream(stream);
		}

		/// <param name="fileNameEndsWith">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<Image> LoadResourceFileAsImageAsync(string fileNameEndsWith)
		{
			using Stream stream = GetResourceStreamForFile(fileNameEndsWith);
			return await Task.Factory.StartNew(() => Image.FromStream(stream));
		}

#endif



	}


	internal static partial class AppTools
	{

		public static string FormatFormTitle(string title) => $"{title} ({uom.AppInfo.AssemblyFileVersionAttribute})" + (uom.AppInfo.IsProcessElevated() ? " [Admin]" : string.Empty);


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
		public static TimeSpan GetSystemUpTime_FromSystemCounters()
		{
			using var rUpTime = new PerformanceCounter(
				"System",
				"System Up Time");
			rUpTime.NextValue(); // Call this an extra time before reading its value
			return TimeSpan.FromSeconds(rUpTime.NextValue());

			// Windows 10: Incorrect Uptime Reported by Task Manager and WMI
			// Using OS As New ROOT.CIMV2.OperatingSystem
			// Dim dtBoot = OS.LastBootUpTime
			// Return dtBoot
			// End Using
		}

		public static async Task<TimeSpan> GetSystemUpTime_FromSystemCountersAsync()
			=> await Task.Factory.StartNew(() => GetSystemUpTime_FromSystemCounters(), TaskCreationOptions.LongRunning);


		#endregion


		internal static DateTime GetOSBootDate_FromSystemCounters()
			=> DateTime.Now.Subtract(GetSystemUpTime_FromSystemCounters());



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Bitmap[] GetScreenshots()
			=> System.Windows.Forms.Screen.AllScreens
			.Select(scr =>
			{
				//capture our Current Screen
				Bitmap bmCapt = new(scr.Bounds.Width, scr.Bounds.Height, PixelFormat.Format32bppArgb);
				var rcCapt = scr.Bounds;
				using (Graphics g = Graphics.FromImage(bmCapt))
				{
					g.CopyFromScreen(rcCapt.Left, rcCapt.Top, 0, 0, rcCapt.Size);
				}

				return bmCapt;
			}).ToArray();


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.IO.FileInfo[] GetScreenshotsAsFiles(ImageFormat fmt, string fileExt = "jpg")
			=> System.Windows.Forms.Screen.AllScreens
			.Select(scr =>
			{
				using Bitmap bmCapt = new(scr.Bounds.Width, scr.Bounds.Height, PixelFormat.Format32bppArgb);
				var rcCapt = scr.Bounds;
				using (Graphics g = Graphics.FromImage(bmCapt))
				{
					g.CopyFromScreen(rcCapt.Left, rcCapt.Top, 0, 0, rcCapt.Size);
				}

				var sBitmapFile = System.IO.Path.Combine(
					System.IO.Path.GetTempPath(), (Guid.NewGuid().ToString() + '.'.ToString() + fileExt));
				bmCapt.Save(sBitmapFile, fmt);
				return new System.IO.FileInfo(sBitmapFile);
			}).ToArray();







		internal static partial class Shell
		{

			#region RegisterAutorun
			internal enum APP_STARTUP_MODES
			{
				Registry,
				AutoStartFolder
			}


			public static void RegisterAutorun(APP_STARTUP_MODES Mode, bool forAllUsers, bool unregister = false)
			{
				switch (Mode)
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
								.OpenSubKey(KEY_RUN_PATH, true);

							if (keyAutoRun == null)
							{
								throw new Exception($"Failed to open '{KEY_RUN_PATH}' key!");
							}

							if (unregister)
							{
								keyAutoRun
									.GetValueNames()?
									.Where(s => s.ToLower() == fiEXE.Name.ToLower())?
									.eForEach(foundFile => keyAutoRun.DeleteValue(foundFile));
							}
							else
							{
								keyAutoRun
									.SetValue(fiEXE.Name, fiEXE.FullName.eEnclose());
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
			/// <param name="RegistryActionName">Внутреннее имя ключа операции в реестре, например 'Open'</param>
			/// <param name="ActionDisplayName">То, что видно в контекстном меню проводника</param>
			/// <param name="ExecutablePath">Можно не указывать, если эта же программа</param>
			/// <param name="CmdLineArgument"></param>
			/// <remarks></remarks>
			internal static string ContextMenu_RegisterForClass(
				string HCCRClass,
				string RegistryActionName,
				string ActionDisplayName,
				string? executablePath = null,
				string? cmdLineArgsPrefix = "",
				string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG)
			{
				if (HCCRClass.eIsNullOrWhiteSpace())
				{
					throw new ArgumentNullException(nameof(HCCRClass));
				}

				if (RegistryActionName.eIsNullOrWhiteSpace())
				{
					throw new ArgumentNullException(nameof(RegistryActionName));
				}

				if (ActionDisplayName.eIsNullOrWhiteSpace())
				{
					throw new ArgumentNullException(nameof(ActionDisplayName));
				}

				executablePath ??= System.Windows.Forms.Application.ExecutablePath;

				string sKey = string.Join(@"\", new[] { HCCRClass, CS_REG_KEY_SHELL, RegistryActionName });
				using RegistryKey keyClass = Registry.ClassesRoot.CreateSubKey(sKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
				keyClass.SetValue("", ActionDisplayName);
				keyClass.Flush();

				using RegistryKey hkCommand = keyClass.CreateSubKey(CS_REG_KEY_COMMAND, RegistryKeyPermissionCheck.ReadWriteSubTree);
				string commandString = ContextMenu_CreateRegCommandString(executablePath, cmdLineArgsPrefix, cmdLineArgs);
				hkCommand.SetValue("", commandString);
				hkCommand.Flush();
				return commandString;
			}

			private static string ContextMenu_CreateRegCommandString(
			   string executablePath,
			   string? cmdLineArgsPrefix = "",
			   string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG)
				=> ($"\"{executablePath.Trim()}\" {(cmdLineArgsPrefix ?? "").Trim()} {cmdLineArgs.Trim()}").Trim();


			internal static bool ContextMenu_IsRegisteredForClass(
				string HCCRClass,
				string RegistryActionName,
				string ActionDisplayName,
				string? executablePath = null,
				string? cmdLineArgsPrefix = "",
				string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG)
			{
				if (HCCRClass.eIsNullOrWhiteSpace())
				{
					throw new ArgumentNullException(nameof(HCCRClass));
				}

				if (RegistryActionName.eIsNullOrWhiteSpace())
				{
					throw new ArgumentNullException(nameof(RegistryActionName));
				}

				if (ActionDisplayName.eIsNullOrWhiteSpace())
				{
					throw new ArgumentNullException(nameof(ActionDisplayName));
				}

				executablePath ??= System.Windows.Forms.Application.ExecutablePath;

				string sKey = string.Join(@"\", new[] { HCCRClass, CS_REG_KEY_SHELL, RegistryActionName });
				using RegistryKey? keyClass = Registry.ClassesRoot.OpenSubKey(sKey, false);
				string? defVlue = keyClass?.eGetValue_StringOrEmpty("");
				if (defVlue != ActionDisplayName)
				{
					return false;
				}

				using RegistryKey? hkCommand = keyClass?.OpenSubKey(CS_REG_KEY_COMMAND, false);
				string commandString = ContextMenu_CreateRegCommandString(executablePath, cmdLineArgsPrefix, cmdLineArgs);
				string? regCommandValue = hkCommand?.eGetValue_StringOrEmpty("");
				if (commandString == regCommandValue)
				{
					return true;
				}

				return false;
			}


			#region ContextMenu_RegisterForDirectory

			internal static bool ContextMenu_IsRegisteredForDirectory(
				string RegistryActionName,
				string ActionDisplayName,
				string? executablePath = null,
				string cmdLineArgsPrefix = "",
				string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG)
				=> ContextMenu_IsRegisteredForClass(C_REG_CLASS_DIRECTORY, RegistryActionName, ActionDisplayName, executablePath, cmdLineArgsPrefix, cmdLineArgs);


			internal static void ContextMenu_RegisterForDirectory(
				string RegistryActionName,
				string ActionDisplayName,
				string? executablePath = null,
				string cmdLineArgsPrefix = "",
				string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG)
				=> ContextMenu_RegisterForClass(C_REG_CLASS_DIRECTORY, RegistryActionName, ActionDisplayName, executablePath, cmdLineArgsPrefix, cmdLineArgs);


			internal static void ContextMenu_UnRegisterForDirectory(string registryActionName)
				=> ContextMenu_UnRegister(C_REG_CLASS_DIRECTORY, registryActionName);

			#endregion


			#region ContextMenu_RegisterForFile(s)

			/// <param name="RegistryActionName">
			/// !!! Registry has some unknown small limit to ActionKeyName in Shell key, so make it max shorter!!!
			/// </param>
			internal static string ContextMenu_RegisterForAllFiles(
				string RegistryActionName,
				string ActionDisplayName,
				string? executablePath = null,
				string cmdLineArgsPrefix = "",
				string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG)
				=> ContextMenu_RegisterForClass("*", RegistryActionName, ActionDisplayName, executablePath, cmdLineArgsPrefix, cmdLineArgs);


			/// <summary>Регистрация для заданного разрешения !!!НЕ КОАССА!!!</summary>
			/// <param name="filesExtensions">Разрешение файла, например '.exe' '.png')</param>
			/// <param name="RegistryActionName">Внутреннее имя ключа операции в реестре, например 'Open'
			/// !!! Registry has some unknown small limit to ActionKeyName in Shell key, so make it max shorter!!!
			/// </param>
			/// <param name="ActionDisplayName">То, что видно в контекстном меню проводника</param>
			/// <param name="ExecutablePath">Можно не указывать, если эта же программа</param>
			/// <param name="CmdLineArgument"></param>
			internal static string[] ContextMenu_RegisterForFileExt(
				string[] filesExtensions,
				string RegistryActionName,
				string ActionDisplayName,
				string? executablePath = null,
				string cmdLineArgsPrefix = "",
				string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG)
				=> filesExtensions
					.Select(ext => ContextMenu_RegisterForFileExt(ext, RegistryActionName, ActionDisplayName, executablePath, cmdLineArgsPrefix, cmdLineArgs))
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
			internal static string ContextMenu_RegisterForFileExt(
				string fileExtensionWithDot,
				string RegistryActionName,
				string ActionDisplayName,
				string? executablePath = null,
				string cmdLineArgsPrefix = "",
				string cmdLineArgs = CS_DEFAULT_CMDLINE_ARG)
			{
				if (fileExtensionWithDot.eIsNullOrWhiteSpace())
				{
					throw new ArgumentNullException(nameof(fileExtensionWithDot));
				}

				// Читаем класс файла из HKEY_CLASSES_ROOT\.pdf
				var fileClass = Extensions_DebugAndErrors.eTryCatch<string?>(
					() =>
					{
						using var hkeyFileExtension = Registry.ClassesRoot.OpenSubKey(fileExtensionWithDot);
						return hkeyFileExtension!.eGetValue_StringOrEmpty("", null);
					}, (string?)null).Result;

				if (Win10OrLater)
				{
					// в W10 контекстные команды работают по другому: 
					// HKEY_CLASSES_ROOT\SystemFileAssociations\.EXT\Shell\Action\Command = ""
					fileClass = @"SystemFileAssociations\" + fileExtensionWithDot;

					#region То работает то нет
					// Dim sW10SystemFileAssociationsClass = erunOn_TryCatch_Func(Of String)(Function()
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
					// Where ((T.Kind = RegistryValueKind.String) AndAlso T.ValueName. eIsNotNullOrWhiteSpace)).ToArray

					// If aOpenProgIDsValues.Any Then
					// Dim FirstValue = aOpenProgIDsValues.First
					// sFileClass = FirstValue.ValueName
					// End If
					// End If
					// End Using
					#endregion
				}

				if (fileClass.eIsNullOrWhiteSpace())
				{
					fileClass = fileExtensionWithDot; // В разделе не указан Класс файла. Сделаем раздел прямо на самом расширении
				}

				return ContextMenu_RegisterForClass(
					fileClass!,
					RegistryActionName,
					ActionDisplayName,
					executablePath,
					cmdLineArgsPrefix,
					cmdLineArgs);
			}

			#endregion

			/// <summary>Unregister this action on every registry class</summary>
			internal static void ContextMenu_UnRegisterAction(string[]? registryActionNames)
				=> registryActionNames?.eForEach(actionName => ContextMenu_UnRegisterAction(actionName));

			/// <inheritdoc cref="ContextMenu_UnRegisterAction" />
			internal static int ContextMenu_UnRegisterAction(string RegistryActionName, bool useActionNameAsPrefix = false)
			{
				int totalUnregistered = Registry
					.ClassesRoot?
					.GetSubKeyNames()?
					.Select(key => ContextMenu_UnRegister(key, RegistryActionName, useActionNameAsPrefix))?
					.Sum() ?? 0;

				if (!Win10OrLater)
				{
					return totalUnregistered;
				}

				try
				{
					// HKEY_CLASSES_ROOT\SystemFileAssociations\.EXT\Shell\Action\Command = ""
					const string C_W10_SystemFileAssociations = "SystemFileAssociations";

					using var hkeyW10SystemFileAssociations = Registry
						.ClassesRoot?
						.OpenSubKey(C_W10_SystemFileAssociations, RegistryKeyPermissionCheck.ReadSubTree);

					totalUnregistered += (hkeyW10SystemFileAssociations?
						.GetSubKeyNames()?
						.Select(key => ContextMenu_UnRegister(
							@$"{C_W10_SystemFileAssociations}\{key}",
							RegistryActionName,
							useActionNameAsPrefix))?
							.Sum() ?? 0);
				}
				catch { }
				return totalUnregistered;
			}

			internal static int ContextMenu_UnRegister(
				string registryClass,
				string registryActionName,
				bool useActionNameAsPrefix = false)
			{
				if (registryClass.eIsNullOrWhiteSpace())
				{
					throw new ArgumentNullException(nameof(registryClass));
				}

				string sShellKey = @$"{registryClass}\{CS_REG_KEY_SHELL}";

				string[]? keyNamesToKill = null;

				using (var keyShell = Registry
					.ClassesRoot
					.OpenSubKey(sShellKey, RegistryKeyPermissionCheck.ReadSubTree))
				{
					string actionL = registryActionName.ToLower().Trim();
					keyNamesToKill = keyShell?
						.GetSubKeyNames()?
						.Where(keyName => useActionNameAsPrefix
						? (keyName.ToLower().Trim().StartsWith(actionL))
						: (keyName.ToLower().Trim() == actionL))?
						.ToArray();

				}

				if (keyNamesToKill != null && keyNamesToKill.Any())
				{
					using var keyShell = Registry
					.ClassesRoot
					.OpenSubKey(sShellKey, RegistryKeyPermissionCheck.ReadWriteSubTree);

					keyNamesToKill?.ToList()?.ForEach(keyNameToKill
						=> keyShell?.DeleteSubKeyTree(keyNameToKill)
						);
					keyShell?.Flush();
				}
				return keyNamesToKill?.Length ?? 0;
			}


			public static void AssociateFileWithApp(
				string FileExtWithoutDot,
				string FileDecription,
				string? iconFile = null,
				int IconIndex = 0)
			{
				string newRegKey = "." + FileExtWithoutDot;
				using var keyExt = Registry.ClassesRoot.CreateSubKey(newRegKey);
				if (null == keyExt)
				{
					throw new Exception($"Failed to create '{newRegKey}' registry key!");
				}

				string sAppKeyName = string.Format("{0} {1}", System.Windows.Forms.Application.CompanyName, System.Windows.Forms.Application.ProductName).Trim();
				keyExt.SetValue("", sAppKeyName);
				keyExt.Flush();
				using var keyAppRoot = Registry.ClassesRoot.CreateSubKey(sAppKeyName);
				if (null == keyAppRoot)
				{
					throw new Exception($"Failed to create '{sAppKeyName}' registry key!");
				}

				keyAppRoot.SetValue("", FileDecription);
				keyAppRoot.Flush();

				iconFile ??= System.Windows.Forms.Application.ExecutablePath;
				using var keyIcon = keyAppRoot.CreateSubKey("DefaultIcon");
				keyIcon!.SetValue("", $"\"{iconFile}\", {IconIndex}");
				keyIcon!.Flush();
			}
			#endregion

			public static void RegisterSoundSchemeEvent(string EventName, string SoundFile, bool OnlyIfNotExist = true)
			{
				string appName = System.Windows.Forms.Application.ProductName!;
				RegistryKey EventKey = Registry.CurrentUser.CreateSubKey(@"AppEvents\Schemes\Apps\" + appName)!;
				EventKey.SetValue("", appName);
				var keyCur = EventKey.OpenSubKey(EventName + @"\.current");
				bool bNeedCreate = false;
				if (OnlyIfNotExist)
				{
					bNeedCreate = (null == keyCur);
				}

				if (bNeedCreate)
				{
					keyCur = EventKey.CreateSubKey(EventName + @"\.current");
					keyCur.SetValue("", SoundFile);
					keyCur.Flush();
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
			public static string[] RegistryHiddenUsers_GetUsers()
			{
				if (!IsRegistryHiddenUsersKeyExist())
				{
					throw new FileNotFoundException(CS_KEY_HIDDEN_USERS_LIST_FULL);
				}

				using var keyFULL = Registry.LocalMachine.OpenSubKey(CS_KEY_HIDDEN_USERS_LIST_FULL, false);
				return keyFULL?
					.GetValueNames().Where(
						sUser =>
						(keyFULL!.GetValueKind(sUser) == RegistryValueKind.DWord) && (keyFULL!.eGetValue_Int32(sUser, -1)!.Value! == 0)
					)
					.ToArray()!;
			}


			/// <summary>Checks for existing 'HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList'</summary>
			public static bool IsRegistryHiddenUsersKeyExist()
			{
				using var keyFULL = Registry.LocalMachine.OpenSubKey(CS_KEY_HIDDEN_USERS_LIST_FULL, false);
				return (null != keyFULL);
			}


			#region Open Keys
			private static RegistryKey OpenWinLogonKey(bool bWritable)
				=> Registry.LocalMachine.OpenSubKey(CS_KEY_WINLOGON, bWritable)!;


			private static RegistryKey OpenUserListKey(RegistryKey keyWINLOGON, bool bWritable, bool bCreateIfNotExist)
			{
				RegistryKey? keyUserList = keyWINLOGON.OpenSubKey(CS_KEY_HIDDEN_USERS_LIST, bWritable);
				if (null != keyUserList)
				{
					return keyUserList;
				}

				if (!bCreateIfNotExist)
				{
					throw new Exception("No hidden user profiles exist!");
				}

				return keyWINLOGON.CreateSubKey(CS_KEY_HIDDEN_USERS_LIST, RegistryKeyPermissionCheck.ReadWriteSubTree)
					?? throw new Exception($"Failed to create registry key '{CS_KEY_HIDDEN_USERS_LIST}'!");

			}
			#endregion

			#region ModifyVisibility

			public static void RegistryHiddenUsers_SetVisibility(string UserName, bool bVisible)
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
				using var keyUserList = OpenUserListKey(keyWINLOGON!, true, !bVisible);
				int iCurrentValue = keyUserList!.eGetValue_Int32(UserName, int.MaxValue)!.Value;
				if ((iCurrentValue == int.MaxValue) && bVisible)
				{
					return; //'HIDE' Registry value is not exist, and bVisible=true
				}

				int iTargetVisible = bVisible.eToInt32ABS();
				if (iCurrentValue == iTargetVisible)
				{
					return;
				}
				// Call OutString("Меняем значение видимости...")
				keyUserList!.SetValue(UserName, iTargetVisible, RegistryValueKind.DWord);
			}
			#endregion

			/// <summary>Удаляем из реестра раздел со скрытыми пользователями</summary>
			public static void RegistryHiddenUsers_DeleteRegKey()
			{
				string killSubKeyName = CS_KEY_HIDDEN_USERS_LIST.Split('\\').First();
				using RegistryKey keyWINLOGON = OpenWinLogonKey(true);
				keyWINLOGON?
					.GetSubKeyNames()?
					.eForEach(sSubKey =>
					{
						if (sSubKey.ToLower() == killSubKeyName.ToLower())
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
			internal static WindowsIdentity GetCurrentUser(bool notUseCache = false)
			{
				if (notUseCache)
				{
					return WindowsIdentity.GetCurrent();
				}

				lock (_CurrentUserSyncLock)
				{
					_CurrentUser ??= WindowsIdentity.GetCurrent();
					return _CurrentUser;
				}
			}


			internal static SecurityIdentifier GetCurrentUserSID()
			{
				_CurrentSID ??= GetCurrentUser(false).User;
				return _CurrentSID!;
			}


			internal static WindowsPrincipal GetCurrentUserPrincipal() => new(GetCurrentUser());


			internal static bool UserInAdminGroup() => GetCurrentUserPrincipal().IsInRole(WindowsBuiltInRole.Administrator);


			/// <summary>domain/user</summary>
			internal static string GetCurrentUserName() => GetCurrentUser().Name;


			///// <summary>Имя пользователя без домена</summary>
			//internal static string GetCurrentUserShortName() => GetCurrentUserSID().LookupAccountSidUserName();




			#region User Tile Image

			[DllImport(WinAPI.core.WINDLL_SHELL, EntryPoint = "#261", CharSet = CharSet.Unicode, PreserveSig = false)]
			private static extern void GetUserTilePath(
				[In, MarshalAs(UnmanagedType.LPTStr)] string? username,
				[In] uint whatever,
				[In, Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder picpath,
				[In] int maxLength);


			/// <summary>When called, OS create Userxxx.bmp file in 'C:\Users\xxx\AppData\Local\temp\' (for caller user) and returns path to that file.</summary>
			/// <param name="UserName">username: use null For current user</param>
			/// <remarks>User account pictures a placed at 'C:\Users\xxx\AppData\Roaming\Microsoft\Windows\AccountPictures</remarks>
			/// <completionlist cref=""/>
			public static string GetUserTilePath(string? UserName = null)
			{
				StringBuilder sb = new(1000);
				GetUserTilePath(UserName, 0x80000000, sb, sb.Capacity);
				return sb.ToString();
			}


			/// <inheritdoc cref="GetUserTilePath(string?)" />
			public static Image? GetUserTileImage(string? UserName = null)
			{
				string sUserImagePath = GetUserTilePath(UserName);
				if (File.Exists(sUserImagePath))
				{
					using var imgFile = Image.FromFile(sUserImagePath);
					// Надо использовать клонирование, чтобы не занимать файл изображения, а освободить его сразу после чтения
					return imgFile!.eCloneAsSomeType();
				}

				return null;

				// ********************** Domain UserAccountPicture stored in Outlook:
				// Newer versions of Office (2010+) use Active Directory to retrieve And display user photos. 
				// It 's a useful feature that also adds visual interest.
				// I can look quickly at the thumbnails at the bottom of an email or meeting request in Outlook to see who’s invited;
				// this is much faster than reading through the semi-colon delimited list of email addresses.
				/*                 
				Private void GetUserPicture(String userName)
				{
					var directoryEntry = New DirectoryEntry("LDAP://YourDomain");
					var directorySearcher = New DirectorySearcher(directoryEntry);
					directorySearcher.Filter = String.Format("(&(SAMAccountName={0}))", UserName);
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
			// Public Static extern void SetUserTile(String username, int whatever, String picpath);

			// [STAThread]
			// Static void Main(String[] args)
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



			internal static SecurityIdentifier GetSID(WellKnownSidType esid, SecurityIdentifier? domain = null)
				=> new(esid, domain);

			internal static SecurityIdentifier GetSID_Everyone()
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
		public AppMutex(string? suffix = null, bool currentUser = true, bool throwAlreadyExist = false) : base()
		{
			(Mutex, IsMutexCreated, MutexName) = CreateAppMutex(currentUser, suffix);
			ForCurrentUser = currentUser;
			Suffix = suffix;

			if (IsMutexCreated)
			{
				RegisterDisposableObject(Mutex, true);
			}
			else if (throwAlreadyExist)
			{
				throw new AppMutexAlreadyExistException();
			}
		}

		/// <inheritdoc cref="AppMutex" />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static (Mutex Mutex, bool MutexCreated, string MutexID) CreateAppMutex(
			bool currentUser,
			string? suffix = null)
		{
			string mutexID = CreateAppIDString(currentUser, suffix);
			var mtx = new Mutex(
				true,
				mutexID,
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string CreateAppIDString(bool currentUser, string? suffix = null)
		{
			string appID = AppInfo.Title ?? string.Empty;
			if (appID.eIsNullOrWhiteSpace())
			{
				throw new Exception("Application.Title = null!");
			}

			if (currentUser)
			{
				appID += C_MUTEX_PARTS_SEPARATOR + OS.UserAccounts.GetCurrentUserSID().ToString();
			}

			if (suffix.eIsNotNullOrWhiteSpace())
			{
				appID += (C_MUTEX_PARTS_SEPARATOR + suffix);
			}

			return appID;
		}

		public override string ToString() => MutexName;


		internal partial class AppMutexAlreadyExistException() : Exception(uom.WinAPI.errors.Win32Errors.ERROR_SERVICE_ALREADY_RUNNING.eToWin32Exception().Message) { }

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
				public ValueChangedUINotifer AttachToUI(Control ValueDisplayControl, Action<T?> OnValueChanged)
					=> new(this, ValueDisplayControl, OnValueChanged);

				/// <summary>Прицепляет обработку изменения значения, к отображению его в текстовом поле, с возможностью форматирования</summary>
				/// <param name="ValueDisplayControl">Элемент управления, в потоке которого будет выполнен обработчик изменения значения</param>
				/// <param name="TemplateFormatString">Шаблон строки для отображения</param>
				/// <returns>При CTL.HandleDestroyed отслеживание изменения значения автоматически прекращается</returns>
				public ValueChangedUINotifer AttachToUI(TextBox ValueDisplayControl, string? TemplateFormatString = null)
				{
					//var VT = Value.GetType();
					switch (Value!)
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
									if (TemplateFormatString.eIsNotNullOrWhiteSpace())
									{
										string S = TemplateFormatString!.eFormat(NewVal?.ToString()!);
										ValueDisplayControl.Text = S;
									}
									else
									{
										ValueDisplayControl.Text = NewVal!.ToString();
									}
								});
								return AttachToUI(ValueDisplayControl, CB);
							}

						default:
							throw new ArgumentOutOfRangeException($"Элемент управления {ValueDisplayControl.GetType()}, не может отобразить тип значения {Value!.GetType()}");
					}
				}

				/// <summary>Прицепляет обработку изменения значения, к установке ProgressBar.Value</summary>
				/// <param name="pb">Элемент управления, в потоке которого будет выполнен обработчик изменения значения</param>
				/// <returns>При CTL.HandleDestroyed отслеживание изменения значения автоматически прекращается</returns>
				public ValueChangedUINotifer AttachToUI(ProgressBar pb)
					=> Value! switch
					{
						int iVal => AttachToUI(pb, NewVal => pb.Value = iVal),
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
							if (__changedNotifer != null)
							{
								__changedNotifer.AfterValueChanged -= _VCN_OnAfterValueChanged!;
							}

							__changedNotifer = value;
							if (__changedNotifer != null)
							{
								__changedNotifer.AfterValueChanged += _VCN_OnAfterValueChanged!;
							}
						}
					}

					public readonly Control control;
					public readonly Action<T?> OnValueChangedCallBack;//{ get; private set; } = null;

					protected internal ValueChangedUINotifer(MTSafeContainerBase<T> MTSC, Control ValueDisplayControl, Action<T?> cbValueChangedCallBack) : base()
					{
						_ChangedNotifer = MTSC;
						control = ValueDisplayControl;
						OnValueChangedCallBack = cbValueChangedCallBack;

						// Отключаем слежение за изменением значения для этого элемента управления, и освобождаем ресурсы
						control.HandleDestroyed += (_, _) => Dispose();
						RegisterDisposeCallback(Destroy);
					}

					/// <summary> IDisposable</summary>
					private void Destroy()
					{
						_ChangedNotifer = null;
						//control = null;
						//OnValueChangedCallBack = null;
					}
					/// <summary>Обновляем показания в UI</summary>
					private void _VCN_OnAfterValueChanged(object sender, ValueChangedEventArgs e)
					{
						if (null == OnValueChangedCallBack)
						{
							return;
						}

						control?.eRunInUIThread(() => OnValueChangedCallBack?.Invoke(e.NewValue!));
					}

					public MTSafeContainerBase<T> ChangedNotifer { get => ChangedNotifer; }
				}
				#endregion

			}
		}

	}


	namespace ComboboxItems
	{


		[DefaultProperty("Value")]
		internal class ComboboxItemContainer<T>
		{
			public readonly T Value;
			private readonly string _displayName = string.Empty;
			private readonly Func<T, string>? _dynamicDisplayNameProvider = null;

			public ComboboxItemContainer(T wrappedValue) : base()
				=> Value = wrappedValue ?? throw new ArgumentNullException(nameof(wrappedValue));

			public ComboboxItemContainer(T wrappedValue, string displayName) : this(wrappedValue) => _displayName = displayName;

			public ComboboxItemContainer(T wrappedValue, Func<T, string> dynamicDisplayNameProvider) : this(wrappedValue) => _dynamicDisplayNameProvider = dynamicDisplayNameProvider;

			public string DisplayName
			{
				get
				{
					if (_dynamicDisplayNameProvider != null)
					{
						return _dynamicDisplayNameProvider.Invoke(Value);
					}

					if (!string.IsNullOrEmpty(_displayName))
					{
						return _displayName;
					}
#pragma warning disable CS8603 // Dereference of a possibly null reference.
					return Value!.ToString();
#pragma warning restore CS8603 // Dereference of a possibly null reference.
				}
			}

			public override string ToString() => DisplayName;
		}


		/// <summary>The wrapper class for any objects which need to be displayed in Comboboxes with custom text</summary>
		[DefaultProperty("Value")]
		internal class ComboboxItemEnumContainer<T> : ComboboxItemContainer<T> where T : Enum
		{
			public ComboboxItemEnumContainer(T val, string displayName)
				: base(val, displayName) { }

			public ComboboxItemEnumContainer(T val)
				: base(val, enumValue => enumValue.eGetDescriptionValue()) { }

		}

	}


	namespace controls
	{

		/// <summary>Typed Writable ListViewItem</summary>
		internal class ListViewItemT<T>(T? value) : ListViewItem()
		{
			public T? Value = value;

			/// <summary>Value Not Null</summary>
			public T Value2 = value!;
		}

		/// <summary>Typed Read-Only ListViewItem</summary>
		internal class ListViewItemTRO<T>(T value) : ListViewItem()
		{
			public readonly T Value = value;
		}

	}


	namespace UI
	{


		///<summary>Saves From Position and size. Process Moved/Sized Events</summary>
		[Serializable]
		public class FormPositionInfo()
		{
			private const string FORMS_SETTTINGS = "Windows";

			/// <summary>Internal storage for avoid GC</summary>
			private static Lazy<Dictionary<Form, FormPositionInfo>> _lll = new(() => []);

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


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Attach(Form f, [CallerMemberName] string caller = "")
			{
				if (caller != ".ctor")
				{
					throw new Exception("e_AttachFormPositionSaver() must be called ONLY FROM form Constructor!");
				}

				string id = GetID(f);
				var fpi = Load(f);
				if (fpi != null)
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

				lock (_lll.Value)
				{
					var dic = _lll.Value;
					if (dic.ContainsKey(f))
					{
						dic[f] = fpi!;
					}
					else
					{
						dic.Add(f, fpi!);
					}
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static FormPositionInfo? Load(Form f)
			{
				string id = GetID(f);
				try
				{
					var lines = uom.AppTools.AppSettings.Get_stringsAsText(id, string.Empty, FORMS_SETTTINGS);
					if (lines != null && lines.eIsNotNullOrWhiteSpace())
					{
						return lines!.eDeSerializeXML<FormPositionInfo>();
					}
				}
				catch { }

				return null;
			}


			private FormPositionInfo(Form f) : this() { _form = f; }


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void AttachEvents()
			{
				_form!.ResizeEnd += (_, _) => Save();
				_form!.LocationChanged += (_, _) => Save();

				_form!.Shown += (_, _) => { _canSave = true; };
				_form!.FormClosed += (_, _) =>
				{
					_canSave = false;

					//Removing from temp storage
					lock (_lll.Value)
					{
						var dic = _lll.Value;
						if (dic.ContainsKey(_form))
						{
							dic.Remove(_form);
						}
					}
					_form = null;
				};
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static string GetID(Form f) => f.GetType().FullName!;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private string GetID() => GetID(this._form!);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void FromUI()
			{
				if (!_canSave || !_form!.IsHandleCreated || _form!.IsDisposed || _form.WindowState == FormWindowState.Minimized || Screen.AllScreens.Length < 1)
				{
					return;
				}

				//Debug.WriteLine("");

				Timestamp = DateTime.Now;

				StartPosition = _form.StartPosition;
				State = _form.WindowState;

				RectOnDesktop = uom.WinAPI.windows.GetWindowRectWithoutShadow(_form.Handle);
				RectOnCurrentDisplay = RectOnDesktop;
				CurrentDisplayBounds = Screen.PrimaryScreen!.Bounds;
				Display = string.Empty;
				if (Screen.AllScreens.Length > 1)
				{
					Screen scr = Screen.FromHandle(_form.Handle);
					Display = scr.DeviceName;

					CurrentDisplayBounds = scr.Bounds;
					RectOnCurrentDisplay.Offset(-CurrentDisplayBounds.Left, -CurrentDisplayBounds.Top);

					/*

					Debug.WriteLine($"CurrentDisplay Bounds: {scr.Bounds}");
					Debug.WriteLine($"CurrentDisplay WorkingArea: {scr.WorkingArea}");
					Debug.WriteLine($"RectOnCurrentDisplay: {RectOnCurrentDisplay}");

					//var rr = uom.WinAPI.windows.GetWindowRectWithoutShadow(_f.Handle);
					//Debug.WriteLine($"GetWindowRect: {rr}");

					 */
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void Apply(Form f)
			{
				if (f.IsDisposed || State == FormWindowState.Minimized)
				{
					return;
				}

				f.StartPosition = FormStartPosition.Manual;

				Screen targetDisplay = Screen
					.AllScreens
					.Where(d => d.DeviceName.Equals(Display, StringComparison.InvariantCultureIgnoreCase))
					.FirstOrDefault() ?? Screen.PrimaryScreen!;

				if (State != FormWindowState.Maximized)
				{
					//Checking that bounds is not out of screen
					{
						int minX = Screen.AllScreens.Select(d => d.WorkingArea.Left).Min();
						int minY = Screen.AllScreens.Select(d => d.WorkingArea.Top).Min();
						int maxX = Screen.AllScreens.Select(d => d.WorkingArea.Right).Max();
						int maxY = Screen.AllScreens.Select(d => d.WorkingArea.Bottom).Max();

						if (RectOnDesktop.X < minX)
						{
							RectOnDesktop.X = minX;
						}

						if (RectOnDesktop.Y < minY)
						{
							RectOnDesktop.Y = minY;
						}

						//Ensuring window Size in not more than Current display Size
						{
							if (RectOnDesktop.Width > targetDisplay.WorkingArea.Width)
							{
								RectOnDesktop.Width = targetDisplay.WorkingArea.Width;
							}

							if (RectOnDesktop.Height > targetDisplay.WorkingArea.Height)
							{
								RectOnDesktop.Height = targetDisplay.WorkingArea.Height;
							}
						}


						//Slide window left and top if out of right and bonttom bounds
						if (RectOnDesktop.Right > maxX)
						{
							RectOnDesktop.X = maxX - RectOnDesktop.Width;
						}

						if (RectOnDesktop.Bottom > maxY)
						{
							RectOnDesktop.Y = maxY - RectOnDesktop.Height;
						}
					}

					uom.WinAPI.windows.SetWindowRectWithoutShadow(f.Handle, RectOnDesktop);
					var bb = targetDisplay.Bounds;

					if (Screen.AllScreens.Length > 1)
					{
						//
					}
				}
				else
				{
					//Maximized
					if (Screen.AllScreens.Length > 1 && !targetDisplay.Primary)
					{
						//need maximize on proper display!
						var r = _form!.Bounds;
						r = r.eCenterTo(targetDisplay.WorkingArea.eGetCenter().eToPoint());
						_form!.Bounds = r;
					}
				}
				f.WindowState = State;
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void Save()
			{
				if (!_canSave || !_form!.IsHandleCreated || _form!.IsDisposed || _form.WindowState == FormWindowState.Minimized)
				{
					return;
				}

				FromUI();
				string xml = this.eSerializeAsXML();
				uom.AppTools.AppSettings.SaveMultiString(GetID(), xml, FORMS_SETTTINGS);
				//Debug.WriteLine($"*********** Save\n{xml}");
			}


			/*

	Public Sub Load()
	If (Me._Form?.IsDisposed) Then Return

	Dim sRecordName = Me.GetID
	If(sRecordName.eIsNullOrWhiteSpace) Then Return

	Dim aRows = uomvb.Settings.GetSetting_Strings(sRecordName,,,, CS_SETTTINGS_FOLDER).Value
	If(aRows Is Nothing) OrElse(Not aRows.Any) Then Return

	Dim sRows = aRows.eJoin(vbCrLf)
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
	If (fps.Display.eIsNotNullOrWhiteSpace()) Then
		Dim lastDisplay = (From SC In Screen.AllScreens
						   Where(SC.DeviceName.eIsNotNullOrWhiteSpace AndAlso (SC.DeviceName.Equals(fps.Display, StringComparison.OrdinalIgnoreCase)))).FirstOrDefault()

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


	Public Overrides Function ToString() As String
	Dim sData = Me.eSerializeXML()
	Return sData
	End Function

	*/


		}


		internal class MessageBoxWithCheckbox : AutoDisposable1T<uom.WinAPI.hooks.LocalCbtHook>
		{
			protected uom.WinAPI.hooks.LocalCbtHook _apiHook;
			protected IntPtr _hwndDialogWindow = IntPtr.Zero;
			protected IntPtr _hwndCheckBox = IntPtr.Zero;
			protected bool _bInit = false;
			protected bool _dialogCheckBoxValue = false;
			protected string? _checkBoxText;

			public MessageBoxWithCheckbox() : base()
			{
				_apiHook = new();
				_apiHook.WindowCreated += OnWndCreated!;
				_apiHook.WindowDestroyed += OnWndDestroyed!;
				_apiHook.WindowActivated += OnWndActivated!;

				RegisterDisposableObject(_apiHook, false);
			}

			public static void ClearLastUserAnswer(string dialogID)
			{
				try { uom.AppTools.AppSettings.Delete(dialogID); }
				catch
				{
					// No processing needed...the convert might throw an exception,
					// but if so we proceed as if the value was false.
				}
			}

			public const string DEFAULT_CHECKBOX_TEXT = "Don't ask me this again";

			private DialogResult Show(
				string dialogID,
				string text,
				string? title = null,
				string? checkBoxText = DEFAULT_CHECKBOX_TEXT,
				MessageBoxButtons buttons = MessageBoxButtons.OK,
				MessageBoxIcon icon = MessageBoxIcon.Information,
				MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1)
			{

				if (string.IsNullOrWhiteSpace(dialogID))
				{
					throw new ArgumentNullException(nameof(dialogID));
				}

				if (string.IsNullOrWhiteSpace(checkBoxText))
				{
					checkBoxText = DEFAULT_CHECKBOX_TEXT;
				}

				if (string.IsNullOrWhiteSpace(title))
				{
					title = Application.ProductName;
				}

				try
				{
					const int VALUE_INVALID = -1;
					int? regOldCheckBoxValue = uom.AppTools.AppSettings.Get_Int32(dialogID, VALUE_INVALID, "MessageBoxWithCheckbox");

					if (regOldCheckBoxValue.HasValue
						&& regOldCheckBoxValue.Value != VALUE_INVALID
						&& System.Enum.IsDefined(typeof(DialogResult), regOldCheckBoxValue.Value))
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
					DialogResult dr = System.Windows.Forms.MessageBox.Show(text, title, buttons, icon, defbtn);
					//Save User Answer to registry
					if (_dialogCheckBoxValue)
					{
						uom.AppTools.AppSettings.Save<int>(dialogID, (int)dr, "MessageBoxWithCheckbox");
					}

					return dr;
				}
				finally
				{
					_apiHook.Uninstall();
				}
			}


			public static DialogResult ShowDialog(
				string dialogID,
				string text,
				string? title = null,
				string? checkBoxText = DEFAULT_CHECKBOX_TEXT,
				MessageBoxButtons buttons = MessageBoxButtons.OK,
				MessageBoxIcon icon = MessageBoxIcon.Information,
				MessageBoxDefaultButton defbtn = MessageBoxDefaultButton.Button1)
			{
				using MessageBoxWithCheckbox dlg = new();
				return dlg.Show(dialogID, text, title, checkBoxText ?? DEFAULT_CHECKBOX_TEXT, buttons, icon, defbtn);
			}


			private void OnWndCreated(object sender, uom.WinAPI.hooks.CbtEventArgs e)
			{
				if (e.IsDialogWindow)
				{
					_bInit = false;
					_hwndDialogWindow = e.Handle;
				}
			}

			private void OnWndDestroyed(object sender, uom.WinAPI.hooks.CbtEventArgs e)
			{
				if (e.Handle == _hwndDialogWindow)
				{
					_bInit = false;
					_hwndDialogWindow = IntPtr.Zero;
					if (BST_CHECKED == (int)uom.WinAPI.windows.SendMessage(_hwndCheckBox, BM_GETCHECK, IntPtr.Zero, IntPtr.Zero))
					{
						_dialogCheckBoxValue = true;
					}
				}
			}

			private void OnWndActivated(object sender, uom.WinAPI.hooks.CbtEventArgs e)
			{
				if (_hwndDialogWindow != e.Handle || _bInit)
				{
					return;
				}

				_bInit = true;
				// Get the current font, either from the static text window or the message box itself
				IntPtr hwndText = uom.WinAPI.windows.GetDlgItem(_hwndDialogWindow, 0xFFFF);
				IntPtr hFont = uom.WinAPI.windows.SendMessage(
					hwndText.eIsValid() ? hwndText : _hwndDialogWindow,
					WinAPI.windows.WindowMessages.WM_GETFONT, IntPtr.Zero, IntPtr.Zero);

				Font fCur = Font.FromHfont(hFont);

				// Get the x coordinate for the check box.  Align it with the icon if possible, or one character height in
				Point ptCheckBoxLocation = new();
				IntPtr hwndIcon = uom.WinAPI.windows.GetDlgItem(_hwndDialogWindow, 0x0014);
				if (hwndIcon != IntPtr.Zero)
				{
					Rectangle rcIcon = uom.WinAPI.windows.GetWindowRect(hwndIcon);
					Point pt = rcIcon.Location;
					uom.WinAPI.windows.ScreenToClient(_hwndDialogWindow, ref pt);
					ptCheckBoxLocation.X = pt.X + (rcIcon.Width / 2) - 4;
				}
				else
				{
					ptCheckBoxLocation.X = (int)fCur.GetHeight();
				}

				// Get the y coordinate for the check box, which is the bottom of the current message box client area
				System.Drawing.Rectangle rcLicent = uom.WinAPI.windows.GetClientRect(_hwndDialogWindow);
				int fontHeight = (int)fCur.GetHeight();
				ptCheckBoxLocation.Y = rcLicent.Height + fontHeight;


				// Resize the message box with room for the check box
				Rectangle rc = uom.WinAPI.windows.GetWindowRect(_hwndDialogWindow);
				uom.WinAPI.windows.MoveWindow(_hwndDialogWindow,
					rc.Left,
					rc.Top,
					rc.Width,
					rc.Height + (fontHeight * 3),
					true);


				_hwndCheckBox = uom.WinAPI.windows.CreateWindowEx(
					0,
					"button",
					_checkBoxText!,

					WinAPI.windows.WindowStyles.BS_AUTOCHECKBOX |
					WinAPI.windows.WindowStyles.WS_CHILD |
					WinAPI.windows.WindowStyles.WS_VISIBLE |
					WinAPI.windows.WindowStyles.WS_TABSTOP,

					ptCheckBoxLocation.X,
					ptCheckBoxLocation.Y,
					rc.Width - ptCheckBoxLocation.X,
					fontHeight,
					_hwndDialogWindow,
					IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);


				uom.WinAPI.windows.SendMessage(_hwndCheckBox, WM_SETFONT, hFont, new IntPtr(1));
			}

			#region Win32 Imports

			private const int WM_SETFONT = 0x00000030;
			//private const int WM_GETFONT = 0x00000031;
			private const int BM_GETCHECK = 0x00F0;
			private const int BST_CHECKED = 0x0001;

			#endregion
		}


	}



	namespace Extensions
	{





		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_Debug_Dump_WinForms
		{



			private static readonly Size _defaultPropertyGridFormSize = new(600, 800);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void ePropertyGrid_DisplayInUI(this object o)
			{
				using Form f = new()
				{
					StartPosition = FormStartPosition.CenterScreen,
					Text = $"{o.GetType()}",
					Size = _defaultPropertyGridFormSize
				};
				f.eAttach_CloseOnEsc();

				using PropertyGrid pg = new()
				{
					Dock = DockStyle.Fill,
					SelectedObject = o
				};

				f.Controls.Add(pg);
				f.ShowDialog();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void ePropertyGrid_DisplayInUI<T>(this T[] o) where T : class
			{
				if (o.Length < 1)
				{
					return;
				}

				using Form f = new()
				{
					StartPosition = FormStartPosition.CenterScreen,
					Text = $"{o.First().GetType()} ({o.Length})",
					Size = _defaultPropertyGridFormSize
				};
				f.eAttach_CloseOnEsc();

				using PropertyGrid pg = new()
				{
					Dock = DockStyle.Fill,
					SelectedObjects = o
				};

				f.Controls.Add(pg);
				f.ShowDialog();
			}

		}





		[EditorBrowsable(EditorBrowsableState.Never)]
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
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static (
				MessageBoxButtons Buttons,
				MessageBoxIcon Icon,
				MessageBoxDefaultButton DefaultButton
				) ParseMsgboxFlags(MsgBoxFlags flags)
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
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DialogResult eMsgboxShow(
				this string msg,
				MsgBoxFlags? flags = MsgBoxFlags.Btn_OK | MsgBoxFlags.Icn_Information | MsgBoxFlags.DefBtn_1,
				string? title = null)
			{
				flags ??= (MsgBoxFlags.Btn_OK | MsgBoxFlags.Icn_Information | MsgBoxFlags.DefBtn_1);
				title ??= Application.ProductName;

				var ff = ParseMsgboxFlags(flags.Value);
				DialogResult dr = MessageBox.Show(msg, title!, ff.Buttons, ff.Icon, ff.DefaultButton);
				return dr;
			}


			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eMsgboxError(this Exception ex, string? title = null)
			{
				ex.Message.eMsgboxShow((MsgBoxFlags.Btn_OK | MsgBoxFlags.Icn_Error));
			}


			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DialogResult eMsgboxAsk(
				this string question,
				bool defButtonYes = true,
				string? title = null)
			{
				MsgBoxFlags flg = MsgBoxFlags.Btn_YesNo | MsgBoxFlags.Icn_Question
					| (defButtonYes
					? MsgBoxFlags.DefBtn_1
					: MsgBoxFlags.DefBtn_2);

				return question.eMsgboxShow(flg, title);
			}

			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool eMsgboxAskIsYes(
				this string question,
				bool defButtonYes = true,
				string? title = null)
				=> question.eMsgboxAsk(defButtonYes, title) == DialogResult.Yes;


			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DialogResult eMsgboxWithCheckboxAsk(
				this string question,
				string dialogID,
				bool defButtonYes = true,
				string? checkBoxText = uom.UI.MessageBoxWithCheckbox.DEFAULT_CHECKBOX_TEXT,
				string? title = null)
			{
				MsgBoxFlags flg = MsgBoxFlags.Btn_YesNo | MsgBoxFlags.Icn_Question
					| (defButtonYes
					? MsgBoxFlags.DefBtn_1
					: MsgBoxFlags.DefBtn_2);

				var ff = ParseMsgboxFlags(flg);

				DialogResult dr = uom.UI.MessageBoxWithCheckbox.ShowDialog(
					dialogID,
					question,
					title,
					checkBoxText,
					ff.Buttons,
					ff.Icon,
					ff.DefaultButton);

				return dr;
			}


			[DebuggerNonUserCode, DebuggerStepThrough]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eMsgboxWithCheckboxClearLastUserAnswer(this string dialogID)
				=> uom.UI.MessageBoxWithCheckbox.ClearLastUserAnswer(dialogID);



		}






		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_WinForms_Controls_Common
		{



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eRunOnDisabled(this Control ctl, Action a)
			{
				_ = a ?? throw new ArgumentNullException(nameof(a));

				bool oldEnabledStatus = ctl.Enabled;
				ctl.Enabled = false;
				try
				{
					a.Invoke();
				}
				finally
				{
					if (oldEnabledStatus)
					{
						ctl.Enabled = true;
					}
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eRunOnDisabled(this IEnumerable<Control> actls, Action a)
			{
				_ = a ?? throw new ArgumentNullException(nameof(a));

				var L = actls.Select(ctl => new { Ctl = ctl, EnabledStatus = ctl.Enabled }).ToList();

				L.ForEach(ctl => ctl.Ctl.Enabled = false);
				try
				{
					a.Invoke();
				}
				finally
				{
					L.ForEach(ctl => ctl.Ctl.Enabled = ctl.EnabledStatus);
				}
			}



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eEnable(this IEnumerable<Control> actls, bool e)
				=> actls.ToList().ForEach(ctl => ctl.Enabled = e);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eEnableItems(this ToolStrip ts, bool e)
			{
				foreach (ToolStripItem ctl in ts.Items)
				{
					ctl.Enabled = e;
				}
			}






			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static async Task eRunOnDisabledAsync(
				this IEnumerable<Control>? actls,
				Func<Task> a,
				Form? waitCursorForm = null)
			{
				_ = a ?? throw new ArgumentNullException(nameof(a));

				if (null != waitCursorForm)
				{
					waitCursorForm!.UseWaitCursor = true;
				}

				var L = actls?.Select(ctl => new { Ctl = ctl, OldEnabledStatus = ctl.Enabled }).ToList();

				try
				{
					L?.ForEach(ctl =>
					{
						ctl.Ctl.Enabled = false;
						ctl.Ctl.Update();
					});

					await a.Invoke();
				}
				finally
				{
					L?.ForEach(ctl =>
					{
						if (ctl.OldEnabledStatus)
						{
							ctl.Ctl.Enabled = true;
						}
					});

					if (null != waitCursorForm)
					{
						waitCursorForm!.UseWaitCursor = false;
					}
				}

			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static async Task eRunOnDisabledAsync(
				this Control ctl,
				Func<Task> a,
				Form? waitCursorForm = null)
				=> await ctl.eToArrayOf().eRunOnDisabledAsync(a, waitCursorForm);




			/// <inheritdoc cref="uom.WinAPI.windows.GetHighDPIScaleFactor" />
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static float eGetHighDPIScaleFactor(this Control ctl)
				=> uom.WinAPI.windows.GetHighDPIScaleFactor(ctl.Handle);



			#region ShortcutKeys to/from string

			/// <summary>Converts Keys value to shortcut keys string like 'Ctrl+I' </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string eToShortcutKeysString(this System.Windows.Forms.Keys eKey)
				=> (string)(new KeysConverter().ConvertTo(eKey, typeof(string))!);


			/// <summary>Converts shortcut keys string to Keys value</summary>
			/// <param name="sKeys">Sample 'Ctrl+I'</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static System.Windows.Forms.Keys eToShortcutKeys(this string sKeys)
				=> (Keys)(new KeysConverter().ConvertFrom(sKeys)!);

			#endregion


			#region runInUIThread











			/*
	/// <summary>MT Safe call code in UI thread.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void eRunInUIThread(this Control context, System.Windows.Forms.MethodInvoker code)
	{
	#region SAMPLE WITH RETURN VALUE:

	//lstvwgrp.ListView.eRunInUIThread(delegate
	//{
	//	result = SendMessage(lstvwgrp.ListView.Handle, ListViewMessages.LVM_SETGROUPINFO, groupId, ref group);
	//});
	//return result;

	#endregion

	#region SAMPLE WITHOUT RETURN VALUE:

	//lstvwgrp.ListView.eRunInUIThread(delegate
	//{
	//	SendMessage(lstvwgrp.ListView.Handle, ListViewMessages.LVM_SETGROUPINFO, groupId, ref group);
	//});

	#endregion

	_ = context ?? throw new ArgumentNullException(nameof(context));
	_ = code ?? throw new ArgumentNullException(nameof(code));

	if (!context.IsHandleCreated || context.IsDisposed) return;
	if (context.InvokeRequired)
	{
	try { context.Invoke(code); }
	catch { }
	}
	else
	code.Invoke();
	}
			 */




			/// <summary>ThreadSafe excute action in control UI thread
			/// <param name="context">Control in which UI thread action erunutes</param>
			/// <param name="asyncInvoke">If true - used BeginInvoke, else used Invoke</param>
			/// <param name="onError">Any action when Error occurs</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eRunInUIThread(this Control context, Action a, bool asyncInvoke = false, Action<Exception>? onError = null)
			{
				_ = context ?? throw new ArgumentNullException(nameof(context));
				_ = a ?? throw new ArgumentNullException(nameof(a));
				if (!context.IsHandleCreated || context.IsDisposed)
				{
					return;
				}

				try
				{
					if (asyncInvoke)
					{
						context.BeginInvoke(a);
					}
					else if (context.InvokeRequired)
					{
						context.Invoke(a);
					}
					else
					{
						a.Invoke();
					}
				}
				catch (Exception EX) { onError?.Invoke(EX); }
			}


			/// <summary>ThreadSafe excute action in control UI thread
			/// <param name="context">Control in which UI thread action erunutes</param>
			/// <param name="useBeginInvoke">If true - used BeginInvoke, else used Invoke</param>
			/// <param name="onError">Any action when Error occurs</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T? eRunInUIThread<T>(this Control context, Func<T> a, Action<Exception>? onError = null)
			{
				_ = context ?? throw new ArgumentNullException(nameof(context));
				_ = a ?? throw new ArgumentNullException(nameof(a));
				if (!context.IsHandleCreated || context.IsDisposed)
				{
					return default;
				}

				try
				{
					if (context.InvokeRequired)
					{
						return (T)context.Invoke(a);
					}
					return a.Invoke();
				}
				catch (Exception EX)
				{
					onError?.Invoke(EX);
					return default;
				}
			}

			/// <summary>ThreadSafe excute action in control UI thread
			/// <param name="context">Control in which UI thread action erunutes</param>
			/// <param name="onError">Any action when Error occurs</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static async Task eRunInUIThreadAsync(this Control context, Func<Task> a, Action<Exception>? onError = null)
			{
				_ = context ?? throw new ArgumentNullException(nameof(context));
				_ = a ?? throw new ArgumentNullException(nameof(a));

				if (!context.IsHandleCreated || context.IsDisposed)
				{
					return;
				}

				try
				{
					if (context.InvokeRequired)
					{
#if NET
						await context.Invoke(a);
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
				catch (Exception EX) { onError?.Invoke(EX); }
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eRunWhenHandleReady<T>(this T ctl, Action<Control> HandleReadyAction) where T : Control
			{
				_ = ctl ?? throw new ArgumentNullException(nameof(ctl));

				if (ctl.Disposing || ctl.IsDisposed)
				{
					return;
				}

				if (ctl.IsHandleCreated)
				{
					HandleReadyAction?.Invoke(ctl);//Control handle already Exist, run immediate
				}
				else
				{
					//Delay action when handle will be ready...
					ctl.HandleCreated += (s, e) => HandleReadyAction?.Invoke((T)s!);
				}
			}





			/// <summary>MT Safe</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eRunInUIThread_AppendText(this TextBox ctl, string Text)
				=> ctl.eRunInUIThread(() => { ctl.AppendText(Text); ctl.Update(); });

#if NET6_0_OR_GREATER
			/// <summary>MT Safe</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eRunInUIThread_AppendLine(this TextBox ctl, string Text, int limitLinesCount = 0)
				=> ctl.eRunInUIThread(() =>
				{
					if (limitLinesCount > 0)
					{
						ctl.Lines = ctl.Lines.TakeLast(limitLinesCount).ToArray();
					}
					ctl.AppendText($"{Text}\r\n"); ctl.Update();
				}
				);
#endif

			/// <summary>MT Safe</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eRunInUIThread_SetText(this Control ctl, string Text)
				=> ctl.eRunInUIThread(() => { ctl.Text = Text; ctl.Update(); });

			/// <summary>MT Safe</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eRunInUIThread_SetText(this ToolStripItem ctl, string Text, Form? frmUI = null)
			{
				_ = ctl ?? throw new ArgumentNullException(nameof(ctl));
				frmUI ??= ctl.GetCurrentParent()?.FindForm();//If form not specifed = search form...
															 //_ = frmUI ?? throw new ArgumentNullException("ToolStripItem.GetCurrentParent.FindForm() = NULL!");
				frmUI?.eRunInUIThread(() => { if (frmUI.IsHandleCreated && !frmUI.IsDisposed && !ctl.IsDisposed) { ctl.Text = Text; } });
			}



			#endregion



			#region FORM


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAttach_CloseOnEsc(this Form f)
			{
				f.KeyPreview = true;
				f.KeyDown += (s, e) =>
				{
					Form f2 = (Form)s!;
					if (e.KeyCode == Keys.Escape)
					{
						f2.DialogResult = DialogResult.Cancel;
					}
				};
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAttach_PositionAndStateSaver(this Form ctx, [CallerMemberName] string caller = "")
				=> UI.FormPositionInfo.Attach(ctx, caller);



			#region eAttach_CloseOnClick


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAttach_CloseOnClick(this Form ctx, System.Windows.Forms.ToolStripMenuItem MI)
				=> MI.Click += (_, _) => ctx?.eCloseSafe();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAttach_CloseOnClick(this Form ctx, Button Ctl)
				=> Ctl.Click += (_, _) => ctx?.eCloseSafe();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void eCloseSafe(this Form f) => f?.eRunInUIThread(() => f?.Close());


			#endregion







			/// <summary>Executes after form shown on screen, with specifed delay.
			/// Delay starts after 'Form.Shown' ebent</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eAttach_RunOnClosed(this Form ctx, Action action, bool ignoreExceptions = false)
			{
				_ = ctx ?? throw new ArgumentNullException(nameof(ctx));

				ctx.FormClosed += (ctx, ea) =>
				{
					try { action.Invoke(); }
					catch
					{
						if (!ignoreExceptions)
						{
							throw;
						}
					}
				};
			}





			#region runDelayed 

			internal const int DEFAULT_DELAY = 100;
			internal const int DEFAULT_FORM_SHOWN_DELAY = 500;




			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Windows.Forms.Timer eRunDelayedTask(this Func<Task> delayedAction, int delay = DEFAULT_DELAY)
			{
				delayedAction.ThrowIfNull();

				//Use 'System.Windows.Forms.Timer' that uses some thread with caller to raise events
				System.Windows.Forms.Timer tmrDelay = new()
				{
					Interval = delay,
					Enabled = false //do not start timer untill we finish it's setup
				};

				tmrDelay.Tick += async (s, e) =>
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
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Windows.Forms.Timer eRunDelayed(this Action delayedAction, int delay = DEFAULT_DELAY)
			{
				delayedAction.ThrowIfNull();

				//Use 'System.Windows.Forms.Timer' that uses some thread with caller to raise events
				System.Windows.Forms.Timer tmrDelay = new()
				{
					Interval = delay,
					Enabled = false //do not start timer untill we finish it's setup
				};
				tmrDelay.Tick += (_, _) =>
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




			private static Lazy<Dictionary<System.Guid, System.Windows.Forms.Timer>> _timersStorage = new(() => []);
			/// <summary>
			/// Usually used when you need to do an action with a slight delay after exiting the current method. 
			/// For example, if some data will be ready only after exiting the control event handler processing branch
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eRunDelayedInUIThread(this Control ctx, Action delayedAction, int delay = DEFAULT_DELAY)
			{
				var id = System.Guid.NewGuid();
				var tmr = eRunDelayed(() =>
				{
					lock (_timersStorage)
					{
						var dic = _timersStorage.Value;
						if (dic.ContainsKey(id))
						{
							dic.Remove(id);
						}
					}
					ctx!.Invoke(delayedAction);
				}
				, delay);

				lock (_timersStorage)
				{
					_timersStorage.Value.Add(id, tmr);
				}
			}


			/// <summary>Executes 'delayedAction' after form shown on screen, with specifed delay in UI Thread.
			/// Delay starts after 'Form.Shown' event</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eRunDelayed_OnShown(
				this Form ctx,
				Action delayedAction,
				int delay = DEFAULT_FORM_SHOWN_DELAY,
				bool useWaitCursor = true,
				bool onErrorShowUI = true,
				Action<Exception>? onError = null,
				bool onErrorCloseForm = true
				)
			{
				async Task t()
				{
					await Task.Delay(1);
					delayedAction.Invoke();
				}

				ctx!.eRunDelayed_OnShown(t, delay, useWaitCursor, onErrorShowUI, onError, onErrorCloseForm);
			}


			/// <inheritdoc cref="eRunDelayed_OnShown" />
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eRunDelayed_OnShown(
				this Form ctx,
				IEnumerable<Func<Task>> delayedTasks,
				int delay = DEFAULT_FORM_SHOWN_DELAY,
				bool useWaitCursor = true,
				bool onErrorShowUI = true,
				Action<Exception>? onError = null,
				bool onErrorCloseForm = true)
			{
				async Task onShown()
				{
					await Task.Delay(delay);

					if (useWaitCursor && !ctx.UseWaitCursor)
					{
						ctx.UseWaitCursor = true;
					}

					try
					{
						foreach (var tsk in delayedTasks)
						{
							await tsk.Invoke();
						}
					}
					catch (OperationCanceledException) { }                 //catch (TaskCanceledException tcex) { }
					catch (Exception ex)
					{
						ex.eLogError(onErrorShowUI);
						onError?.Invoke(ex);
						if (onErrorCloseForm)
						{
							ctx.Close();
						}
					}
					finally
					{
						if (useWaitCursor && ctx.UseWaitCursor)
						{
							ctx.UseWaitCursor = false;
						}
					}

				}
				ctx!.Shown += async (_, _) => await onShown();
			}


			/// <inheritdoc cref="eRunDelayed_OnShown" />
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eRunDelayed_OnShown(
				this Form ctx,
				Func<Task> delayedTask,
				int delay = DEFAULT_FORM_SHOWN_DELAY,
				bool useWaitCursor = true,
				bool onErrorShowUI = true,
				Action<Exception>? onError = null,
				bool onErrorCloseForm = true)
					=> ctx.eRunDelayed_OnShown([delayedTask], delay, useWaitCursor, onErrorShowUI, onError, onErrorCloseForm);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eRunDelayed_OnShown_SetFocus(this Form ctx, int delay = DEFAULT_FORM_SHOWN_DELAY)
				=> ctx?.eRunDelayed_OnShown(delegate
				{
					ctx?.Focus();
					ctx?.Activate();
					ctx?.BringToFront();
				}, delay);





























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









			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool etryOnWaitCursor(
					this Form f,
					Action a,
					bool showErrorMessageBox = true,
					string ErrorMessageBoxTitle = "Error")
			{
				try
				{
					f.Cursor = Cursors.WaitCursor;
					try { a.Invoke(); }
					finally { f.Cursor = Cursors.Default; }

					return true;
				}
				catch (Exception ex)
				{
					if (showErrorMessageBox)
					{
						MessageBox.Show(ex.Message.ToString(), ErrorMessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					return false;
				}
			}





















			#region SetIcon_

#if NETFRAMEWORK

			[ MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void eSetIcon_Core<t>(this t Ctl, System.Drawing.Icon rIcon) where t : class
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


			//[ MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static void SetIconsSafe<t>(this IEnumerable<t> eCtl, int IconID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = LRI_ICON_SIZE.Small) where t : class
			//{
			//    foreach (var rCtl in eCtl) rCtl.SetIconSafe(IconID, eLib, eMethod, eIconSize);
			//}

			//[ MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static void SetIconSafe<t>(this t rCtl, int IconID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = LRI_ICON_SIZE.Small) where t : class
			//{
			//    var rImg = LoadResWin32Icon_Safe(IconID, eLib, eMethod, eIconSize);
			//    rCtl.SetIcon_Core(rImg);
			//}

			//[ MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static void SetIconsSafe<t>(this IEnumerable<t> eCtl, LRI_ID IconID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = LRI_ICON_SIZE.Small) where t : class
			//{
			//    eCtl.SetIconsSafe(IconID, eLib, eMethod, eIconSize);
			//}

			//[ MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static void SetIconSafe<t>(this t rCtl, LRI_ID IconID, LRI_Libs eLib = C_DEFAULT_LRI_Libs, LRI_METHOD eMethod = C_DEFAULT_LRI_METHOD, LRI_ICON_SIZE eIconSize = LRI_ICON_SIZE.Small) where t : class
			//{
			//    rCtl.SetIconSafe(IconID, eLib, eMethod, eIconSize);
			//}

#endif

			#endregion

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool eShowDialog_OK(this Form FormToShow, Form? ParentForm = null)
				=> ((null == ParentForm) ? FormToShow.ShowDialog() : FormToShow.ShowDialog(ParentForm)) == System.Windows.Forms.DialogResult.OK;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool eShowDialog_NotOK(this Form FormToShow, Form? ParentForm = null) => !FormToShow.eShowDialog_OK(ParentForm);

			#endregion


			#region IIf



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eIIF_EnableControl(this bool B, params Component[] cmpnents)
			{
				cmpnents?.eForEach(cmpnent =>
				{
					var BB = B;
					switch (cmpnent)
					{
						case Control ctl: ctl.Enabled = BB; break;
						case ToolStripItem tsi: tsi.Enabled = BB; break;
						default: throw new Exception($"IIFEnableControl failed for '{cmpnent.GetType()}'");
					}
				});
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eIIF_SetVisible(this bool B, params Component[] cmpnents)
			{
				cmpnents?.eForEach(cmpnent =>
				{
					var BB = B;
					switch (cmpnent)
					{
						case Control ctl: ctl.Visible = BB; break;
						case ToolStripItem tsi: tsi.Visible = BB; break;
						default: throw new Exception($"e_IIFSetVisible failed for '{cmpnent.GetType()}'");
					}
				});
			}



			#endregion





			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eSetStyleInternal(this Control ctl, ControlStyles st, bool bset)
			{
				_ = ctl ?? throw new ArgumentNullException(nameof(ctl));

				MethodInfo objMethodInfo = typeof(Control).GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance)!;
				object[] args = [st, bset];
				objMethodInfo.Invoke(ctl, args);
			}



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Control[] eToArray(this Control.ControlCollection cc)
				=> cc
				.Cast<Control>().ToArray();


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T[] eControlsOf<T>(this Control[] cc) where T : Control
				=> cc
				.Where(c => c.GetType() == typeof(T))
				.Cast<T>()
				.ToArray();


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T[] eControlsOf<T>(this Control.ControlCollection cc) where T : Control
				=> cc
				.Cast<Control>()
				.ToArray()
				.eControlsOf<T>();


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T[] eChildControlsOf<T>(this Control ctl) where T : Control
				=> ctl.Controls.eControlsOf<T>();



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static HandleRef eCreateHandleReff(this Control ctl) => new(ctl!, ctl!.Handle);

		}



		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_WinForms_Controls_CheckBox
		{


			public static T eBuildFromFlags<T>(this T initialValue, params (CheckBox flagCondition, T flagToSet)[] flags) where T : Enum
			{
				Int64 val = Convert.ToInt64(initialValue);
				foreach (var item in flags)
				{
					if (item.flagCondition.Checked)
					{
						val |= Convert.ToInt64(item.flagToSet);
					}
				}
				T TResult = (T)Enum.ToObject(typeof(T), val);
				return TResult;
			}



		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_WinForms_Controls_TextBox
		{


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eSetVistaCueBanner(this TextBox ctl, string? BannerText = null)
			{
				_ = ctl ?? throw new ArgumentNullException(nameof(ctl));

				ctl.eRunWhenHandleReady(tb => WinAPI.windows.SendMessage(
					tb.Handle,
					(int)WinAPI.windows.TextBoxMessages.EM_SETCUEBANNER,
					0,
					BannerText));
			}


			#region AttachDelayedFilter

			private const int DEFAULT_TEXT_EDIT_DELAY = 1000;
			private const string DEFAULT_FILTER_CUEBANNER = "Filter";


			/// <summary>
			/// Attaches a deferred text change event handler that makes it possible to react to text changes with some delay, 
			/// allowing the user to correct erroneous input, 
			/// or complete input, rather than reacting immediately to each letter.
			/// </summary>
			/// <param name="onTextChanged">TextChanged Handler</param>
			/// <param name="delay">Delay (ms.) during which repeated input will not call the handler</param>
			/// <param name="cueBanner">Vista cueabanner text</param>
			/// <param name="changeBackColor">Sets the background color for textbox to Systemcolors.Info</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAttachDelayedFilter(
				this TextBox txt,
				Action<string> onTextChanged,
				int delay = DEFAULT_TEXT_EDIT_DELAY,
				string? cueBanner = DEFAULT_FILTER_CUEBANNER,
				bool changeBackColor = true)
			{
				var TMR = new System.Windows.Forms.Timer() { Interval = delay };
				txt.Tag = TMR; //Сохраняем ссылку на таймер хоть где-то, чтобы GC его не грохнул.

				if (cueBanner != null)
				{
					txt.eSetVistaCueBanner(cueBanner);
				}

				if (changeBackColor)
				{
					txt.BackColor = SystemColors.Info;
				}

				TMR.Tick += delegate
				{
					TMR.Stop(); //Останавливаем таймер
					var sNewText = txt.Text;
					onTextChanged.Invoke(sNewText);
				};

				txt.TextChanged += delegate
				{
					//Restart timer...
					TMR.Stop();
					TMR.Start();
				};
			}

			/// <inheritdoc cref="AttachDelayedFilter" />
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAttachDelayedFilter(
				this TextBox txt,
				System.Windows.Forms.MethodInvoker onTextChanged,
				int delay = DEFAULT_TEXT_EDIT_DELAY,
				string cueBanner = DEFAULT_FILTER_CUEBANNER,
				bool changeBackColor = true)
					=> txt.eAttachDelayedFilter(
						new Action<string>((_) => onTextChanged.Invoke()),
						delay, cueBanner, changeBackColor);

			/// <inheritdoc cref="AttachDelayedFilter" />
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAttachDelayedFilter(
				this ToolStripTextBox tstb,
				Action<string> onTextChanged,
				int delay = DEFAULT_TEXT_EDIT_DELAY,
				string cueBanner = DEFAULT_FILTER_CUEBANNER,
				bool changeBackColor = true)
					=> tstb.TextBox.eAttachDelayedFilter(onTextChanged, delay, cueBanner, changeBackColor);





			#endregion

			private const bool C_APPEND_NEW_LINE_DEFAULT = false;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAppendTextWithActionOnSelection(this RichTextBox txt, string text, Action a, bool appendNewLine = C_APPEND_NEW_LINE_DEFAULT)
			{
				int selStart = txt.Text.Length;
				txt.AppendText(text);
				int selLen = txt.Text.Length - selStart;
				txt.Select(selStart, selLen);
				a.Invoke();
				txt.Select(txt.TextLength, 0);
				if (appendNewLine)
				{
					txt.AppendText("\n");
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAppendTextFormatted(
				this RichTextBox txt,
				string text,
				Color? clrFore = null,
				Color? clrBack = null,
				Font? fnt = null,
				bool appendNewLine = C_APPEND_NEW_LINE_DEFAULT)
			{
				txt.eAppendTextWithActionOnSelection(text,
					 delegate
					 {
						 if (clrFore != null)
						 {
							 txt.SelectionColor = clrFore.Value;
						 }

						 if (clrBack != null)
						 {
							 txt.SelectionBackColor = clrBack.Value;
						 }

						 if (fnt != null)
						 {
							 txt.SelectionFont = fnt;
						 }
					 },
					 appendNewLine);
			}

		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_WinForms_Controls_LinkLabel
		{
			/*
			private class LinkLaberLinkTarget
			{
				public string? LinkText;
			}
			 */


			//<a href="mailto:mail@htmlacademy.ru">Напишите нам</a>


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eClearAllLinks(this LinkLabel ll) => ll.LinkArea = new LinkArea(0, 0);


			/// <summary>Sets Text and clears all links</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eSetTextNoLink(this LinkLabel ll, string text)
			{
				ll.Text = text;
				ll.eClearAllLinks();
			}

			/// <summary>Append Text. Links does not changes</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAppendText(this LinkLabel ll, string text)
			{
				ll.Text += text;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAppendTextWithLinkSimple(this LinkLabel ll, string text, Action<LinkLabel.Link>? clickHandler = null, bool clearOldLinks = false)
			{
				string oldText = clearOldLinks
					? string.Empty
					: ll.Text;

				int initLen = oldText.Length;
				if (clearOldLinks)
				{
					ll.eClearAllLinks();
				}

				//"Текущий MAC-адрес: ['{qr.MAC.eToStringHex()}']"

				Regex rxSimpleLink = new(@"^(?<Prefix>.*)\[(?<LinkText>.*)\](?<Suffix>.*)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

				MatchCollection result = rxSimpleLink.Matches(text);
				if (result.Count < 1)
				{
					throw new ArgumentException($"Not found any Links in the string '{text}'", nameof(text));
				}

				if (result.Count > 1)
				{
					throw new ArgumentException($"Found more than one Link in the string '{text}'", nameof(text));
				}

				Match m = result.Cast<Match>().First();
				string prefix = m.Groups["Prefix"].Value;
				string suffix = m.Groups["Suffix"].Value;
				Group link = m.Groups["LinkText"];
				string linkText = link.Value;

				//string ss = text.Substring(link.Index, link.Length);

				Guid linkID = Guid.NewGuid();
				int startPos = prefix.Length;
				int LinkLen = linkText.Length;
				LinkLabel.Link lll = new(initLen + startPos, LinkLen, linkID);
				text = $"{prefix}{linkText}{suffix}";

				ll.Text = oldText + text;
				ll.Links.Add(lll);

				if (clickHandler == null)
				{
					return;
				}

				ll.LinkClicked += (_, e) =>
				{
					LinkLabel.Link? lllClicked = e.Link;
					if (lllClicked == null || lllClicked.LinkData is not Guid || !linkID.Equals((Guid)lllClicked.LinkData))
					{
						return;
					}
					//This is our Link Clicked!

					clickHandler?.Invoke(lllClicked);
				};

			}


			/// <summary>Sets Text and link</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eSetTextAsLink(this ToolStripStatusLabel tsl, string linkText, Action clickAction)
			{
				tsl.Text = linkText;
				tsl.IsLink = true;
				tsl.Click += (_, _) => clickAction.Invoke();
			}

		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_WinForms_Controls_ComboBox
		{


			public static void eDisabeAndShow(this ComboBox cbo, string bannerText)
			{
				cbo.Enabled = false;
				cbo.Items.Clear();
				cbo.Items.Add(bannerText);
				cbo.SelectedIndex = 0;
			}
			public static void eDisabeAndShow(this ComboBox cbo, Exception ex) => cbo.eDisabeAndShow(ex.Message);




			#region FillCBO
			public static void eFillWithObjects(this ComboBox cbo, object[] data, bool enable, bool selectLast = true)
			{
				try
				{
					cbo.BeginUpdate();

					cbo.Items.Clear();
					if (data.Any())
					{
						cbo.Items.AddRange(data);
						if (selectLast)
						{
							cbo.SelectedItem = data.Last();
						}
					}
					cbo.Enabled = enable;
				}
				finally { cbo.EndUpdate(); }
			}




			///<summary>Fills ComboBox with <see cref="ComboboxItemContainer"/> wrappers and returns selected index</summary>
			///<remarks>Sample: cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING)</remarks>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ComboboxItemContainer<T>? eFill<T>(
				this ComboBox cbo,
				IEnumerable<ComboboxItemContainer<T>> items,
				ComboboxItemContainer<T>? selectedItem,
				ComboBoxStyle style = ComboBoxStyle.DropDownList,
				bool notSelectAnyItem = false)
			{
				try
				{
					cbo.BeginUpdate();

					cbo.DataSource = null;
					cbo.Items.Clear();
					cbo.DropDownStyle = style;

					if (!items.Any())
					{
						return null;
					}

					ComboboxItemContainer<T>[] a = items.ToArray();
					cbo.Items.AddRange(a);
					if (notSelectAnyItem)
					{
						return null;
					}

					selectedItem ??= a[0];

					cbo.SelectedItem = selectedItem!;
					return selectedItem;
				}
				finally { cbo.EndUpdate(); }
			}


			///<summary>Fills ComboBox with <see cref="ComboboxItemContainer"/> wrappers and returns selected index</summary>
			///<remarks>Sample: cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING)</remarks>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ComboboxItemContainer<T>? eFill<T>(
				this ComboBox cbo,
				IEnumerable<ComboboxItemContainer<T>> items,
				T? selectedItem = default,
				ComboBoxStyle style = ComboBoxStyle.DropDownList,
				bool notSelectAnyItem = false)
			{
				try
				{
					cbo.BeginUpdate();

					cbo.DataSource = null;
					cbo.Items.Clear();
					cbo.DropDownStyle = style;

					if (!items.Any())
					{
						return null;
					}

					ComboboxItemContainer<T>[] a = items.ToArray();
					cbo.Items.AddRange(a);
					if (notSelectAnyItem)
					{
						return null;
					}

					var containerToSelect = (selectedItem == null)
						? items.FirstOrDefault()
						: items.FirstOrDefault(i => i.Value.eEqualsUniversal(selectedItem));

					containerToSelect ??= items.FirstOrDefault();

					if (containerToSelect != null)
					{
						cbo.SelectedItem = containerToSelect;
					}

					return containerToSelect;
				}
				finally { cbo.EndUpdate(); }
			}


			///<summary>Fills ComboBox with <see cref="ComboboxItemContainer"/> wrappers and returns selected index</summary>
			///<remarks>Sample: cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING)</remarks>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ComboboxItemContainer<T>? eFillWithContainersOf<T>(
				this ComboBox cbo,
				IEnumerable<T> items,
				T? selectedItem = default,
				ComboBoxStyle style = ComboBoxStyle.DropDownList,
				bool notSelectAnyItem = false)
			{

				ComboboxItemContainer<T>? selectedItemContainer = null;

				ComboboxItemContainer<T>[] a = items
					.Select(obj =>
					{
						ComboboxItemContainer<T> cont = new(obj);
						if (selectedItem != null && obj!.Equals(selectedItem))
						{
							selectedItemContainer = cont;
						}

						return cont;
					})
					.ToArray();

				return cbo.eFill<T>(a, selectedItemContainer, style, notSelectAnyItem);
			}


			///<summary>Fills ComboBox with <see cref="EnumContainer"/> wrappers. ONLY FOR ENUM!</summary>
			///<remarks>Sample: cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING.ByCaller)</remarks>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eFillWithContainersOfEnum<T>(this ComboBox cbo, T selectedItem) where T : Enum
				=> cbo.eFill(selectedItem.eGetAllValuesAsEnumContainers(), selectedItem);

			/*
			///<summary>Fills ComboBox with <see cref="EnumContainer"/> wrappers. ONLY FOR ENUM!</summary>
			///<remarks>Sample: cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING.ByCaller)</remarks>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eFillWithContainersOfEnum<T>(this ComboBox cbo, T selectedItem) where T : Enum
			{
				ComboboxItemEnumContainer<T>[] items = selectedItem.eGetAllValuesAsEnumContainers();
				//ComboboxItemEnumContainer<T> sel = items.Where(c => c.Value.Equals(selectedItem)).First();
				cbo.eFill(items, selectedItem);
			}
			 */



			/*
		//////<summary>Заполняет ComboBox лбъектами типа <see cref="My.UOM.EnumTools.EnumContainer"/>, и выбирает текущим элементом EnumItem</summary>
		//////<remarks>Пример: Call Me.cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING.ByCaller)</remarks>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Sub FillCBOWithEnumContainers(Of T)(CBO As ToolStripComboBox, ByVal EnumItem As T)
		//Пример: Call Me.cboGroupFileLogRecordsBy.FillCBOWithEnumContainers(FILE_LOG_RECORDS_GROUPING.ByCaller)

		Call CBO.ComboBox.FillCBOWithEnumContainers(EnumItem)
		End Sub

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function SelectItemWithEnumContainers(Of T)(CBO As ComboBox, EnumItem As T) As Integer
		var I = CBO.ExtEnum_IndexOfEnumContainerForItem(Of T)(EnumItem)
		if(I >= 0) { CBO.SelectedIndex = I
		return I
		End Function


			*/
			#endregion





			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ComboboxItemContainer<T>? eSelectContainerOf<T>(this ComboBox cbo, T? itemToSelect, bool selectFirstItemIfNull = true)
			{
				if (cbo.Items.Count < 1)
				{
					return null;
				}

				var containerToSelect = cbo
					.eItemsAs_ObjectContainerOf<T>()
					.FirstOrDefault(c => c.Value.eEqualsUniversal<T>(itemToSelect));

				if (containerToSelect != null)
				{
					cbo.SelectedItem = containerToSelect;
				}
				else
				{
					if (selectFirstItemIfNull)
					{
						cbo.SelectedIndex = 0;
					}
				}

				return containerToSelect;
			}



			#region Get Items From CBO

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ComboboxItemContainer<T>[] eItemsAs_ObjectContainerOf<T>(this ComboBox cbo)
				=> cbo.Items.Cast<ComboboxItemContainer<T>>().ToArray();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ComboboxItemContainer<T>? eSelectedItemAs_ObjectContainerOf<T>(this ComboBox cbo)
			{
				object? obj = cbo.SelectedItem;
				return (obj == null)
					? null
					: (ComboboxItemContainer<T>)obj!;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T? eSelectedItemAs_ObjectContainerValue<T>(this ComboBox cbo)
			{
				ComboboxItemContainer<T>? ec = cbo.eSelectedItemAs_ObjectContainerOf<T>();
				return (ec == null)
					? default
					: ec.Value;
			}

			/*
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SelectedItemAsEnumContainerValue(Of T)(ByVal CBO As ComboBox) As T
		var objSel = CBO.ExtEnum_SelectedItemAsEnumcontainer(Of T)()
		var V = objSel.Value
		return V
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SelectedItemAsEnumContainerValue(Of T)(CBO As ToolStripComboBox) As T
		return CBO.ComboBox.ExtEnum_SelectedItemAsEnumContainerValue(Of T)
		End Function







		//# Region "CBO EnumContainer Helpers"


		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_ItemsAsEnumContainers(Of T)(CBO As ComboBox) As List(Of My.UOM.EnumTools.EnumContainer(Of T))
		var aWrappers = (From O As Object In CBO.Items
		Let OO = DirectCast(O, My.UOM.EnumTools.EnumContainer(Of T))
		Select OO).ToArray

		return aWrappers.ToList
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_IndexOfEnumContainerForItem(Of T)(CBO As ComboBox, EnumItem As T) As Integer
		var aContainers = CBO.ExtEnum_ItemsAsEnumContainers(Of T)()
		For N = 1 To aContainers.Count
		var iFound = (N - 1)
		if (aContainers(iFound).Value.Equals(EnumItem))
		{
			return iFound
		}
		Next
		return -1
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetEnumContainerForItem(Of T)(CBO As ComboBox, Item As T) As My.UOM.EnumTools.EnumContainer(Of T)
		var aWrappers = CBO.ExtEnum_ItemsAsEnumContainers(Of T)()
		var aFound = (From O In aWrappers Where(O.Value.Equals(Item)))

		if(Not aFound.Any) { return Nothing
		var F = aFound.First
		return F
		End Function


		//# Region "EnumContainer"


		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_ItemAsEnumcontainer(Of T)(ByVal CBO As ComboBox, ItemIndex As Integer) As My.UOM.EnumTools.EnumContainer(Of T)
		var Obj = CBO.Items(ItemIndex)
		var EC As My.UOM.EnumTools.EnumContainer(Of T) = DirectCast(Obj, My.UOM.EnumTools.EnumContainer(Of T))
		return EC
		End Function




		//# End Region





		//# End Region
			 */
			#endregion




			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ComboboxItemEnumContainer<T>[] eGetAllValuesAsEnumContainers<T>(this T anyEnumItem) where T : Enum
			{




				////// <summary>Пример использования: var aContainers = DayOfWeek.Friday.EnumGetAllValuesAsEnumContainers()</summary>
				////// <returns>Возвращает массив EnumContainer(Of T)()</returns>
				////// <remarks>НЕ ИСПОЛЬЗОВАТЬ вот так: typeof(XXX).EnumGetAllValuesAsEnumContainers</remarks>


				//public static ComboboxItemEnumContainer<T>[] eGetAllValuesAsEnumContainers<T>(T EnumItem) where T : Enum

				/*
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ComboboxItemEnumContainer<T>[] eGetAllValuesAsEnumContainers<T>() where T : Enum
		{
		T[] enumValues = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
		ComboboxItemEnumContainer<T>[]? cItems = enumValues.Select(v => new ComboboxItemEnumContainer<T>(v)).ToArray();
		return cItems;

		//***ВОТ ТАК РАБОТАЕТ!
		//Private Enum FILE_LOG_RECORDS_GROUPING As Integer
		//    <DescriptionAttribute("Имя файла")> ByFile
		//    <DescriptionAttribute("GUID")> ByGUID
		//    <DescriptionAttribute("Вызвавший процесс")> ByCaller
		//End Enum
		//var aContainers = FILE_LOG_RECORDS_GROUPING.ByCaller.EnumGetAllValuesAsEnumContainers

		//*** А ВОТ ТАК НЕ БУДЕТ РАБОТАТЬ!!! 
		//var eAA2 = typeof(XXX).EnumGetAllValuesArray
		}
				 */

				T[] enumValues = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
				ComboboxItemEnumContainer<T>[]? cItems = enumValues.Select(v => new ComboboxItemEnumContainer<T>(v)).ToArray();
				return cItems;

				//***ВОТ ТАК РАБОТАЕТ!
				//Private Enum FILE_LOG_RECORDS_GROUPING As Integer
				//    <DescriptionAttribute("Имя файла")> ByFile
				//    <DescriptionAttribute("GUID")> ByGUID
				//    <DescriptionAttribute("Вызвавший процесс")> ByCaller
				//End Enum
				//var aContainers = FILE_LOG_RECORDS_GROUPING.ByCaller.EnumGetAllValuesAsEnumContainers

				//*** А ВОТ ТАК НЕ БУДЕТ РАБОТАТЬ!!! 
				//var eAA2 = typeof(XXX).EnumGetAllValuesArray
			}


		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_WinForms_Controls_ListBox
		{

			///<summary>MT Safe!!!</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void erunOnLockedUpdateMTSafe(this ListBox? lst, Action a)
			{
				Action a2 = delegate
				{
					lst?.BeginUpdate();
					try { a!.Invoke(); }
					finally { lst?.EndUpdate(); }
				};

				if (lst != null && lst.InvokeRequired)
				{
					lst.eRunInUIThread(a2);
				}
				else
				{
					a2.Invoke();
				}
			}

			/// <summary>/Limit count of lines to value</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eLimitRowsCountTo(this ListBox lst, int maxRows)
			{
				lst.erunOnLockedUpdateMTSafe(delegate
				{
					while (lst.Items.Count >= maxRows)
					{
						lst.Items.RemoveAt(0);
					}
				});
			}



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int eLastItemIndex(this ListBox lst) => lst.Items.Count - 1;


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Object eLastItem(this ListBox lst) => lst.Items[lst.eLastItemIndex()];



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Object? eSelectLastRow(this ListBox lst)
			{
				if (lst.Items.Count < 1)
				{
					return null;
				}

				lst.SelectedIndex = (lst.eLastItemIndex());
				return lst.SelectedItem;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eSetLastItem(this ListBox lst, object newValue)
				=> lst.Items[lst.eLastItemIndex()] = newValue;

		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_WinForms_Controls_Listview
		{


			/// <summary>
			/// if (Items.Count > 0) used AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
			/// else used AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAutoSizeColumnsAuto(this ListView? lvw)
			{
				if (lvw == null || lvw.IsDisposed)
				{
					return;
				}

				if (lvw.Items.Count > 0)
				{
					lvw.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				}
				else
				{
					lvw.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<ListViewGroup> eAddGroups(this ListView? lvw, params ListViewGroup[] gg)
			{
				lvw?.Groups.AddRange(gg);
				return gg;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<ListViewGroup> eAddGroups(this ListView? lvw, IEnumerable<ListViewGroup> gg)
			{
				ListViewGroup[] groups = [.. gg];
				return lvw.eAddGroups(groups);
			}



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<ListViewItem> eAddItems(this ListView? lvw, params ListViewItem[] ll)
			{
				lvw?.erunOnLockedUpdate(() => lvw?.Items.AddRange(ll));
				return ll;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<ListViewItem> eAddItems(this ListView? lvw, IEnumerable<ListViewItem> ll, bool autoSizeColumns = false)
			{
				ListViewItem[] groups = [.. ll];
				lvw?.erunOnLockedUpdate(delegate
				{
					lvw?.Items.AddRange(groups);
				}, true);

				return ll;
			}




			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eClear(this ListView? lvw, bool autoSizeColumns = true, bool clearItems = true, bool clearGroups = true, bool clearColumns = false)
			{
				lvw?.erunOnLockedUpdate(delegate
				{
					if (clearItems)
					{
						lvw?.Items.Clear();
					}

					if (clearGroups)
					{
						lvw?.Groups.Clear();
					}

					if (clearColumns)
					{
						lvw?.Columns.Clear();
					}
				}, autoSizeColumns);
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eClearItems(this ListView lvw, bool autoSizeColumns = false)
				=> lvw.eClear(autoSizeColumns, clearItems: true);



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eAddRangeEx(
				this ListView? lvw,
				ListViewItem[] items,
				bool autoSizeColumns = true,
				bool clearItemsBefore = false,
				bool clearGroupsBefore = false,
				bool lockedUpdate = true
				)
			{

				Action a = new(() =>
				{
					if (clearItemsBefore)
					{
						lvw?.Items.Clear();
					}

					if (clearGroupsBefore)
					{
						lvw?.Groups.Clear();
					}

					if (items.Any())
					{
						lvw?.Items.AddRange(items);
					}
				});

				if (lockedUpdate)
				{
					lvw?.erunOnLockedUpdate(a, autoSizeColumns);
				}
				else
				{
					a.Invoke();
					lvw?.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eAddRangeEx(
				this ListView? lvw,
				IEnumerable<ListViewItem> items,
				bool autoSizeColumns = true,
				bool clearItemsBefore = false,
				bool clearGroupsBefore = false,
				bool lockedUpdate = true
				)
				=> lvw?.eAddRangeEx([.. items], autoSizeColumns, clearItemsBefore, clearGroupsBefore, lockedUpdate);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAddFakeSubitems(this ListViewItem li, int ListViewColumnsCount, string fakeText = "")
			{
				if (li != null)
				{
					for (int i = 0 ; i < ListViewColumnsCount ; i++)
					{
						li.SubItems.Add(fakeText);
					}
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eAddFakeSubitems(
				this ListViewItem? li,
				ListView? lvw = null,
				string fakeText = "")
			{
				if (li == null)
				{
					return;
				}

				lvw ??= li.ListView;
				lvw.ThrowIfNull();//ArgumentNullException.ThrowIfNull(lvw);
				li?.eAddFakeSubitems(lvw!.Columns.Count, fakeText);
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eUpdateTexts(
				 this ListViewItem? li,
				 int startIndex,
				 params string?[] aSubItemsText)
			{
				if (li == null || !aSubItemsText.Any())
				{
					return;
				}

				li.ListView.erunOnLockedUpdate(() =>
				{
					int i = 0;
					aSubItemsText.eForEach(text =>
					{
						if (null != text)
						{
							li.SubItems[(startIndex + i)].Text = text;
						}

						i++;
					});
				});
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eUpdateTexts(this ListViewItem? li, params string[] aSubItemsText)
				=> eUpdateTexts(li, 0, aSubItemsText);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewGroup> eGroupsAsIEnumerable(this ListView lvw)
				=> lvw.Groups.Cast<ListViewGroup>();


#if NETCOREAPP3_0_OR_GREATER

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static (ListViewGroup Group, bool Created) eGroupsCreateGroupByKey(
				this ListView lvw,
				string key,
				string? header = null,
				Action<ListViewGroup>? onNewGroup = null,
				ListViewGroupCollapsedState newGroupState = ListViewGroupCollapsedState.Collapsed
				)
			{
				ListViewGroup? grp = lvw.eGroupsAsIEnumerable()?.FirstOrDefault(g => (g.Name == key));
				bool exist = (grp != null);
				if (!exist)
				{
					grp = new ListViewGroup(key, header ?? key);
					lvw.Groups.Add(grp);
					onNewGroup?.Invoke(grp);
					grp.CollapsedState = newGroupState;
				}
				return (grp!, !exist);
			}
#else
			// Defined in ListViewEx extensions...

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static (ListViewGroup Group, bool Created) eGroupsCreateGroupByKey(
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


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static (ListViewGroup Group, bool Created) eGroupsCreateGroupByKey(
				Dictionary<string, ListViewGroup> dicGroups,
				string key,
				string? header = null,
				Action<ListViewGroup>? onNewGroup = null)
			{
				bool exist = dicGroups.TryGetValue(key, out ListViewGroup? grp);
				if (!exist)
				{
					header ??= key;
					grp = new ListViewGroup(key, header);
					dicGroups.Add(key, grp);
					onNewGroup?.Invoke(grp);
				}
				return (grp!, !exist);
			}


			/*



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
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


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eSetGroupsTitlesFast(
				this ListView? lvw,
				Func<ListViewGroup, string>? getGroupHeader = null)
					=> lvw?.eGroupsAsIEnumerable().eForEach(g =>
					{
						string sTitle = g.Name ?? "";
						if (getGroupHeader != null)
						{
							sTitle = getGroupHeader.Invoke(g);
						}
						else
						{
							sTitle = $"{sTitle} ({g.Items.Count:N0})".Trim();
						}

						if (!string.IsNullOrWhiteSpace(sTitle))
						{
							g.Header = sTitle;
						}
					});

#if NETCOREAPP3_0_OR_GREATER


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eSetAllGroupsState(
				this ListView? lvw,
				ListViewGroupCollapsedState state = ListViewGroupCollapsedState.Collapsed)
					=> lvw?.eGroupsAsIEnumerable().eForEach(g => g.CollapsedState = state);

#endif

			///<summary>MT Safe!!!</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void erunOnLockedUpdate(
				this ListView? lvw,
				Action a,
				bool autoSizeColumns = false,
				bool fastUpdateGroupHeaders = false)
			{
				_ = a ?? throw new ArgumentNullException(nameof(a));

				Action a2 = delegate
				{
					lvw?.BeginUpdate();
					try { a!.Invoke(); }
					finally
					{
						if (autoSizeColumns)
						{
							lvw?.eAutoSizeColumnsAuto();
						}

						if (fastUpdateGroupHeaders)
						{
							lvw?.eSetGroupsTitlesFast();
						}

						lvw?.EndUpdate();
					}
				};

				if (lvw != null && lvw.InvokeRequired)
				{
					lvw.eRunInUIThread(a2);
				}
				else
				{
					a2.Invoke();
				}
			}

			///<summary>MT Safe!!!</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static async Task erunOnLockedUpdateAsync(
				this ListView? lvw,
				Func<Task> a,
				bool autoSizeColumns = false,
				bool fastUpdateGroupHeaders = false)
			{
				_ = a ?? throw new ArgumentNullException(nameof(a));

				async Task SafeUpdateCore()
				{
					lvw?.BeginUpdate();
					try
					{
						await a!.Invoke();
					}
					finally
					{
						if (autoSizeColumns)
						{
							lvw?.eAutoSizeColumnsAuto();
						}

						if (fastUpdateGroupHeaders)
						{
							lvw?.eSetGroupsTitlesFast();
						}

						lvw?.EndUpdate();
					}
				}

				if (lvw != null && lvw.InvokeRequired)
				{
					await lvw.eRunInUIThreadAsync(SafeUpdateCore)!;
				}
				else
				{
					await SafeUpdateCore();
				}
			}


			///<summary>MT Safe!!!</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T? erunOnLockedUpdate<T>(
				this ListView? lvw,
				Func<T> a,
				bool autoSizeColumns = false,
				bool fastUpdateGroupHeaders = false)
			{
				_ = a ?? throw new ArgumentNullException(nameof(a));

				T? SafeUpdateCore()
				{
					lvw?.BeginUpdate();
					try { return a!.Invoke(); }
					finally
					{
						if (autoSizeColumns)
						{
							lvw?.eAutoSizeColumnsAuto();
						}

						if (fastUpdateGroupHeaders)
						{
							lvw?.eSetGroupsTitlesFast();
						}

						lvw?.EndUpdate();
					}
				}

				if (lvw != null && lvw.InvokeRequired)
				{
					return lvw.eRunInUIThread(SafeUpdateCore);
				}

				return SafeUpdateCore();
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static async Task<T?> erunOnLockedUpdateAsync<T>(
				this ListView lvw,
				Func<Task<T?>> tsk,
				bool autoSizeColumns = false,
				bool fastUpdateGroupHeaders = false) where T : class
			{
				//ArgumentNullException.ThrowIfNull(tsk);
				tsk.ThrowIfNull();// = tsk ?? throw new ArgumentNullException(nameof(tsk));

				lvw?.BeginUpdate();
				try
				{
					//tsk.Start();
					T? ret = await tsk.Invoke();
					return ret;
				}
				finally
				{
					if (autoSizeColumns)
					{
						lvw?.eAutoSizeColumnsAuto();
					}

					if (fastUpdateGroupHeaders)
					{
						lvw?.eSetGroupsTitlesFast();
					}

					lvw?.EndUpdate();
				}
			}

			/*

			[ MethodImpl(MethodImplOptions.AggressiveInlining)]
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


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eSelectFirstItem(this ListView lvw)
			{
				var first = lvw.eItemsAsIEnumerable().FirstOrDefault();
				if (first == default)
				{
					return;
				}

				first.Selected = true;
				first.Focused = true;
				first.EnsureVisible();
			}


			public static void eAddSubitems(this ListViewItem? li, params string[] subitems)
				=> subitems?.eForEach(s => li?.SubItems.Add(s));


			public static async void eFlashAsync(this ListViewItem? li, int flashCount = 10)
			{
				if (li == null)
				{
					return;
				}

				Color clrFore = li.ForeColor; Color clrBack = li.BackColor;
				try
				{
					for (int i = 1, loopTo = flashCount * 2 ; i <= loopTo ; i++)
					{
						(li.BackColor, li.ForeColor) = (li.ForeColor, li.BackColor);
						await Task.Delay(100);
					}
				}
				finally //Restore original colors
				{ li.ForeColor = clrFore; li.BackColor = clrBack; }
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eItemsRemoveRange(
				this ListView? lvw,
				IEnumerable<ListViewItem> ItemsToRemove,
				bool aAutoSizeColumnsAtFinish = false)
				=> lvw?.erunOnLockedUpdate(() => lvw?.Items.eItemsRemoveRange(ItemsToRemove), aAutoSizeColumnsAtFinish);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eItemsRemoveRange(
				this ListView.ListViewItemCollection liC,
				IEnumerable<ListViewItem> ItemsToRemove)
					=> ItemsToRemove.eForEach(li => liC.Remove(li));


			#region ItemsAsIEnumerable

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItem> eItemsAsIEnumerable(this ListView lvw) => lvw.Items.Cast<ListViewItem>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItem> eItemsAsIEnumerable(this ListViewGroup G) => G.Items.Cast<ListViewItem>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItem> eSelectedItemsAsIEnumerable(this ListView lvw) => lvw.SelectedItems.Cast<ListViewItem>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItem> eCheckedItemsAsIEnumerable(this ListView lvw) => lvw.CheckedItems.Cast<ListViewItem>();

			#endregion


			#region ItemsAndTags


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T? eTagAs<T>(this ListViewGroup lvg) => (T?)lvg.Tag;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T? eTagAs<T>(this ListViewItem li) => (T?)li.Tag;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T? eTagAs<T>(this TreeNode nd) => (T?)nd.Tag;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T? eTagAs<T>(this Control ctl) => (T?)ctl.Tag;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int eItemsCount_Selected(this ListView lvw) => lvw.SelectedItems.Count;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int eItemsCount_Checked(this ListView lvw) => lvw.CheckedItems.Count;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int eItemsCount(this ListView lvw) => lvw.Items.Count;


			#region ItemsAndTags2

			public sealed class ListViewItemAndTag<T>
			{
				public ListViewItem Item { get; set; }

				public ListViewItemAndTag(ListViewItem li) : base() { Item = li; }

				public T? Tag => Item.eTagAs<T>();
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T?> eTags<T>(this IEnumerable<ListViewItemAndTag<T>> A)
				=> A.Select(li => li.Tag);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItem> eItems<T>(this IEnumerable<ListViewItemAndTag<T>> A)
				=> A.Select(li => li.Item);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItemAndTag<T>> eItemsAndTags<T>(this IEnumerable<ListViewItem> A)
				=> A.Select(li => new ListViewItemAndTag<T>(li));

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItemAndTag<T>> eItemsAndTags<T>(this ListViewGroup G)
				=> eItemsAndTags<T>(G.eItemsAsIEnumerable());

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItemAndTag<T>> eItemsAndTags<T>(this ListView lvw)
				=> eItemsAndTags<T>(lvw.eItemsAsIEnumerable());

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItemAndTag<T>> eSelectedItemsAndTags<T>(this ListView lvw)
				=> eItemsAndTags<T>(lvw.eSelectedItemsAsIEnumerable());

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItemAndTag<T>> eCheckedItemsAndTags<T>(this ListView lvw)
				=> eItemsAndTags<T>(lvw.eCheckedItemsAsIEnumerable());


			#endregion


			#region ItemsAs


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eItemsAs<T>(this ListViewGroup lvg) where T : ListViewItem => lvg.Items.Cast<T>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eItemsAs<T>(this ListView lvw) where T : ListViewItem => lvw.Items.Cast<T>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eSelectedItemsAs<T>(this ListView lvw) where T : ListViewItem => lvw.SelectedItems.Cast<T>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eSelectedItems<T>(this IEnumerable<T> rows) where T : ListViewItem => rows.Where(li => li.Selected);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eSelectedItemsAs<T>(this ListView.ListViewItemCollection rows) where T : ListViewItem => rows.Cast<T>().eSelectedItems();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItem> eSelectedItems(this ListViewGroup g) => g.Items.eSelectedItemsAs<ListViewItem>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eSelectedItemsAs<T>(this ListViewGroup g) where T : ListViewItem => g.Items.eSelectedItemsAs<T>();


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eCheckedItemsAs<T>(this ListView lvw) where T : ListViewItem => lvw.CheckedItems.Cast<T>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eCheckedItems<T>(this IEnumerable<T> rows) where T : ListViewItem => rows.Where(li => li.Checked);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eCheckedItemsAs<T>(this ListView.ListViewItemCollection rows) where T : ListViewItem => rows.Cast<T>().eCheckedItems();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ListViewItem> eCheckedItems(this ListViewGroup g) => g.Items.eCheckedItemsAs<ListViewItem>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eCheckedItemsAs<T>(this ListViewGroup g) where T : ListViewItem => g.Items.eCheckedItemsAs<T>();

			#endregion

			#endregion

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<ColumnHeader> eColumnsAsIEnumerable(this ListView lvw) => lvw.Columns.Cast<ColumnHeader>();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ListViewGroup? eGroupsGetByKey(this ListView lvw, string? key)
				=> lvw.eGroupsAsIEnumerable().FirstOrDefault(grp => (grp.Name ?? "") == (key ?? ""));


			/// <summary>Предыдущий элемент (Index меньше на 1)</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ListViewItem? ePrevious(this ListViewItem li)
				=> (li.Index <= 0)
				? null
				: li.ListView!.Items[li.Index - 1];

			/// <summary>Предыдущий элемент в той же группе (Index меньше на 1)</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ListViewItem? ePreviousInGroup(this ListViewItem li)
			{
				var liPrev = li.ePrevious();
				if (liPrev != null && object.ReferenceEquals(liPrev.Group, li.Group))
				{
					return liPrev;
				}

				return null;
			}


			/// <summary>Next элемент (Index +1)</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ListViewItem? eNext(this ListViewItem li)
				=> (li.Index >= (li.ListView!.Items.Count - 1))
					? null
					: li.ListView.Items[li.Index + 1];


			/// <summary>Next элемент в той же группе (Index +1)</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ListViewItem? eNextInGroup(this ListViewItem li)
			{
				var liNext = li.eNext();
				if (liNext != null && object.ReferenceEquals(liNext.Group, li.Group))
				{
					return liNext;
				}

				return null;
			}



			/// <summary>Counts distance from cursor to the nearest item in the list.</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static (ListViewItem? Item, Size? VectorToItemCenter, Double? Distance) eGetNearestItem(this ListView lvw, Point pt)
			{
				if (lvw.Items.Count < 1)
				{
					return (null, null, null);
				}

				var nearest = lvw.Items
							.Cast<ListViewItem>()
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


			/// <summary>Counts distance from cursor to the nearest item in the list.</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eActivate(this ListViewItem li, bool deactivateOther = true)
			{
				ListView? lvw = li.ListView as ListView;

				lvw?.erunOnLockedUpdate(delegate
				{
					if (deactivateOther)
					{
						lvw?.eItemsAsIEnumerable().ToList().ForEach(li => li.Selected = false);
					}

					li.Selected = true;
					li.Focused = true;
					li.EnsureVisible();
				});
			}
		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_WinForms_Controls_ImageList
		{

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void SetKeyNames(this ImageList iml, int startIndex, params string[] keys)
			{
				for (int i = 0 ; i < keys.Length ; i++)
				{
					iml.Images.SetKeyName(startIndex + i, keys[i]);
				}
			}

		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_WinForms_Controls_ProgressBar
		{
			internal enum PBM_STATES : int
			{
				PBST_NORMAL = 1,
				PBST_ERROR = 2,
				PBST_PAUSE = 3
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eSetState(this ProgressBar pb, PBM_STATES state)
			{
				const int PBM_SETSTATE = 0x400 + 16;
				_ = uom.WinAPI.windows.SendMessage(pb.Handle, PBM_SETSTATE, (int)state, 0);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eSetValues(
				this ProgressBar pb,
				int iMin = 0,
				int iMax = 100,
				int iValue = 0)
			{
				pb.SuspendLayout();
				try
				{
					pb.Minimum = iMin;
					pb.Maximum = iMax;
					pb.Value = iValue;
				}
				finally
				{
					pb.ResumeLayout();
					pb.Update();
				}
			}
		}


		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_WinForms_Controls_CommonDislogs
		{


			public const string C_DEFAULT_XML_EXT = "xml";
			public const string C_DEFAULT_XML_EXPORT_FILTER = "XML-files|*." + C_DEFAULT_XML_EXT;


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eInitDefault(this SaveFileDialog sfd, string defaultExt, string filter)
			{
				sfd.ShowHelp = false;
				sfd.AddExtension = true;
				sfd.AutoUpgradeEnabled = true;
				sfd.CheckPathExists = true;
				sfd.DereferenceLinks = true;
				sfd.DefaultExt = defaultExt;
				sfd.SupportMultiDottedExtensions = true;
				sfd.ValidateNames = true;
				sfd.Filter = filter;
				sfd.OverwritePrompt = true;
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eInitDefault(this OpenFileDialog ofd, string defaultExt, string filter, bool Multiselect = false)
			{
				ofd.ShowHelp = false;
				ofd.ShowReadOnly = false;

				ofd.AutoUpgradeEnabled = true;

				ofd.AddExtension = true;
				ofd.CheckPathExists = true;
				ofd.CheckFileExists = true;
				ofd.DereferenceLinks = true;
				ofd.SupportMultiDottedExtensions = true;
				ofd.ValidateNames = true;
				ofd.Multiselect = Multiselect;

				ofd.Filter = filter;
				if (defaultExt.eIsNotNullOrWhiteSpace())
				{
					ofd.DefaultExt = defaultExt;
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string[] eOpenLoadFilesDialog(
				this Form owner,
				string defaultExt = "",
				string filter = C_DEFAULT_XML_EXPORT_FILTER,
				bool multiselect = true,
				string initialFile = "",
				string initialDirectory = "")
			{
				using OpenFileDialog ofd = new();
				ofd.eInitDefault(defaultExt, filter, multiselect);

				ofd.Filter = filter;

				if (initialFile.eIsNotNullOrWhiteSpace())
				{
					ofd.FileName = initialFile;
				}

				if (initialDirectory.eIsNotNullOrWhiteSpace())
				{
					ofd.InitialDirectory = initialDirectory;
				}

				return (ofd.ShowDialog(owner) != DialogResult.OK)
					? Array.Empty<string>()
					: ofd.FileNames;
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string eOpenLoadFileDialog(
				this Form owner,
				string defaultExt = "",
				string filter = C_DEFAULT_XML_EXPORT_FILTER,
				string initialFile = "",
				string initialDirectory = "")
				=> eOpenLoadFilesDialog(owner,
					defaultExt,
					filter,
					false,
					initialFile,
					initialDirectory).FirstOrDefault() ?? string.Empty;



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string eOpenLoadFileDialog(
				this Form owner,
				string defaultExt = "",
				string filter = C_DEFAULT_XML_EXPORT_FILTER,
				string initialFile = "",
				Environment.SpecialFolder initialDirectory = Environment.SpecialFolder.DesktopDirectory)
				=> eOpenLoadFileDialog(owner,
					defaultExt,
					filter,
					initialFile,
					System.Environment.GetFolderPath(initialDirectory));



			/*









					<DebuggerNonUserCode, DebuggerStepThrough>
					<MethodImpl(MethodImplOptions.AggressiveInlining), Extension()>
					Public Function eOpenSaveFileDialog(ParentForm As Form,
															  Optional sDefaultFileName As String = vbNullString,
															  Optional sDefaultExt As String = C_DEFAULT_XML_EXT,
															  Optional sFilter As String = C_DEFAULT_XML_EXPORT_FILTER,
															  Optional sAutoFileNameSuffix As String = "Данные от",
															  Optional neInitialDirectory As Nullable(Of Environment.SpecialFolder) = Environment.SpecialFolder.DesktopDirectory) As String
						Using SFD As New SaveFileDialog
							With SFD
								If (sDefaultFileName.eIsNullOrWhiteSpace) Then
									sDefaultFileName = Now.ToFileName
									If (sAutoFileNameSuffix.eIsNotNullOrWhiteSpace) Then sDefaultFileName = sAutoFileNameSuffix & " " & sDefaultFileName
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













		}




		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static partial class Extensions_WinForms_Controls_Menu
		{


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static ToolStripMenuItem? eSearchMenuItemTree(this IEnumerable<ToolStripItem> emi, Func<ToolStripMenuItem, bool> predicate)
			{
				foreach (ToolStripItem tsi in emi)
				{
					switch (tsi)
					{
						case ToolStripMenuItem mi:
							{
								if (predicate(mi))
								{
									return mi;
								}

								IEnumerable<ToolStripItem> childItems = mi.DropDownItems.Cast<ToolStripItem>();
								if (childItems.Any())
								{
									ToolStripMenuItem? miChild = eSearchMenuItemTree(childItems, predicate);
									if (miChild != null)
									{
										return miChild;
									}
								}
								break;
							}

						default: break;
					}
				}

				return null;
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static ToolStripMenuItem? eSearchMenuItemTree(this ToolStripMenuItem mi, Func<ToolStripMenuItem, bool> predicate)
				=> mi.DropDownItems.Cast<ToolStripItem>().eSearchMenuItemTree(predicate);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static ToolStripMenuItem? eSearchMenuItemTree(this ContextMenuStrip cm, Func<ToolStripMenuItem, bool> predicate)
				=> cm.Items.Cast<ToolStripItem>().eSearchMenuItemTree(predicate);




			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eRemove(this ToolStripMenuItem mi)
			{
				ToolStripItem? o = mi.OwnerItem;
				if (o == null)
				{
					return;
				}

				switch (o)
				{
					case ToolStripMenuItem tsmi:
						{
							tsmi.DropDownItems.Remove(mi);
							break;
						}
					default: throw new ArgumentException($"Unknown {o.GetType()}");
				}
			}



		}



		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static class Extensions_WinForms_Clipboard
		{

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eSetClipboardSafe(this string text)
			{
				try { Clipboard.SetText(text); }
				catch { }
			}
		}


		internal static class Extensions_WinForms_Drawing
		{


			/// <summary>Расчёт точки вывода текста относительно заданной точки</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static (PointF TextPos, SizeF MeasuredTextSize) eMeasureStringLocation(
				this Graphics g,
				string Text,
				Font Font,
				PointF TargetPoint,
				System.Drawing.ContentAlignment alignment)
			{
				SizeF retMeasuredTextSize = g.MeasureString(Text, Font);
				var szfMeasuredTextSize2 = new SizeF(retMeasuredTextSize.Width / 2, retMeasuredTextSize.Height / 2);
				var rcLabel = default(PointF);

				switch (alignment)
				{
					case System.Drawing.ContentAlignment.MiddleLeft: // Слева посередине
						rcLabel = new PointF(TargetPoint.X - retMeasuredTextSize.Width, TargetPoint.Y - szfMeasuredTextSize2.Height); break;

					case System.Drawing.ContentAlignment.BottomCenter: // Снизу по центру
						rcLabel = new PointF(TargetPoint.X - szfMeasuredTextSize2.Width, TargetPoint.Y); break;

					case System.Drawing.ContentAlignment.TopCenter: // Сверху по центру
						rcLabel = new PointF(TargetPoint.X - szfMeasuredTextSize2.Width, TargetPoint.Y - retMeasuredTextSize.Height); break;

					case System.Drawing.ContentAlignment.MiddleCenter:  // Центр
						rcLabel = new PointF(TargetPoint.X - szfMeasuredTextSize2.Width, TargetPoint.Y - szfMeasuredTextSize2.Height); break;

					default:
						throw new ArgumentOutOfRangeException(nameof(alignment));
				}
				return (rcLabel, retMeasuredTextSize);
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Icon eCreate_For(this Icon rIcon, System.Drawing.Size TargetSize) => new(rIcon, TargetSize);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Icon eCreate_For(this Icon rIcon, int iTargetSize) => rIcon.eCreate_For(new System.Drawing.Size(iTargetSize, iTargetSize));


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Icon eCreate_Small(this Icon rIcon) => rIcon.eCreate_For(System.Windows.Forms.SystemInformation.SmallIconSize);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Icon eCreate_Large(this Icon rIcon) => rIcon.eCreate_For(System.Windows.Forms.SystemInformation.IconSize);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eDrawRectangle(this Graphics g, Pen P, RectangleF RCF) => g.DrawRectangle(P, RCF.Left, RCF.Top, RCF.Width, RCF.Height);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eDrawRoundedRectangle(
				this Graphics g,
				Rectangle rc,
				int radius,
				Pen? pen = null,
				Brush? brush = null)
			{
				Rectangle corner = new(rc.X, rc.Y, radius, radius);

				using System.Drawing.Drawing2D.GraphicsPath path = new();
				path.AddArc(corner, 180, 90);
				corner.X = rc.X + rc.Width - radius;
				path.AddArc(corner, 270, 90);
				corner.Y = rc.Y + rc.Height - radius;
				path.AddArc(corner, 0, 90);
				corner.X = rc.X;
				path.AddArc(corner, 90, 90);
				path.CloseFigure();

				if (brush != null)
				{
					g.FillPath(brush, path);
				}

				if (pen != null)
				{
					g.DrawPath(pen, path);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eDrawRoundedRectangle(
				this Graphics g,
				RectangleF rc,
				float radius,
				Pen? pen = null,
				Brush? brush = null)
			{
				RectangleF corner = new(rc.X, rc.Y, radius, radius);

				using System.Drawing.Drawing2D.GraphicsPath path = new();
				path.AddArc(corner, 180, 90);
				corner.X = rc.X + rc.Width - radius;
				path.AddArc(corner, 270, 90);
				corner.Y = rc.Y + rc.Height - radius;
				path.AddArc(corner, 0, 90);
				corner.X = rc.X;
				path.AddArc(corner, 90, 90);
				path.CloseFigure();

				if (brush != null)
				{
					g.FillPath(brush, path);
				}

				if (pen != null)
				{
					g.DrawPath(pen, path);
				}
			}





			#region System.Drawing.ContentAlignment->System.Drawing.StringFormat, System.Windows.Forms.HorizontalAlignment->System.Drawing.StringAlignment


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.StringFormat eToDrawingStringFormat(
				this System.Drawing.ContentAlignment CA,
				StringFormatFlags FormatFlags = 0)
			{
				StringAlignment VA = StringAlignment.Center;
				StringAlignment HA = StringAlignment.Center;
				switch (CA)
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
					Alignment = HA,
					LineAlignment = VA,
					FormatFlags = FormatFlags
				};
				return SF;
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.StringFormat eToDrawingStringFormat(
				this HorizontalAlignment A,
				StringAlignment LineAlignment = StringAlignment.Center,
				StringFormatFlags FormatFlags = 0)
			{
				var SF = new StringFormat();
				switch (A)
				{
					case HorizontalAlignment.Center: SF.Alignment = StringAlignment.Center; break;
					case HorizontalAlignment.Right: SF.Alignment = StringAlignment.Far; break;
				}

				SF.LineAlignment = LineAlignment;
				SF.FormatFlags = FormatFlags;
				return SF;
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Windows.Forms.TextFormatFlags eToTextFormatFlags(this System.Drawing.StringFormat SF)
			{
				TextFormatFlags FF = 0;
				switch (SF.Alignment)
				{
					case StringAlignment.Near: FF |= TextFormatFlags.Left; break;
					case StringAlignment.Center: FF |= TextFormatFlags.HorizontalCenter; break;
					case StringAlignment.Far: FF |= TextFormatFlags.Right; break;
				}
				switch (SF.LineAlignment)
				{
					case StringAlignment.Near: FF |= TextFormatFlags.Top; break;
					case StringAlignment.Center: FF |= TextFormatFlags.VerticalCenter; break;
					case StringAlignment.Far: FF |= TextFormatFlags.Bottom; break;
				}
				return FF;
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.StringAlignment eToDrawingStringAlignment(this System.Windows.Forms.HorizontalAlignment Align)
				=> Align switch
				{
					HorizontalAlignment.Center => StringAlignment.Center,
					HorizontalAlignment.Left => StringAlignment.Near,
					HorizontalAlignment.Right => StringAlignment.Far,
					_ => StringAlignment.Near,
				};

			#endregion


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eDrawShadow(this GraphicsPath gp, int intensity, int radius, Bitmap dest)
			{
				using Graphics g = Graphics.FromImage(dest);
				g.Clear(Color.Transparent);
				g.CompositingMode = CompositingMode.SourceCopy;
				double alpha = 0;
				double astep = 0;
				double astepstep = (double)intensity / radius / (radius / 2D);
				for (int thickness = radius ; thickness > 0 ; thickness--)
				{
					using (Pen p = new Pen(Color.FromArgb((int)alpha, 0, 0, 0), thickness))
					{
						p.LineJoin = LineJoin.Round;
						g.DrawPath(p, gp);
					}
					alpha += astep;
					astep += astepstep;
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eDrawPathShadow(this Graphics g, GraphicsPath gp, int radius, int intensity = 100)
			{
				intensity = intensity.eCheckRange(1, 100);
				double alpha = 0;
				double astep = 0;
				double astepstep = (double)intensity / radius / (radius / 2D);
				for (int thickness = radius ; thickness > 0 ; thickness--)
				{
					using (Pen p = new Pen(Color.FromArgb((int)alpha, 0, 0, 0), thickness))
					{
						p.LineJoin = LineJoin.Round;
						g.DrawPath(p, gp);
					}
					alpha += astep;
					astep += astepstep;
				}
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string eGetExtensionForRawFormat(this Bitmap IMG)
				=> IMG.RawFormat.eGetExtension();




			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string eGetExtension(this System.Drawing.Imaging.ImageFormat fmt)
			{
				string sExt;
				if (fmt.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
				{
					sExt = "jpg";
				}
				else if (fmt.Equals(System.Drawing.Imaging.ImageFormat.Png))
				{
					sExt = "png";
				}
				else if (fmt.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
				{
					sExt = "Bmp";
				}
				else if (fmt.Equals(System.Drawing.Imaging.ImageFormat.Emf))
				{
					sExt = "Emf";
				}
				else if (fmt.Equals(System.Drawing.Imaging.ImageFormat.Exif))
				{
					sExt = "Exif";
				}
				else if (fmt.Equals(System.Drawing.Imaging.ImageFormat.Gif))
				{
					sExt = "Gif";
				}
				else if (fmt.Equals(System.Drawing.Imaging.ImageFormat.Icon))
				{
					sExt = "Ico";
				}
				else if (fmt.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
				{
					sExt = "Tif";
				}
				else if (fmt.Equals(System.Drawing.Imaging.ImageFormat.Wmf))
				{
					sExt = "Wmf";
				}
				else
				{
					throw new Exception($"Unknown image format: {fmt}");
				}

				return "." + sExt.ToLower();
			}






			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Icon eCreateIcon(this Image IMG, System.Drawing.Size IconSize)
			{
				var bmSmall = (Bitmap)IMG.GetThumbnailImage(IconSize.Width, IconSize.Height, default, default);
				return Icon.FromHandle(bmSmall.GetHicon());
			}







#if NETFRAMEWORK
			[ MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string? eExtractAssociatedIcon(
				this FileInfo App,
				ImageList IML,
				bool TryLoadFileAsImage = false) => App!.FullName!.eExtractAssociatedIcon(IML, TryLoadFileAsImage);


			[ MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string? eExtractAssociatedIcon(this string sPath, ImageList IML, bool TryLoadFileAsImage = false)
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



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.Icon? eExtractAssociatedIcon(this string file)
				=> System.Drawing.Icon.ExtractAssociatedIcon(file);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.Icon? eExtractAssociatedIcon(this FileSystemInfo FI)
				=> FI.FullName.eExtractAssociatedIcon();














			/// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение.
			/// Уменьшает большие картинки
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Bitmap eGetThumbnailBitmap(this System.Drawing.Icon rIcon, System.Drawing.Size szCanvas)
				=> rIcon.ToBitmap().eGetThumbnailBitmap(szCanvas, System.Windows.Forms.SystemInformation.IconSize, true);

			/// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение.
			/// Уменьшает большие картинки
			/// </summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Bitmap eGetThumbnailBitmap(this Icon rIcon, ImageList rImageList)
			{
				if (null == rImageList)
				{
					throw new ArgumentNullException(nameof(rImageList));
				}

				return rIcon.eGetThumbnailBitmap(rImageList.ImageSize);
			}



			/// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение. Уменьшает большие картинки</summary>
			/// <param name="CanvasSize">Выходной размер получаемого изображения</param>
			/// <param name="DrawImageSize">Размер рисуемого изображения на холсте. Если не указан, автоматически маленькие как есть, а большие уменьшались</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Bitmap eGetThumbnailBitmap(this Icon rIcon, System.Drawing.Size CanvasSize, System.Drawing.Size? DrawImageSize = default, bool UpScaleSmallImgaes = false)
				=> eGetThumbnailBitmap_CORE(rIcon.Size,
					(G, RC) => G.DrawIcon(rIcon, RC),
					CanvasSize, DrawImageSize, UpScaleSmallImgaes);

			/// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение. Уменьшает большие картинки</summary>
			/// <param name="CanvasSize">Выходной размер получаемого изображения</param>
			/// <param name="DrawImageSize">Размер рисуемого изображения на холсте. Если не указан, автоматически маленькие как есть, а большие уменьшались</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Bitmap eGetThumbnailBitmap(this Image rBitmap, System.Drawing.Size CanvasSize, System.Drawing.Size? DrawImageSize = default, bool UpScaleSmallImgaes = false)
				=> eGetThumbnailBitmap_CORE(rBitmap.Size,
					(G, RC) => G.DrawImage(rBitmap, RC),
					CanvasSize, DrawImageSize, UpScaleSmallImgaes);

			/// <summary>Создаёт прозрачный холст с заданными размерами, и рисует заданное изображение.
			/// Уменьшает большие картинки</summary>
			/// <param name="CanvasSize">Размер холста</param>
			/// <param name="DrawImageSize">Размер рисуемого изображения на холсте. Если не указан, автоматически маленькие как есть, а большие уменьшались</param>
			private static Bitmap eGetThumbnailBitmap_CORE(
				System.Drawing.Size ImageSize,
				Action<Graphics, System.Drawing.Rectangle> DrawImageProc,
				System.Drawing.Size CanvasSize,
				System.Drawing.Size? DrawImageSize = null,
				bool UpScaleSmallImgaes = false)
			{
				var szfCanvas = CanvasSize.eToSizeF();
				Bitmap bmCanvas = new(CanvasSize.Width, CanvasSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				using Graphics g = Graphics.FromImage(bmCanvas);
				g.PageUnit = GraphicsUnit.Pixel;
				g.Clear(Color.Transparent);
				// Call G.DrawRectangle(Pens.Blue, rcFileBitmap)

				var szfDraw = (DrawImageSize ?? ImageSize).eToSizeF();
				if (szfDraw.Width > szfCanvas.Width || szfDraw.Height > szfCanvas.Height)
				{
					// Выводимое изображение превышает размеры холста, надо вписать
					szfDraw = szfDraw.eВписатьВ(szfCanvas).TargetSize;
				}
				else if (UpScaleSmallImgaes)
				{
					// Мелкое изображание надо увеличить ?
					var szfUpscaleTo = (DrawImageSize ?? CanvasSize).eToSizeF();
					if (ImageSize.Width < szfUpscaleTo.Width && ImageSize.Height > szfUpscaleTo.Height)
					{
						// Мелкое изображание надо увеличить!
						szfDraw = szfDraw.eВписатьВ(szfUpscaleTo).TargetSize;
					}
				}

				var ptfCenter = szfCanvas.eToRectangleF().eGetCenter();
				var rcDraw = szfDraw.eToRectangleF().eCenterTo(ptfCenter).eToRectangle();
				DrawImageProc.Invoke(g, rcDraw);
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










			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static StringAlignment eGetAlignment(this ContentAlignment ca)
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

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static StringAlignment eGetLineAlignment(this ContentAlignment ca)
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


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static TextFormatFlags eToTextFormatFlags(this ContentAlignment ca)
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

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static StringFormat eToStringFormat(this ContentAlignment ca)
			{
				var sf = new StringFormat()
				{
					Alignment = ca.eGetAlignment(),
					LineAlignment = ca.eGetLineAlignment()
				};
				return sf;
			}












			/// <returns>measured text size</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static SizeF eDrawTextEx(
				this Graphics g,
				string text,
				Font font,
				Color textcolor,
				RectangleF rect,
				ContentAlignment textAlign,
				StringTrimming trimming = StringTrimming.Character,
				bool autoHeightRect = false)
			{
				using Brush brText = new SolidBrush(textcolor);
				return g.eDrawTextEx(text, font, brText, rect, textAlign, trimming, autoHeightRect);
			}


			/// <returns>measured text size</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static SizeF eDrawTextEx(
				this Graphics g,
				string text,
				Font font,
				Brush textbrush,
				RectangleF rc,
				ContentAlignment textAlign,
				StringTrimming trimming = StringTrimming.Character,
				bool autoHeightRect = false
				)
			{
				using var sf = textAlign.eToStringFormat();
				sf.Trimming = trimming;

				var textSize = g.MeasureString(text, font, rc.Size, sf);
				if (autoHeightRect)
				{
					rc.Height = textSize.Height;
				}

				g.DrawString(text, font, textbrush, rc, sf);

				return textSize;
			}


			/// <returns>measured text size</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static SizeF eDrawTextEx(
				this Graphics g,
				string text,
				Font font,
				Brush textbrush,
				PointF location,
				ContentAlignment textAlign
				)
			{
				var msl = g.eMeasureStringLocation(text, font, location, textAlign);
				g.DrawString(text, font, textbrush, msl.TextPos);
				return msl.MeasuredTextSize;
			}






			public enum Direction { Up, Down, Right, Left }
			public enum GeoOrientation { North, South, East, West }
			public static GeoOrientation ToGeoOrientation(Direction direction) => direction switch
			{
				Direction.Up => GeoOrientation.North,
				Direction.Right => GeoOrientation.East,
				Direction.Down => GeoOrientation.South,
				Direction.Left => GeoOrientation.West,
				_ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
			};



			internal enum PaperOrientations : int { Portrait, Landscape, Square }
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static PaperOrientations eGetOrientation(this System.Drawing.Size szScreen)
				=> (szScreen.Width == szScreen.Height)
				? PaperOrientations.Square
				: (szScreen.Width > szScreen.Height)
					? PaperOrientations.Landscape
					: PaperOrientations.Portrait;


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool eOrientationIsLandscape(this System.Drawing.Size szScreen)
			{
				return szScreen.eGetOrientation() == PaperOrientations.Landscape;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool eOrientationIsPortrait(this System.Drawing.Size szScreen)
			{
				return szScreen.eGetOrientation() == PaperOrientations.Portrait;
			}




			#region Inches / centimeters / pixels


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static SizeF eInches_ToPixels(this Graphics g, SizeF SizeInInches)
				=> new(
					SizeInInches.Width * g.DpiX,
					SizeInInches.Height * g.DpiY
					);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Size eMM_ToPixels(this Graphics g, SizeF SizeInMM)
				=> new(
					(int)(SizeInMM.Width.eMMToInches() * g.DpiX),
					(int)(SizeInMM.Height.eMMToInches() * g.DpiY)
					);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static SizeF eInchesToCM(this SizeF valueInInches)
				=> new(
					valueInInches.Width.eInchesToCM(),
					valueInInches.Height.eInchesToCM());


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eInchesToCM(this RectangleF valueInInches)
				=> new(
					valueInInches.Left.eInchesToCM(),
					valueInInches.Top.eInchesToCM(),
					valueInInches.Width.eInchesToCM(),
					valueInInches.Height.eInchesToCM()
					);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eCMToInches(this RectangleF CM)
				=> new(
					CM.Left.eCMToInches(),
					CM.Top.eCMToInches(),
					CM.Width.eCMToInches(),
					CM.Height.eCMToInches()
					);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.SizeF ePixels_To_Inches(this Size pixels, System.Drawing.Point dpi)
				=> new(
					(float)pixels.Width / (float)dpi.X,
					(float)pixels.Height / (float)dpi.Y);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.Point eInches_To_Pixels(this PointF Inches, System.Drawing.Point DPI)
				=> new(
					(int)(Inches.X * DPI.X),
					(int)(Inches.Y * DPI.Y)
					);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.Size eInches_To_Pixels(this SizeF Inches, System.Drawing.Point DPI)
				=> new(
					(int)(Inches.Width * DPI.X),
					(int)(Inches.Height * DPI.Y)
					);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eInches_To_Pixels(this System.Drawing.RectangleF rcfДюймы, System.Drawing.Point DPI)
				=> new(
					rcfДюймы.Location.eInches_To_Pixels(DPI),
					rcfДюймы.Size.eInches_To_Pixels(DPI)
					);

			#endregion






			#region ВписатьРазмеры

			/// <summary>Вписывает одни размеры в другие, сохраняя пропорции</summary>
			/// <param name="source">Исходный размер, КОТОРЫЙ надо изменить</param>
			/// <param name="target">Целевой размер, В КОТОРЫЙ надо вписать</param>
			/// <returns>Zoom = коэффициент масштабирования</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static (System.Drawing.Size TargetSize, float Zoom) eВписатьВ(this System.Drawing.Size source, System.Drawing.Size target
				)
			{
				//float sngZoom = 0f;
				var SourceF = new SizeF(source.Width, source.Height);
				var LimitF = new SizeF(target.Width, target.Height);
				var (TargetSize, Zoom) = SourceF.eВписатьВ(LimitF);
				return (TargetSize.ToSize(), Zoom);
			}

			/// <summary>Вписывает одни размеры в другие, сохраняя пропорции</summary>
			/// <param name="source">Исходный размер, КОТОРЫЙ надо изменить</param>
			/// <param name="target">Целевой размер, В КОТОРЫЙ надо вписать</param>
			/// <returns>Zoom = коэффициент масштабирования</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static (System.Drawing.SizeF TargetSize, float Zoom) eВписатьВ(this SizeF source, SizeF target)
			{
				float zoom, w, h;

				if (source.Width > source.Height)
				{
					if (target.Width > target.Height)
					{
						zoom = source.Height / source.Width;
						w = target.Width;
						h = target.Width * zoom;
						if (h > target.Height)
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
				else if (target.Height > target.Width) // Source.Height > Source.Width
				{
					zoom = source.Width / source.Height;
					h = target.Height;
					w = target.Height * zoom;
					if (w > target.Width)
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

				var szf = new SizeF(w, h);

				// Рассчитываем коэффициент Zoom'а, по большему из размеров, для большей точночти
				zoom = (w > h)
					? (w / source.Width)
					: (h / source.Height);

				return (szf, zoom);
			}

			#endregion


			/// <summary>Размер = текущему разрешению экрана? Наверное будет гнать в многомониторной системе</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool eIsLikeScreen(this System.Drawing.Size szTarget)
			{
				throw new NotImplementedException();

				//var szScreen = My.Computer.Screen.Bounds.Size;
				//bool IsLikeScr = szTarget == szScreen;
				//return IsLikeScr;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool eIsFullHD(this System.Drawing.Size szTarget)
			{
				var szFullHD = new Size(1920, 1080);
				return (szTarget == szFullHD);
			}





			#region Point / Size / Rectangle - Туда / сюда преобразования


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static SizeF eMultiply(this SizeF source, float Zoom) => new(source.Width * Zoom, source.Height * Zoom);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static PointF eMultiply(this PointF source, float Zoom) => new(source.X * Zoom, source.Y * Zoom);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Point eToPoint(this PointF source) => Point.Round(source);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static PointF eToPoint(this SizeF source) => new(source.Width, source.Height);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Point eToPoint(this Size source) => new(source.Width, source.Height);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static PointF eToPointF(this Size source) => new(source.Width, source.Height);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static SizeF eToSize(this PointF source) => new(source.X, source.Y);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static SizeF eToSizeF(this Size source) => new(source.Width, source.Height);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eToRectangleF(this System.Drawing.SizeF source, PointF? location = null)
				=> new(
					location.HasValue
						? location.Value
						: new PointF(0, 0),
					 source);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eToRectangle(this System.Drawing.Size source, Point location)
				=> new(location, source);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eToRectangle(this System.Drawing.Size source)
				=> new(new Point(0, 0), source);



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eToRectangleF(this Rectangle source) => new(source.Left, source.Top, source.Width, source.Height);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eToRectangle(this RectangleF source) => Rectangle.Round(source);


			/// <summary>Округляет, используя Round(Precission)</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eRound(this RectangleF source, int precission)
			{
				float X, Y, W, H;
				X = source.Left.eRound(precission);
				Y = source.Top.eRound(precission);
				W = source.Width.eRound(precission);
				H = source.Height.eRound(precission);
				var RC = new RectangleF(X, Y, W, H);
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
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static PointF eRotateAround(this PointF ptfCenter, float AngleRad, float sngRadius)
			{
				var ptfOffset = AngleRad.eRotateAround(sngRadius);
				return new PointF(ptfCenter.X + ptfOffset.X, ptfCenter.Y + ptfOffset.Y);
			}

			/// <summary>Расчёт координат новой точки, повернутой вокруг заданной на угол (с разными X/Y радиусами - для овалов))</summary>
			/// <param name="ptfCenter">Точка, относительно оторой вращаемся</param>
			/// <param name="AngleRad">Угол в радианах</param>
			/// <param name="Radius">Радиус X,Y</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.PointF eRotateAround(this PointF ptfCenter, float AngleRad, System.Drawing.SizeF Radius)
			{
				var ptfOffset = AngleRad.eRotateAround(Radius);
				return new PointF(ptfCenter.X + ptfOffset.X, ptfCenter.Y + ptfOffset.Y);
			}

			/// <summary>Расчёт координат новой точки, повернутой вокруг заданной на угол (с разными X/Y радиусами - для овалов))</summary>
			/// <param name="AngleRad">Угол в радианах</param>
			/// <param name="Radius">Радиус X,Y</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.PointF eRotateAround(this float AngleRad, System.Drawing.SizeF Radius)
			{
				return new PointF((float)(Math.Cos(AngleRad) * Radius.Width), (float)(Math.Sin(AngleRad) * Radius.Height));
			}

			/// <summary>Расчёт координат новой точки, повернутой вокруг заданной на угол</summary>
			/// <param name="AngleRad">Угол в радианах</param>
			/// <param name="sngRadius">Радиус</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static PointF eRotateAround(this float AngleRad, float sngRadius)
			{
				return AngleRad.eRotateAround(new SizeF(sngRadius, sngRadius));
			}



			#endregion


			/// <summary>Делит каждое значение на заданное</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eDivideTo(this Rectangle RC, float Делитель) => new(RC.Left / Делитель, RC.Top / Делитель, RC.Width / Делитель, RC.Height / Делитель);


			/// <summary>Делит каждое значение на заданное</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eMultipleTo(this RectangleF RC, float Множитель) => new(RC.Left * Множитель, RC.Top * Множитель, RC.Width * Множитель, RC.Height * Множитель);


			/// <summary>Находит центр прямоугольника</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static PointF eGetCenter(this RectangleF RC) => new(RC.Left + (RC.Width / 2), RC.Top + (RC.Height / 2));

			internal static PointF eRound(this PointF pt, int digits = 0) => new(
				(float)Math.Round(pt.X, digits),
				(float)Math.Round(pt.Y, digits));

			internal static Point eRoundToInt(this PointF pt)
			{
				pt = pt.eRound(0);
				return new((int)pt.X, (int)pt.Y);
			}

			internal static Size eGetVectorTo(this Point source, Point target)
			{
				int dx = source.X - target.X;
				int dy = source.Y - target.Y;
				return new(dx, dy);
			}

			internal static double eGetHypotenuse(this Size vector)
			{
				var dx = Math.Pow((double)vector.Width, 2d);
				var dy = Math.Pow((double)vector.Height, 2d);
				return Math.Sqrt(dx + dy);
			}


			/// <summary>Находит центр прямоугольника</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static PointF eGetCenter(this Rectangle RC) => RC.eToRectangleF().eGetCenter();


			/// <summary>Центрирует Короткую линию по более длинной</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static float eCenterTo(this float ShortLineWidth, float LongLineWidth) => (LongLineWidth - ShortLineWidth) / 2f;


			/// <summary>Центрирует Короткую линию по более длинной</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int eCenterTo(this int ShortLineWidth, int LongLineWidth) => (int)Math.Round((LongLineWidth - ShortLineWidth) / 2f);


			/// <summary>Центрирует прямоугольник относительно заданной точки</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eCenterTo(this RectangleF RC, PointF ptCenter)
				=> new(
					ptCenter.X - RC.Width / 2,
					ptCenter.Y - RC.Height / 2,
					RC.Width,
					RC.Height);



			/// <summary>Центрирует прямоугольник относительно заданной точки</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eCenterTo(this Rectangle RC, Point ptCenter)
				=> new(
					ptCenter.X - RC.Width / 2,
					ptCenter.Y - RC.Height / 2,
					RC.Width,
					RC.Height);




			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eScale(this RectangleF source, PointF zoom)
				=> new(
					source.Left,
					source.Top,
					(source.Size.Width * zoom.X),
					(source.Size.Height * zoom.Y)
					);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eScale(this Rectangle source, PointF zoom)
				=> source.eToRectangleF().eScale(zoom);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eScaleToInt(this Rectangle source, PointF zoom)
				=> source.eScale(zoom).eToRectangle();




			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eMoveLeftEdge(this Rectangle source, int delthaX)
			{
				var rc = source;
				source.Offset(delthaX, 0);
				source.Width -= delthaX;
				return source;
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eMoveTopEdge(this Rectangle source, int delthaY)
			{
				var rc = source;
				source.Offset(0, delthaY);
				source.Height -= delthaY;
				return source;
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eMoveLeftTopCorner(this Rectangle source, Size deltha)
			{
				var rc = source;
				source.Offset(deltha.eToPoint());
				source.Width -= deltha.Width;
				source.Height -= deltha.Height;
				return source;
			}




			/// <summary>Центрирует прямоугольник относительно заданной точки</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle eAlignTo(this Rectangle src, Rectangle trg, ContentAlignment alignment, Point? offset = null)
			{
				Point ptLocation = src.Location;

				{
					//Calculate Y
					switch (alignment)
					{
						case ContentAlignment.TopLeft:
						case ContentAlignment.TopCenter:
						case ContentAlignment.TopRight:
							{
								ptLocation.Y = trg.Y;
								if (offset.HasValue)
								{
									ptLocation.Y += offset.Value.Y;
								}

								break;
							}
					}

					switch (alignment)
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

					switch (alignment)
					{
						case ContentAlignment.BottomLeft:
						case ContentAlignment.BottomCenter:
						case ContentAlignment.BottomRight:
							{
								ptLocation.Y = trg.Bottom - src.Height;
								if (offset.HasValue)
								{
									ptLocation.Y -= offset.Value.Y;
								}

								break;
							}
					}
				}

				{
					//Calculate X

					switch (alignment)
					{
						case ContentAlignment.TopLeft:
						case ContentAlignment.MiddleLeft:
						case ContentAlignment.BottomLeft:
							{
								ptLocation.X = trg.X;
								if (offset.HasValue)
								{
									ptLocation.X += offset.Value.X;
								}

								break;
							}
					}

					switch (alignment)
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

					switch (alignment)
					{
						case ContentAlignment.TopRight:
						case ContentAlignment.MiddleRight:
						case ContentAlignment.BottomRight:
							{
								ptLocation.X = trg.Right - src.Width;
								if (offset.HasValue)
								{
									ptLocation.X -= offset.Value.X;
								}

								break;
							}
					}
				}
				return new(ptLocation, src.Size);
			}

			/// <summary>Центрирует прямоугольник относительно заданной точки</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static RectangleF eAlignTo(this RectangleF src, RectangleF trg, ContentAlignment alignment, Point? offset = null)
			{
				PointF ptLocation = src.Location;

				{
					//Calculate Y
					switch (alignment)
					{
						case ContentAlignment.TopLeft:
						case ContentAlignment.TopCenter:
						case ContentAlignment.TopRight:
							{
								ptLocation.Y = trg.Y;
								if (offset.HasValue)
								{
									ptLocation.Y += offset.Value.Y;
								}

								break;
							}
					}

					switch (alignment)
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

					switch (alignment)
					{
						case ContentAlignment.BottomLeft:
						case ContentAlignment.BottomCenter:
						case ContentAlignment.BottomRight:
							{
								ptLocation.Y = trg.Bottom - src.Height;
								if (offset.HasValue)
								{
									ptLocation.Y -= offset.Value.Y;
								}

								break;
							}
					}
				}

				{
					//Calculate X

					switch (alignment)
					{
						case ContentAlignment.TopLeft:
						case ContentAlignment.MiddleLeft:
						case ContentAlignment.BottomLeft:
							{
								ptLocation.X = trg.X;
								if (offset.HasValue)
								{
									ptLocation.X += offset.Value.X;
								}

								break;
							}
					}

					switch (alignment)
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

					switch (alignment)
					{
						case ContentAlignment.TopRight:
						case ContentAlignment.MiddleRight:
						case ContentAlignment.BottomRight:
							{
								ptLocation.X = trg.Right - src.Width;
								if (offset.HasValue)
								{
									ptLocation.X -= offset.Value.X;
								}

								break;
							}
					}
				}
				return new(ptLocation, src.Size);
			}












			/// <summary>Точка внутри?</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool eIsInRect(this System.Drawing.Rectangle RC, System.Drawing.Point PT)
			{
				if (PT.X < RC.Left)
				{
					return false;
				}

				if (PT.X > RC.Right)
				{
					return false;
				}

				if (PT.Y < RC.Top)
				{
					return false;
				}

				if (PT.Y > RC.Bottom)
				{
					return false;
				}

				return true;
			}

			/// <summary>Сделать, чтобы точка не выходила за пределы</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.RectangleF eEnsureInRect(this System.Drawing.RectangleF src, System.Drawing.RectangleF trg)
			{
				if (src.Left > trg.Right)
				{
					src.X = trg.Right;
				}

				if (src.Top > trg.Bottom)
				{
					src.Y = trg.Bottom;
				}

				if (src.Left < trg.Left)
				{
					src.X = trg.Left;
				}

				if (src.Top < trg.Top)
				{
					src.Y = trg.Top;
				}

				if (src.Right > trg.Right)
				{
					src.Width = trg.Right - src.Left;
				}

				if (src.Bottom > trg.Bottom)
				{
					src.Height = trg.Bottom - src.Top;
				}

				return src;
			}




			/// <inheritdoc cref="eEnsureInRect"/>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.Rectangle eEnsureInRect(this System.Drawing.Rectangle src, System.Drawing.Rectangle trg)
			{
				if (src.Left > trg.Right)
				{
					src.X = trg.Right;
				}

				if (src.Top > trg.Bottom)
				{
					src.Y = trg.Bottom;
				}

				if (src.Left < trg.Left)
				{
					src.X = trg.Left;
				}

				if (src.Top < trg.Top)
				{
					src.Y = trg.Top;
				}

				if (src.Right > trg.Right)
				{
					src.Width = trg.Right - src.Left;
				}

				if (src.Bottom > trg.Bottom)
				{
					src.Height = trg.Bottom - src.Top;
				}

				return src;
			}

			/// <summary>Normalizing rectangle</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.Rectangle eNormalize(this System.Drawing.Rectangle src)
			{
				int l = new int[] { src.Left, src.Right }.Min();
				int r = new int[] { src.Left, src.Right }.Max();
				int t = new int[] { src.Top, src.Bottom }.Min();
				int b = new int[] { src.Top, src.Bottom }.Max();
				return Rectangle.FromLTRB(l, t, r, b);
			}

			/// <summary>Normalizing rectangle</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static System.Drawing.RectangleF eNormalize(this System.Drawing.RectangleF src)
			{
				float l = new float[] { src.Left, src.Right }.Min();
				float r = new float[] { src.Left, src.Right }.Max();
				float t = new float[] { src.Top, src.Bottom }.Min();
				float b = new float[] { src.Top, src.Bottom }.Max();
				return RectangleF.FromLTRB(l, t, r, b);
			}











			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string ToString_WxH(this Point PT) => $"{PT.X}x{PT.Y}";


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string ToString_WxH(this PointF PT) => $"{PT.X}x{PT.Y}";


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string ToString_WxH(this PointF PT, int iRound) => $"{PT.X.eRound(iRound)}x{PT.Y.eRound(iRound)}";


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string ToString_WxH(this Size PT) => $"{PT.Width}x{PT.Height}";


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string ToString_WxH(this SizeF PT) => $"{PT.Width}x{PT.Height}";


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string ToString_WxH(this SizeF PT, int iRound) => $"{PT.Width.eRound(iRound)}x{PT.Height.eRound(iRound)}";

		}




		internal static class Extensions_Imaging
		{
















			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Bitmap eTransformColor(this Bitmap original, ColorMatrix cm)
			{
				// create a blank bitmap the same size as original
				Bitmap newBitmap = new(original.Width, original.Height, PixelFormat.Format32bppArgb);

				// create some image attributes
				using ImageAttributes attributes = new();
				// set the color matrix attribute
				attributes.SetColorMatrix(cm);

				// get a graphics object from the new image
				using Graphics g = Graphics.FromImage(newBitmap);
				// draw the original image on the new image using the grayscale color matrix
				g.DrawImage(
					 original,
					 original.Size.eToRectangle(),
					 0, 0, original.Width, original.Height,
					 GraphicsUnit.Pixel,
					 attributes);

				return newBitmap;
			}



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Bitmap eMakeTransparent(this Bitmap original, float alpha)
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



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Bitmap eToGrayscaled_Matrix(this Bitmap original)
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



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Image eToGrayscaled_ToolStripRenderer(this Bitmap src) => ToolStripRenderer.CreateDisabledImage(src);



			/// <summary>Saves the specified <see cref="Bitmap"/> objects as a single icon into the output stream.</summary>
			/// <param name="iconFrames">The bitmaps to save as an icon frames.<br/>
			/// The expected input image size is less than or equal to 256 and the height is less than or equal to 256</param>
			internal static Stream eSaveAsMultisizedIconStream(this IEnumerable<Bitmap> iconFrames)
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

				using (BinaryWriter bw = new(msOutput, Encoding.ASCII, true))
				{
					// write the header
					bw.Write(ICON_HEADER_RESERVED);
					bw.Write(ICON_HEADER_ICON_TYPE);
					bw.Write((ushort)orderedImages.Length);

					// save the image buffers and offsets
					Dictionary<uint, byte[]> buffers = [];

					// tracks the length of the buffers as the iterations occur
					// and adds that to the offset of the entries
					uint lengthSum = 0;
					uint baseOffset = (uint)(HEADER_LENGTH +
											 ENTRY_LENGTH * orderedImages.Length);

					for (uint i = 0 ; i < orderedImages.Length ; i++)
					{
						Bitmap image = orderedImages[i];

						if (image.PixelFormat != PixelFormat.Format32bppArgb)
						{
							throw new InvalidOperationException($"Required pixel format is PixelFormat.{PixelFormat.Format32bppArgb}.");
						}

						if (image.Width > MAX_ICON_SIZE || image.Height > MAX_ICON_SIZE)
						{
							throw new InvalidOperationException($"Dimensions must be less than or equal to {MAX_ICON_SIZE}x{MAX_ICON_SIZE}");
						}

						if (image.RawFormat.Guid != ImageFormat.Png.Guid)
						{
							//Converting image to png
							using MemoryStream msPngTemp = new();
							image.Save(msPngTemp, ImageFormat.Png);
							msPngTemp.Seek(0, SeekOrigin.Begin);
							image = (Bitmap)Bitmap.FromStream(msPngTemp);
						}

						using var msBuffer = new MemoryStream();
						image.Save(msBuffer, image.RawFormat);
						// creates a byte array from an image
						byte[] buffer = msBuffer.ToArray();

						// calculates what the offset of this image will be
						// in the stream
						uint offset = (baseOffset + lengthSum);

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
						buffers.Add(offset, buffer);
					}

					// writes the buffers for each image
					foreach (var kvp in buffers)
					{

						// seeks to the specified offset required for the image buffer
						bw.BaseStream.Seek(kvp.Key, SeekOrigin.Begin);

						// writes the buffer
						bw.Write(kvp.Value);
					}
				}

				msOutput.Seek(0, SeekOrigin.Begin);
				return msOutput;
			}








			/// <summary>Клонирует объект системным методом CLONE, возвращая объект такого-же типа, что и исходный</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Icon eCloneViaGDICopyIcon(this Icon rSource)
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
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Bitmap eCloneViaDrawImage(this System.Drawing.Image imgSrc)
			{
				Bitmap bmNew = new(imgSrc.Width, imgSrc.Height);
				using Graphics g = Graphics.FromImage(bmNew);
				g.DrawImage(imgSrc, 0, 0, imgSrc.Width, imgSrc.Height);
				return bmNew;
			}


			/// <summary>Клонирует объект системным методом CLONE, возвращая объект такого-же типа, что и исходный</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Icon eCloneViaMemStream(this Icon rSource)
			{
				using MemoryStream ms = new();
				rSource.Save(ms);
				ms.Seek(0L, SeekOrigin.Begin);
				return new Icon(ms);
			}


			/// <summary>Клонирует объект системным методом CLONE, возвращая объект такого-же типа, что и исходный</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Bitmap eCloneViaMemStream(this Bitmap rSource)
			{
				using MemoryStream ms = new();
				rSource.Save(ms, System.Drawing.Imaging.ImageFormat.MemoryBmp);
				ms.Seek(0L, SeekOrigin.Begin);
				return new Bitmap(ms);
			}








		}





		internal static partial class Extensions_UI_Localization
		{



			#region Menus

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eLocalizeUITree(this MenuStrip mnuRoot)
			{
				ToolStripItem[] actl = [.. mnuRoot.Items.Cast<ToolStripItem>()];
				actl.eLocalizeUI(true);
			}


			#endregion


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eLocalizeUI(this Component c, bool recurse = true)
			{

				switch (c)
				{
					case ColumnHeader col:
						{
							var s = col.eGetLocalizedTextByPropertyName();
							if (s != null)
							{
								col.Text = s;
							}
							else
							{
								s = col.Text.eGetLocalizedText();
								if (s != null)
								{
									col.Text = s;
								}
							}
							break;
						}

					case MenuStrip mnu:
						{
							mnu.Items.Cast<ToolStripItem>().eLocalizeUI(recurse);
							break;
						}

					case ToolStripItem tsi:
						{
							switch (tsi)
							{
								case ToolStripSeparator sep: break;

								case ToolStripButton:
								case ToolStripDropDownButton:
								case ToolStripLabel:
								case ToolStripMenuItem:
									{
										var s = tsi.Name.eGetLocalizedText();
										if (s != null)
										{
											tsi.Text = s;
										}
										else
										{
											s = tsi.Text.eGetLocalizedText();
											if (s != null)
											{
												tsi.Text = s;
											}
										}

										if (recurse && tsi is ToolStripMenuItem mnu)
										{
											//ToolStripItem[] actl = [.. mnu.DropDownItems.Cast<ToolStripItem>()];
											mnu.DropDownItems.Cast<ToolStripItem>().eLocalizeUI(recurse);
										}
										break;
									}

							}
							break;
						}

					case Control ctl:
						{
							var s = ctl.eGetLocalizedTextByPropertyName();
							if (s != null)
							{
								ctl.Text = s;
							}
							else
							{
								s = ctl.Text.eGetLocalizedText();
								if (s != null)
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



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eLocalizeUI(this IEnumerable<Component> cmpList, bool recurse = true)
				=> cmpList.eForEach(ctl => ctl.eLocalizeUI(recurse));






		}





		internal static partial class Extensions_Winforms_Errors_TryCatch_Log
		{







			#region TryCatchWin


			/// <summary>Вызывает Callback внутри Try/Catch и при ошибке автоматом вызывает ex.eLogError(ShowModalMessageBox)</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eTryCatchWin(this Action a,
						bool errorUI = true,
						string uiTitle = C_FAILED_TO_RUN,
						MessageBoxIcon icon = MessageBoxIcon.Error,
						MessageBoxButtons btn = MessageBoxButtons.OK,
						bool debugErrorUI = false)
			{
				try { a.Invoke(); }
				catch (Exception ex) { ex.eLogError(errorUI, uiTitle, icon, btn, debugErrorUI); }
			}


			/// <summary>Вызывает Callback внутри Try/Catch и при ошибке автоматом вызывает ex.eLogError(ShowModalMessageBox)</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eTryCatchWin(this System.Windows.Forms.MethodInvoker a,
						bool errorUI = true,
						string uiTitle = C_FAILED_TO_RUN,
						MessageBoxIcon icon = MessageBoxIcon.Error,
						MessageBoxButtons btn = MessageBoxButtons.OK,
						bool debugErrorUI = false)
			{
				try { a.Invoke(); }
				catch (Exception ex) { ex.eLogError(errorUI, uiTitle, icon, btn, debugErrorUI); }
			}


			#endregion



			#region eLogError


			/// <summary>Фиксация ошибки в журнале, в DEBUG, вывод сообщения</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eLogError(this Exception ex,
				bool errorUI,
				string uiTitle = C_FAILED_TO_RUN,
				MessageBoxIcon icon = MessageBoxIcon.Error,
				MessageBoxButtons btn = MessageBoxButtons.OK,
				bool debugErrorUI = false,
				[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0
				)
			{
				try
				{
					string errorDump = ex.eFullDump(callerName, callerFile, callerLine);

					UInt16 eventID = 1001;
					try
					{
						if (ex is Win32Exception wex)
						{
							eventID = (UInt16)wex.ErrorCode.eCheckRange(UInt16.MinValue, UInt16.MaxValue);
						}
					}
					catch { }

					WinAPI.errors.ErrorLogWrite(errorDump, eventID: eventID);
					string msg = ex.Message;
#if DEBUG
					$"{CS_CONSOLE_SEPARATOR}\n{errorDump}\n{CS_CONSOLE_SEPARATOR}".eDebugWriteLine();

					//Показываем расширенные данные в DEBUG режиме
					msg += $"\n{CS_CONSOLE_SEPARATOR}\nUOM DEBUG-MODE DETAILED ERROR INFO:\n{errorDump}";
#endif

					if (errorUI) // Надо показать на экране модальное Сообщение об ошибке
					{
						MessageBox.Show(msg, uiTitle, btn, icon);
					}
					else
					{
#if DEBUG
						if (debugErrorUI)
						{
							//В DEBUG режиме показываем модальное окно с ошибкой, если прямо не запрещено!
							MessageBox.Show(msg, uiTitle, btn, icon);
						}
#endif
					}
				}
				catch (Exception ex2)
				{
					if (errorUI)
					{
						MessageBox.Show(ex2.Message, "Error when journaling previous error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}




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

				private readonly MTSafeBooleanFlag _IsfrmErrorClosing = new();
				private readonly TextBox _ctlTextBox;
				private int _iTotalErrorsCount = 0;

				//private readonly EventArgs _ErrorsQueueSyncObject = new EventArgs();
				private readonly Queue<Exception> _qErrorsToShow = new(); // Очередь сообщений об ошибках для показа

				public FORM_ATTACHMENT(Form PF) : base()
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
					_IsfrmErrorClosing.ClearFlag();
					StartWatchTimer();
				}

				private void UpdateFormTitle() => _frmError!.Text = "Ошибки: {0}".eFormat((object)_iTotalErrorsCount);

				//private FormClosingEventHandler _OnErrorFormClosing2233 = new(OnErrorFormClosing);

				/// <summary>При попытке зукрыть окно не закрываем его, а просто скрываем</summary>
				private void OnErrorFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
				{
					if (CloseReason.UserClosing == e.CloseReason)
					{
						e.Cancel = true;
						_frmError?.Hide();
						return;
					}

					_IsfrmErrorClosing.SetlFlag(); // Ставим флаг, что форма начала закрываться, и больше ничего показывать нельзя.
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

				private void StartWatchTimer()
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
				private async void WatchNewErrorrs()
				{
					__tmrWatchErrors?.Stop(); // Останавливаем таймер, чтобы из-за длительного выполнения не накладывались обработчики таймера
					if (_IsfrmErrorClosing.IsSet) return; // форма начала закрываться, и больше ничего показывать нельзя. Таймер перезапускать не обязательно.

					try
					{
						if (null != _frmError && !_frmError.Handle.eIsValid()) return; // Негде показать. Таймер перезапускать не обязательно.
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
									 aErrorsToShow = _qErrorsToShow.ToArray(); _qErrorsToShow.Clear();
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
									 var sMSG = EX.eLogError_FullErrorDump() + constants.vbCrLf;
#else
									 var sMSG = EX.Message + constants.vbCrLf;
#endif
									 sbAllErrors.Append(sMSG);
								 }
								 catch { }
							 }
							 return sbAllErrors.ToString();
						 });

						string? sAllErrors = await PrepareAllErrorsInfoCallBack.eRunAsync();
						if (sAllErrors.eIsNullOrWhiteSpace()) return;

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
				public void AddErrorToQueue(Exception EX)
				{
					try { EX.eLogError(false, SupressAnyModalPopEvenInDEBUG: true); }// Пишем ошибку в журнал
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
			private static readonly List<FORM_ATTACHMENT> _lListOfAttachments = new();

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
			// Call frmErrorWindowParent.eRunInUIThread(Sub() ShowError_CORE(EX, frmErrorWindowParent))

			// Catch ex2 As Exception
			// Неудалось показать ошибку в окне!
			// End Try
			// End Sub


			/// <summary>Выполняется в вызывающем потоке</summary>
			[ MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eLogError_NONMODAL(this Exception EX, Form frmErrorWindowParent)
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
							void CreateNewAttachment(Form FP)
							{
								FA = new FORM_ATTACHMENT(FP); // Создаём новое
								_lListOfAttachments.Add(FA);
							};

							// Временно переходим в поток этой формы
							frmErrorWindowParent.eRunInUIThread(() => CreateNewAttachment(frmErrorWindowParent));
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



	namespace WinAPI
	{

		internal partial struct RECT
		{


			#region Constructor

			public RECT(Rectangle R) : this(R.Left, R.Top, R.Right, R.Bottom) { }

			public RECT(Point Location, Size Size) : this(new Rectangle(Location, Size)) { }


			#endregion


			internal readonly Point Location => new(Left, Top);

			//public Rectangle ToRectangle() => new(Left, Top, Width, Height);


			public Size Size => new(Width, Height);


			public override string ToString() => ((Rectangle)this).ToString();


			#region Operator

			public static implicit operator Rectangle(RECT R)
				=> new(R.Left, R.Top, R.Width, R.Height);

			#endregion



		}



		/// <summary>Win32 Shell API</summary>
		internal static partial class shell
		{


			[DllImport(core.WINDLL_SHELL, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
			private static extern int PickIconDlg(
			IntPtr hwnd,
			[In, Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszIconPath,
			[In] int cchIconPath,
			[In, Out] ref int piIconIndex);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static (DialogResult DislogResult, string? IconFile, int IconIndex) PickIconDlg(
				IWin32Window hWnd,
				string? iconFile = null,
				int iIconIndex = 0)
			{
				const int C_DEFAULT_BUFFER_SIZE = 2000;
				StringBuilder sbPath = (null != iconFile)
					? new StringBuilder(iconFile, C_DEFAULT_BUFFER_SIZE)
					: new StringBuilder(C_DEFAULT_BUFFER_SIZE);

				int iResult = PickIconDlg(hWnd.Handle, sbPath, sbPath.Capacity, ref iIconIndex);
				if (iResult == 0)
				{
					return (DialogResult.Cancel, null, -1);
				}

				iconFile = Environment.ExpandEnvironmentVariables(sbPath.ToString());
				return (DialogResult.OK, iconFile, iIconIndex);
			}


		}




		internal static partial class windows
		{

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string GetWindowText(IWin32Window wnd) => GetWindowText(wnd.Handle);



			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string GetClassName(IWin32Window wnd) => GetClassName(wnd.Handle);




			/// <inheritdoc cref="GetWindowRect" />
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle GetWindowRect(IntPtr hwnd)
			{
				if (!GetWindowRect(hwnd, out RECT rc))
				{
					throw new Win32Exception();
				}

				return rc;
			}




			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle GetClientRect(IWin32Window wind) => GetClientRect(wind.Handle);
















			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void SetWindowRectWithoutShadow(IntPtr handle, Rectangle window)
			{
				Rectangle excludeshadow = GetWindowRectWithoutShadow(handle);
				Rectangle includeshadow = GetWindowRect(handle);
				RECT shadow = new()
				{
					Left = includeshadow.X - excludeshadow.X,
					Right = includeshadow.Right - excludeshadow.Right,
					Top = includeshadow.Top - excludeshadow.Top,
					Bottom = includeshadow.Bottom - excludeshadow.Bottom
				};

				int width = (window.Right + shadow.Right) - (window.Left + shadow.Left);
				int height = (window.Bottom + shadow.Bottom) - (window.Top - shadow.Top);

				SetWindowPos(handle, IntPtr.Zero,
					  window.Left + shadow.Left,
					  window.Top + shadow.Top,
					  width,
					  height,
					  0);
			}









			#region DwmGetWindowAttribute / DwmSetWindowAttribute


			/// <summary>https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute</summary>
			/// <summary>
			///     Flags used by the <see cref="Dwmapi.DwmGetWindowAttribute" /> and <see cref="Dwmapi.DwmSetWindowAttribute" /> functions to specify window
			///     attributes for non-client rendering.
			/// </summary>
			internal enum DWMWINDOWATTRIBUTE : int
			{
				/// <summary>
				///     Use with<see cref="DwmGetWindowAttribute" />. Discovers whether non-client rendering is enabled. The retrieved value is of type BOOL. TRUE
				///     if non-client rendering is enabled; otherwise, FALSE.
				/// </summary>
				DWMWA_NCRENDERING_ENABLED = 1,

				/// <summary>
				///     Use with <see cref="DwmSetWindowAttribute" />\. Sets the non-client rendering policy. The pvAttribute parameter points to a value from the
				///     <see cref="DWMNCRENDERINGPOLICY" /> enumeration.
				/// </summary>
				DWMWA_NCRENDERING_POLICY,

				/// <summary>
				///     Use with <see cref="DwmSetWindowAttribute" />. Enables or forcibly disables DWM transitions. The pvAttribute parameter points to a value
				///     of TRUE to disable transitions or FALSE to enable transitions.
				/// </summary>
				DWMWA_TRANSITIONS_FORCEDISABLED,

				/// <summary>
				///     Use with <see cref="DwmSetWindowAttribute" />. Enables content rendered in the non-client area to be visible on the frame drawn by DWM.
				///     The pvAttribute parameter points to a value of TRUE to enable content rendered in the non-client area to be visible on the frame; otherwise, it
				///     points to FALSE.
				/// </summary>
				DWMWA_ALLOW_NCPAINT,

				/// <summary>
				///     Use with <see cref="DwmGetWindowAttribute" />. Retrieves the bounds of the caption button area in the window-relative space. The retrieved
				///     value is of type RECT.
				/// </summary>
				DWMWA_CAPTION_BUTTON_BOUNDS,

				/// <summary>
				///     Use with <see cref="DwmSetWindowAttribute" />. Specifies whether non-client content is right-to-left (RTL) mirrored. The pvAttribute
				///     parameter points to a value of TRUE if the non-client content is right-to-left (RTL) mirrored; otherwise, it points to FALSE.
				/// </summary>
				DWMWA_NONCLIENT_RTL_LAYOUT,

				/// <summary>
				///     Use with <see cref="DwmSetWindowAttribute" /> . Forces the window to display an iconic thumbnail or peek representation (a static bitmap),
				///     even if a live or snapshot representation of the window is available. This value normally is set during a window's creation and not changed
				///     throughout the window's lifetime. Some scenarios, however, might require the value to change over time. The pvAttribute parameter points to a
				///     value of TRUE to require a iconic thumbnail or peek representation; otherwise, it points to FALSE.
				/// </summary>
				DWMWA_FORCE_ICONIC_REPRESENTATION,

				/// <summary>
				///     Use with <see cref="DwmSetWindowAttribute" />. Sets how Flip3D treats the window. The pvAttribute parameter points to a value from the
				///     <see cref="DWMFLIP3DWINDOWPOLICY" /> enumeration.
				/// </summary>
				DWMWA_FLIP3D_POLICY,

				/// <summary>
				///     Use with <see cref="DwmGetWindowAttribute" />. Retrieves the extended frame bounds rectangle in screen space. The retrieved value is of
				///     type <see cref="RECT" />.
				/// </summary>
				DWMWA_EXTENDED_FRAME_BOUNDS,

				/// <summary>
				///     Use with<see cref="DwmSetWindowAttribute" />. The window will provide a bitmap for use by DWM as an iconic thumbnail or peek
				///     representation (a static bitmap) for the window. <see cref="DWMWA_HAS_ICONIC_BITMAP" /> can be specified with
				///     <see cref="DWMWA_FORCE_ICONIC_REPRESENTATION" />. <see cref="DWMWA_HAS_ICONIC_BITMAP" /> normally is set during a window's creation and not
				///     changed throughout the window's lifetime. Some scenarios, however, might require the value to change over time. The pvAttribute parameter points
				///     to a value of TRUE to inform DWM that the window will provide an iconic thumbnail or peek representation; otherwise, it points to FALSE.
				/// </summary>
				DWMWA_HAS_ICONIC_BITMAP,

				/// <summary>
				///     Use with <see cref="DwmSetWindowAttribute" />. Do not show peek preview for the window. The peek view shows a full-sized preview of the
				///     window when the mouse hovers over the window's thumbnail in the taskbar. If this attribute is set, hovering the mouse pointer over the window's
				///     thumbnail dismisses peek (in case another window in the group has a peek preview showing). The pvAttribute parameter points to a value of TRUE to
				///     prevent peek functionality or FALSE to allow it.
				/// </summary>
				DWMWA_DISALLOW_PEEK,

				/// <summary>
				///     Use with <see cref="DwmSetWindowAttribute" />. Prevents a window from fading to a glass sheet when peek is invoked. The pvAttribute
				///     parameter points to a value of TRUE to prevent the window from fading during another window's peek or FALSE for normal behavior.
				/// </summary>
				DWMWA_EXCLUDED_FROM_PEEK,


				DWMWA_CLOAK,

				DWMWA_CLOAKED,

				DWMWA_FREEZE_REPRESENTATION,

				DWMWA_PASSIVE_UPDATE_MODE,

				DWMWA_USE_HOSTBACKDROPBRUSH,

				DWMWA_USE_IMMERSIVE_DARK_MODE = 20,

				DWMWA_WINDOW_CORNER_PREFERENCE = 33,

				DWMWA_BORDER_COLOR,

				DWMWA_CAPTION_COLOR,

				DWMWA_TEXT_COLOR,

				DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,

				DWMWA_SYSTEMBACKDROP_TYPE,

				/// <summary>
				///     The maximum recognized <see cref="DWMWINDOWATTRIBUTE" /> value, used for validation purposes.
				/// </summary>
				DWMWA_LAST
			};


			#region DwmGetWindowAttribute 


			[DllImport(core.WINDLL_DWMAPI, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
			internal static extern int DwmGetWindowAttribute(
				[In] IntPtr hwnd,
				[In] DWMWINDOWATTRIBUTE dwAttribute,
				[In, Out] IntPtr pvAttribute,
				[In] int cbAttribute);

			[DllImport(core.WINDLL_DWMAPI, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
			private static extern int DwmGetWindowAttribute(
				[In] IntPtr hwnd,
				[In] DWMWINDOWATTRIBUTE dwAttribute,
				[Out] out uom.WinAPI.RECT pvAttribute,
				[In] int cbAttribute);



			/// <summary>Gets window rect without shadow</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle DwmGetWindowAttribute_DWMWA_EXTENDED_FRAME_BOUNDS(IntPtr hwnd)
			{
				int r = DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_EXTENDED_FRAME_BOUNDS, out uom.WinAPI.RECT rect, Marshal.SizeOf(typeof(uom.WinAPI.RECT)));
				if (r != 0)
				{
					Marshal.ThrowExceptionForHR(r);
				}

				return rect;
			}

			#endregion


			#region DwmGetWindowAttribute 


			[DllImport(core.WINDLL_DWMAPI, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
			internal static extern int DwmSetWindowAttribute(
				[In] IntPtr hwnd,
				[In] DWMWINDOWATTRIBUTE dwAttribute,
				[In] IntPtr pvAttribute,
				[In] int cbAttribute);

			[DllImport(core.WINDLL_DWMAPI, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
			internal static extern int DwmSetWindowAttribute(
				[In] IntPtr hwnd,
				[In] DWMWINDOWATTRIBUTE dwAttribute,
				[In] ref bool value,
				[In] int cbAttribute);

			[DllImport(core.WINDLL_DWMAPI, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
			internal static extern int DwmSetWindowAttribute(
				[In] IntPtr hwnd,
				[In] DWMWINDOWATTRIBUTE dwAttribute,
				[In] ref int value,
				[In] int cbAttribute);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, bool value)
			{
				int r = DwmSetWindowAttribute(hwnd, dwAttribute, ref value, Marshal.SizeOf(typeof(bool)));
				if (r != 0)
				{
					Marshal.ThrowExceptionForHR(r);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void DwmSetWindowAttribute_DWMWA_USE_IMMERSIVE_DARK_MODE(IntPtr hwnd, bool value)
				=> DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, value);





			#endregion



			#endregion



			/// <summary>Gets window rect without shadow</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Rectangle GetWindowRectWithoutShadow(IntPtr hwnd) => DwmGetWindowAttribute_DWMWA_EXTENDED_FRAME_BOUNDS(hwnd);






		}

	}


}


#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning restore IDE1006 // Naming Styles
