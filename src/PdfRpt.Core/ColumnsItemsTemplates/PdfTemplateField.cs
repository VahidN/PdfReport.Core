using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Using an AcroForm or a simple pdf template file as the Cells template.
    /// </summary>
    public class PdfTemplateField : IColumnItemsTemplate
    {
        /// <summary>
        /// An AcroForm or a simple pdf template file's path.
        /// Only first page of the passed AcroForm makes sense here as a cell's template!
        /// </summary>
        public string PdfTemplatePath { set; get; }

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
        /// Gives you the Row's data and AcroForm to set form.SetField method.
        /// It can be null.
        /// </summary>
        public Action<IList<CellData>, AcroFields, PdfStamper> OnFillAcroForm { set; get; }

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

        Image _image;

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        /// <returns>Content as a PdfPCell</returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            if (shouldUseCachedImage)
            {
                return new PdfPCell(_image, true);
            }

            createImageFromImportedPage(attributes);
            return new PdfPCell(_image, true);
        }

        private void createImageFromImportedPage(CellAttributes attributes)
        {
            if (OnFillAcroForm != null)
            {
                _image = attributes.SharedData.PdfWriter.GetITextSharpImageFromAcroForm(
                    PdfTemplatePath, attributes.RowData.TableRowData, 
                    OnFillAcroForm, attributes.BasicProperties.PdfFont.Fonts);
            }
            else
            {
                _image = attributes.SharedData.PdfWriter.GetITextSharpImageFromPdfTemplate(PdfTemplatePath);
            }
        }

        private bool shouldUseCachedImage
        {
            get { return OnFillAcroForm == null && _image != null; }
        }
    }
}
