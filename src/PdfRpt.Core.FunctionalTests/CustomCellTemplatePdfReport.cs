using System;
using System.Collections.Generic;
using System.Dynamic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class CustomCellTemplatePdfReport
    {
        [TestMethod]
        public void Verify_CustomCellTemplatePdfReport_Can_Be_Created()
        {
            var report = CreateCustomCellTemplatePdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateCustomCellTemplatePdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "Test Rpt", Title = "Test" });
                doc.Compression(new CompressionSettings
                {
                    CompressionLevel = CompressionLevel.BestCompression,
                    EnableCompression = true
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
                 template.BasicTemplate(BasicTemplate.SnowyPineTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
                 table.MultipleColumnsPerPage(new MultipleColumnsPerPage
                 {
                     ColumnsGap = 20,
                     ColumnsPerPage = 2,
                     ColumnsWidth = 250,
                     IsRightToLeft = false,
                     TopMargin = 7
                 });
             })
             .MainTableDataSource(dataSource =>
             {
                 var usersList = new List<ExpandoObject>();

                 var rnd = new Random();
                 for (int i = 0; i < 200; i++)
                 {
                     dynamic user = new ExpandoObject();
                     user.User = $"User {i}";
                     user.Month = rnd.Next(1, 12);
                     user.Salary = rnd.Next(400, 2000);
                     usersList.Add(user);
                 }

                 dataSource.DynamicData(usersList);
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
                 events.CellCreated(args =>
                     {
                         //change the background color of the cell based on the value
                         if (args.RowType == RowType.DataTableRow && args.Cell.RowData.Value != null && args.Cell.RowData.Value is decimal)
                         {
                             if ((decimal)args.Cell.RowData.Value <= 1000)
                                 args.Cell.BasicProperties.BackgroundColor = BaseColor.Cyan;
                         }
                     });
             })
             .MainTableSummarySettings(summary =>
             {
                 summary.OverallSummarySettings("Summary");
                 summary.PreviousPageSummarySettings("Previous Col. Summary");
                 summary.PageSummarySettings("Page Summary");
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
                     column.PropertyName("User");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.Width(3);
                     column.HeaderCell("User");
                     column.ColumnItemsTemplate(t => t.CustomTemplate(new MyCustomCellTemplate()));
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Month");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                     column.Width(2);
                     column.HeaderCell("Month");
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.ConditionalFormatFormula(list =>
                         {
                             var cellValue = int.Parse(list.GetSafeStringValueOf("Month", nullValue: "0"));
                             if (cellValue == 7)
                             {
                                 return new CellBasicProperties
                                 {
                                     PdfFontStyle = DocumentFontStyle.Bold | DocumentFontStyle.Underline,
                                     FontColor = new BaseColor(System.Drawing.Color.Brown.ToArgb()),
                                     BackgroundColor = new BaseColor(System.Drawing.Color.Yellow.ToArgb())
                                 };
                             }
                             return new CellBasicProperties { PdfFontStyle = DocumentFontStyle.Normal };
                         });
                     });
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Salary");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                     column.Width(2);
                     column.HeaderCell("Salary");
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
             .Export(export =>
                 {
                     export.ToXml();
                     export.ToExcel();
                 })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public class MyCustomCellTemplate : IColumnItemsTemplate
    {
        readonly Random _rnd = new Random();

        public void CellRendered(PdfPCell cell, Rectangle position, PdfContentByte[] canvases, CellAttributes attributes)
        {
        }

        public CellBasicProperties BasicProperties { set; get; }
        public Func<IList<CellData>, CellBasicProperties> ConditionalFormatFormula { set; get; }

        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            var pdfCell = new PdfPCell();
            var table = new PdfGrid(1) { RunDirection = PdfWriter.RUN_DIRECTION_LTR };

            var filePath = System.IO.Path.Combine(TestUtils.GetBaseDir(), "Images", $"{_rnd.Next(1, 5):00}.png");
            var photo = PdfImageHelper.GetITextSharpImageFromImageFile(filePath);
            table.AddCell(new PdfPCell(photo, fit: true)
            {
                Border = 0,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            var name = attributes.RowData.TableRowData.GetSafeStringValueOf("User");
            table.AddCell(new PdfPCell(attributes.BasicProperties.PdfFont.FontSelector.Process(name))
            {
                Border = 0,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            pdfCell.AddElement(table);

            return pdfCell;
        }
    }
}