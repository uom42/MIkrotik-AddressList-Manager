#nullable enable

using System.Drawing;

using common.Controls;

using Mikrotik.API.Model.IP.Firewall.AddressList;

namespace malm.Core
{
	internal class AddressListItem_ListViewItem : ListViewItem
	{

		public const string C_IMAGE_KEY_GREEN = "GREEN";
		public const string C_IMAGE_KEY_GRAY = "GRAY";

		public AddressListItem MikrotikRow { get; private set; }

		public AddressListItem_ListViewItem(AddressListItem ali, ListViewEx lvw) : base()
		{
			MikrotikRow = ali;
			this.e_AddFakeSubitems(lvw, "@");
			UpdateFromModel(ali, lvw);
		}


		public void UpdateGroupFromModel(ListViewEx lvw, Action<ListViewGroup>? onNewGroupAdded = null)
		{
			_ = lvw ?? throw new ArgumentNullException(nameof(lvw));
			Group = lvw!.e_GroupsCreateGroupByKey(MikrotikRow.List, onNewGroup: onNewGroupAdded).Group;
		}

		public void UpdateFromModel(AddressListItem ali, ListViewEx lvw, Action<ListViewGroup>? onNewGroupAdded = null)
		{
			MikrotikRow = ali;
			_ = lvw ?? throw new ArgumentNullException(nameof(lvw));

			this.e_UpdateTexts(
				ali.Address,
				ali.Comment ?? "",
				ali.CreationTime_AsDateTime.e_ToLongDateTimeString(),
				ali.MKID);

			ImageKey = ali.Disabled ? C_IMAGE_KEY_GRAY : C_IMAGE_KEY_GREEN;
			ForeColor = ali.Disabled ? SystemColors.GrayText : SystemColors.WindowText;

			UpdateGroupFromModel(lvw, onNewGroupAdded);
		}

		public bool Filter(string filter)
		{
			if (string.IsNullOrWhiteSpace(filter)) return true;

			filter = filter.Trim().ToLower();

			if (MikrotikRow.Address.ToLower().Contains(filter)) return true;
			if (MikrotikRow.List.ToLower().Contains(filter)) return true;
			if (MikrotikRow.CreationTime.ToLower().Contains(filter)) return true;
			if (MikrotikRow.Comment != null && MikrotikRow.Comment.ToLower().Contains(filter)) return true;

			return false;
		}
	}
}
