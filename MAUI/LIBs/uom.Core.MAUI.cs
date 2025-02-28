
global using uom.Extensions;

global using static uom.constants;
global using static uom.Extensions.Extensions_DebugAndErrors;

using System.Collections.Concurrent;
using System.Runtime.Versioning;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

using static Microsoft.Maui.ApplicationModel.Permissions;



#region .NET MAUI Community Toolkit (https://github.com/CommunityToolkit/Maui)



#region Validate user Input

/*				
		https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/behaviors/numeric-validation-behavior
	
	
	 <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:mct="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
             x:Class="CommunityToolkit.Maui.Sample.Pages.Behaviors.NumericValidationBehaviorPage">

    <ContentPage.Resources>
        <Style x:Key="InvalidEntryStyle" TargetType="Entry">
            <Setter Property="TextColor" Value="Red" />
        </Style>
        <Style x:Key="ValidEntryStyle" TargetType="Entry">
            <Setter Property="TextColor" Value="Green" />
        </Style>
    </ContentPage.Resources>

	
	
    <Entry Keyboard="Numeric">
        <Entry.Behaviors>
            <mct:NumericValidationBehavior
                InvalidStyle="{StaticResource InvalidEntryStyle}"
                ValidStyle="{StaticResource ValidEntryStyle}"
                Flags="ValidateOnValueChanged"
                MinimumValue="1.0"
                MaximumValue="100.0"
                MaximumDecimalPlaces="2" />
        </Entry.Behaviors>
    </Entry>

</ContentPage>
	
	
 
 */

#endregion



#endregion

/*
 
 using CommunityToolkit.Maui.Alerts; // For the Toast

#if ANDROID
using Android.Content;
	using Android.Locations;
#elif IOS || MACCATALYST
	using CoreLocation;
#elif WINDOWS
	using Windows.Devices.Geolocation;
#endif



#if !WINDOWS
            handler.PlatformView.SetSelectAllOnFocus(true);
#elif IOS || MACCATALYST
            handler.PlatformView.EditingDidBegin += (s, e) =>
            {
                handler.PlatformView.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
            };
#elif WINDOWS
            handler.PlatformView.GotFocus += (s, e) =>
            {
                handler.PlatformView.SelectAll();
            };
#endif

 
 // Create the password string
string myPass = "somePass";

// Save the password to the Preferences system
Preferences.Set("UserPassword", myPass); // The first parameter is the key
After saving the user's data to the app's preferences, you can retrieve it on app startup and perform an automatic login by using the Get() method.

// Get the password from the Preferences system
string passwordFromPrefs = Preferences.Get("UserPassword", "defaultPass");
 

<StackPanel Margin="10">
	<TextBlock Text="{Binding CelsiusTemp, StringFormat={}{0}°C}" />
	<TextBlock Text="{Binding ElementName=wnd, Path=ActualWidth, StringFormat={}{0:#,#.0}}" />
	<TextBlock Text="{Binding Source={x:Static system:DateTime.Now}, ConverterCulture='de-DE', StringFormat=German date: {0:D}}" />
	<TextBlock Text="{Binding Source={x:Static system:DateTime.Now}, ConverterCulture='en-US', StringFormat=American date: {0:D}}" />
	<TextBlock Text="{Binding Source={x:Static system:DateTime.Now}, ConverterCulture='ja-JP', StringFormat=Japanese date: {0:D}}" />
</StackPanel>


ndles null situation and does not throw an exception, but returns true if no value is presented; otherwise takes the inputted Boolean and reverses it.

public class BooleanToReverseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
     => !(bool?) value ?? true;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
     => !(value as bool?);
}
Xaml

IsEnabled="{Binding IsSuccess Converter={StaticResource BooleanToReverseConverter}}"
App.Xaml I like to put all my converter statics in the app.xaml file so I don't have to redeclare them throughout the windows/pages/controls of the project.

<Application.Resources>
    <converters:BooleanToReverseConverter x:Key="BooleanToReverseConverter"/>
    <local:FauxVM x:Key="VM" />
</Application.Resources>

 
 */




namespace uom.maui
{

	namespace ui
	{



		internal static partial class KeyboardHelper
		{



			public static void HideKeyboard()
			{


#if ANDROID

				var context = Platform.AppContext;
				var inputMethodManager = context.GetSystemService(Android.Content.Context.InputMethodService) as Android.Views.InputMethods.InputMethodManager;
				if (inputMethodManager != null)
				{
					var activity = Platform.CurrentActivity;
					var token = activity?.CurrentFocus?.WindowToken;
					inputMethodManager.HideSoftInputFromWindow(token, Android.Views.InputMethods.HideSoftInputFlags.None);
					activity?.Window?.DecorView?.ClearFocus();
				}
#elif IOS
			 UIApplication.SharedApplication.KeyWindow.EndEditing(true);
			 //UIApplication.SharedApplication.Delegate.GetWindow().EndEditing(true);
#endif


			}
		}



		namespace navigation
		{





			internal abstract class ContentPageWithResult<TResult>() : ContentPage()
			{

				private Func<TResult?, Task>? _onDialogResult;

				public TResult? DialogResult { get; private set; } = default;


				public async Task ShowDialog(bool modal, Func<TResult?, Task> onDialogResult)
				{
					_onDialogResult = onDialogResult;
					if (modal)
					{
						await Shell.Current.Navigation.PushModalAsync(this);
					}
					else
					{
					}
				}

				public async Task SetDialogResult(TResult? result, bool navigateBack = true)
				{
					DialogResult = result;
					if (_onDialogResult != null) await _onDialogResult.Invoke(result);
					if (navigateBack) await Shell.Current.Navigation.PopAsync();
				}
			}



			internal abstract class ParametrizedModalPageBase<Tin, TResult>(Tin? inParameter = default) : ContentPage()
			{
				protected readonly Tin? InputParameter = inParameter;

				public TResult? DialogResult { get; private set; } = default;


				private Func<TResult?, Task>? _onDialogResult;

				public async Task ShowDialog(Func<TResult?, Task> onDialogResult)
				{
					_onDialogResult = onDialogResult;
					await Shell.Current.Navigation.PushModalAsync(this);
				}

				public async Task SetDialogResult(TResult? result, bool navigateBack = true)
				{
					DialogResult = result;
					if (_onDialogResult != null) await _onDialogResult.Invoke(result);
					if (navigateBack) await Shell.Current.Navigation.PopAsync();
				}
			}


		}

	}



	namespace security
	{


		/*
		 Usage:
		var status = await PermissionsHelper.CheckAndRequestPermissionAsync<Permissions.LocationWhenInUse>();
		if (status != PermissionStatus.Granted)
		{
			//whatever you like 
		}
		 */

		/// <summary>https://learn.microsoft.com/ru-ru/dotnet/maui/platform-integration/appmodel/permissions</summary>
		internal static class PermissionsHelper
		{

			public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<TPermission>() where TPermission : BasePermission, new()
			{
				return await MainThread.InvokeOnMainThreadAsync(async () =>
				{
					TPermission permission = new();
					var status = await permission.CheckStatusAsync();
					if (status != PermissionStatus.Granted)
					{
						status = await permission.RequestAsync();
					}

					return status;
				});
			}


#if ANDROID
			[SupportedOSPlatform("android")]
			private class Permission_Biometric_Fingerprint : Permissions.BasePlatformPermission
			{
				public override (string androidPermission, bool isRuntime)[] RequiredPermissions
				{
					get
					{
						List<(string androidPermission, bool isRuntime)> l = new();

#if ANDROID28_0_OR_GREATER
#pragma warning disable CA1416
						l.Add((Android.Manifest.Permission.UseBiometric, true));
#pragma warning restore CA1416
#else
						l.Add((Android.Manifest.Permission.UseFingerprint, true));
#endif

						return l.ToArray();
					}
				}
			}
#endif

			[SupportedOSPlatform("windows10.0.18362")]
			[SupportedOSPlatform("android")]
			internal static async Task<bool> CheckAndRequestPermissionAsync_Biometric_Fingerprint()
			{
#if ANDROID
#pragma warning disable CA1416
				return (await CheckAndRequestPermissionAsync<Permission_Biometric_Fingerprint>()).IsGranted();
#pragma warning restore CA1416
#else
				return (await Permissions.RequestAsync<Permissions.Sensors>()).IsGranted();
#endif
			}


			/*

			internal static async Task<bool> CheckAndRequestPermissionAsync_Biometric_Fingerprint()
				=> (await Permissions.RequestAsync<Permissions.Sensors>()).IsGranted();

			 */

			/*
			public class Permission_Net_AccessNetworkStateAndInternet : Permissions.BasePlatformPermission
			{
				public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
					new List<(string androidPermission, bool isRuntime)>
					{
						(global::Android.Manifest.Permission.AccessNetworkState, true),
						(global::Android.Manifest.Permission.Internet, true)
					}.ToArray();
			}
			 */

			internal static async Task<bool> CheckAndRequestPermissionAsync_Net_AccessNetworkStateAndInternet()
							=> (await Permissions.RequestAsync<Permissions.NetworkState>()).IsGranted();


			[SupportedOSPlatform("Android")]
			public class Permission_Android_ReadWriteStorage : Permissions.BasePlatformPermission
			{
				public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
					new List<(string androidPermission, bool isRuntime)>
					{
						(global::Android.Manifest.Permission.ReadExternalStorage, true),
						(global::Android.Manifest.Permission.WriteExternalStorage, true)
					}.ToArray();
			}






			/*
			private async Task<bool> CheckPermissions()
			{
				PermissionStatus bluetoothStatus = await CheckBluetoothPermissions();
				PermissionStatus cameraStatus = await CheckPermissions<Permissions.Camera>();
				PermissionStatus mediaStatus = await CheckPermissions<Permissions.Media>();
				PermissionStatus storageWriteStatus = await CheckPermissions<Permissions.StorageWrite>();
				//PermissionStatus photosStatus = await CheckPermissions<Permissions.Photos>();
				...

		bool locationServices = IsLocationServiceEnabled();

				return IsGranted(cameraStatus) && IsGranted(mediaStatus) && IsGranted(storageWriteStatus) && IsGranted(bluetoothStatus);
			}
			 */



			/*
		internal static async Task<PermissionStatus> CheckPermission_Bluetooth()
		{

			throw new NotImplementedException();

			PermissionStatus bluetoothStatus = PermissionStatus.Granted;

			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				if (DeviceInfo.Version.Major >= 12)
				{
					bluetoothStatus = await CheckPermissions<BluetoothPermissions>();
				}
				else
				{
					bluetoothStatus = await CheckPermissions<Permissions.LocationWhenInUse>();
				}
			}

			return bluetoothStatus;
		}
			 */

		}




	}





	internal static class Extensions_MAUI_Navigation
	{

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task WaitOneAsync(this WaitHandle waitHandle)
		{
			if (waitHandle == null)
				throw new ArgumentNullException("waitHandle");

			var tcs = new TaskCompletionSource<bool>();
			var rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle,
				delegate { tcs.TrySetResult(true); }, null, -1, true);
			var t = tcs.Task;
			t.ContinueWith((antecedent) => rwh.Unregister(null));
			return t;
		}


		//public class SendItemMessageWrapper<T>(T value) : ValueChangedMessage<T>(value) { }



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eGoToAsync(this string gotoPageName, Dictionary<string, object> inputParams)
			=> await Shell.Current.GoToAsync(gotoPageName, inputParams);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eGoToAsync(this string gotoPageName, string inputParamName, object inputParam)
		{
			Dictionary<string, object> p = new() { { inputParamName, inputParam } };
			await gotoPageName.eGoToAsync(p);
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eGoToWithReturnAsync<TResult>(this ContentPage ctx, string gotoPageName, Action<TResult?> onReturnedValue, Dictionary<string, object>? inputParams = null)
		{

			bool registered = WeakReferenceMessenger.Default.IsRegistered<ValueChangedMessage<TResult>>(ctx);
			if (registered)
			{
				WeakReferenceMessenger.Default.Unregister<ValueChangedMessage<TResult>>(ctx);
			}

			WeakReferenceMessenger.Default.Register<ValueChangedMessage<TResult>>(ctx, (r, m) =>
			{
				onReturnedValue.Invoke(m.Value);
				WeakReferenceMessenger.Default.Unregister<ValueChangedMessage<TResult>>(ctx);
			});


			if (inputParams != null)
				await Shell.Current.GoToAsync(gotoPageName, inputParams);
			else
				await Shell.Current.GoToAsync(gotoPageName);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eGoToWithReturnAsync<TResult>(this ContentPage ctx, string gotoPageName, Action<TResult?> onReturnedValue, string inputParamName, object inputParam)
		{
			Dictionary<string, object> p = new() { { inputParamName, inputParam } };

			await ctx.eGoToWithReturnAsync<TResult>(gotoPageName, onReturnedValue, p);
		}





		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eGoToWithReturnAsync<TResult>(this ContentPage ctx, string gotoPageName, Func<TResult?, Task> onReturnedValue, Dictionary<string, object>? inputParams = null)
		{

			bool registered = WeakReferenceMessenger.Default.IsRegistered<ValueChangedMessage<TResult>>(ctx);
			if (registered)
			{
				WeakReferenceMessenger.Default.Unregister<ValueChangedMessage<TResult>>(ctx);
			}

			WeakReferenceMessenger.Default.Register<ValueChangedMessage<TResult>>(ctx, async (r, m) =>
			{
				try
				{
					await onReturnedValue.Invoke(m.Value);
				}
				finally
				{
					WeakReferenceMessenger.Default.Unregister<ValueChangedMessage<TResult>>(ctx);
				}
			});

			if (inputParams != null)
				await Shell.Current.GoToAsync(gotoPageName, inputParams);
			else
				await Shell.Current.GoToAsync(gotoPageName);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eGoToWithReturnAsync<TResult>(this ContentPage ctx, string gotoPageName, Func<TResult?, Task> onReturnedValue, string inputParamName, object inputParam)
		{
			Dictionary<string, object> p = new() { { inputParamName, inputParam } };
			await ctx.eGoToWithReturnAsync<TResult>(gotoPageName, onReturnedValue, p);
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eReturnAsDialogResult<TResult>(this TResult returnValue)
		{
			WeakReferenceMessenger.Default.Send(new ValueChangedMessage<TResult>(returnValue));
			await Shell.Current.Navigation.PopAsync();
		}








		private static Lazy<Dictionary<ContentPage, object>> _dlgResults
			= new([]);



		/// <summary>To get result from the target ContentPage, 
		/// it must call <see cref="eSetDialogResultAndPopBackAsync"/> to return any dialog result value 
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eShowDialogAsync<TDialogResult>(this ContentPage ctx, bool modal, Func<TDialogResult?, Task> onDialogResult, Func<Task>? onDialogClosedWithoutResult = null)
		{
			ctx.NavigatedFrom += async (_, e) =>
				{

					bool found = false;
					object? foundResult = null;
					lock (_dlgResults) found = _dlgResults.Value.TryGetValue(ctx, out foundResult);
					if (!found)
					{
						//e_SetDialogResultAndPopAsync was not called in closed target Page
						if (onDialogClosedWithoutResult != null) await onDialogClosedWithoutResult.Invoke();
						return;
					}

					//Removing DislogResult objject from dictionary
					lock (_dlgResults) _dlgResults.Value.Remove(ctx);

					//Found resutl!
					TDialogResult? r = (foundResult != null)
						? (TDialogResult?)foundResult
						: default;

					await onDialogResult.Invoke(r);
				};

			//Showing target UI
			if (modal)
			{
				await Shell.Current.Navigation.PushModalAsync(ctx);
			}
			else
			{
				await Shell.Current.Navigation.PushAsync(ctx);
			}
		}





		/// <summary>To get result from the target ContentPage, 
		/// it must call <see cref="eSetDialogResultAndPopBackAsync"/> to return any dialog result value 
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<TDialogResult?> eShowDialogAsync<TDialogResult>(this ContentPage ctx, bool modal)
		{
			AutoResetEvent e = new(false);
			TDialogResult? r = default;
			await ctx.eShowDialogAsync<TDialogResult>(true, async dialogResult =>
			{
				r = dialogResult;
				await Task.Delay(1);
				e.Set();    //Signaling that dialog is closed
			},
			async delegate
			{
				//Login dialog closed unexpectable
				await Task.Delay(1);
				e.Set();    //Signaling that dialog is closed
			});

			await e.WaitOneAsync(); //Waitong while dialog will be closed...
			return r;
		}







		/// <remarks>
		/// THIS MUST NOT BE CALLED FROM <see cref="Page.Appearing"/> event !!!
		/// In 'ContentPage.Appearing' event, Page is not fully initialized and back button Navigation handlers not wokring correctly!
		/// Use it from 'ContentPage.Loaded' or any other handlers
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eSetDialogResultAndPopBackAsync(this ContentPage ctx, object dialogResultObject)
		{
			lock (_dlgResults)
			{
				var dic = _dlgResults.Value;
				if (!dic.TryAdd(ctx, dialogResultObject)) dic[ctx] = dialogResultObject;
			}

			//going Back
			await Shell.Current.Navigation.PopAsync();
		}




	}



	internal static class Extensions_MAUI_Security
	{


		public static bool IsGranted(this PermissionStatus status)
			=> (status == PermissionStatus.Granted || status == PermissionStatus.Limited);


	}



	internal static class Extensions_MAUI_SecureStorage
	{
		#region ISecureStorage 


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<Int32?> eGetAsync_Int32(this ISecureStorage ss, string valueName, Int32? defaultValue = default)
		{
			string? s = await SecureStorage.Default.GetAsync(valueName);
			if (s == null) return defaultValue;
			if (Int32.TryParse(s, out var val)) return val;
			return defaultValue;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eSetAsync(this ISecureStorage ss, string valueName, Int32? Value = default)
		{
			if (Value == null || !Value.HasValue)
			{
				ss.Remove(valueName);
			}
			else
			{
				await ss.SetAsync(valueName, Value.Value.ToString());
			}
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<bool?> eGetAsync_Bool(this ISecureStorage ss, string valueName, bool? defaultValue = default)
		{
			var val = await ss.eGetAsync_Int32(valueName);
			if (!val.HasValue) return defaultValue;
			return val.HasValue
				? (val.Value != 0)
				: defaultValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eSetAsync(this ISecureStorage ss, string valueName, bool? Value = default)
		{
			if (Value == null || !Value.HasValue)
			{
				ss.Remove(valueName);
			}
			else
			{
				Int32 v = Value.Value
					? 1
					: 0;

				await ss.eSetAsync(valueName, v);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<bool> eGetAsync_BoolSafe(this ISecureStorage ss, string valueName, bool defaultValue = default)
		{
			var val = await ss.eGetAsync_Bool(valueName);
			return val.HasValue
				? val.Value
				: false;
		}


		#endregion

	}


	/*
	public class DialogService : IDialogService
	{
	public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
	{
	return await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
	}

	public async Task<bool> DisplayConfirm(string title, string message, string accept, string cancel)
	{
	return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
	}
	}
	 */



	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static class Extensions_UI_MAUI
	{




		//MsgBoxFlags? flags = MsgBoxFlags.Btn_OK | MsgBoxFlags.Icn_Information | MsgBoxFlags.DefBtn_1,



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eToastShow(this string msg, int toastFontSize = 14)
		{
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				//CancellationTokenSource cancellationTokenSource = new();
				await Toast
					.Make(msg, CommunityToolkit.Maui.Core.ToastDuration.Long, toastFontSize)
					.Show();

			});
		}




		[DebuggerNonUserCode, DebuggerStepThrough]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eMsgboxShow(
			this string msg,
			string? title = null,
			string okButtonText = "OK",
			Page? ctx = null)
		{
			ctx ??= uom.AppInfo.CurrentUIContext!;
			title ??= uom.AppInfo.TitleShort;

			//flags ??= (MsgBoxFlags.Btn_OK | MsgBoxFlags.Icn_Information | MsgBoxFlags.DefBtn_1);
			//			var ff = ParseMsgboxFlags(flags.Value);
			await ctx!.DisplayAlert(title, msg, okButtonText);

		}

		[DebuggerNonUserCode, DebuggerStepThrough]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<bool> eMsgboxAskIsYes(
			this string question,
			string? title = null, string? yes = "Yes", string? no = "NO",

			Page? ctx = null)
		{
			ctx ??= uom.AppInfo.CurrentUIContext!;
			title ??= uom.AppInfo.TitleShort;

			/*
			MsgBoxFlags flg = MsgBoxFlags.Btn_YesNo | MsgBoxFlags.Icn_Question
				| (defButtonYes
				? MsgBoxFlags.DefBtn_1
				: MsgBoxFlags.DefBtn_2);
			 */
			//var dd = await ctx!.DisplayAlert(title, msg, okButtonText, "asd");
			return await ctx!.DisplayAlert(title, question, yes, no);
		}


		[DebuggerNonUserCode, DebuggerStepThrough]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task eMsgboxError(this Exception ex, string? title = C_FAILED_TO_RUN)
		{
			await ex.Message.eMsgboxShow(title);
		}


		#region runDelayed 

		internal const int DEFAULT_DELAY = 100;
		internal const int DEFAULT_FORM_SHOWN_DELAY = 500;



		/// <summary>
		/// Usually used when you need to do an action with a slight delay after exiting the current method. 
		/// For example, if some data will be ready only after exiting the control event handler processing branch
		/// </summary>
		/// <returns>
		/// You must save anywhere returned timer to avoid it grom GC collection
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static System.Threading.Timer eRunDelayedTask(this Func<Task> delayedAction, int delay = DEFAULT_DELAY)
		{

			System.Threading.TimerCallback tc = new(async s =>
			{
				System.Threading.Timer tmr = (System.Threading.Timer)s!;
				tmr.Dispose();

				//Now start action incontrols UI thread
				await delayedAction.Invoke();
			});

			System.Threading.Timer tmrDelay = new(tc, null, delay, Timeout.Infinite);

			//We need to avoid dispose timer after exit this proc
			return tmrDelay;
		}


		/// <summary>
		/// Usually used when you need to do an action with a slight delay after exiting the current method. 
		/// For example, if some data will be ready only after exiting the control event handler processing branch
		/// </summary>
		/// <returns>
		/// You must save anywhere returned timer to avoid it grom GC collection
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static System.Threading.Timer eRunDelayed(this Action delayedAction, int delay = DEFAULT_DELAY)
		{
			System.Threading.TimerCallback tc = new(s =>
			{
				System.Threading.Timer tmr = (System.Threading.Timer)s!;
				tmr.Dispose();

				//Now start action incontrols UI thread
				delayedAction.Invoke();
			});

			System.Threading.Timer tmrDelay = new(tc, null, delay, Timeout.Infinite);

			//We need to avoid dispose timer after exit this proc
			return tmrDelay;
		}


		private static Lazy<ConcurrentDictionary<System.Guid, System.Threading.Timer>> _timersStorage = new([]);

		/// <summary>
		/// Usually used when you need to do an action with a slight delay after exiting the current method. 
		/// For example, if some data will be ready only after exiting the control event handler processing branch
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eRunDelayedInUIThread(this Action delayedAction, int delay = DEFAULT_DELAY)
		{
			var id = System.Guid.NewGuid();

			void uiThreadAction()
			{
				var dic = _timersStorage.Value;
				bool removed = dic.TryRemove(id, out var tmr);
				MainThread.BeginInvokeOnMainThread(delayedAction);
			}

			Action a = uiThreadAction;
			var tmr = a.eRunDelayed(delay);

			var dic = _timersStorage.Value;
			bool added = dic.TryAdd(id, tmr);
		}

		/// <inheritdoc cref="eRunDelayedInUIThread(Action, int)" />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eRunDelayedInUIThread(this Func<Task> delayedAction, int delay = DEFAULT_DELAY)
		{
			var id = System.Guid.NewGuid();

			async Task uiThreadAction()
			{
				var dic = _timersStorage.Value;
				bool removed = dic.TryRemove(id, out var tmr);
				await MainThread.InvokeOnMainThreadAsync(delayedAction);
			}

			var t = uiThreadAction;
			var tmr = t.eRunDelayedTask(delay);

			var dic = _timersStorage.Value;
			bool added = dic.TryAdd(id, tmr);
		}




		/// <summary>Executes 'delayedAction' after Page load (before it will displayed).
		/// Delay starts after 'Page.Loaded' event</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eRunDelayed_OnLoaded(
			this ContentPage ctx,
			IEnumerable<Func<Task>> delayedTasks,
			int delay = DEFAULT_FORM_SHOWN_DELAY,
			bool onErrorShowUI = true,
			Func<Exception, Task>? onError = null)
		{
			async Task onLoaded()
			{
				await Task.Delay(delay);
				try
				{
					foreach (var tsk in delayedTasks) await tsk.Invoke();
				}
				catch (OperationCanceledException) { }                 //catch (TaskCanceledException tcex) { }
				catch (Exception ex)
				{
					await ex.eLogError(onErrorShowUI);
					if (onError != null) await onError.Invoke(ex);
				}
			}
			ctx.Loaded += async (_, _) => await onLoaded();
		}


		/// <inheritdoc cref="eRunDelayed_OnLoaded(ContentPage, IEnumerable{Func{Task}}, int, bool, Func{Exception, Task})" />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eRunDelayed_OnLoaded(
			this ContentPage ctx,
			Func<Task> delayedTask,
			int delay = DEFAULT_FORM_SHOWN_DELAY,
			bool onErrorShowUI = true,
			Func<Exception, Task>? onError = null
			)
				=> ctx.eRunDelayed_OnLoaded([delayedTask], delay, onErrorShowUI, onError);



		/// <inheritdoc cref="eRunDelayed_OnLoaded(ContentPage, IEnumerable{Func{Task}}, int, bool, Func{Exception, Task})" />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eRunDelayed_OnLoaded(
			this ContentPage ctx,
			Action delayedAction,
			int delay = DEFAULT_FORM_SHOWN_DELAY,
			bool onErrorShowUI = true,
			Func<Exception, Task>? onError = null)
		{
			async Task t()
			{
				await Task.Delay(1);
				delayedAction.Invoke();
			}
			ctx!.eRunDelayed_OnLoaded(t, delay, onErrorShowUI, onError);
		}






		#endregion



		/* 	 


		internal static Task<bool> TouchHold(this View element, TimeSpan duration)
		{

			Timer timer = new(delegate
			{
				element.PreviewMouseUp -= touchUpHandler;
				timer.Stop();
				task.SetResult(true);
			});

			TaskCompletionSource<bool> task = new < bool > ();
			timer.Interval = duration;

			MouseButtonEventHandler touchUpHandler = delegate
			{
				timer.Stop();
				if (task.Task.Status == TaskStatus.Running)
				{
					task.SetResult(false);
				}
			};

			element.PreviewMouseUp += touchUpHandler;

			timer.Tick += delegate
			{
				element.PreviewMouseUp -= touchUpHandler;
				timer.Stop();
				task.SetResult(true);
			};

			timer.Start();
			return task.Task;
		}

		//Great piece of code.I add just an example usage for completeness:
		private async void btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (await TouchHold(btn, TimeSpan.FromSeconds(2)))
			{
				// todo: long press code goes here
			}
		}
		//And from XAML:
		<Button Name = "btn" PreviewMouseDown="btn_PreviewMouseDown">Press long</Button>


		 */




		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ExecuteMTSafe(this System.Windows.Input.ICommand cmd, object o)
			=> MainThread.BeginInvokeOnMainThread(() => cmd?.Execute(o));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static CancellationTokenSource ExecuteForBindedAnimation(this System.Windows.Input.ICommand cmd)
		{
			CancellationTokenSource cts = new();
			cmd?.ExecuteMTSafe(cts.Token);
			return cts;
		}

	}


	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static class Extensions_Errors_MAUI
	{


		/*

			MessageBoxIcon icon = MessageBoxIcon.Error,
			MessageBoxButtons btn = MessageBoxButtons.OK,

	*/




		/// <summary>Фиксация ошибки в журнале, в DEBUG, вывод сообщения</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string eLogErrorNoUI(this Exception ex,
			[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
		{

			string msg = ex.Message;
#if DEBUG
			string errorDump = ex.eFullDump(callerName, callerFile, callerLine);
			$"{CS_SEPARATOR_10}\n{errorDump}\n{CS_SEPARATOR_10}".eDebugWriteLine();

			//Display Extened data in DEBUG mode
			msg += $"\n{CS_SEPARATOR_10}\nUOM DEBUG-MODE DETAILED ERROR INFO:\n{errorDump}";

			Debug.WriteLine(msg);

#elif ANDROID
			string tag = $"{AppInfo.Title ?? string.Empty} {AppInfo.AssemblyFileVersionAttribute ?? string.Empty}";
			Android.Util.Log.Error(tag, msg);
#endif
			return msg;
		}


		/// <summary>Фиксация ошибки в журнале, в DEBUG, вывод сообщения</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static async Task eLogError(this Exception ex,
			bool showMessageBox,
			string modalMessageBoxTitle = C_FAILED_TO_RUN,
			bool supressAnyModalPopEvenInDEBUG = false,
			Page? ctx = null,
			[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
		{

			string msg = ex.eLogErrorNoUI(callerName, callerFile, callerLine);

			if (showMessageBox || !supressAnyModalPopEvenInDEBUG)
			{

				MainThread.BeginInvokeOnMainThread(async () =>
				{
					if (showMessageBox) // Надо показать на экране модальное Сообщение об ошибке
					{
						await msg.eMsgboxShow(modalMessageBoxTitle, ctx: ctx);
					}
					else
					{
#if DEBUG
						if (!supressAnyModalPopEvenInDEBUG)
						{
							//В DEBUG режиме показываем модальное окно с ошибкой, если прямо не запрещено!
							await msg.eMsgboxShow(modalMessageBoxTitle, ctx: ctx);
						}
#endif
					}
				});
			}
			await Task.Delay(1);
		}


		/// <summary>Фиксация ошибки в журнале, в DEBUG, вывод сообщения</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void eLogErrorToast(this Exception ex,
			int toastFontSize = 14,
			[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
		{
			//Write rrror to device log
			ex.eLogErrorNoUI();

			MainThread.BeginInvokeOnMainThread(async () =>
			{
				//CancellationTokenSource cancellationTokenSource = new();
				await Toast
					.Make(ex.Message.Trim(), CommunityToolkit.Maui.Core.ToastDuration.Long, toastFontSize)
					.Show();

			});
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string eFullDump(this Exception? ex,
			[CallerMemberName] string callerName = "",
			[CallerFilePath] string callerFile = "",
			[CallerLineNumber] int callerLine = 0)
		{
			if (ex == null) return string.Empty;

			StringBuilder sbExceptionTree = new();
			using StringWriter sw = new(sbExceptionTree);
			try
			{

				sw.WriteLine($"{ex.GetType()}: '{ex.Message}'");
				sw.WriteLine($"Caller: '{callerName}', File: '{callerFile}', Line: {callerLine}");
				sw.WriteLine($"StackTrace:\n{ex.StackTrace}");


				if (ex.InnerException != null)
				{
					sw.WriteLine($"Exception Stack Tree:");
					while (ex.InnerException != null)
					{
						ex = ex.InnerException;
						sw.WriteLine($"{ex.GetType()}\n{ex.Message}");
					}
				}
			}
			catch { }
			return sbExceptionTree.ToString();
		}


	}





}
