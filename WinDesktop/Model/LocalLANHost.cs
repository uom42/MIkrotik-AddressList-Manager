using static MALM.Localization.LStrings;
using MALM.UI;

using System.Net.NetworkInformation;

namespace MALM.Model
{

	internal class LocalLANHost //: Mikrotik.API.Model.IP.ARP.ARPListItem
	{
		public enum SOURCES : int
		{
			DHCP,
			ARP
		}

		private static Lazy<OUI.OUIDatabase?> _ouiDb = new(() => OUI.Manager.Database);

		private string _mfgString = string.Empty;

		public readonly IPAddress IP;
		public readonly PhysicalAddress MAC;

		public readonly string HostName = string.Empty;
		public readonly string Comment = string.Empty;

		public readonly SOURCES Source;

		public readonly TimeSpan? LastSeen;


		private LocalLANHost(IPAddress ip, PhysicalAddress mac, SOURCES s)
		{
			IP = ip;
			MAC = mac;

			UpdateInternal();
		}


		internal LocalLANHost(MikrotikDotNet.Model.IP.DHCPServer.LeaseItem dhcp) : this(dhcp.ActiveAddress!, dhcp.ActiveMacAddress!, SOURCES.DHCP)
		{
			LastSeen = dhcp.LastSeen;

			HostName = (dhcp?.HostName ?? string.Empty).e_IsNotNullOrWhiteSpace()
				? dhcp!.HostName!
				: "";

			Comment = dhcp?.Comment ?? string.Empty;
		}


		internal LocalLANHost(MikrotikDotNet.Model.IP.ARP.ARPItem arp) : this(arp.Address!, arp.MacAddress!, SOURCES.ARP)
		{
			HostName = string.Empty;
			Comment = arp?.Comment ?? string.Empty;
		}


		private void UpdateInternal()
		{
			var db = _ouiDb.Value;
			if (db != null && db.GetMacRecordString(MAC, out var mfgString, out var mrg))
			{
				_mfgString = mfgString!;
			}
		}




		public void Update(ListViewItem li, ListView lvw)
		{
			string name = string.Empty;
			if (HostName.e_IsNotNullOrWhiteSpace()) name = HostName;
			if (Comment.e_IsNotNullOrWhiteSpace())
			{
				if (name.e_IsNotNullOrWhiteSpace())
					name += $" ({Comment})";
				else
					name = Comment;
			}

			string lastSeen = LastSeen.HasValue
				? LastSeen.Value.ToString()
				: string.Empty;

#if WINDOWS
			if (LastSeen.HasValue && Math.Truncate(LastSeen.Value.TotalMilliseconds) < UInt32.MaxValue)
			{
				var ms = (uint)Math.Truncate(LastSeen!.Value.TotalMilliseconds);
				lastSeen = ms.e_ToShellTimeString();
			}
#endif

			li.e_UpdateTexts(
				  IP.ToString(),
						  MAC!.e_ToStringHex(),
						  name,
						  lastSeen,
						  _mfgString,
						  Source.ToString()
						  );

#if WINDOWS
			li.Group = lvw.e_GroupsCreateGroupByKey(
			Source.ToString(),
			newGroupState: ListViewGroupCollapsedState.Expanded)
			.Group;

			/*
			 */
#endif

		}
	}
}
