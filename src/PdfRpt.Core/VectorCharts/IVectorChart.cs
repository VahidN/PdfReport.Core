using iTextSharp.text;

namespace PdfRpt.VectorCharts
{
    /// <summary>
    /// VectorChart's template.
    /// </summary>
    public interface IVectorChart
    {
        /// <summary>
        /// This method returns a vector image which can be painted on a table's cell or part of a page.
        /// </summary>
        /// <returns>An instance of iTextSharp.text.pdf.Image</returns>
        Image Draw();
    }
}