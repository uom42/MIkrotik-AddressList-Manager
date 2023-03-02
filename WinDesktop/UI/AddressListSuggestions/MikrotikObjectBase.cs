using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MALM.UI.AddressListSuggestions
{
	internal abstract class MikrotikObjectBase
	{
		public readonly global::Mikrotik.API.Model.ItemBase MikrotikRow;
		public MikrotikObjectBase(global::Mikrotik.API.Model.ItemBase mkRow) { MikrotikRow = mkRow; }

		public abstract string GetAddress();
	}


}
