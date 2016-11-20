using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Applies Annotations to a list of PdfColumnAttributes.
    /// </summary>
    public static class ApplyDataAnnotations
    {
        /// <summary>
        /// Applies Annotations to a list of PdfColumnAttributes.
        /// </summary>
        /// <param name="bodyDataSource">Data source</param>        
        /// <param name="result">A list of PdfColumnAttributes</param>
        /// <param name="areColumnsAdHoc">It's possible to remove the MainTableColumns part completely</param>
        public static void ApplyPropertyDataAnnotations(this IDataSource bodyDataSource, IList<ColumnAttributes> result, bool areColumnsAdHoc = false)
        {
            var properties = bodyDataSource.GetPropertiesInfoOfStronglyTypedListDataSource();
            if (properties == null || !properties.Any()) return;

            var fieldsInfo = bodyDataSource.GetFieldsInfoOfStronglyTypedListDataSource();

            foreach (var property in properties)
            {
                var columnAttributes = result.FirstOrDefault(x => x.PropertyName == property.Name);
                if (columnAttributes == null) continue;

                applyPropertyName(property, columnAttributes, areColumnsAdHoc);
                applyAggregateFunction(property, columnAttributes);
                applyFixedHeight(property, columnAttributes);
                applyMinimumHeight(property, columnAttributes);
                applyCellsHorizontalAlignment(property, columnAttributes);
                applyColumnIsVisible(property, columnAttributes);
                applyOrder(property, columnAttributes);
                applyWidth(property, columnAttributes);
                applyDataFormatString(property, columnAttributes);
                applyColumnItemsTemplate(property, columnAttributes);
                applyCalculatedField(property, columnAttributes, fieldsInfo);
                applyIncludedGroupFieldEqualityComparer(property, columnAttributes, fieldsInfo);
            }
        }

        private static void applyIncludedGroupFieldEqualityComparer(PropertyInfo property, ColumnAttributes columnAttributes, FieldInfo[] fieldsInfo)
        {
            var includeInGrouping = property.GetColumnIncludeInGroupingAttribute();
            if (includeInGrouping.HasValue)
                columnAttributes.IncludeInGrouping = includeInGrouping.Value;

            if (columnAttributes.IncludeInGrouping)
            {
                var equalityComparer = fieldsInfo.GetIncludedGroupFieldEqualityComparerAttribute(property.Name);
                if (equalityComparer != null)
                    columnAttributes.IncludedGroupFieldEqualityComparer = equalityComparer;
            }
        }

        private static void applyCalculatedField(PropertyInfo property, ColumnAttributes columnAttributes, FieldInfo[] fieldsInfo)
        {
            var isCalculatedField = property.GetColumnIsCalculatedFieldAttribute();
            if (isCalculatedField != null)
                columnAttributes.IsCalculatedField = isCalculatedField.Value;

            if (columnAttributes.IsCalculatedField)
            {
                var calculatedFieldFormula = fieldsInfo.GetCalculatedFieldFormulaAttribute(property.Name);
                if (calculatedFieldFormula != null)
                {
                    columnAttributes.CalculatedFieldFormula = calculatedFieldFormula;
                }
            }
        }

        private static void applyColumnItemsTemplate(PropertyInfo property, ColumnAttributes columnAttributes)
        {
            var columnItemsTemplateType = property.GetPdfRptColumnTemplateAttribute();
            if (columnItemsTemplateType != null)
                columnAttributes.ColumnItemsTemplate = Activator.CreateInstance(columnItemsTemplateType) as IColumnItemsTemplate;
        }

        private static void applyDataFormatString(PropertyInfo property, ColumnAttributes columnAttributes)
        {
            var dataFormatString = property.GetDataFormatStringAttribute();
            if (!string.IsNullOrEmpty(dataFormatString))
            {
                columnAttributes.ColumnItemsTemplate.BasicProperties.DisplayFormatFormula = data => string.Format(CultureInfo.InvariantCulture, dataFormatString, data);
                if (columnAttributes.AggregateFunction != null)
                    columnAttributes.AggregateFunction.DisplayFormatFormula = data => string.Format(CultureInfo.InvariantCulture, dataFormatString, data);
            }
        }

        private static void applyWidth(PropertyInfo property, ColumnAttributes columnAttributes)
        {
            var columnWidth = property.GetColumnWidthAttribute();
            if (columnWidth != null)
                columnAttributes.Width = columnWidth.Value;
        }

        private static void applyOrder(PropertyInfo property, ColumnAttributes columnAttributes)
        {
            var order = property.GetColumnOrderAttribute();
            if (order != null)
                columnAttributes.Order = order.Value;
        }

        private static void applyColumnIsVisible(PropertyInfo property, ColumnAttributes columnAttributes)
        {
            var isVisible = property.GetColumnIsVisibleAttribute();
            if (isVisible.HasValue)
                columnAttributes.IsVisible = isVisible.Value;
        }

        private static void applyCellsHorizontalAlignment(PropertyInfo property, ColumnAttributes columnAttributes)
        {
            var cellsHorizontalAlignment = property.GetCellsHorizontalAlignmentAttribute();
            if (cellsHorizontalAlignment != null)
                columnAttributes.CellsHorizontalAlignment = cellsHorizontalAlignment.Value;
        }

        private static void applyAggregateFunction(PropertyInfo property, ColumnAttributes columnAttributes)
        {
            var aggregateFunction = property.GetAggregateFunctionAttribute();
            if (aggregateFunction != null)
                columnAttributes.AggregateFunction = new AggregateProvider(aggregateFunction.Value);

            if (columnAttributes.AggregateFunction == null)
            {
                var customAggregateFunction = property.GetPdfRptColumnCustomAggregateFunctionAttribute();
                if (customAggregateFunction != null)
                    columnAttributes.AggregateFunction = Activator.CreateInstance(customAggregateFunction) as IAggregateFunction;
            }
        }

        private static void applyPropertyName(PropertyInfo property, ColumnAttributes columnAttributes, bool areColumnsAdHoc)
        {
            if (columnAttributes.HeaderCell == null)
                columnAttributes.HeaderCell = new HeadingCell();

            if (string.IsNullOrEmpty(columnAttributes.HeaderCell.Caption) || areColumnsAdHoc)
            {
                var caption = property.GetColumnPropertyNameAttribute();
                if (!string.IsNullOrEmpty(caption))
                    columnAttributes.HeaderCell.Caption = caption;
            }

            if (string.IsNullOrEmpty(columnAttributes.HeaderCell.Caption))
                columnAttributes.HeaderCell.Caption = property.Name;
        }

        private static void applyFixedHeight(PropertyInfo property, ColumnAttributes columnAttributes)
        {
            var fixedHeight = property.GetFixedHeightAttribute();
            if (fixedHeight.HasValue)
                columnAttributes.FixedHeight = fixedHeight.Value;
        }

        private static void applyMinimumHeight(PropertyInfo property, ColumnAttributes columnAttributes)
        {
            var minimumHeight = property.GetMinimumHeightAttribute();
            if (minimumHeight.HasValue)
                columnAttributes.MinimumHeight = minimumHeight.Value;
        }
    }
}