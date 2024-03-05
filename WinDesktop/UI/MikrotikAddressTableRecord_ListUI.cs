#nullable enable

using MALM.UI.AddressListSuggestions;

using MikrotikDotNet.Model.IP.Firewall.AddressList;


namespace MALM.UI;


public partial class MikrotikAddressTableRecord_ListUI : Form
{


	private void UpdateUIState()
	{
		var sel = lvwRows.e_SelectedItemsAs<AddressListItemRow>();
		btnRows_Enable.Enabled = sel.Any();
		btnRows_Disable.Enabled = sel.Any();

		btnRows_Refresh.Enabled = true;
		btnRows_Add.Enabled = true;


		btnViewARPList.Enabled = true;

		txtFilter.Enabled = true;
	}


	/// <summary>Return ID - used for save/restore ListView groups collapsed states</summary>
	private string GetHostDataID()
	{
		var abHash = _connection.Host.ToLower().e_ComputeHashUni(Extensions_Security_Hash.HashNames.SHA1);
		return Convert.ToHexString(abHash).ToLower();
	}




	private async Task SelectedRows_EnableDisable(bool enable)
	{
		this.UseWaitCursor = true;

		try
		{
			var sel = lvwRows.e_SelectedItemsAs<AddressListItemRow>();
			if (sel == null || !sel.Any()) return;

			async Task updateRows()
			{
				foreach (AddressListItemRow li in sel)
				{
					var mkRow = li.MikrotikRow;
					mkRow = await mkRow.ReQueryAsync();//Get current row info from kikrotik to avoid modifications of rows with wrong .id
													   //if item with this id will be not found - error will bw thrown and no modification on item with wrong id will be made.

					await mkRow.EnableAsync(enable);//item with this id was found. Make modification
					mkRow = await mkRow.ReQueryAsync();//Again requery item to get new properties
					li.UpdateFromModel(mkRow, lvwRows);
				}
			}

			await lvwRows.e_runOnLockedUpdateAsync(updateRows, true, true);

		}
		catch (Exception ex)
		{
			ex.e_LogError(true, Localization.LStrings.E_TITLE_DEFAULT);

			await QueryDataFromDevice();//On any error we refill list with actual data from mikrotik
		}
		finally
		{
			this.UseWaitCursor = false;
		}
	}



	#region UI Controls Events



	private async Task OnRows_Add()
	{
		var sel = lvwRows.e_SelectedItemsAs<AddressListItemRow>().FirstOrDefault();
		string groupName = sel?.MikrotikRow?.List ?? string.Empty;

		List<MikrotikObjectBase> addressSuggestionList = [];

		#region Collecting all addreses from other groups

		{
			//Collecting all addreses from other groups
			var fog = _mikrotikRows
				.Select(li => li.MikrotikRow)
				.Where(mr => mr.List != groupName)
				.OrderBy(mr => mr.Address).ThenBy(mr => mr.List)
				.Select(mr => new FromOtherGroup(mr))
				.ToArray();

			addressSuggestionList.AddRange(fog);
		}

		#endregion

		#region Append suggestion list from DHCP leases

		{
			//Append suggestion list from DHCP leases
			var dhcpLeaseList = (await MikrotikDotNet.Model.IP.DHCPServer.LeaseItem.GetItemsAsync(_connection))
				.OrderBy(mr => mr.Address)
				.Select(mr => new FromDHCPLeaseItem(mr))
				.ToArray();
			addressSuggestionList.AddRange(dhcpLeaseList);
		}

		#endregion

		var groupNames = _mikrotikRows
			.Select(li => li.MikrotikRow.List.Trim())
			.Distinct()
			.OrderBy(s => s)
			.ToArray();

		try
		{

			using MikrotikAddressTableRecord_EditorUI fe = new(_connection, [.. addressSuggestionList], groupNames, groupName);
			if (fe.ShowDialog(this) != DialogResult.OK) return;
			/*

			var r = STAShowDialog(()
				=> new MikrotikDevicesListRecordEditorUI(_connection, [.. addressSuggestionList], groupNames, groupName)
				, this
				);

			var fe = r.UI;
			 */
			AddressListItem addedMKRow = fe._dialogResult!;
			AddressListItemRow li = new(addedMKRow, lvwRows);
			var lmkRows = _mikrotikRows.ToList();
			lmkRows.Add(li);
			_mikrotikRows = [.. lmkRows];

			DisplayFilteredMKData();
			li.e_Activate();
		}
		catch (Exception ex)
		{
			ex.e_LogError(true, Localization.LStrings.E_TITLE_DEFAULT);
			await QueryDataFromDevice();//On any error we refill list with actual data from mikrotik
		}
		finally
		{
			this.UseWaitCursor = false;
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

	private void OnFilterChanged(string filter)
	{
		if (!_tableRowsReady) return;

		DisplayFilteredMKData();
	}



	private void ViewARPList()
	{
		using LocalSubnetBrowserUI f = new(_connection);
		f.ShowDialog(this);
	}
}