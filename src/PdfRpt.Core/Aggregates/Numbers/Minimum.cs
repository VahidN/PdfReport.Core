using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Aggregates.Numbers
{
    /// <summary>
    /// Minimum function class.
    /// It only works with numbers. If you want to apply it on other data types, you need to create your own AggregateFunction by implementing the IAggregateFunc.
    /// </summary>
    public class Minimum : IAggregateFunction
    {
        /// <summary>
        /// Fires before rendering of this cell.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public Func<object, string> DisplayFormatFormula { set; get; }

        #region Fields (4)

        double _groupMin;
        long _groupRowNumber;
        double _overallMin;
        long _overallRowNumber;

        #endregion Fields

        #region Properties (2)

        /// <summary>
        /// Returns current groups' aggregate value.
        /// </summary>
        public object GroupValue
        {
            get { return _groupMin; }
        }

        /// <summary>
        /// Returns current row's aggregate value without considering the presence of the groups.
        /// </summary>
        public object OverallValue
        {
            get { return _overallMin; }
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

            _groupRowNumber++;
            _overallRowNumber++;

            var cellValue = Convert.ToDouble(cellDataValue, CultureInfo.InvariantCulture);
            groupMin(cellValue);
            overallMin(cellValue);
        }
        // Private Methods (3) 

        private void checkNewGroupStarted(bool newGroupStarted)
        {
            if (newGroupStarted)
            {
                _groupMin = 0;
                _groupRowNumber = 0;
            }
        }

        private void groupMin(double cellValue)
        {
            if (_groupRowNumber == 1)
            {
                _groupMin = cellValue;
            }
            else
            {
                if (_groupMin > cellValue)
                    _groupMin = cellValue;
            }
        }

        private void overallMin(double cellValue)
        {
            if (_overallRowNumber == 1)
            {
                _overallMin = cellValue;
            }
            else
            {
                if (_overallMin > cellValue)
                    _overallMin = cellValue;
            }
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

            var cellValue = Convert.ToDouble(list.First().CellData, CultureInfo.InvariantCulture);
            var min = cellValue;

            foreach (var item in list)
            {
                cellValue = Convert.ToDouble(item.CellData.PropertyValue, CultureInfo.InvariantCulture);
                if (cellValue < min)
                    min = cellValue;
            }

            return min;
        }

        #endregion Methods
    }
}
