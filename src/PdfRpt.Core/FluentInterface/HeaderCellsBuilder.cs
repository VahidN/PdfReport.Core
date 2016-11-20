using System.Collections.Generic;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Header Cells Builder Class.
    /// </summary>
    public class HeaderCellsBuilder
    {
        readonly List<CellAttributes> _headerCells = new List<CellAttributes>();

        /// <summary>
        /// Current Row's Data.
        /// </summary>
        public IList<CellData> RowData { set; get; }

        /// <summary>
        /// List of Summary Data.
        /// </summary>
        public IList<SummaryCellData> SummaryData { set; get; }

        /// <summary>
        /// List of Header Cells.
        /// </summary>
        internal List<CellAttributes> HeaderCells
        {
            get { return _headerCells; }
        }

        /// <summary>
        /// Adds a new cell
        /// </summary>
        /// <param name="cell">cell attributes</param>
        public void AddCell(CellAttributes cell)
        {
            _headerCells.Add(cell);
        }
    }
}
