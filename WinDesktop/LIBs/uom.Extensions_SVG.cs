#nullable enable


using System.Runtime.CompilerServices;

using Svg;


#pragma warning disable IDE1006 // Naming Styles



namespace uom.Extensions;


/// <summary>
/// Svg by davescriven
/// </summary>
internal static class Extensions_SVG_NET
{




	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static SvgDocument eLoadSVGFromResourceFile ( this Assembly asm, string svgFileEndsWith )
	{
		var svgResourceFile = asm.eGetManifestResourceNames (s => s.EndsWith (svgFileEndsWith, StringComparison.InvariantCultureIgnoreCase)).Single ();
		using var sm = uom.AppInfo.Assembly.GetManifestResourceStream (svgResourceFile);
		var svg = SvgDocument.Open<SvgDocument> (sm);
		return svg;
	}


	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static Bitmap eLoadSVGFromResourceFileAsBitmap ( this Assembly asm, string svgFileEndsWith, Size iconSize )
		=> asm
			.eLoadSVGFromResourceFile (svgFileEndsWith)
			.eToBitmap (iconSize);




	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static void eReColorizeNodes ( this IEnumerable<SvgElement> nodes, SvgPaintServer colorServer )
	{
		foreach (var node in nodes)
		{
			if (node.Fill != SvgPaintServer.None)
				node.Fill = colorServer;
			else
			{
				//if (node.Color != SvgPaintServer.None) node.Color = colorServer;
				//if (node. .StopColor != SvgPaintServer.None) node.StopColor = colorServer;
				if (node.Stroke != SvgPaintServer.None) node.Stroke = colorServer;
			}

			eReColorizeNodes (node.Descendants (), colorServer);
		}
	}


	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static void eReColorize ( this SvgDocument svg, Color clr )
		=> svg.Descendants ().eReColorizeNodes (new SvgColourServer (clr));


	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static Bitmap eToBitmap ( this SvgDocument svg, Size iconSize, Color? iconColor = null, bool makeTransparent = true )
	{
		if (iconColor.HasValue) svg.eReColorize (iconColor.Value);

		var bm = svg.Draw (iconSize.Width, iconSize.Height);
		if (makeTransparent) bm.MakeTransparent ();
		return bm;
	}


	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static Bitmap eToBitmap ( this SvgDocument svg, bool useSmallIconSize, Color? iconColor = null, bool makeTransparent = true )
	{
		var iconSize = useSmallIconSize
			? System.Windows.Forms.SystemInformation.SmallIconSize
			: System.Windows.Forms.SystemInformation.IconSize;

		if (iconColor.HasValue) svg.eReColorize (iconColor.Value);

		var bm = svg.Draw (iconSize.Width, iconSize.Height);
		if (makeTransparent) bm.MakeTransparent ();
		return bm;
	}



	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static Icon eToIcon ( this SvgDocument svg, Size iconSize )
	{
		var bm = svg.eToBitmap (iconSize);
		var i = bm.GetHicon ();
		var icn = Icon.FromHandle (i);

		return icn;
	}

	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static Icon eToIcon ( this SvgDocument svg, bool useSmallIconSize = false )
	{
		var iconSize = useSmallIconSize
			? SystemInformation.SmallIconSize
			: SystemInformation.IconSize;

		return svg.eToIcon (iconSize);
	}


	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static Stream eToIconSet ( this SvgDocument svg, uint[]? iconSizes = null )
	{
		iconSizes ??= [ 16, 24, 32, 64, 128, 256 ];

		var bitmaps = iconSizes
			.Select (w =>
			{
				Size sz = new ((int) w, (int) w);
				var bm = svg.eToBitmap (sz);
				return bm;
			})
			.ToArray ();

		using var ss = bitmaps.eSaveAsMultisizedIconStream ();
		return ss;
	}


	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static void eWriteIconSet ( this SvgDocument svg, Stream output, uint[]? iconSizes = null )
	{
		iconSizes = [ 16, 32, 64, 128, 256 ];

		var bitmaps = iconSizes
			.Select (w =>
			{
				Size sz = new ((int) w, (int) w);
				var bm = svg.eToBitmap (sz);
				return bm;
			})
			.ToArray ();

		using var ss = bitmaps.eSaveAsMultisizedIconStream ();
		ss.CopyTo (output);
		return;
	}



}


#pragma warning restore IDE1006 // Naming Styles

