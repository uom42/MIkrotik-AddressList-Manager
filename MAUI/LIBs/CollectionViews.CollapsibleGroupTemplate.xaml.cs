namespace uom.controls.MAUI.CollectionViews;


public partial class CollapsibleGroupTemplate : DataTemplate
{


	public event EventHandler<Layout> OnChangeCollapsedState = delegate { };

	public CollapsibleGroupTemplate()
	{
		//Checking reference to MauiIcon (AathifMahir.Maui.MauiIcons.Material.Outlined) http://www.aathifmahir.com/dotnet/2022/maui/icons
		//_ = new MauiIcons.Core.MauiIcon();

		InitializeComponent();

		//this.
		//lblCollapseGroupGlyph.
		//this.
		/*
				GestureRecognizers >


			< TapGestureRecognizer NumberOfTapsRequired = "1" Tapped = "OnGesture_Tapped" />
		</ StackLayout.GestureRecognizers >

		 */
	}

	protected virtual void OnGesture_Tapped(object sender, TappedEventArgs e)
	{
		/*
		var ctlGroup = sender as Layout;
		if (ctlGroup == null) return;
		OnChangeCollapsedState?.Invoke(sender, ctlGroup);
		 */
	}
}