using Jot;


/*
	 <ItemGroup>
		<PackageReference Include="Jot" Version="2.1.17" />
	</ItemGroup>
 */


namespace Jot
{

	//https://github.com/anakic/Jot
	internal static class JotServices
	{


		private static Tracker? _tracker;

		public static Tracker Tracker => _tracker ??= new();

		static JotServices ()
		{
			// tell Jot how to track Window objects
			Tracker.Configure<Form>()
				.Id(f => f.Name , SystemInformation.VirtualScreen.Size) // <-- include the screen resolution in the id
				.Properties(f => new { f.Top , f.Width , f.Height , f.Left , f.WindowState })
				.PersistOn(nameof(Form.Move) , nameof(Form.Resize) , nameof(Form.FormClosing))
				.WhenPersistingProperty(( f , p ) => p.Cancel = f.WindowState != FormWindowState.Normal && (p.Property == nameof(Form.Height) || p.Property == nameof(Form.Width) || p.Property == nameof(Form.Top) || p.Property == nameof(Form.Left))) // do not track form size and location when minimized/maximized
				.StopTrackingOn(nameof(Form.FormClosing)); // <-- a form should not be persisted after it is closed since properties will be empty

		}


		internal static void Track ( Form f )
			=> Tracker.Track(f);

	}

}


namespace uom.Extensions
{

	internal static class Extensions_JotServices
	{


		extension( Form form )
		{

			public void attach_PositionAndStateSaver_Jot ()
				=> Jot.JotServices.Track(form);
		}
	}
}