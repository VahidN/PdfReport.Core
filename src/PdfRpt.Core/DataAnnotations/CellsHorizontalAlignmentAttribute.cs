using System;
using PdfRpt.Core.Contracts;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CellsHorizontalAlignmentAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="cellsHorizontalAlignment">Content's Horizontal alignment.</param>
        public CellsHorizontalAlignmentAttribute(HorizontalAlignment cellsHorizontalAlignment)
        {
            CellsHorizontalAlignment = cellsHorizontalAlignment;
        }

        /// <summary>
        /// Content's Horizontal alignment.
        /// </summary>
        public HorizontalAlignment CellsHorizontalAlignment { private set; get; }
    }
}