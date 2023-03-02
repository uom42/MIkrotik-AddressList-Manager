using CommunityToolkit.Maui.Animations;
using CommunityToolkit.Mvvm.Input;

namespace uom.controls.MAUI.CollectionViews;


partial class CollapsibleGroupTemplate : ContentView
{
	//Checking reference to MauiIcon (AathifMahir.Maui.MauiIcons.Material.Outlined) http://www.aathifmahir.com/dotnet/2022/maui/icons
	//_ = new MauiIcons.Core.MauiIcon();

	public CollapsibleGroupTemplate() => InitializeComponent();


	[RelayCommand]
	private async Task Tapped()
	{
		var fa = new FadeAnimation()
		{
			Easing = Easing.Default,
			Length = 100
		};
		await fa.Animate(Ellipse1);
	}
}