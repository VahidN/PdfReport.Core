using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// PdfCells Shared Data
    /// </summary>
    public class CellSharedData
    {
        /// <summary>
        /// Cell's overall row number value
        /// </summary>
        public int DataRowNumber { set; get; }

        /// <summary>
        /// Cell's group number value
        /// </summary>
        public int GroupNumber { set; get; }

        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        public ColumnAttributes PdfColumnAttributes { set; get; }

        /// <summary>
        /// PDF Document object
        /// </summary>
        public Document PdfDoc { get;  set; }

        /// <summary>
        /// PdfWriter object
        /// </summary>
        public PdfWriter PdfWriter { get;  set; }

        /// <summary>
        /// Pages and groups summary values settings.
        /// </summary>
        public SummaryCellSettings SummarySettings { set; get; }

        /// <summary>
        /// Main table's template
        /// </summary>
        public ITableTemplate Template { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public CellSharedData()
        {
            SummarySettings = new SummaryCellSettings();
        }
    }
}
