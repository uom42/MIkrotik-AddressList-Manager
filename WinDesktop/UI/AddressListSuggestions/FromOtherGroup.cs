using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MALM.UI.AddressListSuggestions
{


	internal class FromOtherGroup : MikrotikObjectBase
	{
		public global::Mikrotik.API.Model.IP.Firewall.AddressList.AddressListItem MikrotikRow2 => (MikrotikRow as global::Mikrotik.API.Model.IP.Firewall.AddressList.AddressListItem)!;

		public FromOtherGroup(global::Mikrotik.API.Model.IP.Firewall.AddressList.AddressListItem mkRow) : base(mkRow) { }

		public override string GetAddress() => MikrotikRow2.Address;

		public override string ToString()
		{
			string s = GetAddress();
			if (!string.IsNullOrWhiteSpace(MikrotikRow.Comment)) s += $" | {MikrotikRow.Comment}";
			s += $" | from list: {MikrotikRow2.List}";

			return s;
		}


	}


}
