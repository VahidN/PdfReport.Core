using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Aggregates.Numbers
{
    /// <summary>
    /// Standard deviation function class.
    /// It only works with numbers. If you want to apply it on other data types, you need to create your own AggregateFunction by implementing the IAggregateFunc.
    /// </summary>
    public class StdDev : IAggregateFunction
    {
        /// <summary>
        /// Fires before rendering of this cell.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public Func<object, string> DisplayFormatFormula { set; get; }

        #region Fields (3)

        double _groupStdDev;
        double _overallStdDev;
        readonly Variance _varianceFunc = new Variance();

        #endregion Fields

        #region Properties (2)

        /// <summary>
        /// Returns current groups' aggregate value.
        /// </summary>
        public object GroupValue
        {
            get { return _groupStdDev; }
        }

        /// <summary>
        /// Returns current row's aggregate value without considering the presence of the groups.
        /// </summary>
        public object OverallValue
        {
            get { return _overallStdDev; }
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        /// <summary>
        /// Fires after adding a cell to the main table.
        /// </summary>
        /// <param name="cellDataValue">Current cell's data</param>
        /// <param name="isNewGroupStarted">Indicated starting a new group</param>
        public void CellAdded(object cellDataValue, bool isNewGroupStarted)
        {
            _varianceFunc.CellAdded(cellDataValue, isNewGroupStarted);
            _groupStdDev = Math.Sqrt(Convert.ToDouble(_varianceFunc.GroupValue, CultureInfo.InvariantCulture));
            _overallStdDev = Math.Sqrt(Convert.ToDouble(_varianceFunc.OverallValue, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// A general method which takes a list of data and calculates its corresponding aggregate value.
        /// It will be used to calculate the aggregate value of each pages individually, with considering the previous pages data.
        /// </summary>
        /// <param name="columnCellsSummaryData">List of data</param>
        /// <returns>Aggregate value</returns>
        public object ProcessingBoundary(IList<SummaryCellData> columnCellsSummaryData)
        {
            if (columnCellsSummaryData == null || !columnCellsSummaryData.Any()) return 0;
            return Math.Sqrt((double)_varianceFunc.ProcessingBoundary(columnCellsSummaryData));
        }
        #endregion Methods
    }
}
