
using MvvmHelpers;


namespace uom.controls.MAUI.CollectionViews;


internal class CollectionViewExExpandableGroupsCollection<TRow> : ObservableRangeCollection<TRow>, INotifyPropertyChanged
{

	private readonly CollectionView _cvControl;

	private readonly string _name = string.Empty;
	private readonly string _emptyGroupNamePlaceholder;

	private readonly List<TRow> _rowsCache = [];

	private bool _collapsed = false;



	public CollectionViewExExpandableGroupsCollection(CollectionView cv,
		string name,
		IEnumerable<TRow> rows,
		string emptyGroupNamePlaceholder,
		bool defaultCollapsedStateIfSettingsNotFound,
		bool? overrideSettingsCollapsedState = null
		) : base()
	{
		_cvControl = cv;
		_name = name;
		_emptyGroupNamePlaceholder = emptyGroupNamePlaceholder;

		_rowsCache = [.. rows];

		if (overrideSettingsCollapsedState.HasValue)
		{
			_collapsed = overrideSettingsCollapsedState.Value;
		}
		else
		{
			try
			{
				string prefKey = CollapsedStatePrefKey;
				if (Preferences.ContainsKey(prefKey)) _collapsed = Preferences.Get(prefKey, defaultCollapsedStateIfSettingsNotFound);
			}
			catch { _collapsed = defaultCollapsedStateIfSettingsNotFound; }
		}

		ApplyCollapsedState(_collapsed);
	}


	/// <summary>Real group Name (may be emptyString)</summary>
	public string Name => _name;


	/// <summary>Returns Name or 'emptyGroupNamePlaceholder' is name empty</summary>
	public string DisplayName => string.IsNullOrWhiteSpace(_name)
			? _emptyGroupNamePlaceholder
			: _name;

	public int CachedItemsCount => _rowsCache.Count();

	public string ItemsCountString => $"({CachedItemsCount})";


	public override string ToString() => $"{Name} ({ItemsCountString})";




	#region Collapsing / Expanding

	/// <summary>Get 'Key' for get Group Collapsed State from Preferences</summary>
	protected virtual string CollapsedStatePrefKey
	{
		get
		{
			string gName = _name.ToLower().Trim();
			string prefKey = $"{GetType().Name}_{gName}";
			return prefKey;
		}
	}


	public bool IsCollapsed
	{
		get => _collapsed;
		set
		{
			if (value == _collapsed) return;

			try
			{
				Preferences.Set(CollapsedStatePrefKey, value);
			}
			catch { }

			_collapsed = value;
			//SetField(ref _collapsed, value);
			//OnPropertyChanged(new PropertyChangedEventArgs(nameof(NotCollapsed)));

			ApplyCollapsedState(value);
		}
	}

	/// <summary>Used for second group header Collapse/Expand glyph for MAXML</summary>
	public bool NotCollapsed => !IsCollapsed;


	/// <summary>Removing all group Items from CollectionView when group collapsed and add all items back when expanded</summary>
	private void ApplyCollapsedState(bool collapsed)
	{
		if (this.Count > 0) this.Clear();
		if (collapsed) return;

		#region for ObservableRangeCollection

		base.AddRange(_rowsCache.ToArray());

		#endregion

		#region for for ObservableCollection

		/*
		
		foreach (var r in _rows)
			this.Add(r.Clone());
		 */

		#endregion
	}


	/// <summary>Reverse group collapsed state</summary>
	/// <returns>Return True if need to rebing data</returns>
	internal bool SwitchCollapsed()
	{
		bool groupNeedExpand = IsCollapsed;
		if (groupNeedExpand)
		{
			//We will expand group

			/*
			 To avoid: Java.Lang.IndexOutOfBoundsException
				Message=Inconsistency detected.Invalid view holder adapter positionTemplatedItemViewHolder{ 1a890d4 position = 53 id = -1, oldPos = 9, pLpos: 9 scrap[attachedScrap] tmpDetached no parent}
				crc645d80431ce5f73f11.MauiRecyclerView_3{5afd16 VFED..... ......ID 0,0-1058,2032}, adapter:crc645d80431ce5f73f11.ReorderableItemsViewAdapter_2@2175d46, layout:androidx.recyclerview.widget.LinearLayoutManager @f9bf7ab, context:crc6425204e64db84d535.MainActivity@5a366aa

				this error occurs when adding many items to CollectionView, and the animation of adding some of them is not finished before added next ones...

				to adoid this - We detaching dataSource fromt our CollectionView, adding rows to 'offline' group, and than rebind them back to control
			 */
		}
		_cvControl.ItemsSource = null;
		IsCollapsed = !IsCollapsed;
		return true;// groupNeedExpand;
	}



	#endregion



	#region overrides


	//protected override void RemoveItem(int index)	{			//base.RemoveItem(index);	}
	public new bool Remove(TRow item)
	{
		_rowsCache.Remove(item);
		base.Remove(item);
		OnPropertyChanged(new PropertyChangedEventArgs("ItemsCountString"));
		return true;
	}


	public new void Add(TRow item)
	{
		_rowsCache.Add(item);
		base.Add(item);
		OnPropertyChanged(new PropertyChangedEventArgs("ItemsCountString"));
	}


	#endregion


	/*
	/// <summary>from INotifyPropertyChanged</summary>
	protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
	{
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;
		OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		return true;
	}
	 */
}

