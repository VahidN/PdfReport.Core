using System;
using System.Collections;
using System.Collections.Generic;
using PdfRpt.Core.FunctionalTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Templates;
using PdfRpt.FluentInterface;
using PdfRpt.DataSources;
using System.Linq;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class ExtraHeadingCellsPdfReport
    {
        [TestMethod]
        public void Verify_ExtraHeadingCellsPdfReport_Can_Be_Created()
        {
            var report = CreateExtraHeadingCellsPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateExtraHeadingCellsPdfReport()
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
                 template.CustomTemplate(new TransparentTemplate());
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.FitToContent);
             })
             .MainTableDataSource(dataSource =>
             {
                 dataSource.Crosstab(DataGenerator.ContactsList(), topFieldsAreVariableInEachRow: true);
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
                     column.HeaderCell("#");//------- Main Header Row
                     column.AddHeadingCell("Contacts List", mergeHeaderCell: true);//------- Extra Header Row - 1
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: false);//------- Extra Header Row - 2
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Id");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.HeaderCell("Id");//------- Main Header Row
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 1
                     column.AddHeadingCell("Person", mergeHeaderCell: true);//------- Extra Header Row - 2
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("FirstName");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                     column.HeaderCell("Name");//------- Main Header Row
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 1
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 2
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("LastName");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                     column.HeaderCell("Last Name");//------- Main Header Row
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 1
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: false);//------- Extra Header Row - 2
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Home");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(4);
                     column.HeaderCell("Home");//------- Main Header Row
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 1
                     column.AddHeadingCell("Phones", mergeHeaderCell: true);//------- Extra Header Row - 2
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Office");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(5);
                     column.HeaderCell("Office");//------- Main Header Row
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 1
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 2
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Cell");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(6);
                     column.HeaderCell("Cell");//------- Main Header Row
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 1
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 2
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName("Fax");
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(7);
                     column.HeaderCell("Fax");//------- Main Header Row
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 1
                     column.AddHeadingCell(string.Empty, mergeHeaderCell: true);//------- Extra Header Row - 2
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

    public static class DataGenerator
    {
        public static IEnumerable ContactsList()
        {
            return createContacts().flattenContacts()
                        .Pivot(
                            x =>
                               new
                               {
                                   x.Id,
                                   x.FirstName,
                                   x.LastName
                               },
                            x1 => x1.PhoneType,
                            persons => string.Concat("(", persons.First().AreaCode, ") ", persons.First().PhoneNumber),
                            x3 => new { Count = x3.Count() });
        }

        private static IEnumerable<FlatContact> flattenContacts(this IList<Person> source)
        {
            foreach (var person in source)
                foreach (var phone in person.Phones)
                    yield return new FlatContact
                    {
                        Id = person.Id,
                        FirstName = person.Name,
                        LastName = person.LastName,
                        PhoneType = phone.PhoneType,
                        PhoneNumber = phone.PhoneNumber,
                        AreaCode = phone.AreaCode
                    };
        }

        private static IList<Person> createContacts()
        {
            return new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "John",
                    LastName = "Doe",
                    Phones = new List<Phone>
                                 {
                                     new Phone(PhoneType.Home,   "305", "555-1111"),
                                     new Phone(PhoneType.Office, "305", "555-2222"),
                                     new Phone(PhoneType.Cell,   "305", "555-3333")
                                 }
                },
                new Person
                {
                    Id = 2,
                    Name = "Jane",
                    LastName = "Doe",
                    Phones = new List<Phone>
                                 {
                                     new Phone(PhoneType.Home,   "305", "555-1111"),
                                     new Phone(PhoneType.Office, "305", "555-4444"),
                                     new Phone(PhoneType.Cell,   "305", "555-5555")
                                 }
                },
                new Person
                {
                    Id = 3,
                    Name = "Jerome",
                    LastName = "Doe",
                    Phones = new List<Phone>
                                 {
                                     new Phone(PhoneType.Home,   "305", "555-6666"),
                                     new Phone(PhoneType.Office, "305", "555-2222"),
                                     new Phone(PhoneType.Cell,   "305", "555-7777")
                                 }
                },
                new Person
                {
                    Id = 4,
                    Name = "Joel",
                    LastName = "Smith",
                    Phones = new List<Phone>
                                 {
                                     new Phone(PhoneType.Fax,    "305", "555-6666"),
                                     new Phone(PhoneType.Cell,   "305", "555-7777")
                                 }
                }
            };
        }
    }
}