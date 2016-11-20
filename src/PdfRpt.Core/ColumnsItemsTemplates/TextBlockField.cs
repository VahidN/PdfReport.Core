using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Displaying current cell's data as text
    /// </summary>
    public class TextBlockField : IColumnItemsTemplate
    {
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
        }

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            var data = FuncHelper.ApplyFormula(attributes.BasicProperties.DisplayFormatFormula, attributes.RowData.Value);
            attributes.RowData.FormattedValue = data;

            if (attributes.BasicProperties != null && attributes.BasicProperties.RunDirection == PdfRunDirection.RightToLeft)
                data = data.FixWeakCharacters();

            if (attributes.BasicProperties.PdfFont == null)
            {
                return new PdfPCell();
            }

            data = showEmptyCell(data);

            var phrase = attributes.BasicProperties.PdfFont.FontSelector.Process(data);
            return new PdfPCell(phrase);
        }

        private static string showEmptyCell(string data)
        {
            data = data.ToSafeString();
            if (string.IsNullOrEmpty(data))
            {
                data = " ";
            }
            return data;
        }
    }
}