using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.ColumnsItemsTemplates
{
    /// <summary>
    /// Displaying current cell's data as an Image. It's assumed that this data point to the physical image's path on the disk.
    /// </summary>
    public class ImageFilePathField : IColumnItemsTemplate
    {
        readonly bool _fitImages;
        readonly string _defaultImageFilePath;
        readonly bool _cacheImages;

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
        /// Displaying current cell's data as an Image. It's assumed that this data point to the physical image's path on the disk.
        /// </summary>        
        /// <param name="defaultImageFilePath">Default image will be used in case of missing images</param>
        /// <param name="fitImages">Set fitImages to true to fit the image to the cell</param>
        /// <param name="cacheImages">If true, the image bytes will be added to the PDF only once, not per each new instance. Therefore the result won't be a bloated PDF file. Choose this option if there are many similar images in your data source.</param>
        public ImageFilePathField(string defaultImageFilePath, bool fitImages, bool cacheImages = true)
        {
            _fitImages = fitImages;
            _defaultImageFilePath = defaultImageFilePath;
            _cacheImages = cacheImages;
        }

        /// <summary>
        /// Displaying current cell's data as an Image. It's assumed that this data point to the physical image's path on the disk.
        /// </summary>
        public ImageFilePathField()
        {
            _fitImages = false;
            _defaultImageFilePath = string.Empty;
            _cacheImages = true;
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
            if (attributes.RowData.Value == null) return new PdfPCell();

            var iTextImg = getImage(attributes);
            var pdfCell = new PdfPCell(iTextImg, fit: _fitImages)
            {
                PaddingTop = 2,
                PaddingBottom = 2,
            };

            if (!_fitImages)
            {
                pdfCell.MinimumHeight = iTextImg.Height + 5;
                pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            }

            return pdfCell;
        }

        private Image getImage(CellAttributes attributes)
        {
            var path = FuncHelper.ApplyFormula(attributes.BasicProperties.DisplayFormatFormula, attributes.RowData.Value);
            attributes.RowData.FormattedValue = path;
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                return path.GetITextSharpImageFromImageFile(_cacheImages);
            }

            if (!string.IsNullOrEmpty(_defaultImageFilePath) && File.Exists(_defaultImageFilePath))
            {
                return _defaultImageFilePath.GetITextSharpImageFromImageFile(this._cacheImages);
            }
            throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "{0} does not exists & defaultImageFilePath was not found.", path));
        }
    }
}
