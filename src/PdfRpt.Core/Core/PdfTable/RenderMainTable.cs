using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// Renders MainTable's Rows
    /// </summary>
    public class RenderMainTable
    {
        #region Fields (21)

        private IDataSource _bodyDataSource;
        private int _columnsCount;
        SharedData _commonManagersInfoData;
        private AdHocColumnsConventions _conventions;
        private IList<string> _groupByProperties;
        private GroupsManager _groupsManager;
        private InitTable _initTable;
        private bool _isGroupingEnabled;
        private PdfGrid _mainGroupTable;
        private IList<ColumnAttributes> _originalPdfColumnsDefinitions;
        private DocumentPreferences _pageSetup;
        private List<ColumnAttributes> _pdfColumnsDefinitions;
        private IPdfFont _pdfRptFont;
        private IPageHeader _pdfRptHeader;
        private SummaryCellSettings _pdfRptSummarySettings;
        private RowsManager _rowsManager;
        private Events _rptEvents;
        private ITableTemplate _rptTemplate;
        private bool _shouldWrapTablesInColumns;
        private PdfGrid _mainTable;
        private TableCellHelper _tableCellHelper;

        #endregion Fields

        #region Properties (5)

        /// <summary>
        /// List of the summaries data
        /// </summary>
        public IList<SummaryCellData> ColumnSummaryCellsData { set; get; }

        /// <summary>
        /// Holds last result of the actual rendering engine of iTextSharp during its processes
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// Pdf Document object
        /// </summary>
        public Document PdfDoc { get; set; }

        /// <summary>
        /// Main interface of PdfRpt
        /// </summary>
        public IPdfReportData PdfRptData { get; set; }

        /// <summary>
        /// PdfWriter object
        /// </summary>
        public PdfWriter PdfWriter { get; set; }

        #endregion Properties

        #region Methods (10)

        // Public Methods (1)

        /// <summary>
        /// Starts rendering of the MainTable
        /// </summary>
        public void AddToDocument()
        {
            initData();
            _rowsManager.AddMainTableRows();
            _rowsManager.CloseMainTable();
        }
        // Private Methods (9)

        private void initData()
        {
            setLocalCache();
            setColumnsVisibility();
            setColumnsDefinitions();
            setSharedData();
            initTable();
            initWrapping();
            initManagers();
        }

        private void initManagers()
        {
            _groupsManager = new GroupsManager
            {
                SharedData = _commonManagersInfoData,
                InitTable = _initTable,
                MainTable = _mainTable,
                CurrentRowInfoData = CurrentRowInfoData,
                TableCellHelper = _tableCellHelper,
                MainGroupTable = _mainGroupTable
            };

            _rowsManager = new RowsManager
            {
                SharedData = _commonManagersInfoData,
                TableCellHelper = _tableCellHelper,
                MainTable = _mainTable,
                GroupsManager = _groupsManager,
                MainGroupTable = _mainGroupTable,
                CurrentRowInfoData = CurrentRowInfoData
            };

            _groupsManager.RowsManager = _rowsManager;
        }

        private void initTable()
        {
            _initTable = new InitTable
            {
                SharedData = _commonManagersInfoData,
                CurrentRowInfoData = CurrentRowInfoData
            };
            _initTable.CreateMainTable();
            _mainTable = _initTable.MainTable;
            _tableCellHelper = _initTable.TableCellHelper;
        }

        private void initWrapping()
        {
            if (!_shouldWrapTablesInColumns) return;
            if (_pageSetup.PagePreferences.RunDirection == null)
                _pageSetup.PagePreferences.RunDirection = PdfRunDirection.LeftToRight;

            _mainGroupTable = new PdfGrid(1)
            {
                SplitLate = false,
                WidthPercentage = 100,
                RunDirection = (int)_pageSetup.PagePreferences.RunDirection
            };
        }

        private void setSharedData()
        {
            _commonManagersInfoData = new SharedData
            {
                PageSetup = _pageSetup,
                MainTableEvents = _rptEvents,
                GroupByProperties = _groupByProperties,
                IsGroupingEnabled = _isGroupingEnabled,
                Header = _pdfRptHeader,
                ShouldWrapTablesInColumns = _shouldWrapTablesInColumns,
                PdfColumnsAttributes = _pdfColumnsDefinitions,
                SummarySettings = _pdfRptSummarySettings,
                ColumnCellsSummaryData = ColumnSummaryCellsData,
                Template = _rptTemplate,
                PdfFont = _pdfRptFont,
                PdfDoc = PdfDoc,
                PdfWriter = PdfWriter,
                MainTableDataSource = _bodyDataSource,
                ColumnsCount = _columnsCount,
                OriginalPdfColumnsAttributes = _originalPdfColumnsDefinitions
            };
        }

        private void setColumnsDefinitions()
        {
            _columnsCount = _pdfColumnsDefinitions.Count;
            ColumnSummaryCellsData = new List<SummaryCellData>();
            _shouldWrapTablesInColumns = (_pageSetup.MultipleColumnsPerPage != null) && (_pageSetup.MultipleColumnsPerPage.ColumnsPerPage > 1);
        }

        private void setColumnsVisibility()
        {
            _pageSetup = PdfRptData.DocumentPreferences;
            _originalPdfColumnsDefinitions = PdfRptData.PdfColumnsAttributes;
            bool areColumnsAdHoc = false;
            if (_originalPdfColumnsDefinitions == null || !_originalPdfColumnsDefinitions.Any())
            {
                areColumnsAdHoc = true;
                _originalPdfColumnsDefinitions = new AdHocPdfColumnDefinitions(_bodyDataSource, _conventions).CreatePdfColumnDefinitions();
            }
            var visibleColumns = _conventions?.VisibleColumnNames;
            _bodyDataSource.ApplyPropertyDataAnnotations(this._originalPdfColumnsDefinitions, visibleColumns, areColumnsAdHoc);

            if (_pageSetup.GroupsPreferences == null ||
                _pageSetup.GroupsPreferences.GroupType == GroupType.HideGroupingColumns)
            {
                _pdfColumnsDefinitions = _originalPdfColumnsDefinitions.Where(x => x.IsVisible && !x.IncludeInGrouping)
                                             .OrderBy(x => x.Order)
                                             .ToList();
            }

            if (_pageSetup.GroupsPreferences != null &&
                _pageSetup.GroupsPreferences.GroupType == GroupType.IncludeGroupingColumns)
            {
                _pdfColumnsDefinitions = _originalPdfColumnsDefinitions.Where(x => x.IsVisible || x.IncludeInGrouping)
                                             .OrderBy(x => x.Order)
                                             .ToList();
            }

            if (_pageSetup.MainTablePreferences.TableType == TableType.HorizontalStackPanel)
            {
                var columnsCount = _pageSetup.MainTablePreferences.HorizontalStackPanelPreferences.ColumnsPerRow;
                var tempColumnsDefinitions = _pdfColumnsDefinitions.Where(x => !x.IsRowNumber).ToList();
                while (_pdfColumnsDefinitions.Count < columnsCount)
                {
                    _pdfColumnsDefinitions.AddRange(tempColumnsDefinitions);
                }
                _pdfColumnsDefinitions = _pdfColumnsDefinitions.Take(columnsCount).ToList();
            }

            _groupByProperties = _originalPdfColumnsDefinitions.Where(x => x.IncludeInGrouping)
                                     .Select(x => x.PropertyName)
                                     .ToList();
            _isGroupingEnabled = _originalPdfColumnsDefinitions.Any(x => x.IncludeInGrouping);
        }

        private void setLocalCache()
        {
            _bodyDataSource = PdfRptData.MainTableDataSource();
            _rptTemplate = PdfRptData.Template ?? new BasicTemplateProvider(BasicTemplate.NullTemplate);
            _pdfRptSummarySettings = PdfRptData.SummarySettings;
            _rptEvents = PdfRptData.MainTableEvents;
            _pdfRptFont = PdfRptData.PdfFont;
            _pdfRptHeader = PdfRptData.Header;
            _conventions = PdfRptData.AdHocColumnsConventions;
        }

        #endregion Methods
    }
}
