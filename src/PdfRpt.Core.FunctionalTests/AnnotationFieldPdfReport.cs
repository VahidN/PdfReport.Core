using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.FluentInterface;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class AnnotationFieldPdfReport
    {
        [TestMethod]
        public void Verify_AnnotationFieldPdfReport_Can_Be_Created()
        {
            var report = CreateAnnotationFieldPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateAnnotationFieldPdfReport()
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
                 template.BasicTemplate(BasicTemplate.ColorfulTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.FitToContent);
             })
             .MainTableDataSource(dataSource =>
             {
                 var result = new List<StudentGrade>();
                 var rnd = new Random();
                 for (int i = 0; i < 30; i++)
                 {
                     result.Add(new StudentGrade
                     {
                         StudentId = i + 1,
                         StudentName = $"Student {i}",
                         Grade = rnd.Next(70, 95),
                         ClassName = "ClassA",
                         Notes = "Some note ..."
                     });
                 }
                 var listOfRows = result.OrderByDescending(x => x.Grade).ToList();
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
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(0);
                     column.HeaderCell("#");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<StudentGrade>(x => x.StudentId);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.HeaderCell("Id");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<StudentGrade>(x => x.StudentName);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                     column.HeaderCell("Name");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<StudentGrade>(x => x.Grade);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                     column.HeaderCell("Grade");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<StudentGrade>(x => x.Notes);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(4);
                     column.HeaderCell("Notes");
                     column.ColumnItemsTemplate(template => template.Annotation(
                                rowData =>
                                {
                                    return new AnnotationFieldData
                                    {
                                        Icon = AnnotationIcon.Note,
                                        Text = rowData.GetSafeStringValueOf<StudentGrade>(x => x.Notes),
                                        Title = rowData.GetSafeStringValueOf<StudentGrade>(x => x.StudentName)
                                    };
                                })
                         );
                 });
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }
}