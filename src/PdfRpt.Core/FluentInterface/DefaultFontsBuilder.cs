using System.Drawing;
using iTextSharp.text;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Default Fonts Settings Class.
    /// </summary>
    public class DefaultFontsBuilder
    {
        readonly PdfReport _pdfReport;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public DefaultFontsBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
        }

        /// <summary>
        /// Sets the default fonts size.
        /// </summary>
        /// <param name="value">font size</param>
        public void Size(int value)
        {
            _pdfReport.DataBuilder.DefaultFontsSize(value);
        }

        /// <summary>
        /// Sets the default fonts color.
        /// </summary>
        /// <param name="colorValue"></param>
        public void Color(Color colorValue)
        {
            _pdfReport.DataBuilder.DefaultFontsColor(new BaseColor(colorValue.ToArgb()));
        }

        /// <summary>
        /// Sets the optional fonts path.
        /// </summary>
        /// <param name="defaultFont1">font1's path</param>
        /// <param name="defaultFont2">font2's path</param>
        public void Path(string defaultFont1, string defaultFont2)
        {
            _pdfReport.DataBuilder.DefaultFontsPath(defaultFont1, defaultFont2);
        }
    }
}
