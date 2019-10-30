using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using PdfRpt.Core.Contracts;
using PdfRpt.DataAnnotations;
using PdfRpt.DataSources;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Processing custom data annotations.
    /// </summary>
    public static class PropertyDataAnnotations
    {
        /// <summary>
        /// Returns PdfRptColumnAggregateFunctionAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>AggregateFunction</returns>
        public static AggregateFunction? GetAggregateFunctionAttribute(this MemberInfo info)
        {
            var aggregateFunction = info.GetCustomAttributes(true).OfType<AggregateFunctionAttribute>().FirstOrDefault();
            if (aggregateFunction == null) return null;
            return aggregateFunction.AggregateFunction;
        }

        /// <summary>
        /// Returns PdfRptColumnCellsHorizontalAlignmentAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>CellsHorizontalAlignment</returns>
        public static HorizontalAlignment? GetCellsHorizontalAlignmentAttribute(this MemberInfo info)
        {
            var cellsHorizontalAlignment = info.GetCustomAttributes(true).OfType<CellsHorizontalAlignmentAttribute>().FirstOrDefault();
            if (cellsHorizontalAlignment == null) return null;
            return cellsHorizontalAlignment.CellsHorizontalAlignment;
        }

        /// <summary>
        /// Returns PdfRptColumnIsVisibleAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>IsVisible</returns>
        public static bool? GetColumnIsVisibleAttribute(this MemberInfo info)
        {
            var columnIsVisible = info.GetCustomAttributes(true).OfType<IsVisibleAttribute>().FirstOrDefault();
            if (columnIsVisible == null) return null;
            return columnIsVisible.IsVisible;
        }

        /// <summary>
        /// Returns PdfRptColumnOrderAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>Order</returns>
        public static int? GetColumnOrderAttribute(this MemberInfo info)
        {
            var columnOrder = info.GetCustomAttributes(true).OfType<OrderAttribute>().FirstOrDefault();
            if (columnOrder == null) return null;
            return columnOrder.Order;
        }

        /// <summary>
        /// Returns PdfRptColumnPdfRptColumnWidthAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>Width</returns>
        public static float? GetColumnWidthAttribute(this MemberInfo info)
        {
            var columnWidth = info.GetCustomAttributes(true).OfType<WidthAttribute>().FirstOrDefault();
            if (columnWidth == null) return null;
            return columnWidth.Width;
        }

        /// <summary>
        /// Returns PdfRptColumnPropertyNameAttribute data.
        /// Processing order is checking PdfRptColumnPropertyNameAttribute first, then DisplayNameAttribute and finally DescriptionAttribute.
        /// If none of these is available, the actual property name will be returned.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>PropertyName</returns>
        public static string GetColumnPropertyNameAttribute(this MemberInfo info)
        {
            var columnPropertyName = info?.GetCustomAttributes(true).OfType<PropertyNameAttribute>().FirstOrDefault();
            if (columnPropertyName != null)
            {
                return columnPropertyName.PropertyName;
            }

            var displayName = info?.GetCustomAttributes(true).OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null)
            {
                return displayName.DisplayName;
            }

            var description = info?.GetCustomAttributes(true).OfType<DescriptionAttribute>().FirstOrDefault();
            if (description != null)
            {
                return description.Description;
            }

            var display = info?.GetCustomAttributes(true).OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null)
            {
                return display.Name;
            }

            return null;
        }

        /// <summary>
        /// Getting string attribute of Enum's value.
        /// Processing order is checking DisplayNameAttribute first and then DescriptionAttribute.
        /// If none of these is available, value.ToString() will be returned.
        /// </summary>
        /// <param name="flags">enum value</param>
        /// <returns>string attribute of Enum's value</returns>
        public static string GetEnumStringValue(this Enum flags)
        {
#if NET40
            if (Attribute.IsDefined(flags.GetType(), typeof(FlagsAttribute)))
#else
            if (flags.GetType().GetTypeInfo().GetCustomAttributes(true).OfType<FlagsAttribute>().Any())
#endif
            {
                var text = getEnumFlagsText(flags);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    return text;
                }
            }
            return getEnumValueText(flags);
        }

        private static string getEnumFlagsText(Enum flags)
        {
            const char leftToRightSeparator = ',';
            const char rightToRightSeparator = '،';

            var sb = new StringBuilder();
            var items = Enum.GetValues(flags.GetType());
            foreach (var value in items)
            {
                if (flags.HasFlag((Enum)value) && Convert.ToInt64((Enum)value) != 0)
                {
                    string text = getEnumValueText((Enum)value);
                    var separator = text.ContainsRtlText() ? rightToRightSeparator : leftToRightSeparator;
                    sb.Append(text).Append(separator).Append(" ");
                }
            }

            return sb.ToString().Trim().TrimEnd(leftToRightSeparator).TrimEnd(rightToRightSeparator);
        }

        private static string getEnumValueText(Enum value)
        {
            var text = value.ToString();
            var info = value.GetType().GetField(text);

            var description = info?.GetCustomAttributes(true).OfType<DescriptionAttribute>().FirstOrDefault();
            if (description != null)
            {
                return description.Description;
            }

            var displayName = info?.GetCustomAttributes(true).OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null)
            {
                return displayName.DisplayName;
            }

            var display = info?.GetCustomAttributes(true).OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null)
            {
                return display.Name;
            }

            return text;
        }

        /// <summary>
        /// Returns DisplayFormatAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>NullDisplayText</returns>
        public static string GetNullDisplayTextAttribute(this MemberInfo info)
        {
            var displayFormat = info.GetCustomAttributes(true).OfType<DisplayFormatAttribute>().FirstOrDefault();
            if (displayFormat == null) return string.Empty;
            return displayFormat.NullDisplayText;
        }

        /// <summary>
        /// Returns DisplayFormatAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>DataFormatString</returns>
        public static string GetDataFormatStringAttribute(this MemberInfo info)
        {
            var displayFormat = info.GetCustomAttributes(true).OfType<DisplayFormatAttribute>().FirstOrDefault();
            if (displayFormat == null) return string.Empty;
            return displayFormat.DataFormatString;
        }

        /// <summary>
        /// Determines whether a IPdfReportDataSource is type of StronglyTypedListDataSource or not.
        /// </summary>
        /// <param name="bodyDataSource">data source</param>
        /// <returns>true/false</returns>
        public static bool IsStronglyTypedListDataSource(this IDataSource bodyDataSource)
        {
            var baseType = bodyDataSource.GetType();
#if NET40
            if (!baseType.IsGenericType) return false;
#else
            if (!baseType.GetTypeInfo().IsGenericType) return false;
#endif
            return baseType.GetGenericTypeDefinition() == typeof(StronglyTypedListDataSource<>);
        }

        /// <summary>
        /// Gets PropertiesInfo[] Of StronglyTypedListDataSource.
        /// </summary>
        /// <param name="bodyDataSource">data source</param>
        /// <returns>properties list</returns>
        public static PropertyInfo[] GetPropertiesInfoOfStronglyTypedListDataSource(this IDataSource bodyDataSource)
        {
            if (!bodyDataSource.IsStronglyTypedListDataSource()) return null;
            var genericType = bodyDataSource.GetType();
            var genericArguments = genericType.GetGenericArguments();
            var objectType = genericArguments[0];
            return objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Gets FieldInfo[] Of StronglyTypedListDataSource
        /// </summary>
        /// <param name="bodyDataSource">data source</param>
        /// <returns>Fields Info</returns>
        public static FieldInfo[] GetFieldsInfoOfStronglyTypedListDataSource(this IDataSource bodyDataSource)
        {
            if (!bodyDataSource.IsStronglyTypedListDataSource()) return null;
            var genericType = bodyDataSource.GetType();
            var genericArguments = genericType.GetGenericArguments();
            var objectType = genericArguments[0];
            return objectType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        }

        /// <summary>
        /// Returns ColumnTemplateAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>A typeof(IPdfColumnItemsTemplate) value.</returns>
        public static Type GetPdfRptColumnTemplateAttribute(this MemberInfo info)
        {
            var templateType = info.GetCustomAttributes(true).OfType<ColumnItemsTemplateAttribute>().FirstOrDefault();
            if (templateType == null) return null;
            return templateType.TemplateType;
        }

        /// <summary>
        /// Returns PdfRptColumnCustomAggregateFunctionAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>A typeof(IAggregateFunction) value.</returns>
        public static Type GetPdfRptColumnCustomAggregateFunctionAttribute(this MemberInfo info)
        {
            var aggregateFunctionType = info.GetCustomAttributes(true).OfType<CustomAggregateFunctionAttribute>().FirstOrDefault();
            if (aggregateFunctionType == null) return null;
            return aggregateFunctionType.AggregateFunctionType;
        }

        /// <summary>
        /// Returns PdfRptColumnIsVisibleAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>IsCalculated</returns>
        public static bool? GetColumnIsCalculatedFieldAttribute(this MemberInfo info)
        {
            var isCalculatedField = info.GetCustomAttributes(true).OfType<IsCalculatedFieldAttribute>().FirstOrDefault();
            if (isCalculatedField == null) return null;
            return isCalculatedField.IsCalculated;
        }

        /// <summary>
        /// Returns CalculatedFieldFormulaAttribute data.
        /// </summary>
        /// <param name="fieldsInfo">current object's FieldInfo[]</param>
        /// <param name="forPropertyName">corresponding property</param>
        /// <returns>CalculatedFieldFormula</returns>
        public static Func<IList<CellData>, object> GetCalculatedFieldFormulaAttribute(this FieldInfo[] fieldsInfo, string forPropertyName)
        {
            foreach (var info in fieldsInfo)
            {
                var attr = info.GetCustomAttributes(true).OfType<CalculatedFieldFormulaAttribute>().FirstOrDefault(a => a.PropertyName == forPropertyName);
                if (attr == null) continue;

                var attributeValue = info.GetValue(null);
                return attributeValue as Func<IList<CellData>, object>;
            }
            return null;
        }

        /// <summary>
        /// Returns IncludeInGroupingAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>IncludeInGrouping</returns>
        public static bool? GetColumnIncludeInGroupingAttribute(this MemberInfo info)
        {
            var includeInGroupingAttr = info.GetCustomAttributes(true).OfType<IncludeInGroupingAttribute>().FirstOrDefault();
            if (includeInGroupingAttr == null) return null;
            return includeInGroupingAttr.IncludeInGrouping;
        }

        /// <summary>
        /// Returns IncludedGroupFieldEqualityComparerAttribute data.
        /// </summary>
        /// <param name="fieldsInfo">current object's FieldInfo[]</param>
        /// <param name="forPropertyName">corresponding property</param>
        /// <returns>IncludedGroupFieldEqualityComparer</returns>
        public static Func<object, object, bool> GetIncludedGroupFieldEqualityComparerAttribute(this FieldInfo[] fieldsInfo, string forPropertyName)
        {
            foreach (var info in fieldsInfo)
            {
                var attr = info.GetCustomAttributes(true).OfType<IncludedGroupFieldEqualityComparerAttribute>().FirstOrDefault(a => a.PropertyName == forPropertyName);
                if (attr == null) continue;

                var attributeValue = info.GetValue(null);
                return attributeValue as Func<object, object, bool>;
            }
            return null;
        }

        /// <summary>
        /// Returns FixedHeightAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>FixedHeight</returns>
        public static float? GetFixedHeightAttribute(this MemberInfo info)
        {
            var fixedHeight = info.GetCustomAttributes(true).OfType<FixedHeightAttribute>().FirstOrDefault();
            if (fixedHeight == null) return null;
            return fixedHeight.FixedHeight;
        }

        /// <summary>
        /// Returns MinimumHeightAttribute data.
        /// </summary>
        /// <param name="info">Property metadata info</param>
        /// <returns>MinimumHeight</returns>
        public static float? GetMinimumHeightAttribute(this MemberInfo info)
        {
            var minimumHeight = info.GetCustomAttributes(true).OfType<MinimumHeightAttribute>().FirstOrDefault();
            if (minimumHeight == null) return null;
            return minimumHeight.MinimumHeight;
        }
    }
}