namespace MALM.UI.AddressListSuggestions;


internal class FromDHCPLease(MikrotikDotNet.Model.IP.DHCPServer.LeaseItem mkRow)
	: SuggestionItemBase(mkRow, mkRow!.Address!.ToString(), mkRow!.Comment ?? string.Empty, "DHCP")
{

	public MikrotikDotNet.Model.IP.DHCPServer.LeaseItem MikrotikRow2 => (MikrotikRow as MikrotikDotNet.Model.IP.DHCPServer.LeaseItem)!;


	/// <summary>Gets suggestion list from DHCP leases</summary>
	public static async Task<FromDHCPLease[]> GetRowsAsync(MKConnection c)
	{
		var dhcpLeaseList = (await MikrotikDotNet.Model.IP.DHCPServer.LeaseItem.GetItemsAsync(c, false))
			.OrderBy(mr => mr.Address, uom.Network.IP4AddressComparer.StaticInstance.Value!)
			.Select(mr => new FromDHCPLease(mr))
			.ToArray();

		return dhcpLeaseList;
	}
}
