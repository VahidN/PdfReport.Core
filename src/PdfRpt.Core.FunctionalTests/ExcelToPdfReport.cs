using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{

    public static class ExcelData
    {
        public static void CreateSampleFile()
        {
            var newFile = new FileInfo(TestUtils.GetDataFilePath("sample.xlsx"));
            if (newFile.Exists)
            {
                return;
            }

            using (var package = new ExcelPackage(newFile))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                //Add the headers
                worksheet.Cells[1, 1].Value = "User";
                worksheet.Cells[1, 2].Value = "Path";

                //Add some items...
                worksheet.Cells["A2"].Value = "User 1";
                worksheet.Cells["B2"].Value = "/path1";

                worksheet.Cells["A3"].Value = "User 2";
                worksheet.Cells["B3"].Value = "/path2";

                worksheet.Cells["A4"].Value = "User 3";
                worksheet.Cells["B4"].Value = "/path3";

                worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells

                // set some document properties
                package.Workbook.Properties.Title = "Sample";
                package.Workbook.Properties.Author = "Vahid";
                package.Workbook.Properties.Comments = "This is a sample file.";

                package.Save();
            }
        }
    }

    public class ExcelDataReaderDataSource : IDataSource
    {
        private readonly string _filePath;
        private readonly string _worksheet;

        public ExcelDataReaderDataSource(string filePath, string worksheet)
        {
            _filePath = filePath;
            _worksheet = worksheet;
        }

        public IEnumerable<IList<CellData>> Rows()
        {
            var fileInfo = new FileInfo(_filePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"{_filePath} file not found.");
            }

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[_worksheet];
                var startCell = worksheet.Dimension.Start;
                var endCell = worksheet.Dimension.End;

                for (var row = startCell.Row + 1; row < endCell.Row + 1; row++)
                {
                    var i = 0;
                    var result = new List<CellData>();
                    for (var col = startCell.Column; col <= endCell.Column; col++)
                    {
                        var pdfCellData = new CellData
                        {
                            PropertyName = worksheet.Cells[1, col].Value.ToString(),
                            PropertyValue = worksheet.Cells[row, col].Value,
                            PropertyIndex = i++
                        };
                        result.Add(pdfCellData);
                    }
                    yield return result;
                }
            }
        }
    }

    public static class ExcelUtils
    {
        public static IList<string> GetColums(string filePath, string excelWorksheet)
        {
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"{filePath} file not found.");
            }

            var columns = new List<string>();
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[excelWorksheet];
                var startCell = worksheet.Dimension.Start;
                var endCell = worksheet.Dimension.End;

                for (int col = startCell.Column; col <= endCell.Column; col++)
                {
                    var colHeader = worksheet.Cells[1, col].Value.ToString();
                    columns.Add(colHeader);
                }
            }
            return columns;
        }
    }

    [TestClass]
    public class ExcelToPdfReport
    {
        [TestMethod]
        public void Verify_ExcelToPdfReport_Can_Be_Created()
        {
            ExcelData.CreateSampleFile();

            var report = CreateExcelToPdfReport(
                filePath: TestUtils.GetDataFilePath("sample.xlsx"),
                excelWorksheet: "Sheet1");
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateExcelToPdfReport(string filePath, string excelWorksheet)
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
                    footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
                })
                .PagesHeader(header =>
                {
                    header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                    header.DefaultHeader(defaultHeader =>
                    {
                        defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                        defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                        defaultHeader.Message("Excel To Pdf Report");
                    });
                })
                .MainTableTemplate(template =>
                {
                    template.BasicTemplate(BasicTemplate.ClassicTemplate);
                })
                .MainTablePreferences(table =>
                {
                    table.ColumnsWidthsType(TableColumnWidthType.Relative);
                    table.MultipleColumnsPerPage(new MultipleColumnsPerPage
                    {
                        ColumnsGap = 7,
                        ColumnsPerPage = 3,
                        ColumnsWidth = 170,
                        IsRightToLeft = false,
                        TopMargin = 7
                    });
                })
                .MainTableDataSource(dataSource =>
                {
                    dataSource.CustomDataSource(() => new ExcelDataReaderDataSource(filePath, excelWorksheet));
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

                    var order = 1;
                    foreach (var columnInfo in ExcelUtils.GetColums(filePath, excelWorksheet))
                    {
                        columns.AddColumn(column =>
                        {
                            column.PropertyName(columnInfo);
                            column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                            column.IsVisible(true);
                            column.Order(order++);
                            column.Width(1);
                            column.HeaderCell(columnInfo);
                        });
                    }
                })
                .MainTableEvents(events =>
                {
                    events.DataSourceIsEmpty(message: "There is no data available to display.");
                })
                .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }
}