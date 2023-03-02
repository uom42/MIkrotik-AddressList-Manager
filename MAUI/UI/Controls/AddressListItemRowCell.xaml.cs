namespace MALM.Pages.Controls;

public partial class AddressListItemRowCell : ViewCell
{
	public AddressListItemRowCell()
	{
		InitializeComponent();
	}


	public static readonly BindableProperty OnProperty =
		BindableProperty.Create("On", typeof(bool), typeof(AddressListItemRowCell), false);

	public bool On
	{
		get { return (bool)GetValue(OnProperty); }
		set { SetValue(OnProperty, value); }
	}



	public static readonly BindableProperty HeaderProperty =
		BindableProperty.Create("Header", typeof(string), typeof(AddressListItemRowCell), string.Empty);

	public string Header
	{
		get { return (string)GetValue(HeaderProperty); }
		set { SetValue(HeaderProperty, value); }
	}

	public static readonly BindableProperty DetailProperty =
		BindableProperty.Create("Detail", typeof(string), typeof(AddressListItemRowCell), string.Empty);

	public string Detail
	{
		get { return (string)GetValue(DetailProperty); }
		set { SetValue(DetailProperty, value); }
	}

}