using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Defines how to display the current cell's data
    /// </summary>
    public class InlineField : IColumnItemsTemplate
    {
        /// <summary>
        /// Table's Cells Definitions. If you don't set this value, 
        /// it will be filled by using current template's settings internally.
        /// </summary>
        public CellBasicProperties BasicProperties { set; get; }

        /// <summary>
        /// Defines the current cell's properties, based on the other cells values. 
        /// Here IList contains actual row's cells values.
        /// It can be null.
        /// </summary>
        public Func<IList<CellData>, CellBasicProperties> ConditionalFormatFormula { set; get; }

        /// <summary>
        /// It will be called at the end of the cell's rendering.
        /// </summary>
        public Action<InlineFieldData> DrawOnCell { set; get; }

        /// <summary>
        /// This method is called at the end of the cell's rendering.
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        /// <param name="attributes">Current cell's custom attributes</param>
        public void CellRendered(PdfPCell cell, Rectangle position, PdfContentByte[] canvases, CellAttributes attributes)
        {
            if (DrawOnCell == null) 
                return;

            DrawOnCell(new InlineFieldData
            {
                Attributes = attributes,
                BasicProperties = BasicProperties,
                Canvases = canvases,
                Cell = cell,
                ConditionalFormatFormula = ConditionalFormatFormula,
                Position = position
            });
        }

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        public Func<InlineFieldData, PdfPCell> RenderCell { set; get; }

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            if (RenderCell == null)
                return new PdfPCell();

            return RenderCell(new InlineFieldData
            {
                Attributes = attributes,
                BasicProperties = BasicProperties,
                Canvases = null,
                Cell = null,
                ConditionalFormatFormula = ConditionalFormatFormula,
                Position = null
            });
        }
    }
}
