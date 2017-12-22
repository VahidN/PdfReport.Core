using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Security;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// General PDF Document Settings
    /// </summary>
    public class DocumentSettings
    {
        /// <summary>
        /// PdfWriter Object
        /// </summary>
        public PdfWriter PdfWriter { get; set; }

        /// <summary>
        /// Sets the encryption options for this document.
        /// Leave it as null if you don't want to use it.
        /// </summary>
        public DocumentSecurity DocumentSecurity { get; set; }

        /// <summary>
        /// Document settings.
        /// </summary>
        public DocumentPreferences PageSetup { get; set; }

        readonly IDictionary<PdfVersion, PdfName> _pdfVersion = new Dictionary<PdfVersion, PdfName>
        {
            { PdfVersion.Version12, PdfWriter.PdfVersion12 },
            { PdfVersion.Version13, PdfWriter.PdfVersion13 },
            { PdfVersion.Version14, PdfWriter.PdfVersion14 },
            { PdfVersion.Version15, PdfWriter.PdfVersion15 },
            { PdfVersion.Version16, PdfWriter.PdfVersion16 },
            { PdfVersion.Version17, PdfWriter.PdfVersion17 }
        };

        /// <summary>
        /// Some settings should be called before PdfDoc.Open().
        /// </summary>
        public void ApplyBeforePdfDocOpenSettings()
        {
            setPdfVersion();
            setCompressionLevel();
        }

        /// <summary>
        /// Apply initial settings
        /// </summary>
        public void ApplySettings()
        {
            setInitialDocumentZoomPercent();
            addMetadata();
            applyPrintingPreferences();
            addAutoPrint();
            applyViewerSettings();
        }

        private void setCompressionLevel()
        {
            if (!isCompressionEnabled) return;

            var version = _pdfVersionName.ToString().Replace("/", string.Empty);
            if (double.Parse(version, CultureInfo.InvariantCulture) >= 1.5)
            {
                if (PageSetup.CompressionSettings.EnableFullCompression)
                    PdfWriter.SetFullCompression();
                else
                    PdfWriter.CompressionLevel = (int)PageSetup.CompressionSettings.CompressionLevel;
            }
        }

        private bool isCompressionEnabled
        {
            get
            {
                return PageSetup.CompressionSettings != null &&
                                (PageSetup.CompressionSettings.EnableCompression ||
                                    PageSetup.CompressionSettings.EnableFullCompression);
            }
        }

        PdfName _pdfVersionName;
        private void setPdfVersion()
        {
            _pdfVersionName = new PdfName("1.5");
            if (PageSetup.ViewerPreferences != null)
            {
                _pdfVersionName = _pdfVersion[PageSetup.ViewerPreferences.PdfVersion];
            }
            PdfWriter.SetPdfVersion(_pdfVersionName);
        }

        private void applyPrintingPreferences()
        {
            var printingPreferences = PageSetup.PrintingPreferences;
            if (printingPreferences == null) return;

            if (printingPreferences.PrintScaling == PrintScaling.None)
                PdfWriter.AddViewerPreference(PdfName.Printscaling, PdfName.None);

            if (printingPreferences.PrintScaling == PrintScaling.Default)
                PdfWriter.AddViewerPreference(PdfName.Printscaling, PdfName.Appdefault);

            if (printingPreferences.PrintSide == PrintSide.Simplex)
                PdfWriter.AddViewerPreference(PdfName.Duplex, PdfName.Simplex);

            if (printingPreferences.PrintSide == PrintSide.DuplexFlipShortEdge)
                PdfWriter.AddViewerPreference(PdfName.Duplex, PdfName.Duplexflipshortedge);

            if (printingPreferences.PrintSide == PrintSide.DuplexFlipLongEdge)
                PdfWriter.AddViewerPreference(PdfName.Duplex, PdfName.Duplexfliplongedge);

            if (printingPreferences.PickTrayByPdfSize)
            {
                PdfWriter.AddViewerPreference(PdfName.Picktraybypdfsize, PdfBoolean.Pdftrue);
            }

            if (printingPreferences.NumberOfCopies >= 2 && printingPreferences.NumberOfCopies <= 5)
            {
                PdfWriter.AddViewerPreference(PdfName.Numcopies, new PdfNumber(printingPreferences.NumberOfCopies));
            }
        }

        private void applyViewerSettings()
        {
            var viewerSettings = PageSetup.ViewerPreferences;
            if (viewerSettings == null) return;

            if (PageSetup.FileAttachments != null || PageSetup.ExportSettings != null)
            {
                if (PageSetup.ConformanceLevel != PdfXConformance.PDFXNONE)
                    throw new InvalidOperationException("Embedded files are not allowed in PDF/A standard.");

                viewerSettings.PageMode = ViewerPageMode.UseAttachments;
            }

            int settings = PdfWriter.PageLayoutSinglePage;
            if (viewerSettings.PageLayout != ViewerPageLayout.SinglePage)
            {
                settings = (int)viewerSettings.PageLayout;
            }

            if (viewerSettings.PageMode != ViewerPageMode.UseNone)
            {
                settings |= (int)viewerSettings.PageMode;
            }

            if (viewerSettings.PageMode == ViewerPageMode.FullScreen)
            {
                if (viewerSettings.NonFullScreenPageMode != NonFullScreenPageMode.UseNone)
                {
                    settings |= (int)viewerSettings.NonFullScreenPageMode;
                }
            }

            if (viewerSettings.ViewerPreferences != null && viewerSettings.ViewerPreferences != ViewerPreferences.UseNone)
            {
                settings |= (int)viewerSettings.ViewerPreferences;
            }

            settings |= (int)viewerSettings.PagesDirection;
            PdfWriter.ViewerPreferences = settings;
        }


        /// <summary>
        /// Enable Encryption
        /// </summary>
        public void SetEncryption()
        {
            new EncryptionWorker
            {
                DocumentSecurity = DocumentSecurity,
                PdfDoc = PdfDoc,
                PdfWriter = PdfWriter
            }.ApplyEncryption();
        }

        /// <summary>
        /// Applies a digital signature to a document
        /// </summary>
        public void ApplySignature(Stream pdfStreamOutput)
        {
            new EncryptionWorker
            {
                DocumentSecurity = DocumentSecurity,
                PdfDoc = PdfDoc,
                PdfWriter = PdfWriter
            }.ApplySignature(pdfStreamOutput);
        }

        /// <summary>
        /// Document object
        /// </summary>
        public Document PdfDoc { get; set; }

        private void setInitialDocumentZoomPercent()
        {
            if (PageSetup.ViewerPreferences == null) return;
            var zoom = PageSetup.ViewerPreferences.ZoomPercent.ApproxEquals(0) ? 1 : PageSetup.ViewerPreferences.ZoomPercent / 100;
            var pdfDest = new PdfDestination(PdfDestination.XYZ, 0, PdfDoc.PageSize.Height, zoom);
            var action = PdfAction.GotoLocalPage(1, pdfDest, PdfWriter);
            PdfWriter.SetOpenAction(action);
        }

        private void addMetadata()
        {
#if NET40
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#else
            var version = typeof(DocumentSettings).GetTypeInfo().Assembly.GetName().Version.ToString();
#endif

            if (DocumentProperties != null)
            {
                PdfDoc.AddTitle(DocumentProperties.Title);
                PdfDoc.AddSubject(DocumentProperties.Subject);
                PdfDoc.AddAuthor(DocumentProperties.Author);
                PdfDoc.AddCreator(DocumentProperties.Application + ", Using PdfRpt V" + version);
                PdfDoc.AddKeywords(DocumentProperties.Keywords);
            }
            else
            {
                PdfDoc.AddCreator("PdfRpt V" + version);
            }

            PdfWriter.CreateXmpMetadata();
        }

        /// <summary>
        /// Pdf document's meta-data properties
        /// </summary>
        public DocumentMetadata DocumentProperties { get; set; }

        private void addAutoPrint()
        {
            if (PageSetup.PrintingPreferences == null) return;
            if (PageSetup.PrintingPreferences.ShowPrintDialogAutomatically)
            {
                PdfWriter.AddJavaScript("this.print(true);");
            }
        }

        /// <summary>
        /// Adds all of the file attachments.
        /// </summary>
        public void AddFileAttachments()
        {
            if (PageSetup.FileAttachments == null || !PageSetup.FileAttachments.Any()) return;
            foreach (var file in PageSetup.FileAttachments)
            {
                var fileInfo = new FileInfo(file.FilePath);
                var pdfDictionary = new PdfDictionary();
                pdfDictionary.Put(PdfName.Moddate, new PdfDate(fileInfo.LastWriteTime));
                var fs = PdfFileSpecification.FileEmbedded(PdfWriter, file.FilePath, fileInfo.Name, null, true, null, pdfDictionary);
                PdfWriter.AddFileAttachment(file.Description, fs);
            }
        }

        /// <summary>
        /// Sets the PageSize and its background color.
        /// </summary>
        /// <returns>PageSize info</returns>
        public static Rectangle GetPageSizeAndColor(DocumentPreferences pageSetup)
        {
            var originalPageSize = pageSetup.PagePreferences.Orientation == PageOrientation.Landscape
                                            ? pageSetup.PagePreferences.Size.Rotate() : pageSetup.PagePreferences.Size;
            var notRotatedPageSize = new Rectangle(originalPageSize.Left, originalPageSize.Bottom, originalPageSize.Width, originalPageSize.Height);

            if (pageSetup.PagePreferences.PagesBackgroundColor != null)
                notRotatedPageSize.BackgroundColor = new BaseColor(pageSetup.PagePreferences.PagesBackgroundColor.Value.ToArgb());

            return notRotatedPageSize;
        }
    }
}