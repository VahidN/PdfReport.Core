namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Represents a main table's cell type.
    /// </summary>
    public enum CellType
    {
        /// <summary>
        /// Undefined value
        /// </summary>
        None,

        /// <summary>
        /// Represents a main table's cell which holds the SummaryRowCell of the previous page.
        /// </summary>
        PreviousPageSummaryCell,

        /// <summary>
        /// Represents a main table's summary cell. It's not different for each page and will be calculated based on the previous pages data.
        /// </summary>
        SummaryRowCell,

        /// <summary>
        /// Represents a main table's automatically generated row number cell.
        /// </summary>
        RowNumberCell,

        /// <summary>
        /// Represents a main table's data cell.
        /// </summary>
        DataTableCell,

        /// <summary>
        /// Represents a main table's header's cell.
        /// </summary>
        HeaderCell,

        /// <summary>
        /// Represents a main table's summary cell. It's diffent for each page.
        /// </summary>
        PageSummaryCell
    }
}
