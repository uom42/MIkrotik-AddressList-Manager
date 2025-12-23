#nullable enable

using MikrotikDotNet.Model.IP.Firewall.AddressList;
using System.Drawing;
using uom.controls;

namespace MALM.UI
{
    internal class AddressListItemRow : ListViewItem
    {

        public const string C_IMAGE_KEY_GREEN = "GREEN";
        public const string C_IMAGE_KEY_GRAY = "GRAY";

        public AddressListItem MikrotikRow { get; private set; }

        public AddressListItemRow ( AddressListItem ali , ListViewEx lvw ) : base()
        {
            MikrotikRow = ali;
            this.addFakeSubitems(lvw , "@");
            UpdateFromModel(ali , lvw);
            //UpdateFromModel(ali, lvw, onNewGroupAdded);
        }


        public void UpdateGroupFromModel ( ListViewEx lvw )
            => Group = lvw!.groupsCreateGroupByKey(MikrotikRow.List).Group;

        public void UpdateFromModel ( AddressListItem ali , ListViewEx lvw )
        {
            MikrotikRow = ali;
            _ = lvw ?? throw new ArgumentNullException(nameof(lvw));

            this.updateTexts(
                ali.Address ,
                ali.Comment ?? "" ,
                ali.CreationTime!.eToLongDateTimeString() ,
                ali.MKID);

            ImageKey = ali.Disabled ? C_IMAGE_KEY_GRAY : C_IMAGE_KEY_GREEN;
            ForeColor = ali.Disabled ? SystemColors.GrayText : SystemColors.WindowText;

            UpdateGroupFromModel(lvw);
        }

        public bool Filter ( string filter )
        {
            if ( string.IsNullOrWhiteSpace(filter) ) return true;

            filter = filter.Trim().ToLower();

            if ( MikrotikRow.Address.ToLower().Contains(filter) ) return true;
            if ( MikrotikRow.List.ToLower().Contains(filter) ) return true;
            return MikrotikRow.CreationTime.ToString().Contains(filter) || MikrotikRow.Comment != null && MikrotikRow.Comment.ToLower().Contains(filter);
        }
    }
}
