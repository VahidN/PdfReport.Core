using System;
using System.Collections.Generic;
using PdfRpt.Aggregates.Numbers;
using PdfRpt.Core.Contracts;

namespace PdfRpt
{
    /// <summary>
    /// Predefined aggregate functions provider
    /// </summary>
    public class AggregateProvider : IAggregateFunction
    {
        /// <summary>
        /// Fires before rendering of this cell.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public Func<object, string> DisplayFormatFormula { set; get; }


        /// <summary>
        /// Column's AggregateFunction
        /// </summary>
        public IAggregateFunction ColumnAggregateFunction { get; private set; }
        
        /// <summary>
        /// Predefined aggregate functions provider
        /// </summary>
        /// <param name="aggregateFunction">A set of a predefined aggregate functions.</param>
        public AggregateProvider(AggregateFunction aggregateFunction)
        {
            initializeFunction(aggregateFunction);
        }

        private void initializeFunction(AggregateFunction aggregateFunction)
        {
            switch (aggregateFunction)
            {
                case AggregateFunction.Average:
                    ColumnAggregateFunction = new Average { DisplayFormatFormula = DisplayFormatFormula };
                    break;
                case AggregateFunction.Maximum:
                    ColumnAggregateFunction = new Maximum { DisplayFormatFormula = DisplayFormatFormula };
                    break;
                case AggregateFunction.Minimum:
                    ColumnAggregateFunction = new Minimum { DisplayFormatFormula = DisplayFormatFormula };
                    break;
                case AggregateFunction.StdDev:
                    ColumnAggregateFunction = new StdDev { DisplayFormatFormula = DisplayFormatFormula };
                    break;
                case AggregateFunction.Sum:
                    ColumnAggregateFunction = new Sum { DisplayFormatFormula = DisplayFormatFormula };
                    break;
                case AggregateFunction.Variance:
                    ColumnAggregateFunction = new Variance { DisplayFormatFormula = DisplayFormatFormula };
                    break;
                case AggregateFunction.Empty:
                    ColumnAggregateFunction = new Empty { DisplayFormatFormula = DisplayFormatFormula };
                    break;
                default:
                    throw new NotSupportedException("Please select a defined IAggregateFunction.");
            }
        }

        /// <summary>
        /// Returns current groups' aggregate value.
        /// </summary>
        public object GroupValue
        {
            get { return ColumnAggregateFunction.GroupValue; }
        }

        /// <summary>
        /// Returns current row's aggregate value without considering the presence of the groups.
        /// </summary>
        public object OverallValue
        {
            get { return ColumnAggregateFunction.OverallValue; }
        }

        /// <summary>
        /// Fires after adding a cell to the main table.
        /// </summary>
        /// <param name="cellDataValue">Current cell's data</param>
        /// <param name="isNewGroupStarted">Indicated starting a new group</param>
        public void CellAdded(object cellDataValue, bool isNewGroupStarted)
        {
            ColumnAggregateFunction.CellAdded(cellDataValue, isNewGroupStarted);
        }

        /// <summary>
        /// A general method which takes a list of data and calculates its corresponding aggregate value.
        /// It will be used to calculate the aggregate values of each pages individually, without considering the previous pages data.
        /// </summary>
        /// <param name="columnCellsSummaryData">List of data</param>
        /// <returns>Aggregate value</returns>
        public object ProcessingBoundary(IList<SummaryCellData> columnCellsSummaryData)
        {
            return ColumnAggregateFunction.ProcessingBoundary(columnCellsSummaryData);
        }
    }
}
