using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// MainTable Preferences Builder Class.
    /// </summary>
    public class MainTablePreferencesBuilder
    {
        readonly PdfReport _pdfReport;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public MainTablePreferencesBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
        }

        /// <summary>
        /// Sets the visibility of the main table's header row.
        /// It's true by default.
        /// </summary>
        /// <param name="show">show header row</param>
        public void ShowHeaderRow(bool show)
        {
            _pdfReport.DataBuilder.DefaultShowHeaderRow(show);
        }

        /// <summary>
        /// Determines the WidthType of the column.
        /// </summary>
        /// <param name="widthType"></param>
        public void ColumnsWidthsType(TableColumnWidthType widthType)
        {
            _pdfReport.DataBuilder.DefaultColumnsWidthsType(widthType);
        }

        /// <summary>
        /// If sets to zero, NumberOfDataRowsPerPage will be calculated automatically, otherwise as specified.
        /// Its default value is 0.
        /// </summary>
        /// <param name="rowsPerPage">number of data rows per page</param>
        public void NumberOfDataRowsPerPage(int rowsPerPage)
        {
            _pdfReport.DataBuilder.DefaultNumberOfDataRowsPerPage(rowsPerPage);
        }

        /// <summary>
        /// Split the cells of the first row that doesn't fit the page.
        /// If true, a that row doesn't fit on the page, its complete row will be forwarded to the next page.
        /// If false, rows that are too high to fit on a page will be dropped from the table.
        /// Its default value is true.
        /// </summary>
        public void SplitLate(bool split)
        {
            _pdfReport.DataBuilder.DefaultSplitLate(split);
        }

        /// <summary>
        /// If true, splits rows that are forwarded to the next page but that still
        /// don't fit because the row height exceeds the available page height.
        /// Its default value is true.
        /// </summary>
        public void SplitRows(bool split)
        {
            _pdfReport.DataBuilder.DefaultSplitRows(split);
        }

        /// <summary>
        /// If true, the table will be kept on one page if it fits, by forcing a
        /// new page if it doesn't fit on the current page.
        /// </summary>
        public void KeepTogether(bool keep)
        {
            _pdfReport.DataBuilder.DefaultKeepTogether(keep);
        }

        /// <summary>
        /// Spacing before the main table.
        /// </summary>
        public void SpacingBefore(float spacing)
        {
            _pdfReport.DataBuilder.DefaultSpacingBefore(spacing);
        }

        /// <summary>
        /// Spacing after the main table.
        /// </summary>
        public void SpacingAfter(float spacing)
        {
            _pdfReport.DataBuilder.DefaultSpacingAfter(spacing);
        }

        /// <summary>
        /// Wrapping main table in multiple columns per pages.
        /// </summary>
        /// <param name="multipleColumns">multiple columns per page</param>
        public void MultipleColumnsPerPage(MultipleColumnsPerPage multipleColumns)
        {
            _pdfReport.DataBuilder.DefaultMultipleColumnsPerPage(multipleColumns);
        }

        /// <summary>
        /// Groups Preferences.
        /// </summary>
        /// <param name="preferences">Groups Preferences</param>
        public void GroupsPreferences(GroupsPreferences preferences)
        {
            _pdfReport.DataBuilder.DefaultGroupsPreferences(preferences);
        }

        /// <summary>
        /// Sets the TableType. Its default value is a noraml PdfGrid.
        /// </summary>
        /// <param name="tableType">Value of the TableType</param>
        public void MainTableType(TableType tableType)
        {
            _pdfReport.DataBuilder.DefaultTableType(tableType);
        }

        /// <summary>
        /// If MainTableType is set to HorizontalStackPanel, here you can define its preferences such as
        /// number of columns per row.
        /// Please note that All columns and properties of an object will create a single cell here.
        /// </summary>
        /// <param name="columnsPerRow">number of columns per row</param>
        public void HorizontalStackPanelPreferences(int columnsPerRow)
        {
            var data = new HorizontalStackPanelPreferences
            {
                ColumnsPerRow = columnsPerRow
            };
            _pdfReport.DataBuilder.DefaultHorizontalStackPanelPreferences(data);
        }
    }
}