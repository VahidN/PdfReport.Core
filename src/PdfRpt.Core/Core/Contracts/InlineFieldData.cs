using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Custom IColumnItemsTemplate's data.
    /// </summary>
    public class InlineFieldData
    {
        /// <summary>
        /// Table's Cells Definitions.
        /// </summary>
        public CellBasicProperties BasicProperties { set; get; }

        /// <summary>
        /// Defines the current cell's properties, based on the other cells values. 
        /// Here IList contains actual row's cells values. It can be null.
        /// </summary>
        public Func<IList<CellData>, CellBasicProperties> ConditionalFormatFormula { set; get; }

        /// <summary>
        /// Important main table's cells attributes.
        /// </summary>
        public CellAttributes Attributes { set; get; }

        /// <summary>
        /// The current cell at the end of the cell's rendering.
        /// It can be null.
        /// </summary>
        public PdfPCell Cell { set; get; }

        /// <summary>
        /// The coordinates of the cell.
        /// It can be null.
        /// </summary>
        public Rectangle Position { set; get; }

        /// <summary>
        /// An array of PdfContentByte to add text or graphics.
        /// It can be null.
        /// </summary>
        public PdfContentByte[] Canvases { set; get; }
    }
}
