namespace uom.controls.MAUI.CollectionViews;


public partial class CollectionViewEx : CollectionView
{


	public event EventHandler<Layout> OnGroupHeaderTap = delegate { };


	public CollectionViewEx() => InitializeComponent();


	protected virtual void OnGroup_OnChangeCollapsedState(object sender, Layout l)
	{
		//=> OnGroupHeaderTap?.Invoke(sender, l);
	}

}