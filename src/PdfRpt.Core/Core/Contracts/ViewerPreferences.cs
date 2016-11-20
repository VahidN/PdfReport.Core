using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// select toolbar items that must be shown or hidden. these values can be or'ed.
    /// </summary>    
    public enum ViewerPreferences
    {
        /// <summary>
        /// Not set value
        /// </summary>
        UseNone = -10,

        /// <summary>
        /// Hides the toolbar when the document is opened.
        /// </summary>
        HideToolBar = PdfWriter.HideToolbar,

        /// <summary>
        /// Hides the menu bar when the document is opened.
        /// </summary>
        HideMenubar = PdfWriter.HideMenubar,

        /// <summary>
        /// Hides UI elements in the document’s window (such as scrollbars and navigation controls), 
        /// leaving only the document’s contents displayed.
        /// </summary>
        HideWindowUI = PdfWriter.HideWindowUI,

        /// <summary>
        /// Resizes the document’s window to fit the size of the first displayed page.
        /// </summary>
        FitWindow = PdfWriter.FitWindow,

        /// <summary>
        /// Puts the document’s window in the center of the screen.
        /// </summary>
        CenterWindow = PdfWriter.CenterWindow,

        /// <summary>
        /// Displays the title that was added in the metadata in the top bar (otherwise the filename is displayed).
        /// </summary>
        DisplayDocTitle = PdfWriter.DisplayDocTitle
    }
}
