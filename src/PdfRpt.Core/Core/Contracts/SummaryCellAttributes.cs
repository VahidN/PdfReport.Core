
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// General summary settings of pages and groups
    /// </summary>
    public class SummaryCellAttributes
    {
        /// <summary>
        /// Sets the visibility of the cell
        /// </summary>
        public bool ShowOnEachPage { get; set; }

        /// <summary>
        /// Sets the location of summary cell's label, based on the available visible properties.
        /// </summary>
        public string LabelColumnProperty { get; set; }

        /// <summary>
        /// Sets summary cell's label horizontal alignment
        /// </summary>
        public HorizontalAlignment? LabelHorizontalAlignment { get; set; }

        /// <summary>
        /// Sets the value of summary cell's label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Summary Cell's Location
        /// </summary>
        public SummaryLocation SummaryLocation { set; get; }
    }
}
