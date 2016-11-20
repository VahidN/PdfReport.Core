using System;
using System.Collections.Generic;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Conventions Builder Class.
    /// </summary>
    public class MainTableAdHocColumnsConventionsBuilder
    {
        private readonly AdHocColumnsConventions _builder = new AdHocColumnsConventions();

        /// <summary>
        /// Gets the AdHoc Columns Conventions
        /// </summary>
        internal AdHocColumnsConventions PdfRptAdHocColumnsConventions
        {
            get { return _builder; }
        }

        /// <summary>
        /// Sets the visibility of auto generated row # column.
        /// </summary>
        public void ShowRowNumberColumn(bool show)
        {
            _builder.ShowRowNumberColumn = show;
        }

        /// <summary>
        /// Sets the caption of auto generated row # column.
        /// </summary>
        public void RowNumberColumnCaption(string caption)
        {
            _builder.RowNumberColumnCaption = caption;
        }

        /// <summary>
        /// Here you can control how cells should be rendered based on their specific data types.
        /// </summary>
        public void AddTypeDisplayFormatFormula(Type type, Func<object, string> func)
        {
            if (_builder.TypesDisplayFormatFormulas == null)
                _builder.TypesDisplayFormatFormulas = new Dictionary<Type, Func<object, string>>();

            _builder.TypesDisplayFormatFormulas.Add(type, func);
        }

        /// <summary>
        /// Here you can control how cells should be rendered based on their column names.
        /// ColumnNamesDisplayFormatFormulas has higher priority.
        /// </summary>
        public void AddColumnDisplayFormatFormula(string name, Func<object, string> func)
        {
            if (_builder.ColumnNamesDisplayFormatFormulas == null)
                _builder.ColumnNamesDisplayFormatFormulas = new Dictionary<string, Func<object, string>>();

            _builder.ColumnNamesDisplayFormatFormulas.Add(name, func);
        }

        /// <summary>
        /// Custom template of the in use property, controls how and what should be rendered in each cell of this column.
        /// It can be null.
        /// If you don't set it, new DisplayAsText() template will be used automatically.
        /// ColumnNamesItemsTemplates has higher priority.
        /// </summary>
        public void AddTypeColumnItemsTemplate(Type type, IColumnItemsTemplate template)
        {
            if (_builder.TypesColumnItemsTemplates == null)
                _builder.TypesColumnItemsTemplates = new Dictionary<Type, IColumnItemsTemplate>();

            _builder.TypesColumnItemsTemplates.Add(type, template);
        }

        /// <summary>
        /// Custom template of the in use property, controls how and what should be rendered based on their column names in each cell of this column.
        /// It can be null.
        /// If you don't set it, new DisplayAsText() template will be used automatically.
        /// </summary>
        public void AddColumnItemsTemplate(string name, IColumnItemsTemplate template)
        {
            if (_builder.ColumnNamesItemsTemplates == null)
                _builder.ColumnNamesItemsTemplates = new Dictionary<string, IColumnItemsTemplate>();

            _builder.ColumnNamesItemsTemplates.Add(name, template);
        }

        /// <summary>
        /// Here you can assign an AggregateFunction to the specific data type.
        /// ColumnNamesAggregateFunctions has higher priority.
        /// </summary>
        public void AddTypeAggregateFunction(Type type, IAggregateFunction func)
        {
            if (_builder.TypesAggregateFunctions == null)
                _builder.TypesAggregateFunctions = new Dictionary<Type, IAggregateFunction>();

            _builder.TypesAggregateFunctions.Add(type, func);
        }

        /// <summary>
        /// Here you can assign an AggregateFunction to the specific column name.
        /// </summary>
        public void AddColumnAggregateFunction(string name, IAggregateFunction func)
        {
            if (_builder.ColumnNamesAggregateFunctions == null)
                _builder.ColumnNamesAggregateFunctions = new Dictionary<string, IAggregateFunction>();

            _builder.ColumnNamesAggregateFunctions.Add(name, func);
        }
    }
}
