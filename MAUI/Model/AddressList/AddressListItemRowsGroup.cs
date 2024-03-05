using static MALM.Localization.LStrings;

using MvvmHelpers;

using uom.controls.MAUI.CollectionViews;

namespace MALM.Model.AddressList
{

	internal class AddressListItemRowsGroup(CollectionView cv, string name, IEnumerable<AddressListItemRow> rows)
		: ExpandableGroupsCollection<AddressListItemRow>(cv, name, rows, L_EMPTY_GROUP_NAME, true)
	{ }

}
