using System;
using System.Collections.Generic;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// If you don't set PdfColumnsDefinitions, list of the main table's columns will be extracted from MainTableDataSource automatically.
    /// Here you can control how cells should be rendered based on their specific data types.
    /// </summary>
    public class AdHocColumnsConventions
    {
        /// <summary>
        /// Sets the visibility of auto generated row # column.
        /// </summary>
        public bool ShowRowNumberColumn { set; get; }

        /// <summary>
        /// Sets the caption of auto generated row # column.
        /// </summary>
        public string RowNumberColumnCaption { set; get; }

        /// <summary>
        /// Here you can control how cells should be rendered based on their specific data types.
        /// </summary>
        public IDictionary<Type, Func<object, string>> TypesDisplayFormatFormulas { set; get; }

        /// <summary>
        /// Here you can control how cells should be rendered based on their column names.
        /// ColumnNamesDisplayFormatFormulas has higher priority.
        /// </summary>
        public IDictionary<string, Func<object, string>> ColumnNamesDisplayFormatFormulas { set; get; }

        /// <summary>
        /// Custom template of the in use property, controls how and what should be rendered in each cell of this column.
        /// It can be null.
        /// If you don't set it, new DisplayAsText() template will be used automatically.
        /// ColumnNamesItemsTemplates has higher priority.
        /// </summary>
        public IDictionary<Type, IColumnItemsTemplate> TypesColumnItemsTemplates { set; get; }

        /// <summary>
        /// Custom template of the in use property, controls how and what should be rendered based on their column names in each cell of this column.
        /// It can be null.
        /// If you don't set it, new DisplayAsText() template will be used automatically.
        /// </summary>
        public IDictionary<string, IColumnItemsTemplate> ColumnNamesItemsTemplates { set; get; }

        /// <summary>
        /// Here you can assign an AggregateFunction to the specific data type.
        /// ColumnNamesAggregateFunctions has higher priority.
        /// </summary>
        public IDictionary<Type, IAggregateFunction> TypesAggregateFunctions { set; get; }

        /// <summary>
        /// Here you can assign an AggregateFunction to the specific column name.
        /// </summary>
        public IDictionary<string, IAggregateFunction> ColumnNamesAggregateFunctions { set; get; }
    }
}
