using System;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class WidthAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="width">The column's width according to the PdfRptPageSetup.MainTableColumnsWidthsType value.</param>
        public WidthAttribute(float width)
        {
            Width = width;
        }

        /// <summary>
        /// The column's width according to the PdfRptPageSetup.MainTableColumnsWidthsType value.
        /// </summary>
        public float Width { private set; get; }
    }
}