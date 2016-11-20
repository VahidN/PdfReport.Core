using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.FunctionalTests.OxyPlotExportUtils;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;
using Element = iTextSharp.text.Element;
using HorizontalAlignment = PdfRpt.Core.Contracts.HorizontalAlignment;
using PageOrientation = PdfRpt.Core.Contracts.PageOrientation;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class ChartImagePdfReport
    {
        [TestMethod]
        public void Verify_ChartImagePdfReport_Can_Be_Created()
        {
            var report = CreateChartImagePdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateChartImagePdfReport()
        {
            var plotModel = new PlotModel
            {
                Title = "OxyPlot Chart",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
                DefaultFont = "Tahoma",
                TitleFont = "Tahoma",
                TextColor = OxyColors.Black
            };

            var categoryAxis1 = new CategoryAxis
            {
                MinorStep = 1,
                Font = "Tahoma",
                Position = AxisPosition.Bottom,
                TextColor = OxyColors.Black,
                TitleColor = OxyColors.Black
            };
            plotModel.Axes.Add(categoryAxis1);

            var linearAxis1 = new LinearAxis
            {
                AbsoluteMinimum = 0,
                MaximumPadding = 0.06,
                MinimumPadding = 0,
                Font = "Tahoma",
                TitleFont = "Tahoma",
                TextColor = OxyColors.Black
            };
            plotModel.Axes.Add(linearAxis1);

            var columnSeries1 = new ColumnSeries
            {
                LabelFormatString = "{0}",
                LabelPlacement = LabelPlacement.Middle,
                StrokeThickness = 1,
                Title = "Series 1",
                Font = "Tahoma",
                TextColor = OxyColors.Black,
                FillColor = OxyColors.Blue,
                NegativeFillColor = OxyColors.YellowGreen,
                StrokeColor = OxyColors.DarkRed

            };
            plotModel.Series.Add(columnSeries1);

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
                 template.BasicTemplate(BasicTemplate.ClassicTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
             })
             .MainTableDataSource(dataSource =>
             {
                 var listOfRows = new List<User>();
                 for (var i = 0; i < 7; i++)
                 {
                     listOfRows.Add(new User { Id = i, LastName = "Last Name " + i, Name = "Name " + i, Balance = (i * 50) + 1000 });
                 }
                 dataSource.StronglyTypedList(listOfRows);
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
                 events.DocumentOpened(args =>
                 {

                 });
                 events.RowAdded(args =>
                 {
                     if (args.RowType == RowType.DataTableRow)
                     {
                         var name = args.TableRowData.GetSafeStringValueOf<User>(x => x.Name);
                         if (name == null) return;

                         var balance = args.TableRowData.GetValueOf<User>(x => x.Balance);
                         if (balance == null) return;

                         categoryAxis1.Labels.Add(name);
                         columnSeries1.Items.Add(new ColumnItem((long)balance) { Color = OxyColors.RosyBrown });
                     }
                 });
                 events.DocumentClosing(args =>
                 {
                     AddChartToPage(plotModel, args.PdfDoc, width: (int)args.PdfWriter.PageSize.Width - 100, height: 250);
                 });
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
                     column.Width(2);
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
                     column.HeaderCell("Balance");
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.Width(2);
                     column.AggregateFunction(aggregateFunction =>
                     {
                         aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                         aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(4);
                 });
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }


        public void AddChartToPage(PlotModel model,
                                   Document pdfDoc,
                                   int width, int height,
                                   float spacingBefore = 50,
                                   float spacingAfter = 10,
                                   float widthPercentage = 80)
        {
            using (var chartimage = new MemoryStream())
            {
                //BMP gives the best compression result
                var pngExporter = new PngExporter { Width = width, Height = height, Background = OxyColors.White };
                pngExporter.Export(model, chartimage);

                var imageBytes = chartimage.ToArray();
                var iTextSharpImage = PdfImageHelper.GetITextSharpImageFromByteArray(imageBytes);
                iTextSharpImage.Alignment = Element.ALIGN_CENTER;

                var table = new PdfGrid(1)
                {
                    WidthPercentage = widthPercentage,
                    SpacingBefore = spacingBefore,
                    SpacingAfter = spacingAfter
                };
                table.AddCell(iTextSharpImage);

                pdfDoc.Add(table);
            }
        }
    }
}
