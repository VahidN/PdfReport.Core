using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
	public class StackedPropertiesPdfReport
	{
        [TestMethod]
        public void Verify_StackedPropertiesPdfReport_Can_Be_Created()
        {
            var report = CreateStackedPropertiesPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateStackedPropertiesPdfReport()
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
					 defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
					 defaultHeader.Message("Our new Rpt");
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
				 var listOfRows = new List<Shipping>();
				 for (int i = 1; i <= 100; i++)
				 {
					 listOfRows.Add(new Shipping
					 {
						 Type = "Type " + i,
						 Number = i,
						 OrderNumber = i + 1000,
						 Name = "Name " + i,
						 Quantity = i * 2,
						 Weight = i + 50,
						 Destination = "Destination " + i,
						 ClearanceDate = DateTime.Now.AddDays(i),
						 Description = "Description " + i
					 });
				 }
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
					 column.Width(1);
					 column.HeaderCell("#");//------- Main Header Row
					 column.AddHeadingCell(string.Empty, mergeHeaderCell: false);//------- Extra Header Row - 1
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName("col2");
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.IsVisible(true);
					 column.Order(1);
					 column.Width(2);
					 column.HeaderCell("Number");//------- Main Header Row
					 column.AddHeadingCell("Type", mergeHeaderCell: false);//------- Extra Header Row - 1
					 column.ColumnItemsTemplate(itemsTemplate =>
						 {
							 itemsTemplate.Html();
						 });
					 column.CalculatedField(list =>
					 {
						 var type = list.GetSafeStringValueOf<Shipping>(x => x.Type);
						 var number = list.GetSafeStringValueOf<Shipping>(x => x.Number);
						 return
								@"<table style='width: 100%; font-size:9pt;'>
												<tr>
													<td align='center'>" + type + @"</td>
												</tr>
												<tr>
													<td align='center'>" + number + @"</td>
												</tr>
									   </table>
									 ";
					 });
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName("col3");
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.IsVisible(true);
					 column.Order(2);
					 column.Width(2);
					 column.HeaderCell("Name");//------- Main Header Row
					 column.AddHeadingCell("Order Number", mergeHeaderCell: false);//------- Extra Header Row - 1
					 column.ColumnItemsTemplate(itemsTemplate =>
					 {
						 itemsTemplate.Html();
					 });
					 column.CalculatedField(list =>
					 {
						 var name = list.GetSafeStringValueOf<Shipping>(x => x.Name);
						 var orderNumber = list.GetSafeStringValueOf<Shipping>(x => x.OrderNumber);
						 return
								@"<table style='width: 100%; font-size:9pt;'>
												<tr>
													<td align='center'>" + orderNumber + @"</td>
												</tr>
												<tr>
													<td align='center'>" + name + @"</td>
												</tr>
									   </table>
									 ";
					 });
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName("col4");
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.IsVisible(true);
					 column.Order(3);
					 column.Width(2);
					 column.HeaderCell("Weight");//------- Main Header Row
					 column.AddHeadingCell("Quantity", mergeHeaderCell: false);//------- Extra Header Row - 1
					 column.ColumnItemsTemplate(itemsTemplate =>
					 {
						 itemsTemplate.Html();
					 });
					 column.CalculatedField(list =>
					 {
						 var weight = list.GetSafeStringValueOf<Shipping>(x => x.Weight);
						 var quantity = list.GetSafeStringValueOf<Shipping>(x => x.Quantity);
						 return
								@"<table style='width: 100%; font-size:9pt;'>
												<tr>
													<td align='center'>" + quantity + @"</td>
												</tr>
												<tr>
													<td align='center'>" + weight + @"</td>
												</tr>
									   </table>
									 ";
					 });
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName("col5");
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.IsVisible(true);
					 column.Order(4);
					 column.Width(2);
					 column.HeaderCell("Clearance Date");//------- Main Header Row
					 column.AddHeadingCell("Destination", mergeHeaderCell: false);//------- Extra Header Row - 1
					 column.ColumnItemsTemplate(itemsTemplate =>
					 {
						 itemsTemplate.Html();
					 });
					 column.CalculatedField(list =>
					 {
						 var clearanceDate = list.GetSafeStringValueOf<Shipping>(x => x.ClearanceDate);
						 var destination = list.GetSafeStringValueOf<Shipping>(x => x.Destination);
						 return
								@"<table style='width: 100%; font-size:9pt;'>
												<tr>
													<td align='center'>" + destination + @"</td>
												</tr>
												<tr>
													<td align='center'>" + clearanceDate + @"</td>
												</tr>
									   </table>
									 ";
					 });
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<Shipping>(x => x.Description);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.IsVisible(true);
					 column.Order(5);
					 column.Width(3);
					 column.HeaderCell("Description");//------- Main Header Row
					 column.AddHeadingCell(string.Empty, mergeHeaderCell: false);//------- Extra Header Row - 1
				 });
			 })
			 .MainTableEvents(events =>
			 {
				 events.DataSourceIsEmpty(message: "There is no data available to display.");
			 })
			 .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
		}
	}
}