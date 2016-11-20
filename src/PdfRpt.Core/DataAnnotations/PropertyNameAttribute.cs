using System;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class PropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="propertyName">Field name of the current column's cells.</param>
        public PropertyNameAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Field name of the current column's cells.
        /// If it's a RowNumber or CalculatedField, it can be an arbitrary string
        /// </summary>
        public string PropertyName { private set; get; }
    }
}