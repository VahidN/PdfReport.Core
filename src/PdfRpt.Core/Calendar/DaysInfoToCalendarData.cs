using System.Collections.Generic;
using System.Linq;

namespace PdfRpt.Calendar
{
    /// <summary>
    /// Maps list of the DayInfo's to the list of CalendarData's.
    /// </summary>
    public static class DaysInfoToCalendarData
    {
        /// <summary>
        /// Maps list of the DayInfo's to the list of CalendarData's.
        /// </summary>
        /// <param name="dataRows">List of the DayInfo's</param>
        /// <returns>List of CalendarData's</returns>
        public static IList<CalendarData> MapToCalendarDataList(this IList<DayInfo> dataRows)
        {
            if (dataRows == null || !dataRows.Any())
                return null;

            var rows = dataRows.OrderBy(x => x.Year).ThenBy(x => x.Month);
            return rows.GroupBy(x =>
                new
                {
                    Year = x.Year,
                    Month = x.Month
                })
                .Select(x => new CalendarData
                {
                    Month = x.Key.Month,
                    Year = x.Key.Year,
                    MonthDaysInfo = x.ToList()
                })
                .ToList();
        }
    }
}