using System;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CalculatedFieldFormulaAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="propertyName">Name of the corresponding column's property.</param>
        public CalculatedFieldFormulaAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Name of the corresponding column's property.
        /// </summary>
        public string PropertyName { private set; get; }
    }
}