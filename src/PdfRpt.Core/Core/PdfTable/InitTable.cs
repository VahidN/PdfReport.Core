using System;
using System.Linq;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// Initializes the MainTable's settings
    /// </summary>
    public class InitTable
    {
        #region Fields (2)

        int _footerRows;
        int _headerRows;

        #endregion Fields

        #region Properties (4)

        /// <summary>
        /// Holds shared info between rendering classes.
        /// </summary>
        public SharedData SharedData { set; get; }

        /// <summary>
        /// Holds last result of the actual rendering engine of iTextSharp during its processes.
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// MainTable of the PdfRpt
        /// </summary>
        public PdfGrid MainTable { private set; get; }

        /// <summary>
        /// Helps rendering main table's cells
        /// </summary>
        public TableCellHelper TableCellHelper { private set; get; }

        #endregion Properties

        #region Methods (11)

        // Public Methods (2)


        /// <summary>
        /// Creates and initializes the MainTable's settings
        /// </summary>
        public void CreateMainTable()
        {
            setHeaderAndFooterRows();

            var len = SharedData.ColumnsWidths.Length;
            if (SharedData.IsMainTableHorizontalStackPanel)
            {
                len = SharedData.HorizontalStackPanelColumnsPerRow;
            }
            if (len == 0)
                throw new InvalidOperationException("At least one column should be defined or visible.");

            if (SharedData.PageSetup.PagePreferences.RunDirection == null)
                SharedData.PageSetup.PagePreferences.RunDirection = PdfRunDirection.LeftToRight;

            setDefaultSpacingAfterForGroups();

            MainTable = new PdfGrid(len)
            {
                RunDirection = (int)SharedData.PageSetup.PagePreferences.RunDirection,
                WidthPercentage = SharedData.PageSetup.MainTablePreferences.WidthPercentage,
                HeadersInEvent = true,
                HeaderRows = _headerRows,
                FooterRows = _footerRows,
                SkipFirstHeader = true,
                SplitLate = SharedData.PageSetup.MainTablePreferences.SplitLate,
                SpacingAfter = SharedData.PageSetup.MainTablePreferences.SpacingAfter,
                SpacingBefore = SharedData.PageSetup.MainTablePreferences.SpacingBefore,
                KeepTogether = SharedData.PageSetup.MainTablePreferences.KeepTogether,
                SplitRows = SharedData.PageSetup.MainTablePreferences.SplitRows
            };

            setWidths();

            TableCellHelper = new TableCellHelper
            {
                SharedData = SharedData,
                MainTable = MainTable,
                CurrentRowInfoData = CurrentRowInfoData,
                ShowAllGroupsSummaryRow = showAllGroupsSummaryRow
            };
        }

        bool showAllGroupsSummaryRow
        {
            get
            {
                if (SharedData.PageSetup.GroupsPreferences == null) return false;
                return SharedData.PageSetup.GroupsPreferences.ShowAllGroupsSummaryRow;
            }
        }

        private void setDefaultSpacingAfterForGroups()
        {
            if (SharedData.PageSetup.MainTablePreferences.SpacingAfter > 0) return;
            if (SharedData.IsGroupingEnabled)
                SharedData.PageSetup.MainTablePreferences.SpacingAfter = 20;
        }

        private void setWidths() //todo: remove duplicates
        {
            switch (SharedData.PageSetup.MainTablePreferences.ColumnsWidthsType)
            {
                case TableColumnWidthType.Relative:
                    if (SharedData.PageSetup.MainTablePreferences.TableType == TableType.NormalTable)
                        MainTable.SetWidths(SharedData.ColumnsWidths);
                    break;
                case TableColumnWidthType.Absolute:
                    if (SharedData.PageSetup.MainTablePreferences.TableType == TableType.NormalTable)
                        MainTable.SetTotalWidth(SharedData.ColumnsWidths);
                    break;
                case TableColumnWidthType.FitToContent:
                    break;
                case TableColumnWidthType.EquallySized:
                    break;
            }
        }
        // Private Methods (9)

        private void applyPageSummary()
        {
            if (areSettingsNull()) return;
            if (!SharedData.SummarySettings.PageSummarySettings.ShowOnEachPage) return;

            if (SharedData.SummarySettings.PageSummarySettings.ShowOnEachPage)
            {
                _headerRows++;
                _footerRows++;
            }
        }

        private void applyShowMainTableHeaderRow()
        {
            if (!SharedData.PageSetup.MainTablePreferences.ShowHeaderRow)
            {
                _headerRows--;
            }
        }

        bool areSettingsNull()
        {
            return SharedData.SummarySettings == null ||
                SharedData.SummarySettings.OverallSummarySettings == null ||
                SharedData.SummarySettings.PreviousPageSummarySettings == null;
        }

        private void checkNullValue()
        {
            if (SharedData.SummarySettings == null)
            {
                _headerRows = 1;
                _footerRows = 0;
                return;
            }

            if (SharedData.SummarySettings.PreviousPageSummarySettings == null)
            {
                SharedData.SummarySettings.PreviousPageSummarySettings = new SummaryCellAttributes { ShowOnEachPage = false };
            }

            if (SharedData.SummarySettings.OverallSummarySettings == null)
            {
                SharedData.SummarySettings.OverallSummarySettings = new SummaryCellAttributes { ShowOnEachPage = false };
            }

            if (SharedData.SummarySettings.PageSummarySettings == null)
            {
                SharedData.SummarySettings.PageSummarySettings = new SummaryCellAttributes { ShowOnEachPage = false };
            }
        }

        private void dontShowSummaryDontShowRemaining()
        {
            if (areSettingsNull()) return;

            if (!SharedData.SummarySettings.OverallSummarySettings.ShowOnEachPage &&
                !SharedData.SummarySettings.PreviousPageSummarySettings.ShowOnEachPage)
            {
                _headerRows = 1;
                _footerRows = 0;
            }
        }

        private void dontShowSummaryShowRemaining()
        {
            if (areSettingsNull()) return;

            if (!SharedData.SummarySettings.OverallSummarySettings.ShowOnEachPage &&
                SharedData.SummarySettings.PreviousPageSummarySettings.ShowOnEachPage)
            {
                _headerRows = 2;
                _footerRows = 0;
            }
        }

        private void setHeaderAndFooterRows()
        {
            _headerRows = 3;
            _footerRows = 1;

            checkNullValue();
            showSummaryShowRemaining();
            dontShowSummaryShowRemaining();
            showSummaryDontShowRemaining();
            dontShowSummaryDontShowRemaining();
            applyPageSummary();
            applyShowMainTableHeaderRow();
            setAdditionalHeadingCellsRows();
        }

        private void setAdditionalHeadingCellsRows()
        {
            var numberOfAdditionalHeaderRows = 0;
            foreach (var item in SharedData.PdfColumnsAttributes.Where(x => x.HeadingCells != null && (x.IsVisible || x.IncludeInGrouping)))
            {
                if (numberOfAdditionalHeaderRows < item.HeadingCells.Count)
                    numberOfAdditionalHeaderRows = item.HeadingCells.Count;
            }

            SharedData.ExtraHeaderRows = numberOfAdditionalHeaderRows;
            _headerRows += numberOfAdditionalHeaderRows;
        }

        private void showSummaryDontShowRemaining()
        {
            if (areSettingsNull()) return;

            if (SharedData.SummarySettings.OverallSummarySettings.ShowOnEachPage &&
                !SharedData.SummarySettings.PreviousPageSummarySettings.ShowOnEachPage)
            {
                _headerRows = 2;
                _footerRows = 1;
            }
        }

        private void showSummaryShowRemaining()
        {
            if (areSettingsNull()) return;

            if (SharedData.SummarySettings.OverallSummarySettings.ShowOnEachPage &&
                SharedData.SummarySettings.PreviousPageSummarySettings.ShowOnEachPage)
            {
                _headerRows = 3;//take first 3 rows
                _footerRows = 1;//last one is footer and the rest are headers
            }
        }

        #endregion Methods
    }
}
