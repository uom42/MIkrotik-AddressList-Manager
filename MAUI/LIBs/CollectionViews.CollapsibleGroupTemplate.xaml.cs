namespace uom.controls.MAUI.CollectionViews;


public partial class CollapsibleGroupTemplate : ContentView
{
	public event EventHandler<Layout> OnChangeCollapsedState = delegate { };

	public CollapsibleGroupTemplate()
	{
		//Checking reference to MauiIcon (AathifMahir.Maui.MauiIcons.Material.Outlined) http://www.aathifmahir.com/dotnet/2022/maui/icons
		//_ = new MauiIcons.Core.MauiIcon();

		InitializeComponent();

	}
}