using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using iTextSharp.text;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// Helps rendering of the main table's cells
    /// </summary>
    public class TableCellHelper
    {
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
        public PdfGrid MainTable { set; get; }

        /// <summary>
        /// Indicates visibility of the groups SummaryRows
        /// </summary>
        public bool ShowAllGroupsSummaryRow { set; get; }

        #endregion Properties

        #region Methods (8)

        // Public Methods (7)

        /// <summary>
        /// Adds a new PdfPCell to the MainTable
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="rawData"></param>
        /// <param name="columnNumber"></param>
        /// <param name="pdfRowType"></param>
        /// <param name="pdfCellType"></param>
        /// <param name="rowValues"></param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="pdfFontStyle"></param>
        /// <param name="rotation"></param>
        /// <param name="setItemTemplate"></param>
        /// <param name="colSpan"></param>
        /// <returns></returns>
        public CellAttributes AddGeneralCell(
                    BaseColor backgroundColor,
                    BaseColor foreColor,
                    object rawData,
                    int columnNumber,
                    RowType pdfRowType,
                    CellType pdfCellType,
                    IList<CellData> rowValues = null,
                    HorizontalAlignment horizontalAlignment = HorizontalAlignment.None,
                    DocumentFontStyle pdfFontStyle = DocumentFontStyle.None,
                    int rotation = 0,
                    bool setItemTemplate = false,
                    int colSpan = 1)
        {
            var col = SharedData.PdfColumnsAttributes[columnNumber];

            var cellData = new CellAttributes
            {
                RowData = new CellRowData
                {
                    TableRowData = rowValues,
                    Value = rawData,
                    PdfRowType = pdfRowType,
                    ColumnNumber = columnNumber,
                    PropertyName = col.PropertyName,
                    LastRenderedRowNumber = CurrentRowInfoData.LastRenderedRowNumber
                },
                SharedData = new CellSharedData
                {
                    PdfColumnAttributes = col,
                    DataRowNumber = CurrentRowInfoData.LastOverallDataRowNumber,
                    GroupNumber = CurrentRowInfoData.LastGroupRowNumber,
                    PdfDoc = SharedData.PdfDoc,
                    PdfWriter = SharedData.PdfWriter,
                    SummarySettings = SharedData.SummarySettings,
                    Template = SharedData.Template
                },
                ItemTemplate = setItemTemplate ? col.ColumnItemsTemplate : null,
                BasicProperties = new CellBasicProperties
                {
                    PdfFont = SharedData.PdfFont,
                    Rotation = rotation,
                    PdfFontStyle = (pdfFontStyle == DocumentFontStyle.None) ? DocumentFontStyle.Normal : pdfFontStyle,
                    BackgroundColor = backgroundColor,
                    BorderColor = SharedData.Template.CellBorderColor,
                    FontColor = foreColor,
                    RunDirection = SharedData.PageSetup.PagePreferences.RunDirection,
                    ShowBorder = SharedData.Template.ShowGridLines,
                    HorizontalAlignment = (horizontalAlignment == HorizontalAlignment.None) ? col.CellsHorizontalAlignment : horizontalAlignment,
                    FixedHeight = col.FixedHeight,
                    MinimumHeight = col.MinimumHeight,
                    CellPadding = col.Padding,
                    PaddingBottom = col.PaddingBottom,
                    PaddingLeft = col.PaddingLeft,
                    PaddingRight = col.PaddingRight,
                    PaddingTop = col.PaddingTop
                }
            };

            if (SharedData.MainTableEvents != null) SharedData.MainTableEvents.CellCreated(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Cell = cellData, CellType = pdfCellType, RowType = pdfRowType, ColumnNumber = columnNumber, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = CurrentRowInfoData.PreviousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });
            var cell = cellData.CreateSafePdfPCell(new TextBlockField());

            if (SharedData.ColumnCellsFinalSummaryData == null)
                SharedData.ColumnCellsFinalSummaryData = new List<CellRowData>();

            cell.CellEvent = new MainTableCellsEvent(cellData)
            {
                SummaryCellsData = SharedData.ColumnCellsSummaryData,
                IsGroupingEnabled = SharedData.IsGroupingEnabled,
                CurrentRowInfoData = CurrentRowInfoData,
                SharedData = SharedData,
                CellType = pdfCellType
            };

            if (colSpan > 1) cell.Colspan = colSpan;

            MainTable.AddCell(cell);
            if (SharedData.MainTableEvents != null) SharedData.MainTableEvents.CellAdded(new EventsArguments { PdfDoc = SharedData.PdfDoc, PdfWriter = SharedData.PdfWriter, Cell = cellData, CellType = pdfCellType, RowType = pdfRowType, ColumnNumber = columnNumber, ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData, PreviousTableRowData = CurrentRowInfoData.PreviousTableRowData, PageSetup = SharedData.PageSetup, PdfFont = SharedData.PdfFont, PdfColumnsAttributes = SharedData.PdfColumnsAttributes });

            return cellData;
        }

        /// <summary>
        /// Adds a new header PdfPCell to the MainTable
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <param name="colSpan"></param>
        /// <returns></returns>
        public CellAttributes AddHeaderCell(int columnNumber, int colSpan)
        {
            var col = SharedData.PdfColumnsAttributes[columnNumber];
            return AddGeneralCell(
                SharedData.Template.HeaderBackgroundColor[0],
                SharedData.Template.HeaderFontColor,
                col.HeaderCell.Caption,
                columnNumber,
                RowType.HeaderRow,
                CellType.HeaderCell,
                pdfFontStyle: DocumentFontStyle.Bold,
                rotation: col.HeaderCell.CaptionRotation,
                horizontalAlignment: col.HeaderCell.HorizontalAlignment == null ? SharedData.Template.HeaderHorizontalAlignment : col.HeaderCell.HorizontalAlignment.Value,
                colSpan: colSpan);
        }

        /// <summary>
        /// Adds a new extra header PdfPCell to the MainTable
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <param name="colSpan"></param>
        /// <param name="pdfHeadingCell"></param>
        /// <returns></returns>
        public CellAttributes AddExtraHeaderCell(int columnNumber, int colSpan, HeadingCell pdfHeadingCell)
        {
            return AddGeneralCell(
                SharedData.Template.HeaderBackgroundColor[0],
                SharedData.Template.HeaderFontColor,
                pdfHeadingCell.Caption,
                columnNumber,
                RowType.HeaderRow,
                CellType.HeaderCell,
                pdfFontStyle: DocumentFontStyle.Bold,
                rotation: pdfHeadingCell.CaptionRotation,
                horizontalAlignment: pdfHeadingCell.HorizontalAlignment == null ? SharedData.Template.HeaderHorizontalAlignment : pdfHeadingCell.HorizontalAlignment.Value,
                colSpan: colSpan);
        }

        /// <summary>
        /// Adds a new PreviousPageSummary PdfPCell to the MainTable
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public CellAttributes AddPreviousPageSummaryCell(BaseColor backgroundColor, BaseColor foreColor, int columnNumber)
        {
            var alignment = SharedData.SummarySettings.PreviousPageSummarySettings.LabelHorizontalAlignment;
            return AddGeneralCell(
                backgroundColor,
                foreColor,
                SharedData.SummarySettings.PreviousPageSummarySettings.Label,
                columnNumber,
                RowType.PreviousPageSummaryRow,
                CellType.PreviousPageSummaryCell,
                horizontalAlignment: alignment.HasValue ? alignment.Value : HorizontalAlignment.None);
        }

        /// <summary>
        /// Adds a new data PdfPCell to the MainTable
        /// </summary>
        /// <param name="rowValues"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public CellAttributes AddRowCell(IList<CellData> rowValues, BaseColor backgroundColor, BaseColor foreColor, int columnNumber)
        {
            var col = SharedData.PdfColumnsAttributes[columnNumber];

            checkProperty(columnNumber, col);

            object data;
            if (col.IsCalculatedField)
            {
                data = FuncHelper.ApplyCalculatedFieldFormula(col.CalculatedFieldFormula, rowValues);
                rowValues.Add(new CellData { PropertyName = col.PropertyName, PropertyValue = data, FormattedValue = data.ToSafeString() });
            }
            else
            {
                CellData pdfCellData;
                if (col.PropertyIndex >= 0)
                {
                    pdfCellData = rowValues.FirstOrDefault(x => x.PropertyName == col.PropertyName && x.PropertyIndex == col.PropertyIndex);
                }
                else
                {
                    pdfCellData = rowValues.FirstOrDefault(x => x.PropertyName == col.PropertyName);
                }

                if (pdfCellData == null)
                {
                    var propertiesList = rowValues.Select(x => x.PropertyName).Aggregate((p1, p2) => p1 + ", " + p2);
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                        "'{0}' property not found. Available properties list: {1}", col.PropertyName, propertiesList));
                }
                data = pdfCellData.PropertyValue;
            }

            return AddGeneralCell(
                        backgroundColor,
                        foreColor,
                        data,
                        columnNumber,
                        RowType.DataTableRow,
                        CellType.DataTableCell,
                        rowValues,
                        setItemTemplate: true);
        }

        /// <summary>
        /// Adds a new row number PdfPCell to the MainTable
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public CellAttributes AddRowNumberCell(BaseColor backgroundColor, BaseColor foreColor, int columnNumber)
        {
            return AddGeneralCell(
                backgroundColor,
                foreColor,
                CurrentRowInfoData.LastGroupRowNumber,
                columnNumber,
                RowType.DataTableRow,
                CellType.DataTableCell);
        }

        /// <summary>
        /// Adds a new Summary PdfPCell to the MainTable
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="columnNumber"></param>
        /// <param name="pdfCellType"></param>
        /// <param name="pdfRowType"></param>
        /// <returns></returns>
        public CellAttributes AddSummaryCell(BaseColor backgroundColor, BaseColor foreColor, int columnNumber, CellType pdfCellType, RowType pdfRowType)
        {
            var horizontalAlignment = SharedData.SummarySettings.OverallSummarySettings.LabelHorizontalAlignment;
            var text = SharedData.SummarySettings.OverallSummarySettings.Label;

            if (ShowAllGroupsSummaryRow && pdfRowType == RowType.AllGroupsSummaryRow)
            {
                horizontalAlignment = SharedData.SummarySettings.AllGroupsSummarySettings.LabelHorizontalAlignment;
                text = SharedData.SummarySettings.AllGroupsSummarySettings.Label;
            }

            if (pdfCellType == CellType.PageSummaryCell)
            {
                text = SharedData.SummarySettings.PageSummarySettings.Label;
                horizontalAlignment = SharedData.SummarySettings.PageSummarySettings.LabelHorizontalAlignment;
            }

            return AddGeneralCell(
                            backgroundColor,
                            foreColor,
                            text,
                            columnNumber,
                            pdfRowType,
                            pdfCellType,
                            horizontalAlignment: horizontalAlignment.HasValue ? horizontalAlignment.Value : HorizontalAlignment.None);
        }

        /// <summary>
        /// Adds a new Summary PdfPCell to the MainTable
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="rawData"></param>
        /// <param name="columnNumber"></param>
        /// <param name="pdfRowType"></param>
        /// <param name="pdfCellType"></param>
        /// <returns></returns>
        public CellAttributes AddSummaryCell(BaseColor backgroundColor, BaseColor foreColor, object rawData, int columnNumber, RowType pdfRowType, CellType pdfCellType)
        {
            return AddGeneralCell(backgroundColor, foreColor, rawData, columnNumber, pdfRowType, pdfCellType);
        }
        // Private Methods (1)

        private static void checkProperty(int columnNumber, ColumnAttributes col)
        {
            if (col == null)
                throw new ArgumentNullException("col");

            if (string.IsNullOrEmpty(col.PropertyName))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "pdfColumnsDefinitions[{0}].PropertyName is empty.", columnNumber));
            }
        }

        #endregion Methods
    }
}
