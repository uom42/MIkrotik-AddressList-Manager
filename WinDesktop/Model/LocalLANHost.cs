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

		private static Lazy<OUI.OUIDatabase?> _ouiDb = new (() => OUI.Manager.Database);

		private string _mfgString = string.Empty;

		public readonly PhysicalAddress MAC;
		public readonly IPAddress[] IPList;

		public readonly string HostName = string.Empty;
		public readonly string Comment = string.Empty;

		public readonly SOURCES Source;

		public readonly TimeSpan? LastSeen;

		private Color? _clrError = null;

		private static readonly Color _COLOR_ERROR_DYNAMIC_WITOUT_DHCP = Color.MistyRose;
		private static readonly Color _COLOR_ERROR_MULTI_IP = Color.Tomato;


		private LocalLANHost ( PhysicalAddress mac, IPAddress[] ip, SOURCES s )
		{
			IPList = ip;
			MAC = mac;
			Source = s;

			UpdateInternal ();
		}


		internal LocalLANHost ( MikrotikDotNet.Model.IP.DHCPServer.LeaseItem dhcp ) : this (dhcp.ActiveMacAddress!, dhcp.ActiveAddress!.eToArrayOf (), SOURCES.DHCP)
		{
			LastSeen = dhcp.LastSeen;

			HostName = ( dhcp?.HostName ?? string.Empty ).IsNotNullOrWhiteSpace ()
				? dhcp!.HostName!
				: "";

			Comment = dhcp?.Comment ?? string.Empty;
		}



		internal LocalLANHost (
			MikrotikDotNet.Model.IP.ARP.ARPItem arp,
			MikrotikDotNet.Model.IP.DHCPServer.LeaseItem? dhcp
			)
			: this (arp.MacAddress!, arp.Address!.eToArrayOf (), SOURCES.ARP)
		{
			HostName = string.Empty;

			string com = arp.Dynamic
				? "Dynamic"
				: "Static";

			com += $" ARP";
			if (arp.Dynamic)
			{
				if (dhcp == null)
				{
					// Dynamic ARP record without corresponding DHCP record!
					_clrError = _COLOR_ERROR_DYNAMIC_WITOUT_DHCP;
				}
			}
			else
			{
				//Static ARP record
				if (arp.Comment.IsNotNullOrWhiteSpace ()) com += $" ({arp!.Comment})";
			}
			com += $".";


			if (dhcp != null)
			{
				com += $" Found DHCP record";
				if (dhcp.Comment.IsNotNullOrWhiteSpace ()) com += $" ({dhcp!.Comment})";
			}
			com += $".";

			Comment = com.Trim ();
		}

		internal LocalLANHost (
			MikrotikDotNet.Model.IP.ARP.ARPItem[] arp,
			MikrotikDotNet.Model.IP.DHCPServer.LeaseItem[] dhcp
			)
			: this (arp.First ().MacAddress!, arp.Select (a => a.Address).ToArray ()!, SOURCES.ARP)
		{
			HostName = string.Empty;

			string com = string.Empty;


			var fa = arp.First ();

			/*
			var foundInDHCP = dhcp
				.Where(d => d.MacAddress!.Equals(fa.MacAddress))
				.FirstOrDefault();

			if (foundInDHCP != null)
			{
				com += $" Has DHCP record";

				if (foundInDHCP.Comment.IsNotNullOrWhiteSpace())
				{
					com += $" with comment: {foundInDHCP!.Comment}";
				}
			}
			 */

			com += $" Multi IP route via MAC!";
			_clrError = _COLOR_ERROR_MULTI_IP;

			Comment = com.Trim ();
		}


		private void UpdateInternal ()
		{
			var db = _ouiDb.Value;
			if (db != null && db.GetMacRecordString (MAC, out var mfgString, out var mrg))
			{
				_mfgString = mfgString!;
			}
		}




		public void Update ( ListViewItem li, ListView lvw )
		{
			string name = string.Empty;
			if (HostName.IsNotNullOrWhiteSpace ()) name = HostName;
			if (Comment.IsNotNullOrWhiteSpace ())
			{
				if (name.IsNotNullOrWhiteSpace ())
					name += $" ({Comment})";
				else
					name = Comment;
			}

			string lastSeen = LastSeen.HasValue
				? LastSeen.Value.ToString ()
				: string.Empty;

#if WINDOWS
			if (LastSeen.HasValue && Math.Truncate (LastSeen.Value.TotalMilliseconds) < UInt32.MaxValue)
			{
				var ms = (uint) Math.Truncate (LastSeen!.Value.TotalMilliseconds);
				lastSeen = ms.StrFromTimeInterval ();
			}
#endif

			string ipString = ( IPList.Length == 1 )
				? IPList.First ().ToString ()
				: $"({IPList.Length}): " + IPList.Select (ip => ip.ToString ()).eJoin (", ")!;


			li.eUpdateTexts (
						  MAC!.eToStringHex (),
						  ipString,
						  name,
						  lastSeen,
						  _mfgString,
						  Source.ToString ()
						  );

#if WINDOWS
			li.Group = lvw.eGroupsCreateGroupByKey (
			Source.ToString (),
			newGroupState: ListViewGroupCollapsedState.Expanded)
			.Group;

			li.BackColor = _clrError ?? SystemColors.Window;
#endif

		}
	}
}
