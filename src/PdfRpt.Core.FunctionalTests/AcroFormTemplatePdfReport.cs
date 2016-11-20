using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class AcroFormTemplatePdfReport
    {
        [TestMethod]
        public void Verify_AcroFormTemplatePdfReport_Can_Be_Created()
        {
            var report = CreateAcroFormTemplatePdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateAcroFormTemplatePdfReport()
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
                 fonts.Path(TestUtils.GetTahomaFontPath(),
                            TestUtils.GetVerdanaFontPath());
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
                 template.BasicTemplate(BasicTemplate.SilverTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
             })
             .MainTableDataSource(dataSource =>
             {
                 var listOfRows = new List<AcroFormModel>();
                 for (int i = 1; i <= 20; i++)
                 {
                     listOfRows.Add(new AcroFormModel
                                    {
                                        Id = i,
                                        Age = 20 + i,
                                        Name = "N" + i,
                                        LastName = "L" + i,
                                        Gender = (i % 2 == 0) ? "Male" : "Female"
                                    });
                 }
                 dataSource.StronglyTypedList(listOfRows);
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
             })
             .MainTableColumns(columns =>
             {
                 columns.AddColumn(column =>
                 {
                     column.PropertyName("rowNo");
                     column.IsRowNumber(true);
                     column.Width(1);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(0);
                     column.HeaderCell(caption: "#");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<AcroFormModel>(x => x.Id);
                     column.HeaderCell(caption: "AcroForm");
                     column.Width(6);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.ColumnItemsTemplate(template =>
                     {
                         template.PdfTemplate(
                                 pdfTemplatePath: System.IO.Path.Combine(TestUtils.GetBaseDir(), "data", "AcroFormSample.pdf"),
                                 onFillAcroForm: (data, form, pdfStamper) =>
                                 {
                                     form.GenerateAppearances = true;

                                     foreach (var item in data)
                                     {
                                         var value = item.PropertyValue == null ? string.Empty : item.PropertyValue.ToString();
                                         if (item.PropertyNameEquals<AcroFormModel>(x => x.Gender))
                                         {
                                             if (value == "Male")
                                                 form.SetField("Male", string.Empty); // "" and "Off" are valid values here

                                             if (value == "Female")
                                                 form.SetField("Female", string.Empty);
                                         }
                                         else
                                         {
                                             form.SetField(item.PropertyName, value);
                                         }
                                     }
                                 });
                     });
                 });
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }
}
