using System;
using System.Drawing;
using iTextSharp.text;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Calendar
{
    /// <summary>
    /// MonthCalendar's attributes
    /// </summary>
    public class CalendarAttributes
    {
        /// <summary>
        /// If true, splits rows that are forwarded to the next page but that still
        /// don't fit because the row height exceeds the available page height.
        /// </summary>
        public bool SplitRows { set; get; }

        /// <summary>
        /// BackgroundColor of the day names row.
        /// Its default value is WhiteSmoke.
        /// </summary>
        public BaseColor DayNamesRowBackgroundColor { set; get; }

        /// <summary>
        /// Description text's horizontal alignment
        /// </summary>
        public HorizontalAlignment DescriptionHorizontalAlignment { set; get; }

        /// <summary>
        /// MonthCalendar's TablePadding.
        /// Its default value is 5.
        /// </summary>
        public float Padding { set; get; }

        /// <summary>
        /// Type of the in use calendar.
        /// </summary>
        public CalendarType CalendarType { set; get; }

        /// <summary>
        /// This callback will be called before adding a cell to the table. It's useful for conditional formatting.
        /// </summary>
        public Action<MonthTableCell> CellsCustomizer { set; get; }

        /// <summary>
        /// Gradient's Start Color. Set it to null to make it disappear.
        /// Its default value is LightGray.
        /// </summary>
        public BaseColor GradientStartColor { set; get; }

        /// <summary>
        /// Gradient's End Color. Set it to null to make it disappear.
        /// Its default value is DarkGray.
        /// </summary>
        public BaseColor GradientEndColor { set; get; }

        /// <summary>
        /// MonthCalendar's border color.
        /// </summary>
        public BaseColor BorderColor { set; get; }

        /// <summary>
        /// MonthCalendar's font.
        /// </summary>
        public IPdfFont Font { set; get; }

        /// <summary>
        /// Should I display "Sunday" or "Su"?
        /// Its default value is true.
        /// </summary>
        public bool UseLongDayNamesOfWeek { set; get; }

        /// <summary>
        /// Relative Column Widths of the MonthCalendar.
        /// Its default value is new float[] { 1, 1, 1, 1, 1, 1, 1 };
        /// </summary>
        public float[] RelativeColumnWidths { set; get; }

        /// <summary>
        /// MonthCalendar's attributes.
        /// </summary>
        public CalendarAttributes()
        {
            RelativeColumnWidths = new float[] { 1, 1, 1, 1, 1, 1, 1 };
            BorderColor = BaseColor.LightGray;
            GradientStartColor = new BaseColor(Color.LightGray.ToArgb());
            GradientEndColor = new BaseColor(Color.DarkGray.ToArgb());
            Padding = 5;
            UseLongDayNamesOfWeek = true;
            DayNamesRowBackgroundColor = new BaseColor(Color.WhiteSmoke.ToArgb());
        }
    }
}
