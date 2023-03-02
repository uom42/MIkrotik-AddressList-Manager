using static MALM.Localization.LStrings;

using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;

using MALM.UI;

using MauiIcons;
using MauiIcons.SegoeFluent;
using MauiIcons.Fluent;
using MauiIcons.Material;
using MauiIcons.Material.Outlined;
using MauiIcons.Material.Rounded;
using MauiIcons.Material.Sharp;
using MauiIcons.Cupertino;
using MauiIcons.FontAwesome.Brand;
using MauiIcons.FontAwesome.Solid;



namespace MALM;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder()
			.UseMauiApp<App>()

			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})

			.UseSegoeFluentMauiIcons()
			.UseFluentMauiIcons()
			.UseMaterialMauiIcons()
			.UseMaterialRoundedMauiIcons()
			.UseMaterialSharpMauiIcons()
			.UseMaterialOutlinedMauiIcons()
			.UseCupertinoMauiIcons()
			.UseFontAwesomeBrandMauiIcons()
			.UseFontAwesomeBrandMauiIcons()
			.UseFontAwesomeSolidMauiIcons()
			.UseFontAwesomeBrandMauiIcons()
		;

		builder.Services.AddTransient<uom.maui.IGetDeviceInfo, uom.maui.GetDeviceInfo>();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		//builder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);
		//builder.Services.AddSingleton<MasterKeyUI>();
		return builder.Build();
	}
}
