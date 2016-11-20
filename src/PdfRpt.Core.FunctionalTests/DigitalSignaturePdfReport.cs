using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class DigitalSignaturePdfReport
    {
        //For more info about creating a self-signed X.509 certificate, please visit:
        //http://www.pluralsight-training.net/community/blogs/keith/archive/2009/01/22/create-self-signed-x-509-certificates-in-a-flash-with-self-cert.aspx

        [TestMethod]
        public void Verify_DigitalSignaturePdfReport_Can_Be_Created()
        {
            var report = CreateDigitalSignaturePdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateDigitalSignaturePdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "Test Rpt", Title = "Test" });
                doc.ViewerPreferences(new PdfViewerPreferences
                {
                    ZoomPercent = 95,
                    PageMode = ViewerPageMode.UseAttachments,
                    PagesDirection = PagesDirection.DirectionR2L,
                    ViewerPreferences = ViewerPreferences.HideMenubar | ViewerPreferences.HideToolBar
                });
                doc.PrintingPreferences(new PrintingPreferences
                {
                    NumberOfCopies = 3
                });
                doc.AddFileAttachment(new FileAttachment { FilePath = System.IO.Path.Combine(TestUtils.GetBaseDir(), "data", "cert123.pfx"), Description = "Test" });
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
                 fonts.Color(System.Drawing.Color.Black);
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
                 for (int i = 0; i < 50; i++)
                 {
                     listOfRows.Add(new User { Id = i, LastName = "Last Name " + i, Name = "Name " + i, Balance = i + 1000 });
                 }
                 dataSource.StronglyTypedList(listOfRows);
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
             })
             .MainTableSummarySettings(summarySettings =>
             {
                 summarySettings.OverallSummarySettings("Summary");
                 summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                 summarySettings.PageSummarySettings("Page Summary");
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
             .Encrypt(encrypt =>
             {
                 encrypt.WithPassword("1234");
                 encrypt.WithPermissions(new DocumentPermissions
                 {
                     AllowAssembly = false,
                     AllowCopy = false,
                     AllowDegradedPrinting = true,
                     AllowFillIn = false,
                     AllowModifyAnnotations = false,
                     AllowModifyContents = false,
                     AllowPrinting = true,
                     AllowScreenReaders = false
                 });
             })
             .Sign(sign =>
             {
                 sign.WithCertificateFile(appendSignature: true, pfxPassword: "123", pfxPath: System.IO.Path.Combine(TestUtils.GetBaseDir(), "data", "cert123.pfx"));
                 sign.SigningInfo(reason: "I'm the author!", contact: "email@site.com", location: "Tehran");
                 sign.VisibleSignature(text: "My sig. امضاي من", useLastPageToShowSignature: true, position: new iTextSharp.text.Rectangle(0, 0, 100, 100), runDirection: PdfRunDirection.RightToLeft);
             })
             .Export(export =>
             {
                 export.ToXml();
                 export.ToExcel();
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }
}