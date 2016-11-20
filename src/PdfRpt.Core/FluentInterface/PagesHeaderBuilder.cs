using System;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Pages Header Builder Class.
    /// </summary>
    public class PagesHeaderBuilder
    {
        readonly PdfReport _pdfReport;
        private IPdfFont _font;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public PagesHeaderBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
            _font = new GenericFontProvider(_pdfReport.DataBuilder.PdfFont);
        }

        /// <summary>
        /// You can define different headers for each page.
        /// If all of the headers of the document's pages are the same, set this value to true, to optimize the performance and document size.
        /// Its default value is true.
        /// </summary>
        /// <param name="cache">true or false</param>
        public void CacheHeader(bool cache = true)
        {
            _pdfReport.DataBuilder.CacheHeader(cache);
        }

        /// <summary>
        /// Using the predefined DefaultHeaderProvider class.
        /// </summary>
        /// <param name="defaultHeaderProviderBuilder">Header Provider Builder</param>
        public void DefaultHeader(Action<DefaultHeaderProviderBuilder> defaultHeaderProviderBuilder)
        {
            var builder = new DefaultHeaderProviderBuilder(_pdfReport);
            defaultHeaderProviderBuilder(builder);
            _pdfReport.DataBuilder.SetHeader(builder.DefaultHeaderProvider);
        }

        /// <summary>
        /// Using the predefined HtmlHeaderProvider class.
        /// </summary>
        /// <param name="htmlHeaderProviderBuilder">Header Provider Builder</param>
        public void HtmlHeader(Action<HtmlHeaderProviderBuilder> htmlHeaderProviderBuilder)
        {
            var builder = new HtmlHeaderProviderBuilder();
            htmlHeaderProviderBuilder(builder);
            _pdfReport.DataBuilder.SetHeader(builder.HtmlHeaderProvider);
        }

        /// <summary>
        /// Defines dynamic headers of the pages and individual groups.
        /// </summary>
        /// <param name="inlineHeaderProviderBuilder">Defines dynamic headers of the pages and individual groups.</param>
        public void InlineHeader(Action<InlineHeaderProviderBuilder> inlineHeaderProviderBuilder)
        {
            var builder = new InlineHeaderProviderBuilder();
            inlineHeaderProviderBuilder(builder);
            _pdfReport.DataBuilder.SetHeader(builder.InlineHeaderProvider);
        }

        /// <summary>
        /// Gets/Sets the default fonts of the header.
        /// </summary>
        public IPdfFont PdfFont
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// Defines dynamic headers for pages and individual groups.
        /// </summary>
        /// <param name="header">a dynamic header</param>
        public void CustomHeader(IPageHeader header)
        {
            _pdfReport.DataBuilder.SetHeader(header);
        }
    }
}
