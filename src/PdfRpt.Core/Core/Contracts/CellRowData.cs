using System;
using System.Collections.Generic;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// PdfCell's Raw Data
    /// </summary>
    public class CellRowData
    {
        /// <summary>
        /// Contains actual row's fields values from TableDataSource and CalculatedFieldFormula.
        /// </summary>
        public IList<CellData> TableRowData { set; get; }

        /// <summary>
        /// Cell's row type value.
        /// </summary>
        public RowType PdfRowType { set; get; }

        /// <summary>
        /// Cell's raw data value.
        /// </summary>
        public object Value { set; get; }

        /// <summary>
        /// Formatted Property value of the current cell.
        /// </summary>
        public string FormattedValue { set; get; }

        /// <summary>
        /// Cell's index.
        /// </summary>
        public int ColumnNumber { set; get; }

        /// <summary>
        /// Last rendered row number which can be different in multiple groups
        /// </summary>
        public int LastRenderedRowNumber { set; get; }

        /// <summary>
        /// Field name of the current column's cells
        /// If it's a RowNumber or CalculatedField, it can be an arbitrary string
        /// </summary>
        public string PropertyName { set; get; }

        /// <summary>
        /// Type of the  Property.
        /// </summary>
        public Type PropertyType { set; get; }

        /// <summary>
        /// Current cell's template.
        /// </summary>
        public IColumnItemsTemplate CellTemplate { set; get; }
    }
}
