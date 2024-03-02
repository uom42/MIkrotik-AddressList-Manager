using static MALM.Localization.Strings;

using MvvmHelpers;

using uom.controls.MAUI.CollectionViews;

namespace MALM.Model.Mikrotik
{

	internal class AddressListItemRowsGroup(CollectionView cv, string name, IEnumerable<AddressListItemRow> rows)
		: ExpandableGroupsCollection<AddressListItemRow>(cv, name, rows, L_EMPTY_GROUP_NAME, true)
	{ }

}
