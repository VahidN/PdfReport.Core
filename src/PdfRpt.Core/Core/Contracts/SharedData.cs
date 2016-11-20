using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// A class to hold shared info between rendering classes.
    /// </summary>
    public class SharedData
    {
        /// <summary>
        /// Summary cells data list
        /// </summary>
        public IList<SummaryCellData> ColumnCellsSummaryData { set; get; }

        /// <summary>
        /// Final summary cells data list, after applying aggregate functions
        /// </summary>
        public IList<CellRowData> ColumnCellsFinalSummaryData { set; get; }

        /// <summary>
        /// List of properties of the defined group
        /// </summary>
        public IList<string> GroupByProperties { set; get; }

        /// <summary>
        /// Indicates grouping is enabled or not
        /// </summary>
        public bool IsGroupingEnabled { set; get; }

        /// <summary>
        /// Pages and groups summary values settings.
        /// </summary>
        public SummaryCellSettings SummarySettings { set; get; }

        /// <summary>
        /// Document settings.
        /// </summary>
        public DocumentPreferences PageSetup { set; get; }

        /// <summary>
        /// Defining which properties of MainTableDataSource should be rendered and how.
        /// </summary>
        public IList<ColumnAttributes> PdfColumnsAttributes { set; get; }

        /// <summary>
        /// None filtered version of PdfColumnsAttributes
        /// </summary>
        public IList<ColumnAttributes> OriginalPdfColumnsAttributes { set; get; }

        /// <summary>
        /// PDF Document
        /// </summary>
        public Document PdfDoc { get; set; }

        /// <summary>
        /// Pdf document's font
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// Defines dynamic headers of pages and individual groups.
        /// </summary>
        public IPageHeader Header { set; get; }

        /// <summary>
        /// PdfWriter object
        /// </summary>
        public PdfWriter PdfWriter { get; set; }

        /// <summary>
        /// Main table's cells and rows events
        /// </summary>
        public Events MainTableEvents { set; get; }

        /// <summary>
        /// Main table's template
        /// </summary>
        public ITableTemplate Template { set; get; }

        /// <summary>
        /// Is wrapping main table in multiple columns enabled?
        /// </summary>
        public bool ShouldWrapTablesInColumns { set; get; }

        /// <summary>
        /// Main table's data source. The data to render.
        /// </summary>
        public IDataSource MainTableDataSource { set; get; }

        /// <summary>
        /// Number of columns.
        /// </summary>
        public int ColumnsCount { set; get; }

        /// <summary>
        /// Returns ordered list of MainTable's columns widths.
        /// </summary>
        /// <returns>Columns Widths</returns>
        public float[] ColumnsWidths
        {
            get
            {
                return PageSetup.PagePreferences.RunDirection == PdfRunDirection.LeftToRight
                    ? PdfColumnsAttributes.OrderBy(x => x.Order).Select(x => x.Width).ToArray()
                    : PdfColumnsAttributes.OrderBy(x => x.Order).Select(x => x.Width).Reverse().ToArray();
            }
        }

        /// <summary>
        /// If TableType is set to HorizontalStackPanel, HorizontalStackPanelColumnsPerRow will define the
        /// number of columns per row.
        /// </summary>
        public int HorizontalStackPanelColumnsPerRow
        {
            get
            {
                if (PageSetup.MainTablePreferences.HorizontalStackPanelPreferences == null)
                    return 0;
                return PageSetup.MainTablePreferences.HorizontalStackPanelPreferences.ColumnsPerRow;
            }
        }

        /// <summary>
        /// Determines whether MainTable is a HorizontalStackPanel or not.
        /// </summary>
        public bool IsMainTableHorizontalStackPanel
        {
            get
            {
                return
                    PageSetup.MainTablePreferences.TableType == TableType.HorizontalStackPanel &&
                    HorizontalStackPanelColumnsPerRow > 0;
            }
        }

        /// <summary>
        /// Do we have a row number column?
        /// </summary>
        public bool HasTableRowNumberColumn
        {
            get
            {
                return PdfColumnsAttributes.Any(x => x.IsRowNumber);
            }
        }

        /// <summary>
        /// Holds Extra Header Rows Value.
        /// </summary>
        public int ExtraHeaderRows { set; get; }
    }
}
