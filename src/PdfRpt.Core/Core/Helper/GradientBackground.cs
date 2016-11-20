using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Draws a rectangular gradient background color.
    /// </summary>
    public static class GradientBackground
    {
        /// <summary>
        /// Draws a rectangular gradient background color.
        /// </summary>
        /// <param name="pdfRowType">Main table's row types</param>
        /// <param name="sharedData">PdfCells Shared Data</param>        
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        public static void ApplyGradientBackground(RowType pdfRowType, CellSharedData sharedData, Rectangle position, PdfContentByte[] canvases)
        {
            BaseColor startColor = null;
            BaseColor endColor = null;

            switch (pdfRowType)
            {
                case RowType.HeaderRow:
                    if (sharedData.Template.HeaderBackgroundColor.Count < 2) return;
                    startColor = sharedData.Template.HeaderBackgroundColor[0];
                    endColor = sharedData.Template.HeaderBackgroundColor[1];
                    break;
                case RowType.PreviousPageSummaryRow:
                    if (sharedData.Template.PreviousPageSummaryRowBackgroundColor.Count < 2) return;
                    startColor = sharedData.Template.PreviousPageSummaryRowBackgroundColor[0];
                    endColor = sharedData.Template.PreviousPageSummaryRowBackgroundColor[1];
                    break;
                case RowType.SummaryRow:
                case RowType.AllGroupsSummaryRow:
                    if (sharedData.Template.SummaryRowBackgroundColor.Count < 2) return;
                    startColor = sharedData.Template.SummaryRowBackgroundColor[0];
                    endColor = sharedData.Template.SummaryRowBackgroundColor[1];
                    break;
                case RowType.PageSummaryRow:
                    if (sharedData.Template.PageSummaryRowBackgroundColor.Count < 2) return;
                    startColor = sharedData.Template.PageSummaryRowBackgroundColor[0];
                    endColor = sharedData.Template.PageSummaryRowBackgroundColor[1];
                    break;
            }

            DrawGradientBackground(position, canvases, startColor, endColor);
        }

        /// <summary>
        /// Draws a rectangular gradient background color.
        /// </summary>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        /// <param name="startColor">Gradient's Start Color</param>
        /// <param name="endColor">Gradient's End Color</param>
        public static void DrawGradientBackground(this Rectangle position, PdfContentByte[] canvases, BaseColor startColor, BaseColor endColor)
        {
            if (startColor == null || endColor == null) return;

            var cb = canvases[PdfPTable.BACKGROUNDCANVAS];
            cb.SaveState();

            var shading = PdfShading.SimpleAxial(
                                    cb.PdfWriter,
                                    position.Left, position.Top, position.Left, position.Bottom,
                                    startColor, endColor);
            var shadingPattern = new PdfShadingPattern(shading);

            cb.SetShadingFill(shadingPattern);
            cb.Rectangle(position.Left, position.Bottom, position.Width, position.Height);
            cb.Fill();

            cb.RestoreState();
        }
    }
}