using System;
using PdfRpt.Core.Contracts;
using PdfRpt.FooterTemplates;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Defines dynamic footer of the pages by using iTextSharp's limited HTML to PDF capabilities (HTMLWorker class).
    /// </summary>
    public class HtmlFooterProviderBuilder
    {
        private readonly HtmlFooterProvider _builder = new HtmlFooterProvider();

        internal HtmlFooterProvider HtmlFooterProvider
        {
            get { return _builder; }
        }

        /// <summary>
        /// Properties of page footers.
        /// </summary>
        public void PageFooterProperties(FooterBasicProperties properties)
        {
            _builder.FooterProperties = properties;
        }

        /// <summary>
        /// Returns dynamic HTML content of the page footer.
        /// </summary>
        public void AddPageFooter(Func<FooterData, string> footer)
        {
            _builder.AddPageFooter = footer;
        }
    }
}
