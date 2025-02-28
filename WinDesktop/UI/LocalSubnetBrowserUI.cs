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

			lvwRows.eClearItems();
#if WINDOWS
			lvwRows.GroupCollapsedStateChanged += (_, _) => lvwRows.SaveAllGroupsCollapsedStates(dataID: "ARP_List");


#else


		//await RefreshList();
#endif

			//Query router for data

			using var h = _connection!.Open();


			var ds = await MikrotikDotNet.Model.System.DeviceStatus.GetDeviceStatusAsync(_connection!);


			string[] bridgeNames = [..
				(await MikrotikDotNet.Model.Interface.Bridge.BridgeItem.GetItemsAsync(_connection,true ))
				.Where(br => br.Enabled && br.Running)
				.Select(br => br.Name.ToUpper())
				];

			var mikrotikNetworks = (await MikrotikDotNet.Model.IP.AddressList.AddressItem.GetItemsAsync(_connection, true))
				.Where(adr => adr.Address_Parsed != null && bridgeNames.Contains(adr.Interface.ToUpper()))
				.ToArray();

			MikrotikDotNet.Model.IP.DHCPServer.LeaseItem[] dhcpRowsAll = [..
				(await MikrotikDotNet.Model.IP.DHCPServer.LeaseItem.GetItemsAsync(_connection, true))
				.Where(r => r.Enabled)
				];

			MikrotikDotNet.Model.IP.DHCPServer.LeaseItem[] dhcpActiveRows = [.. dhcpRowsAll
				.Where(r =>
						r.Status == MikrotikDotNet.Model.IP.DHCPServer.LeaseItem.STATUSES.Bound
						&& r.ActiveMacAddress != null
						&& r.ActiveAddress != null
						&& r.LastSeen.HasValue)                                ];


			var l = dhcpActiveRows
				.Select(dhcp => new LocalLANHost(dhcp))
				.ToList();


			IPAddress[] dhcpActiveIPList = [.. dhcpActiveRows.Select(dhcp => dhcp.ActiveAddress!)];

			var arpRows = (await MikrotikDotNet.Model.IP.ARP.ARPItem.GetItemsAsync(_connection, true))
				.Where(arp => arp.Dynamic)
				.Where(arp => arp.Status.eEqualsOneOf(ARPItem.STATUSES.Reachable, ARPItem.STATUSES.Stale))
				.Where(arp => arp.Complete && !arp.Invalid && arp.Address != null && arp.MacAddress != null)
				.Where(arp => !dhcpActiveIPList.Contains(arp.Address!))
				.Where(arp =>
				{
					var arpIP = arp.Address!;
					var iip = mikrotikNetworks.FirstOrDefault(subnetIP => arpIP.eIsInSubnet(subnetIP.Address_Parsed!));
					return iip != null;
				})
				.ToArray();


			MikrotikDotNet.Model.IP.DHCPServer.LeaseItem[] dhcpWaiting = [..
				dhcpRowsAll
					.Where(r => !r.Dynamic
					&& r.Status == MikrotikDotNet.Model.IP.DHCPServer.LeaseItem.STATUSES.Waiting)];

			var arpByMAC = (from arp in arpRows
							group arp by arp.MacAddress!.ToString() into arpMacGroup
							orderby arpMacGroup.Key
							select arpMacGroup)
						   .ToArray();


			arpByMAC.eForEach(arpGrp =>
			{
				var arpRecordsForSomeMAC = arpGrp.ToArray();

				if (arpRecordsForSomeMAC.Length == 1)
				{
					var arp = arpRecordsForSomeMAC.First();

					var foundInDHCP = dhcpWaiting
						.FirstOrDefault(dhcp => dhcp.Address!.Equals(arp.Address));

					l.Add(new LocalLANHost(arp, foundInDHCP));
				}
				else
				{
					// Multi ARP records for some MAC
					// May be some devices is behind network router/commutetor with this mac

					//Removing IP which is in the DHCP...

					//var larpByIP = arpGrp.ToDictionary(r=>r.Address!);

					var arpRecordsForSomeMAC_NotInDHCP = arpGrp
						.Where(arp =>
						{
							var foundInDHCP = dhcpRowsAll
								.FirstOrDefault(dhcp =>
									!dhcp.Dynamic
									&& dhcp.Status == MikrotikDotNet.Model.IP.DHCPServer.LeaseItem.STATUSES.Bound
									&& dhcp.Address!.Equals(arp.Address));

							return foundInDHCP == null;
						}
						)
						.OrderBy(arp => arp.Address!.eToUInt32CalculableOrder())
						.ToArray();


					if (arpRecordsForSomeMAC_NotInDHCP.Any())
					{
						l.Add(new LocalLANHost(arpRecordsForSomeMAC_NotInDHCP, dhcpRowsAll));
					}

				}

				//l.Add(new LocalLANHost(arp, dhcpRowsAll));
			});

			var query = l
				//.OrderBy(r => r.IP.eToUInt32CalculableOrder())
				.ToArray();

			var rows = query
				.Select(r => AddRecordToList(r, false))
				.ToArray();


			//lvwRows.erunOnLockedUpdate(delegate
			//{
			lvwRows.RestoreAllGroupsCollapsedStateFromStorage(dataID: "ARP_List");
			lvwRows.eAddItems(rows, true);
			//});
			//OnDeviceSelected();

		}





		private uom.controls.ListViewItemT<LocalLANHost> AddRecordToList(LocalLANHost r, bool add)
		{
			var li = new uom.controls.ListViewItemT<LocalLANHost>(r);
			li.eAddFakeSubitems(lvwRows);
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
			if (abr.HostName.IsNotNullOrWhiteSpace()) name = abr.HostName;
			if (abr.Comment.IsNotNullOrWhiteSpace())
			{
				if (name.IsNotNullOrWhiteSpace())
					name += $" ({abr.Comment})";
				else
					name = abr.Comment;
			}

			li.eUpdateTexts(
				  abr.IP.ToString(),
				  name,
				  abr.MAC.ToString(),
				  abr.LastSeen
				  );

			li.Group = lvwRows.eGroupsCreateGroupByKey(
				abr.Source.ToString(),
				newGroupState: ListViewGroupCollapsedState.Expanded)
			.Group;
			 */

			//lvwRows.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}





	}
}
