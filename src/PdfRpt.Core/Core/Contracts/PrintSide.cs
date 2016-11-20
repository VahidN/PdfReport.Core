
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Indicates whether duplex printing should be used or not
    /// </summary>
    public enum PrintSide
    {
        /// <summary>
        /// print single-sided.
        /// </summary>
        Simplex = 0,

        /// <summary>
        /// duplex printing, flip on the short edge of the sheet.
        /// </summary>
        DuplexFlipShortEdge = 1,

        /// <summary>
        /// duplex printing, flip on the long edge of the sheet
        /// </summary>
        DuplexFlipLongEdge = 2
    }
}
