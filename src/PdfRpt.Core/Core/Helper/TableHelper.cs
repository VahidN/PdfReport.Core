using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// PdfGrid helper class
    /// </summary>
    public static class TableHelper
    {
        #region Methods (6)

        // Public Methods (6)

        /// <summary>
        /// Adds a border to an existing PdfGrid
        /// </summary>
        /// <param name="table">Table</param>
        /// <param name="borderColor">Border's color</param>
        /// <param name="spacingBefore">Spacing before the table</param>
        /// <returns>A new PdfGrid</returns>
        public static PdfGrid AddBorderToTable(this PdfGrid table, BaseColor borderColor, float spacingBefore)
        {
            var outerTable = new PdfGrid(numColumns: 1)
            {
                WidthPercentage = table.WidthPercentage,
                SpacingBefore = spacingBefore
            };
            var pdfCell = new PdfPCell(table) { BorderColor = borderColor };
            outerTable.AddCell(pdfCell);
            return outerTable;
        }

        /// <summary>
        /// Adds a border to an existing PdfGrid
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>A new PdfGrid</returns>
        public static PdfGrid AddBorderToTable(this PdfGrid table)
        {
            return AddBorderToTable(table, BaseColor.LightGray, 0);
        }

        /// <summary>
        /// Adds a SummaryRow to an existing PdfGrid
        /// </summary>
        /// <param name="table">An existing PdfGrid</param>
        /// <param name="pdfColumnsDefinitions">List of the PdfColumnAttributes</param>
        /// <param name="summaryProperty">Sets the location of summary cell's data</param>
        /// <param name="labelProperty">Sets the location of summary cell's label</param>
        /// <param name="summaryCell">SummaryCell's Attributes</param>
        /// <param name="labelCell">LabelCell's Attributes</param>
        /// <param name="emptyCell">The other not in use cell's Attributes</param>
        /// <param name="itemsTemplate">Default ItemsTemplate</param>
        public static void AddSummaryRow(this PdfGrid table,
                                         IList<ColumnAttributes> pdfColumnsDefinitions,
                                         string summaryProperty,
                                         string labelProperty,
                                         CellAttributes summaryCell,
                                         CellAttributes labelCell,
                                         CellAttributes emptyCell,
                                         IColumnItemsTemplate itemsTemplate)
        {
            foreach (var col in pdfColumnsDefinitions)
            {
                if (col.PropertyName == summaryProperty)
                {
                    table.AddCell(summaryCell.CreateSafePdfPCell(itemsTemplate));
                }
                else if (col.PropertyName == labelProperty)
                {
                    table.AddCell(labelCell.CreateSafePdfPCell(itemsTemplate));
                }
                else
                {
                    table.AddCell(emptyCell.CreateSafePdfPCell(itemsTemplate));
                }
            }
        }

        /// <summary>
        /// Adds a new row to the specified table.
        /// The default IColumnItemsTemplate would be TextBlockField.
        /// </summary>
        public static void AddSimpleRow(this PdfGrid table, params Action<CellRowData, CellBasicProperties>[] cellsData)
        {
            if (table.NumberOfColumns != cellsData.Length)
                throw new InvalidOperationException("table.NumberOfColumns(" + table.NumberOfColumns + ") != cellsData.Length(" + cellsData.Length + ")");

            foreach (var item in cellsData)
            {
                addSimpleRowCell(table, item);
            }
        }

        private static void addSimpleRowCell(PdfGrid table, Action<CellRowData, CellBasicProperties> cellDataItem)
        {
            var cellBasicProperties = new CellBasicProperties
            {
                BorderColor = BaseColor.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                RunDirection = PdfRunDirection.LeftToRight,
                FontColor = new BaseColor(Color.Black.ToArgb()),
                BackgroundColor = BaseColor.White,
                PdfFontStyle = DocumentFontStyle.Normal
            };
            var cellData = new CellRowData { Value = string.Empty, FormattedValue = string.Empty };

            if (cellDataItem != null)
                cellDataItem(cellData, cellBasicProperties);

            if (cellData.CellTemplate == null)
                cellData.CellTemplate = new TextBlockField();

            var cellAttributes = new CellAttributes
            {
                BasicProperties = cellBasicProperties,
                RowData = cellData
            };
            table.AddCell(cellAttributes.CreateSafePdfPCell(cellData.CellTemplate));
        }

        /// <summary>
        /// To add manual AddSummaryRows, we need to create a clone of the MainTable's structure.
        /// </summary>
        /// <param name="pageSetup">Document settings</param>
        /// <param name="pdfColumnsDefinitions">List of the PdfColumnAttributes</param>
        /// <returns>A PdfGrid</returns>
        public static PdfGrid CloneMainTableStructure(DocumentPreferences pageSetup, IList<ColumnAttributes> pdfColumnsDefinitions)
        {
            if (pageSetup.GroupsPreferences == null || pageSetup.GroupsPreferences.GroupType == GroupType.HideGroupingColumns)
            {
                pdfColumnsDefinitions = pdfColumnsDefinitions.Where(x => x.IsVisible && !x.IncludeInGrouping).OrderBy(x => x.Order).ToList();
            }

            if (pageSetup.GroupsPreferences != null && pageSetup.GroupsPreferences.GroupType == GroupType.IncludeGroupingColumns)
            {
                pdfColumnsDefinitions = pdfColumnsDefinitions.Where(x => x.IsVisible || x.IncludeInGrouping).OrderBy(x => x.Order).ToList();
            }

            var widths = pageSetup.PagePreferences.RunDirection == PdfRunDirection.LeftToRight
                ? pdfColumnsDefinitions.OrderBy(x => x.Order).Select(x => x.Width).ToArray()
                : pdfColumnsDefinitions.OrderBy(x => x.Order).Select(x => x.Width).Reverse().ToArray();

            if (pageSetup.PagePreferences.RunDirection == null)
                pageSetup.PagePreferences.RunDirection = PdfRunDirection.LeftToRight;

            var mainTable = new PdfGrid(widths.Length)
            {
                RunDirection = (int)pageSetup.PagePreferences.RunDirection,
                WidthPercentage = pageSetup.MainTablePreferences.WidthPercentage,
                SplitLate = pageSetup.MainTablePreferences.SplitLate,
                SpacingAfter = pageSetup.MainTablePreferences.SpacingAfter,
                SpacingBefore = pageSetup.MainTablePreferences.SpacingBefore,
                KeepTogether = pageSetup.MainTablePreferences.KeepTogether,
                SplitRows = pageSetup.MainTablePreferences.SplitRows
            };

            switch (pageSetup.MainTablePreferences.ColumnsWidthsType)
            {
                case TableColumnWidthType.Relative:
                    if(pageSetup.MainTablePreferences.TableType ==  TableType.NormalTable)
                    mainTable.SetWidths(widths);
                    break;
                case TableColumnWidthType.Absolute:
                    if (pageSetup.MainTablePreferences.TableType == TableType.NormalTable)
                    mainTable.SetTotalWidth(widths);
                    break;
                case TableColumnWidthType.FitToContent:
                    break;
                case TableColumnWidthType.EquallySized:
                    break;
            }

            return mainTable;
        }

        /// <summary>
        /// Creates a new PdfGrid with one column and row.
        /// </summary>
        /// <param name="phrase">An optional phrase to display</param>
        /// <param name="widthPercentage">Width of the table</param>
        /// <param name="fixedHeight">Height of the table</param>
        /// <param name="border">Border width</param>
        /// <returns>A PdfGrid</returns>
        public static PdfGrid CreateEmptyRowTable(string phrase = " ", float widthPercentage = 100, float fixedHeight = 35, int border = 0)
        {
            var table = new PdfGrid(1) { WidthPercentage = widthPercentage };
            var emptyHeaderCell = new PdfPCell(new Phrase(phrase)) { Border = border, FixedHeight = fixedHeight };
            table.AddCell(emptyHeaderCell);
            return table;
        }

        /// <summary>
        /// Creates a simple PdfGrid.
        /// </summary>
        /// <param name="columnsNumber">number of columns</param>
        /// <param name="pdfCellAttributesList">PdfCells Attributes List</param>
        /// <param name="showBorder">Indicates visibility of the table's border</param>
        /// <returns>A PdfGrid</returns>
        public static PdfGrid SimpleTable(int columnsNumber, IList<CellAttributes> pdfCellAttributesList, bool showBorder)
        {
            return SimpleTable(columnsNumber, pdfCellAttributesList, BaseColor.White, showBorder);
        }

        /// <summary>
        /// Creates a simple PdfGrid.
        /// </summary>
        /// <param name="columnsNumber">number of columns</param>
        /// <param name="pdfCellAttributesList">PdfCells Attributes List</param>
        /// <returns>A PdfGrid</returns>
        public static PdfGrid SimpleTable(int columnsNumber, IList<CellAttributes> pdfCellAttributesList)
        {
            return SimpleTable(columnsNumber, pdfCellAttributesList, BaseColor.White);
        }

        /// <summary>
        /// Creates a simple PdfGrid.
        /// </summary>
        /// <param name="columnsNumber">Number of columns</param>
        /// <param name="pdfCellAttributesList">PdfCells Attributes List</param>
        /// <param name="nullRowBackgroundColor">You can set the cells attributes to null. nullRowBackgroundColor indicates background color of these cells. Default color is white here.</param>
        /// <param name="showBorder">Indicates visibility of the table's border</param>
        /// <returns>A PdfGrid</returns>
        public static PdfGrid SimpleTable(int columnsNumber, IList<CellAttributes> pdfCellAttributesList, BaseColor nullRowBackgroundColor, bool showBorder = true)
        {
            var table = new PdfGrid(numColumns: columnsNumber) { WidthPercentage = 100 };

            for (var i = 0; i < pdfCellAttributesList.Count; i += columnsNumber)
            {
                var row = pdfCellAttributesList.Skip(i).Take(columnsNumber);
                var nonNullCells = row.Where(x => x != null).ToList();
                if (nonNullCells.Count == 1)
                {
                    //merge null cells
                    var rowCell = nonNullCells[0].CreateSafePdfPCell(new TextBlockField());
                    rowCell.Colspan = columnsNumber;
                    table.AddCell(rowCell);
                }
                else
                {
                    foreach (var cell in row)
                    {
                        var rowCell = cell == null
                            ? new PdfPCell { Border = 0, BackgroundColor = nullRowBackgroundColor }
                            : cell.CreateSafePdfPCell(new TextBlockField());
                        table.AddCell(rowCell);
                    }
                }
            }

            return showBorder ? AddBorderToTable(table) : table;
        }

        /// <summary>
        /// It will be used for OnDataSourceIsEmptyEvent.
        /// </summary>
        /// <param name="pdfDoc">Pdf document object</param>
        /// <param name="pdfRptFont">fonts</param>
        /// <param name="runDirection">A possible run direction value, left-to-right or right-to-left</param>
        /// <param name="message">a message to show</param>
        /// <returns>PdfGrid</returns>
        public static void AddDefaultEmptyDataSourceTable(Document pdfDoc, IPdfFont pdfRptFont, PdfRunDirection? runDirection, string message = "There is no data available to display.")
        {
            var table = SimpleTable(
                        columnsNumber: 1,
                        pdfCellAttributesList: new List<CellAttributes>
                        {
                            null,
                            new CellAttributes
                            {
                                RowData = new CellRowData
                                {
                                    Value = message
                                },
                                ItemTemplate = new TextBlockField(),
                                BasicProperties = new CellBasicProperties
                                {
                                    BackgroundColor = BaseColor.White,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    ShowBorder = false,
                                    RunDirection = runDirection.HasValue? runDirection.Value : PdfRunDirection.LeftToRight,
                                    PdfFont = pdfRptFont,
                                    FontColor = new BaseColor(Color.Black.ToArgb()),
                                }
                            },
                            null
                        },
                        showBorder: true
                );
            pdfDoc.Add(table);
        }

        #endregion Methods
    }
}
