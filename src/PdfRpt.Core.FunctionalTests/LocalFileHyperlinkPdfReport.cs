using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class LocalFileHyperlinkPdfReport
    {
        [TestMethod]
        public void Verify_LocalFileHyperlinkPdfReport_Can_Be_Created()
        {
            var report = CreateLocalFileHyperlinkPdfReportPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateLocalFileHyperlinkPdfReportPdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "IList Rpt.", Subject = "Test Rpt", Title = "Test" });
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
                var data = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*")
                                    .Select(file =>
                                               new
                                               {
                                                   Name = Path.GetFileName(file),
                                                   File = new Uri(file).AbsoluteUri // Convert it to file:/// protocol
                                               });
                dataSource.AnonymousTypeList(data);
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
                    column.PropertyName("Name");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("Name");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("File");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(4);
                    column.Width(2);
                    column.HeaderCell("File");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.Hyperlink(
                            foreColor: System.Drawing.Color.Blue,
                            fontUnderline: true,
                            textPropertyName: "Name",
                            navigationUrlPropertyName: "File");
                    });
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }
}