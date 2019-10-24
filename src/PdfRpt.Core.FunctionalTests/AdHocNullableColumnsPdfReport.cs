using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class AdHocNullableColumnsPdfReport
    {
        [TestMethod]
        public void Verify_AdHocColumnsPdfReport_Can_Be_Created()
        {
            var report = CreateAdHocNullableColumnsPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateAdHocNullableColumnsPdfReport()
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
                 var listOfRows = new List<TestNullableEntity>
                 {
                    new TestNullableEntity {Id = 0, NullableBoolean = null, NullableDateTime = null}
                 };

                 for (var i = 1; i <= 200; i++)
                 {
                     listOfRows.Add(new TestNullableEntity
                     {
                         Id = i,
                         NullableBoolean = i % 2 != 0,
                         NullableDateTime = i % 2 == 0 ? (DateTime?)null : DateTime.Now.AddDays(i)
                     });
                 }
                 dataSource.StronglyTypedList(listOfRows);
             })
             .MainTableSummarySettings(summary =>
             {
                 summary.OverallSummarySettings("Summary");
                 summary.PreviousPageSummarySettings("Previous Page Summary");
                 summary.PageSummarySettings("Page Summary");
             })
             .MainTableAdHocColumnsConventions(adHocColumns =>
             {
                 adHocColumns.AddTypeDisplayFormatFormula(
                        typeof(DateTime?),
                        data => ((DateTime?)data)?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));

                 adHocColumns.AddTypeDisplayFormatFormula(
                     typeof(bool?),
                     data =>
                     {
                         if (data == null) return string.Empty;
                         return ((bool?)data).Value ? "Yes" : "No";
                     });
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