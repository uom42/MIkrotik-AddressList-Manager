using static MALM.Localization.LStrings;

using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using MauiIcons.Material.Outlined;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using MALM.UI;



namespace MALM;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder()
			.UseMauiApp<App>()


			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitCore()
			.UseMauiCommunityToolkitMediaElement()

			.UseMaterialOutlinedMauiIcons()

			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddTransient<uom.maui.IGetDeviceInfo, uom.maui.GetDeviceInfo>();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		//builder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);
		//builder.Services.AddSingleton<MasterKeyUI>();
		return builder.Build();
	}
}
