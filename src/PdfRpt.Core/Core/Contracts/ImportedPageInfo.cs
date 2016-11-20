using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// To add a new content to the imported page, use the PageStamp object.
    /// </summary>
    public class ImportedPageInfo
    {
        /// <summary>
        /// Current page's number.
        /// </summary>
        public int CurrentPageNumber { set; get; }

        /// <summary>
        /// Current file's number.
        /// </summary>
        public int FileNumber { set; get; }

        /// <summary>
        /// Current imported page's data.
        /// </summary>
        public PdfImportedPage ImportedPage { set; get; }

        /// <summary>
        /// Current page's size.
        /// </summary>
        public Rectangle PageSize { set; get; }

        /// <summary>
        /// Final PdfDocument object.
        /// </summary>
        public Document PdfDocument { set; get; }

        /// <summary>
        /// Current PdfReader Object.
        /// </summary>
        public PdfReader Reader { set; get; }

        /// <summary>
        /// Allows adding the new content to a PdfImportedPage.
        /// </summary>
        public PdfCopy.PageStamp Stamp { set; get; }

        /// <summary>
        /// Total Number Of all PDF files pages.
        /// </summary>
        public int TotalNumberOfPages { set; get; }
    }
}