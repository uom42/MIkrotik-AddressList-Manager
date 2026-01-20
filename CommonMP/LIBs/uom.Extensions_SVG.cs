
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





            public static SvgDocument eLoadSVGFromResourceFile ( this Assembly asm , string svgFileEndsWith )
            {
                var svgResourceFile = asm.getManifestResourceNames(s => s.EndsWith(svgFileEndsWith , StringComparison.InvariantCultureIgnoreCase)).Single();
                using var sm = uom.AppInfo.AppAssembly.GetManifestResourceStream(svgResourceFile);
                var svg = SvgDocument.Open<SvgDocument>(sm);
                return svg;
            }



            public static Bitmap eLoadSVGFromResourceFileAsBitmap ( this Assembly asm , string svgFileEndsWith , Size iconSize )
                => asm
                    .eLoadSVGFromResourceFile(svgFileEndsWith)
                    .toBitmap(iconSize);





            public static void eReColorizeNodes ( this IEnumerable<SvgElement> nodes , SvgPaintServer colorServer )
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

                    eReColorizeNodes(node.Descendants() , colorServer);
                }
            }



            public static void reColorize ( this SvgDocument svg , Color clr )
                => svg.Descendants().eReColorizeNodes(new SvgColourServer(clr));



            public static Bitmap toBitmap ( this SvgDocument svg , Size iconSize , Color? iconColor = null , bool makeTransparent = true )
            {
                if ( iconColor.HasValue ) svg.reColorize(iconColor.Value);

                var bm = svg.Draw(iconSize.Width , iconSize.Height);
                if ( makeTransparent ) bm.MakeTransparent();
                return bm;
            }

            public static Bitmap toBitmap ( this SvgDocument svg , bool useSmallIconSize , Color? iconColor = null , bool makeTransparent = true )
            {
                var iconSize = useSmallIconSize
                    ? System.Windows.Forms.SystemInformation.SmallIconSize
                    : System.Windows.Forms.SystemInformation.IconSize;

                if ( iconColor.HasValue ) svg.reColorize(iconColor.Value);

                var bm = svg.Draw(iconSize.Width , iconSize.Height);
                if ( makeTransparent ) bm.MakeTransparent();
                return bm;
            }


            public static Icon toIcon ( this SvgDocument svg , Size iconSize )
            {
                var bm = svg.toBitmap(iconSize);
                var i = bm.GetHicon();
                var icn = Icon.FromHandle(i);

                return icn;
            }


            public static Icon eToIcon ( this SvgDocument svg , bool useSmallIconSize = false )
            {
                var iconSize = useSmallIconSize
                    ? SystemInformation.SmallIconSize
                    : SystemInformation.IconSize;

                return svg.toIcon(iconSize);
            }



            public static Stream eToIconSet ( this SvgDocument svg , uint[]? iconSizes = null )
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



            public static void eWriteIconSet ( this SvgDocument svg , Stream output , uint[]? iconSizes = null )
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

#pragma warning restore IDE1006 // Naming Styles

}