using System;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IsCalculatedFieldAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="isCalculated">It enables using CalculatedFieldFormula to produce a new column based on the existing values of other columns according to the provided formula.</param>
        public IsCalculatedFieldAttribute(bool isCalculated)
        {
            IsCalculated = isCalculated;
        }

        /// <summary>
        /// It enables using CalculatedFieldFormula to produce a new column based on the existing values of other columns according to the provided formula.
        /// </summary>
        public bool IsCalculated { private set; get; }
    }
}