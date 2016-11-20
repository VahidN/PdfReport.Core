using System;
using System.Collections.Generic;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    public class ColumnAttributes
    {
        /// <summary>
        /// ctor.
        /// </summary>
        public ColumnAttributes()
        {
            PropertyIndex = -1;
        }

        /// <summary>
        /// Creates an auto generated row # column starting from 1
        /// </summary>
        public bool IsRowNumber { set; get; }

        /// <summary>
        /// Field name of the current column's cells
        /// If it's a RowNumber or CalculatedField, it can be an arbitrary string
        /// </summary>
        public string PropertyName { set; get; }

        /// <summary>
        /// Index of the property in data source columns.
        /// If you are not specifying the duplicate property names, ignore it.
        /// </summary>
        public int PropertyIndex { set; get; }

        /// <summary>
        /// The column's width according to the PdfRptPageSetup.MainTableColumnsWidthsType value.
        /// </summary>
        public float Width { set; get; }

        /// <summary>
        /// Height of each row will be calculated automatically based on its content. 
        /// Also you can set the FixedHeight to define the height yourself.
        /// In this case the overflowed text with be trimmed. 
        /// Set FixedHeight to zero (its default value) to ignore this setting.
        /// </summary>
        public float FixedHeight { set; get; }

        /// <summary>
        /// Height of each row will be calculated automatically based on its content. 
        /// To assure a certain cell height, without losing any content, you can set the MinimumHeight.
        /// Set MinimumHeight to zero (its default value) to ignore this setting.
        /// </summary>
        public float MinimumHeight { set; get; }

        /// <summary>
        /// Determines exclusion or visibility of this column.
        /// If GroupsPreferences.GroupType is set to PdfRptGroupType.IncludeGroupingColumns, this property will be ignored.
        /// </summary>
        public bool IsVisible { set; get; }

        /// <summary>
        /// Column's padding value
        /// </summary>
        public float Padding { set; get; }

        /// <summary>
        /// Column's PaddingBottom value
        /// </summary>
        public float PaddingBottom { get; set; }

        /// <summary>
        /// Column's PaddingLeft value
        /// </summary>
        public float PaddingLeft { get; set; }

        /// <summary>
        /// Column's PaddingRight value
        /// </summary>
        public float PaddingRight { get; set; }

        /// <summary>
        /// Column's PaddingTop value
        /// </summary>
        public float PaddingTop { get; set; }

        /// <summary>
        /// Column's order 
        /// </summary>
        public int Order { set; get; }

        /// <summary>
        /// Column's header cell.
        /// It can not be null.
        /// </summary>
        public HeadingCell HeaderCell { set; get; }

        /// <summary>
        /// Places the optional heading cell(s) above the current column's HeaderCell.
        /// It can be null.
        /// </summary>
        public IList<HeadingCell> HeadingCells { set; get; }

        /// <summary>
        /// Custom template of the in use property, controls how and what should be rendered in each cell of this column.
        /// It can be null.
        /// If you don't set it, new DisplayAsText() template will be used automatically.
        /// </summary>
        public IColumnItemsTemplate ColumnItemsTemplate { set; get; }

        /// <summary>
        /// Content's Horizontal alignment
        /// </summary>
        public HorizontalAlignment? CellsHorizontalAlignment { set; get; }

        /// <summary>
        /// By setting IsCalculatedField to true, you will be able to define a completely new column which is not exist in the main table's DataSource.
        /// CalculatedFieldFormula fires before a CalculatedField is being rendered.
        /// Now you can calculate the current cell's value, based on the other cells values. 
        /// Here IList contains actual row's cells values.
        /// It can be null.
        /// </summary>
        public Func<IList<CellData>, object> CalculatedFieldFormula { set; get; }

        /// <summary>
        /// It enables using CalculatedFieldFormula to produce a new column based on the existing values of other columns according to the provided formula.
        /// </summary>
        public bool IsCalculatedField { set; get; }

        /// <summary>
        /// Custom Aggregate Function
        /// It can be null.
        /// </summary>
        public IAggregateFunction AggregateFunction { set; get; }

        /// <summary>
        /// Defining a group of rows by including this filed in grouping
        /// </summary>
        public bool IncludeInGrouping { set; get; }

        /// <summary>
        /// Custom comparison implementation of current and last field values of this column, to detect start of a new group
        /// It can be null.
        /// </summary>
        public Func<object, object, bool> IncludedGroupFieldEqualityComparer { set; get; }
    }
}