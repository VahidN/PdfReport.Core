using System;
using PdfRpt.Core.Contracts;
using PdfRpt.HeaderTemplates;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Defines dynamic headers for pages and individual groups by using iTextSharp's limited HTML to PDF capabilities (HTMLWorker class).
    /// </summary>
    public class HtmlHeaderProviderBuilder
    {
        private readonly HtmlHeaderProvider _builder = new HtmlHeaderProvider();

        internal HtmlHeaderProvider HtmlHeaderProvider
        {
            get { return _builder; }
        }

        /// <summary>
        /// Properties of pages headerds.
        /// </summary>
        public void PageHeaderProperties(HeaderBasicProperties data)
        {
            _builder.PageHeaderProperties = data;
        }

        /// <summary>
        /// Properties of groups headerds.
        /// </summary>
        public void GroupHeaderProperties(HeaderBasicProperties data)
        {
            _builder.GroupHeaderProperties = data;
        }

        /// <summary>
        /// Returns dynamic HTML content of the group header.
        /// </summary>
        public void AddGroupHeader(Func<HeaderData, string> header)
        {
            _builder.AddGroupHeader = header;
        }

        /// <summary>
        /// Returns dynamic HTML content of the page header.
        /// </summary>
        public void AddPageHeader(Func<HeaderData, string> header)
        {
            _builder.AddPageHeader = header;
        }
    }
}
