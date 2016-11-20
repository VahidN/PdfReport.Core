using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// Allows catching several document events.
    /// </summary>
    public class PageEvents : PdfPageEventHelper
    {
        BackgroundImageTemplate _backgroundImageTemplate;
        DiagonalWatermarkManager _diagonalWatermarkManager;
        HeaderFooterManager _headerFooterManager;

        /// <summary>
        /// Rows summaries data
        /// </summary>
        public IList<SummaryCellData> ColumnSummaryCellsData { set; get; }

        /// <summary>
        /// Stores the last rendered row's data
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// Main table's cells and rows events
        /// </summary>
        public Events MainTableEvents { get; set; }

        /// <summary>
        /// Document settings
        /// </summary>
        public DocumentPreferences PageSetup { set; get; }

        /// <summary>
        /// Defining which properties of MainTableDataSource should be rendered and how.
        /// If you don't set it, list of the main table's columns will be extracted from MainTableDataSource automatically.
        /// </summary>
        /// <returns></returns>
        public IList<ColumnAttributes> PdfColumnsAttributes { get; set; }

        /// <summary>
        /// Pdf document's font
        /// </summary>
        public IPdfFont PdfFont { get; set; }

        /// <summary>
        /// Optional custom footer of the pages.
        /// </summary>
        public IPageFooter PdfRptFooter { get; set; }

        /// <summary>
        /// Optional custom header of the pages.
        /// </summary>
        public IPageHeader PdfRptHeader { set; get; }

        /// <summary>
        /// Fires when the document is closed.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            _backgroundImageTemplate.ApplyBackgroundImage(document);
            _headerFooterManager.ApplyFooter(writer, document, ColumnSummaryCellsData);
            _diagonalWatermarkManager.ApplyWatermark(document);
        }

        /// <summary>
        /// Fires when a page is finished, just before being written to the document.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            if (!shouldSkipFooter(writer, document))
            {
                _headerFooterManager.AddFooter(writer, document, ColumnSummaryCellsData);
            }

            _diagonalWatermarkManager.ReserveWatermarkSpace(writer);
        }

        /// <summary>
        /// Fires when the document is opened.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            initBackgroundImageTemplate(writer, document);
            initHeaderFooterManager(writer);
            initDiagonalWatermarkManager(writer, document);
        }

        /// <summary>
        /// Fires when a page is initialized.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            if (!shouldSkipHeader(writer, document))
            {
                _headerFooterManager.AddHeader(writer, document);
            }

            _backgroundImageTemplate.ReserveBackgroundImageSpace(writer);
        }

        private EventsArguments getEventsArguments(PdfWriter writer, Document document)
        {
            return new EventsArguments
            {
                PdfDoc = document,
                PdfWriter = writer,
                RowType = RowType.HeaderRow,
                ColumnCellsSummaryData = ColumnSummaryCellsData,
                PreviousTableRowData = CurrentRowInfoData.PreviousTableRowData,
                PageSetup = PageSetup,
                PdfFont = PdfFont,
                PdfColumnsAttributes = PdfColumnsAttributes
            };
        }

        private void initBackgroundImageTemplate(PdfWriter writer, Document document)
        {
            _backgroundImageTemplate = new BackgroundImageTemplate { PageSetup = PageSetup };
            _backgroundImageTemplate.InitBackgroundImageTemplate(writer, document);
        }

        private void initDiagonalWatermarkManager(PdfWriter writer, Document document)
        {
            _diagonalWatermarkManager = new DiagonalWatermarkManager { PageSetup = PageSetup };
            _diagonalWatermarkManager.InitWatermarkTemplate(writer, document);
        }

        private void initHeaderFooterManager(PdfWriter writer)
        {
            _headerFooterManager = new HeaderFooterManager
            {
                ColumnSummaryCellsData = ColumnSummaryCellsData,
                CurrentRowInfoData = CurrentRowInfoData,
                PdfRptFooter = PdfRptFooter,
                PdfRptHeader = PdfRptHeader,
                CacheHeader = PageSetup.PagePreferences.CacheHeader
            };
            _headerFooterManager.InitFooter(writer, ColumnSummaryCellsData);
        }

        private bool shouldSkipFooter(PdfWriter writer, Document document)
        {
            return MainTableEvents != null && MainTableEvents.ShouldSkipFooter(getEventsArguments(writer, document));
        }

        private bool shouldSkipHeader(PdfWriter writer, Document document)
        {
            return MainTableEvents != null && MainTableEvents.ShouldSkipHeader(getEventsArguments(writer, document));
        }
    }
}