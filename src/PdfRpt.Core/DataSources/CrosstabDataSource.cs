using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.DataSources
{
    /// <summary>
    /// A Dynamic Crosstab DataSource
    /// </summary>
    public class CrosstabDataSource : IDataSource
    {
        readonly IEnumerable _source;
        readonly bool _topFieldsAreVariableInEachRow;
        readonly IList<string> _topFields = new List<string>();
        private int _index;
        private readonly int _dumpLevel;

        /// <summary>
        /// Converts the result of the CrosstabExtension.Pivot method to an IEnumerable of Pdf Cells Data
        /// </summary>
        /// <param name="source">Result of the CrosstabExtension.Pivot method</param>
        /// <param name="topFieldsAreVariableInEachRow">Indicates whether top fields should be prepopulated before starting the main table's rendering or not</param>
        /// <param name="dumpLevel">how many levels should be searched</param>
        public CrosstabDataSource(IEnumerable source, bool topFieldsAreVariableInEachRow = false, int dumpLevel = 2)
        {
            _source = source;
            _topFieldsAreVariableInEachRow = topFieldsAreVariableInEachRow;
            _dumpLevel = dumpLevel;
        }

        /// <summary>
        /// The data to render.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IList<CellData>> Rows()
        {
            if (_source == null) yield break;

            var rowsData = new List<List<CellData>>();

            foreach (var row in _source)
            {
                var list = new List<CellData>();
                foreach (var rowObjects in row.GetType().GetProperties())
                {
                    var columnObject = rowObjects.GetPropertyValue(row);
                    if (columnObject is IEnumerable)
                    {
                        processIEnumerables(list, columnObject);
                    }
                    else
                    {
                        processNormalProperties(list, columnObject);
                    }
                }

                if (!_topFieldsAreVariableInEachRow)
                {
                    yield return list;
                }
                else
                {
                    //we need to know the largest row, because top fields are variable in each row
                    rowsData.Add(list);
                }
            }

            if (_topFieldsAreVariableInEachRow)
            {
                foreach (var list in rowsData)
                {
                    modifyRowInsertVariableFields(list);
                    yield return list;
                }
            }
        }

        private void modifyRowInsertVariableFields(List<CellData> firstRowsDataItems)
        {
            int topFieldsIndex;
            int insertIndex;
            findFirstInsertIndex(firstRowsDataItems, out topFieldsIndex, out insertIndex);
            insertMissingTopFields(firstRowsDataItems, topFieldsIndex, insertIndex);
        }

        private void insertMissingTopFields(List<CellData> firstRowsDataItems, int topFieldsIndex, int insertIndex)
        {
            for (var i = topFieldsIndex; i < _topFields.Count; i++)
            {
                var item = firstRowsDataItems.FirstOrDefault(x => x.PropertyName == this._topFields[i]);
                if (item == null)
                {
                    firstRowsDataItems.Insert(insertIndex,
                        new CellData
                        {
                            PropertyName = _topFields[i],
                            PropertyValue = string.Empty,
                            PropertyIndex = _index++
                        });
                }
                insertIndex++;
            }
        }

        private void findFirstInsertIndex(List<CellData> firstRowsDataItems, out int topFieldsIndex, out int insertIndex)
        {
            var itemsCount = firstRowsDataItems.Count;
            var topFieldsStartIndex = 0;
            for (var i = 0; i < itemsCount; i++)
            {
                if (firstRowsDataItems[i].PropertyName == _topFields[0])
                {
                    topFieldsStartIndex = i;
                    break;
                }
            }

            topFieldsIndex = 0;
            insertIndex = 0;
            var topFieldsCount = _topFields.Count;
            for (var i = topFieldsStartIndex; i < itemsCount; i++)
            {
                if (topFieldsIndex >= topFieldsCount) break;
                if (firstRowsDataItems[i].PropertyName != _topFields[topFieldsIndex])
                {
                    insertIndex = i;
                    break;
                }
                topFieldsIndex++;
            }
        }

        private void processNormalProperties(ICollection<CellData> list, object columnObject)
        {
            var dataList = new DumpNestedProperties().DumpPropertyValues(columnObject, string.Empty, _dumpLevel);
            foreach (var item in dataList)
            {
                if (item == null) continue;
                list.Add(item);
            }
        }

        private void processIEnumerables(ICollection<CellData> list, object columnObject)
        {
            foreach (var keyValue in (IEnumerable)columnObject)
            {
                object key = null;
                object value = null;
                foreach (var column in keyValue.GetType().GetProperties())
                {
                    if (column.Name == "Z")
                        key = column.GetPropertyValue(keyValue);
                    if (column.Name == "V")
                        value = column.GetPropertyValue(keyValue);
                }

                if (key == null)
                    throw new InvalidOperationException("key/PropertyName is null. Source should be the result of the CrosstabExtension.Pivot method here.");

                list.Add(new CellData
                {
                    PropertyName = key.ToString(),
                    PropertyValue = value,
                    PropertyIndex = _index++
                });

                if (_topFieldsAreVariableInEachRow)
                {
                    if (!_topFields.Contains(key.ToString())) _topFields.Add(key.ToString());
                }
            }
        }
    }
}