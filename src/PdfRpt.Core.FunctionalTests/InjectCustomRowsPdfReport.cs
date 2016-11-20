using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PdfRpt.Core.FunctionalTests.Models;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class InjectCustomRowsPdfReport
    {
        [TestMethod]
        public void Verify_InjectCustomRowsPdfReport_Can_Be_Created()
        {
            var report = CreateInjectCustomRowsPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateInjectCustomRowsPdfReport()
        {
            int row = 0;
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
                    defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                    defaultHeader.Message("Our new rpt.");
                    defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                });
            })
            .MainTableTemplate(template =>
            {
                template.BasicTemplate(BasicTemplate.SilverTemplate);
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                table.NumberOfDataRowsPerPage(5);
            })
            .MainTableDataSource(dataSource =>
            {
                var listOfRows = InjectCustomRowsDataSource.MyTransactions();
                dataSource.AnonymousTypeList(listOfRows);
            })
            .MainTableSummarySettings(summarySettings =>
            {
                summarySettings.OverallSummarySettings("Total");
                summarySettings.PreviousPageSummarySettings("Pervious Page Summary");
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
                    column.PropertyName("Id");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("Id");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Date");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("Date");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.TextBlock();
                        template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : ((DateTime)obj).ToString("MM/dd/yyyy"));
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Description");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(3);
                    column.Width(3);
                    column.HeaderCell("Description");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Income");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(4);
                    column.Width(2);
                    column.HeaderCell("Income");
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

                columns.AddColumn(column =>
                {
                    column.PropertyName("Payment");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(5);
                    column.Width(2);
                    column.HeaderCell("Payment");
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

                columns.AddColumn(column =>
                {
                    column.PropertyName("Sign");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(6);
                    column.Width(2);
                    column.HeaderCell("Sign");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.TextBlock();
                        template.BasicProperties(new CellBasicProperties
                        {
                            PdfFont = new GenericFontProvider(
                                TestUtils.GetWingdingFontPath(),
                                TestUtils.GetVerdanaFontPath())
                        });
                    });
                    column.CalculatedField(
                    args =>
                    {
                        var residue = args.GetValueOf("Residue", 0);
                        if ((int)residue > 0)
                            return "J"; /*:)*/
                        return "L"; /*:(*/
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("Residue");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(7);
                    column.Width(2);
                    column.HeaderCell("Residue");
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
                events.RowStartedInjectCustomRows(customRows =>
                {
                    if (row > 0)
                    {
                        return; //stop adding new rows.
                    }

                    row++;
                    customRows.AddRow(newRow =>
                    {
                        newRow.AddCell("rowNo", 1);
                        newRow.AddCell("Id", 1);
                        newRow.AddCell("Date", DateTime.Parse("3/17/2012", CultureInfo.InvariantCulture));
                        newRow.AddCell("Description", "before 3/17/2012");
                        newRow.AddCell("Income", 100);
                        newRow.AddCell("Payment", 70);
                        newRow.AddCell("Sign", "J");
                        newRow.AddCell("Residue", 30);
                    });
                });
            })
            .Export(export =>
            {
                export.ToExcel();
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public static class InjectCustomRowsDataSource
    {
        public static IEnumerable MyTransactions()
        {
            var list = createTransactions();
            return list.Select(t => new
            {
                Id = t.Id,
                Date = t.Date,
                Description = t.Description,
                Income = t.Type == TransactionType.Income ? t.SalePrice : 0,
                Payment = t.Type == TransactionType.Payment ? t.SalePrice : 0,
                Residue = t.Type == TransactionType.Income ? (int)t.SalePrice : (int)-t.SalePrice
            });
        }

        private static IList<Transaction> createTransactions()
        {
            return new[]
            {
                new Transaction
                {
                     Id = 1,
                     Date = DateTime.Now.AddDays(-10),
                     Description = "Desc 1",
                     SalePrice = 10,
                     Type = TransactionType.Income
                },
                new Transaction
                {
                     Id = 2,
                     Date = DateTime.Now.AddDays(-9),
                     Description = "Desc 2",
                     SalePrice = 2,
                     Type = TransactionType.Payment
                },
                new Transaction
                {
                     Id = 3,
                     Date = DateTime.Now.AddDays(-8),
                     Description = "Desc 3",
                     SalePrice = 5,
                     Type = TransactionType.Payment
                },
                new Transaction
                {
                     Id = 4,
                     Date = DateTime.Now.AddDays(-7),
                     Description = "Desc 4",
                     SalePrice = 7,
                     Type = TransactionType.Income
                },
                new Transaction
                {
                     Id = 5,
                     Date = DateTime.Now.AddDays(-5),
                     Description = "Desc 5",
                     SalePrice = 6,
                     Type = TransactionType.Payment
                }
            };
        }
    }
}