using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// A class to encapsulate the diagonal watermark related methods.
    /// </summary>
    public class DiagonalWatermarkManager
    {
        PdfTemplate _watermarkTemplate;

        /// <summary>
        /// Document settings
        /// </summary>
        public DocumentPreferences PageSetup { set; get; }

        /// <summary>
        /// Apply Watermark to the added templates
        /// </summary>
        /// <param name="document">PDF Document</param>
        public void ApplyWatermark(Document document)
        {
            var diagonalWatermark = PageSetup.DiagonalWatermark;
            if (diagonalWatermark == null) return;

            var textAngle = (float)getHypotenuseAngleInDegreesFrom(document.PageSize.Height, document.PageSize.Width);
            var text = diagonalWatermark.Text;

            if (diagonalWatermark.RunDirection == null)
                diagonalWatermark.RunDirection = PdfRunDirection.LeftToRight;

            if (diagonalWatermark.RunDirection == PdfRunDirection.RightToLeft)
                text = text.FixWeakCharacters();

            var phrase = diagonalWatermark.Font.FontSelector.Process(text.ToSafeString());

            ColumnText.ShowTextAligned(
                _watermarkTemplate,
                Element.ALIGN_CENTER,
                phrase,
                document.PageSize.Width / 2,
                document.PageSize.Height / 2,
                textAngle,
                (int)diagonalWatermark.RunDirection,
                0);
        }

        /// <summary>
        /// Create an empty template
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public void InitWatermarkTemplate(PdfWriter writer, Document document)
        {
            if (PageSetup.DiagonalWatermark == null) return;
            _watermarkTemplate = writer.DirectContent.CreateTemplate(document.PageSize.Width, document.PageSize.Height);
        }

        /// <summary>
        /// Add an empty template to each page
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        public void ReserveWatermarkSpace(PdfWriter writer)
        {
            if (_watermarkTemplate == null) return;

            var canvas = writer.DirectContent;
            canvas.SaveState();
            canvas.SetGState(new PdfGState
            {
                FillOpacity = PageSetup.DiagonalWatermark.FillOpacity,
                StrokeOpacity = PageSetup.DiagonalWatermark.StrokeOpacity
            });
            canvas.AddTemplate(_watermarkTemplate, 0, 0);
            canvas.RestoreState();
        }

        private static double getHypotenuseAngleInDegreesFrom(double opposite, double adjacent)
        {
            double radians = Math.Atan2(opposite, adjacent); // Get Radians for Atan2
            double angle = radians * (180 / Math.PI); // Change back to degrees
            return angle;
        }
    }
}