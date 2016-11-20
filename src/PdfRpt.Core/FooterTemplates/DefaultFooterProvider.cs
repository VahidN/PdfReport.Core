using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FooterTemplates
{
    /// <summary>
    /// A helper class/sample which demonstrates how to implement the IPdfRptCustomFooter to produce the pdfDoc's footer
    /// </summary>
    public class DefaultFooterProvider : IPageFooter
    {
        #region Fields (4)

        PdfContentByte _pdfContentByte;
        readonly IPdfFont _pdfRptFont;
        readonly string _printDate;
        readonly PdfRunDirection _direction;
        // we will put the final number of pages in a template
        PdfTemplate _totalPageCountTemplate;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Our DefaultFooter writes current date and page numbers at the bottom of the pages.
        /// </summary>
        /// <param name="pdfRptFont">Selected font</param>
        /// <param name="printDate">Current date</param>
        /// <param name="direction">rtl or ltr</param>
        public DefaultFooterProvider(IPdfFont pdfRptFont, string printDate, PdfRunDirection direction)
        {
            _pdfRptFont = pdfRptFont;
            _printDate = printDate;
            _direction = direction;
        }

        #endregion Constructors

        #region Methods (7)

        // Public Methods (3)

        /// <summary>
        /// Fires before closing the document
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        /// <param name="columnCellsSummaryData">List of all rows summaries data</param>
        public void ClosingDocument(PdfWriter writer, Document document, IList<SummaryCellData> columnCellsSummaryData)
        {
            setFinalPageNumber(writer);
        }

        /// <summary>
        /// Fires when a page is finished, just before being written to the document.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        /// <param name="columnCellsSummaryData">List of all rows summaries data</param>
        public void PageFinished(PdfWriter writer, Document document, IList<SummaryCellData> columnCellsSummaryData)
        {
            var pageSize = addTotalPageNumebersTemplate(writer, document);
            addPrintDate(pageSize, _printDate);
        }

        /// <summary>
        /// Fires when the document is opened.
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="columnCellsSummaryData">List of all rows summaries data</param>
        public void DocumentOpened(PdfWriter writer, IList<SummaryCellData> columnCellsSummaryData)
        {
            initTemplate(writer);
        }
        // Private Methods (4)

        private void addPrintDate(Rectangle pageSize, string printTime)
        {
            ColumnText.ShowTextAligned(
                        canvas: _pdfContentByte,
                        alignment: Element.ALIGN_RIGHT,
                        phrase: _pdfRptFont.FontSelector.Process(printTime),
                        x: pageSize.GetRight(40),
                        y: pageSize.GetBottom(30),
                        rotation: 0,
                        runDirection: (int)_direction,
                        arabicOptions: 0);
        }

        private Rectangle addTotalPageNumebersTemplate(PdfWriter writer, Document document)
        {
            var pageN = writer.PageNumber;
            var text = pageN + " / ";
            var len = _pdfRptFont.Fonts[0].BaseFont.GetWidthPoint(text, 8);

            var pageSize = document.PageSize;

            _pdfContentByte.SetRgbColorFill(100, 100, 100);

            _pdfContentByte.BeginText();
            _pdfContentByte.SetFontAndSize(_pdfRptFont.Fonts[0].BaseFont, 8);
            _pdfContentByte.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(30));
            _pdfContentByte.ShowText(text);
            _pdfContentByte.EndText();

            _pdfContentByte.AddTemplate(_totalPageCountTemplate, pageSize.GetLeft(40) + len, pageSize.GetBottom(30));
            return pageSize;
        }

        private void initTemplate(PdfWriter writer)
        {
            _totalPageCountTemplate = writer.DirectContent.CreateTemplate(50, 50);
            _pdfContentByte = writer.DirectContent;
        }

        private void setFinalPageNumber(PdfWriter writer)
        {
            _totalPageCountTemplate.BeginText();
            _totalPageCountTemplate.SetFontAndSize(_pdfRptFont.Fonts[0].BaseFont, 8);
            _totalPageCountTemplate.SetTextMatrix(0, 0);
            _totalPageCountTemplate.ShowText("" + (writer.PageNumber - 1));
            _totalPageCountTemplate.EndText();
        }

        #endregion Methods
    }
}
