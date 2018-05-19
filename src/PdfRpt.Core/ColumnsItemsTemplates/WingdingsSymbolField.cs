using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Core.Helper;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Displaying the current cell's data as a Wingdings symbol.
    /// </summary>
    public class WingdingsSymbolField : IColumnItemsTemplate
    {
        /// <summary>
        /// Wingdings FontPath
        /// </summary>
        public string WingdingsFontPath
        {
            get { return System.IO.Path.Combine(FontsDirPath.SystemFontsFolder, "WINGDING.TTF"); }
        }

        /// <summary>
        /// Displaying the current cell's data as a Wingdings symbol.
        /// </summary>
        public WingdingsSymbolField()
        {
            if (!FontFactory.IsRegistered(WingdingsFontPath))
            {
                FontFactory.Register(WingdingsFontPath);
            }
        }

        /// <summary>
        /// Choose a  Wingdings symbol based on the passed value.
        /// </summary>
        public Func<IList<CellData>, Wingdings> OnSelectSymbol { set; get; }

        /// <summary>
        /// Defines the current cell's properties, based on the other cells values.
        /// Here IList contains actual row's cells values.
        /// It can be null.
        /// </summary>
        public Func<IList<CellData>, CellBasicProperties> ConditionalFormatFormula { set; get; }

        /// <summary>
        /// Table's Cells Definitions. If you don't set this value, it will be filled by using current template's settings internally.
        /// </summary>
        public CellBasicProperties BasicProperties { set; get; }

        /// <summary>
        /// This method is called at the end of the cell's rendering.
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        /// <param name="attributes">Current cell's custom attributes</param>
        public void CellRendered(PdfPCell cell, Rectangle position, PdfContentByte[] canvases, CellAttributes attributes)
        {
        }

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            if (OnSelectSymbol == null)
                throw new InvalidOperationException("Please set the OnSelectSymbol formula.");

            var font = FontFactory.GetFont(WingdingsFontPath, BaseFont.IDENTITY_H, true, attributes.BasicProperties.PdfFont.Size, Font.NORMAL, attributes.BasicProperties.FontColor);
            var symbol = (int)OnSelectSymbol.Invoke(attributes.RowData.TableRowData);
            return new PdfPCell(new Phrase(new Chunk((char)symbol, font)));
        }
    }
}