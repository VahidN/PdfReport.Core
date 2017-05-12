using System;
using System.Collections.Generic;
using System.IO;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.SampleWebApp.Models;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.SampleWebApp.Reports
{
    public static class InMemoryPdfReport
    {
        public static byte[] CreateInMemoryPdfReport(string wwwroot)
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
                    fonts.Path(Path.Combine(wwwroot, "fonts", "verdana.ttf"),
                        Path.Combine(wwwroot, "fonts", "tahoma.ttf"));
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
                        defaultHeader.ImagePath(Path.Combine(wwwroot, "images", "01.png"));
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
                    var listOfRows = new List<User>();
                    for (int i = 0; i < 40; i++)
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
                })
                .Export(export =>
                {
                    export.ToExcel();
                })
                .GenerateAsByteArray(); // creating an in-memory PDF file
        }
    }
}