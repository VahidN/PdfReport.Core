using System.Collections.Generic;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// PagePreferences Builder Class.
    /// </summary>
    public class DocumentPreferencesBuilder
    {
        readonly PdfReport _pdfReport;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public DocumentPreferencesBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
        }

        /// <summary>
        /// Sets the new document's margins. Its predefined values are Bottom = 60, Left = 36, Right = 36, Top = 36.
        /// </summary>
        /// <param name="margins">The new document's margins</param>
        public void DocumentMargins(DocumentMargins margins)
        {
            _pdfReport.DataBuilder.DefaultDocumentMargins(margins);
        }

        /// <summary>
        /// Sets the run direction to rtl or ltr.
        /// </summary>
        /// <param name="direction">run direction, rtl or ltr</param>
        public void RunDirection(PdfRunDirection direction)
        {
            _pdfReport.DataBuilder.DefaultRunDirection(direction);
        }

        /// <summary>
        /// Sets the page size to PageSize.A4 for instance.
        /// </summary>
        /// <param name="pageSize">selected page size</param>
        public void CustomPageSize(iTextSharp.text.Rectangle pageSize)
        {
            _pdfReport.DataBuilder.DefaultPageSize(pageSize);
        }

        /// <summary>
        /// Sets the page size to PageSize.A4 for instance.
        /// </summary>
        /// <param name="size">selected page size</param>
        public void PageSize(PdfPageSize size)
        {
            _pdfReport.DataBuilder.DefaultPageSize(size.ToRectangle());
        }

        /// <summary>
        /// Sets the PageOrientation.
        /// </summary>
        /// <param name="value">Page Orientation</param>
        public void Orientation(PageOrientation value)
        {
            _pdfReport.DataBuilder.DefaultOrientation(value);
        }

        /// <summary>
        /// Background image's file path and position.
        /// </summary>
        /// <param name="backgroundImageFilePath">Background image's file path.</param>
        /// <param name="position">Background image's position. If it's set to null, the image will be painted at the center of the page.</param>
        public void BackgroundImage(string backgroundImageFilePath, System.Drawing.PointF? position = null)
        {
            _pdfReport.DataBuilder.DefaultBackgroundImageFilePath(backgroundImageFilePath);
            _pdfReport.DataBuilder.DefaultBackgroundImagePosition(position);
        }

        /// <summary>
        /// Setting Page Background Color.
        /// </summary>
        /// <param name="backgroundColor">Pages Background Color</param>
        public void PagesBackgroundColor(System.Drawing.Color backgroundColor)
        {
            _pdfReport.DataBuilder.DefaultPagesBackgroundColor(backgroundColor);
        }

        /// <summary>
        /// Sets document's metadata.
        /// </summary>
        /// <param name="metadata">document's metadata</param>
        public void DocumentMetadata(DocumentMetadata metadata)
        {
            _pdfReport.DataBuilder.DefaultDocumentMetadata(metadata);
        }

        /// <summary>
        /// Specifies the Adobe Reader's settings when a document is opened.
        /// </summary>
        /// <param name="preferences">viewer preferences</param>
        public void ViewerPreferences(PdfViewerPreferences preferences)
        {
            _pdfReport.DataBuilder.DefaultViewerPreferences(preferences);
        }

        /// <summary>
        /// A watermark text to display.
        /// </summary>
        /// <param name="watermark">watermark settings</param>
        public void DiagonalWatermark(DiagonalWatermark watermark)
        {
            _pdfReport.DataBuilder.DefaultDiagonalWatermark(watermark);
        }

        /// <summary>
        /// Compression settings.
        /// </summary>
        /// <param name="settings">Compression settings</param>
        public void Compression(CompressionSettings settings)
        {
            _pdfReport.DataBuilder.DefaultCompression(settings);
        }

        /// <summary>
        /// Sets subsets of the PDF specification (ISO 15930-1 to ISO 15930-8) that promise 
        /// predictable and consistent output for press printing.
        /// </summary>
        /// <param name="level">conformance level</param>
        public void ConformanceLevel(PdfXConformance level)
        {
            _pdfReport.DataBuilder.DefaultConformanceLevel(level);
        }

        /// <summary>
        /// Indicates default values of print dialog box.
        /// </summary>
        /// <param name="preferences">printing preferences</param>
        public void PrintingPreferences(PrintingPreferences preferences)
        {
            _pdfReport.DataBuilder.DefaultPrintingPreferences(preferences);
        }

        /// <summary>
        /// Adds an optional file attachment at the document level.
        /// </summary>
        /// <param name="fileAttachment">a file attachment</param>
        public void AddFileAttachment(FileAttachment fileAttachment)
        {
            if (_pdfReport.DataBuilder.FileAttachments == null)
                _pdfReport.DataBuilder.FileAttachments = new List<FileAttachment>();

            _pdfReport.DataBuilder.FileAttachments.Add(fileAttachment);
        }
    }
}
