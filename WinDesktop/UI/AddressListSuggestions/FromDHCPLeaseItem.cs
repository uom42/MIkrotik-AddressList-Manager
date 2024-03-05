using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MALM.UI.AddressListSuggestions
{





	internal class FromDHCPLeaseItem : MikrotikObjectBase
	{
		public MikrotikDotNet.Model.IP.DHCPServer.LeaseItem MikrotikRow2 => (MikrotikRow as MikrotikDotNet.Model.IP.DHCPServer.LeaseItem)!;

		public FromDHCPLeaseItem(MikrotikDotNet.Model.IP.DHCPServer.LeaseItem mkRow) : base(mkRow) { }

		public override string GetAddress() => MikrotikRow2.Address!.ToString();

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
