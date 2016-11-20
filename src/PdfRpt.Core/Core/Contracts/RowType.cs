namespace PdfRpt.Core.Contracts
{    
    /// <summary>
    /// Main table's row types
    /// </summary>
    public enum RowType
    {
        /// <summary>
        /// Undefined value.
        /// </summary>
        None,

        /// <summary>
        /// Represents a main table's previous page summary row
        /// </summary>
        PreviousPageSummaryRow,

        /// <summary>
        /// Represents a main table's summary row
        /// </summary>
        SummaryRow,
        
        /// <summary>
        /// Represents a main table's datatable row
        /// </summary>
        DataTableRow,

        /// <summary>
        /// Represents a main table's header row
        /// </summary>
        HeaderRow,

        /// <summary>
        /// Represents a main table's header row
        /// </summary>
        MainHeaderRow,

        /// <summary>
        /// Represents a main table's all groups summary row
        /// </summary>
        AllGroupsSummaryRow,

        /// <summary>
        /// Represents a main table's page summary row
        /// </summary>
        PageSummaryRow
    }
}
