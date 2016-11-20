using System;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="order">Column's order.</param>
        public OrderAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// Column's order.
        /// </summary>
        public int Order { private set; get; }
    }
}