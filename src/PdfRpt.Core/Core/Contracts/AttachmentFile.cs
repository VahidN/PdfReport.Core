
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Attachment File Info.
    /// </summary>
    public class AttachmentFile
    {
        /// <summary>
        /// Attachment File's Content.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Attachment File's Name.
        /// </summary>
        public string FileName { get; set; }
    }
}
