using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Displaying current cell's data as a hyperlink
    /// </summary>
    public class HyperlinkField : IColumnItemsTemplate
    {
        readonly BaseColor _foreColor;
        readonly bool _fontUnderline;

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
        /// ctor.
        /// </summary>
        /// <param name="foreColor"></param>
        /// <param name="fontUnderline"></param>
        public HyperlinkField(BaseColor foreColor, bool fontUnderline)
        {
            _foreColor = foreColor;
            _fontUnderline = fontUnderline;
        }

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
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            var font = setFontStyles(attributes);
            var anchor = getAnchor(font, attributes);
            return new PdfPCell(anchor);
        }


        /// <summary>
        /// If you don't set TextPropertyName, current cell's text will be used as hyperlink's text.
        /// </summary>
        public string TextPropertyName { set; get; }


        /// <summary>
        /// If you don't set NavigationUrlPropertyName, current cell's text will be used as hyperlink's target url.
        /// </summary>
        public string NavigationUrlPropertyName { set; get; }

        private Anchor getAnchor(FontSelector fontSelector, CellAttributes attributes)
        {
            var text = getText(attributes);
            var url = getUrl(attributes);
            var anchor = new Anchor(fontSelector.Process(text.ToSafeString())) { Reference = url };
            return anchor;
        }

        private string getUrl(CellAttributes attributes)
        {
            var url = attributes.RowData.Value.ToSafeString();
            if (!string.IsNullOrEmpty(NavigationUrlPropertyName))
            {
                url = attributes.RowData.TableRowData.GetSafeStringValueOf(NavigationUrlPropertyName);
            }
            return url;
        }

        private string getText(CellAttributes attributes)
        {
            var text = FuncHelper.ApplyFormula(attributes.BasicProperties.DisplayFormatFormula, attributes.RowData.Value);
            if (!string.IsNullOrEmpty(TextPropertyName))
            {
                text = attributes.RowData.TableRowData.GetSafeStringValueOf(TextPropertyName);
            }
            attributes.RowData.FormattedValue = text;
            return text;
        }

        private FontSelector setFontStyles(CellAttributes attributes)
        {
            foreach (var font in attributes.BasicProperties.PdfFont.Fonts)
            {
                font.Color = _foreColor;
                if (_fontUnderline) font.SetStyle(Font.UNDERLINE);
            }
            return attributes.BasicProperties.PdfFont.FontSelector;
        }
    }
}
