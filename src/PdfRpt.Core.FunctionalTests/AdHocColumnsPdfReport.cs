using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.CustomDataSources;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class AdHocColumnsPdfReport
    {
        [TestMethod]
        public void Verify_AdHocColumnsPdfReport_Can_Be_Created()
        {
            var report = CreateAdHocColumnsPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateAdHocColumnsPdfReport()
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
                     defaultHeader.Message("Our new rpt.");
                 });
             })
             .MainTableTemplate(template =>
             {
                 template.BasicTemplate(BasicTemplate.SilverTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
             })
             .MainTableDataSource(dataSource =>
             {
                 dataSource.CustomDataSource(
                     () => new SqliteDataReaderDataSource(
                         connectionString: $"Data Source={System.IO.Path.Combine(TestUtils.GetBaseDir(), "data", "blogs.sqlite")}",
                         sql: @"SELECT [url] as 'Url', [name] as 'Name', [NumberOfPosts] as 'Number Of Posts', [AddDate] as 'Add Date'
                                FROM [tblBlogs]
                                WHERE [NumberOfPosts]>=@p1",
                    parametersValues: new object[] { 10 }));
             })
             .MainTableSummarySettings(summary =>
             {
                 summary.OverallSummarySettings("Summary");
                 summary.PreviousPageSummarySettings("Previous Page Summary");
                 summary.PageSummarySettings("Page Summary");
             })
             .MainTableAdHocColumnsConventions(adHocColumns =>
             {
                 //We want sum of the int columns
                 adHocColumns.AddTypeAggregateFunction(
                     typeof(Int64),
                     new AggregateProvider(AggregateFunction.Sum)
                     {
                         DisplayFormatFormula = obj => string.IsNullOrEmpty(obj?.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj)
                     });

                 //We want to dispaly all of the dateTimes as dd/MM/yyyy HH:mm
                 adHocColumns.AddTypeDisplayFormatFormula(
                     typeof(DateTime),
                     data => { return ((DateTime)data).ToString("dd/MM/yyyy HH:mm"); }
                 );
                 adHocColumns.ShowRowNumberColumn(true);
                 adHocColumns.RowNumberColumnCaption("#");
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
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