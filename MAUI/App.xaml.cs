using Android.Runtime;

using MALM.Model;
using MALM.UI;

using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace MALM;

public partial class App : Application
{


	internal string? _DevID;

	public App(uom.maui.IGetDeviceInfo getDeviceInfo)
	{
		InitializeComponent();

		_DevID = getDeviceInfo.GetDeviceID();

		MainPage = new AppShell();
	}
}

