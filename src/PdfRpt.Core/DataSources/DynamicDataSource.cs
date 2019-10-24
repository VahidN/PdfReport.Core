using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using PdfRpt.Core.Contracts;

namespace PdfRpt.DataSources
{
    /// <summary>
    /// A collection of dynamic ExpandoObjects
    /// </summary>
    public class DynamicDataSource : IDataSource
    {
        private readonly IList<string> _fields = new List<string>();
        private readonly IEnumerable<ExpandoObject> _listOfRows;

        /// <summary>
        /// Converts a collection of dynamic ExpandoObjects to an IEnumerable of Pdf Cells Data.
        /// </summary>
        /// <param name="listOfRows">list of items</param>
        public DynamicDataSource(IEnumerable<ExpandoObject> listOfRows)
        {
            _listOfRows = listOfRows;
            getColumns();
        }

        /// <summary>
        /// The data to render.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IList<CellData>> Rows()
        {
            if (_listOfRows == null) yield break;

            foreach (IDictionary<string, object> row in _listOfRows)
            {
                var list = new List<CellData>();
                var index = 0;
                foreach (var field in _fields)
                {
                    var value = row[field];
                    var pdfCellData = new CellData
                    {
                        PropertyName = field,
                        PropertyValue = value,
                        PropertyIndex = index++,
                        PropertyType = value?.GetType()
                    };
                    list.Add(pdfCellData);
                }
                yield return list;
            }
        }

        private void getColumns()
        {
            var firstItem = _listOfRows.FirstOrDefault();
            if (firstItem == null)
            {
                return;
            }

            foreach (var item in firstItem)
            {
                _fields.Add(item.Key);
            }
        }
    }
}