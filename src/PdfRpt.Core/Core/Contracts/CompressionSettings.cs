
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Compression settings
    /// </summary>
    public class CompressionSettings
    {
        /// <summary>
        /// A possible compression level.
        /// </summary>
        public CompressionLevel CompressionLevel { set; get; }

        /// <summary>
        /// Applies Full Compression. Content streams will be compressed, but so will some other objects, 
        /// such as the cross-reference table. This is only possible since PDF version 1.5.
        /// </summary>
        public bool EnableFullCompression { set; get; }


        /// <summary>
        /// Applies Normal Compression
        /// </summary>
        public bool EnableCompression { set; get; }
    }
}
