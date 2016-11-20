using iTextSharp.text;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Pages Preferences
    /// </summary>
    public class PagePreferences
    {
        /// <summary>
        /// Sets Margins of the PDF Document.
        /// </summary>
        public DocumentMargins Margins { get; set; }

        /// <summary>
        /// RunDirection sets the special setting of RTL or LTR languages automatically.
        /// </summary>
        public PdfRunDirection? RunDirection { get; set; }

        /// <summary>
        /// The paper size.
        /// The iTextSharp.text.PageSize class contains a number of read only rectangles representing the most common paper sizes,
        /// such as PageSize.A4, PageSize.LETTER, etc.
        /// </summary>
        public Rectangle Size { get; set; }

        /// <summary>
        /// Indicates pages ordination, portrait or landscape
        /// </summary>
        public PageOrientation Orientation { get; set; }

        /// <summary>
        /// Background image's file path.
        /// Leave it as null or empty if you don't want to use it.
        /// AlternatingRowBackgroundColor and RowBackgroundColor of RptTemplate should be set to null to make this image visible.
        /// </summary>
        public string BackgroundImageFilePath { set; get; }

        /// <summary>
        /// Gets or sets the absolute position of the Background image.
        /// If it's set to null, the image will be painted at the center of the page.
        /// </summary>
        public System.Drawing.PointF? BackgroundImagePosition { set; get; }

        /// <summary>
        /// Setting Page Background Color.
        /// It can be null.
        /// </summary>
        public System.Drawing.Color? PagesBackgroundColor { set; get; }

        /// <summary>
        /// You can define different headers for each page. 
        /// If all of the headers of the document's pages are the same, set this value to true, to optimize the performance and document size.
        /// </summary>
        public bool CacheHeader { set; get; }
    }
}
