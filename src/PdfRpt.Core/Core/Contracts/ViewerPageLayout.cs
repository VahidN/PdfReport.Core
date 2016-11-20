using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Page layout values. Specifies the page layout to be used when a document is opened in Adobe Reader.
    /// </summary>
    public enum ViewerPageLayout
    {
        /// <summary>
        /// Displays one page at a time (this is the default).
        /// </summary>
        SinglePage = PdfWriter.PageLayoutSinglePage,

        /// <summary>
        /// Displays the pages in one column.
        /// </summary>
        OneColumn = PdfWriter.PageLayoutOneColumn,

        /// <summary>
        /// Displays the pages in two columns, with odd numbered pages on the left.
        /// </summary>
        TwoColumnLeft = PdfWriter.PageLayoutTwoColumnLeft,

        /// <summary>
        /// Displays the pages in two columns, with odd numbered pages on the right.
        /// </summary>
        TwoColumnRight = PdfWriter.PageLayoutTwoColumnRight,

        /// <summary>
        /// Displays the pages two at a time, with odd numbered pages on the left.
        /// </summary>
        TwoPageLeft = PdfWriter.PageLayoutTwoPageLeft,

        /// <summary>
        /// Displays the pages two at a time, with oddnumbered pages on the right.
        /// </summary>
        TwoPageRight = PdfWriter.PageLayoutTwoPageRight
    }
}
