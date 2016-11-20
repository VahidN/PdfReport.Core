using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Header callbacks data.
    /// </summary>
    public class HeaderData
    {
        /// <summary>
        /// Pdf Document Object.
        /// </summary>
        public Document PdfDoc {set;get;}

        /// <summary>
        /// PdfWriter Object.
        /// </summary>
        public PdfWriter PdfWriter {set;get;}

        /// <summary>
        /// Cells of the new group's first row. It can be null.
        /// </summary>
        public IList<CellData> NewGroupInfo {set;get;}

        /// <summary>
        /// Holds summary info of the main table's rows and cells.
        /// </summary>
        public IList<SummaryCellData> SummaryData { set; get; }
    }
}
