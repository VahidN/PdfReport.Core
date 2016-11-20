using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// This item template is useful for displaying Boolean data (true/false) as checkMarks
    /// </summary>
    public class CheckmarkField : IColumnItemsTemplate
    {
        // This way, the image bytes will be added to the PDF only once, not per each new instance.
        // Therefore the result won't be a bloated PDF file.
        Image _cachedCheckmark;
        Image _cachedCross;

        /// <summary>
        /// CheckMark's fill color.
        /// </summary>
        public System.Drawing.Color? CheckmarkFillColor { set; get; }

        /// <summary>
        /// Fill color of the cross sign.
        /// </summary>
        public System.Drawing.Color? CrossSignFillColor { set; get; }

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
            checkNulls();
            var cb = canvases[PdfPTable.BACKGROUNDCANVAS];
            cb.SaveState();
            var data = (bool)attributes.RowData.Value;

            if (_cachedCheckmark == null)
                _cachedCheckmark = VectorImages.CheckmarkImage(cb, CheckmarkFillColor.Value);

            if (_cachedCross == null)
                _cachedCross = VectorImages.CrossImage(cb, CrossSignFillColor.Value);

            var image = data ? _cachedCheckmark.DrawCheckmarkImageAtPosition(position) :
                               _cachedCross.DrawCrossImageAtPosition(position);
            cb.AddImage(image);
            cb.RestoreState();
        }

        private void checkNulls()
        {
            if (CheckmarkFillColor == null) CheckmarkFillColor = System.Drawing.Color.Green;
            if (CrossSignFillColor == null) CrossSignFillColor = System.Drawing.Color.DarkRed;
        }

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            return new PdfPCell();
        }
    }
}
