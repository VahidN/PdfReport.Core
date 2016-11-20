using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Template Builder Class.
    /// </summary>
    public class MainTableTemplateBuilder
    {
        readonly PdfReport _pdfReport;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public MainTableTemplateBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
        }

        /// <summary>
        /// Sets a predefined template.
        /// </summary>
        /// <param name="template">selected template</param>
        public void BasicTemplate(BasicTemplate template)
        {
            _pdfReport.DataBuilder.DefaultBasicTemplate(template);
        }

        /// <summary>
        /// Sets the default template. 
        /// It can be null. In this case a new BasicTemplateProvider based on the DefaultBasicTemplate will be used automatically.
        /// </summary>
        /// <param name="template">custom template</param>
        public void CustomTemplate(ITableTemplate template)
        {
            _pdfReport.DataBuilder.Template = template;
        }
    }
}
