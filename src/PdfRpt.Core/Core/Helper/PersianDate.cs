using System;
using System.Globalization;
using PdfRpt.Calendar;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Persian Date Converter
    /// </summary>
    public static class PersianDate
    {
        /// <summary>
        /// RTL Embedding Char, 0x202B.
        /// </summary>
        public const char RightToLeftEmbedding = (char)0x202B;

        /// <summary>
        /// Pop Directional Formatting Char, 0x202C.
        /// </summary>
        public const char PopDirectionalFormatting = (char)0x202C;

        /// <summary>
        /// If you see dd/mm/yyy instead of yyyy/mm/dd in your RTL reports, use this method to fix it.
        /// </summary>
        /// <param name="data">string data</param>
        /// <returns>A fixed string</returns>
        public static string FixWeakCharacters(this string data)
        {
            if (string.IsNullOrEmpty(data)) return string.Empty;
            var weakCharacters = new[] { @"\", "/", "+", "-", "=", ";", "$", ":" };
            foreach (var weakCharacter in weakCharacters)
            {
                data = data.Replace(weakCharacter, RightToLeftEmbedding + weakCharacter + PopDirectionalFormatting);
            }
            return data;
        }

        /// <summary>
        /// Converts Gregorian date to Shamsi/Persian date
        /// </summary>
        /// <param name="gregorianDate">Gregorian date</param>
        /// <param name="dateSeparator">Defines an optional separator between date's parts. Its default value is /</param>
        /// <param name="includeHourMinute">Should converter include hour and minutes in final result. Its default value is true</param>
        /// <param name="showLeftAlignedHourMinute">If includeHourMinute is true, indicates whether to show hh:mm yyyy/mm/dd or yyyy/mm/dd hh:mm</param>
        /// <param name="timeSeparator">Defines an optional separator between time's parts. Its default value is :</param>
        /// <returns>Persian/Shamsi DateTime string</returns>
        public static string ToPersianDateTime(this DateTime gregorianDate, string dateSeparator = "/", bool includeHourMinute = true, bool showLeftAlignedHourMinute = true, string timeSeparator = ":")
        {
            var gregorianYear = gregorianDate.Year;
            var gregorianMonth = gregorianDate.Month;
            var gregorianDay = gregorianDate.Day;
            var persianCalendar = new PersianCalendar();
            var dateTime = new DateTime(gregorianYear, gregorianMonth, gregorianDay);
            var persianYear = persianCalendar.GetYear(dateTime);
            var persianMonth = persianCalendar.GetMonth(dateTime);
            var persianDay = persianCalendar.GetDayOfMonth(dateTime);
            return includeHourMinute ?
                (showLeftAlignedHourMinute ? string.Format(CultureInfo.InvariantCulture, "{0}{6}{1}  {2}{3}{4}{3}{5}", gregorianDate.Hour, gregorianDate.Minute, persianYear, dateSeparator, persianMonth.ToString("00", CultureInfo.InvariantCulture), persianDay.ToString("00", CultureInfo.InvariantCulture), timeSeparator)
                : string.Format(CultureInfo.InvariantCulture, "{2}{3}{4}{3}{5}  {0}{6}{1}", gregorianDate.Hour, gregorianDate.Minute, persianYear, dateSeparator, persianMonth.ToString("00", CultureInfo.InvariantCulture), persianDay.ToString("00", CultureInfo.InvariantCulture), timeSeparator))
                : string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{1}{3}", persianYear, dateSeparator, persianMonth.ToString("00", CultureInfo.InvariantCulture), persianDay.ToString("00", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Finds 1st day of the given year and month.
        /// </summary>
        public static int Find1StDayOfMonth(int year, int monthIndex, CalendarType calendarType = CalendarType.PersianCalendar)
        {
            if (calendarType == CalendarType.GregorianCalendar)
                return (int)new DateTime(year, monthIndex, 1).DayOfWeek;


            int outYear, outMonth, outDay, dayWeek = 1;
            HijriToGregorian(year, monthIndex, 1, out outYear, out outMonth, out outDay);

            var res = new DateTime(outYear, outMonth, outDay);

            switch (res.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    dayWeek = 0;
                    break;

                case DayOfWeek.Sunday:
                    dayWeek = 1;
                    break;

                case DayOfWeek.Monday:
                    dayWeek = 2;
                    break;

                case DayOfWeek.Tuesday:
                    dayWeek = 3;
                    break;

                case DayOfWeek.Wednesday:
                    dayWeek = 4;
                    break;

                case DayOfWeek.Thursday:
                    dayWeek = 5;
                    break;

                case DayOfWeek.Friday:
                    dayWeek = 6;
                    break;
            }

            return dayWeek;
        }

        /// <summary>
        /// Converts Hijri date To Gregorian date.
        /// </summary>
        public static void HijriToGregorian(
                    int inYear, int inMonth, int inDay,
                    out int outYear, out int outMonth, out int outDay)
        {
            var ys = inYear;
            var ms = inMonth;
            var ds = inDay;

            var gregorianCalendar = new GregorianCalendar();
            var dateTime = new PersianCalendar().ToDateTime(ys, ms, ds, 0, 0, 0, 0);
            outYear = gregorianCalendar.GetYear(dateTime);
            outMonth = gregorianCalendar.GetMonth(dateTime);
            outDay = gregorianCalendar.GetDayOfMonth(dateTime);
        }

        /// <summary>
        /// Is a given year leap?
        /// </summary>
        public static bool IsLeapYear(this int year, CalendarType calendarType = CalendarType.PersianCalendar)
        {
            if (calendarType == CalendarType.GregorianCalendar)
                return DateTime.IsLeapYear(year);

            var r = year % 33;
            return (r == 1 || r == 5 || r == 9 || r == 13 || r == 17 || r == 22 || r == 26 || r == 30);
        }
    }
}