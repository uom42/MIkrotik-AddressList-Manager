#nullable enable

using System.Drawing;

using common.Controls;

using Extensions;

using malm.Properties;

using Mikrotik.API.Model.IP.Firewall.AddressList;

using MikrotikDotNet;

using uom.Extensions;

namespace malm.Core
{
	public partial class frmMain : Form
	{

		private readonly MKConnection _connection;
		private bool _wasFirstDataGet = false;
		private AddressListItem_ListViewItem[] _mikrotikRows = System.Array.Empty<AddressListItem_ListViewItem>();
		//private ListViewGroup[] _oldGroups = Array.Empty<ListViewGroup>();


		#region Constructors

		/// <summary>Just for UI Designer. Must not be used direct in code</summary>
		public frmMain()
		{
			InitializeComponent();
			this.Load += _Load;
		}


		/// <summary>Creates new instance with specifed Connection object</summary>
		public frmMain(MKConnection c) : this() { _connection = c; }


		#endregion

		/// <summary>Return ID - used for save/restore ListView groups collapsed states</summary>
		private void _Load(object? sender, EventArgs e)
		{
			Text = Application.ProductName + $" ({_connection.Host})";

			Bitmap imgBall_Green = Resources.ball_green16;
			var imgBall_Gray = imgBall_Green.e_GetGrayscaled();

			var iml = new ImageList()
			{
				ImageSize = new Size(16, 16),
				ColorDepth = ColorDepth.Depth32Bit
			};
			iml.Images.Add(AddressListItem_ListViewItem.C_IMAGE_KEY_GRAY, imgBall_Gray);
			iml.Images.Add(AddressListItem_ListViewItem.C_IMAGE_KEY_GREEN, imgBall_Green);

			lvwRows.SmallImageList = iml;
			lvwRows.EmptyText = "Initializing...";

			toolBarMain.e_EnableItems(false);
			this.e_runDelayedOnFormShown(async () => await QueryMKData());

			txtFilter.e_AttachDelayedFilter(OnFilterChanged);

			{
				const string URL_FREE_ICONS = "https://www.flaticon.com/free-icons/google-plus";
				llFreeIcons.Text = $"This programm uses icons from '{URL_FREE_ICONS}";
				llFreeIcons.IsLink = true;
				llFreeIcons.Click += delegate
				{
					try { URL_FREE_ICONS.e_OpenURLInBrowser(); } catch { }
				};
			}

		}


		/// <summary>Queries router for data</summary>
		private async Task QueryMKData()
		{
			this.UseWaitCursor = true;

			_mikrotikRows = Array.Empty<AddressListItem_ListViewItem>();

			lvwRows.EmptyText = "Quering data...";
			lvwRows.e_ClearItems();

			this.toolBarMain.e_EnableItems(false);

			try
			{
				_mikrotikRows = (await AddressListItem.GetItemsAsync(_connection))
					.Where(r => r.Dynamic == false)
					.Select(ip => new AddressListItem_ListViewItem(ip, lvwRows))
					.ToArray();
			}
			/*
			catch (MikrotikConnectionLostException mclex)
			{
				//try to reconnect!
				//await ConnectMikrotik();

			}
			 */
			catch (Exception ex)
			{
				lvwRows.EmptyText = ex.Message;
				ex.FIX_ERROR(true);
			}
			finally
			{

				DisplayFilteredMKData();
				this.UseWaitCursor = false;
			}
		}


		/// <summary>Queries router for data</summary>
		private void UpdateUIState()
		{
			IEnumerable<AddressListItem_ListViewItem> sel = lvwRows.e_SelectedItemsAs<AddressListItem_ListViewItem>();
			btnRows_Enable.Enabled = sel.Any();
			btnRows_Disable.Enabled = sel.Any();

			btnRows_Refresh.Enabled = true;
			btnRows_Add.Enabled = true;

			{//Filter
				lblFilter.Enabled = true;
				txtFilter.Enabled = true;
			}
		}


		/// <summary>Return ID - used for save/restore ListView groups collapsed states</summary>
		private string GetHostDataID() => _connection.Host.ToLower().Trim().GetHashCode().ToString();


		/// <summary>Populates ListView with filtered rows</summary>
		private void DisplayFilteredMKData()
		{
			lvwRows.EmptyText = _mikrotikRows.Any() ? "Filtering data..." : "List is empty";
			try
			{
				string filter = txtFilter.Text;
				var rowsToDisplay = _mikrotikRows
					.Where(r => r.Filter(filter))
					.ToArray();

				if (!rowsToDisplay.Any()) lvwRows.EmptyText = rowsToDisplay.Any() ? "Filtering data..." : $"No rows were found with '{filter}'.";

				lvwRows.e_runOnLockedUpdate(() =>
				{
					lvwRows.e_ClearItems();
					lvwRows.Items.AddRange(rowsToDisplay);

					foreach (AddressListItem_ListViewItem li in lvwRows.e_ItemsAs<AddressListItem_ListViewItem>())
						li.UpdateGroupFromModel(lvwRows);

				}, true, true);

				/*
				lvwRows
					.e_GroupsAsIEnumerable()
					.ToList()
					.ForEach(grp => grp.e_SetStateFlag(ListViewEx.ListViewGroupState.Collapsible));
				 */

				//if (!_wasFirstDataGet) 
				lvwRows.RestoreAllGroupsCollapsedStateFromStorage(dataID: GetHostDataID());
				_wasFirstDataGet = true;

			}
			finally { UpdateUIState(); }
		}


		private async Task SelectedRows_EnableDisable(bool enable)
		{
			this.UseWaitCursor = true;

			try
			{
				var sel = lvwRows.e_SelectedItemsAs<AddressListItem_ListViewItem>();
				if (sel == null || !sel.Any()) return;

				Action<ListViewGroup> a = new Action<ListViewGroup>(grp =>
				{
					//lvwRows.Refresh();
					grp.e_SetState(ListViewEx.ListViewGroupState.Collapsible);
				});

				lvwRows.e_runOnLockedUpdate(async () =>
				{
					foreach (AddressListItem_ListViewItem li in sel)
					{
						var mkRow = li.MikrotikRow;
						mkRow = await mkRow.ReQueryAsync();//Get current row info from kikrotik to avoid modifications of rows with wrong .id
														   //if item with this id will be not found - error will bw thrown and no modification on item with wrong id will be made.

						await mkRow.EnableAsync(enable);//item with this id was found. Make modification
						mkRow = await mkRow.ReQueryAsync();//Again requery item to get new properties
						li.UpdateFromModel(mkRow, lvwRows, onNewGroupAdded: a);
					}
				}, true, true);

			}
			catch (Exception ex)
			{
				ex.FIX_ERROR(true);

				await QueryMKData();//On any error we refill list with actual data from mikrotik
			}
			finally
			{
				this.UseWaitCursor = false;
			}
		}


		private async Task RefreshList()
		{
			if (!_wasFirstDataGet) return;
			await QueryMKData();
		}


		#region UI Controls Events

		private void lwvRows_SelectedIndexChanged(object sender, EventArgs e) => UpdateUIState();

		private async void btnRows_Refresh_Click(object sender, EventArgs e) => await RefreshList();
		private async void lwvRows_Items_NeedRefreshList(object sender, EventArgs e) => await RefreshList();
		private async void btnRows_Enable_Click(object sender, EventArgs e) => await SelectedRows_EnableDisable(true);
		private async void btnRows_Disable_Click(object sender, EventArgs e) => await SelectedRows_EnableDisable(false);
		private void lvwRows_GroupsCollapsedStateChangedByMouse(object sender, string e)
		{
			if (!_wasFirstDataGet) return;
			lvwRows.SaveAllGroupsCollapsedStates(dataID: GetHostDataID());
		}


		private abstract class AddressListSuggestion_MikrotikObject
		{
			public readonly global::Mikrotik.API.Model.ItemBase MikrotikRow;
			public AddressListSuggestion_MikrotikObject(global::Mikrotik.API.Model.ItemBase mkRow) { MikrotikRow = mkRow; }

			public abstract string GetAddress();
		}

		private class AddressListSuggestion_FromOtherGroup : AddressListSuggestion_MikrotikObject
		{
			public global::Mikrotik.API.Model.IP.Firewall.AddressList.AddressListItem MikrotikRow2 => (MikrotikRow as global::Mikrotik.API.Model.IP.Firewall.AddressList.AddressListItem)!;

			public AddressListSuggestion_FromOtherGroup(global::Mikrotik.API.Model.IP.Firewall.AddressList.AddressListItem mkRow) : base(mkRow) { }

			public override string GetAddress() => MikrotikRow2.Address;

			public override string ToString()
			{
				string s = GetAddress();
				if (!string.IsNullOrWhiteSpace(MikrotikRow.Comment)) s += $" | {MikrotikRow.Comment}";
				s += $" | from list: {MikrotikRow2.List}";

				return s;
			}


		}

		private class AddressListSuggestion_DHCPLeaseItem : AddressListSuggestion_MikrotikObject
		{
			public global::Mikrotik.API.Model.IP.DHCPServer.LeaseListItem MikrotikRow2 => (MikrotikRow as global::Mikrotik.API.Model.IP.DHCPServer.LeaseListItem)!;

			public AddressListSuggestion_DHCPLeaseItem(global::Mikrotik.API.Model.IP.DHCPServer.LeaseListItem mkRow) : base(mkRow) { }

			public override string GetAddress() => MikrotikRow2.Address;

			public override string ToString()
			{
				string s = GetAddress();
				if (!string.IsNullOrWhiteSpace(MikrotikRow.Comment)) s += $" | {MikrotikRow.Comment}";
				if (!string.IsNullOrWhiteSpace(MikrotikRow2.HostName)) s += $" | {MikrotikRow2.HostName}";
				s += $" | {MikrotikRow2.MacAddress}";

				s += $" | from DHCP Server: {MikrotikRow2.Server}";

				return s;
			}
		}


		private async void btnRows_Add_Click(object sender, EventArgs e)
		{

			string groupName = string.Empty;
			var sel = lvwRows.e_SelectedItemsAs<AddressListItem_ListViewItem>().FirstOrDefault();
			if (sel != null) groupName = sel.MikrotikRow.List;

			using frmAddressListItemEditor fe = new();

			Func<AddressListSuggestion_MikrotikObject?> getSelectedAddress = new(() =>
			{
				switch (fe.cboAddress.SelectedItem)
				{
					case AddressListSuggestion_MikrotikObject mk: return mk;
					default: return null;
				}
			});

			Func<bool> validateUserInput = new(() =>
			{
				//Validate user input 
				string address = fe.cboAddress.Text.Trim();
				string group = fe.cboGroup.Text.Trim();

				bool canAdd = !string.IsNullOrWhiteSpace(address) && !string.IsNullOrWhiteSpace(group);
				fe.btnAdd.Enabled = canAdd;
				return canAdd;
			});


			validateUserInput.Invoke();

			var allGroups = _mikrotikRows
				.Select(li => li.MikrotikRow.List.Trim())
				.Distinct()
				.OrderBy(s => s)
				.ToArray();

			fe.cboGroup.Items.Clear();
			fe.cboGroup.Items.AddRange(allGroups);
			if (!string.IsNullOrWhiteSpace(groupName)) fe.cboGroup.Text = groupName;



			List<object> addressSuggestionList = new();

			#region Collecting all addreses from other groups

			//Collecting all addreses from other groups
			var a = _mikrotikRows
				.Select(li => li.MikrotikRow)
				.Where(mr => mr.List != groupName)
				.OrderBy(mr => mr.Address).ThenBy(mr => mr.List)
				.Select(mr => new AddressListSuggestion_FromOtherGroup(mr))
				.ToArray();

			addressSuggestionList.AddRange(a);

			#endregion

			#region Append suggestion list from DHCP leases

			//Append suggestion list from DHCP leases
			var DHCPLeaseList = (await Mikrotik.API.Model.IP.DHCPServer.LeaseListItem.GetItemsAsync(_connection))
				.OrderBy(mr => mr.Address)
				.Select(mr => new AddressListSuggestion_DHCPLeaseItem(mr))
				.ToArray();
			addressSuggestionList.AddRange(DHCPLeaseList);

			#endregion

			fe.cboAddress.Items.Clear();
			fe.cboAddress.Items.AddRange(addressSuggestionList.ToArray());
			fe.cboAddress.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			fe.cboAddress.AutoCompleteSource = AutoCompleteSource.ListItems;

			fe.cboAddress.SelectedIndexChanged += delegate
			{
				//If the user selected an item from suggestion List - reset selected object and leave only host address
				var mko = getSelectedAddress.Invoke();
				if (mko != null)
				{
					fe.cboAddress.e_RunDelayed(delegate
					{
						fe.cboAddress.SelectedItem = null;
						fe.cboAddress.Text = mko.GetAddress();
					}, 10);
					fe.txtComment.Text = mko.MikrotikRow.Comment;
				}
			};

			fe.cboAddress.TextChanged += delegate { validateUserInput.Invoke(); };
			fe.btnAdd.Click += async delegate
			{
				if (!validateUserInput.Invoke()) return;

				string address = fe.cboAddress.Text;
				string group = fe.cboGroup.Text;
				string comment = fe.txtComment.Text;

				this.UseWaitCursor = true;

				try
				{
					AddressListItem? newRow = await AddressListItem.Add(_connection, group, address, true, comment);
					if (newRow == null) throw new Exception($"Failed to add new item '{address}'!");
					fe.Tag = newRow;
					fe.DialogResult = DialogResult.OK;
				}
				catch (Exception ex)
				{
					ex.FIX_ERROR(true);
					await QueryMKData();//On any error we refill list with actual data from mikrotik
				}
				finally
				{
					this.UseWaitCursor = false;
				}
			};

			if (fe.ShowDialog(this) != DialogResult.OK) return;

			AddressListItem addedMKRow = (AddressListItem)fe.Tag;
			AddressListItem_ListViewItem li = new(addedMKRow, lvwRows);

			var lmkRows = _mikrotikRows.ToList();
			lmkRows.Add(li);
			_mikrotikRows = lmkRows.ToArray();

			DisplayFilteredMKData();

			li.e_Activate();
		}


		#endregion

		private void OnFilterChanged(string filter)
		{
			if (!_wasFirstDataGet) return;

			DisplayFilteredMKData();
		}
	}



}