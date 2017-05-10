using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class CsvToPdfReport
    {
        [TestMethod]
        public void Verify_CsvToPdfReport_Can_Be_Created()
        {
            var report = CreateCsvToPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateCsvToPdfReport()
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
                 fonts.Color(System.Drawing.Color.Black);
             })
             .PagesFooter(footer =>
             {
                 footer.DefaultFooter(printDate: DateTime.Now.ToString("MM/dd/yyyy"));
             })
             .PagesHeader(header =>
             {
                 header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                 header.DefaultHeader(defaultHeader =>
                 {
                     defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                     defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                     defaultHeader.Message("Our new rpt. 1 2 3 4 5 test 6 7 def");
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
                 var data = File.ReadAllLines(Path.Combine(TestUtils.GetBaseDir(), "data", "sample.csv"))
                                      .Select(line => line.Split(','))
                                      .Select(columns =>
                                               new
                                               {
                                                   UserName = columns[0],
                                                   Age = int.Parse(columns[1]),
                                                   Salary = decimal.Parse(columns[2])
                                               });
                 dataSource.AnonymousTypeList(data);
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
                     column.PropertyName("UserName");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.Width(2);
                     column.HeaderCell("Name");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Age");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                     column.Width(3);
                     column.HeaderCell("Age");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Salary");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                     column.Width(3);
                     column.HeaderCell("Salary");
                     column.AggregateFunction(aggregateFunction =>
                     {
                         aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                         aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj));
                     });
                 });
             })
             .Encrypt(encrypt =>
             {
                 encrypt.WithPassword("12345678");
                 encrypt.WithPermissions(new DocumentPermissions
                 {
                     AllowAssembly = false,
                     AllowCopy = true,
                     AllowDegradedPrinting = false,
                     AllowFillIn = false,
                     AllowModifyAnnotations = false,
                     AllowModifyContents = false,
                     AllowPrinting = false,
                     AllowScreenReaders = true
                 });
             })
             .Export(export =>
             {
                 export.ToExcel();
                 export.ToXml();
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }
}
