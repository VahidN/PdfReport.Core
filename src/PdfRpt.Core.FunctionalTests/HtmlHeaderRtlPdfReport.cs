using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class HtmlHeaderRtlPdfReport
    {
        [TestMethod]
        public void Verify_HtmlHeaderRtlPdfReport_Can_Be_Created()
        {
            var report = CreateHtmlHeaderRtlPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateHtmlHeaderRtlPdfReport()
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
                 fonts.Path(System.IO.Path.Combine(TestUtils.GetBaseDir(), "fonts" ,"irsans.ttf"),
                            TestUtils.GetTahomaFontPath());
                 fonts.Size(9);
                 fonts.Color(System.Drawing.Color.Black);
             })
             .PagesFooter(footer =>
             {
                 footer.HtmlFooter(rptFooter =>
                 {
                     rptFooter.PageFooterProperties(new FooterBasicProperties
                     {
                         RunDirection = PdfRunDirection.RightToLeft,
                         ShowBorder = true,
                         PdfFont = footer.PdfFont,
                         TotalPagesCountTemplateHeight = 10,
                         TotalPagesCountTemplateWidth = 50,
                         SpacingBeforeTable = 25f
                     });
                     rptFooter.AddPageFooter(pageFooter =>
                     {
                         // TotalPagesNumber is a custom image.
                         var page = string.Format("صفحه {0} از <img src='TotalPagesNumber' />", pageFooter.CurrentPageNumber);
                         var date = PersianDate.ToPersianDateTime(DateTime.Now, "/", true);
                         return string.Format(@"<table style='font-size:9pt;font-family:tahoma;'>
														<tr>
															<td width='50%' align='center'>{0}</td>
															<td width='50%' align='center'>{1}</td>
														 </tr>
												</table>", page, date);
                     });
                 });
             })
             .PagesHeader(header =>
             {
                 header.HtmlHeader(rptHeader =>
                 {
                     rptHeader.PageHeaderProperties(new HeaderBasicProperties
                     {
                         RunDirection = PdfRunDirection.RightToLeft,
                         ShowBorder = true,
                         PdfFont = header.PdfFont
                     });
                     rptHeader.AddPageHeader(pageHeader =>
                     {
                         var message = "گروه بندي كاركنان بر اساس بخش و سن";
                         var photo = TestUtils.GetImagePath("01.png");
                         var image = string.Format("<img src='{0}' />", photo);
                         var subNote = @"بدهي پيشين طبق صورتحساب شماره 454 تاريخ 1391/08/05 و كسورات قانوني طبق ماده 33 محاسبه مي گردد(255/12)";
                         //نكته: در متن فوق براي اينكه تاريخ برعكس نمايش داده نشود نياز است به نحو زير عمل شود
                         subNote = subNote.FixWeakCharacters();
                         // تنظيم فونت در اينجا فراموش نشود. اين فونت بايد در ابتداي برنامه نيز رجيستر شده باشد يا معرفي به نحو متداول
                         return string.Format(@"<table style='width: 100%;font-size:9pt;font-family:tahoma;'>
										            <tr>
											            <td align='center'>{0}</td>
										            </tr>
										            <tr>
											            <td align='center'><b>{1}</b></td>
										            </tr>
										            <tr>
											            <td align='center'>{2}</td>
										            </tr>
								                </table>", image, message, subNote);
                     });

                     rptHeader.GroupHeaderProperties(new HeaderBasicProperties
                     {
                         RunDirection = PdfRunDirection.RightToLeft,
                         ShowBorder = true,
                         SpacingBeforeTable = 10f,
                         PdfFont = header.PdfFont
                     });
                     rptHeader.AddGroupHeader(groupHeader =>
                     {
                         var data = groupHeader.NewGroupInfo;
                         var groupName = data.GetSafeStringValueOf<Employee>(x => x.Department);
                         var age = data.GetSafeStringValueOf<Employee>(x => x.Age);
                         //چند نكته:
                         //براي استفاده از چند نوع فونت
                         //در حالت استفاده از اچ تي ام ال
                         //اين فونت‌ها حتما بايد در سيستم ثبت شده باشند مانند قسمت تنظيمات پيش فرض فونت‌ها
                         //به علاوه اولين فونت در اينجا يا حاوي تمام حروف فارسي و انگليسي بايد باشد يا اگر نيست نياز است
                         //صريحا فونت انگليسي مورد نظر اعلام شود
                         //مثلا در اينجا از يك اسپن كمك گرفته شده است
                         return string.Format(@"<table style='width: 100%; font-size:9pt;font-family:tahoma;'>
												            <tr>
                                                                <td width='75%'>{0}</td>
													            <td width='25%'>بخش(<span style='font-family:tahoma;'>Department</span>):</td>
												            </tr>
												            <tr>
													            <td width='75%'>{1}</td>
													            <td width='25%'>سن:</td>
												            </tr>
								                </table>",
                                                groupName, age);
                     });
                 });
             })
             .MainTableTemplate(template =>
             {
                 template.BasicTemplate(BasicTemplate.SilverTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
                 table.GroupsPreferences(new GroupsPreferences
                 {
                     GroupType = GroupType.HideGroupingColumns,
                     RepeatHeaderRowPerGroup = true,
                     ShowOneGroupPerPage = false,
                     SpacingBeforeAllGroupsSummary = 5f,
                     NewGroupAvailableSpacingThreshold = 150
                 });
             })
             .MainTableDataSource(dataSource =>
             {
                 var listOfRows = new List<Employee>();
                 var rnd = new Random();
                 for (int i = 0; i < 170; i++)
                 {
                     listOfRows.Add(
                         new Employee
                         {
                             Age = rnd.Next(25, 35),
                             Id = i + 1000,
                             Salary = rnd.Next(1000, 4000),
                             Name = "كارمند " + i,
                             Department = "بخش " + rnd.Next(1, 3)
                         });
                 }

                 listOfRows = listOfRows.OrderBy(x => x.Department).ThenBy(x => x.Age).ToList();
                 dataSource.StronglyTypedList(listOfRows);
             })
             .MainTableSummarySettings(summarySettings =>
             {
                 summarySettings.PreviousPageSummarySettings("نقل از صفحه قبل");
                 summarySettings.OverallSummarySettings("جمع گروه");
                 summarySettings.AllGroupsSummarySettings("جمع كل گروه‌ها");
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
                     column.Width(20);
                     column.HeaderCell("#");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Employee>(x => x.Department);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.Order(1);
                     column.Width(20);
                     column.HeaderCell("بخش");
                     column.Group(
                     (val1, val2) =>
                     {
                         return val1.ToString() == val2.ToString();
                     });
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Employee>(x => x.Age);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.Order(2);
                     column.Width(20);
                     column.HeaderCell("سن");
                     column.Group(
                     (val1, val2) =>
                     {
                         return (int)val1 == (int)val2;
                     });
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Employee>(x => x.Id);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                     column.Width(20);
                     column.HeaderCell("شماره");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Employee>(x => x.Name);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(4);
                     column.Width(20);
                     column.HeaderCell("نام");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<Employee>(x => x.Salary);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(5);
                     column.Width(20);
                     column.HeaderCell("حقوق");
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
                 events.DataSourceIsEmpty(message: "ركوردي جهت نمايش يافت نشد.");
             })
             .Export(export =>
             {
                 export.ToExcel();
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }
}