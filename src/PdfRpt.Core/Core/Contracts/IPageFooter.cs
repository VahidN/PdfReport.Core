using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Defines custom footer of the each page.
    /// </summary>
    public interface IPageFooter
    {
        /// <summary>
        /// Fires when the document is opened.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="columnCellsSummaryData">List of all rows summaries data</param>
        void DocumentOpened(PdfWriter writer, IList<SummaryCellData> columnCellsSummaryData);

        /// <summary>
        /// Fires when a page is finished, just before being written to the document.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        /// <param name="columnCellsSummaryData">List of all rows summaries data</param>
        void PageFinished(PdfWriter writer, Document document, IList<SummaryCellData> columnCellsSummaryData);

        /// <summary>
        /// Fires before closing the document
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        /// <param name="columnCellsSummaryData">List of all rows summaries data</param>
        void ClosingDocument(PdfWriter writer, Document document, IList<SummaryCellData> columnCellsSummaryData);
    }
}
