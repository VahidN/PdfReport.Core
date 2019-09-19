using System;
using System.Collections;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper.HtmlToPdf
{
    /// <summary>
    /// Using iTextSharp's limited HTML to PDF capabilities.
    /// </summary>
    public class HtmlWorkerHelper
    {
        /// <summary>
        /// Custom HTML Element.
        /// </summary>
        public Image PdfElement;

        /// <summary>
        /// The HTML to show.
        /// </summary>
        public string Html { set; get; }

        /// <summary>
        /// Defines styles for HTMLWorker.
        /// </summary>
        public StyleSheet StyleSheet { set; get; }

        /// <summary>
        /// Run direction, left-to-right or right-to-left.
        /// </summary>
        public PdfRunDirection RunDirection { set; get; }

        /// <summary>
        /// Cells horizontal alignment value
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { set; get; }

        /// <summary>
        /// Custom font's definitions
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// Using iTextSharp's limited HTML to PDF capabilities.
        /// </summary>
        public PdfPCell RenderHtml()
        {
            if (PdfFont == null)
            {
                throw new NullReferenceException($"{nameof(PdfFont)} of the HTML section is null.");
            }

            var pdfCell = new PdfPCell
            {
                UseAscender = true,
                UseDescender = true,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            applyStyleSheet();

            var unicodeFontProvider = FontFactoryImp.Instance;
            unicodeFontProvider.DefaultEmbedding = BaseFont.EMBEDDED;
            unicodeFontProvider.DefaultEncoding = BaseFont.IDENTITY_H;

            var props = new Hashtable
            {
                { "img_provider", new HtmlImageFactory(this.PdfElement) },
                { "font_factory", unicodeFontProvider } // Always use Unicode fonts
            };

            using (var reader = new StringReader(Html))
            {
                var parsedHtmlElements = HtmlWorker.ParseToList(reader, StyleSheet, props);

                foreach (IElement htmlElement in parsedHtmlElements)
                {
                    applyRtlRunDirection(htmlElement);
                    pdfCell.AddElement(htmlElement);
                }

                return pdfCell;
            }
        }

        private void applyRtlRunDirection(IElement htmlElement)
        {
            if (RunDirection != PdfRunDirection.RightToLeft) return;

            var table = htmlElement as PdfPTable;
            if (table == null) return;

            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            foreach (PdfPRow row in table.Rows)
            {
                foreach (var cell in row.GetCells())
                {
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                }
            }
        }

        private void applyStyleSheet()
        {
            if (StyleSheet == null) StyleSheet = new StyleSheet();
            // set the default font's properties
            StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.FONT, PdfFont.Fonts[0].Familyname);
            StyleSheet.LoadTagStyle(HtmlTags.BODY, "size", string.Format("{0}pt", PdfFont.Fonts[0].Size));
            StyleSheet.LoadTagStyle(HtmlTags.BODY, "encoding", "Identity-H");

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    StyleSheet.LoadTagStyle(HtmlTags.IMAGE, HtmlTags.ALIGN, HtmlTags.ALIGN_CENTER);
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_CENTER);
                    break;
                case HorizontalAlignment.Justified:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_JUSTIFIED);
                    break;
                case HorizontalAlignment.JustifiedAll:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, "JustifyAll");
                    break;
                case HorizontalAlignment.Left:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_LEFT);
                    break;
                case HorizontalAlignment.Right:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_RIGHT);
                    break;
                default:
                    StyleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ALIGN, HtmlTags.ALIGN_LEFT);
                    break;
            }
        }
    }
}