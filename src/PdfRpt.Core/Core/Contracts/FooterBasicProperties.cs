
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Properties of pages footers
    /// </summary>
    public class FooterBasicProperties : HeaderBasicProperties
    {
        /// <summary>
        /// Height of the template.
        /// </summary>
        public float TotalPagesCountTemplateHeight { set; get; }

        /// <summary>
        /// Width of the template.
        /// </summary>
        public float TotalPagesCountTemplateWidth { set; get; }

        /// <summary>
        /// Properties of pages footers
        /// </summary>
        public FooterBasicProperties()
        {
            SpacingBeforeTable = 7f;
            TotalPagesCountTemplateHeight = 10;
            TotalPagesCountTemplateWidth = 50;
        }
    }
}
