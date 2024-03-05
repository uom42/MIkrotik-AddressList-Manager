using MikrotikDotNet.Model;

namespace MALM.UI.AddressListSuggestions;


internal abstract class SuggestionItemBase(MKDataRowBase mkRow, string a, string c, string g)
{

	/*

	public enum SOURCES : int
	{
		FAL,
		DHCP,

	}
	 */

	public readonly MKDataRowBase MikrotikRow = mkRow;

	//public readonly SOURCES Source = s;

	public readonly string Address = a;

	public readonly string Comment = c;

	public readonly string GrpupName = g;



}
