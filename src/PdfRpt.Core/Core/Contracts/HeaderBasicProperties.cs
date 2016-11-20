using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Properties of pages and groups headerds
    /// </summary>
    public class HeaderBasicProperties
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
        /// Defines styles for HTMLWorker, such as StyleSheet.LoadTagStyle(HtmlTags.IMG, HtmlTags.ALIGN, HtmlTags.ALIGN_CENTER)
        /// </summary>
        public StyleSheet StyleSheet { set; get; }

        /// <summary>
        /// Cells horizontal alignment value
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { set; get; }

        /// <summary>
        /// Message's font.
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public HeaderBasicProperties()
        {
            BorderColor = BaseColor.LightGray;
            TableWidthPercentage = 100;
        }
    }
}