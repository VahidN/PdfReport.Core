
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Holds MainTable's general properties
    /// </summary>
    public class MainTablePreferences
    {
        /// <summary>
        /// Width percentage of the main table.
        /// </summary>
        public float WidthPercentage { set; get; }

        /// <summary>
        /// Determines the WidthType of the columns.
        /// </summary>
        public TableColumnWidthType ColumnsWidthsType { set; get; }

        /// <summary>
        /// Sets the visibility of the main table's header row.
        /// </summary>
        public bool ShowHeaderRow { set; get; }

        /// <summary>
        /// Split the cells of the first row that doesn't fit the page.
        /// If true, a that row doesn't fit on the page, its complete row will be forwarded to the next page.
        /// If false, rows that are too high to fit on a page will be dropped from the table.
        /// </summary>
        public bool SplitLate { set; get; }

        /// <summary>
        /// If true, splits rows that are forwarded to the next page but that still
        /// don't fit because the row height exceeds the available page height.
        /// </summary>
        public bool SplitRows { set; get; }

        /// <summary>
        /// If true, the table will be kept on one page if it fits, by forcing a
        /// new page if it doesn't fit on the current page.
        /// </summary>
        public bool KeepTogether { set; get; }

        /// <summary>
        /// Spacing before the main table.
        /// </summary>
        public float SpacingBefore { set; get; }

        /// <summary>
        /// Spacing after the main table.
        /// </summary>
        public float SpacingAfter { set; get; }

        /// <summary>
        /// If sets to zero, NumberOfDataRowsPerPage will be calculated automatically, otherwise as specified.
        /// </summary>
        public int NumberOfDataRowsPerPage { set; get; }

        /// <summary>
        /// Sets the TableType. Its default value is a noraml PdfGrid.
        /// </summary>
        public TableType TableType { set; get; }

        /// <summary>
        /// If TableType is set to HorizontalStackPanel, here you can define its preferences such as
        /// number of columns per row.
        /// </summary>
        public HorizontalStackPanelPreferences HorizontalStackPanelPreferences { set; get; }
    }
}
