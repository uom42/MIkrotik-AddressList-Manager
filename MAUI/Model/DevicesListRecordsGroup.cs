using static MALM.Localization.LStrings;

using MvvmHelpers;
using uom.controls.MAUI.CollectionViews;



namespace MALM.Model.Mikrotik
{

	internal class DevicesListRecordsGroup(CollectionView cv, string name, IEnumerable<DevicesListRecord> rows)
		: ExpandableGroupsCollection<DevicesListRecord>(cv, name, rows, L_EMPTY_GROUP_NAME, false)
	{ }

}
