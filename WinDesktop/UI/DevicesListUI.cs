
using MALM.Model;

#nullable enable

namespace MALM.UI;

internal partial class DevicesListUI : Form
{

    private const string RECENT_FILE = "Recent.dat";


    public static MKConnection? SelectDeviceAndConnect ( LoginResult l )
    {
        using DevicesListUI f = new(l);
        if ( f.ShowDialog() != DialogResult.OK ) return null; // User canceled device selection
        return f._dialogResult!;
    }


    private void OnLoad ()
    {
        lvwDevices.GroupCollapsedStateChanged += ( _ , _ )
            => lvwDevices.SaveGroupsState(dataID: "DevicesList");

        lvwDevices.clearItems();

        var iconSize = System.Windows.Forms.SystemInformation.IconSize;
        var iml = new ImageList()
        {
            ColorDepth = ColorDepth.Depth32Bit ,
            ImageSize = iconSize ,
        };

        var bmRouter = uom.AppInfo.AppAssembly.eLoadSVGFromResourceFileAsBitmap("Router_gray.svg" , iconSize);

        iml.Images.Add(bmRouter);
        lvwDevices.SmallImageList = iml;

        //var sss = ((Func<string?>)LoadRecent).tryCatch(out var recent);
        var recent = ((Func<string?>)LoadRecent).tryCatch().Value.valueOrEmpty;
        foreach ( var abr in _devices )
        {
            var li = AddRecordToList(abr);
            if ( li.Text.Equals(recent , StringComparison.OrdinalIgnoreCase) ) li.activate();
        }

        lvwDevices.RestoreGroupsStateFromStorage(dataID: "DevicesList");

        OnDeviceSelected();
    }


    #region Recent Load / Save


    private static FileInfo GetRecentStorage () => uom.AppTools.GetFileIn_AppData(RECENT_FILE , true).eToFileInfo()!;
    internal static string? LoadRecent () => GetRecentStorage().eReadAsText();

    private static void SaveRecent ( string host ) => GetRecentStorage().eWriteAllText(host);


    #endregion


    #region Internal Helpers


    private uom.controls.ListViewItemT<DevicesListRecord> AddRecordToList ( DevicesListRecord abr )
    {
        var li = new uom.controls.ListViewItemT<DevicesListRecord>(abr);
        li.addFakeSubitems(lvwDevices);
        lvwDevices.Items.Add(li);
        UpdateRecordInList(li);
        return li;
    }


    private void UpdateRecordInList ( uom.controls.ListViewItemT<DevicesListRecord> li )
    {
        DevicesListRecord abr = li.Value2;
        string title = abr.AddressString;
        if ( abr.PortInt.HasValue ) title += $":{abr.PortInt.Value}";

        li.updateTexts(title , abr.UserName);
        li.ImageIndex = 0;

        li.Group = lvwDevices.groupsCreateGroupByKey(
            string.IsNullOrWhiteSpace(abr.Group)
                    ? L_DEFAULT
                    : abr.Group ,

            newGroupState: ListViewGroupCollapsedState.Expanded)
        .Group;

        lvwDevices.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

    }



    #endregion


    private uom.controls.ListViewItemT<DevicesListRecord>? SelectedDevice
        => lvwDevices.selectedItemsAs<uom.controls.ListViewItemT<DevicesListRecord>>().FirstOrDefault();


    private void OnDeviceSelected ()
    {
        var sel = SelectedDevice;
        var hasSel = sel != null;

        btnConnect.Enabled = hasSel;
        btnDelete.Enabled = hasSel;
        btnEdit.Enabled = hasSel;

        try { SaveRecent(sel?.Value?.AddressString ?? ""); } catch { }
    }



    #region Editing

    private async void Device_Edit ( object s , EventArgs e )
    {
        var sel = SelectedDevice;
        if ( sel == null ) return;
        DevicesListRecord dev = sel.Value2;

        using DevicesListRecordEditorUI fe = DevicesListRecordEditorUI.InitUI(dev);
        if ( fe.ShowDialog(this) != DialogResult.OK ) return;

        dev = DevicesListRecord.FromEditor(fe);
        sel.Value2 = dev;
        UpdateRecordInList(sel);
        await SaveDevicesList();
    }


    private async void Device_Delete ( object s , EventArgs e )
    {
        var sel = SelectedDevice;
        if ( sel == null ) return;
        DevicesListRecord dev = sel.Value2;

        string q = string.Format(Q_ADDRESSBOOK_DELETE_RECORD , dev.AddressString);

        if ( !q.eMsgboxAskIsYes(false , L_DEVICES_LIST_EDITOR) ) return;
        lvwDevices.Items.Remove(sel);
        await SaveDevicesList();
        OnDeviceSelected();
    }



    #endregion


    #region Connect


    private async Task OnTryConnectDevice ( object? s , EventArgs e )
    {
        var sel = SelectedDevice;
        if ( sel == null ) return;
        DevicesListRecord dev = sel.Value2;

        if ( string.IsNullOrWhiteSpace(dev.AddressString) ) return;

        Control[] buttonsToDisable = [ btnAdd , btnEdit , btnDelete , btnConnect , btnSetMasterKey ];
        UseWaitCursor = true;
        Update();

        buttonsToDisable.enable(false);

        try
        {
            var con = await MikrotikDotNet.Model.Helpers.ConnectDeviceAsync(dev.CreateConnection());

            //Connected successfully
            _dialogResult = con!;
            DialogResult = DialogResult.OK;
        }
        catch ( Exception ex ) { ex.eLogError(true , E_TITLE_DEFAULT); }
        finally
        {
            buttonsToDisable.enable(true);
            UseWaitCursor = false;
            OnDeviceSelected();
        }

    }


    #endregion




}
