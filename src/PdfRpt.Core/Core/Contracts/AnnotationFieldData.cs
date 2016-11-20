
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Defines a PdfAnnotation data.
    /// </summary>
    public class AnnotationFieldData
    {
        /// <summary>
        /// The icon that should be used.
        /// </summary>
        public AnnotationIcon Icon { set; get; }

        /// <summary>
        /// A title for the annotations.
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// The content of the text annotation.
        /// </summary>
        public string Text { set; get; }
    }
}
