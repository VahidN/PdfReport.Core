
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// A set of a predefined aggregate functions.
    /// It only works with numbers. If you want to apply it on other data types, you need to create your own AggregateFunction by implementing the IAggregateFunc.
    /// </summary>
    public enum AggregateFunction
    {
        /// <summary>
        /// Average function.
        /// </summary>
        Average,

        /// <summary>
        /// Maximum function.
        /// </summary>
        Maximum,

        /// <summary>
        /// Minimum function.
        /// </summary>
        Minimum,

        /// <summary>
        /// Standard deviation function.
        /// </summary>
        StdDev,

        /// <summary>
        /// Sum function.
        /// </summary>
        Sum,

        /// <summary>
        /// Variance function.
        /// </summary>
        Variance,

        /// <summary>
        /// Prints empty cell.
        /// </summary>
        Empty
    }
}
