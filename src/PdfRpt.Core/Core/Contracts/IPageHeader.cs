using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Defines dynamic headers for pages and individual groups.
    /// </summary>
    public interface IPageHeader
    {
        /// <summary>
        /// Fires when a new groups is being created.
        /// </summary>
        PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData);

        /// <summary>
        /// Fires when a new page is being added.
        /// </summary>        
        PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData);
    }
}
