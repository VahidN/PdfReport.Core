using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Custom Rows Builder Class.
    /// </summary>
    public class CustomRowsBuilder
    {
        readonly IList<CellData> _rowCells = new List<CellData>();

        /// <summary>
        /// List of the cells of the current row.
        /// </summary>
        internal IList<CellData> RowCells
        {
            get { return _rowCells; }
        }

        /// <summary>
        /// Available data
        /// </summary>
        public EventsArguments EventsArgs { set; get; }

        /// <summary>
        /// Adds a new cell to the current row
        /// </summary>
        /// <param name="propertyName">Property name of the current cell</param>
        /// <param name="propertyValue">Property value of the current cell</param>
        /// <param name="formattedValue">Formatted Property value of the current cell</param>
        public void AddCell(string propertyName, object propertyValue, string formattedValue = null)
        {
            _rowCells.Add(new CellData
                {
                    FormattedValue = formattedValue,
                    PropertyName = propertyName,
                    PropertyValue = propertyValue
                });
        }

        /// <summary>
        /// Adds a new cell to the current row
        /// </summary>
        /// <param name="propertyName">Property name of the current cell</param>
        /// <param name="propertyValue">Property value of the current cell</param>
        /// <param name="formattedValue">Formatted Property value of the current cell</param>
        public void AddCell<TEntity>(Expression<Func<TEntity, object>> propertyName, object propertyValue, string formattedValue = null)
        {
            _rowCells.Add(new CellData
            {
                FormattedValue = formattedValue,
                PropertyName = PropertyHelper.Name(propertyName),
                PropertyValue = propertyValue
            });
        }
    }
}
