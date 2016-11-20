using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.HeaderTemplates
{
    /// <summary>
    /// Defines dynamic headers for pages and individual groups.
    /// </summary>
    public class InlineHeaderProvider : IPageHeader
    {
        /// <summary>
        /// Returns dynamic content of the group header.
        /// </summary>
        public Func<HeaderData, PdfGrid> AddGroupHeader { set; get; }

        /// <summary>
        /// Fires when a new groups is being created.
        /// </summary>
        public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData)
        {
            if (AddGroupHeader == null)
                return null;

            return AddGroupHeader(new HeaderData
            {
                NewGroupInfo = newGroupInfo,
                PdfDoc = pdfDoc,
                PdfWriter = pdfWriter,
                SummaryData = summaryData
            });
        }

        /// <summary>
        /// Returns dynamic content of the page header.
        /// </summary>
        public Func<HeaderData, PdfGrid> AddPageHeader { set; get; }

        /// <summary>
        /// Fires when a new page is being added.
        /// </summary>
        public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
        {
            return AddPageHeader(new HeaderData
            {
                NewGroupInfo = null,
                PdfDoc = pdfDoc,
                PdfWriter = pdfWriter,
                SummaryData = summaryData
            });
        }
    }
}