
global using uom.Extensions;

global using static uom.constants;
global using static uom.Extensions.Extensions_DebugAndErrors;

using System.Collections.Concurrent;

using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;



#region .NET MAUI Community Toolkit (https://github.com/CommunityToolkit/Maui)



#region Validate user Input

/*				
		https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/behaviors/numeric-validation-behavior
	
	
	 <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
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
            <toolkit:NumericValidationBehavior
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


	public class InvertableBool(bool b)
	{
		private bool _value = b;

		public bool Value { get { return _value; } }
		public bool Invert { get { return !_value; } }


		public static implicit operator InvertableBool(bool b) => new(b);

		public static implicit operator bool(InvertableBool b) => b._value;
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
		public static async Task e_GoToAsync(this string gotoPageName, Dictionary<string, object> inputParams)
			=> await Shell.Current.GoToAsync(gotoPageName, inputParams);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task e_GoToAsync(this string gotoPageName, string inputParamName, object inputParam)
		{
			Dictionary<string, object> p = new() { { inputParamName, inputParam } };
			await gotoPageName.e_GoToAsync(p);
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task e_GoToWithReturnAsync<TResult>(this ContentPage ctx, string gotoPageName, Action<TResult?> onReturnedValue, Dictionary<string, object>? inputParams = null)
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
		public static async Task e_GoToWithReturnAsync<TResult>(this ContentPage ctx, string gotoPageName, Action<TResult?> onReturnedValue, string inputParamName, object inputParam)
		{
			Dictionary<string, object> p = new() { { inputParamName, inputParam } };

			await ctx.e_GoToWithReturnAsync<TResult>(gotoPageName, onReturnedValue, p);
		}





		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task e_GoToWithReturnAsync<TResult>(this ContentPage ctx, string gotoPageName, Func<TResult?, Task> onReturnedValue, Dictionary<string, object>? inputParams = null)
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
		public static async Task e_GoToWithReturnAsync<TResult>(this ContentPage ctx, string gotoPageName, Func<TResult?, Task> onReturnedValue, string inputParamName, object inputParam)
		{
			Dictionary<string, object> p = new() { { inputParamName, inputParam } };
			await ctx.e_GoToWithReturnAsync<TResult>(gotoPageName, onReturnedValue, p);
		}



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task e_ReturnAsDialogResult<TResult>(this TResult returnValue)
		{
			WeakReferenceMessenger.Default.Send(new ValueChangedMessage<TResult>(returnValue));
			await Shell.Current.Navigation.PopAsync();
		}








		private static Lazy<Dictionary<ContentPage, object>> _dlgResults
			= new([]);



		/// <summary>To get result from the target ContentPage, 
		/// it must call <see cref="e_SetDialogResultAndPopBackAsync"/> to return any dialog result value 
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task e_ShowDialogAsync<TDialogResult>(this ContentPage ctx, bool modal, Func<TDialogResult?, Task> onDialogResult, Func<Task>? onDialogClosedWithoutResult = null)
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
		/// it must call <see cref="e_SetDialogResultAndPopBackAsync"/> to return any dialog result value 
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<TDialogResult?> e_ShowDialogAsync<TDialogResult>(this ContentPage ctx, bool modal)
		{
			AutoResetEvent e = new(false);
			TDialogResult? r = default;
			await ctx.e_ShowDialogAsync<TDialogResult>(true, async dialogResult =>
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
		/// Use it from <see cref="ContentPage.Loaded"/> or any other handlers
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task e_SetDialogResultAndPopBackAsync(this ContentPage ctx, object dialogResultObject)
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


	internal static class Extensions_MAUI_SecureStorage
	{
		#region ISecureStorage 


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<Int32?> e_GetAsync_Int32(this ISecureStorage ss, string valueName, Int32? defaultValue = default)
		{
			string? s = await SecureStorage.Default.GetAsync(valueName);
			if (s == null) return defaultValue;
			if (Int32.TryParse(s, out var val)) return val;
			return defaultValue;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task e_SetAsync(this ISecureStorage ss, string valueName, Int32? Value = default)
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
		public static async Task<bool?> e_GetAsync_Bool(this ISecureStorage ss, string valueName, bool? defaultValue = default)
		{
			var val = await ss.e_GetAsync_Int32(valueName);
			if (!val.HasValue) return defaultValue;
			return val.HasValue
				? (val.Value != 0)
				: defaultValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task e_SetAsync(this ISecureStorage ss, string valueName, bool? Value = default)
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

				await ss.e_SetAsync(valueName, v);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<bool> e_GetAsync_BoolSafe(this ISecureStorage ss, string valueName, bool defaultValue = default)
		{
			var val = await ss.e_GetAsync_Bool(valueName);
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


		[DebuggerNonUserCode, DebuggerStepThrough]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task e_MsgboxShow(
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
		public static async Task<bool> e_MsgboxAskIsYes(
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
		public static async Task e_MsgboxError(this Exception ex, string? title = C_FAILED_TO_RUN)
		{
			await ex.Message.e_MsgboxShow(title);
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
		private static System.Threading.Timer e_RunDelayedTask(this Func<Task> delayedAction, int delay = DEFAULT_DELAY)
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
		private static System.Threading.Timer e_RunDelayed(this Action delayedAction, int delay = DEFAULT_DELAY)
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
		internal static void e_RunDelayedInUIThread(this Action delayedAction, int delay = DEFAULT_DELAY)
		{
			var id = System.Guid.NewGuid();

			void uiThreadAction()
			{
				var dic = _timersStorage.Value;
				bool removed = dic.TryRemove(id, out var tmr);
				MainThread.BeginInvokeOnMainThread(delayedAction);
			}

			Action a = uiThreadAction;
			var tmr = a.e_RunDelayed(delay);

			var dic = _timersStorage.Value;
			bool added = dic.TryAdd(id, tmr);
		}

		/// <summary>
		/// Usually used when you need to do an action with a slight delay after exiting the current method. 
		/// For example, if some data will be ready only after exiting the control event handler processing branch
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void e_RunDelayedInUIThread(this Func<Task> delayedAction, int delay = DEFAULT_DELAY)
		{
			var id = System.Guid.NewGuid();

			async Task uiThreadAction()
			{
				var dic = _timersStorage.Value;
				bool removed = dic.TryRemove(id, out var tmr);
				await MainThread.InvokeOnMainThreadAsync(delayedAction);
			}

			var t = uiThreadAction;
			var tmr = t.e_RunDelayedTask(delay);

			var dic = _timersStorage.Value;
			bool added = dic.TryAdd(id, tmr);
		}




		/// <inheritdoc cref="e_RunDelayed_OnLoaded" />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void e_RunDelayed_OnLoaded(
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
				catch (OperationCanceledException ocex) { }                 //catch (TaskCanceledException tcex) { }
				catch (Exception ex)
				{
					await ex.e_LogError(onErrorShowUI);
					if (onError != null) await onError.Invoke(ex);
				}
			}
			ctx.Loaded += async (_, _) => await onLoaded();
		}


		/// <inheritdoc cref="e_RunDelayed_OnLoaded" />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void e_RunDelayed_OnLoaded(
			this ContentPage ctx,
			Func<Task> delayedTask,
			int delay = DEFAULT_FORM_SHOWN_DELAY,
			bool onErrorShowUI = true,
			Func<Exception, Task>? onError = null
			)
				=> ctx.e_RunDelayed_OnLoaded([delayedTask], delay, onErrorShowUI, onError);



		/// <summary>Executes 'delayedAction' after form shown on screen, with specifed delay in UI Thread.
		/// Delay starts after 'Page.Shown' event</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void e_RunDelayed_OnLoaded(
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
			ctx!.e_RunDelayed_OnLoaded(t, delay, onErrorShowUI, onError);
		}






		#endregion















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
		internal static async Task e_LogError(this Exception ex,
			bool showMessageBox,
			string modalMessageBoxTitle = C_FAILED_TO_RUN,
			bool supressAnyModalPopEvenInDEBUG = false,
			Page? ctx = null,
			[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0)
		{
			string errorDump = ex.Message;// .e_FullDump(callerName, callerFile, callerLine);
			string msg = ex.Message;
#if DEBUG
			$"{CS_CONSOLE_SEPARATOR}\n{errorDump}\n{CS_CONSOLE_SEPARATOR}".e_DebugWriteLine();

			//Показываем расширенные данные в DEBUG режиме
			msg += $"\n{CS_CONSOLE_SEPARATOR}\nUOM DEBUG-MODE DETAILED ERROR INFO:\n{errorDump}";
#endif

			if (showMessageBox) // Надо показать на экране модальное Сообщение об ошибке
			{
				await msg.e_MsgboxShow(modalMessageBoxTitle, ctx: ctx);
			}
			else
			{
#if DEBUG
				if (!supressAnyModalPopEvenInDEBUG)
				{
					//В DEBUG режиме показываем модальное окно с ошибкой, если прямо не запрещено!
					await msg.e_MsgboxShow(modalMessageBoxTitle, ctx: ctx);
				}
#endif
			}

		}
	}



}
