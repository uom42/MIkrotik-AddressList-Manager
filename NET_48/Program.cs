#nullable enable


///////////////////////////////////////////////////////////
// This programm uses Icons from 'https://www.flaticon.com/free-icons/google-plus'
// <a href="https://www.flaticon.com/free-icons/google-plus" title="google plus icons">Google plus icons created by Smashicons - Flaticon</a>
///////////////////////////////////////////////////////////


using malm.AddressBook;

using MikrotikDotNet;


namespace malm
{
	internal static class Program
	{

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			do
			{
				try
				{
					MKConnection? conTmp;
					try
					{
						conTmp = AddressbookRecord.Login();
					}
					catch (Exception ex) when (ex is System.Security.Cryptography.CryptographicException || ex is System.FormatException)
					{
						throw new Exception("Wrong Master Key or Addressbook file corrypted!", ex);
					}

					if (conTmp == null) break;

					using MKConnection? con = conTmp;
					if (!con.IsOpen) throw new Exception("Failed to connect to mikrotik!");

					using Core.frmMain fm = new(con);
					Application.Run(fm);
					break;

				}
				catch (Exception ex)
				{
					ex.FIX_ERROR(true);
				}

			}
			while (true);

		}


	}
}