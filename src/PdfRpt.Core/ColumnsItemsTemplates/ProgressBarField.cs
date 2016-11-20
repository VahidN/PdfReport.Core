using System;
using System.Collections.Generic;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Displaying current cell's data as text plus a ProgressBar
    /// </summary>
    public class ProgressBarField : IColumnItemsTemplate
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
        /// Progress bar's background color, if ProgressBarColorFormula is not specified.
        /// </summary>
        public System.Drawing.Color ProgressBarColor { set; get; }

        /// <summary>
        /// Progress bar's background color based on the current row's values.
        /// </summary>
        public Func<IList<CellData>, System.Drawing.Color> ProgressBarColorFormula { set; get; }

        /// <summary>
        /// Indicates whether the percentage text should be displayed or not
        /// </summary>
        public bool ShowPercentText { set; get; }

        /// <summary>
        /// This method is called at the end of the cell's rendering.
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        /// <param name="attributes">Current cell's custom attributes</param>
        public void CellRendered(PdfPCell cell, Rectangle position, PdfContentByte[] canvases, CellAttributes attributes)
        {
            var contentByte = canvases[PdfPTable.BACKGROUNDCANVAS];
            contentByte.SaveState();
            if (ProgressBarColorFormula != null)
            {
                ProgressBarColor = ProgressBarColorFormula(attributes.RowData.TableRowData);
            }
            contentByte.SetColorFill(new BaseColor(ProgressBarColor.ToArgb()));
            contentByte.Rectangle(
                          position.Left,
                          position.Bottom,
                          position.Width * getPercent(attributes) / 100,
                          position.Height);
            contentByte.Fill();
            contentByte.RestoreState();
        }

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            if (ShowPercentText)
            {
                var data = FuncHelper.ApplyFormula(attributes.BasicProperties.DisplayFormatFormula, attributes.RowData.Value);
                attributes.RowData.FormattedValue = data;

                if (attributes.BasicProperties != null && attributes.BasicProperties.RunDirection == PdfRunDirection.RightToLeft)
                    data = data.FixWeakCharacters();

                var phrase = attributes.BasicProperties.PdfFont.FontSelector.Process(data.ToSafeString());
                return new PdfPCell(phrase);
            }
            var emptyPhrase = attributes.BasicProperties.PdfFont.FontSelector.Process(string.Empty);
            return new PdfPCell(emptyPhrase);
        }

        private static float getPercent(CellAttributes attributes)
        {
            return attributes.RowData.Value == null ? 0 : float.Parse(attributes.RowData.Value.ToSafeString(), CultureInfo.InvariantCulture);
        }
    }
}
