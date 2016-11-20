using System;
using PdfRpt.Core.Contracts;
using PdfRpt.FooterTemplates;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Defines dynamic footer of the pages.
    /// </summary>
    public class InlineFooterProviderBuilder
    {
        private readonly InlineFooterProvider _builder = new InlineFooterProvider();

        internal InlineFooterProvider InlineFooterProvider
        {
            get { return _builder; }
        }

        /// <summary>
        /// Properties of page footers.
        /// </summary>
        public void FooterProperties(FooterBasicProperties properties)
        {
            _builder.FooterProperties = properties;
        }

        /// <summary>
        /// Returns dynamic content of the page footer.
        /// </summary>
        public void AddPageFooter(Func<FooterData, PdfGrid> footer)
        {
            _builder.AddPageFooter = footer;
        }
    }
}