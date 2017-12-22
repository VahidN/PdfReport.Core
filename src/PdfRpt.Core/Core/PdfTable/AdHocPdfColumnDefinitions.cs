using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// Here you can control how cells should be rendered based on their specific data types.
    /// </summary>
    public class AdHocPdfColumnDefinitions
    {
        #region Fields (2)

        readonly IDataSource _bodyDataSource;
        readonly AdHocColumnsConventions _conventions;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Here you can control how cells should be rendered based on their specific data types.
        /// </summary>
        /// <param name="bodyDataSource">PdfRpt's DataSource Contract</param>
        /// <param name="conventions">Here you can control how cells should be rendered based on their specific data types.</param>
        public AdHocPdfColumnDefinitions(IDataSource bodyDataSource, AdHocColumnsConventions conventions)
        {
            _bodyDataSource = bodyDataSource;
            _conventions = conventions;
        }

        #endregion Constructors

        #region Methods (6)

        // Public Methods (1)

        /// <summary>
        /// Creates PdfColumnDefinitions list based on the PdfRptAdHocColumnsConventions
        /// </summary>
        /// <returns>PdfColumnDefinitions list</returns>
        public IList<ColumnAttributes> CreatePdfColumnDefinitions()
        {
            var result = new List<ColumnAttributes>();

            if (_bodyDataSource == null || !_bodyDataSource.Rows().Any())
            {
                result.Add(getRowNoCol());
                return result;
            }

            tryShowRowNumberColumn(result);

            var firstRowCells = _bodyDataSource.Rows().FirstOrDefault();
            if (firstRowCells == null)
            {
                result.Add(getRowNoCol());
                return result;
            }

            var order = 2;
            foreach (var cellData in firstRowCells)
            {
                Type type = null;
                if (cellData.PropertyValue != null)
                    type = cellData.PropertyValue.GetType();

#if NET40
                if (type != null && type.BaseType == typeof(MulticastDelegate))
#else
                if (type != null && type.GetTypeInfo().BaseType == typeof(MulticastDelegate))
#endif
                    continue;

                var itemsTemplate = getColumnItemsTemplate(cellData.PropertyName, type);
                if (itemsTemplate.BasicProperties == null)
                    itemsTemplate.BasicProperties = new CellBasicProperties();

                setDisplayFormatFormula(cellData.PropertyName, type, itemsTemplate);
                var colDef = new ColumnAttributes
                {
                    HeaderCell = new HeadingCell
                    {
                        Caption = cellData.PropertyName
                    },
                    ColumnItemsTemplate = itemsTemplate,
                    Width = 1,
                    PropertyName = cellData.PropertyName,
                    IsVisible = true,
                    Order = order++,
                    CellsHorizontalAlignment = HorizontalAlignment.Center
                };

                setAggregateFunc(cellData.PropertyName, colDef, type);

                result.Add(colDef);
            }

            if (!result.Any())
            {
                result.Add(getRowNoCol());
            }

            return result;
        }

        // Private Methods

        private static ColumnAttributes getRowNoCol()
        {
            return new ColumnAttributes
            {
                HeaderCell = new HeadingCell
                {
                    Caption = "No."
                },
                ColumnItemsTemplate = new TextBlockField(),
                Width = 1,
                PropertyName = "_Row_No.",
                IsVisible = true,
                CellsHorizontalAlignment = HorizontalAlignment.Center,
                Order = 1,
                IsRowNumber = true
            };
        }

        private void setAggregateFunc(string columnName, ColumnAttributes colDef, Type type)
        {
            if (setColumnAggregateFunc(columnName, colDef)) return;
            if (_conventions == null || _conventions.TypesAggregateFunctions == null || type == null) return;

            var func = _conventions.TypesAggregateFunctions.FirstOrDefault(x => x.Key == type);
            if (func.Value != null)
            {
                colDef.AggregateFunction = func.Value;
            }
        }

        private bool setColumnAggregateFunc(string columnName, ColumnAttributes colDef)
        {
            if (_conventions == null || _conventions.ColumnNamesAggregateFunctions == null) return false;

            var func = _conventions.ColumnNamesAggregateFunctions.FirstOrDefault(x => x.Key == columnName);
            if (func.Value == null) return false;
            colDef.AggregateFunction = func.Value;
            return true;
        }

        private IColumnItemsTemplate getColumnItemsTemplate(string columnName, Type type)
        {
            var colTemplate = getColumnNameItemsTemplate(columnName);
            if (colTemplate != null) return colTemplate;

            if (_conventions == null || _conventions.TypesColumnItemsTemplates == null || type == null) return new TextBlockField();
            var itemsTemplate = _conventions.TypesColumnItemsTemplates.FirstOrDefault(x => x.Key == type);
            if (itemsTemplate.Value == null) return new TextBlockField();
            return itemsTemplate.Value;
        }

        private IColumnItemsTemplate getColumnNameItemsTemplate(string columnName)
        {
            if (_conventions == null || _conventions.ColumnNamesItemsTemplates == null) return null;
            var itemsTemplate = _conventions.ColumnNamesItemsTemplates.FirstOrDefault(x => x.Key == columnName);
            if (itemsTemplate.Value == null) return null;
            return itemsTemplate.Value;
        }

        private void setDisplayFormatFormula(string columnName, Type type, IColumnItemsTemplate template)
        {
            if (setColumnsDisplayFormatFormula(columnName, template)) return;
            if (_conventions == null || _conventions.TypesDisplayFormatFormulas == null || type == null) return;

            var displayFormatFormula = _conventions.TypesDisplayFormatFormulas.FirstOrDefault(x => x.Key == type);
            template.BasicProperties.DisplayFormatFormula = displayFormatFormula.Value;
        }

        private bool setColumnsDisplayFormatFormula(string columnName, IColumnItemsTemplate template)
        {
            if (_conventions == null || _conventions.ColumnNamesDisplayFormatFormulas == null) return false;

            var displayFormatFormula = _conventions.ColumnNamesDisplayFormatFormulas.FirstOrDefault(x => x.Key == columnName);
            if (displayFormatFormula.Value == null) return false;
            template.BasicProperties.DisplayFormatFormula = displayFormatFormula.Value;
            return true;
        }

        private void tryShowRowNumberColumn(ICollection<ColumnAttributes> result)
        {
            if (_conventions == null) return;
            if (!_conventions.ShowRowNumberColumn) return;

            var colDef = getRowNoCol();
            if (!string.IsNullOrEmpty(_conventions.RowNumberColumnCaption))
            {
                colDef.HeaderCell.Caption = _conventions.RowNumberColumnCaption;
            }

            result.Add(colDef);
        }

        #endregion Methods
    }
}