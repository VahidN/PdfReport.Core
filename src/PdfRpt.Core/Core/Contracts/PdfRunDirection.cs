
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Possible run direction values, left-to-right or right-to-left
    /// </summary>
    public enum PdfRunDirection
    {
        /// <summary>
        /// Undefined value.
        /// </summary>
        None = -1,

        /// <summary>
        /// Use the default run direction.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Use bidirectional reordering with left-to-right preferential run direction.
        /// </summary>
        LeftToRight = 2,

        /// <summary>
        /// Do not use bidirectional reordering.
        /// </summary>
        NoBidi = 1,

        /// <summary>
        /// Use bidirectional reordering with right-to-left preferential run direction.
        /// </summary>
        RightToLeft = 3
    }
}