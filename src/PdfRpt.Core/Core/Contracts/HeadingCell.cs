namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Defines a heading cell
    /// </summary>
    public class HeadingCell
    {
        /// <summary>
        /// If true, the current header cell (and not its data column) will be merged with the next one and the next label will be ignored.
        /// </summary>
        public bool MergeHeaderCell { set; get; }

        /// <summary>
        /// The string to be displayed as the current column's caption.
        /// </summary>
        public string Caption { set; get; }

        /// <summary>
        /// The rotation of the column's caption. Possible values are 0, 90, 180 and 270.
        /// </summary>
        public int CaptionRotation { set; get; }

        /// <summary>
        /// Content's Horizontal alignment.
        /// If null, IPdfRptTemplate.HeaderHorizontalAlignment will be used.
        /// </summary>
        public HorizontalAlignment? HorizontalAlignment { set; get; }
    }
}