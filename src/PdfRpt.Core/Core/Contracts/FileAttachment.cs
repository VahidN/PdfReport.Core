
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Adding a file attachment at the document level.
    /// </summary>
    public class FileAttachment
    {
        /// <summary>
        /// File's path.
        /// </summary>
        public string FilePath { set; get; }

        /// <summary>
        /// File's description.
        /// </summary>
        public string Description { set; get; }
    }
}
