using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Extracting all of the document level attachments and file attachment annotations of a given PDF file.
    /// </summary>
    public static class ExtractPdfFileAttachments
    {
        /// <summary>
        /// Start extracting all of the document level attachments and file attachment annotations of a given PDF file.
        /// </summary>
        /// <param name="pdfFileStream">Input file's stream.</param>
        /// <returns>List of the extracted attachments.</returns>
        public static IList<AttachmentFile> PerformExtraction(this Stream pdfFileStream)
        {
            var results = new List<AttachmentFile>();

            PdfReader.AllowOpenWithFullPermissions = true; // Allows reading the protected files.
            pdfFileStream = pdfFileStream.ReopenForReading();
            var reader = new PdfReader(pdfFileStream);
            prepareAttachments(results, reader);
            reader.Close();

            return results;
        }

        /// <summary>
        /// Start extracting all of the document level attachments and file attachment annotations of a given PDF file.
        /// </summary>
        /// <param name="pdfFilePath">Input file's path.</param>
        /// <returns>List of the extracted attachments.</returns>
        public static IList<AttachmentFile> PerformExtraction(string pdfFilePath)
        {
            var results = new List<AttachmentFile>();

            PdfReader.AllowOpenWithFullPermissions = true; // Allows reading the protected files.
            var reader = new PdfReader(pdfFilePath);
            prepareAttachments(results, reader);
            reader.Close();

            return results;
        }

        private static IList<AttachmentFile> extractDocumentLevelAttachments(PdfReader reader)
        {
            var results = new List<AttachmentFile>();

            var names = PdfReader.GetPdfObject(reader.Catalog.Get(PdfName.Names)) as PdfDictionary;
            if (names == null)
                return null;

            var files = PdfReader.GetPdfObject(names.Get(PdfName.Embeddedfiles)) as PdfDictionary;
            if (files == null)
                return null;

            var filesMap = PdfNameTree.ReadTree(files);
            foreach (var item in filesMap.Values)
            {
                var filespec = PdfReader.GetPdfObject((PdfObject)item) as PdfDictionary;
                var file = extractFile(filespec);
                if (file == null)
                    continue;
                results.Add(file);
            }

            return results;
        }

        private static AttachmentFile extractFile(PdfDictionary filespec)
        {
            if (filespec == null)
                return null;

            var type = PdfReader.GetPdfObject(filespec.Get(PdfName.TYPE)) as PdfName;
            if (!PdfName.F.Equals(type) && !PdfName.Filespec.Equals(type))
                return null;

            var ef = PdfReader.GetPdfObject(filespec.Get(PdfName.EF)) as PdfDictionary;
            if (ef == null)
                return null;

            var fn = PdfReader.GetPdfObject(filespec.Get(PdfName.F)) as PdfString;
            if (fn == null)
                return null;

            var prs = PdfReader.GetPdfObject(ef.Get(PdfName.F)) as PrStream;
            if (prs == null)
                return null;

            return new AttachmentFile
            {
                Content = PdfReader.GetStreamBytes(prs),
                FileName = fn.ToUnicodeString()
            };
        }

        private static IList<AttachmentFile> extractFileAttachmentAnnotations(PdfReader reader)
        {
            var results = new List<AttachmentFile>();

            var numberOfPages = reader.NumberOfPages;
            for (int pageNumber = 1; pageNumber <= numberOfPages; ++pageNumber)
            {
                var annots = PdfReader.GetPdfObject(reader.GetPageN(pageNumber).Get(PdfName.Annots)) as PdfArray;
                if (annots == null)
                    continue;

                foreach (var item in annots.ArrayList)
                {
                    var annot = PdfReader.GetPdfObject((PdfObject)item) as PdfDictionary;
                    if (annot == null)
                        continue;

                    var subType = PdfReader.GetPdfObject(annot.Get(PdfName.Subtype)) as PdfName;
                    if (!PdfName.Fileattachment.Equals(subType))
                        continue;

                    var filespec = PdfReader.GetPdfObject(annot.Get(PdfName.Fs)) as PdfDictionary;
                    var file = extractFile(filespec);
                    if (file == null)
                        continue;
                    results.Add(file);
                }
            }

            return results;
        }

        private static void prepareAttachments(List<AttachmentFile> results, PdfReader reader)
        {
            var documentLevelFiles = extractDocumentLevelAttachments(reader);
            if (documentLevelFiles != null && documentLevelFiles.Any())
                results.AddRange(documentLevelFiles);

            var fileAttachmentAnnotations = extractFileAttachmentAnnotations(reader);
            if (fileAttachmentAnnotations != null && fileAttachmentAnnotations.Any())
                results.AddRange(fileAttachmentAnnotations);
        }
    }
}
