
using System.Runtime.CompilerServices;

using Svg;


namespace uom
{

	internal static partial class AppTools
	{

		/// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>

		public static Icon GetEmbeddedSVGResourceAsIcon ( string embeddedResourceFileSuffix , Size iconSize )
		{
			using Stream stream = GetEmbeddedResourceStream(embeddedResourceFileSuffix);
			SvgDocument svgDoc = SvgDocument.Open<SvgDocument>(stream);
			var ic = svgDoc.toIcon(iconSize);
			return ic;




		}

		/// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>

		public static async Task<Icon> GetEmbeddedSVGResourceAsIconAsync ( string embeddedResourceFileSuffix , Size iconSize )
			=> await Task.Factory.StartNew(() => GetEmbeddedSVGResourceAsIcon(embeddedResourceFileSuffix , iconSize));



	}







#pragma warning disable IDE1006 // Naming Styles
	namespace Extensions
	{

		/// <summary>
		/// Svg by davescriven
		/// </summary>
		internal static class Extensions_SVG_NET
		{




			extension( Assembly asm )
			{



				public SvgDocument loadSVGFromResourceFile ( string svgFileSuffix )
				{
					var svgResourceFile = asm.getManifestResourceNames(s => s.EndsWith(svgFileSuffix , StringComparison.OrdinalIgnoreCase)).Single();
					using var sm = uom.AppInfo.AppAssembly.GetManifestResourceStream(svgResourceFile);
					var svg = SvgDocument.Open<SvgDocument>(sm);
					return svg;
				}


				public Bitmap loadSVGFromResourceFileAsBitmap ( string svgFileSuffix , Size iconSize )
					=> asm
					.loadSVGFromResourceFile(svgFileSuffix)
					.toBitmap(iconSize);

			}


			public static void reColorizeNodes ( this IEnumerable<SvgElement> nodes , SvgPaintServer colorServer )
			{
				foreach ( var node in nodes )
				{
					if ( node.Fill != SvgPaintServer.None )
						node.Fill = colorServer;
					else
					{
						//if (node.Color != SvgPaintServer.None) node.Color = colorServer;
						//if (node. .StopColor != SvgPaintServer.None) node.StopColor = colorServer;
						if ( node.Stroke != SvgPaintServer.None ) node.Stroke = colorServer;
					}

					reColorizeNodes(node.Descendants() , colorServer);
				}
			}



			extension( SvgDocument svg )
			{


				public void reColorize ( Color clr )
					=> svg.Descendants().reColorizeNodes(new SvgColourServer(clr));



				public Bitmap toBitmap ( Size iconSize , Color? iconColor = null , bool makeTransparent = true )
				{
					if ( iconColor.HasValue ) svg.reColorize(iconColor.Value);

					var bm = svg.Draw(iconSize.Width , iconSize.Height);
					if ( makeTransparent ) bm.MakeTransparent();
					return bm;
				}

				public Bitmap toBitmap ( bool useSmallIconSize , Color? iconColor = null , bool makeTransparent = true )
				{
					var iconSize = useSmallIconSize
						? System.Windows.Forms.SystemInformation.SmallIconSize
						: System.Windows.Forms.SystemInformation.IconSize;

					if ( iconColor.HasValue ) svg.reColorize(iconColor.Value);

					var bm = svg.Draw(iconSize.Width , iconSize.Height);
					if ( makeTransparent ) bm.MakeTransparent();
					return bm;
				}


				public Icon toIcon ( Size iconSize )
				{
					var bm = svg.toBitmap(iconSize);
					var i = bm.GetHicon();
					var icn = Icon.FromHandle(i);

					return icn;
				}


				public Icon toIcon ( bool useSmallIconSize = false )
				{
					var iconSize = useSmallIconSize
						? SystemInformation.SmallIconSize
						: SystemInformation.IconSize;

					return svg.toIcon(iconSize);
				}



				public Stream toIconSet ( uint[]? iconSizes = null )
				{
					iconSizes ??= [ 16 , 24 , 32 , 64 , 128 , 256 ];

					var bitmaps = iconSizes
						.Select(w =>
						{
							Size sz = new((int)w , (int)w);
							var bm = svg.toBitmap(sz);
							return bm;
						})
						.ToArray();

					using var ss = bitmaps.saveAsMultisizedIconStream();
					return ss;
				}



				public void writeIconSet ( Stream output , uint[]? iconSizes = null )
				{
					iconSizes = [ 16 , 32 , 64 , 128 , 256 ];

					var bitmaps = iconSizes
						.Select(w =>
						{
							Size sz = new((int)w , (int)w);
							var bm = svg.toBitmap(sz);
							return bm;
						})
						.ToArray();

					using var ss = bitmaps.saveAsMultisizedIconStream();
					ss.CopyTo(output);
					return;
				}


			}

		}

	}

#pragma warning restore IDE1006 // Naming Styles

}