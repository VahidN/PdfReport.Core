using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Column Builder Class.
    /// </summary>
    public class ColumnAttributesBuilder
    {
        readonly ColumnAttributes _pdfColumnAttributes;

        /// <summary>
        /// Gets the Columns Attributes
        /// </summary>
        internal ColumnAttributes PdfColumnAttributes
        {
            get { return _pdfColumnAttributes; }
        }

        /// <summary>
        /// ctor.
        /// </summary>
        public ColumnAttributesBuilder()
        {
            _pdfColumnAttributes = new ColumnAttributes();
        }

        /// <summary>
        /// Creates an auto generated row # column starting from 1
        /// </summary>
        public void IsRowNumber(bool rowNumber)
        {
            _pdfColumnAttributes.IsRowNumber = rowNumber;
        }

        /// <summary>
        /// Field name of the current column's cells.
        /// If it's a RowNumber or CalculatedField, it can be an arbitrary string.
        /// If you are not specifying the duplicate property names, ignore the index of the property in data source columns.
        /// </summary>
        public void PropertyName(string name, int index = -1)
        {
            _pdfColumnAttributes.PropertyName = name;
            _pdfColumnAttributes.PropertyIndex = index;
        }

        /// <summary>
        /// Field name of the current column's cells.
        /// If it's a RowNumber or CalculatedField, it can be an arbitrary string.
        /// If you are not specifying the duplicate property names, ignore the index of the property in data source columns.
        /// </summary>
        public void PropertyName<TEntity>(Expression<Func<TEntity, object>> expression, int index = -1)
        {
            _pdfColumnAttributes.PropertyName = PropertyHelper.Name(expression);
            _pdfColumnAttributes.PropertyIndex = index;
        }

        /// <summary>
        /// The column's width according to the PdfRptPageSetup.MainTableColumnsWidthsType value.
        /// </summary>
        public void Width(float value)
        {
            _pdfColumnAttributes.Width = value;
        }

        /// <summary>
        /// Determines exclusion or visibility of this column.
        /// If GroupsPreferences.GroupType is set to PdfRptGroupType.IncludeGroupingColumns, this property will be ignored.
        /// </summary>
        public void IsVisible(bool visible)
        {
            _pdfColumnAttributes.IsVisible = visible;
        }

        /// <summary>
        /// Column's order
        /// </summary>
        public void Order(int value)
        {
            _pdfColumnAttributes.Order = value;
        }

        /// <summary>
        /// Height of each row will be calculated automatically based on its content. 
        /// Also you can set the FixedHeight to define the height yourself.
        /// In this case the overflowed text with be trimmed. 
        /// Set FixedHeight to zero (its default value) to ignore this setting.
        /// </summary>
        public void FixedHeight(float value)
        {
            _pdfColumnAttributes.FixedHeight = value;
        }

        /// <summary>
        /// Height of each row will be calculated automatically based on its content. 
        /// To assure a certain cell height, without losing any content, you can set the MinimumHeight.
        /// Set MinimumHeight to zero (its default value) to ignore this setting.
        /// </summary>
        public void MinimumHeight(float value)
        {
            _pdfColumnAttributes.MinimumHeight = value;
        }

        /// <summary>
        /// Column's padding value
        /// </summary>
        public void Padding(float value)
        {
            _pdfColumnAttributes.Padding = value;
        }

        /// <summary>
        /// Column's PaddingBottom value
        /// </summary>
        public void PaddingBottom(float value)
        {
            _pdfColumnAttributes.PaddingBottom = value;
        }

        /// <summary>
        /// Column's PaddingLeft value
        /// </summary>
        public void PaddingLeft(float value)
        {
            _pdfColumnAttributes.PaddingLeft = value;
        }

        /// <summary>
        /// Column's PaddingRight value
        /// </summary>
        public void PaddingRight(float value)
        {
            _pdfColumnAttributes.PaddingRight = value;
        }

        /// <summary>
        /// Column's PaddingTop value
        /// </summary>
        public void PaddingTop(float value)
        {
            _pdfColumnAttributes.PaddingTop = value;
        }

        /// <summary>
        /// Column's header cell. It can not be null.
        /// </summary>
        /// <param name="caption">The string to be displayed as the current column's caption.</param>
        /// <param name="mergeHeaderCell">If true, the current header cell (and not its data column) will be merged with the next one and the next label will be ignored.</param>
        /// <param name="captionRotation">The rotation of the column's caption. Possible values are 0, 90, 180 and 270.</param>
        /// <param name="horizontalAlignment">Content's Horizontal alignment. If null, IPdfRptTemplate.HeaderHorizontalAlignment will be used.</param>
        public void HeaderCell(string caption, bool mergeHeaderCell = false, int captionRotation = 0, HorizontalAlignment? horizontalAlignment = null)
        {
            _pdfColumnAttributes.HeaderCell = new HeadingCell
            {
                Caption = caption,
                CaptionRotation = captionRotation,
                HorizontalAlignment = horizontalAlignment,
                MergeHeaderCell = mergeHeaderCell
            };
        }

        /// <summary>
        /// Places the optional heading cell(s) above the current column's HeaderCell.
        /// It can be null.
        /// </summary>
        /// <param name="caption">The string to be displayed as the current column's caption.</param>
        /// <param name="mergeHeaderCell">If true, the current header cell (and not its data column) will be merged with the next one and the next label will be ignored.</param>
        /// <param name="captionRotation">The rotation of the column's caption. Possible values are 0, 90, 180 and 270.</param>
        /// <param name="horizontalAlignment">Content's Horizontal alignment. If null, IPdfRptTemplate.HeaderHorizontalAlignment will be used.</param>         
        public void AddHeadingCell(string caption, bool mergeHeaderCell = false, int captionRotation = 0, HorizontalAlignment? horizontalAlignment = null)
        {
            if (_pdfColumnAttributes.HeadingCells == null)
                _pdfColumnAttributes.HeadingCells = new List<HeadingCell>();

            _pdfColumnAttributes.HeadingCells.Add(new HeadingCell
            {
                Caption = caption,
                CaptionRotation = captionRotation,
                HorizontalAlignment = horizontalAlignment,
                MergeHeaderCell = mergeHeaderCell
            });
        }

        /// <summary>
        /// Custom template of the in use property, controls how and what should be rendered in each cell of this column.
        /// It can be null.
        /// If you don't set it, new DisplayAsText() template will be used automatically.
        /// </summary>
        public void ColumnItemsTemplate(Action<ColumnItemsTemplateBuilder> columnItemsTemplateBuilder)
        {
            var builder = new ColumnItemsTemplateBuilder();
            columnItemsTemplateBuilder(builder);

            _pdfColumnAttributes.ColumnItemsTemplate = builder.ColumnItemsTemplate;
        }

        /// <summary>
        /// If you don't want to use the default font's settings for this column, set this optional column's font info.
        /// </summary>        
        public void Font(Action<GenericFontProviderBuilder> fontProviderBuilder)
        {
            var builder = new GenericFontProviderBuilder();
            fontProviderBuilder(builder);

            var font = builder.GenericFontProvider;

            if (_pdfColumnAttributes.ColumnItemsTemplate == null)
                _pdfColumnAttributes.ColumnItemsTemplate = new TextBlockField();

            if (_pdfColumnAttributes.ColumnItemsTemplate.BasicProperties == null)
                _pdfColumnAttributes.ColumnItemsTemplate.BasicProperties = new CellBasicProperties();

            _pdfColumnAttributes.ColumnItemsTemplate.BasicProperties.PdfFont = font;
            _pdfColumnAttributes.ColumnItemsTemplate.BasicProperties.FontColor = font.Color;
            _pdfColumnAttributes.ColumnItemsTemplate.BasicProperties.PdfFontStyle = font.Style;
        }

        /// <summary>
        /// Content's Horizontal alignment
        /// </summary>
        public void CellsHorizontalAlignment(HorizontalAlignment? alignment)
        {
            _pdfColumnAttributes.CellsHorizontalAlignment = alignment;
        }

        /// <summary>
        /// Produce a new column based on the existing values of other columns.
        /// </summary>
        /// <param name="isCalculatedField">It enables using CalculatedFieldFormula to produce a new column based on the existing values of other columns according to the provided formula.</param>
        /// <param name="calculatedFieldFormula">
        /// By setting IsCalculatedField to true, you will be able to define a completely new column which is not exist in the main table's DataSource.
        /// CalculatedFieldFormula fires before a CalculatedField is being rendered.
        /// Now you can calculate the current cell's value, based on the other cells values.
        /// Here IList contains actual row's cells values.
        /// It can be null.         
        /// </param>
        public void CalculatedField(bool isCalculatedField, Func<IList<CellData>, object> calculatedFieldFormula)
        {
            _pdfColumnAttributes.IsCalculatedField = isCalculatedField;
            _pdfColumnAttributes.CalculatedFieldFormula = calculatedFieldFormula;
        }

        /// <summary>
        /// Produce a new column based on the existing values of other columns.
        /// </summary>
        /// <param name="calculatedFieldFormula">
        /// CalculatedFieldFormula fires before a CalculatedField is being rendered.
        /// Now you can calculate the current cell's value, based on the other cells values.
        /// Here IList contains actual row's cells values.
        /// It can be null.                  
        /// </param>
        public void CalculatedField(Func<IList<CellData>, object> calculatedFieldFormula)
        {
            CalculatedField(true, calculatedFieldFormula);
        }

        /// <summary>
        /// Aggregate Function
        /// It can be null.
        /// </summary>
        public void AggregateFunction(Action<AggregateFunctionBuilder> aggregateFunctionBuilder)
        {
            var builder = new AggregateFunctionBuilder();
            aggregateFunctionBuilder(builder);

            _pdfColumnAttributes.AggregateFunction = builder.AggregateFunction;
        }

        /// <summary>
        /// Group Settings
        /// </summary>
        /// <param name="includeInGrouping">Defining a group of rows by including this filed in grouping</param>
        /// <param name="includedGroupFieldEqualityComparer">Custom comparison implementation of current and last field values of this column, to detect start of a new group</param>
        public void Group(bool includeInGrouping, Func<object, object, bool> includedGroupFieldEqualityComparer)
        {
            _pdfColumnAttributes.IncludeInGrouping = includeInGrouping;
            _pdfColumnAttributes.IncludedGroupFieldEqualityComparer = includedGroupFieldEqualityComparer;
        }

        /// <summary>
        /// Group Settings. Defining a group of rows by including this filed in grouping.
        /// </summary>
        /// <param name="includedGroupFieldEqualityComparer">Custom comparison implementation of current and last field values of this column, to detect start of a new group</param>
        public void Group(Func<object, object, bool> includedGroupFieldEqualityComparer)
        {
            Group(true, includedGroupFieldEqualityComparer);
        }
    }
}
