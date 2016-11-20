using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// determine the predominant order of the pages.
    /// </summary>
    public enum PagesDirection
    {
        /// <summary>
        /// Left to right (the default).
        /// </summary>
        DirectionL2R = PdfWriter.DirectionL2R,

        /// <summary>
        /// Right to left, including vertical writing systems such as Chinese, Japanese, and Korean.
        /// </summary>
        DirectionR2L = PdfWriter.DirectionR2L
    }
}
