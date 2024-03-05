#nullable enable

namespace MALM.UI;

using MALM.Model;


internal partial class DevicesListUI : Form
{

	private const string RECENT_FILE = "Recent.dat";


	public static MKConnection? SelectDeviceAndConnect(LoginResult l)
	{
		using DevicesListUI f = new(l);
		if (f.ShowDialog() != DialogResult.OK) return null; // User canceled device selection
		return f._dialogResult!;
	}


	private void OnLoad()
	{
		lvwDevices.GroupCollapsedStateChanged += (_, _)
			=> lvwDevices.SaveAllGroupsCollapsedStates(dataID: "DevicesList");

		lvwDevices.e_ClearItems();

		string? recent = Extensions_DebugAndErrors.e_tryCatch(LoadRecent, null).Result;

		foreach (var abr in _devices)
		{
			var li = AddRecordToList(abr);
			if (recent != null && li.Text.ToLower().Trim() == recent.ToLower().Trim())
			{
				li.e_Activate();
			}
		}

		lvwDevices.RestoreAllGroupsCollapsedStateFromStorage(dataID: "DevicesList");

		OnDeviceSelected();
	}


	#region Recent Load / Save


	private static FileInfo GetRecentStorage() => uom.AppTools.GetFileIn_AppData(RECENT_FILE, true).e_ToFileInfo()!;
	internal static string? LoadRecent() => GetRecentStorage().e_ReadAsText();

	private static void SaveRecent(string host) => GetRecentStorage().e_WriteAllText(host);


	#endregion


	#region Internal Helpers


	private ListViewItem AddRecordToList(DevicesListRecord abr)
	{
		var li = new ListViewItem(abr.AddressString);
		li.e_AddFakeSubitems(lvwDevices);
		li.Tag = abr;
		lvwDevices.Items.Add(li);
		UpdateRecordInList(li);
		return li;
	}


	private void UpdateRecordInList(ListViewItem li)
	{
		DevicesListRecord abr = (DevicesListRecord)li.Tag!;
		string title = abr.AddressString;
		if (abr.PortInt.HasValue) title += $":{abr.PortInt.Value}";

		li.e_UpdateTexts(title, abr.UserName);

		li.Group = lvwDevices.e_GroupsCreateGroupByKey(
			string.IsNullOrWhiteSpace(abr.Group)
					? L_DEFAULT
					: abr.Group,

			newGroupState: ListViewGroupCollapsedState.Expanded)
		.Group;

		lvwDevices.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

	}



	#endregion


	#region Connect


	private async Task OnTryConnectDevice(object? s, EventArgs e)
	{
		var sel = lvwDevices.e_SelectedItemsAndTags<DevicesListRecord>().FirstOrDefault();
		var md = sel?.Tag;
		if (md == null || string.IsNullOrWhiteSpace(md.AddressString)) return;

		Control[] buttonsToDisable = [btnAdd, btnEdit, btnDelete, btnConnect, btnSetMasterKey];
		UseWaitCursor = true;
		Update();

		buttonsToDisable.e_Enable(false);

		try
		{
			var con = await DevicesListRecord.OpenConnection(md);

			//Connected successfully
			_dialogResult = con!;
			DialogResult = DialogResult.OK;
		}
		catch (Exception ex) { ex.e_LogError(true, E_TITLE_DEFAULT); }
		finally
		{
			buttonsToDisable.e_Enable(true);
			UseWaitCursor = false;
			OnDeviceSelected();
		}

	}


	#endregion




}
