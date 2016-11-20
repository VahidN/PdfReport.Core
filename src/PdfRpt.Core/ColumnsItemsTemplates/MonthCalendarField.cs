using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Calendar;
using PdfRpt.Core.Contracts;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Displaying current cell's data as a calendar.
    /// Calendar's cell data type should be PdfRpt.Calendar.CalendarData. Use DaysInfoToCalendarData.MapToCalendarDataList to map list of the DayInfo's to the list of CalendarData's.
    /// </summary>
    public class MonthCalendarField : IColumnItemsTemplate
    {
        /// <summary>
        /// MonthCalendarField's data
        /// </summary>
        public CalendarAttributes MonthCalendarFieldData { set; get; }

        /// <summary>
        /// Table's Cells Definitions. If you don't set this value, it will be filled by using current template's settings internally.
        /// </summary>
        public CellBasicProperties BasicProperties { set; get; }

        /// <summary>
        /// Defines the current cell's properties, based on the other cells values. 
        /// Here IList contains actual row's cells values.
        /// It can be null.
        /// </summary>
        public Func<IList<CellData>, CellBasicProperties> ConditionalFormatFormula { set; get; }

        /// <summary>
        /// This method is called at the end of the cell's rendering.
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        /// <param name="attributes">Current cell's custom attributes</param>
        public void CellRendered(PdfPCell cell, Rectangle position, PdfContentByte[] canvases, CellAttributes attributes)
        {
        }

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            var data = attributes.RowData.Value as CalendarData;
            if (data == null)
                throw new InvalidOperationException("Calendar's cell data type is not PdfRpt.Calendar.CalendarData. Use DaysInfoToCalendarData.MapToCalendarDataList to map list of the DayInfo's to the list of CalendarData's.");

            if (attributes.BasicProperties.PdfFont == null)
            {
                return new PdfPCell();
            }

            if (MonthCalendarFieldData.RelativeColumnWidths == null)
                MonthCalendarFieldData.RelativeColumnWidths = new float[] { 1, 1, 1, 1, 1, 1, 1 };

            var table = new MonthCalendar
                {
                    CalendarData = data,
                    CalendarAttributes = new CalendarAttributes
                    {
                        BorderColor = attributes.SharedData.Template.CellBorderColor,
                        Font = attributes.BasicProperties.PdfFont,
                        RelativeColumnWidths = MonthCalendarFieldData.RelativeColumnWidths,
                        UseLongDayNamesOfWeek = MonthCalendarFieldData.UseLongDayNamesOfWeek,
                        CalendarType = MonthCalendarFieldData.CalendarType,
                        CellsCustomizer = MonthCalendarFieldData.CellsCustomizer,
                        GradientEndColor = MonthCalendarFieldData.GradientEndColor,
                        GradientStartColor = MonthCalendarFieldData.GradientStartColor,
                        DescriptionHorizontalAlignment = MonthCalendarFieldData.DescriptionHorizontalAlignment,
                        DayNamesRowBackgroundColor = MonthCalendarFieldData.DayNamesRowBackgroundColor
                    }
                }.CreateMonthCalendar();
            var cell = new PdfPCell(table);
            cell.Padding = MonthCalendarFieldData.Padding;
            return cell;
        }
    }
}