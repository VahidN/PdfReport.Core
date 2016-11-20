
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Specifies the Adobe Reader's settings when a document is opened.
    /// </summary>
    public class PdfViewerPreferences
    {
        /// <summary>
        /// Page layout values. Specifies the page layout to be used when a document is opened in Adobe Reader.
        /// </summary>
        public ViewerPageLayout PageLayout { set; get; }

        /// <summary>
        /// PageMode values. Sets different panels available in Adobe Reader.
        /// </summary>
        public ViewerPageMode PageMode { set; get; }

        /// <summary>
        /// specifies how to display the document on exiting full-screen mode.
        /// These options only make sense if the page mode is full screen.
        /// </summary>
        public NonFullScreenPageMode NonFullScreenPageMode { set; get; }


        /// <summary>
        /// select toolbar items that must be shown or hidden. these values can be or'ed.
        /// </summary>
        public ViewerPreferences? ViewerPreferences { set; get; }

        /// <summary>
        /// determine the predominant order of the pages.
        /// </summary>
        public PagesDirection PagesDirection { set; get; }

        /// <summary>
        /// Setting the initial zoom of the PDF document. 
        /// Only advanced PDF viewers such as Adobe Reader, understand this value.       
        /// </summary>
        public float ZoomPercent { set; get; }

        /// <summary>
        /// Sets the PdfVersion. Select at least PDF version 1.5, If you want the best compression support.
        /// </summary>
        public PdfVersion PdfVersion { set; get; }
    }
}
