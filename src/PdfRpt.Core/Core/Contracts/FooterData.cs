using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Footer callbacks data.
    /// </summary>
    public class FooterData
    {
        /// <summary>
        /// Current Page Number.
        /// </summary>
        public int CurrentPageNumber { set; get; }

        /// <summary>
        /// Pdf Document Object.
        /// </summary>
        public Document PdfDoc { set; get; }

        /// <summary>
        /// PdfWriter Object.
        /// </summary>
        public PdfWriter PdfWriter { set; get; }

        /// <summary>
        /// Holds summary info of the main table's rows and cells.
        /// </summary>
        public IList<SummaryCellData> SummaryData { set; get; }

        /// <summary>
        /// This image will be filled with the total pages number of the document
        /// at the end of the process.
        /// </summary>
        public Image TotalPagesCountImage { set; get; }
    }
}
