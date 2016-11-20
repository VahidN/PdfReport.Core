
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// A class to hold summary info of the main table's rows and cells.
    /// </summary>
    public class SummaryCellData
    {
        /// <summary>
        /// Row number of the current row without considering the presence of the different groups
        /// </summary>
        public int OverallRowNumber { set; get; }

        /// <summary>
        /// Row number of the current row in its group
        /// </summary>
        public int GroupRowNumber { set; get; }

        /// <summary>
        /// Current row's group number
        /// </summary>
        public int GroupNumber { set; get; }

        /// <summary>
        /// Current cell's raw data
        /// </summary>
        public CellData CellData { set; get; }

        /// <summary>
        /// Aggregate value of the current row and cell without considering the presence of the different groups
        /// </summary>
        public object OverallAggregateValue { set; get; }

        /// <summary>
        /// Aggregate value of the current row and cell in its group
        /// </summary>
        public object GroupAggregateValue { set; get; }
    }
}