using System;
using System.Collections.Generic;
using iTextSharp.text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.FluentInterface;
using PdfRpt.Core.Helper;
using System.Linq;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class AccountingBalanceColumnPdfReport
    {
        [TestMethod]
        public void Verify_AccountingBalanceColumnPdfReport_Can_Be_Created()
        {
            var report = CreateAccountingBalanceColumnPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateAccountingBalanceColumnPdfReport()
        {
            // Balance of each row = Balance of the previous row + current row's credit - current row's debit
            long lastBalance = 0;

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
                     defaultHeader.Message(
                         "Accounting Rpt.\n" +
                         "Balance of each row = Balance of the previous row + current row's credit - current row's debit");
                 });
             })
             .MainTableTemplate(template =>
             {
                 template.BasicTemplate(BasicTemplate.SilverTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
             })
             .MainTableDataSource(dataSource =>
             {
                 var listOfRows = new List<Payment>();
                 for (int i = 1; i < 20; i++)
                 {
                     listOfRows.Add(new Payment
                     {
                         Id = i,
                         Credit = (i % 2 == 0) ? (1000 * i) : 0,
                         Debit = (i % 2 == 0) ? 0 : (2000 * i),
                         UnitPrice = 3000 + i
                     });
                 }
                 dataSource.StronglyTypedList(listOfRows);
             })
             .MainTableSummarySettings(summary =>
             {
                 summary.OverallSummarySettings("Summary");
                 summary.PreviousPageSummarySettings("Previous Page Summary");
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
                     column.PropertyName<Payment>(x => x.UnitPrice);
                     column.HeaderCell("UnitPrice ($)");
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj));
                     });
                     column.Width(2);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Payment>(x => x.Credit);
                     column.HeaderCell("Credit ($)");
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
                     column.Width(2);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Payment>(x => x.Debit);
                     column.HeaderCell("Debit ($)");
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
                     column.Width(2);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("C.F.1");
                     column.HeaderCell("Balance ($)");
                     column.Width(3);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.ColumnItemsTemplate(template =>
                     {
                         template.TextBlock();
                         template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", Math.Abs((long)obj)));
                         template.ConditionalFormatFormula(list =>
                         {
                             var currentRowCredit = (long)list.GetValueOf<Payment>(x => x.Credit);
                             if (currentRowCredit > 0)
                             {
                                 return new CellBasicProperties
                                 {
                                     FontColor = new BaseColor(System.Drawing.Color.Brown.ToArgb()),
                                     HorizontalAlignment = HorizontalAlignment.Right
                                 };
                             }
                             return new CellBasicProperties { PdfFontStyle = DocumentFontStyle.Normal };
                         });
                     });
                     column.CalculatedField(
                         list =>
                         {
                             if (list == null) return string.Empty;
                             var currentRowCredit = (long)list.GetValueOf<Payment>(x => x.Credit);
                             var currentRowDebit = (long)list.GetValueOf<Payment>(x => x.Debit);
                             lastBalance = lastBalance + currentRowCredit - currentRowDebit;
                             return lastBalance;
                         });
                     column.AggregateFunction(aggregateFunction =>
                     {
                         // just adds an empty cell for events processing
                         aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                         aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", (Math.Abs(Convert.ToInt64(obj.ToSafeString().Replace(",", string.Empty))))));
                     });
                     column.IsVisible(true);
                     column.Order(4);
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("C.F.2");
                     column.HeaderCell("Status");
                     column.Width(3);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.CalculatedField(
                         list =>
                         {
                             if (lastBalance > 0)
                                 return "Deb.";
                             else
                                 return "Cred.";
                         });
                     column.AggregateFunction(aggregateFunction =>
                         {
                             // just adds an empty cell for events processing
                             aggregateFunction.NumericAggregateFunction(AggregateFunction.Empty);
                         });
                     column.IsVisible(true);
                     column.Order(5);
                 });
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");

                 events.CellCreated(args =>
                 {
                     if (args.CellType == CellType.PreviousPageSummaryCell ||
                         args.CellType == CellType.PageSummaryCell ||
                         args.CellType == CellType.SummaryRowCell)
                     {
                         if (args.TableRowData != null &&
                             args.Cell.RowData.PropertyName == "C.F.2")
                         {
                             var summaryRowCredit = args.TableRowData.FirstOrDefault(x => x.PropertyName == "Credit");
                             var summaryRowDebit = args.TableRowData.FirstOrDefault(x => x.PropertyName == "Debit");
                             if (summaryRowCredit != null && summaryRowDebit != null)
                             {
                                 args.Cell.RowData.FormattedValue =
                                     (summaryRowCredit.PropertyValue.ToSafeDouble() - summaryRowDebit.PropertyValue.ToSafeDouble()) > 0 ? "Deb." : "Cred.";
                             }
                         }
                     }
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
