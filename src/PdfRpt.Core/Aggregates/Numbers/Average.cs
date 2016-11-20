using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Aggregates.Numbers
{
    /// <summary>
    /// Average function class.
    /// It only works with numbers. If you want to apply it on other data types, you need to create your own AggregateFunction by implementing the IAggregateFunc.
    /// </summary>
    public class Average : IAggregateFunction
    {
        /// <summary>
        /// Fires before rendering of this cell.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public Func<object, string> DisplayFormatFormula { set; get; }

        #region Fields (6)

        double _groupAvg;
        long _groupRowNumber;
        double _groupSum;
        double _overallAvg;
        long _overallRowNumber;
        double _overallSum;

        #endregion Fields

        #region Properties (2)

        /// <summary>
        /// Returns current groups' aggregate value.
        /// </summary>
        public object GroupValue
        {
            get { return _groupAvg; }
        }

        /// <summary>
        /// Returns current row's aggregate value without considering the presence of the groups.
        /// </summary>
        public object OverallValue
        {
            get { return _overallAvg; }
        }

        #endregion Properties

        #region Methods (4)

        // Public Methods (1) 

        /// <summary>
        /// Fires after adding a cell to the main table.
        /// </summary>
        /// <param name="cellDataValue">Current cell's data</param>
        /// <param name="isNewGroupStarted">Indicated starting a new group</param>
        public void CellAdded(object cellDataValue, bool isNewGroupStarted)
        {
            checkNewGroupStarted(isNewGroupStarted);

            _overallRowNumber++;
            _groupRowNumber++;

            var cellValue = Convert.ToDouble(cellDataValue, CultureInfo.InvariantCulture);
            groupAvg(cellValue);
            overallAvg(cellValue);
        }
        // Private Methods (3) 

        private void checkNewGroupStarted(bool newGroupStarted)
        {
            if (newGroupStarted)
            {
                _groupRowNumber = 0;
                _groupAvg = 0;
                _groupSum = 0;
            }
        }

        private void groupAvg(double cellValue)
        {
            _groupSum += cellValue;
            _groupAvg = _groupSum / _groupRowNumber;
        }

        private void overallAvg(double cellValue)
        {
            _overallSum += cellValue;
            _overallAvg = _overallSum / _overallRowNumber;
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

            var list = columnCellsSummaryData;
            var count = list.Count();

            var sum = list.Sum(item => Convert.ToDouble(item.CellData.PropertyValue, CultureInfo.InvariantCulture));
            return sum / count;

        }
        #endregion Methods
    }
}
