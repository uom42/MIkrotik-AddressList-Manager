using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MALM.UI.AddressListSuggestions
{





	internal class FromDHCPLeaseItem : MikrotikObjectBase
	{
		public global::Mikrotik.API.Model.IP.DHCPServer.LeaseListItem MikrotikRow2 => (MikrotikRow as global::Mikrotik.API.Model.IP.DHCPServer.LeaseListItem)!;

		public FromDHCPLeaseItem(global::Mikrotik.API.Model.IP.DHCPServer.LeaseListItem mkRow) : base(mkRow) { }

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


}
