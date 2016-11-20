using System;
using PdfRpt.Core.Contracts;
using PdfRpt.FooterTemplates;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Pages Footer Builder Class.
    /// </summary>
    public class PagesFooterBuilder
    {
        readonly PdfReport _pdfReport;
        private IPdfFont _font;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public PagesFooterBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
            _font = new GenericFontProvider(_pdfReport.DataBuilder.PdfFont);
        }

        /// <summary>
        /// Gets/Sets the default fonts of the footer.
        /// </summary>
        public IPdfFont PdfFont
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// Using the DefaultFooterProvider class.
        /// </summary>
        /// <param name="printDate">Sets the optional print date value of the DefaultFooterProvider.</param>
        /// <param name="direction">Possible run direction values, left-to-right or right-to-left.</param>
        public void DefaultFooter(string printDate, PdfRunDirection direction = PdfRunDirection.LeftToRight)
        {
            var footer = new DefaultFooterProvider(PdfFont, printDate, direction);
            _pdfReport.DataBuilder.SetFooter(footer);
        }

        /// <summary>
        /// Using the predefined HtmlFooterProvider class.
        /// </summary>
        /// <param name="htmlFooterProviderBuilder">Footer Provider Builder</param>
        public void HtmlFooter(Action<HtmlFooterProviderBuilder> htmlFooterProviderBuilder)
        {
            var builder = new HtmlFooterProviderBuilder();
            htmlFooterProviderBuilder(builder);
            _pdfReport.DataBuilder.SetFooter(builder.HtmlFooterProvider);
        }

        /// <summary>
        /// Defines dynamic footers of the pages.
        /// </summary>
        /// <param name="inlineFooterProviderBuilder">Defines dynamic footers of the pages.</param>
        public void InlineFooter(Action<InlineFooterProviderBuilder> inlineFooterProviderBuilder)
        {
            var builder = new InlineFooterProviderBuilder();
            inlineFooterProviderBuilder(builder);
            _pdfReport.DataBuilder.SetFooter(builder.InlineFooterProvider);
        }

        /// <summary>
        /// Defines custom footer of the each page.
        /// </summary>
        /// <param name="footer">a custom footer.</param>
        public void CustomFooter(IPageFooter footer)
        {
            _pdfReport.DataBuilder.SetFooter(footer);
        }
    }
}
