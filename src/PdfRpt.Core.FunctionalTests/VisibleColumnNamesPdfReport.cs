using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Aggregates.Numbers;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.DataAnnotations;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class VisibleColumnNamesPdfReport
    {
        [TestMethod]
        public void Verify_VisibleColumnNamesPdfReport_Can_Be_Created()
        {
            var report = CreateVisibleColumnNamesPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateVisibleColumnNamesPdfReport()
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
                     defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                     defaultHeader.Message("new rpt.");
                     defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                 });
             })
             .MainTableTemplate(template =>
             {
                 template.BasicTemplate(BasicTemplate.ClassicTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.FitToContent);
             })
             .MainTableDataSource(dataSource =>
             {
                 dataSource.StronglyTypedList(PersonnelDataSource.CreatePersonnelList());
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
             })
             .MainTableSummarySettings(summary =>
             {
                 summary.OverallSummarySettings("Total");
                 summary.PageSummarySettings("Page Summary");
                 summary.PreviousPageSummarySettings("Pervious Page Summary");
             })
             .MainTableAdHocColumnsConventions(adHocColumns =>
             {
                 adHocColumns.ShowRowNumberColumn(true);
                 adHocColumns.RowNumberColumnCaption("#");
                 adHocColumns.VisibleColumnNames(new List<string>() { "Name", "JobTitle", "Salary" });
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