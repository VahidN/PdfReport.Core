using System.Collections.Generic;
using iTextSharp.text;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Properties of pages and groups headerds
    /// </summary>
    public class XHeaderBasicProperties
    {
        /// <summary>
        /// Width percentage of the table. Its default value is 100.
        /// </summary>
        public float TableWidthPercentage { set; get; }

        /// <summary>
        /// Spacing before each table.
        /// </summary>
        public float SpacingBeforeTable { set; get; }

        /// <summary>
        /// Adds a border to an existing PdfGrid.
        /// </summary>
        public bool ShowBorder { set; get; }

        /// <summary>
        /// Border's Color. Its default value is BaseColor.LIGHT_GRAY.
        /// </summary>
        public BaseColor BorderColor { set; get; }

        /// <summary>
        /// A Possible run direction value, left-to-right or right-to-left.
        /// </summary>
        public PdfRunDirection RunDirection { set; get; }

        /// <summary>
        /// Optional external CSS files.
        /// </summary>
        public IList<string> CssFilesPath { set; get; }

        /// <summary>
        /// Optional inline CSS content.
        /// </summary>
        public string InlineCss { set; get; }

        /// <summary>
        /// Optional images directory path.
        /// </summary>
        public string ImagesPath { set; get; }

        /// <summary>
        /// Message's font.
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public XHeaderBasicProperties()
        {
            BorderColor = BaseColor.LightGray;
            TableWidthPercentage = 100;
            try
            {
                PdfFont = new GenericFontProviderBuilder().GenericFontProvider;
            }
            catch { }
        }
    }
}