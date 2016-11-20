using System;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IncludeInGroupingAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="includeInGrouping">Defining a group of rows by including this filed in grouping.</param>
        public IncludeInGroupingAttribute(bool includeInGrouping)
        {
            IncludeInGrouping = includeInGrouping;
        }

        /// <summary>
        /// Defining a group of rows by including this filed in grouping.
        /// </summary>
        public bool IncludeInGrouping { private set; get; }
    }
}