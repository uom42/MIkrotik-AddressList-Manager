using System.Data;

using MALM.Model;

using MikrotikDotNet.Model.IP.ARP;

namespace MALM.UI
{
	public partial class LocalSubnetBrowserUI : Form
	{

		private readonly MKConnection _connection;

		public LocalSubnetBrowserUI(MKConnection c) : base()
		{
			_connection = c;

			InitializeComponent();

			this.MinimizeBox = false;

			LocalizeUI();

			this.Load += async (s, e) => await OnLoad();

		}

		private void LocalizeUI()
		{

#if WINDOWS
			Text = $"{Application.ProductName} ({_connection.Host}) ARP List";


			colAddress.Text = L_DEVICE_ADDRESS;
			colMAC.Text = "MAC";
			colComment.Text = L_COMMENT;
			colCreated.Text = L_CREATED;
			colID.Text = L_ID;
			lvwRows.EmptyText = L_INITIALIZING;

			/*
			btnRows_Refresh.Text = L_REFRESH;
			btnRows_Enable.Text = L_ENABLE;
			btnRows_Disable.Text = L_DISABLE;
			 */

#else
		Title = _connection.Host;
		btnExitApp.Text = L_APP_EXIT;

		lvwRows.EmptyView(new Label()
		   .Text(L_LIST_IS_EMPTY)
		   .TextCenterVertical()
		   .TextCenterHorizontal()
		   );
#endif

			//btnRows_Add.Text = L_ADD;
		}

		private async Task OnLoad()
		{

			lvwRows.e_ClearItems();
#if WINDOWS
			lvwRows.GroupCollapsedStateChanged += (_, _) => lvwRows.SaveAllGroupsCollapsedStates(dataID: "ARP_List");


			//toolBarMain.e_EnableItems(false);
			//txtFilter.e_AttachDelayedFilter(OnFilterChanged, cueBanner: L_FILTER);
			//this.e_RunDelayed_OnShown(QueryDataFromDevice);
			//await Task.Delay(1);
#else


		//await RefreshList();
#endif

			//Query router for data

			/*
			//IP-блок 1.0.0.0/8
			uom.Network.IP4AddressWithMask ipmRecursiveCloudFlareDNS = new(IPAddress.Parse("1.0.0.0"), 8);
			var ip1 = IPAddress.Parse("1.1.1.1");
			bool b = ip1.e_IsInSubnet(ipmRecursiveCloudFlareDNS);
			 */

			var ds = await MikrotikDotNet.Model.System.DeviceStatus.GetDeviceStatusAsync(_connection!);


			string[] bridgeNames = [..
				(await MikrotikDotNet.Model.Interface.Bridge.BridgeItem.GetItemsAsync(_connection!))
				.Where(br => br.Enabled && br.Running)
				.Select(br => br.Name.ToUpper())
				];

			var mikrotikNetworks = (await MikrotikDotNet.Model.IP.AddressList.AddressItem.GetItemsAsync(_connection!))
				.Where(adr => adr.Address_Parsed != null && bridgeNames.Contains(adr.Interface.ToUpper()))
				.ToArray();

			var dhcpRows = (await MikrotikDotNet.Model.IP.DHCPServer.LeaseItem.GetItemsAsync(_connection!))
				.Where(r =>
						r.Status == MikrotikDotNet.Model.IP.DHCPServer.LeaseItem.STATUSES.bound
						&& r.ActiveMacAddress != null
						&& r.ActiveAddress != null
						&& r.LastSeen.HasValue)
				//.OrderBy(dhcp => dhcp.ActiveAddress_Parsed!.e_ToUInt32CalculableOrder()!)
				.ToArray();


			var l = dhcpRows
				.Select(dhcp => new LocalLANHost(dhcp))
				.ToList();


			IPAddress[] dhcpIPList = [.. dhcpRows.Select(dhcp => dhcp.ActiveAddress!)];

			var arpRows = (await MikrotikDotNet.Model.IP.ARP.ARPItem.GetItemsAsync(_connection!))
				.Where(arp => arp.Status.e_EqualsOneOf(ARPItem.STATUSES.reachable, ARPItem.STATUSES.stale))
				.Where(arp => arp.Complete && !arp.Invalid && arp.Address != null && arp.MacAddress != null)
				.Where(arp => !dhcpIPList.Contains(arp.Address!))
				.Where(arp =>
				{
					var arpIP = arp.Address!;
					var iip = mikrotikNetworks.FirstOrDefault(subnetIP => arpIP.e_IsInSubnet(subnetIP.Address_Parsed!));
					return iip != null;
				})
				.ToArray();

			arpRows.e_ForEach(arp =>
			{
				l.Add(new LocalLANHost(arp));
			});

			var query = l
				.OrderBy(r => r.IP.e_ToUInt32CalculableOrder())
				.ToArray();

			var rows = query
				.Select(r => AddRecordToList(r, false))
				.ToArray();


			//lvwRows.e_runOnLockedUpdate(delegate
			//{
			lvwRows.RestoreAllGroupsCollapsedStateFromStorage(dataID: "ARP_List");
			lvwRows.e_AddItems(rows, true);
			//});
			//OnDeviceSelected();
		}





		private uom.controls.ListViewItemT<LocalLANHost> AddRecordToList(LocalLANHost r, bool add)
		{
			var li = new uom.controls.ListViewItemT<LocalLANHost>(r);
			li.e_AddFakeSubitems(lvwRows);
			if (add)
			{
				lvwRows.Items.Add(li);
			}
			UpdateRecordInList(li);
			return li;
		}

		private void UpdateRecordInList(uom.controls.ListViewItemT<LocalLANHost> li)
		{
			LocalLANHost abr = li.Value2;
			abr.Update(li, lvwRows);

			/*
			string name = string.Empty;
			if (abr.HostName.e_IsNotNullOrWhiteSpace()) name = abr.HostName;
			if (abr.Comment.e_IsNotNullOrWhiteSpace())
			{
				if (name.e_IsNotNullOrWhiteSpace())
					name += $" ({abr.Comment})";
				else
					name = abr.Comment;
			}

			li.e_UpdateTexts(
				  abr.IP.ToString(),
				  name,
				  abr.MAC.ToString(),
				  abr.LastSeen
				  );

			li.Group = lvwRows.e_GroupsCreateGroupByKey(
				abr.Source.ToString(),
				newGroupState: ListViewGroupCollapsedState.Expanded)
			.Group;
			 */

			//lvwRows.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}





	}
}
