using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MALM.Model
{


#if !WINDOWS
	public class DevicesDatabase() : ObservableCollection<DevicesListRecord>()
#else
	public class DevicesDatabase() : Collection<DevicesListRecord>()
#endif
	{

#if WINDOWS
	//private MKConnection? _dialogResult = null;
#endif



	}
}
