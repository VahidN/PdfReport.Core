using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// PdfCellData class extensions
    /// </summary>
    public static class CellDataExt
    {
        /// <summary>
        /// Gets the PropertyValue of an item in list of PdfCellData, which its PropertyName is specified by property expression.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="tableRowData">List of PdfCellData items</param>
        /// <param name="property">A property to find</param>
        /// <param name="nullValue">An optional nullValue of the expected object.</param>
        /// <param name="propertyIndex">Index of the property in data source, in case of duplicate properties</param> 
        /// <returns>It returns null if nothing has found in the list.</returns>
        public static object GetValueOf<T>(this IList<CellData> tableRowData, Expression<Func<T, object>> property, object nullValue = null, int propertyIndex = -1)
        {
            if (tableRowData == null) return nullValue;

            var selectedProperty = PropertyHelper.Name(property);
            CellData data;
            if (propertyIndex > -1)
            {
                data = tableRowData.FirstOrDefault(x => x.PropertyName == selectedProperty && x.PropertyIndex == propertyIndex);
            }
            else
            {
                data = tableRowData.FirstOrDefault(x => x.PropertyName == selectedProperty);
            }
            return data == null ? nullValue : data.PropertyValue;
        }

        /// <summary>
        /// Gets the PropertyValue of an item in list of PdfCellData.
        /// Please note that property names are case sensitive.
        /// </summary>
        /// <param name="tableRowData">List of PdfCellData items</param>
        /// <param name="property">A property to find</param>
        /// <param name="nullValue">An optional nullValue of the expected object.</param>
        /// <param name="propertyIndex">Index of the property in data source, in case of duplicate properties</param>
        /// <returns></returns>
        public static object GetValueOf(this IList<CellData> tableRowData, string property, object nullValue = null, int propertyIndex = -1)
        {
            if (tableRowData == null) return nullValue;
            CellData data;
            if (propertyIndex > -1)
            {
                data = tableRowData.FirstOrDefault(x => x.PropertyName == property && x.PropertyIndex == propertyIndex);
            }
            else
            {
                data = tableRowData.FirstOrDefault(x => x.PropertyName == property);
            }
            return data == null ? nullValue : data.PropertyValue;
        }

        /// <summary>
        /// Gets the PropertyValue of an item in list of PdfCellData, which its PropertyName is specified by property expression.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="tableRowData">List of PdfCellData items</param>
        /// <param name="property">A property to find</param>
        /// <param name="format">An optional format of the expected object.</param>
        /// <param name="nullValue">An optional nullValue of the expected object.</param>
        /// <param name="propertyIndex">Index of the property in data source, in case of duplicate properties</param>
        /// <returns>It returns string.Empty if nothing has found in the list.</returns>
        public static string GetSafeStringValueOf<T>(this IList<CellData> tableRowData, Expression<Func<T, object>> property, string format = "", string nullValue = "", int propertyIndex = -1)
        {
            var data = tableRowData.GetValueOf(property, nullValue, propertyIndex);
            if (data == null) return nullValue;
            return string.IsNullOrEmpty(format) ? data.ToString() : string.Format(CultureInfo.InvariantCulture, format, data);
        }

        /// <summary>
        /// Gets the PropertyValue of an item in list of PdfCellData, which its PropertyName is specified by property value.
        /// Please note that property names are case sensitive.
        /// </summary>
        /// <param name="tableRowData">List of PdfCellData items</param>
        /// <param name="property">A property to find</param>
        /// <param name="format">An optional format of the expected object.</param>
        /// <param name="nullValue">An optional nullValue of the expected object.</param>
        /// <param name="propertyIndex">Index of the property in data source, in case of duplicate properties</param>
        /// <returns>It returns string.Empty if nothing has found in the list.</returns>
        public static string GetSafeStringValueOf(this IList<CellData> tableRowData, string property, string format = "", string nullValue = "", int propertyIndex = -1)
        {
            if (tableRowData == null) return nullValue;
            CellData data;
            if (propertyIndex > -1)
            {
                data = tableRowData.FirstOrDefault(x => x.PropertyName == property && x.PropertyIndex == propertyIndex);
            }
            else
            {
                data = tableRowData.FirstOrDefault(x => x.PropertyName == property);
            }
            if (data == null || data.PropertyValue == null) return nullValue;
            return string.IsNullOrEmpty(format) ? data.PropertyValue.ToString() : string.Format(CultureInfo.InvariantCulture, format, data.PropertyValue);
        }

        /// <summary>
        /// Sets the PropertyValue of an item in list of PdfCellData.
        /// Please note that property names are case sensitive.
        /// </summary>
        /// <param name="tableRowData">List of PdfCellData items</param>
        /// <param name="property">A property to find</param>
        /// <param name="value">a value to set</param>
        /// <param name="propertyIndex">Index of the property in data source, in case of duplicate properties</param>
        public static void SetValueOf(this IList<CellData> tableRowData, string property, object value, int propertyIndex = -1)
        {
            if (tableRowData == null) return;
            CellData data;
            if (propertyIndex > -1)
            {
                data = tableRowData.FirstOrDefault(x => x.PropertyName == property && x.PropertyIndex == propertyIndex);
            }
            else
            {
                data = tableRowData.FirstOrDefault(x => x.PropertyName == property);
            }
            if (data == null) return;
            data.PropertyValue = value;
        }
    }
}
