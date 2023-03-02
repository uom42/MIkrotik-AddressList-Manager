using System.Xml.Serialization;

using CommunityToolkit.Mvvm.ComponentModel;

using static MALM.Localization.Strings;

namespace MALM.Model
{

	//Android Specific Code
	partial class DevicesListRecord : ObservableObject, INotifyPropertyChanged
	{


		[XmlIgnore] public string Title => $"{AddressString} ({UserName})";


		[XmlIgnore]
		public string PortString2 => PortInt.HasValue
			? $":{PortInt.Value}"
			: string.Empty;



		/*



		[XmlIgnore]
		public string PortString2 => $"{L_DEVICE_PORT}: {(PortInt.HasValue
			? PortInt.Value
			: L_DEFAULT)}";



		//public event PropertyChangedEventHandler? PropertyChanged2 = delegate { };
		public void OnPropertyChanged2([CallerMemberName] string propName = "")
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		 */

		public void OnPropertyChanged2([CallerMemberName] string propName = "")
			=> OnPropertyChanged(new PropertyChangedEventArgs(propName));


	}
}
