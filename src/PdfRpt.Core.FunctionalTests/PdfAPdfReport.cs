using System;
using System.Collections;
using System.Collections.Generic;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.FunctionalTests.Templates;
using PdfRpt.DataSources;
using PdfRpt.FluentInterface;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PdfRpt.Core.FunctionalTests
{
    // PDF/A:
    // Encryption is not allowed.
    // Embedded files are not allowed.
    // All fonts must be embedded.
    // Transparent images are forbidden.
    [TestClass]
    public class PdfAPdfReport
    {
        [TestMethod]
        public void Verify_PdfAPdfReport_Can_Be_Created()
        {
            var report = CreatePdfAPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreatePdfAPdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "Test Rpt", Title = "Test" });
                doc.ConformanceLevel(PdfXConformance.PDFX32002);
            })
            .DefaultFonts(fonts =>
            {
                fonts.Path(TestUtils.GetVerdanaFontPath(),
                            TestUtils.GetTahomaFontPath());
                fonts.Size(9);
                fonts.Color(System.Drawing.Color.Black);
            })
            .PagesHeader(header =>
            {
                header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                header.DefaultHeader(h =>
                {
                    h.Message("Our new Rpt");
                    h.ImagePath(TestUtils.GetImagePath("05.png"));
                });
            })
            .PagesFooter(footer =>
            {
                footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
            })
            .MainTableTemplate(t => t.CustomTemplate(new TransparentTemplate()))
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.FitToContent);
            })
            .MainTableDataSource(dataSource =>
            {
                dataSource.Crosstab(TransactionsDataSource.PivotTransactionsList());
            })
            .MainTableSummarySettings(summarySettings =>
            {
                summarySettings.OverallSummarySettings("Grand Total");
                summarySettings.PreviousPageSummarySettings("PerviousPage Summary");
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
                    column.HeaderCell("#"); //------- Main Header Row
                    column.AddHeadingCell(string.Empty); //------- Extra Header Row - 1
                });

                addColumn(columns, propertyName: "SalesPerson", caption: "Sales Person", headingCaption: string.Empty, mergeHeaderCell: false, order: 1, showTotal: false);

                addColumn(columns, "Corolla SalePrice", "SalePrice", "Corolla", true, 2);
                addColumn(columns, "Corolla Count", "Count", string.Empty, false, 3);

                addColumn(columns, "Camry SalePrice", "SalePrice", "Camry", true, 4);
                addColumn(columns, "Camry Count", "Count", string.Empty, false, 5);

                addColumn(columns, "Prius SalePrice", "SalePrice", "Prius", true, 6);
                addColumn(columns, "Prius Count", "Count", string.Empty, false, 7);

                addColumn(columns, "SalePrice", "SalePrice", "Total", true, 8);
                addColumn(columns, "Count", "Count", string.Empty, false, 9);

            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }

        private static void addColumn(MainTableColumnsBuilder columns, string propertyName, string caption, string headingCaption, bool mergeHeaderCell, int order, bool showTotal = true)
        {
            columns.AddColumn(column =>
            {
                column.PropertyName(propertyName);
                column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                column.IsVisible(true);
                column.Order(order);
                column.HeaderCell(caption); //------- Main Header Row
                column.AddHeadingCell(headingCaption, mergeHeaderCell: mergeHeaderCell); //------- Extra Header Row
                if (showTotal)
                {
                    column.AggregateFunction(aggregateFunction =>
                    {
                        aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                        aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj));
                    });
                }
                column.ColumnItemsTemplate(template =>
                {
                    template.TextBlock();
                    template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : string.Format("{0:n0}", obj));
                });
            });
        }
    }

    public static class TransactionsDataSource
    {
        public static IEnumerable PivotTransactionsList()
        {
            return createTransactionsList()
                      .Pivot(
                            x => new
                            {
                                SalesPerson = x.SalesPerson
                            },
                            x1 => x1.Product + " SalePrice",
                            x2 => x2.Sum(x4 => x4.SalePrice),
                            x8 => x8.Product + " Count",
                            x7 => x7.Count(),
                            x3 => new
                            {
                                Count = x3.Count(),
                                SalePrice = x3.Sum(x6 => x6.SalePrice)
                            });
        }

        private static IList<Transaction> createTransactionsList()
        {
            return new List<Transaction>
            {
                  new Transaction(new DateTime(2011, 11, 28), "Chris", "Corolla", 4000F),
                  new Transaction(new DateTime(2011, 11, 29), "Brian", "Prius", 2000F),
                  new Transaction(new DateTime(2011, 11, 30), "Chris", "Camry", 5000F),
                  new Transaction(new DateTime(2011, 11, 30), "Jason", "Corolla", 1000F),
                  new Transaction(new DateTime(2011, 12, 1),  "Brian", "Camry",  4000F),
                  new Transaction(new DateTime(2011, 12, 1),  "Chris", "Camry", 2000F),
                  new Transaction(new DateTime(2011, 12, 2),  "Chris", "Corolla", 2500F),
                  new Transaction(new DateTime(2011, 12, 3),  "Jason", "Camry", 1100F),
                  new Transaction(new DateTime(2011, 12, 4),  "Brian", "Corolla", 1200F),
                  new Transaction(new DateTime(2011, 12, 4),  "Jason", "Prius", 1700F),
                  new Transaction(new DateTime(2011, 12, 5),  "Brian", "Corolla", 900F),
                  new Transaction(new DateTime(2011, 12, 6),  "Jason", "Prius", 1300F),
                  new Transaction(new DateTime(2011, 12, 7),  "Chris", "Prius", 2000F)
            };
        }
    }
}
