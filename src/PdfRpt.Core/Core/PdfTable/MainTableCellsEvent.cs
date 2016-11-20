using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// This class allows accessing cell's canvas after finishing its rendering to add additional text or graphics.
    /// At this point we can add summary cells data.
    /// </summary>
    public class MainTableCellsEvent : IPdfPCellEvent
    {
        private readonly CellAttributes _pdfRptCell;

        /// <summary>
        /// Allows accessing cell's canvas after finishing its rendering to add additional text or graphics
        /// </summary>
        /// <param name="pdfRptCell">Related cell's attributes</param>
        public MainTableCellsEvent(CellAttributes pdfRptCell)
        {
            _pdfRptCell = pdfRptCell;
        }

        /// <summary>
        /// Represents a main table's cell type.
        /// </summary>
        public CellType CellType { set; get; }

        /// <summary>
        /// Holds shared info between rendering classes.
        /// </summary>
        public SharedData SharedData { set; get; }

        /// <summary>
        /// List of the SummaryCells Data
        /// </summary>
        public IList<SummaryCellData> SummaryCellsData { set; get; }

        /// <summary>
        /// Holds last result of the actual rendering engine of iTextSharp during its processes.
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// Indicates grouping is enabled or not
        /// </summary>
        public bool IsGroupingEnabled { set; get; }

        /// <summary>
        /// This method is called at the end of the cell's rendering.
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            setLastRenderedRowNumber();

            applyCustomCellLayout(cell, position, canvases);
            applyGradientBackground(position, canvases);

            if (_pdfRptCell.SharedData.PdfColumnAttributes == null) return;
            if (_pdfRptCell.SharedData.PdfColumnAttributes.AggregateFunction == null) return;
            if (_pdfRptCell.SharedData.PdfColumnAttributes.IsRowNumber) return;

            if (_pdfRptCell.RowData.PdfRowType == RowType.PreviousPageSummaryRow ||
                _pdfRptCell.RowData.PdfRowType == RowType.AllGroupsSummaryRow ||
                _pdfRptCell.RowData.PdfRowType == RowType.PageSummaryRow ||
                _pdfRptCell.RowData.PdfRowType == RowType.SummaryRow)
            {
                setPagesInfo();
                printSummary(position, canvases);
            }
        }

        private void applyGradientBackground(Rectangle position, PdfContentByte[] canvases)
        {
            GradientBackground.ApplyGradientBackground(
                _pdfRptCell.RowData.PdfRowType,
                _pdfRptCell.SharedData,
                position,
                canvases);
        }

        private void applyCustomCellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            if (_pdfRptCell.ItemTemplate == null) return;
            _pdfRptCell.ItemTemplate.CellRendered(cell, position, canvases, _pdfRptCell);
        }

        private object getRowSummaryData()
        {
            return _pdfRptCell.RowData.PdfRowType == RowType.PageSummaryRow ? thisPageSummary() : getTotalSummaries();
        }

        private object getTotalSummaries()
        {
            var row = CurrentRowInfoData.LastRenderedRowNumber;
            var propertyName = _pdfRptCell.SharedData.PdfColumnAttributes.PropertyName;

            var result = SummaryCellsData.FirstOrDefault(x => x.CellData.PropertyName == propertyName &&
                                                              x.OverallRowNumber == row);

            if (result == null)
            {
                return string.Empty;
            }

            object data;
            if (IsGroupingEnabled &&
                (_pdfRptCell.RowData.PdfRowType == RowType.SummaryRow ||
                _pdfRptCell.RowData.PdfRowType == RowType.PreviousPageSummaryRow))
            {
                data = result.GroupAggregateValue;
            }
            else
            {
                data = result.OverallAggregateValue;
            }

            return data;
        }

        private void printSummary(Rectangle position, PdfContentByte[] canvases)
        {
            var rowSummaryDataObj = getRowSummaryData() ?? string.Empty;

            var rowSummaryData = FuncHelper.ApplyFormula(_pdfRptCell.SharedData.PdfColumnAttributes.AggregateFunction.DisplayFormatFormula, rowSummaryDataObj);

            if (!_pdfRptCell.BasicProperties.RunDirection.HasValue)
                _pdfRptCell.BasicProperties.RunDirection = PdfRunDirection.LeftToRight;

            if (_pdfRptCell.BasicProperties.RunDirection == PdfRunDirection.RightToLeft)
                rowSummaryData = rowSummaryData.FixWeakCharacters();

            var formattedValue = rowSummaryData.ToSafeString();
            setCellData(rowSummaryDataObj, formattedValue);
            setMainTableEvents();

            var phrase = _pdfRptCell.BasicProperties.PdfFont.FontSelector.Process(_pdfRptCell.RowData.FormattedValue);
            var alignment = getAggregateValuePosition(position);
            ColumnText.ShowTextAligned(
                                    canvas: canvases[PdfPTable.TEXTCANVAS],
                                    alignment: (int)alignment.HorizontalAlignment,
                                    phrase: phrase,
                                    x: alignment.X,
                                    y: ((position.Bottom + position.Top) / 2) - 4,
                                    rotation: 0,
                                    runDirection: (int)_pdfRptCell.BasicProperties.RunDirection,
                                    arabicOptions: 0);
        }

        private AggregateValuePosition getAggregateValuePosition(Rectangle position)
        {
            float x;
            if (!_pdfRptCell.BasicProperties.HorizontalAlignment.HasValue)
                _pdfRptCell.BasicProperties.HorizontalAlignment = HorizontalAlignment.Center;

            if (!_pdfRptCell.BasicProperties.RunDirection.HasValue)
                _pdfRptCell.BasicProperties.RunDirection = PdfRunDirection.LeftToRight;

            var emptyCell = new PdfPCell();
            var paddingLeft = _pdfRptCell.BasicProperties.PaddingLeft;
            if (paddingLeft.ApproxEquals(0))
            {
                paddingLeft = emptyCell.PaddingLeft;
            }

            var paddingRight = _pdfRptCell.BasicProperties.PaddingRight;
            if (paddingRight.ApproxEquals(0))
            {
                paddingRight = emptyCell.PaddingRight;
            }

            var borderWidth = _pdfRptCell.BasicProperties.BorderWidth;
            if (borderWidth.ApproxEquals(0))
            {
                borderWidth = emptyCell.BorderWidth;
            }

            var alignment = _pdfRptCell.BasicProperties.HorizontalAlignment.Value;
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    if (_pdfRptCell.BasicProperties.RunDirection.Value == PdfRunDirection.RightToLeft)
                    {
                        x = position.Right - paddingRight - borderWidth;
                        alignment = HorizontalAlignment.Right;
                    }
                    else
                    {
                        x = position.Left + paddingLeft + borderWidth;
                    }
                    break;

                case HorizontalAlignment.Right:
                    if (_pdfRptCell.BasicProperties.RunDirection.Value == PdfRunDirection.RightToLeft)
                    {
                        x = position.Left + paddingLeft + borderWidth;
                        alignment = HorizontalAlignment.Left;
                    }
                    else
                    {
                        x = position.Right - paddingRight - borderWidth;
                    }
                    break;

                default:
                    x = (position.Left + position.Right) / 2;
                    break;
            }

            return new AggregateValuePosition
            {
                HorizontalAlignment = alignment,
                X = x
            };
        }

        private void setMainTableEvents()
        {
            if (SharedData.MainTableEvents == null) return;
            SharedData.MainTableEvents.CellCreated(new EventsArguments
            {
                PdfDoc = SharedData.PdfDoc,
                PdfWriter = SharedData.PdfWriter,
                Cell = _pdfRptCell,
                CellType = CellType,
                RowType = _pdfRptCell.RowData.PdfRowType,
                ColumnNumber = _pdfRptCell.RowData.ColumnNumber,
                ColumnCellsSummaryData = SharedData.ColumnCellsSummaryData,
                PreviousTableRowData = CurrentRowInfoData.PreviousTableRowData,
                PageSetup = SharedData.PageSetup,
                PdfFont = SharedData.PdfFont,
                PdfColumnsAttributes = SharedData.PdfColumnsAttributes,
                TableRowData = getTableRowData()
            });
        }

        private List<CellData> getTableRowData()
        {
            return SharedData.ColumnCellsFinalSummaryData
                             .Where(x => x.LastRenderedRowNumber == CurrentRowInfoData.LastRenderedRowNumber &&
                                         x.PdfRowType == _pdfRptCell.RowData.PdfRowType)
                             .Select(x => new CellData
                             {
                                 PropertyValue = x.Value,
                                 FormattedValue = x.FormattedValue,
                                 PropertyName = x.PropertyName,
                                 PropertyIndex = x.ColumnNumber,
                                 PropertyType = x.PropertyType
                             })
                             .ToList();
        }

        private void setCellData(object rowSummaryData, string formattedValue)
        {
            if (_pdfRptCell.RowData == null)
            {
                _pdfRptCell.RowData = new CellRowData();
            }

            _pdfRptCell.RowData.ColumnNumber = _pdfRptCell.RowData.ColumnNumber;
            _pdfRptCell.RowData.FormattedValue = formattedValue;
            _pdfRptCell.RowData.PdfRowType = _pdfRptCell.RowData.PdfRowType;
            _pdfRptCell.RowData.Value = rowSummaryData;
            _pdfRptCell.RowData.PropertyName = _pdfRptCell.SharedData.PdfColumnAttributes.PropertyName;
            _pdfRptCell.RowData.LastRenderedRowNumber = CurrentRowInfoData.LastRenderedRowNumber;

            SharedData.ColumnCellsFinalSummaryData.Add(_pdfRptCell.RowData);
        }

        private void setLastRenderedRowNumber()
        {
            if (_pdfRptCell.SharedData.DataRowNumber <= 0) return;
            if (_pdfRptCell.SharedData.DataRowNumber <= CurrentRowInfoData.LastRenderedRowNumber) return;

            CurrentRowInfoData.LastRenderedRowNumber = _pdfRptCell.SharedData.DataRowNumber;
            CurrentRowInfoData.LastRenderedGroupNumber = _pdfRptCell.SharedData.GroupNumber;
        }

        private void setPagesInfo()
        {
            if (CurrentRowInfoData.PagesBoundaries == null)
            {
                CurrentRowInfoData.PagesBoundaries = new List<int>();
            }

            if (!CurrentRowInfoData.PagesBoundaries.Contains(CurrentRowInfoData.LastRenderedRowNumber))
            {
                CurrentRowInfoData.PagesBoundaries.Add(CurrentRowInfoData.LastRenderedRowNumber);
            }
        }

        private object thisPageSummary()
        {
            var pageBoundary = CurrentRowInfoData.PagesBoundaries.OrderByDescending(x => x).Take(2).ToList();
            if (!pageBoundary.Any()) return string.Empty;

            int firstRowOfThePage, lastRowOfThePage;
            if (pageBoundary.Count == 1)
            {
                firstRowOfThePage = 1;
                lastRowOfThePage = pageBoundary[0];
            }
            else
            {
                firstRowOfThePage = pageBoundary[1] + 1;
                lastRowOfThePage = pageBoundary[0];
            }

            var propertyName = _pdfRptCell.SharedData.PdfColumnAttributes.PropertyName;
            var list = SummaryCellsData.Where(x => x.CellData.PropertyName == propertyName
                                           && x.OverallRowNumber >= firstRowOfThePage && x.OverallRowNumber <= lastRowOfThePage)
                                       .ToList();
            return _pdfRptCell.SharedData.PdfColumnAttributes.AggregateFunction.ProcessingBoundary(list);
        }
    }
}