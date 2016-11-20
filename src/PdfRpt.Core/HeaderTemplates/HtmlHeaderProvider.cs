using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.Core.Helper.HtmlToPdf;

namespace PdfRpt.HeaderTemplates
{
    /// <summary>
    /// Defines dynamic headers for pages and individual groups by using iTextSharp's limited HTML to PDF capabilities (HTMLWorker class).
    /// </summary>
    public class HtmlHeaderProvider : IPageHeader
    {
        /// <summary>
        /// Properties of page headers.
        /// </summary>
        public HeaderBasicProperties PageHeaderProperties { set; get; }

        /// <summary>
        /// Properties of group headers.
        /// </summary>
        public HeaderBasicProperties GroupHeaderProperties { set; get; }

        /// <summary>
        /// Returns dynamic HTML content of the group header.
        /// </summary>
        public Func<HeaderData, string> AddGroupHeader { set; get; }

        /// <summary>
        /// Fires when a new groups is being created.
        /// </summary>
        public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData)
        {
            var groupHeaderHtml = AddGroupHeader(new HeaderData
            {
                NewGroupInfo = newGroupInfo,
                PdfDoc = pdfDoc,
                PdfWriter = pdfWriter,
                SummaryData = summaryData
            });

            return createTable(groupHeaderHtml, GroupHeaderProperties);
        }

        /// <summary>
        /// Returns dynamic HTML content of the page header.
        /// </summary>
        public Func<HeaderData, string> AddPageHeader { set; get; }

        /// <summary>
        /// Fires when a new page is being added.
        /// </summary>
        public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
        {
            var pageHeaderHtml = AddPageHeader(new HeaderData
            {
                NewGroupInfo = null,
                PdfDoc = pdfDoc,
                PdfWriter = pdfWriter,
                SummaryData = summaryData
            });

            return createTable(pageHeaderHtml, PageHeaderProperties);
        }

        private PdfGrid createTable(string html, HeaderBasicProperties basicProperties)
        {
            var table = new PdfGrid(1)
            {
                RunDirection = (int)basicProperties.RunDirection,
                WidthPercentage = basicProperties.TableWidthPercentage
            };
            var htmlCell = new HtmlWorkerHelper
            {
                PdfFont = basicProperties.PdfFont,
                HorizontalAlignment = basicProperties.HorizontalAlignment,
                Html = html,
                RunDirection = basicProperties.RunDirection,
                StyleSheet = basicProperties.StyleSheet
            }.RenderHtml();
            htmlCell.HorizontalAlignment = (int)basicProperties.HorizontalAlignment;
            htmlCell.Border = 0;
            table.AddCell(htmlCell);

            if (basicProperties.ShowBorder)
                return table.AddBorderToTable(basicProperties.BorderColor, basicProperties.SpacingBeforeTable);
            table.SpacingBefore = basicProperties.SpacingBeforeTable;

            return table;
        }
    }
}
