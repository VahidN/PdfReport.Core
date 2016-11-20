using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// PageMode values. Sets different panels available in Adobe Reader.
    /// </summary>
    public enum ViewerPageMode
    {
        /// <summary>
        /// None of the tabs on the left are selected (this is the default).
        /// </summary>
        UseNone = PdfWriter.PageModeUseNone,

        /// <summary>
        /// The document outline is visible (bookmarks).
        /// </summary>
        UseOutlines = PdfWriter.PageModeUseOutlines,

        /// <summary>
        /// Images corresponding with the page are visible.
        /// </summary>
        UseThumbs = PdfWriter.PageModeUseThumbs,

        /// <summary>
        /// Full-screen mode; no menu bar, window controls, or any other windows are visible.
        /// </summary>
        FullScreen = PdfWriter.PageModeFullScreen,

        /// <summary>
        /// The optional content group panel is visible (since PDF 1.5).
        /// </summary>
        UseOC = PdfWriter.PageModeUseOC,

        /// <summary>
        /// The attachments panel is visible (since PDF 1.6).
        /// </summary>
        UseAttachments = PdfWriter.PageModeUseAttachments
    }
}
