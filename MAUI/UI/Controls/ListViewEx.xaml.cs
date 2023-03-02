namespace MALM.Pages.Controls;


public partial class ListViewEx : ListView
{
	public ListViewEx()
	{
		InitializeComponent();

		//CachingStrategy = ListViewCachingStrategy.RetainElement;
		IsGroupingEnabled = true;
		SelectionMode = ListViewSelectionMode.None;
		SeparatorVisibility = SeparatorVisibility.Default;
	}
}