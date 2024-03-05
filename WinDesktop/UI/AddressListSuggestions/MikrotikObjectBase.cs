using MikrotikDotNet.Model;

namespace MALM.UI.AddressListSuggestions;

internal abstract class MikrotikObjectBase
{
	public readonly MKDataRowBase MikrotikRow;
	public MikrotikObjectBase(MKDataRowBase mkRow) { MikrotikRow = mkRow; }

	public abstract string GetAddress();
}
