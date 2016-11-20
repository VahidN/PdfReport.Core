namespace PdfRpt.Core.Contracts
{    
    /// <summary>
    /// New document's margins
    /// </summary>
    public class DocumentMargins
    {
        /// <summary>
        /// The margin on the left
        /// </summary>
        public float Left { set; get; }

        /// <summary>
        /// The margin on the right
        /// </summary>
        public float Right { set; get; }

        /// <summary>
        /// The margin on the top
        /// </summary>
        public float Top { set; get; }

        /// <summary>
        /// The margin on the bottom
        /// </summary>
        public float Bottom { set; get; }
    }
}
