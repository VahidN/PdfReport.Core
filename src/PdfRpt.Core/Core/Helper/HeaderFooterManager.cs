using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// A class to encapsulate header and footer related methods.
    /// </summary>
    public class HeaderFooterManager
    {
        private PdfGrid _header;

        /// <summary>
        /// You can define different headers for each page. 
        /// If all of the headers of the document's pages are the same, set this value to true, to optimize the performance and document size.
        /// </summary>
        public bool CacheHeader { set; get; }

        /// <summary>
        /// Stores the last rendered row's data
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// Optional custom footer of the pages.
        /// </summary>
        public IPageFooter PdfRptFooter { get; set; }

        /// <summary>
        /// Rows summaries data
        /// </summary>
        public IList<SummaryCellData> ColumnSummaryCellsData { set; get; }

        /// <summary>
        /// Optional custom header of the pages.
        /// </summary>
        public IPageHeader PdfRptHeader { set; get; }

        /// <summary>
        /// Fires when a page is finished, just before being written to the document.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        /// <param name="columnCellsSummaryData">Rows summaries data</param>
        public void AddFooter(PdfWriter writer, Document document, IList<SummaryCellData> columnCellsSummaryData)
        {
            if (PdfRptFooter == null) return;
            PdfRptFooter.PageFinished(writer, document, columnCellsSummaryData);
        }

        /// <summary>
        /// Fires when a new page is being added
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public void AddHeader(PdfWriter writer, Document document)
        {
            if (PdfRptHeader == null) return;

            if (!CacheHeader || _header == null)
                _header = PdfRptHeader.RenderingReportHeader(document, writer, ColumnSummaryCellsData);

            if (_header == null) return;

            document.Add(_header);
            CurrentRowInfoData.HeaderHeight = _header.TotalHeight;
        }

        /// <summary>
        /// Fires before closing the document
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        /// <param name="columnCellsSummaryData">Rows summaries data</param>
        public void ApplyFooter(PdfWriter writer, Document document, IList<SummaryCellData> columnCellsSummaryData)
        {
            if (PdfRptFooter == null) return;
            PdfRptFooter.ClosingDocument(writer, document, columnCellsSummaryData);
        }

        /// <summary>
        /// Fires when the document is opened
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="columnCellsSummaryData">Rows summaries data</param>
        public void InitFooter(PdfWriter writer, IList<SummaryCellData> columnCellsSummaryData)
        {
            if (PdfRptFooter == null) return;
            PdfRptFooter.DocumentOpened(writer, columnCellsSummaryData);
        }
    }
}
