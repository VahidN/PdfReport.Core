using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// possible compression levels
    /// </summary>
    public enum CompressionLevel
    {
        /// <summary>
        /// A possible compression level.
        /// </summary>
        Default =  PdfStream.DEFAULT_COMPRESSION,

        /// <summary>
        /// A possible compression level.
        /// </summary>
        NoCompression =  PdfStream.NO_COMPRESSION,

        /// <summary>
        /// A possible compression level.
        /// </summary>
        BestSpeed = PdfStream.BEST_SPEED,

        /// <summary>
        /// A possible compression level.
        /// </summary>
        BestCompression = PdfStream.BEST_COMPRESSION
    }
}
