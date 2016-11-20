using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// Helps rendering of different rows of the MainTable
    /// </summary>
    public class RowsManager
    {
        IList<CellData> _previousTableRowData = new List<CellData>();

        #region Properties (6)

        /// <summary>
        /// Holds shared info between rendering classes.
        /// </summary>
        public SharedData SharedData { set; get; }

        /// <summary>
        /// Holds last result of the actual rendering engine of iTextSharp during its processes.
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// Groups rendering helper class
        /// </summary>
        public GroupsManager GroupsManager { set; get; }

        /// <summary>
        /// It will be the container of all groups' tables to enable wrapping them in multiple columns
        /// </summary>
        public PdfGrid MainGroupTable { set; get; }

        /// <summary>
        /// MainTable of the PdfRpt
        /// </summary>
        public PdfGrid MainTable { set; get; }

        /// <summary>
        /// Helps rendering main table's cells
        /// </summary>
        public TableCellHelper TableCellHelper { set; get; }

        #endregion Properties

        #region Methods (9)

        // Public Methods (4)


        int getFirstDefinedAggregateCell()
        {
            int idx;
            for (idx = 0; idx < SharedData.PdfColumnsAttributes.Count; idx++)
            {
                if (SharedData.PdfColumnsAttributes[idx].AggregateFunction != null)
                    break;
            }
            if (idx == 0) return 0;
            return idx - 1;
        }

        /// <summary>
        /// Adds a footer/summary row
        /// </summary>
        /// <param name="pdfRowType"></param>
        public void AddFooterRow(RowType pdfRowType)
        {
            if (SharedData.SummarySettings == null || SharedData.SummarySettings.OverallSummarySettings == null ||
                !SharedData.SummarySettings.OverallSummarySettings.ShowOnEachPage) return;

            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.RowStarted(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = pdfRowType, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

            for (var columnNumber = 0; columnNumber < SharedData.ColumnsCount; columnNumber++)
            {
                var backgroundColor = SharedData.Template.SummaryRowBackgroundColor;
                var foreColor = SharedData.Template.SummaryRowFontColor;
                var col = SharedData.PdfColumnsAttributes[columnNumber];

                int location = -1;
                switch (SharedData.SummarySettings.OverallSummarySettings.SummaryLocation)
                {
                    case SummaryLocation.AtFirstDefinedAggregateCell:
                        if (columnNumber == getFirstDefinedAggregateCell())
                            location = columnNumber;
                        break;
                    case SummaryLocation.AtRowNumberColumn:
                        if (columnNumber == 0)
                            location = 0;
                        break;
                    case SummaryLocation.AtSpecifiedLabelColumnProperty:
                        if (SharedData.SummarySettings.OverallSummarySettings.LabelColumnProperty == col.PropertyName)
                            location = columnNumber;
                        break;
                }

                if (location != -1)
                    TableCellHelper.AddSummaryCell(backgroundColor[0], foreColor, location, CellType.SummaryRowCell, pdfRowType);
                else
                    TableCellHelper.AddSummaryCell(backgroundColor[0], foreColor, null, columnNumber, pdfRowType, CellType.SummaryRowCell);
            }

            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.RowAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = pdfRowType, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
        }

        ExporterManager _exporterManager;
        bool _dataSourceIsEmpty;
        bool _setNumberOfRowsPerPageNeedsNewPage;

        /// <summary>
        /// Adds all main data rows of the MainTable
        /// </summary>
        public void AddMainTableRows()
        {

            if (SharedData.MainTableDataSource == null || !SharedData.MainTableDataSource.Rows().Any())
            {
                _dataSourceIsEmpty = true;
                if (SharedData.MainTableEvents != null)
                    SharedData.MainTableEvents.DataSourceIsEmpty(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
                return;
            }

            _exporterManager = new ExporterManager(SharedData);
            _exporterManager.InitExporter();
            printNormalTable();
            printHorizontalStackPanel();
            _exporterManager.CloseExporter();
        }

        private void printHorizontalStackPanel()
        {
            if (SharedData.PageSetup.MainTablePreferences.TableType != TableType.HorizontalStackPanel)
                return;

            var hasTableRowNumberColumn = SharedData.HasTableRowNumberColumn;
            var columnsNumber = SharedData.HorizontalStackPanelColumnsPerRow;
            if (hasTableRowNumberColumn)
                columnsNumber--;

            var itemsToTake = columnsNumber;
            var customRow = new List<CellData>();

            foreach (var row in SharedData.MainTableDataSource.Rows())
            {
                if (row == null) continue;

                var rowObjectColumnsCount = row.Count;
                var tempRow = row.Take(itemsToTake).ToList();
                var diff = (columnsNumber * rowObjectColumnsCount) - customRow.Count - tempRow.Count;
                bool rowIsReady;
                if (diff == 0)
                {
                    rowIsReady = true;
                    itemsToTake = columnsNumber;
                }
                else
                {
                    itemsToTake = diff;
                    rowIsReady = false;
                }
                customRow.AddRange(tempRow);

                if (!rowIsReady) continue;
                var index = hasTableRowNumberColumn ? 1 : 0;
                for (int i = 0; i < customRow.Count; i++)
                {
                    if ((i > 0) && (i % rowObjectColumnsCount == 0))
                        index++; // All columns of an object will create a single cell here.
                    customRow[i].PropertyIndex = index;
                }

                if (shouldSkipRow(customRow)) continue;
                fireRowStartedInjectCustomRowsEvent(customRow);
                addSingleRow(customRow);
                fireRowAddedInjectCustomRowsEvent(customRow);

                customRow.Clear();
            }
        }

        private void printNormalTable()
        {
            if (SharedData.PageSetup.MainTablePreferences.TableType != TableType.NormalTable)
                return;

            foreach (var row in SharedData.MainTableDataSource.Rows())
            {
                if (row == null) continue;
                if(shouldSkipRow(row)) continue;
                fireRowStartedInjectCustomRowsEvent(row);
                addSingleRow(row);
                fireRowAddedInjectCustomRowsEvent(row);
            }
        }

        private bool shouldSkipRow(IList<CellData> row)
        {
            return SharedData.MainTableEvents != null &&
                SharedData.MainTableEvents.ShouldSkipRow(new EventsArguments
                {
                    PdfDoc = SharedData.PdfDoc,
                    PdfWriter = SharedData.PdfWriter,
                    Table = MainTable,
                    RowType = RowType.DataTableRow,
                    TableRowData = row,
                    ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData,
                    PreviousTableRowData = _previousTableRowData,
                    PageSetup = SharedData.PageSetup,
                    PdfFont = SharedData.PdfFont,
                    PdfColumnsAttributes = SharedData.PdfColumnsAttributes
                });
        }

        private void addSingleRow(IList<CellData> row)
        {
            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.RowStarted(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.DataTableRow, TableRowData = row, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

            var rowColors = applyRowSettings(row);
            addTableRow(row, rowColors[0], rowColors[1]);

            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.RowAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.DataTableRow, TableRowData = row, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

            _setNumberOfRowsPerPageNeedsNewPage = setNumberOfRowsPerPage();
            _previousTableRowData = row;
            firePageTableAdded(row);
        }

        private void firePageTableAdded(IList<CellData> row)
        {
            if (!_setNumberOfRowsPerPageNeedsNewPage)
                return;

            var args = new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.DataTableRow, TableRowData = row, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes };
            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.PageTableAdded(args);
        }

        private void fireRowStartedInjectCustomRowsEvent(IList<CellData> row)
        {
            if (SharedData.MainTableEvents == null) return;
            if (SharedData.MainTableEvents.RowStartedInjectCustomRows == null) return;

            var args = new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.DataTableRow, TableRowData = row, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes };
            var customRows = SharedData.MainTableEvents.RowStartedInjectCustomRows(args);
            if (customRows == null || !customRows.Any()) return;

            foreach (var customRow in customRows)
            {
                if (customRow == null) continue;
                addSingleRow(customRow);
            }
        }

        private void fireRowAddedInjectCustomRowsEvent(IList<CellData> row)
        {
            if (SharedData.MainTableEvents == null) return;
            if (SharedData.MainTableEvents.RowAddedInjectCustomRows == null) return;

            var args = new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.DataTableRow, TableRowData = row, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes };
            var customRows = SharedData.MainTableEvents.RowAddedInjectCustomRows(args);
            if (customRows == null || !customRows.Any()) return;

            foreach (var customRow in customRows)
            {
                if (customRow == null) continue;
                addSingleRow(customRow);
            }
        }

        private IList<BaseColor> applyRowSettings(IList<CellData> row)
        {
            if (_setNumberOfRowsPerPageNeedsNewPage) SharedData.PdfDoc.NewPage();

            var backColor = CurrentRowInfoData.LastOverallDataRowNumber % 2 != 0 ? SharedData.Template.RowBackgroundColor : SharedData.Template.AlternatingRowBackgroundColor;
            var foreColor = CurrentRowInfoData.LastOverallDataRowNumber % 2 != 0 ? SharedData.Template.RowFontColor : SharedData.Template.AlternatingRowFontColor;
            CurrentRowInfoData.LastOverallDataRowNumber++;
            CurrentRowInfoData.LastGroupRowNumber++;

            if (GroupsManager.ShouldStartNewGroup(row))
            {
                GroupsManager.StartNewGroup(row, !_setNumberOfRowsPerPageNeedsNewPage);
            }
            else
            {
                if (CurrentRowInfoData.LastOverallDataRowNumber == 1)
                    TableInitAddHeaderAndFooter();
            }

            return new List<BaseColor> { backColor, foreColor };
        }

        private bool setNumberOfRowsPerPage()
        {
            var num = SharedData.PageSetup.MainTablePreferences.NumberOfDataRowsPerPage;
            if (num == 0)
                return false;

            var rowsCount = MainTable.Rows.Count;
            if (rowsCount == 0) return false;

            var rowNumber = CurrentRowInfoData.LastOverallDataRowNumber;
            if ((rowNumber > 0) && (rowNumber % num == 0))
            {
                tryFitToContent();
                SharedData.PdfDoc.Add(MainTable);
                if (MainTable.Rows.Count > 0) MainTable.DeleteBodyRows();
                MainTable.SkipFirstHeader = false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Finalizing the MainTable's rendering
        /// </summary>
        public void CloseMainTable()
        {
            if (_dataSourceIsEmpty)
            {
                if (SharedData.MainTableEvents != null)
                    SharedData.MainTableEvents.MainTableAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
                return;
            }

            MainTable.ElementComplete = true; //print the last footer
            if (SharedData.ShouldWrapTablesInColumns)
            {
                MainGroupTable.AddCell(new PdfPCell(MainTable) { Border = 0 });
                MainGroupTable.AddCell(new PdfPCell(TableHelper.CreateEmptyRowTable(fixedHeight: 30)) { Border = 0 });

                GroupsManager.AddAllGroupsSummary();

                new TablesInColumns
                {
                    PdfDoc = SharedData.PdfDoc,
                    PdfWriter = SharedData.PdfWriter,
                    PageSetup = SharedData.PageSetup,
                    CurrentRowInfoData = CurrentRowInfoData
                }.Wrap(MainGroupTable);
            }
            else
            {
                MainTable.SpacingAfter += MainTable.HeaderHeight + 2.5f;
                tryFitToContent();
                SharedData.PdfDoc.Add(MainTable);

                var lastRow = SharedData.ColumnCellsSummaryData.OrderByDescending(x => x.OverallRowNumber).FirstOrDefault();
                if (lastRow != null)
                    CurrentRowInfoData.LastRenderedRowNumber = lastRow.OverallRowNumber;

                GroupsManager.AddAllGroupsSummary();
            }

            if (MainTable.Rows.Count > 0) MainTable.DeleteBodyRows();
            mainTableAdded();
        }

        private void mainTableAdded()
        {
            if (SharedData.MainTableEvents == null)
                return;

            MainTable.SpacingBefore = MainTable.FooterHeight - MainTable.SpacingAfter;
            MainTable.Rows.Clear();
            MainTable.HeaderRows = 0;
            MainTable.FooterRows = 0;
            if (SharedData.IsGroupingEnabled)
            {
                SharedData.MainTableEvents.GroupAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = CurrentRowInfoData.PreviousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
            }
            SharedData.MainTableEvents.MainTableAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
        }

        private void tryFitToContent()
        {
            if (SharedData.PageSetup.MainTablePreferences.ColumnsWidthsType != TableColumnWidthType.FitToContent) return;
            MainTable.AutoResizeTableColumns();
        }

        /// <summary>
        /// Adds headers and footers rows of the MainTable
        /// </summary>
        public void TableInitAddHeaderAndFooter()
        {
            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.MainTableCreated(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

            addExtraHeaderRows(name: "for HeaderRows > 0");
            addMainHeaderRow(name: "for HeaderRows > 0");

            addSummaryRemainingRow();
            addPageSummary();
            AddFooterRow(RowType.SummaryRow);

            addExtraHeaderRows(name: "for SkipFirstHeader = true");
            addMainHeaderRow(name: "for SkipFirstHeader = true");
        }
        // Private Methods

        int getColSpan(int startFrom)
        {
            int initColSpan = 1;
            for (var column = startFrom; column < SharedData.ColumnsCount - 1; column++)
            {
                if (SharedData.PdfColumnsAttributes[column].HeaderCell.MergeHeaderCell)
                    initColSpan++;
                else
                    break;
            }
            return initColSpan;
        }

        private void addMainHeaderRow(string name)
        {
            if (shouldRepeatGroupHeader(name)) return;
            if (!SharedData.PageSetup.MainTablePreferences.ShowHeaderRow) return;
            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.RowStarted(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.HeaderRow, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

            var column = 0;
            while (column < SharedData.ColumnsCount)
            {
                int initColSpan = 1;
                if (SharedData.PdfColumnsAttributes[column].HeaderCell.MergeHeaderCell)
                {
                    initColSpan = getColSpan(column);
                }
                TableCellHelper.AddHeaderCell(column, initColSpan);
                column += initColSpan;
            }

            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.RowAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.HeaderRow, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
        }

        private void addExtraHeaderRows(string name)
        {
            if (SharedData.ExtraHeaderRows == 0) return;
            if (shouldRepeatGroupHeader(name)) return;
            if (!SharedData.PageSetup.MainTablePreferences.ShowHeaderRow) return;

            for (int i = 0; i < SharedData.ExtraHeaderRows; i++)
            {
                if (SharedData.MainTableEvents != null)
                    SharedData.MainTableEvents.RowStarted(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.HeaderRow, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

                var column = 0;
                while (column < SharedData.ColumnsCount)
                {
                    int initColSpan = 1;
                    var col = SharedData.PdfColumnsAttributes[column];
                    if (col.HeadingCells.Count < i)
                    {
                        TableCellHelper.AddExtraHeaderCell(column, initColSpan, new HeadingCell { Caption = string.Empty });
                    }
                    else
                    {
                        if (col.HeadingCells[i].MergeHeaderCell)
                        {
                            initColSpan = getExtraRowColSpan(i, column);
                        }
                        TableCellHelper.AddExtraHeaderCell(column, initColSpan, col.HeadingCells[i]);
                    }
                    column += initColSpan;
                }

                if (SharedData.MainTableEvents != null) SharedData.MainTableEvents.RowAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.HeaderRow, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
            }
        }

        private int getExtraRowColSpan(int i, int startFrom)
        {
            int initColSpan = 1;
            for (var column = startFrom; column < SharedData.ColumnsCount - 1; column++)
            {
                if (SharedData.PdfColumnsAttributes[column].HeadingCells[i].MergeHeaderCell)
                    initColSpan++;
                else
                    break;
            }
            return initColSpan;
        }

        private bool shouldRepeatGroupHeader(string name)
        {
            return SharedData.IsGroupingEnabled &&
                   (CurrentRowInfoData.LastGroupNumber > 0) &&
                   (name == "for SkipFirstHeader = true") &&
                   (SharedData.PageSetup.GroupsPreferences != null) &&
                   (!SharedData.PageSetup.GroupsPreferences.RepeatHeaderRowPerGroup);
        }

        private void addPageSummary()
        {
            if (SharedData.SummarySettings == null || SharedData.SummarySettings.PageSummarySettings == null ||
                !SharedData.SummarySettings.PageSummarySettings.ShowOnEachPage) return;
            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.RowStarted(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.PageSummaryRow, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

            for (var columnNumber = 0; columnNumber < SharedData.ColumnsCount; columnNumber++)
            {
                var backgroundColor = SharedData.Template.PageSummaryRowBackgroundColor;
                var foreColor = SharedData.Template.PageSummaryRowFontColor;
                var col = SharedData.PdfColumnsAttributes[columnNumber];

                int location = -1;
                switch (SharedData.SummarySettings.PageSummarySettings.SummaryLocation)
                {
                    case SummaryLocation.AtFirstDefinedAggregateCell:
                        if (columnNumber == getFirstDefinedAggregateCell())
                            location = columnNumber;
                        break;
                    case SummaryLocation.AtRowNumberColumn:
                        if (columnNumber == 0)
                            location = 0;
                        break;
                    case SummaryLocation.AtSpecifiedLabelColumnProperty:
                        if (SharedData.SummarySettings.PageSummarySettings.LabelColumnProperty == col.PropertyName)
                            location = columnNumber;
                        break;
                }

                if (location != -1)
                    TableCellHelper.AddSummaryCell(backgroundColor[0], foreColor, location, CellType.PageSummaryCell, RowType.PageSummaryRow);
                else
                    TableCellHelper.AddSummaryCell(backgroundColor[0], foreColor, null, columnNumber, RowType.PageSummaryRow, CellType.PageSummaryCell);
            }

            if (SharedData.MainTableEvents != null) SharedData.MainTableEvents.RowAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.PageSummaryRow, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
        }


        private void addSummaryRemainingRow()
        {
            if (SharedData.SummarySettings == null || SharedData.SummarySettings.PreviousPageSummarySettings == null ||
                !SharedData.SummarySettings.PreviousPageSummarySettings.ShowOnEachPage) return;
            if (SharedData.MainTableEvents != null)
                SharedData.MainTableEvents.RowStarted(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.PreviousPageSummaryRow, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

            for (var columnNumber = 0; columnNumber < SharedData.ColumnsCount; columnNumber++)
            {
                var backgroundColor = SharedData.Template.PreviousPageSummaryRowBackgroundColor;
                var foreColor = SharedData.Template.PreviousPageSummaryRowFontColor;
                var col = SharedData.PdfColumnsAttributes[columnNumber];

                int location = -1;
                switch (SharedData.SummarySettings.PreviousPageSummarySettings.SummaryLocation)
                {
                    case SummaryLocation.AtFirstDefinedAggregateCell:
                        if (columnNumber == getFirstDefinedAggregateCell())
                            location = columnNumber;
                        break;
                    case SummaryLocation.AtRowNumberColumn:
                        if (columnNumber == 0)
                            location = 0;
                        break;
                    case SummaryLocation.AtSpecifiedLabelColumnProperty:
                        if (SharedData.SummarySettings.PreviousPageSummarySettings.LabelColumnProperty == col.PropertyName)
                            location = columnNumber;
                        break;
                }

                if (location != -1)
                    TableCellHelper.AddPreviousPageSummaryCell(backgroundColor[0], foreColor, location);
                else
                    TableCellHelper.AddSummaryCell(backgroundColor[0], foreColor, null, columnNumber, RowType.PreviousPageSummaryRow, CellType.PreviousPageSummaryCell);
            }

            if (SharedData.MainTableEvents != null) SharedData.MainTableEvents.RowAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainTable, RowType = RowType.PreviousPageSummaryRow, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = _previousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
        }

        private void addTableRow(IList<CellData> rowData, BaseColor backgroundColor, BaseColor foreColor)
        {
            var finalRowDataList = new List<CellData>();
            for (var columnNumber = 0; columnNumber < SharedData.ColumnsCount; columnNumber++)
            {
                var col = SharedData.PdfColumnsAttributes[columnNumber];
                CellAttributes cell;
                if (col.IsRowNumber)
                {
                    cell = TableCellHelper.AddRowNumberCell(backgroundColor, foreColor, columnNumber);
                }
                else
                {
                    GroupsManager.ModifyRowData(rowData);
                    cell = TableCellHelper.AddRowCell(rowData, backgroundColor, foreColor, columnNumber);
                    updateAggregates(col, cell);
                }

                finalRowDataList.Add(new CellData
                {
                    PropertyName = col.PropertyName,
                    PropertyValue = cell.RowData.Value,
                    FormattedValue = cell.RowData.FormattedValue,
                    PropertyType = cell.RowData.PropertyType
                });
            }

            _exporterManager.ApplyExporter(finalRowDataList, CurrentRowInfoData);
            CurrentRowInfoData.IsNewGroupStarted = false;
            CurrentRowInfoData.PreviousTableRowData = rowData;
        }


        private void updateAggregates(ColumnAttributes col, CellAttributes cell)
        {
            if (cell == null || col.AggregateFunction == null) return;

            col.AggregateFunction.CellAdded(cell.RowData.Value, CurrentRowInfoData.IsNewGroupStarted);

            var columnRowSummary = new SummaryCellData
            {
                CellData = new CellData
                {
                    PropertyName = col.PropertyName,
                    PropertyValue = cell.RowData.Value,
                    PropertyType = cell.RowData.PropertyType
                },
                GroupAggregateValue = col.AggregateFunction.GroupValue,
                GroupRowNumber = CurrentRowInfoData.LastGroupRowNumber,
                OverallAggregateValue = col.AggregateFunction.OverallValue,
                OverallRowNumber = CurrentRowInfoData.LastOverallDataRowNumber,
                GroupNumber = CurrentRowInfoData.LastGroupNumber
            };
            SharedData.ColumnCellsSummaryData.Add(columnRowSummary);
        }

        #endregion Methods
    }
}
