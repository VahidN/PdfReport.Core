using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using iTextSharp.text;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FooterTemplates;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// A sample template class for IPdfReportData.
    /// </summary>
    public abstract class DataTemplate : IPdfReportData
    {
        AdHocColumnsConventions _adHocColumnsConventions;
        string _backgroundImageFilePath;
        PointF? _backgroundImagePosition;
        bool _cacheHeader = true;
        TableColumnWidthType? _columnsWidthsType;
        PdfXConformance _conformanceLevel;
        string _defaultFont1;
        string _defaultFont2;
        BaseColor _defaultFontsColor = new BaseColor(Color.Black.ToArgb());
        int _defaultFontsSize = 9;
        BasicTemplate? _defaultTemplate;
        DiagonalWatermark _diagonalWatermark;
        PdfRunDirection _direction = PdfRunDirection.LeftToRight;
        DocumentPreferences _documentPreferences;
        DocumentSecurity _documentSecurity;
        string _fileName;
        IPageFooter _footer;
        GroupsPreferences _groupsPreferences;
        IPageHeader _header;
        HorizontalStackPanelPreferences _horizontalStackPanelPreferences = new HorizontalStackPanelPreferences();
        bool _keepTogether;
        Func<IDataSource> _mainTableDataSource;
        Events _mainTableEvents = new Events();
        DocumentMargins _margins = new DocumentMargins { Bottom = 60, Left = 36, Right = 36, Top = 36 };
        DocumentMetadata _metaData;
        MultipleColumnsPerPage _multipleColumnsPerPage;
        int _numberOfDataRowsPerPage;
        PageOrientation _orientation = PageOrientation.Portrait;
        Color? _pagesBackgroundColor;
        iTextSharp.text.Rectangle _pageSize;
        IList<ColumnAttributes> _pdfColumnsAttributes;
        IPdfFont _pdfFont;
        CompressionSettings _pdfRptCompression;
        PdfViewerPreferences _pdfRptViewerPreferences = new PdfViewerPreferences { ZoomPercent = 95 };
        Stream _pdfStreamOutput;
        string _printDate;
        PrintingPreferences _printingPreferences;
        PdfRunDirection? _runDirection;
        bool _showHeaderRow = true;
        float _spacingAfter;
        float _spacingBefore;
        bool _splitLate = true;
        bool _splitRows = true;
        SummaryCellSettings _summarySettings;
        TableType _tableType = TableType.NormalTable;
        ITableTemplate _template;

        /// <summary>
        /// Close the document by closing the underlying stream. Its default value is true.
        /// If you want to access the PDF stream after it has been created, set it to false.
        /// </summary>
        public bool CloseStream { set; get; } = true;

        /// <summary>
        /// If you don't set PdfColumnsDefinitions, list of the main table's columns will be extracted from MainTableDataSource automatically.
        /// Here you can control how cells should be rendered based on their specific data types.
        /// </summary>
        public AdHocColumnsConventions AdHocColumnsConventions
        {
            get { return _adHocColumnsConventions; }
            set { _adHocColumnsConventions = value; }
        }

        /// <summary>
        /// Gets or Sets The Custom Export Settings.
        /// </summary>
        public IList<IDataExporter> CustomExportSettings { set; get; }

        /// <summary>
        /// Sets Document's settings.
        /// It can be null. In this case some common default settings will be applied.
        /// </summary>
        public DocumentPreferences DocumentPreferences
        {
            get
            {
                if (_documentPreferences == null)
                    setDefaultDocumentPreferences();
                return _documentPreferences;
            }
            set { _documentPreferences = value; }
        }

        /// <summary>
        /// Sets the encryption options for this document.
        /// </summary>
        public DocumentSecurity DocumentSecurity
        {
            get { return _documentSecurity; }
            set { _documentSecurity = value; }
        }

        /// <summary>
        /// Gets or Sets The File Attachments.
        /// </summary>
        public IList<FileAttachment> FileAttachments { set; get; }

        /// <summary>
        /// Sets produced PDF file's path and name.
        /// It can be null if you are using an in memory stream.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Defines custom footer of the each page.
        /// If sets to null, the DefaultFooterProvider will be used automatically.
        /// </summary>
        public IPageFooter Footer
        {
            get
            {
                if (_footer == null)
                    setDefaultFooter();
                return _footer;
            }
            set { _footer = value; }
        }

        /// <summary>
        /// Defines dynamic headers for pages and individual groups.
        /// </summary>
        public IPageHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        /// <summary>
        /// Sets PdfRpt's DataSource.
        /// </summary>
        /// <returns></returns>
        public Func<IDataSource> MainTableDataSource
        {
            get { return _mainTableDataSource; }
            set { _mainTableDataSource = value; }
        }

        /// <summary>
        /// Sets the Main table's cells and rows events. It can be null.
        /// </summary>
        public Events MainTableEvents
        {
            get { return _mainTableEvents; }
            set { _mainTableEvents = value; }
        }

        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        public IList<ColumnAttributes> PdfColumnsAttributes
        {
            get { return _pdfColumnsAttributes; }
            set { _pdfColumnsAttributes = value; }
        }

        /// <summary>
        /// Sets the default fonts of the document. At least 2 fonts should be defined.
        /// Or ignore this property and use DefaultFontsPath method.
        /// </summary>
        public IPdfFont PdfFont
        {
            get
            {
                if (_pdfFont == null)
                    setDefaultPdfFonts();
                return _pdfFont;
            }
            set { _pdfFont = value; }
        }

        /// <summary>
        /// Sets the PDF file's stream.
        /// It can be null. In this case a new FileStream will be used automatically and you need to provide the FileName.
        /// </summary>
        public Stream PdfStreamOutput
        {
            get
            {
                if (_pdfStreamOutput == null)
                    return new FileStream(_fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                return _pdfStreamOutput;
            }
            set
            {
                _pdfStreamOutput = value;
            }
        }

        /// <summary>
        /// Sets Pages and groups summary values settings.
        /// </summary>
        public SummaryCellSettings SummarySettings
        {
            get { return _summarySettings; }
            set { _summarySettings = value; }
        }

        /// <summary>
        /// Sets the default template.
        /// It can be null. In this case a new BasicTemplateProvider based on the DefaultBasicTemplate will be used automatically.
        /// </summary>
        public ITableTemplate Template
        {
            get
            {
                if (_template == null)
                    setDefaultTemplate();
                return _template;
            }
            set { _template = value; }
        }

        /// <summary>
        /// You can define different headers for each page.
        /// If all of the headers of the document's pages are the same, set this value to true, to optimize the performance and document size.
        /// Its default value is true.
        /// </summary>
        /// <param name="cache">true or false</param>
        public void CacheHeader(bool cache = true)
        {
            _cacheHeader = cache;
        }

        /// <summary>
        /// Fires after a cell has been added.
        /// By setting MainTableEvents.CellAddedEvent property value directly, this method will be ignored.
        /// </summary>
        public void CellAddedEvent(Action<EventsArguments> onCellAdded)
        {
            _mainTableEvents.CellAdded = onCellAdded;
        }

        /// <summary>
        /// Fires when a cell is created.
        /// By setting MainTableEvents.CellCreatedEvent property value directly, this method will be ignored.
        /// </summary>
        public void CellCreatedEvent(Action<EventsArguments> onCellCreated)
        {
            _mainTableEvents.CellCreated = onCellCreated;
        }

        /// <summary>
        /// Fires when there's no data to render.
        /// By setting MainTableEvents.DataSourceIsEmptyEvent property value directly, this method will be ignored.
        /// </summary>
        /// <param name="message">a message to show</param>
        public void DataSourceIsEmptyEvent(string message)
        {
            _mainTableEvents.DataSourceIsEmpty = args => TableHelper.AddDefaultEmptyDataSourceTable(args.PdfDoc, PdfFont, _runDirection, message);
        }

        /// <summary>
        /// Background image's file path.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="backgroundImageFilePath">Background image's file path.</param>
        public void DefaultBackgroundImageFilePath(string backgroundImageFilePath)
        {
            _backgroundImageFilePath = backgroundImageFilePath;
        }

        /// <summary>
        /// Sets the absolute position of the Background image.
        /// If it's set to null, the image will be painted at the center of the page.
        /// </summary>
        /// <param name="position">Background image's position</param>
        public void DefaultBackgroundImagePosition(PointF? position)
        {
            _backgroundImagePosition = position;
        }

        /// <summary>
        /// Sets the default template.
        /// Use this method or set the Template property value directly.
        /// </summary>
        /// <param name="template">selected template</param>
        public void DefaultBasicTemplate(BasicTemplate template)
        {
            _defaultTemplate = template;
        }

        /// <summary>
        /// Determines the WidthType of the column.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="columnsWidthsType"></param>
        public void DefaultColumnsWidthsType(TableColumnWidthType columnsWidthsType)
        {
            _columnsWidthsType = columnsWidthsType;
        }

        /// <summary>
        /// Compression settings.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="compression">Compression settings</param>
        public void DefaultCompression(CompressionSettings compression)
        {
            _pdfRptCompression = compression;
        }

        /// <summary>
        /// Sets subsets of the PDF specification (ISO 15930-1 to ISO 15930-8) that promise
        /// predictable and consistent output for press printing.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="conformanceLevel">conformance level</param>
        public void DefaultConformanceLevel(PdfXConformance conformanceLevel)
        {
            _conformanceLevel = conformanceLevel;
        }

        /// <summary>
        /// A watermark text to display.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="diagonalWatermark">watermark settings</param>
        public void DefaultDiagonalWatermark(DiagonalWatermark diagonalWatermark)
        {
            _diagonalWatermark = diagonalWatermark;
        }

        /// <summary>
        /// Sets the new document's margins. Its predefined values are Bottom = 60, Left = 36, Right = 36, Top = 36.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="margins">The new document's margins</param>
        public void DefaultDocumentMargins(DocumentMargins margins)
        {
            _margins = margins;
        }

        /// <summary>
        /// Sets document's metadata.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="metadata">document's metadata</param>
        public void DefaultDocumentMetadata(DocumentMetadata metadata)
        {
            _metaData = metadata;
        }

        /// <summary>
        /// Sets the desired exporters such as ExportToExcel.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="exportSettings">export settings</param>
        public void DefaultExportSettings(IList<IDataExporter> exportSettings)
        {
            CustomExportSettings = exportSettings;
        }

        /// <summary>
        /// Adds the optional file attachments at the document level.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="fileAttachments">file attachments</param>
        public void DefaultFileAttachments(IList<FileAttachment> fileAttachments)
        {
            FileAttachments = fileAttachments;
        }

        /// <summary>
        /// Sets the default fonts color.
        /// Use this method or SetFonts method or set the PdfFont property value directly.
        /// </summary>
        /// <param name="color"></param>
        public void DefaultFontsColor(BaseColor color)
        {
            _defaultFontsColor = color;
        }

        /// <summary>
        /// Sets the optional fonts path.
        /// Use this method or SetFonts method or set the PdfFont property value directly.
        /// </summary>
        /// <param name="defaultFont1">font1's path</param>
        /// <param name="defaultFont2">font2's path</param>
        public void DefaultFontsPath(string defaultFont1, string defaultFont2)
        {
            _defaultFont1 = defaultFont1;
            _defaultFont2 = defaultFont2;
        }

        /// <summary>
        /// Sets the default fonts size.
        /// Use this method or SetFonts method or set the PdfFont property value directly.
        /// </summary>
        /// <param name="size">font size</param>
        public void DefaultFontsSize(int size)
        {
            _defaultFontsSize = size;
        }

        /// <summary>
        /// Sets the optional print date value of the DefaultFooterProvider.
        /// Use this method or SetFooter method or set the Footer property value directly.
        /// </summary>
        /// <param name="printDate">print date value</param>
        /// <param name="direction">Possible run direction values, left-to-right or right-to-left.</param>
        public void DefaultFooterPrintDate(string printDate, PdfRunDirection direction = PdfRunDirection.LeftToRight)
        {
            _printDate = printDate;
            _direction = direction;
        }

        /// <summary>
        /// Groups Preferences.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="groupsPreferences">Groups Preferences</param>
        public void DefaultGroupsPreferences(GroupsPreferences groupsPreferences)
        {
            _groupsPreferences = groupsPreferences;
        }

        /// <summary>
        /// If MainTableType is set to HorizontalStackPanel, here you can define its preferences such as
        /// number of columns per row.
        /// </summary>
        /// <param name="data">preferences</param>
        public void DefaultHorizontalStackPanelPreferences(HorizontalStackPanelPreferences data)
        {
            _horizontalStackPanelPreferences = data;
        }

        /// <summary>
        /// If true, the table will be kept on one page if it fits, by forcing a
        /// new page if it doesn't fit on the current page.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        public void DefaultKeepTogether(bool keepTogether)
        {
            _keepTogether = keepTogether;
        }

        /// <summary>
        /// Wrapping main table in multiple columns per pages.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="multipleColumnsPerPage">multiple columns per page</param>
        public void DefaultMultipleColumnsPerPage(MultipleColumnsPerPage multipleColumnsPerPage)
        {
            _multipleColumnsPerPage = multipleColumnsPerPage;
        }

        /// <summary>
        /// If sets to zero, NumberOfDataRowsPerPage will be calculated automatically, otherwise as specified.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="numberOfDataRowsPerPage">number of data rows per page</param>
        public void DefaultNumberOfDataRowsPerPage(int numberOfDataRowsPerPage)
        {
            _numberOfDataRowsPerPage = numberOfDataRowsPerPage;
        }
        /// <summary>
        /// Sets the PageOrientation.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="orientation">Page Orientation</param>
        public void DefaultOrientation(PageOrientation orientation)
        {
            _orientation = orientation;
        }

        /// <summary>
        /// Setting Page Background Color.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="backgroundColor">Pages Background Color</param>
        public void DefaultPagesBackgroundColor(Color backgroundColor)
        {
            _pagesBackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Sets the page size to PageSize.A4 for instance.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="pageSize">selected page size</param>
        public void DefaultPageSize(iTextSharp.text.Rectangle pageSize)
        {
            _pageSize = pageSize;
        }

        /// <summary>
        /// Indicates default values of print dialog box.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="printingPreferences">printing preferences</param>
        public void DefaultPrintingPreferences(PrintingPreferences printingPreferences)
        {
            _printingPreferences = printingPreferences;
        }

        /// <summary>
        /// Sets the run direction to rtl or ltr.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="runDirection">run direction, rtl or ltr</param>
        public void DefaultRunDirection(PdfRunDirection runDirection)
        {
            _runDirection = runDirection;
        }

        /// <summary>
        /// Sets the visibility of the main table's header row.
        /// It's true by default.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="showHeaderRow">show header row</param>
        public void DefaultShowHeaderRow(bool showHeaderRow)
        {
            _showHeaderRow = showHeaderRow;
        }

        /// <summary>
        /// Spacing after the main table.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        public void DefaultSpacingAfter(float spacingAfter)
        {
            _spacingAfter = spacingAfter;
        }

        /// <summary>
        /// Spacing before the main table.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        public void DefaultSpacingBefore(float spacingBefore)
        {
            _spacingBefore = spacingBefore;
        }

        /// <summary>
        /// Split the cells of the first row that doesn't fit the page.
        /// If true, a that row doesn't fit on the page, its complete row will be forwarded to the next page.
        /// If false, rows that are too high to fit on a page will be dropped from the table.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        public void DefaultSplitLate(bool splitLate)
        {
            _splitLate = splitLate;
        }

        /// <summary>
        /// If true, splits rows that are forwarded to the next page but that still
        /// don't fit because the row height exceeds the available page height.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        public void DefaultSplitRows(bool splitRows)
        {
            _splitRows = splitRows;
        }

        /// <summary>
        /// Sets the TableType. Its default value is a noraml PdfGrid.
        /// </summary>
        /// <param name="tableType">Value of the TableType</param>
        public void DefaultTableType(TableType tableType)
        {
            _tableType = tableType;
        }

        /// <summary>
        /// Specifies the Adobe Reader's settings when a document is opened.
        /// If you are setting SetDocumentPreferences method or DocumentPreferences property value directly, ignore this method.
        /// </summary>
        /// <param name="viewerPreferences">viewer preferences</param>
        public void DefaultViewerPreferences(PdfViewerPreferences viewerPreferences)
        {
            _pdfRptViewerPreferences = viewerPreferences;
        }

        /// <summary>
        /// Fires before closing the Document.
        /// By setting MainTableEvents.DocumentClosingEvent property value directly, this method will be ignored.
        /// </summary>
        public void DocumentClosingEvent(Action<EventsArguments> onDocumentClosing)
        {
            _mainTableEvents.DocumentClosing = onDocumentClosing;
        }

        /// <summary>
        /// Fires when Document is created.
        /// By setting MainTableEvents.DocumentOpenedEvent property value directly, this method will be ignored.
        /// </summary>
        public void DocumentOpenedEvent(Action<EventsArguments> onDocumentOpened)
        {
            _mainTableEvents.DocumentOpened = onDocumentOpened;
        }

        /// <summary>
        /// Fires after a group has been added.
        /// By setting MainTableEvents.GroupAddedEvent property value directly, this method will be ignored.
        /// </summary>
        public void GroupAddedEvent(Action<EventsArguments> onGroupAdded)
        {
            _mainTableEvents.GroupAdded = onGroupAdded;
        }

        /// <summary>
        /// Fires after MainTable has been added.
        /// By setting MainTableEvents.MainTableAddedEvent property value directly, this method will be ignored.
        /// </summary>
        public void MainTableAddedEvent(Action<EventsArguments> onMainTableAdded)
        {
            _mainTableEvents.MainTableAdded = onMainTableAdded;
        }

        /// <summary>
        /// Fires when MainTable is created.
        /// By setting MainTableEvents.MainTableCreatedEvent property value directly, this method will be ignored.
        /// </summary>
        public void MainTableCreatedEvent(Action<EventsArguments> onMainTableCreated)
        {
            _mainTableEvents.MainTableCreated = onMainTableCreated;
        }

        /// <summary>
        /// Fires after each part of the MainTable has been added to the current page.
        /// </summary>
        public void PageTableAddedEvent(Action<EventsArguments> onPageTableAdded)
        {
            _mainTableEvents.PageTableAdded = onPageTableAdded;
        }

        /// <summary>
        /// Fires after a row has been added.
        /// By setting MainTableEvents.RowAddedEvent property value directly, this method will be ignored.
        /// </summary>
        public void RowAddedEvent(Action<EventsArguments> onRowAdded)
        {
            _mainTableEvents.RowAdded = onRowAdded;
        }

        /// <summary>
        /// Fires after RowAddedEvent when the RowType is DataTableRow.
        /// </summary>
        /// <param name="injectCustomRows">list of custom rows to inject</param>
        public void RowAddedInjectCustomRowsEvent(Func<EventsArguments, IList<IList<CellData>>> injectCustomRows)
        {
            _mainTableEvents.RowAddedInjectCustomRows = injectCustomRows;
        }

        /// <summary>
        /// Fires before a row has been added.
        /// By setting MainTableEvents.RowStartedEvent property value directly, this method will be ignored.
        /// </summary>
        public void RowStartedEvent(Action<EventsArguments> onRowStarted)
        {
            _mainTableEvents.RowStarted = onRowStarted;
        }

        /// <summary>
        /// Fires before RowStartedEvent when the RowType is DataTableRow.
        /// </summary>
        /// <param name="injectCustomRows">list of custom rows to inject</param>
        public void RowStartedInjectCustomRowsEvent(Func<EventsArguments, IList<IList<CellData>>> injectCustomRows)
        {
            _mainTableEvents.RowStartedInjectCustomRows = injectCustomRows;
        }

        /// <summary>
        /// If you don't set PdfColumnsDefinitions, list of the main table's columns will be extracted from MainTableDataSource automatically.
        /// Here you can control how cells should be rendered based on their specific data types.
        /// </summary>
        /// <param name="adHocColumnsConventions">conventions</param>
        public void SetAdHocColumnsConventions(AdHocColumnsConventions adHocColumnsConventions)
        {
            _adHocColumnsConventions = adHocColumnsConventions;
        }

        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="pdfColumnsAttributes"></param>
        public void SetColumnsAttributes(IList<ColumnAttributes> pdfColumnsAttributes)
        {
            _pdfColumnsAttributes = pdfColumnsAttributes;
        }

        /// <summary>
        /// Sets Document's settings.
        /// It can be null. In this case some common default settings will be applied.
        /// </summary>
        /// <param name="documentPreferences">Document's settings</param>
        public void SetDocumentPreferences(DocumentPreferences documentPreferences)
        {
            _documentPreferences = documentPreferences;
        }

        /// <summary>
        /// Sets produced PDF file's path and name.
        /// It can be null if you are using an in memory stream.
        /// </summary>
        /// <param name="fileName">produced PDF file's path and name</param>
        public void SetFileName(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Sets the default fonts of the document. At least 2 fonts should be defined.
        /// Or ignore this method and use DefaultFontsPath method.
        /// </summary>
        /// <param name="defaultFonts">default fonts</param>
        public void SetFonts(IPdfFont defaultFonts)
        {
            _pdfFont = defaultFonts;
        }

        /// <summary>
        /// Defines custom footer of the each page.
        /// If sets to null, the DefaultFooterProvider will be used automatically.
        /// </summary>
        /// <param name="footer">a custom footer.</param>
        public void SetFooter(IPageFooter footer)
        {
            _footer = footer;
        }

        /// <summary>
        /// Defines dynamic headers for pages and individual groups.
        /// </summary>
        /// <param name="header">a dynamic header</param>
        public void SetHeader(IPageHeader header)
        {
            _header = header;
        }

        /// <summary>
        /// Main table's data source. The data to render.
        /// </summary>
        /// <param name="mainTableDataSource">Main table's data source</param>
        public void SetMainTableDataSource(Func<IDataSource> mainTableDataSource)
        {
            _mainTableDataSource = mainTableDataSource;
        }

        /// <summary>
        /// Sets the Main table's cells and rows events. It can be null.
        /// </summary>
        /// <param name="mainTableEvents">Main table's cells and rows events</param>
        public void SetMainTableEvents(Events mainTableEvents)
        {
            _mainTableEvents = mainTableEvents;
        }

        /// <summary>
        /// Sets the encryption options for this document.
        /// </summary>
        /// <param name="documentSecurity">encryption options</param>
        public void SetPdfDocumentSecurity(DocumentSecurity documentSecurity)
        {
            _documentSecurity = documentSecurity;
        }

        /// <summary>
        /// Sets the PDF file's stream.
        /// It can be null. In this case a new FileStream will be used automatically and you need to provide the FileName.
        /// </summary>
        /// <param name="pdfStreamOutput">the PDF file's stream</param>
        public void SetStreamOutput(Stream pdfStreamOutput)
        {
            _pdfStreamOutput = pdfStreamOutput;
        }

        /// <summary>
        /// Sets Pages and groups summary values settings.
        /// </summary>
        /// <param name="summarySettings">summary values settings</param>
        public void SetSummarySettings(SummaryCellSettings summarySettings)
        {
            _summarySettings = summarySettings;
        }

        /// <summary>
        /// Sets the default template.
        /// It can be null. In this case a new BasicTemplateProvider based on the DefaultBasicTemplate will be used automatically.
        /// </summary>
        /// <param name="template"></param>
        public void SetTemplate(ITableTemplate template)
        {
            _template = template;
        }

        /// <summary>
        /// Fires before a footer row has been added.
        /// By setting MainTableEvents.ShouldSkipFooter property value directly, this method will be ignored.
        /// </summary>
        public void ShouldSkipFooterEvent(Func<EventsArguments, bool> onShouldSkipFooter)
        {
            _mainTableEvents.ShouldSkipFooter = onShouldSkipFooter;
        }

        /// <summary>
        /// Fires before a header row has been added.
        /// By setting MainTableEvents.ShouldSkipHeader property value directly, this method will be ignored.
        /// </summary>
        public void ShouldSkipHeaderEvent(Func<EventsArguments, bool> onShouldSkipHeader)
        {
            _mainTableEvents.ShouldSkipHeader = onShouldSkipHeader;
        }

        /// <summary>
        /// Fires before a row has been added.
        /// By setting MainTableEvents.ShouldSkipRow property value directly, this method will be ignored.
        /// </summary>
        public void ShouldSkipRowEvent(Func<EventsArguments, bool> onShouldSkipRow)
        {
            _mainTableEvents.ShouldSkipRow = onShouldSkipRow;
        }

        private void setDefaultDocumentPreferences()
        {
            _documentPreferences = new DocumentPreferences
            {
                PagePreferences = new PagePreferences
                {
                    Margins = _margins,
                    RunDirection = _runDirection.HasValue ? _runDirection.Value : PdfRunDirection.LeftToRight,
                    Size = _pageSize ?? PageSize.A4,
                    Orientation = _orientation,
                    BackgroundImageFilePath = _backgroundImageFilePath,
                    PagesBackgroundColor = _pagesBackgroundColor,
                    CacheHeader = _cacheHeader,
                    BackgroundImagePosition = _backgroundImagePosition
                },
                ViewerPreferences = _pdfRptViewerPreferences,
                MainTablePreferences = new MainTablePreferences
                {
                    WidthPercentage = 100,
                    ShowHeaderRow = _showHeaderRow,
                    ColumnsWidthsType = _columnsWidthsType.HasValue ? _columnsWidthsType.Value : TableColumnWidthType.FitToContent,
                    NumberOfDataRowsPerPage = _numberOfDataRowsPerPage,
                    SplitLate = _splitLate,
                    SplitRows = _splitRows,
                    KeepTogether = _keepTogether,
                    SpacingBefore = _spacingBefore,
                    SpacingAfter = _spacingAfter,
                    TableType = _tableType,
                    HorizontalStackPanelPreferences = _horizontalStackPanelPreferences
                },
                DocumentMetadata = _metaData,
                ExportSettings = CustomExportSettings,
                MultipleColumnsPerPage = _multipleColumnsPerPage,
                DiagonalWatermark = _diagonalWatermark,
                GroupsPreferences = _groupsPreferences,
                CompressionSettings = _pdfRptCompression,
                ConformanceLevel = _conformanceLevel,
                PrintingPreferences = _printingPreferences,
                FileAttachments = FileAttachments
            };
        }

        private void setDefaultFooter()
        {
            if (string.IsNullOrEmpty(_printDate)) return;
            _footer = new DefaultFooterProvider(PdfFont, _printDate, _direction);
        }

        void setDefaultPdfFonts()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            var mainFontPath = string.IsNullOrEmpty(_defaultFont1) ? Path.Combine(root, "fonts", "arial.ttf") : _defaultFont1;
            var defaultFontPath = string.IsNullOrEmpty(_defaultFont2) ? Path.Combine(root, "fonts", "verdana.ttf") : _defaultFont2;
            _pdfFont = new GenericFontProvider(mainFontPath, defaultFontPath)
            {
                Color = _defaultFontsColor,
                Size = _defaultFontsSize,
                Style = DocumentFontStyle.Normal
            };
        }

        private void setDefaultTemplate()
        {
            if (_defaultTemplate == null) return;
            _template = new BasicTemplateProvider(_defaultTemplate.Value);
        }
    }
}