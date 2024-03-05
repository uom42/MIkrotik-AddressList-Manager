using Switch = Microsoft.Maui.Controls.Switch;

namespace uom.controls.MAUI;


public partial class SwitchLabel : StackLayout, INotifyPropertyChanged, ISwitch
{
	public event EventHandler<ToggledEventArgs> CheckStateChanged = delegate { };

	private string _text = "Lorem ipsum dolor";

	private readonly Switch _chk;
	private readonly Label _lblText;
	private readonly TapGestureRecognizer _grLabelTap;



	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
		 nameof(IsChecked),
		  typeof(bool),
		   typeof(SwitchLabel),
			false, propertyChanged: (bindable, oldValue, newValue) =>
	{
		((SwitchLabel)bindable).CheckStateChanged?.Invoke(bindable, new ToggledEventArgs((bool)newValue));
		((SwitchLabel)bindable).ChangeVisualState();
		((IView)bindable)?.Handler?.UpdateValue(nameof(ISwitch.TrackColor));

	}, defaultBindingMode: BindingMode.TwoWay);


	public static readonly BindableProperty OnColorProperty = BindableProperty.Create(nameof(OnColor), typeof(Color), typeof(SwitchLabel), null,
		propertyChanged: (bindable, oldValue, newValue) =>
		{
			((IView)bindable)?.Handler?.UpdateValue(nameof(ISwitch.TrackColor));
		});


	public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(SwitchLabel), null);


	public SwitchLabel() : base()
	{
		//InitializeComponent();
		Orientation = StackOrientation.Horizontal;
		VerticalOptions = LayoutOptions.Start;

		_chk = new();
		_lblText = new()
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			HorizontalOptions = LayoutOptions.FillAndExpand,

			HorizontalTextAlignment = TextAlignment.Start,
			VerticalTextAlignment = TextAlignment.Center,
			Text = _text,
		};
		//BackgroundColor = Colors.SlateGrey;

		Children.Add(_chk);
		Children.Add(_lblText);

		_grLabelTap = new() { NumberOfTapsRequired = 1 };
		_grLabelTap.Tapped += OnLabelTapped!;
		_lblText.GestureRecognizers.Add(_grLabelTap);


	}


	#region Properties


	public string Text
	{
		get => _text;
		set
		{
			if (value == _text) return;
			SetField(ref _text, value);
			_lblText.Text = _text;
			//OnPropertyChanged();
		}
	}


	public bool IsChecked
	{
		get => _chk.IsToggled;
		set
		{
			if (value == _chk.IsToggled) return;
			_chk.IsToggled = value;
			OnPropertyChanged();
		}
	}

	/// <summary>Only for compatibility with WinForm</summary>
	public bool Checked
	{
		get => IsChecked;
		set => IsChecked = value;
	}



	public TextAlignment VerticalTextAlignment
	{
		get => _lblText.VerticalTextAlignment;
		set
		{
			if (value == _lblText.VerticalTextAlignment) return;
			_lblText.VerticalTextAlignment = value;
			OnPropertyChanged();
		}
	}

	public TextAlignment HorizontalTextAlignment
	{
		get => _lblText.HorizontalTextAlignment;
		set
		{
			if (value == _lblText.HorizontalTextAlignment) return;
			_lblText.HorizontalTextAlignment = value;
			OnPropertyChanged();
		}
	}






	//public bool IsOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	bool ISwitch.IsOn
	{
		get => IsChecked;
		set => SetValue(IsCheckedProperty, value);
	}
	Color ISwitch.TrackColor
	{
		get
		{
#if WINDOWS
				return OnColor;
#else
			if (IsChecked) return OnColor;
			return null;
#endif
		}
	}

	public Color OnColor
	{
		get { return (Color)GetValue(OnColorProperty); }
		set { SetValue(OnColorProperty, value); }
	}

	public Color ThumbColor
	{
		get { return (Color)GetValue(ThumbColorProperty); }
		set { SetValue(ThumbColorProperty, value); }
	}



	/// <summary>from INotifyPropertyChanged</summary>
	protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
	{
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;
		OnPropertyChanged(propertyName);
		return true;
	}


	#endregion


	#region Gestures

	private void OnLabelTapped(object sender, TappedEventArgs e)
	{
		IsChecked = !IsChecked;
	}

	#endregion

	//protected virtual void OnGroupHeaderTapped(object sender, Layout l)		=> OnGroupHeaderTap?.Invoke(sender, l);


}