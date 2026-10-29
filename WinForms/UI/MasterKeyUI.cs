#nullable enable

using MALM.Model;

namespace MALM.UI
{
	internal partial class MasterKeyUI : Form
	{


		/// <summary>Display UI for login or Reset masterkey</summary>
		/// <returns><see langword="null"/> when user canceled, or decrypted rows when fogin succesfull</returns>
		internal static async Task<DevicesListRecord[]?> Login(MasterKeyManager mkm)
		{
			MasterKeyUI ui = new(mkm);
			//var isOk = await ui.eShowDialogAsync<bool>(true);

			ui.StartPosition = FormStartPosition.CenterScreen;
			ui.ShowInTaskbar = true;
			var isOk = ui.ShowDialog() == DialogResult.OK;

			if (!isOk) return null; //User canceled

			await Task.Delay(1);
			//Login fully completed
			return ui.LoginResult;
		}


	}

}