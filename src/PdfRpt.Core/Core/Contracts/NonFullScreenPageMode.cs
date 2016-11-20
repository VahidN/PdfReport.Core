using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// specifies how to display the document on exiting full-screen mode.
    /// These options only make sense if the page mode is full screen.
    /// </summary>
    public enum NonFullScreenPageMode
    {
        /// <summary>
        /// None of the tabs at the left are visible.
        /// </summary>
        UseNone = PdfWriter.NonFullScreenPageModeUseNone,

        /// <summary>
        /// The document outline is visible.
        /// </summary>
        UseOutlines = PdfWriter.NonFullScreenPageModeUseOutlines,

        /// <summary>
        /// Thumbnail images corresponding with the pages are visible.
        /// </summary>
        UseThumbs = PdfWriter.NonFullScreenPageModeUseThumbs,

        /// <summary>
        /// The optional content group panel is visible.
        /// </summary>
        UseOC = PdfWriter.NonFullScreenPageModeUseOC
    }
}
