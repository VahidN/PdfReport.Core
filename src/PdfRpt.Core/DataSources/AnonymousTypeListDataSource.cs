using System.Collections;
using System.Collections.Generic;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.DataSources
{
    /// <summary>
    /// A list DataSource
    /// </summary>    
    public class AnonymousTypeListDataSource : IDataSource
    {
        #region Fields

        readonly IEnumerable _listOfRows;
        private readonly int _dumpLevel;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Converts a list of anonymous type items to an IEnumerable of Pdf Cells Data.
        /// </summary>
        /// <param name="listOfRows">list of items</param>
        /// <param name="dumpLevel">how many levels should be searched</param> 
        public AnonymousTypeListDataSource(IEnumerable listOfRows, int dumpLevel = 2)
        {
            _listOfRows = listOfRows;
            _dumpLevel = dumpLevel;
        }

        #endregion Constructors

        #region Methods (1)

        // Public Methods (1) 

        /// <summary>
        /// The data to render.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IList<CellData>> Rows()
        {
            if (_listOfRows == null) yield break;

            foreach (var item in _listOfRows)
            {
                var list = new DumpNestedProperties().DumpPropertyValues(item, string.Empty, _dumpLevel);
                yield return list;
            }
        }

        #endregion Methods
    }
}