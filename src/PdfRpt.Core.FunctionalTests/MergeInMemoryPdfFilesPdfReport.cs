using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class MergeInMemoryPdfFilesPdfReport
    {
        [TestMethod]
        public void Verify_MergeInMemoryPdfFilesPdfReport_Can_Be_Created()
        {
            var report = CreateMergeInMemoryPdfFilesPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report);
        }

        public string CreateMergeInMemoryPdfFilesPdfReport()
        {
            return mergeMultipleReports();
        }

        private string mergeMultipleReports()
        {
            // It's an in-memory PDF report
            var file1ContentBytes = new PdfReportToArray().CreatePdfReport();

            var file1Path = Path.Combine(TestUtils.GetOutputFolder(), "PdfReportToArray.pdf");
            File.WriteAllBytes(file1Path, file1ContentBytes);

            using (var mergedFileStream = new MemoryStream())
            {
                new MergePdfDocuments
                {
                    DocumentMetadata =
                        new DocumentMetadata
                        {
                            Author = "Vahid",
                            Application = "PdfRpt",
                            Keywords = "Test",
                            Subject = "MergePdfFiles Rpt.",
                            Title = "Test"
                        },
                    InputFileStreams = new Stream[]
                    {
                        // Using the input in-memory PDF report(s)
                        new MemoryStream(file1ContentBytes),
                        new MemoryStream(file1ContentBytes)
                    },
                    OutputFileStream = mergedFileStream,
                    AttachmentsBookmarkLabel = "Attachment(s) ",
                    WriterCustomizer = importedPageInfo =>
                    {
                        addNewPageNumbersToFinalMergedFile(importedPageInfo);
                    }
                }.PerformMerge();

                // It's still an in-memory PDF file. Save it to a file or flush it in the browser.
                var mergedFileContentBytes = mergedFileStream.ToArray();

                // Save it to a file.
                var finalMergedFile = Path.Combine(TestUtils.GetOutputFolder(), "mergedFile2.pdf");
                File.WriteAllBytes(finalMergedFile, mergedFileContentBytes);
                return finalMergedFile;
            }
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
                phrase: new Phrase("Page " + importedPageInfo.CurrentPageNumber + "/" + importedPageInfo.TotalNumberOfPages),
                x: center,
                y: pageSize.GetBottom(25),
                rotation: 0,
                runDirection: PdfWriter.RUN_DIRECTION_LTR,
                arabicOptions: 0);
        }
    }

    public class PdfReportToArray
    {
        public byte[] CreatePdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "Test Rpt", Title = "Test" });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
            })
             .DefaultFonts(fonts =>
             {
                 fonts.Path(TestUtils.GetVerdanaFontPath(),
                            TestUtils.GetTahomaFontPath());
                 fonts.Size(9);
                 fonts.Color(Color.Black);
             })
             .PagesFooter(footer =>
             {
                 footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
             })
             .PagesHeader(header =>
             {
                 header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                 header.DefaultHeader(defaultHeader =>
                 {
                     defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                     defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                     defaultHeader.Message("Our new rpt.");
                 });
             })
             .MainTableSummarySettings(summarySettings =>
             {
                 summarySettings.OverallSummarySettings("Summary");
                 summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                 summarySettings.PageSummarySettings("Page Summary");
             })
             .MainTableTemplate(template =>
             {
                 template.BasicTemplate(BasicTemplate.ClassicTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
             })
             .MainTableDataSource(dataSource =>
             {
                 var listOfRows = new List<User>();
                 for (int i = 0; i < 200; i++)
                 {
                     listOfRows.Add(new User { Id = i, LastName = "Last Name " + i, Name = "Name " + i, Balance = i + 1000 });
                 }
                 dataSource.StronglyTypedList(listOfRows);
             })
             .MainTableColumns(columns =>
             {
                 columns.AddColumn(column =>
                 {
                     column.PropertyName("rowNo");
                     column.IsRowNumber(true);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(0);
                     column.Width(1);
                     column.HeaderCell("#");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<User>(x => x.Id);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.Width(2);
                     column.HeaderCell("Id");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<User>(x => x.Name);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                     column.Width(3);
                     column.HeaderCell("Name");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<User>(x => x.LastName);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                     column.Width(3);
                     column.HeaderCell("Last Name");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<User>(x => x.Balance);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(4);
                     column.Width(2);
                     column.HeaderCell("Balance");
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.AggregateFunction(aggregateFunction =>
                     {
                         aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                         aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj));
                     });
                 });
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
                 events.DocumentClosing(args =>
                 {
                     // close the document without closing the underlying stream
                     //args.PdfWriter.CloseStream = false;
                     //args.PdfDoc.Close();

                     //args.PdfStreamOutput.Position = 0;
                     //var pdf = ((MemoryStream)args.PdfStreamOutput).ToArray();
                 });
             })
             .Export(export =>
             {
                 export.ToExcel();
                 export.ToXml();
             })
             .GenerateAsByteArray(); // An in-memory pdf file
        }
    }
}