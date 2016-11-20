using System.Collections.Generic;

namespace PdfRpt.Calendar
{
    /// <summary>
    /// MonthCalendar's Data.
    /// </summary>
    public class CalendarData
    {
        /// <summary>
        /// MonthCalendar's month number.
        /// It will be interpreted based on the CalendarType's value.
        /// </summary>
        public int Month { set; get; }

        /// <summary>
        /// MonthCalendar's day number.
        /// It will be interpreted based on the CalendarType's value.
        /// </summary>
        public int Year { set; get; }

        /// <summary>
        /// Associated descriptions of the MonthCalendar's days.
        /// </summary>
        public IList<DayInfo> MonthDaysInfo { set; get; }
    }
}
