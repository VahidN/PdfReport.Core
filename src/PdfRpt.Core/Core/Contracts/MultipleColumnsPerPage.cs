
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Wrapping main table in multiple columns per pages
    /// </summary>
    public class MultipleColumnsPerPage
    {
        /// <summary>
        /// Width of the each column
        /// </summary>
        public float ColumnsWidth { set; get; }

        /// <summary>
        /// Gap/Margin between columns
        /// </summary>
        public float ColumnsGap { set; get; }

        /// <summary>
        /// Number of the columns per pages
        /// </summary>
        public int ColumnsPerPage { set; get; }

        /// <summary>
        /// Sets starting from right of the screen
        /// </summary>
        public bool IsRightToLeft { set; get; }

        /// <summary>
        /// A gap between the main table and the header
        /// </summary>
        public float TopMargin { set; get; }
    }
}
