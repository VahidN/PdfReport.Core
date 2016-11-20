using System.Text.RegularExpressions;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Indicates whether the input text contains rtl data or not
    /// </summary>
    public static class RunDirectionHelper
    {
        static readonly Regex MatchArabicHebrew = new Regex(@"[\u0600-\u06FF,\u0590-\u05FF]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Indicates whether the input text contains rtl data or not
        /// </summary>
        /// <param name="data">input text</param>
        /// <returns></returns>
        public static bool ContainsRtlText(this string data)
        {
            if (string.IsNullOrEmpty(data)) return false;
            return MatchArabicHebrew.IsMatch(data);
        }

        /// <summary>
        /// Determines the text direction based on the ContainsRtlText method's result.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static PdfRunDirection GetRunDirection(this string data)
        {
            return data.ContainsRtlText() ? PdfRunDirection.RightToLeft : PdfRunDirection.LeftToRight;
        }
    }
}
