using iTextSharp.text.pdf;

namespace PdfRpt.Calendar
{
    /// <summary>
    /// Represents a MonthCalendar's cell.
    /// </summary>
    public class MonthTableCell
    {
        /// <summary>
        /// MonthCalendar's day number.
        /// It will be interpreted based on the CalendarType's value.
        /// </summary>
        public int DayNumber { set; get; }

        /// <summary>
        /// MonthCalendar's month number.
        /// It will be interpreted based on the CalendarType's value.
        /// </summary>
        public int Month { set; get; }

        /// <summary>
        /// MonthCalendar's year number.
        /// It will be interpreted based on the CalendarType's value.
        /// </summary>
        public int Year { set; get; }

        /// <summary>
        /// Each cell of the calendar is composed of NumberCell and DescriptionCell.
        /// </summary>
        public PdfPCell NumberCell { set; get; }

        /// <summary>
        /// Each cell of the calendar is composed of NumberCell and DescriptionCell.
        /// It can be null.
        /// </summary>
        public PdfPCell DescriptionCell { set; get; }
    }
}