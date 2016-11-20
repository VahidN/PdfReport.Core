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
	public class HtmlHeaderPdfReport
	{
        [TestMethod]
        public void Verify_HtmlHeaderPdfReport_Can_Be_Created()
        {
            var report = CreateHtmlHeaderPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateHtmlHeaderPdfReport()
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
				 footer.HtmlFooter(rptFooter =>
				 {
					 rptFooter.PageFooterProperties(new FooterBasicProperties
					 {
						 RunDirection = PdfRunDirection.LeftToRight,
						 ShowBorder = true,
						 PdfFont = footer.PdfFont,
						 TotalPagesCountTemplateHeight = 10,
						 TotalPagesCountTemplateWidth = 50
					 });
					 rptFooter.AddPageFooter(pageFooter =>
					 {
						 // TotalPagesNumber is a custom image.
						 var page = string.Format("Page {0} Of <img src='TotalPagesNumber' />", pageFooter.CurrentPageNumber);
						 var date = DateTime.Now.ToString("MM/dd/yyyy");
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
						 RunDirection = PdfRunDirection.LeftToRight,
						 ShowBorder = true,
                         PdfFont = header.PdfFont
                     });
					 rptHeader.AddPageHeader(pageHeader =>
					 {
						 var message = "Grouping employees by department and age.";
						 var photo = TestUtils.GetImagePath("01.png");
						 var image = string.Format("<img src='{0}' />", photo);
						 return string.Format(@"<table style='width: 100%;font-size:9pt;font-family:tahoma;'>
													<tr>
														<td align='center'>{0}</td>
													</tr>
													<tr>
														<td align='center'>{1}</td>
													</tr>
												</table>", image, message);
					 });

					 rptHeader.GroupHeaderProperties(new HeaderBasicProperties
					 {
						 RunDirection = PdfRunDirection.LeftToRight,
						 ShowBorder = true,
						 SpacingBeforeTable = 10f,
                         PdfFont = header.PdfFont
                     });
					 rptHeader.AddGroupHeader(groupHeader =>
					 {
						 var data = groupHeader.NewGroupInfo;
						 var groupName = data.GetSafeStringValueOf<Employee>(x => x.Department);
						 var age = data.GetSafeStringValueOf<Employee>(x => x.Age);
						 return string.Format(@"<table style='width: 100%; font-size:9pt;font-family:tahoma;'>
															<tr>
																<td style='width:25%;border-bottom-width:0.2; border-bottom-color:red;border-bottom-style:solid'>Department:</td>
																<td style='width:75%'>{0}</td>
															</tr>
															<tr>
																<td style='width:25%'>Age:</td>
																<td style='width:75%'>{1}</td>
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
							 Name = "Employee " + i,
							 Department = "Department " + rnd.Next(1, 3)
						 });
				 }

				 listOfRows = listOfRows.OrderBy(x => x.Department).ThenBy(x => x.Age).ToList();
				 dataSource.StronglyTypedList(listOfRows);
			 })
			 .MainTableSummarySettings(summarySettings =>
			 {
				 summarySettings.PreviousPageSummarySettings("Cont.");
				 summarySettings.OverallSummarySettings("Sum");
				 summarySettings.AllGroupsSummarySettings("Groups Sum");
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
					 column.HeaderCell("Department");
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
					 column.HeaderCell("Age");
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
					 column.HeaderCell("Id");
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<Employee>(x => x.Name);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.IsVisible(true);
					 column.Order(4);
					 column.Width(20);
					 column.HeaderCell("Name");
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<Employee>(x => x.Salary);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.IsVisible(true);
					 column.Order(5);
					 column.Width(20);
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
			 .MainTableEvents(events =>
			 {
				 events.DataSourceIsEmpty(message: "There is no data available to display.");
			 })
			 .Export(export =>
			 {
				 export.ToExcel();
			 })
			 .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
		}
	}
}
