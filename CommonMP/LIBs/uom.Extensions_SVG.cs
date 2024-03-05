#nullable enable


using Svg;


#pragma warning disable IDE1006 // Naming Styles

namespace uom.Extensions;


internal static class Extensions_SVG_NET
{




	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static SvgDocument eLoadSVGFromResourceFile(this Assembly asm, string svgFileEndsWith)
	{
		var svgResourceFile = asm.eGetManifestResourceNames(s => s.EndsWith(svgFileEndsWith)).Single();
		using var sm = uom.AppInfo.Assembly.GetManifestResourceStream(svgResourceFile);
		var svg = SvgDocument.Open<SvgDocument>(sm);
		return svg;
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Bitmap eLoadSVGFromResourceFileToBitmap(this Assembly asm, string svgFileEndsWith, Size iconSize)
		=> asm
			.eLoadSVGFromResourceFile(svgFileEndsWith)
			.eToBitmap(iconSize);




	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void eReColorizeNodes(this IEnumerable<SvgElement> nodes, SvgPaintServer colorServer)
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

			eReColorizeNodes(node.Descendants(), colorServer);
		}
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void eReColorize(this SvgDocument svg, Color clr)
		=> svg.Descendants().eReColorizeNodes(new SvgColourServer(clr));


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Bitmap eToBitmap(this SvgDocument svg, Size iconSize, Color? iconColor = null, bool makeTransparent = true)
	{
		if (iconColor.HasValue) svg.eReColorize(iconColor.Value);

		var bm = svg.Draw(iconSize.Width, iconSize.Height);
		if (makeTransparent) bm.MakeTransparent();
		return bm;
	}

}




#pragma warning restore IDE1006 // Naming Styles
