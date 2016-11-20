using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Templates;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class HexDumpPdfReport
    {
        [TestMethod]
        public void Verify_HexDumpPdfReport_Can_Be_Created()
        {
            var report = CreateHexDumpPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateHexDumpPdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "Hex Dump Rpt.", Title = "Test" });
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
                    defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                    defaultHeader.Message("Hex Dump");
                });
            })
            .MainTableTemplate(template =>
            {
                template.CustomTemplate(new GrayTemplate());
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
            })
            .MainTableDataSource(dataSource =>
            {
                var data = Encoding.UTF8.GetBytes("The quick brown fox jumps over the lazy dog.");
                var list = data.HexDump();
                dataSource.AnonymousTypeList(list);
            })
            .MainTableColumns(columns =>
            {
                columns.AddColumn(column =>
                {
                    column.PropertyName("Offset");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(0);
                    column.Width(0.5f);
                    column.HeaderCell("Offset");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Hex");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2.5f);
                    column.HeaderCell("Hex");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Chars");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(1f);
                    column.HeaderCell("Chars");
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public static class PrintHex
    {
        public static char ToSafeAscii(this int b)
        {
            if (b >= 32 && b <= 126)
            {
                return (char)b;
            }
            return '_';
        }

        public static IEnumerable HexDump(this byte[] data)
        {
            int bytesPerLine = 16;
            return data
                        .Select((c, i) => new { Char = c, Chunk = i / bytesPerLine })
                        .GroupBy(c => c.Chunk)
                        .Select(g =>
                                  new
                                  {
                                      Hex = g.Select(c => String.Format("{0:X2} ", c.Char)).Aggregate((s, i) => s + i),
                                      Chars = g.Select(c => ToSafeAscii(c.Char).ToString()).Aggregate((s, i) => s + i)
                                  })
                        .Select((s, i) =>
                                        new
                                        {
                                            Offset = String.Format("{0:d6}", i * bytesPerLine),
                                            Hex = s.Hex,
                                            Chars = s.Chars
                                        });
        }
    }
}