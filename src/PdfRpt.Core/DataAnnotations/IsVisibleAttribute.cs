using System;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IsVisibleAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="isVisible">Determines exclusion or visibility of this column.</param>
        public IsVisibleAttribute(bool isVisible)
        {
            IsVisible = isVisible;
        }

        /// <summary>
        /// Defining a group of rows by including this filed in grouping.
        /// </summary>
        public bool IsVisible { private set; get; }
    }
}