using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MALM.UI.AddressListSuggestions;

internal class FromFirewallAddressList(MikrotikDotNet.Model.IP.Firewall.AddressList.AddressListItem mkRow)
	: SuggestionItemBase(mkRow, mkRow!.Address!.ToString(), mkRow!.Comment ?? string.Empty, mkRow.List)
{


	public MikrotikDotNet.Model.IP.Firewall.AddressList.AddressListItem MikrotikRow2 => (MikrotikRow as MikrotikDotNet.Model.IP.Firewall.AddressList.AddressListItem)!;


}
