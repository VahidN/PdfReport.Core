using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class PersianRtlPdfReport
    {
        [TestMethod]
        public void Verify_PersianRtlPdfReport_Can_Be_Created()
        {
            var report = CreatePersianRtlPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreatePersianRtlPdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.RightToLeft);
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
                fonts.Path(System.IO.Path.Combine(TestUtils.GetBaseDir(), "fonts", "irsans.ttf"),
                           TestUtils.GetVerdanaFontPath());
                fonts.Size(9);
                fonts.Color(System.Drawing.Color.Black);
            })
            .PagesFooter(footer =>
            {
                footer.DefaultFooter("تاريخ: " + PersianDate.ToPersianDateTime(DateTime.Now, "/", true).FixWeakCharacters(),
                                     PdfRunDirection.RightToLeft);
            })
            .PagesHeader(header =>
            {
                header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                header.DefaultHeader(defaultHeader =>
                {
                    defaultHeader.Message("گزارش جديد ما");
                    defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
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
                for (int i = 0; i < 320; i++)
                {
                    listOfRows.Add(new User
                    {
                        Id = i + 1,
                        LastName = "نام خانوادگي " + i,
                        Name = "نام " + i,
                        Balance = i + 1000,
                        RegisterDate = DateTime.Now.AddDays(-i)
                    });
                }
                dataSource.StronglyTypedList(listOfRows);
            })
            .MainTableSummarySettings(summarySettings =>
            {
                summarySettings.PreviousPageSummarySettings("منقول از صفحه قبل");
                summarySettings.PageSummarySettings("جمع صفحه");
                summarySettings.OverallSummarySettings("جمع");
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
                    column.HeaderCell("رديف");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.Id);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("شماره");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.TextBlock();
                        template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : long.Parse(obj.ToString()).NumberToText(Language.Persian));
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.Name);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(2);
                    column.HeaderCell("نام");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.LastName);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(3);
                    column.Width(2.5f);
                    column.HeaderCell("نام خانوادگي");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.Balance);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(4);
                    column.Width(2);
                    column.HeaderCell("موجودي");
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
                    column.PropertyName<User>(x => x.RegisterDate);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(5);
                    column.Width(3);
                    column.HeaderCell("ثبت نام");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.TextBlock();
                        template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                            ? string.Empty : PersianDate.ToPersianDateTime((DateTime)obj));
                    });
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
                events.MainTableAdded(args =>
                {
                    var data = args.LastOverallAggregateValueOf<User>(y => y.Balance);
                    var msg = "جمع نهايي گزارش : " + data + "، معادل: " + long.Parse(data, NumberStyles.AllowThousands, CultureInfo.InvariantCulture).NumberToText(Language.Persian);
                    var infoTable = new PdfGrid(numColumns: 1) { WidthPercentage = 100 };
                    infoTable.AddSimpleRow(
                         (cellData, properties) =>
                         {
                             cellData.Value = msg;
                             properties.PdfFont = events.PdfFont;
                             properties.RunDirection = PdfRunDirection.RightToLeft;
                         });
                    args.PdfDoc.Add(infoTable.AddBorderToTable());
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