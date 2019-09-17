using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// iTextSharp.text.Image class utilities
    /// </summary>
    public static class PdfImageHelper
    {
        // This way, the image bytes will be added to the PDF only once, not per each new instance.
        // Therefore the result won't be a bloated PDF file.
        private static readonly ConcurrentDictionary<string, iTextSharp.text.Image> _imageFilesCache =
              new ConcurrentDictionary<string, iTextSharp.text.Image>();
        private static readonly ConcurrentDictionary<string, iTextSharp.text.Image> _imageBytesCache =
              new ConcurrentDictionary<string, iTextSharp.text.Image>();

        private static iTextSharp.text.Image checkImage(this iTextSharp.text.Image img)
        {
            if (img == null)
                throw new InvalidOperationException("iTextSharp.text.Image is null.");

            return img;
        }

        /// <summary>
        /// Converts the barcodeText to a barcode image and then returns an instance of iTextSharp.text.Image
        /// </summary>
        /// <param name="barcode">Barcode type defined in the iTextSharp.text.pdf namespace</param>
        /// <param name="barcodeText">Text to convert</param>
        /// <param name="cb">Canvas</param>
        /// <returns>A barcode image</returns>
        public static iTextSharp.text.Image GetBarcodeImage(this Barcode barcode, string barcodeText, PdfContentByte cb)
        {
            if (barcode.BarHeight.ApproxEquals(0))
                barcode.BarHeight = 7;

            barcode.Code = barcodeText;
            barcode.ChecksumText = true;
            barcode.GenerateChecksum = true;
            barcode.StartStopText = true;

            var imageIn = barcode.CreateImageWithBarcode(cb, BaseColor.Black, BaseColor.White);
            //PdfStream.BEST_COMPRESSION is optimized for ImageFormat.Bmp
            return imageIn.checkImage();
        }

        /// <summary>
        /// Gets an image's file path and returns an instance of iTextSharp.text.Image
        /// </summary>
        /// <param name="imageFilePath">Image's file path</param>
        /// <param name="cacheImages">If true, the image bytes will be added to the PDF only once, not per each new instance. Therefore the result won't be a bloated PDF file. Choose this option if there are many similar images in your data source.</param>
        /// <returns>An instance of iTextSharp.text.Image</returns>
        public static iTextSharp.text.Image GetITextSharpImageFromImageFile(this string imageFilePath, bool cacheImages = true)
        {
            if (!cacheImages)
                return iTextSharp.text.Image.GetInstance(imageFilePath).checkImage();

            iTextSharp.text.Image image;
            var pathHash = imageFilePath.ToLowerInvariant().MD5Hash();
            if (_imageFilesCache.TryGetValue(pathHash, out image))
                return image.checkImage();

            image = iTextSharp.text.Image.GetInstance(imageFilePath);
            _imageFilesCache.TryAdd(pathHash, image);
            return image.checkImage();
        }

        /// <summary>
        /// Converts an array of bytes/blobs of an image file to an instance of iTextSharp.text.Image
        /// </summary>
        /// <param name="data">blob data</param>
        /// <param name="cacheImages">If true, the image bytes will be added to the PDF only once, not per each new instance. Therefore the result won't be a bloated PDF file. Choose this option if there are many similar images in your data source.</param>
        /// <returns>An instance of iTextSharp.text.Image</returns>
        public static iTextSharp.text.Image GetITextSharpImageFromByteArray(this byte[] data, bool cacheImages = true)
        {
            if (!cacheImages)
                return iTextSharp.text.Image.GetInstance(data).checkImage();

            iTextSharp.text.Image image;
            var pathHash = data.MD5Hash();
            if (_imageBytesCache.TryGetValue(pathHash, out image))
                return image.checkImage();

            image = iTextSharp.text.Image.GetInstance(data);
            _imageBytesCache.TryAdd(pathHash, image);
            return image.checkImage();
        }

        /// <summary>
        /// Converts the selected page number of an existing pdf template file to an instance of iTextSharp.text.Image
        /// </summary>
        /// <param name="pdfWriter">PdfWriter object</param>
        /// <param name="pdfTemplateFilePath">pdf file path</param>
        /// <param name="pageNumber">selected page number of an existing pdf template file</param>
        /// <returns>An instance of iTextSharp.text.Image</returns>
        public static iTextSharp.text.Image GetITextSharpImageFromPdfTemplate(
                            this PdfWriter pdfWriter,
                            string pdfTemplateFilePath,
                            int pageNumber = 1)
        {
            var reader = new PdfReader(pdfTemplateFilePath);
            var importedPage = pdfWriter.GetImportedPage(reader, pageNumber);
            //reader.Close();  // iTextSharp 5.4.1.0 needs this to be open.
            return iTextSharp.text.Image.GetInstance(importedPage).checkImage();
        }

        /// <summary>
        /// Fills an AcroForm automatically and then Converts the selected page number of an existing pdf template file to an instance of iTextSharp.text.Image
        /// </summary>
        /// <param name="pdfWriter">PdfWriter object</param>
        /// <param name="pdfTemplateFilePath">pdf file path</param>
        /// <param name="data">Row's data</param>
        /// <param name="onFillAcroForm">FillAcroForm Formula</param>
        /// <param name="fonts">Controls fonts</param>
        /// <param name="pageNumber">selected page number of an existing pdf template file</param>
        /// <returns>An instance of iTextSharp.text.Image</returns>
        public static iTextSharp.text.Image GetITextSharpImageFromAcroForm(
                            this PdfWriter pdfWriter,
                            string pdfTemplateFilePath,
                            IList<CellData> data,
                            Action<IList<CellData>, AcroFields, PdfStamper> onFillAcroForm,
                            IList<iTextSharp.text.Font> fonts,
                            int pageNumber = 1)
        {
            using (var writerStream = new MemoryStream())
            {
                var pdfReader = new PdfReader(pdfTemplateFilePath);
                var stamper = new PdfStamper(pdfReader, writerStream);

                foreach (var font in fonts)
                {
                    stamper.AcroFields.AddSubstitutionFont(font.BaseFont);
                }

                var form = stamper.AcroFields;
                form.GenerateAppearances = true;
                onFillAcroForm.Invoke(data, form, stamper);

                stamper.FormFlattening = true;

                stamper.Writer.CloseStream = false;
                stamper.Close();
                pdfReader.Close();

                writerStream.Position = 0;
                var reader = new PdfReader(writerStream);
                var importedPage = pdfWriter.GetImportedPage(reader, pageNumber);
                //reader.Close(); // iTextSharp 5.4.1.0 needs this to be open.
                return iTextSharp.text.Image.GetInstance(importedPage).checkImage();
            }
        }
    }
}