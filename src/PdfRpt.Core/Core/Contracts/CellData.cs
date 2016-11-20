using System;
using System.Diagnostics;
using System.Linq.Expressions;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// A class to hold data of the main table's cells
    /// </summary>    
    [DebuggerDisplay("{PropertyIndex} - {PropertyName} - {PropertyValue} - {FormattedValue}")]
    public class CellData
    {
        /// <summary>
        /// Property index of the current cell
        /// </summary>
        public int PropertyIndex { set; get; }

        /// <summary>
        /// Property name of the current cell
        /// </summary>
        public string PropertyName { set; get; }

        /// <summary>
        /// Property value of the current cell
        /// </summary>
        public object PropertyValue { set; get; }

        /// <summary>
        /// Type of the property.
        /// </summary>
        public Type PropertyType { set; get; }

        /// <summary>
        /// Formatted Property value of the current cell
        /// </summary>
        public string FormattedValue { set; get; }

        /// <summary>
        /// Determines whether PropertyName of the this instance and another specified object have the same string value.             
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity (interface or class).</typeparam>
        /// <param name="expression">The expression returning the entity property, in the form x =&gt; x.Id</param>
        /// <returns>true or false</returns>
        public bool PropertyNameEquals<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return PropertyName.Equals(PropertyHelper.Name(expression));
        }
    }
}
