using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class MergePdfFilesPdfReport
    {
        [TestMethod]
        public void Verify_MergePdfFilesPdfReport_Can_Be_Created()
        {
            var report = CreateMergePdfFilesPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report);
        }

        public string CreateMergePdfFilesPdfReport()
        {
            return mergeMultipleReports();
        }

        private string mergeMultipleReports()
        {
            var rpt1 = new IListPdfReport().CreateIListPdfReport();
            var rpt2 = new HexDumpPdfReport().CreateHexDumpPdfReport();

            var finalMergedFile = Path.Combine(TestUtils.GetOutputFolder(), "mergedFile.pdf");

            var mergePdfDocuments = new MergePdfDocuments
            {
                DocumentMetadata = new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "MergePdfFiles Rpt.", Title = "Test" },
                InputFileStreams = new Stream[]
                {
                    new FileStream(rpt1.FileName, FileMode.Open, FileAccess.Read, FileShare.Read),
                    new FileStream(rpt2.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
                },
                OutputFileStream = new FileStream(finalMergedFile, FileMode.Create),
                AttachmentsBookmarkLabel = "Attachment(s) ",
                WriterCustomizer = importedPageInfo =>
                {
                    addNewPageNumbersToFinalMergedFile(importedPageInfo);
                }
            };
            mergePdfDocuments.PerformMerge();

            foreach (var stream in mergePdfDocuments.InputFileStreams)
            {
                stream?.Dispose();
            }
            mergePdfDocuments.OutputFileStream?.Dispose();

            return finalMergedFile;
        }

        private void addNewPageNumbersToFinalMergedFile(ImportedPageInfo importedPageInfo)
        {
            var bottomMargin = importedPageInfo.PdfDocument.BottomMargin;
            var pageSize = importedPageInfo.PageSize;
            var contentByte = importedPageInfo.Stamp.GetOverContent();

            // hide the old footer
            contentByte.SaveState();
            contentByte.SetColorFill(BaseColor.White);
            contentByte.Rectangle(0, 0, pageSize.Width, bottomMargin);
            contentByte.Fill();
            contentByte.RestoreState();

            // write the new page numbers
            var center = (pageSize.Left + pageSize.Right) / 2;
            ColumnText.ShowTextAligned(
                canvas: contentByte,
                alignment: Element.ALIGN_CENTER,
                phrase: new Phrase($"Page {importedPageInfo.CurrentPageNumber}/{importedPageInfo.TotalNumberOfPages}"),
                x: center,
                y: pageSize.GetBottom(25),
                rotation: 0,
                runDirection: PdfWriter.RUN_DIRECTION_LTR,
                arabicOptions: 0);
        }
    }
}