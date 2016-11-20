using iTextSharp.text;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Holds VisibleSignature's info
    /// </summary>
    public class VisibleSignature
    {
        /// <summary>
        /// Sets the signature text identifying the signer.
        /// </summary>
        public string CustomText { get; set; }

        /// <summary>
        /// Position and dimension of the field in the page.
        /// </summary>
        public Rectangle Position { get; set; }

        /// <summary>
        /// The page to place the field. The fist page is 1.
        /// </summary>
        public int PageNumberToShowSignature { get; set; }

        /// <summary>
        /// If it sets to true, value of the Page property will be ignored.
        /// </summary>
        public bool UseLastPageToShowSignature { set; get; }

        /// <summary>
        /// Sets the background image for the layer 2.
        /// It can be null or empty.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Possible run direction values, left-to-right or right-to-left
        /// </summary>
        public PdfRunDirection? RunDirection { set; get; }

        /// <summary>
        /// Custom font's definitions
        /// </summary>
        public IPdfFont Font { set; get; }
    }
}
