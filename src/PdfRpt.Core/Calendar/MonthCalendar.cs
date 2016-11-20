using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Helper;

namespace PdfRpt.Calendar
{
    /// <summary>
    /// Draws a MonthCalendar
    /// </summary>
    public class MonthCalendar
    {
        #region Fields

        readonly StringBuilder _footerText = new StringBuilder();
        PdfPTable _mainTable;
        string _monthName = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// MonthCalendar's attributes
        /// </summary>
        public CalendarAttributes CalendarAttributes { set; get; }

        /// <summary>
        /// MonthCalendar's Data.
        /// </summary>
        public CalendarData CalendarData { set; get; }


        private int runDirection
        {
            get
            {
                return CalendarAttributes.CalendarType == CalendarType.PersianCalendar ? PdfWriter.RUN_DIRECTION_RTL : PdfWriter.RUN_DIRECTION_LTR;
            }
        }

        #endregion Properties

        #region Methods

        // Public Methods

        /// <summary>
        /// Draws a MonthCalendar
        /// </summary>
        /// <returns>A PdfPTable</returns>
        public PdfPTable CreateMonthCalendar()
        {
            createMainTable();
            addMonthNameRow();
            addDayNamesRow();
            drawMonthCalendar();
            addTextRow(_footerText.ToString(), Element.ALIGN_LEFT);

            return _mainTable;
        }
        // Private Methods

        private void addCells(IEnumerable<MonthTableCell> monthCells)
        {
            int number = 0;
            foreach (var cell in monthCells)
            {
                number++;
                if (number == 36)
                {
                    if (cell.DayNumber == 0)
                        return;
                }

                if (cell.DayNumber == 0)
                {
                    var empryCell = new PdfPCell();
                    setCommonPdfPCellProperties(empryCell);
                    _mainTable.AddCell(empryCell);
                    continue;
                }

                cell.DescriptionCell = createCellDescription(cell);
                if (cell.DescriptionCell == null)
                {
                    if (CalendarAttributes.CellsCustomizer != null)
                        CalendarAttributes.CellsCustomizer(cell);

                    _mainTable.AddCell(cell.NumberCell);
                }
                else
                {
                    var cellTable = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        RunDirection = runDirection
                    };
                    cell.NumberCell.Border = 0;
                    cell.DescriptionCell.Border = 0;
                    cell.DescriptionCell.HorizontalAlignment = (int)CalendarAttributes.DescriptionHorizontalAlignment;

                    if (CalendarAttributes.CellsCustomizer != null)
                        CalendarAttributes.CellsCustomizer(cell);

                    cellTable.AddCell(cell.NumberCell);
                    cellTable.AddCell(cell.DescriptionCell);
                    var tableCell = new PdfPCell(cellTable);
                    setCommonPdfPCellProperties(tableCell);
                    _mainTable.AddCell(tableCell);
                }
            }
        }

        private void addDayNamesRow()
        {
            var days = getDays();
            foreach (var day in days)
            {
                var cell = new PdfPCell(getPhrase(day));
                cell.BackgroundColor = CalendarAttributes.DayNamesRowBackgroundColor;
                setCommonPdfPCellProperties(cell);
                _mainTable.AddCell(cell);
            }
        }

        private Phrase getPhrase(string text)
        {
            if (string.IsNullOrEmpty(text))
                return CalendarAttributes.Font.FontSelector.Process(string.Empty);

            if (CalendarAttributes.CalendarType == CalendarType.GregorianCalendar)
                return CalendarAttributes.Font.FontSelector.Process(text);

            return CalendarAttributes.Font.FontSelector.Process(text.ToPersianNumbers().FixWeakCharacters());
        }

        private void addMonthNameRow()
        {
            setMonthName();
            var text = _monthName + " " + CalendarData.Year;
            addTextRow(text, showGradient: true);
        }

        private void addTextRow(string text, int horizontalAlignment = Element.ALIGN_CENTER, bool showGradient = false)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var monthNameCell = new PdfPCell(getPhrase(text));
            if (showGradient)
            {
                monthNameCell.CellEvent = new GradientCellEvent
                {
                    GradientEndColor = CalendarAttributes.GradientEndColor,
                    GradientStartColor = CalendarAttributes.GradientStartColor
                };
            }
            setCommonPdfPCellProperties(monthNameCell, horizontalAlignment);
            monthNameCell.Colspan = 7;
            _mainTable.AddCell(monthNameCell);
        }

        private PdfPCell createCellDescription(MonthTableCell cell)
        {
            var cellDescription = getCellDescription(cell);
            PdfPCell descriptionCell = null;
            if (!string.IsNullOrEmpty(cellDescription))
            {
                descriptionCell = new PdfPCell(getPhrase(cellDescription));
            }
            return descriptionCell;
        }

        private List<MonthTableCell> createEmptyCells()
        {
            var monthCells = new List<MonthTableCell>();
            for (var idx = 0; idx <= 41; idx++)
            {
                var numberCell = new PdfPCell();
                setCommonPdfPCellProperties(numberCell);
                var monthCell = new MonthTableCell
                {
                    NumberCell = numberCell,
                    DescriptionCell = null
                };
                monthCells.Add(monthCell);
            }
            return monthCells;
        }

        private void createMainTable()
        {
            _mainTable = new PdfPTable(CalendarAttributes.RelativeColumnWidths)
            {
                WidthPercentage = 100,
                RunDirection = runDirection,
                HeaderRows = 2,
                SplitRows = CalendarAttributes.SplitRows
            };
        }

        private void drawMonthCalendar()
        {
            var startDay = PersianDate.Find1StDayOfMonth(CalendarData.Year, CalendarData.Month, CalendarAttributes.CalendarType);

            int noOfDays;
            if (CalendarAttributes.CalendarType == CalendarType.PersianCalendar)
            {
                noOfDays = CalendarData.Month <= 6 ? 31 : 30;
            }
            else
            {
                noOfDays = DateTime.DaysInMonth(CalendarData.Year, CalendarData.Month);
            }

            if (CalendarData.Month == 12 && CalendarAttributes.CalendarType == CalendarType.PersianCalendar)
                noOfDays = CalendarData.Year.IsLeapYear(CalendarAttributes.CalendarType) ? 30 : 29;

            var monthCells = createEmptyCells();
            fillCells(startDay, noOfDays, monthCells);
            addCells(monthCells);
        }

        private void fillCells(int startDay, int noOfDays, IList<MonthTableCell> monthCells)
        {
            int i, j;
            for (i = startDay; i <= 6; i++)
            {
                var curDay = i - startDay + 1;
                monthCells[i].NumberCell.Phrase = getPhrase(curDay.ToString(CultureInfo.InvariantCulture));
                monthCells[i].Month = CalendarData.Month;
                monthCells[i].Year = CalendarData.Year;
                monthCells[i].DayNumber = curDay;
            }

            var k = 7;
            for (j = 6 - startDay + 1; j <= noOfDays - 1; j++)
            {
                var curDay = j + 1;
                monthCells[k].NumberCell.Phrase = getPhrase(curDay.ToString(CultureInfo.InvariantCulture));
                monthCells[k].Month = CalendarData.Month;
                monthCells[k].Year = CalendarData.Year;
                monthCells[k].DayNumber = curDay;
                k = k + 1;
            }
        }

        private string getCellDescription(MonthTableCell cell)
        {
            if (CalendarData.MonthDaysInfo == null || !CalendarData.MonthDaysInfo.Any())
                return string.Empty;

            var daysInfo = CalendarData.MonthDaysInfo.Where(x => x.DayNumber == cell.DayNumber).ToList();
            if (!daysInfo.Any())
                return string.Empty;

            var text = new StringBuilder();
            foreach (var day in daysInfo)
            {
                if (day.ShowDescriptionInFooter)
                    _footerText.AppendLine(day.DayNumber + " " + _monthName + ": " + day.Description);
                else
                    text.AppendLine(day.Description);
            }
            return text.ToString();
        }

        private IEnumerable<string> getDays()
        {
            var days = CalendarAttributes.UseLongDayNamesOfWeek ? CalendarNames.LongPersianDayNamesOfWeek : CalendarNames.ShortPersianDayNamesOfWeek;
            if (CalendarAttributes.CalendarType == CalendarType.GregorianCalendar)
            {
                days = CalendarAttributes.UseLongDayNamesOfWeek ? CalendarNames.LongGregorianDayNamesOfWeek : CalendarNames.ShortGregorianDayNamesOfWeek;
            }
            return days;
        }

        private void setCommonPdfPCellProperties(PdfPCell cell, int horizontalAlignment = Element.ALIGN_CENTER)
        {
            if (cell == null)
                return;

            cell.RunDirection = runDirection;
            cell.HorizontalAlignment = horizontalAlignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.UseAscender = true;
            cell.UseDescender = true;
            cell.BorderColor = CalendarAttributes.BorderColor;
        }

        private void setMonthName()
        {
            _monthName = CalendarAttributes.CalendarType == CalendarType.PersianCalendar ? CalendarNames.PersianMonthNames[CalendarData.Month - 1] : new DateTime(CalendarData.Year, CalendarData.Month, 1).ToString("MMM", CultureInfo.InvariantCulture);
        }

        #endregion Methods
    }
}
