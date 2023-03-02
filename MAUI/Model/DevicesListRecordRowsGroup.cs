using static MALM.Localization.Strings;

using MvvmHelpers;
using uom.controls.MAUI.CollectionViews;



namespace MALM.Model.Mikrotik
{

	internal class DevicesListRecordRowsGroup(CollectionView cv, string name, IEnumerable<DevicesListRecord> rows)
		: CollectionViewExExpandableGroupsCollection<DevicesListRecord>(cv, name, rows, L_EMPTY_GROUP_NAME, false)
	{ }

}
