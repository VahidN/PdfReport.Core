
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Determines the WidthType of the column.
    /// </summary>
    public enum TableColumnWidthType
    {
        /// <summary>
        /// Each column has a relative width equal to 1.
        /// Example: Relative values = 2, 1, 1. This means that you want to divide the width of the table into four parts (2 + 1 + 1):
        /// two parts for the first column, one part for columns two and three.
        /// </summary>
        Relative = 0,

        /// <summary>
        /// The absolute width expressed in user space units.
        /// </summary>
        Absolute = 1,

        /// <summary>
        /// Tries to resize the columns automatically.
        /// All of the specified widths will be ignored.
        /// </summary>
        FitToContent = 2,

        /// <summary>
        /// Equally sized columns.
        /// All of the specified widths will be ignored.
        /// </summary>
        EquallySized = 3
    }
}
