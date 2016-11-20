using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class NestedPropertiesPdfReport
    {
        [TestMethod]
        public void Verify_NestedPropertiesPdfReport_Can_Be_Created()
        {
            var report = CreateNestedPropertiesPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateNestedPropertiesPdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "NestedProperties Rpt.", Subject = "Test Rpt", Title = "Test" });
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
                template.BasicTemplate(BasicTemplate.SilverTemplate);
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
            })
            .MainTableDataSource(dataSource =>
            {
                var listOfRows = SessionsDataSource.CreateSessions();
                dataSource.StronglyTypedList(listOfRows);
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
                    column.Width(0.5f);
                    column.HeaderCell("#");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<WeekClassSessions>(x => x.ClassName);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("Class Name", horizontalAlignment: HorizontalAlignment.Left);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<WeekClassSessions>(x => x.IsSelected);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(2);
                    column.HeaderCell("Is Selected", horizontalAlignment: HorizontalAlignment.Left);
                    column.ColumnItemsTemplate(template =>
                    {
                        template.Checkmark();
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<WeekClassSessions>(x => x.WD0.Percent);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(3);
                    column.Width(3);
                    column.HeaderCell("Sunday", horizontalAlignment: HorizontalAlignment.Left);
                    column.ColumnItemsTemplate(template =>
                    {
                        template.ProgressBar(list =>
                        {
                            var isSelected = (bool)list.GetValueOf<WeekClassSessions>(x => x.IsSelected);
                            return isSelected ? Color.SkyBlue : Color.SlateBlue;
                        }, showPercentText: true);
                        template.DisplayFormatFormula(obj =>
                        {
                            if (obj == null) return "% 0";
                            return "% " + obj.ToString();
                        });
                    });
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }


    public static class SessionsDataSource
    {
        public static IList<WeekClassSessions> CreateSessions()
        {
            var dataSource = new List<WeekClassSessions>();
            for (int i = 0; i < 10; i++)
            {
                for (int w = 0; w < 17; w++)
                {
                    var row = new WeekClassSessions
                    {
                        ClassName = "Class " + i,
                        IsSelected = w % 2 == 0,
                        WeekNumber = w + 1,
                        WeekTitle = "Week " + (w + 1),
                        WD0 = createCell(w + 1, 0),
                        WD1 = createCell(w + 1, 1),
                        WD2 = createCell(w + 1, 2),
                        WD3 = createCell(w + 1, 3),
                        WD4 = createCell(w + 1, 4),
                        WD5 = createCell(w + 1, 5),
                        WD6 = createCell(w + 1, 6)
                    };
                    dataSource.Add(row);
                }
            }
            return dataSource;
        }

        private static ClassSession createCell(int weekNumber, int day)
        {
            int minuteSum = new Random().Next(45, 120);
            return new ClassSession
            {
                DayNumber = day,
                HasSession = true,
                Percent = (minuteSum * 100) / 720,
                Date = DateTime.Now.AddDays(day),
                WeekNumber = weekNumber + 1
            };
        }
    }
}