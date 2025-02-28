global using static uom.constants;

using System.Net.NetworkInformation;

namespace MALM.OUI
{

	internal class MacRecordsGroup
	{
		public readonly string MFGCode;
		public readonly MacRecordInfo[] InfoList;

		public MacRecordsGroup(string mFGCode, MacRecordInfo[] infoList)
		{
			MFGCode = mFGCode ?? throw new ArgumentNullException(nameof(mFGCode));
			InfoList = infoList ?? throw new ArgumentNullException(nameof(infoList));

			InfoList.eForEach(il => il.Parent = this);
		}

		public string? MfgCountryID => InfoList.FirstOrDefault(r => r.Country2LetterUCode.IsNotNullOrWhiteSpace())?.Country2LetterUCode;

	}
}

