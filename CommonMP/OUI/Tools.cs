using System.Net.NetworkInformation;

namespace MALM.OUI
{

	/// <summary>Позволяет узнать название производителя адаптера по его MAC</summary>
	///<remarks>Файл http://standards.ieee.org/develop/regauth/oui/oui.txt - ежедневно обновляемый список назначеных кодов производителей в MAC-адресах</remarks>
	internal static class Tools
	{

		internal static string FormatMfgOctets(this byte[] mac)
		{
			if (mac.Length < 3)
			{
				throw new ArgumentOutOfRangeException(nameof(mac));
			}


			mac[0].eSetBitRef(0, false);
			mac[0].eSetBitRef(1, false);

			return $"{mac[0]:X2}-{mac[1]:X2}-{mac[2]:X2}";
			//byte[] macBytes = [.. mac.GetAddressBytes().Take(3)];
		}

		internal static string FormatMfgOctets(this PhysicalAddress mac)
			=> mac.GetAddressBytes().FormatMfgOctets();


	}

}
