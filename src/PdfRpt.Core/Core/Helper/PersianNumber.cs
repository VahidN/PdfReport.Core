using System.Globalization;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// English numbers to Persian numbers converter and vice versa.
    /// </summary>
    public static class PersianNumber
    {
        /// <summary>
        /// Converts English digits of a given string to their equivalent Persian digits.
        /// </summary>
        /// <param name="number">English number</param>
        /// <returns></returns>
        public static string ToPersianNumbers(this int number)
        {
            return ToPersianNumbers(number.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts English digits of a given string to their equivalent Persian digits.
        /// </summary>
        /// <param name="number">English number</param>
        /// <returns></returns>
        public static string ToPersianNumbers(this long number)
        {
            return ToPersianNumbers(number.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts English digits of a given string to their equivalent Persian digits.
        /// </summary>
        /// <param name="number">English number</param>
        /// <returns></returns>
        public static string ToPersianNumbers(this int? number)
        {
            if (!number.HasValue) number = 0;
            return ToPersianNumbers(number.Value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts English digits of a given string to their equivalent Persian digits.
        /// </summary>
        /// <param name="number">English number</param>
        /// <returns></returns>
        public static string ToPersianNumbers(this long? number)
        {
            if (!number.HasValue) number = 0;
            return ToPersianNumbers(number.Value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts English digits of a given string to their equivalent Persian digits.
        /// </summary>
        /// <param name="data">English number</param>
        /// <returns></returns>
        public static string ToPersianNumbers(this string data)
        {
            if (string.IsNullOrEmpty(data)) return string.Empty;
            return
               data.Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹")
                .Replace(".", ",");
        }

        /// <summary>
        /// Converts Persian digits of a given string to their equivalent English digits.
        /// </summary>
        /// <param name="data">Persian number</param>
        /// <returns></returns>
        public static string ToEnglishNumbers(this string data)
        {
            if (string.IsNullOrEmpty(data)) return string.Empty;
            return
               data.Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9");
        }
    }
}
