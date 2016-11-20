using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Displaying current cell's data as an annotation.
    /// </summary>
    public class AnnotationField : IColumnItemsTemplate
    {
        readonly IDictionary<AnnotationIcon, string> _annotationIcon = new Dictionary<AnnotationIcon, string>
        {
            { AnnotationIcon.Comment, "Comment" },
            { AnnotationIcon.Help, "Help" },
            { AnnotationIcon.Insert, "Insert" },
            { AnnotationIcon.Key, "Key" },
            { AnnotationIcon.NewParagraph, "NewParagraph" },
            { AnnotationIcon.Note, "Note" },
            { AnnotationIcon.Paragraph, "Paragraph" }
        };

        /// <summary>
        /// Return an AnnotationFieldData based on the passed value, 
        /// which is the actual row's fields values from TableDataSource and CalculatedFieldFormula.
        /// If you don't want to add the annotation, return null.
        /// </summary>
        public Func<IList<CellData>, AnnotationFieldData> OnPrintAnnotation { set; get; }

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
            if (OnPrintAnnotation == null)
                throw new InvalidOperationException("Please set the OnPrintAnnotation formula.");

            var data = OnPrintAnnotation.Invoke(attributes.RowData.TableRowData);
            if (data == null) return new PdfPCell();

            var defaultFont = attributes.BasicProperties.PdfFont.Fonts[0];
            var chunk = new Chunk(".", defaultFont);
            chunk.SetAnnotation(
                    PdfAnnotation.CreateText(
                           attributes.SharedData.PdfWriter,
                           new Rectangle(100, 100),
                           data.Title,
                           data.Text,
                           false,
                           _annotationIcon[data.Icon]));

            return new PdfPCell(new Phrase(chunk));
        }
    }
}
