using System;
using System.Globalization;
using OfficeOpenXml;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// ExcelHelper class.
    /// </summary>
    public static class ExcelHelper
    {
        //The correct method to convert width to pixel is:
        //Pixel =Truncate(((256 * {width} + Truncate(128/{Maximum DigitWidth}))/256)*{Maximum Digit Width})

        //The correct method to convert pixel to width is:
        //1. use the formula =Truncate(({pixels}-5)/{Maximum Digit Width} * 100+0.5)/100 
        //    to convert pixel to character number.
        //2. use the formula width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256 
        //    to convert the character number to width.

        /// <summary>
        /// MTU PER PIXEL
        /// </summary>
        public const int MtuPerPixel = 9525;

        /// <summary>
        /// convert width to pixel
        /// </summary>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="excelColumnWidth">ColumnWidth</param>
        /// <returns></returns>
        public static int ColumnWidth2Pixel(this ExcelWorksheet ws, double excelColumnWidth)
        {
            //The correct method to convert width to pixel is:
            //Pixel =Truncate(((256 * {width} + Truncate(128/{Maximum DigitWidth}))/256)*{Maximum Digit Width})

            //get the maximum digit width
            var mdw = ws.Workbook.MaxFontWidth;

            //convert width to pixel
            var pixels = decimal.Truncate(((256 * (decimal)excelColumnWidth + decimal.Truncate(128 / mdw)) / 256) * mdw);
            //double columnWidthInTwips = (double)(pixels * (1440f / 96f));

            return Convert.ToInt32(pixels, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// convert pixel to width
        /// </summary>
        /// <param name="ws">ExcelWorksheet</param>
        /// <param name="pixels">pixels</param>
        /// <returns></returns>
        public static double Pixel2ColumnWidth(this ExcelWorksheet ws, int pixels)
        {
            //The correct method to convert pixel to width is:
            //1. use the formula =Truncate(({pixels}-5)/{Maximum Digit Width} * 100+0.5)/100 
            //    to convert pixel to character number.
            //2. use the formula width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256 
            //    to convert the character number to width.

            //get the maximum digit width
            var mdw = ws.Workbook.MaxFontWidth;

            //convert pixel to character number
            var numChars = decimal.Truncate(decimal.Add((pixels - 5) / mdw * 100, (decimal)0.5)) / 100;
            //convert the character number to width
            var excelColumnWidth = decimal.Truncate((decimal.Add(numChars * mdw, 5)) / mdw * 256) / 256;

            return Convert.ToDouble(excelColumnWidth, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// convert height to pixel
        /// </summary>
        /// <param name="excelRowHeight">RowHeight</param>
        /// <returns></returns>
        public static int RowHeight2Pixel(this double excelRowHeight)
        {
            var pixels = decimal.Truncate((decimal)(excelRowHeight / 0.75));
            return Convert.ToInt32(pixels, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// convert height to pixel
        /// </summary>
        /// <param name="pixels">pixels</param>
        /// <returns></returns>
        public static double Pixel2RowHeight(this int pixels)
        {
            var excelRowHeight = pixels * 0.75;
            return excelRowHeight;
        }

        /// <summary>
        /// convert MTU to pixel
        /// </summary>
        /// <param name="mtus">mtus</param>
        /// <returns></returns>
        public static int Mtu2Pixel(this int mtus)
        {
            var pixels = decimal.Truncate(mtus / MtuPerPixel);
            return Convert.ToInt32(pixels, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// convert pixel to MTU
        /// </summary>
        /// <param name="pixels">pixels</param>
        /// <returns></returns>
        public static int Pixel2Mtu(this int pixels)
        {            
            var mtus = pixels * MtuPerPixel;
            return mtus;
        }
    }
}
