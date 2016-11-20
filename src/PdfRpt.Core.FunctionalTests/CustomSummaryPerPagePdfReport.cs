using System;
using System.Collections.Generic;
using iTextSharp.text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class CustomSummaryPerPagePdfReport
    {
        [TestMethod]
        public void Verify_CustomSummaryPerPagePdfReport_Can_Be_Created()
        {
            var report = CreateCustomSummaryPerPagePdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateCustomSummaryPerPagePdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "CustomSummaryPerPage Rpt.", Subject = "Test Rpt", Title = "Test" });
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
                table.NumberOfDataRowsPerPage(5);
            })
            .MainTableDataSource(dataSource =>
            {
                var listOfRows = new List<User>();
                for (int i = 0; i < 50; i++)
                {
                    listOfRows.Add(new User { Id = i, LastName = "LastName " + i, Name = "Name " + i, Balance = i + 1000 });
                }
                dataSource.StronglyTypedList(listOfRows);
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
                    column.Font(font =>
                    {
                        font.Size(10);
                        font.Color(System.Drawing.Color.Brown);
                    });
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
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
                var page = 0;
                events.PageTableAdded(args =>
                {
                    page++;
                    var balanceData = args.LastOverallAggregateValueOf<User>(u => u.Balance);
                    var balance = double.Parse(balanceData, System.Globalization.NumberStyles.AllowThousands);

                    var others = Math.Round(balance * 1.8 / 100);
                    var tax = Math.Round(balance * 2.2 / 100);
                    var total = balance + tax + others;

                    var taxTable = new PdfGrid(args.Table.RelativeWidths); // Create a clone of the MainTable's structure
                    taxTable.WidthPercentage = args.Table.WidthPercentage;
                    if (page == 1)
                    {
                        taxTable.SpacingBefore = args.Table.FooterHeight;
                    }

                    taxTable.AddSimpleRow(
                        null /* null = empty cell */, null, null,
                        (data, cellProperties) =>
                        {
                            data.Value = "tax";
                            cellProperties.PdfFont = args.PdfFont;
                            cellProperties.HorizontalAlignment = HorizontalAlignment.Right;
                        },
                        (data, cellProperties) =>
                        {
                            data.Value = string.Format("{0:n0}", tax);
                            cellProperties.PdfFont = args.PdfFont;
                            cellProperties.BorderColor = BaseColor.LightGray;
                            cellProperties.ShowBorder = true;
                        });

                    taxTable.AddSimpleRow(
                        null, null, null,
                        (data, cellProperties) =>
                        {
                            data.Value = "others";
                            cellProperties.PdfFont = args.PdfFont;
                            cellProperties.HorizontalAlignment = HorizontalAlignment.Right;
                        },
                        (data, cellProperties) =>
                        {
                            data.Value = string.Format("{0:n0}", others);
                            cellProperties.PdfFont = args.PdfFont;
                            cellProperties.BorderColor = BaseColor.LightGray;
                            cellProperties.ShowBorder = true;
                        });

                    taxTable.AddSimpleRow(
                        null, null, null,
                        (data, cellProperties) =>
                        {
                            data.Value = "Total";
                            cellProperties.PdfFont = args.PdfFont;
                            cellProperties.HorizontalAlignment = HorizontalAlignment.Right;
                        },
                        (data, cellProperties) =>
                        {
                            data.Value = string.Format("{0:n0}", total);
                            cellProperties.PdfFont = args.PdfFont;
                            cellProperties.BorderColor = BaseColor.LightGray;
                            cellProperties.ShowBorder = true;
                        });

                    taxTable.AddSimpleRow(
                        null, null, null,
                        null,
                        (data, cellProperties) =>
                        {
                            data.Value = total.NumberToText(Language.English) + " $";
                            cellProperties.PdfFont = args.PdfFont;
                            cellProperties.BorderColor = BaseColor.LightGray;
                            cellProperties.ShowBorder = true;
                            cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                        });

                    args.PdfDoc.Add(taxTable);

                });
            })
            .Export(export =>
            {
                export.ToExcel();
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }
}