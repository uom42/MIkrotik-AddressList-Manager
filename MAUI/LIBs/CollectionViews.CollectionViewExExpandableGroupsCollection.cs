
using Android.Service.Voice;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MvvmHelpers;

using uom.maui;


namespace uom.controls.MAUI.CollectionViews;


//public abstract partial class ExpandableGroupsCollection<TRow> : ObservableRangeCollection<TRow>, INotifyPropertyChanged
internal abstract partial class ExpandableGroupsCollection<TRow> : IList<TRow>, INotifyCollectionChanged, INotifyPropertyChanged
{
	private readonly CollectionView _cvControl;

	private readonly string _name = string.Empty;
	private readonly string _emptyGroupNamePlaceholder;

	private readonly List<TRow> _rowsCache = [];
	private readonly List<TRow> _rowsVisible = [];

	private InvertableBool _collapsed = false;

	public event PropertyChangedEventHandler? PropertyChanged;
	public event NotifyCollectionChangedEventHandler? CollectionChanged;

	public ExpandableGroupsCollection(CollectionView cv,
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

	/// <summary>Returns 'real' count of group items</summary>
	public int CachedItemsCount => _rowsCache.Count();

	public override string ToString() => $"{Name} ({CachedItemsCount})";




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


	public InvertableBool IsCollapsed
	{
		get => _collapsed;
		set
		{
			if (value == _collapsed) return;

			try
			{
				Preferences.Set(CollapsedStatePrefKey, value.Value);
			}
			catch { }

			_collapsed = value;
			MainThread.BeginInvokeOnMainThread(() =>
			{
				PropertyChanged?.Invoke(this, new(nameof(IsCollapsed)));
			});

			ApplyCollapsedState(value);
		}
	}


	/// <summary>Collapsing-Expanding implementation is:<br></br>
	/// Removing all group items from the CollectionView when group collapsed and add them back when expanded</summary>
	private void ApplyCollapsedState(bool collapsed)
	{
		//_cvControl.BatchBegin();
		try
		{
			if (collapsed)
			{
				/*
				Clear();
				 */

				MainThread.BeginInvokeOnMainThread(() =>
				{
					//var killRows = _rowsVisible;
					//CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Remove, killRows));
					_rowsVisible.Clear();
					CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
				});
			}
			else
			{
				MainThread.BeginInvokeOnMainThread(() =>
				{
					_rowsVisible.Clear();
					_rowsVisible.AddRange(_rowsCache);
					CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));

					//var addRows = _rowsCache.ToArray();
					//CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add, addRows));
				});

				//_rowsVisible.AddRange(_rowsCache.ToArray());
				//OnCollectionChanged(NotifyCollectionChangedAction.Reset);
			}
		}
		finally
		{
			//_cvControl.BatchCommit();
		}
	}


	#endregion

	#region overrides

	/*
	public new bool Remove(TRow item)
	{
		_rowsCache.Remove(item);
		base.Remove(item);
		OnPropertyChanged(new PropertyChangedEventArgs(nameof(CachedItemsCount)));
		return true;
	}


	public new void Add(TRow item)
	{
		_rowsCache.Add(item);
		base.Add(item);
		OnPropertyChanged(new PropertyChangedEventArgs(nameof(CachedItemsCount)));
	}
	 */


	#endregion


	[RelayCommand]
	private async Task SwitchExpandedState()
	{
		//await Task.Delay(400);

		await MainThread.InvokeOnMainThreadAsync(() =>
		{
			//await Toast.Make("ChangeGroupExpandedState", CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();

			//	_cvControl.BatchBegin();
			try
			{



				bool expandingMode = _collapsed;

				/*
				 To avoid: Java.Lang.IndexOutOfBoundsException
					Message=Inconsistency detected.Invalid view holder adapter positionTemplatedItemViewHolder{ 1a890d4 position = 53 id = -1, oldPos = 9, pLpos: 9 scrap[attachedScrap] tmpDetached no parent}
					crc645d80431ce5f73f11.MauiRecyclerView_3{5afd16 VFED..... ......ID 0,0-1058,2032}, adapter:crc645d80431ce5f73f11.ReorderableItemsViewAdapter_2@2175d46, layout:androidx.recyclerview.widget.LinearLayoutManager @f9bf7ab, context:crc6425204e64db84d535.MainActivity@5a366aa

					this error occurs when adding many items to CollectionView, and the animation of adding some of them is not finished before added next ones...
					------------------------------------
				 */

				//We detaching dataSource from our CollectionView, adding rows to 'offline' group, and than rebind them back to control

				var src = _cvControl.ItemsSource;

				//if (expandingMode)
				//_cvControl.ItemsSource = null;

				IsCollapsed = !IsCollapsed;

				//if (expandingMode)
				//_cvControl.ItemsSource = src;

			}
			catch { }
			finally
			{
				//	_cvControl.BatchCommit();
			}
		});
	}





	#region Unsupported


	public void Insert(int index, TRow item) => throw new NotImplementedException();
	public void RemoveAt(int index) => throw new NotImplementedException();
	public void CopyTo(TRow[] array, int arrayIndex) => throw new NotImplementedException();



	#endregion





	public IEnumerator<TRow> GetEnumerator() => _rowsVisible.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


	public int Count => _rowsVisible.Count;
	public bool IsReadOnly => false;

	public TRow this[int index]
	{
		get => _rowsVisible[index];
		set
		{
			var oldItem = _rowsVisible[index];
			var n = _rowsCache.IndexOf(oldItem);
			_rowsVisible[index] = value;
			_rowsCache[n] = value;

			MainThread.BeginInvokeOnMainThread(() => CollectionChanged?.Invoke(this,
			 new(NotifyCollectionChangedAction.Replace, value, oldItem)));
		}
	}


	public int IndexOf(TRow item) => _rowsVisible.IndexOf(item);
	public bool Contains(TRow item) => _rowsVisible.Contains(item);

	[Obsolete("Internal use only!", true)]
	void ICollection<TRow>.Clear()
	{
		//_rowsVisible.Clear();
		//OnCollectionChanged(NotifyCollectionChangedAction.Reset);
		//OnCollectionChanged(NotifyCollectionChangedAction.Remove);
	}

	public bool Remove(TRow item)
	{
		_rowsCache.Remove(item);
		if (_rowsVisible.Contains(item)) _rowsVisible.Remove(item);
		OnCollectionChanged_Remove(item);
		return true;
	}

	public void Add(TRow item)
	{
		_rowsCache.Add(item);
		_rowsVisible.Add(item);
		OnCollectionChanged_Add(item);
	}


	private void OnCollectionChanged_Add(TRow row) => MainThread.BeginInvokeOnMainThread(() => CollectionChanged?.Invoke(this,
			 new(NotifyCollectionChangedAction.Add, row)));

	private void OnCollectionChanged_Remove(TRow row) => MainThread.BeginInvokeOnMainThread(() => CollectionChanged?.Invoke(this,
			 new(NotifyCollectionChangedAction.Remove, row)));








	/*
	 * 
	 * 		private void OnCountChanged2222()
	{
		//OnPropertyChanged(new PropertyChangedEventArgs(nameof(CachedItemsCount)));

		//PropertyChanged?.Invoke(this, new(nameof(Count)));
		//CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add(nameof(Count)));
		//ObservableRangeCollection<int> oc = new();
		//oc.CollectionChanged
	}
	 * 
	private void OnCollectionChanged_(NotifyCollectionChangedAction a)
	{
		//OnPropertyChanged(new PropertyChangedEventArgs(nameof(CachedItemsCount)));

		MainThread.BeginInvokeOnMainThread(() =>
		{


			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(a));

			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(a));


		});

		//ObservableRangeCollection<int> oc = new();
		//oc.CollectionChanged
	}
	 */


	/*

	/// <summary>from INotifyPropertyChanged</summary>
	protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
	{
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;
		//OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		return true;
	}

*/









}

