using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Aggregates.Numbers
{
    /// <summary>
    /// Variance function class.
    /// It only works with numbers. If you want to apply it on other data types, you need to create your own AggregateFunction by implementing the IAggregateFunc.
    /// </summary>
    public class Variance : IAggregateFunction
    {
        /// <summary>
        /// Fires before rendering of this cell.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public Func<object, string> DisplayFormatFormula { set; get; }

        #region Fields (8)

        int _groupRowDataNumber;
        readonly IList<double> _groupRowValues = new List<double>();
        double _groupSum;
        double _groupVariance;
        int _overallRowDataNumber;
        readonly IList<double> _overallRowValues = new List<double>();
        double _overallSum;
        double _overallVariance;

        #endregion Fields

        #region Properties (2)

        /// <summary>
        /// Returns current groups' aggregate value.
        /// </summary>
        public object GroupValue
        {
            get { return _groupVariance; }
        }

        /// <summary>
        /// Returns current row's aggregate value without considering the presence of the groups.
        /// </summary>
        public object OverallValue
        {
            get { return _overallVariance; }
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

            _overallRowDataNumber++;
            _groupRowDataNumber++;

            double cellValue = Convert.ToDouble(cellDataValue, CultureInfo.InvariantCulture);
            overallVariance(cellValue);
            groupVariance(cellValue);
        }
        // Private Methods (3) 

        private void checkNewGroupStarted(bool newGroupStarted)
        {
            if (newGroupStarted)
            {
                _groupRowDataNumber = 0;
                _groupSum = 0;
                _groupRowValues.Clear();
                _groupVariance = 0;
            }
        }

        private void groupVariance(double cellValue)
        {
            _groupRowValues.Add(cellValue);

            _groupSum += cellValue;
            var mean = (_groupSum / _groupRowDataNumber);

            double variance = 0;
            foreach (var item in _groupRowValues)
            {
                variance = variance + Math.Pow((item - mean), 2);
            }

            _groupVariance = variance / _groupRowDataNumber;
        }

        private void overallVariance(double cellValue)
        {
            _overallRowValues.Add(cellValue);

            _overallSum += cellValue;
            var mean = (_overallSum / _overallRowDataNumber);

            double variance = 0;
            foreach (var item in _overallRowValues)
            {
                variance = variance + Math.Pow((item - mean), 2);
            }

            _overallVariance = variance / _overallRowDataNumber;
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

            double sum = 0;
            foreach (var item in list)
            {
                double cellValue = Convert.ToDouble(item.CellData.PropertyValue, CultureInfo.InvariantCulture);
                sum += cellValue;
            }

            var mean = sum / count;

            double variance = 0;
            foreach (var item in list)
            {
                double cellValue = Convert.ToDouble(item.CellData.PropertyValue, CultureInfo.InvariantCulture);
                variance = variance + Math.Pow((cellValue - mean), 2);
            }

            return variance / count;
        }

        #endregion Methods
    }
}
