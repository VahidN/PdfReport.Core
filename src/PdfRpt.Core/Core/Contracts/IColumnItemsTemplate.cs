using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Custom template of the in use property, controls how and what should be rendered in each cell of a column.
    /// </summary>
    public interface IColumnItemsTemplate
    {
        /// <summary>
        /// Table's Cells Definitions. If you don't set this value, it will be filled by using current template's settings internally.
        /// </summary>
        CellBasicProperties BasicProperties { set; get; }

        /// <summary>
        /// Defines the current cell's properties, based on the other cells values. 
        /// Here IList contains actual row's cells values.
        /// It can be null.
        /// </summary>
        Func<IList<CellData>, CellBasicProperties> ConditionalFormatFormula { set; get; }


        /// <summary>
        /// Custom cell's content template as a PdfPCell.
        /// This method is called at the beginning of the cell's rendering.
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        PdfPCell RenderingCell(CellAttributes attributes);

        /// <summary>
        /// This method is called at the end of the cell's rendering and gives you exact size
        /// of the cell and its raw canvases to draw texts and shapes on it.
        /// </summary>
        /// <param name="cell">The current cell. This is just for readonly purposes! Do not try to change the content of this cell, it won’t have any effect. Once the method of the cell event is triggered, the cell has already been rendered.</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        /// <param name="attributes">Current cell's custom attributes</param>
        void CellRendered(PdfPCell cell, Rectangle position, PdfContentByte[] canvases, CellAttributes attributes);
    }
}
