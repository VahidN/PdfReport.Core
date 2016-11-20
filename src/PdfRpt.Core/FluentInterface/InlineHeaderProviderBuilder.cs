using System;
using PdfRpt.Core.Contracts;
using PdfRpt.HeaderTemplates;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Defines dynamic headers for pages and individual groups.
    /// </summary>
    public class InlineHeaderProviderBuilder
    {
        private readonly InlineHeaderProvider _builder = new InlineHeaderProvider();

        internal InlineHeaderProvider InlineHeaderProvider
        {
            get { return _builder; }
        }

        /// <summary>
        /// Returns dynamic content of the group header.
        /// </summary>
        public void AddGroupHeader(Func<HeaderData, PdfGrid> header)
        {
            _builder.AddGroupHeader = header;
        }

        /// <summary>
        /// Returns dynamic content of the page header.
        /// </summary>
        public void AddPageHeader(Func<HeaderData, PdfGrid> header)
        {
            _builder.AddPageHeader = header;
        }
    }
}
