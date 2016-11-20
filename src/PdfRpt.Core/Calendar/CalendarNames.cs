
namespace PdfRpt.Calendar
{
    /// <summary>
    /// Default names of the Persian calendar.
    /// </summary>
    public static class CalendarNames
    {
        /// <summary>
        /// MonthNames in the Persian calendar.
        /// </summary>
        public static readonly string[] PersianMonthNames = new[]
            {
            "فروردين" ,
            "ارديبهشت" ,
            "خرداد" ,
            "تير" ,
            "مرداد" ,
            "شهريور" ,
            "مهر" ,
            "آبان" ,
            "آذر" ,
            "دي" ,
            "بهمن" ,
            "اسفند" 
            };

        /// <summary>
        /// DaysOfWeek in the Gregorian calendar.
        /// </summary>
        public static readonly string[] LongGregorianDayNamesOfWeek = new[] {
            "Sunday", 
            "Monday", 
            "Tuesday", 
            "Wednesday", 
            "Thursday", 
            "Friday", 
            "Saturday" 
        };

        /// <summary>
        /// DaysOfWeek in the Gregorian calendar.
        /// </summary>
        public static readonly string[] ShortGregorianDayNamesOfWeek = new[] {
            "Su", 
            "Mo", 
            "Tu", 
            "We", 
            "Th", 
            "Fr", 
            "Sa" 
        };

        /// <summary>
        /// DaysOfWeek in the Persian calendar.
        /// </summary>
        public static readonly string[] LongPersianDayNamesOfWeek = new[] {
                "شنبه", 
                "يك‌ شنبه", 
                "دو‌شنبه",
                "سه ‌شنبه", 
                "چهار ‌شنبه",
                "پنج ‌شنبه",
                "جمعه"
            };

        /// <summary>
        /// DaysOfWeek in the Persian calendar.
        /// </summary>
        public static readonly string[] ShortPersianDayNamesOfWeek = new[] {
                "ش", 
                "ى", 
                "د",
                "س", 
                "چ",
                "پ",
                "ج"
            };
    }
}