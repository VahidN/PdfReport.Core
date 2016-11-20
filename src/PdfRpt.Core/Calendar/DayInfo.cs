using System.Diagnostics;

namespace PdfRpt.Calendar
{
    /// <summary>
    /// Defines associated description of a MonthCalendar's day.
    /// </summary>
    [DebuggerDisplay("{DayNumber}-{Month}-{Year}-{Description}-{ShowDescriptionInFooter}")]
    public class DayInfo
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
        /// A text to show.
        /// Each day can have multiple DayInfo's.
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// If true, the Description will be displayed in the footer of the MonthCalendar,
        /// instead of displaying it in the associated cell.
        /// </summary>
        public bool ShowDescriptionInFooter { set; get; }
    }
}