using iTextSharp.text;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Some helper methods for working with colors
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts System.Drawing.Color to BaseColor
        /// </summary>
        public static BaseColor ToBaseColor(System.Drawing.Color color)
        {
            return new BaseColor(color.R, color.G, color.B, color.A);
        }
    }
}