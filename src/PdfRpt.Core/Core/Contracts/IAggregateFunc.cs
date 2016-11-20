using System;
using System.Collections.Generic;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Aggregate functions contract
    /// </summary>
    public interface IAggregateFunction
    {
        /// <summary>
        /// Fires before rendering of this cell.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        Func<object, string> DisplayFormatFormula { set; get; }

        /// <summary>
        /// Returns current groups' aggregate value.
        /// </summary>
        object GroupValue { get; }

        /// <summary>
        /// Returns current row's aggregate value without considering the presence of the groups.
        /// </summary>
        object OverallValue { get; }

        /// <summary>
        /// Fires after adding a cell to the main table.
        /// </summary>
        /// <param name="cellDataValue">Current cell's data</param>
        /// <param name="isNewGroupStarted">Indicates starting a new group</param>
        void CellAdded(object cellDataValue, bool isNewGroupStarted);

        /// <summary>
        /// A general method which takes a list of data and calculates its corresponding aggregate value.
        /// It will be used to calculate the aggregate values of each pages individually, without considering the previous pages data.
        /// </summary>
        /// <param name="columnCellsSummaryData">List of data</param>
        /// <returns>Aggregate value</returns>
        object ProcessingBoundary(IList<SummaryCellData> columnCellsSummaryData);
    }
}
