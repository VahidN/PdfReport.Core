using System;
using System.Collections.Generic;
using PdfRpt.Core.FunctionalTests.Models;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class ExpensesCrosstabPdfReport
    {
        [TestMethod]
        public void Verify_ExpensesCrosstabPdfReport_Can_Be_Created()
        {
            var report = CreateExpensesCrosstabPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateExpensesCrosstabPdfReport()
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
                     defaultHeader.Message("Our new rpt.");
                 });
             })
             .MainTableTemplate(template =>
             {
                 template.BasicTemplate(BasicTemplate.ClassicTemplate);
             })
             .MainTableSummarySettings(summarySettings =>
             {
                 summarySettings.OverallSummarySettings("Summary");
                 summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                 summarySettings.PageSummarySettings("Page Summary");
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
             })
             .MainTableDataSource(dataSource =>
             {
                 dataSource.AnonymousTypeList(ExpenseDataSource.ExpensesCrossTabList());
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
                     column.PropertyName("Year");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.Width(2);
                     column.HeaderCell("Year");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Month");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                     column.Width(3);
                     column.HeaderCell("Month");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("ComputerDepartment");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                     column.Width(3);
                     column.HeaderCell("Computer Department");
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
                     column.PropertyName("MathDepartment");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(4);
                     column.Width(3);
                     column.HeaderCell("Math Department");
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
                     column.PropertyName("PhysicsDepartment");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(5);
                     column.Width(3);
                     column.HeaderCell("Physics Department");
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
                     column.PropertyName("Total");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(6);
                     column.Width(3);
                     column.HeaderCell("Total");
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
                 export.ToXml();
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public static class ExpenseDataSource
    {
        public static IList<Expense> ExpensesDataSource()
        {
            return new List<Expense>
            {
                new Expense { Date = new DateTime(2011,11,1), Department = "Computer", Expenses = 100 },
                new Expense { Date = new DateTime(2011,11,1), Department = "Math", Expenses = 200 },
                new Expense { Date = new DateTime(2011,11,1), Department = "Physics", Expenses = 150 },

                new Expense { Date = new DateTime(2011,10,1), Department = "Computer", Expenses = 75 },
                new Expense { Date = new DateTime(2011,10,1), Department = "Math", Expenses = 150 },
                new Expense { Date = new DateTime(2011,10,1), Department = "Physics", Expenses = 130 },

                new Expense { Date = new DateTime(2011,9,1), Department = "Computer", Expenses = 90 },
                new Expense { Date = new DateTime(2011,9,1), Department = "Math", Expenses = 95 },
                new Expense { Date = new DateTime(2011,9,1), Department = "Physics", Expenses = 100 }
            };
        }

        public static System.Collections.IList ExpensesCrossTabList()
        {
            return ExpenseDataSource
                        .ExpensesDataSource()
                        .GroupBy(t =>
                                   new
                                   {
                                       Year = t.Date.Year,
                                       Month = t.Date.Month
                                   })
                        .Select(myGroup =>
                                   new
                                   {
                                       Year = myGroup.Key.Year,
                                       Month = myGroup.Key.Month,
                                       ComputerDepartment = myGroup.Where(x => x.Department == "Computer").Sum(x => x.Expenses),
                                       MathDepartment = myGroup.Where(x => x.Department == "Math").Sum(x => x.Expenses),
                                       PhysicsDepartment = myGroup.Where(x => x.Department == "Physics").Sum(x => x.Expenses),
                                       Total = myGroup.Sum(x => x.Expenses)
                                   })
                        .ToList();
        }
    }
}
