using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Merge multiple PDF files into one PDF file.
    /// </summary>
    public class MergePdfDocuments
    {
        private int _attachmentsCount;
        private Document _document;
        private int _fileNumber;
        private int _overallPageNumber;
        private int _totalNumberOfPages;
        private PdfSmartCopy _writer;

        /// <summary>
        /// ctor.
        /// </summary>
        public MergePdfDocuments()
        {
            PdfReader.AllowOpenWithFullPermissions = true; // Allows reading the protected files.
        }

        /// <summary>
        /// MergePdfDocuments adds a link to the attachments panel of Adobe reader automatically and
        /// AttachmentsBookmarkLabel sets its display label.
        /// </summary>
        public string AttachmentsBookmarkLabel { set; get; }

        /// <summary>
        /// Input files list to merge.
        /// </summary>
        public IList<Stream> InputFileStreams { set; get; }

        /// <summary>
        /// Defines metadata information of the Document.
        /// </summary>
        public DocumentMetadata DocumentMetadata { set; get; }

        /// <summary>
        /// Merged file's stream.
        /// </summary>
        public Stream OutputFileStream { set; get; }

        /// <summary>
        /// WriterCustomizer allows writing additional information to the final merged file.
        /// </summary>
        public Action<ImportedPageInfo> WriterCustomizer { set; get; }

        /// <summary>
        /// Start merging multiple PDF files into one PDF file.
        /// </summary>
        public void PerformMerge()
        {
            try
            {
                init();
                addMetadata();
                findTotalNumberOfPages();
                processInputFiles();
                addAttachmentsOutline();
                setOpenAction();
            }
            finally
            {
                freeResources();
            }
        }

        private void setOpenAction()
        {
            _writer.SetOpenAction(PdfAction.GotoLocalPage(1, new PdfDestination(PdfDestination.XYZ, 0, _document.PageSize.Height, 1), _writer));
        }

        private void init()
        {
            _document = new Document();
            _writer = new PdfSmartCopy(_document, OutputFileStream);
            _writer.SetFullCompression();
            _document.Open();
        }

        private void addAttachment(AttachmentFile file)
        {
            var pdfDictionary = new PdfDictionary();
            pdfDictionary.Put(PdfName.Moddate, new PdfDate(DateTime.Now));
            var fs = PdfFileSpecification.FileEmbedded(_writer, string.Empty, file.FileName, file.Content, true, null, pdfDictionary);
            _writer.AddFileAttachment(fs);
        }

        private void addAttachmentsOutline()
        {
            if (_attachmentsCount <= 0) return;
            var action = PdfAction.JavaScript("app.execMenuItem('ShowHideFileAttachment');", _writer);
            var rootOutline = _writer.DirectContent.RootOutline;
            new PdfOutline(rootOutline, action, AttachmentsBookmarkLabel + " " + _attachmentsCount);
        }

        private void addBookmark(PdfReader reader)
        {
            var subject = getSubject(reader);
            var rootOutline = _writer.DirectContent.RootOutline;
            var name = Guid.NewGuid().ToString();
            _document.Add(new Chunk(" ").SetLocalDestination(name));
            new PdfOutline(rootOutline, PdfAction.GotoLocalPage(name, false), subject);
            _writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
        }

        private void addContentToPage(PdfReader reader, Rectangle size, PdfImportedPage page)
        {
            if (WriterCustomizer == null)
                return;

            var stamp = _writer.CreatePageStamp(page);
            WriterCustomizer(new ImportedPageInfo
            {
                FileNumber = _fileNumber,
                Reader = reader,
                Stamp = stamp,
                PageSize = size,
                ImportedPage = page,
                CurrentPageNumber = _overallPageNumber,
                PdfDocument = _document,
                TotalNumberOfPages = _totalNumberOfPages
            });
            stamp.AlterContents();
        }

        private void addFilePages(Stream fileStream)
        {
            PdfReader reader = null;
            try
            {
                _fileNumber++;
                _attachmentsCount += copyAttachments(fileStream);
                fileStream = fileStream.ReopenForReading();
                reader = new PdfReader(fileStream);
                reader.ConsolidateNamedDestinations();

                addBookmark(reader);

                int numberOfPages = reader.NumberOfPages;
                for (int pageNumber = 1; pageNumber <= numberOfPages; pageNumber++)
                {
                    var size = reader.GetPageSizeWithRotation(pageNumber);
                    _document.SetPageSize(size);
                    _document.NewPage();
                    _overallPageNumber++;

                    var page = _writer.GetImportedPage(reader, pageNumber);
                    addContentToPage(reader, size, page);
                    _writer.AddPage(page);
                }
            }
            finally
            {
                if (reader != null)
                    _writer.FreeReader(reader);
            }
        }

        private void addMetadata()
        {
            var version = typeof(MergePdfDocuments).GetTypeInfo().Assembly.GetName().Version.ToString();

            if (DocumentMetadata != null)
            {
                _document.AddTitle(DocumentMetadata.Title);
                _document.AddSubject(DocumentMetadata.Subject);
                _document.AddAuthor(DocumentMetadata.Author);
                _document.AddCreator(DocumentMetadata.Application + ", Using PdfRpt V" + version);
                _document.AddKeywords(DocumentMetadata.Keywords);
            }
            else
            {
                _document.AddCreator("PdfRpt V" + version);
            }

            _writer.CreateXmpMetadata();
            _writer.SetPdfVersion(PdfWriter.PdfVersion15);
            _writer.RgbTransparencyBlending = true;
        }

        private int copyAttachments(Stream file)
        {
            var attachmentsCount = 0;

            var attachments = file.PerformExtraction();
            if (!attachments.Any())
                return attachmentsCount;

            foreach (var attachment in attachments)
            {
                addAttachment(attachment);
                attachmentsCount++;
            }
            return attachmentsCount;
        }

        private void findTotalNumberOfPages()
        {
            foreach (var fileStream in InputFileStreams)
            {
                var reader = new PdfReader(fileStream);
                _totalNumberOfPages += reader.NumberOfPages;
                reader.Close();
            }
        }

        private void freeResources()
        {
            if (_document == null)
                return;

            _writer.Close();
            _document.Close();
        }

        private string getSubject(PdfReader reader)
        {
            string subject;
            subject = (string)reader.Info["Subject"];

            if (string.IsNullOrEmpty(subject))
                subject = (string)reader.Info["Title"];

            return string.IsNullOrEmpty(subject) ? $"File-{_fileNumber}" : subject;
        }

        private void processInputFiles()
        {
            foreach (var fileStream in InputFileStreams)
            {
                addFilePages(fileStream);
            }
        }
    }
}