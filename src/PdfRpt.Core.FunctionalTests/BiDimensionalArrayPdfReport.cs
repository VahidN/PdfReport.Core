using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    public class BiDimensionalArrayDataSource : IDataSource
    {
        private readonly string[,] _inputTable;

        public BiDimensionalArrayDataSource(string[,] inputTable)
        {
            _inputTable = inputTable;
        }

        public IEnumerable<IList<CellData>> Rows()
        {
            for (int i = 0; i < _inputTable.GetLength(0); i++)
            {
                var result = new List<CellData>();
                for (int j = 0; j < _inputTable.GetLength(1); j++)
                {
                    var pdfCellData = new CellData
                    {
                        PropertyName = $"Col{j}",
                        PropertyValue = _inputTable[i, j],
                        PropertyIndex = j
                    };
                    result.Add(pdfCellData);
                }
                yield return result;
            }
        }
    }

    [TestClass]
    public class BiDimensionalArrayPdfReport
    {
        [TestMethod]
        public void Verify_BiDimensionalArrayPdfReport_Can_Be_Created()
        {
            string[,] table = new string[3, 3]
            {
                {"a", "b", "c"},
                {"d", "e", "f"},
                {"g", "h", "i"}
            };
            var report = CreateBiDimensionalArrayPdfReport(table);
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateBiDimensionalArrayPdfReport(string[,] inputTable)
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "2d-array Rpt.", Subject = "Test Rpt", Title = "Test" });
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
                dataSource.CustomDataSource(() => new BiDimensionalArrayDataSource(inputTable));
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

                for (int j = 0; j < inputTable.GetLength(1); j++)
                {
                    columns.AddColumn(column =>
                    {
                        column.PropertyName($"Col{j}");
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(j);
                        column.Width(2);
                        column.HeaderCell($"Col{j}");
                    });
                }
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Export(export =>
            {
                export.ToExcel();
                export.ToCsv();
                export.ToXml();
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }
}