using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// PdfGrid represents the iTextSharp.text.pdf.PdfPTable class.
    /// </summary>
    public class PdfGrid : PdfPTable
    {
        /// <summary>
        /// Constructs a PdfPTable with the relative column widths.
        /// </summary>
        /// <param name="relativeWidths">the relative column widths</param>
        public PdfGrid(float[] relativeWidths) : base(relativeWidths) { }

        /// <summary>
        /// Constructs a PdfPTable with numColumns columns.
        /// </summary>
        /// <param name="numColumns">the number of columns</param>
        public PdfGrid(int numColumns) : base(numColumns) { }

        /// <summary>
        /// Constructs a copy of a PdfPTable.
        /// </summary>
        /// <param name="table">the PdfPTableto be copied</param>
        public PdfGrid(PdfPTable table) : base(table) { }
    }
}
