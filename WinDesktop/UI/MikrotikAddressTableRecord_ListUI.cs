#nullable enable

using MALM.UI.AddressListSuggestions;

using MikrotikDotNet.Model.IP.Firewall.AddressList;


namespace MALM.UI;


public partial class MikrotikAddressTableRecord_ListUI : Form
{


    private void UpdateUIState ()
    {
        var sel = lvwRows
            .selectedItemsAs<AddressListItemRow>()
            .ToArray();

        //var enabledRows = sel.Where(li => li.MikrotikRow.Enabled);
        var grp = sel
            .GroupBy(
            li => li.MikrotikRow.Enabled ,
            li => li.MikrotikRow ,
            ( enabled , rows ) => new { Enabled = enabled , Rows = rows });

        var enabledRows = grp
            .FirstOrDefault(g => g.Enabled == true)?
            .Rows?
            .ToArray()?
            .Length ?? 0;

        var disabledRows = grp
            .FirstOrDefault(g => g.Enabled == false)?
            .Rows?
            .ToArray()?
            .Length ?? 0;

        btnRows_Enable.Enabled = sel.Any();
        if ( sel.Any() )
        {
            //Debug.WriteLine($"Enabled: {enabledRows} Disabled: {disabledRows}");
            CheckState cs = CheckState.Indeterminate;
            if ( sel.Length == enabledRows )
            {
                cs = CheckState.Checked;
            }
            else if ( sel.Length == disabledRows )
            {
                cs = CheckState.Unchecked;
            }

            _dontChangerCheckedState = true;
            btnRows_Enable.CheckState = cs;
            _dontChangerCheckedState = false;
        }

        btnRows_Refresh.Enabled = true;
        btnRows_Add.Enabled = true;


        btnViewARPList.Enabled = true;

        txtFilter.Enabled = true;
    }


    /// <summary>Return ID - used for save/restore ListView groups collapsed states</summary>
    private string GetHostDataID ()
    {
        var abHash = _connection.Host.ToLower().eComputeHashUni(Extensions_Security_Hash.HashNames.SHA1);
        return Convert.ToHexString(abHash).ToLower();
    }

    private bool _dontChangerCheckedState = false;

    private async Task SelectedRows_EnableDisable ( bool enable )
    {
        if ( _dontChangerCheckedState ) return;

        UseWaitCursor = true;


        try
        {
            var sel = lvwRows.selectedItemsAs<AddressListItemRow>();
            if ( sel == null || !sel.Any() ) return;

            async Task updateRows ()
            {

                using var h = _connection!.Open();

                foreach ( AddressListItemRow li in sel )
                {
                    var mkRow = li.MikrotikRow;
                    mkRow = await mkRow.ReQueryAsync(true);//Get current row info from kikrotik to avoid modifications of rows with wrong .id
                                                           //if item with this id will be not found - error will bw thrown and no modification on item with wrong id will be made.

                    await mkRow.EnableAsync(true , enable);//item with this id was found. Make modification
                    mkRow = await mkRow.ReQueryAsync(true);//Again requery item to get new properties
                    li.UpdateFromModel(mkRow , lvwRows);
                }
            }

            await lvwRows.runOnLockedUpdateAsync(updateRows , true , true);

        }
        catch ( Exception ex )
        {
            ex.eLogError(true , Localization.LStrings.E_TITLE_DEFAULT);

            await QueryDataFromDevice();//On any error we refill list with actual data from mikrotik
        }
        finally
        {
            UseWaitCursor = false;
        }
    }



    #region UI Controls Events



    private async Task OnRows_Add ()
    {


        string selectedRowGroupName = lvwRows.selectedItemsAs<AddressListItemRow>()
            .FirstOrDefault()?
            .MikrotikRow?
            .List
                ?? string.Empty;

        if ( selectedRowGroupName.isNullOrWhiteSpace )
        {
            //No items is selected in list. Search opened groups
            var gg = lvwRows
                .groupsAsIEnumerable()
                .FirstOrDefault(g => g.CollapsedState == ListViewGroupCollapsedState.Expanded && g.Items.Count > 0);

            selectedRowGroupName = gg?.Name ?? string.Empty;
        }

        List<SuggestionItemBase> addressSuggestionList = [];

        #region Collecting addreses from other groups

        {
            //Collecting all addreses from other groups
            var fog = _mikrotikRows
                .Select(li => li.MikrotikRow)
                .Where(mr => mr.List != selectedRowGroupName)
                .OrderBy(mr => mr.List).ThenBy(mr => mr.Address)
                .Select(mr => new FromFirewallAddressList(mr))
                .ToArray();

            addressSuggestionList.AddRange(fog);
        }

        #endregion

        #region Append suggestion list from DHCP leases

        {
            //Append suggestion list from DHCP leases
            var dhcpLeaseList = await FromDHCPLease.GetRowsAsync(_connection);
            addressSuggestionList.AddRange(dhcpLeaseList);
        }

        #endregion

        /*
		var groupNames = _mikrotikRows
			.Select(li => li.MikrotikRow.List.Trim())
			.Distinct()
			.OrderBy(s => s)
			.ToArray();
		 */

        try
        {
            var gg = lvwRows
                .groupsAsIEnumerable()
                .ToArray()
                .ToDictionary(grp => grp.Name! , grp => grp.CollapsedState);

            using MikrotikAddressTableRecord_AddItemUI fe = new(_connection , [ .. addressSuggestionList ] , gg , selectedRowGroupName);
            if ( fe.ShowDialog(this) != DialogResult.OK ) return;

            AddressListItem addedMKRow = fe._dialogResult!;
            AddressListItemRow li = new(addedMKRow , lvwRows);
            var lmkRows = _mikrotikRows.ToList();
            lmkRows.Add(li);
            _mikrotikRows = [ .. lmkRows ];

            DisplayFilteredMKData();
            li.activate();
        }
        catch ( Exception ex )
        {
            ex.eLogError(true , Localization.LStrings.E_TITLE_DEFAULT);
            await QueryDataFromDevice();//On any error we refill list with actual data from mikrotik
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    /*
	private (DialogResult Result, T UI) STAShowDialog<T>(Func<T> dialogCreator, Form parent) where T : Form
	{
		//DialogState state = new DialogState();
		//state.dialog = dialog;

		DialogResult dr = DialogResult.None;
		T? ui = null;
		System.Threading.Thread t = new(() =>
		{
			ui = dialogCreator.Invoke();
			dr = ui.ShowDialog(parent);

		});

		t.SetApartmentState(System.Threading.ApartmentState.STA);
		t.Start();
		t.Join();
		return (dr, ui!);
	}
	 */




    #endregion

    private void OnFilterChanged ( string filter )
    {
        if ( !_tableRowsReady ) return;

        DisplayFilteredMKData();
    }



    private void ShowLANDevices ()
    {
        using LocalSubnetBrowserUI f = new(_connection);
        f.ShowDialog(this);
    }
}