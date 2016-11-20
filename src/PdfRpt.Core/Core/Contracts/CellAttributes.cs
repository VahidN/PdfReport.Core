
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// A wrapper class around important main table's cells attributes
    /// </summary>
    public class CellAttributes
    {
        /// <summary>
        /// Basic properties of the main table's cells
        /// </summary>
        public CellBasicProperties BasicProperties { set; get; }

        /// <summary>
        /// Cell's template
        /// </summary>
        public IColumnItemsTemplate ItemTemplate { set; get; }

        /// <summary>
        /// PdfCells Shared Data
        /// </summary>
        public CellSharedData SharedData { set; get; }

        /// <summary>
        /// PdfCell's Raw Data
        /// </summary>
        public CellRowData RowData { set; get; }
    }
}
