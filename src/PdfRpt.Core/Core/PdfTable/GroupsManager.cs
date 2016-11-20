using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// Groups rendering helper class
    /// </summary>
    public class GroupsManager
    {
        #region Fields

        private readonly IDictionary<string, object> _currentRowValues;
        private int _groupNumber;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Groups rendering helper class
        /// </summary>
        public GroupsManager()
        {
            _currentRowValues = new Dictionary<string, object>();
        }

        #endregion Constructors

        #region Properties (7)

        /// <summary>
        /// Holds shared info between rendering classes.
        /// </summary>
        public SharedData SharedData { set; get; }

        /// <summary>
        /// Holds last result of the actual rendering engine of iTextSharp during its processes.
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// Initializes the MainTable's settings
        /// </summary>
        public InitTable InitTable { set; get; }

        /// <summary>
        /// It will be the container of all groups' tables to enable wrapping them in multiple columns
        /// </summary>
        public PdfGrid MainGroupTable { set; get; }

        /// <summary>
        /// Helps rendering of different rows of the MainTable
        /// </summary>
        public RowsManager RowsManager { set; get; }

        /// <summary>
        /// MainTable of the PdfRpt
        /// </summary>
        public PdfGrid MainTable { set; get; }

        /// <summary>
        /// Helps rendering MainTable's cells
        /// </summary>
        public TableCellHelper TableCellHelper { set; get; }

        #endregion Properties

        #region Methods (9)

        // Public Methods (3)

        /// <summary>
        /// Creates a new summary row table to display summary of the all of the available groups
        /// </summary>
        public void AddAllGroupsSummary()
        {
            if (!SharedData.IsGroupingEnabled) return;
            if (SharedData.SummarySettings == null) return;
            if (!SharedData.SummarySettings.OverallSummarySettings.ShowOnEachPage &&
                !SharedData.SummarySettings.PreviousPageSummarySettings.ShowOnEachPage) return;

            if (SharedData.PageSetup.PagePreferences.RunDirection == null)
                SharedData.PageSetup.PagePreferences.RunDirection = PdfRunDirection.LeftToRight;

            var mainTableAbsoluteWidths = MainTable.AbsoluteWidths;
            var len = SharedData.ColumnsWidths.Length;
            if (SharedData.IsMainTableHorizontalStackPanel)
            {
                len = SharedData.HorizontalStackPanelColumnsPerRow;
            }
            MainTable = new PdfGrid(len)
            {
                RunDirection = (int)SharedData.PageSetup.PagePreferences.RunDirection,
                WidthPercentage = SharedData.PageSetup.MainTablePreferences.WidthPercentage,
                HeadersInEvent = true,
                HeaderRows = 0,
                FooterRows = 1,
                SkipFirstHeader = true,
                SplitLate = SharedData.PageSetup.MainTablePreferences.SplitLate,
                SpacingAfter = spacingAfterAllGroupsSummary,
                SpacingBefore = spacingBeforeAllGroupsSummary,
                KeepTogether = SharedData.PageSetup.MainTablePreferences.KeepTogether,
                SplitRows = SharedData.PageSetup.MainTablePreferences.SplitRows
            };

            setSetTotalWidths(mainTableAbsoluteWidths);

            TableCellHelper = new TableCellHelper
                                  {
                                      SharedData = SharedData,
                                      MainTable = MainTable,
                                      ShowAllGroupsSummaryRow = true,
                                      CurrentRowInfoData = CurrentRowInfoData
                                  };

            RowsManager.MainTable = MainTable;
            RowsManager.TableCellHelper = TableCellHelper;

            RowsManager.AddFooterRow(RowType.AllGroupsSummaryRow);

            MainTable.ElementComplete = true; //print footer
            if (SharedData.ShouldWrapTablesInColumns)
            {
                MainGroupTable.AddCell(new PdfPCell(MainTable) { Border = 0 });
            }
            else
            {
                MainTable.SpacingAfter += MainTable.HeaderHeight + 2.5f;
                SharedData.PdfDoc.Add(MainTable);
            }
        }

        private void setSetTotalWidths(float[] mainTableAbsoluteWidths)
        {
            if (SharedData.PageSetup.MainTablePreferences.TableType != TableType.NormalTable)
                return;

            if (mainTableAbsoluteWidths.Any(x => x > 0))
            {
                MainTable.SetTotalWidth(mainTableAbsoluteWidths);
            }
            else
            {
                MainTable.SetWidths(SharedData.ColumnsWidths);
            }
        }

        private void tryFitToContent()
        {
            if (SharedData.PageSetup.MainTablePreferences.ColumnsWidthsType != TableColumnWidthType.FitToContent) return;
            MainTable.AutoResizeTableColumns();
        }

        /// <summary>
        /// Should we start a new group based on the current row's data?
        /// </summary>
        /// <param name="rowCellsData">Current row's data</param>
        /// <returns>true/false</returns>
        public bool ShouldStartNewGroup(IList<CellData> rowCellsData)
        {
            if (!SharedData.IsGroupingEnabled) return false;

            if (CurrentRowInfoData.LastOverallDataRowNumber == 1)
            {
                updateLastRowValues(rowCellsData);
                return true;
            }

            foreach (var property in SharedData.GroupByProperties)
            {
                var currentCellValue = getCurrentCellValue(rowCellsData, property);
                object lastRowCellValue;
                if (_currentRowValues.TryGetValue(property, out lastRowCellValue))
                {
                    if (!areEqual(lastRowCellValue, currentCellValue, property))
                    {
                        _currentRowValues.Clear();
                        updateLastRowValues(rowCellsData);
                        return true;
                    }
                }
                else
                {
                    _currentRowValues.Add(new KeyValuePair<string, object>(property, currentCellValue));
                }
            }

            return false;
        }

        /// <summary>
        /// Starts a new group by creating a new table and initializing its properties.
        /// </summary>
        /// <param name="groupHeaderRowCellsData">New group's header row data</param>
        /// <param name="shouldCheckOneGroupPerPage">Do we need a new page again?</param>
        public void StartNewGroup(IEnumerable<CellData> groupHeaderRowCellsData, bool shouldCheckOneGroupPerPage)
        {
            MainTable.ElementComplete = true; //print the last footer
            var hasRows = MainTable.Rows.Count > 0;
            if (hasRows)
            {
                if (SharedData.ShouldWrapTablesInColumns)
                {
                    MainGroupTable.AddCell(new PdfPCell(MainTable) { Border = 0 });
                }
                else
                {
                    MainTable.SpacingAfter += MainTable.HeaderHeight + 2.5f;
                    tryFitToContent();
                    SharedData.PdfDoc.Add(MainTable);
                    MainTable.DeleteBodyRows();
                }
            }

            if (SharedData.MainTableEvents != null && _groupNumber > 0)
                SharedData.MainTableEvents.GroupAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Table = MainGroupTable, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = CurrentRowInfoData.PreviousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

            _groupNumber++;
            if (shouldCheckOneGroupPerPage) showOneGroupPerPage();
            renderGroupHeader(groupHeaderRowCellsData);
            initMainTable();
            RowsManager.TableInitAddHeaderAndFooter();
            reset();
        }

        // Private Methods

        private void showOneGroupPerPage()
        {
            if (CurrentRowInfoData.LastGroupNumber == 0) return;

            if (SharedData.PageSetup.GroupsPreferences == null ||
                !SharedData.PageSetup.GroupsPreferences.ShowOneGroupPerPage) return;

            SharedData.PdfDoc.NewPage();
        }

        float spacingBeforeAllGroupsSummary
        {
            get
            {
                if (SharedData.PageSetup.GroupsPreferences == null) return 15f;
                return SharedData.PageSetup.GroupsPreferences.SpacingBeforeAllGroupsSummary;
            }
        }

        float spacingAfterAllGroupsSummary
        {
            get
            {
                if (SharedData.PageSetup.GroupsPreferences == null) return 0f;
                return SharedData.PageSetup.GroupsPreferences.SpacingAfterAllGroupsSummary;
            }
        }

        private static object getCurrentCellValue(IEnumerable<CellData> row, string property)
        {
            var cellValue = row.FirstOrDefault(x => x.PropertyName == property);
            if (cellValue == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Please include {0} property in PdfColumnsDefinitions too.", property));
            }
            var currentCellValue = cellValue.PropertyValue;
            return currentCellValue;
        }

        private void initMainTable()
        {
            createMainTable();
            initTableCellHelper();
            initRowsManager();
        }

        private void createMainTable()
        {
            InitTable.CreateMainTable();
            MainTable = InitTable.MainTable;
        }

        private void initRowsManager()
        {
            RowsManager.MainTable = MainTable;
            RowsManager.TableCellHelper = TableCellHelper;
        }

        private void initTableCellHelper()
        {
            TableCellHelper = InitTable.TableCellHelper;
        }

        bool areEqual(object lastRowCellValue, object currentCellValue, string property)
        {
            var cell = this.SharedData.OriginalPdfColumnsAttributes.FirstOrDefault(x => x.PropertyName == property);

            if (cell == null)
            {
                var propertiesList = this.SharedData.OriginalPdfColumnsAttributes.Select(x => x.PropertyName).Aggregate((p1, p2) => p1 + ", " + p2);
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "[{0}] not found in PdfColumnsDefinitions. Available properties list: {1}", property, propertiesList));
            }

            if (cell.IncludedGroupFieldEqualityComparer == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Please implement IncludedGroupFieldEqualityComparer of [{0}] property.", property));

            return cell.IncludedGroupFieldEqualityComparer.Invoke(lastRowCellValue, currentCellValue);
        }

        private void renderGroupHeader(IEnumerable<CellData> row)
        {
            applySpacingThreshold();

            if (SharedData.Header == null) return;
            var groupHeader = SharedData.Header.RenderingGroupHeader(SharedData.PdfDoc, SharedData.PdfWriter, row.ToList(), SharedData.ColumnCellsSummaryData);
            if (groupHeader == null) return;

            if (SharedData.ShouldWrapTablesInColumns)
            {
                MainGroupTable.AddCell(new PdfPCell(groupHeader) { Border = 0 });
            }
            else
            {
                SharedData.PdfDoc.Add(groupHeader);
            }
        }

        private void applySpacingThreshold()
        {
            if (SharedData.PageSetup.GroupsPreferences == null) return;
            var threshold = SharedData.PageSetup.GroupsPreferences.NewGroupAvailableSpacingThreshold;
            if (threshold.ApproxEquals(0)) return;
            var currentVerticalPosition = SharedData.PdfWriter.GetVerticalPosition(true);
            if (currentVerticalPosition < threshold)
            {
                SharedData.PdfDoc.NewPage();
            }
        }

        private void reset()
        {
            CurrentRowInfoData.LastGroupRowNumber = 1;
            CurrentRowInfoData.IsNewGroupStarted = true;
            CurrentRowInfoData.LastGroupNumber++;
        }

        private void updateLastRowValues(IList<CellData> row)
        {
            foreach (var property in SharedData.GroupByProperties)
            {
                var currentCellValue = getCurrentCellValue(row, property);
                _currentRowValues.Add(new KeyValuePair<string, object>(property, currentCellValue));
            }
        }

        /// <summary>
        /// Applies ShowGroupingPropertiesInAllRows if it's necessary.
        /// </summary>
        /// <param name="rowData">row's data</param>
        public void ModifyRowData(IList<CellData> rowData)
        {
            if (!SharedData.IsGroupingEnabled) return;
            if (SharedData.PageSetup.GroupsPreferences == null) return;
            if (SharedData.PageSetup.GroupsPreferences.GroupType != GroupType.IncludeGroupingColumns) return;
            if (SharedData.PageSetup.GroupsPreferences.ShowGroupingPropertiesInAllRows) return;
            if (CurrentRowInfoData.LastGroupRowNumber == 1) return;

            var groupByProperties = SharedData.PdfColumnsAttributes.Where(x => x.IncludeInGrouping).ToList();

            foreach (var cell in rowData)
            {
                var groupingProperties = groupByProperties.Where(x => x.PropertyName == cell.PropertyName).ToList();
                foreach (var property in groupingProperties)
                {
                    if (property.PropertyIndex >= 0)
                    {
                        if (property.PropertyIndex == cell.PropertyIndex && property.PropertyName == cell.PropertyName)
                        {
                            cell.PropertyValue = string.Empty;
                            cell.FormattedValue = string.Empty;
                        }
                    }
                    else if (property.PropertyName == cell.PropertyName)
                    {
                        cell.PropertyValue = string.Empty;
                        cell.FormattedValue = string.Empty;
                    }
                }
            }
        }

        #endregion Methods
    }
}
