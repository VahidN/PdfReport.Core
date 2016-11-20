using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Aggregates.Numbers;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.DataAnnotations;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class DataAnnotationsPdfReport
    {
        [TestMethod]
        public void Verify_DataAnnotationsPdfReport_Can_Be_Created()
        {
            var report = CreateDataAnnotationsPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateDataAnnotationsPdfReport()
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
                     defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                     defaultHeader.Message("new rpt.");
                     defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                 });
             })
             .MainTableTemplate(template =>
             {
                 template.BasicTemplate(BasicTemplate.ClassicTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.FitToContent);
             })
             .MainTableDataSource(dataSource =>
             {
                 dataSource.StronglyTypedList(PersonnelDataSource.CreatePersonnelList());
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
             })
             .MainTableSummarySettings(summary =>
             {
                 summary.OverallSummarySettings("Total");
                 summary.PageSummarySettings("Page Summary");
                 summary.PreviousPageSummarySettings("Pervious Page Summary");
             })
             .MainTableAdHocColumnsConventions(adHocColumns =>
             {
                 adHocColumns.ShowRowNumberColumn(true);
                 adHocColumns.RowNumberColumnCaption("#");
             })
             .Export(export =>
             {
                 export.ToExcel();
                 export.ToXml();
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public class PersonInfo
    {
        [IsVisible(false)]
        public int Id { get; set; }

        [DisplayName("User name")]
        //Note: If you don't specify the ColumnItemsTemplate, a new TextBlockField() will be used automatically.
        [ColumnItemsTemplate(typeof(TextBlockField))]
        public string Name { get; set; }

        [DisplayName("Job title")]
        public JobTitle JobTitle { set; get; }

        [DisplayName("Date of birth")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Date of death")]
        [DisplayFormat(NullDisplayText = "-", DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateOfDeath { get; set; }

        [DisplayFormat(DataFormatString = "{0:n0}")]
        [CustomAggregateFunction(typeof(Sum))]
        public int Salary { get; set; }

        [IsCalculatedField(true)]
        [DisplayName("Calculated Field")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        [AggregateFunction(AggregateFunction.Sum)]
        public string CalculatedField { get; set; }

        [CalculatedFieldFormula("CalculatedField")]
        public static Func<IList<CellData>, object> CalculatedFieldFormula =
                                                    list =>
                                                    {
                                                        if (list == null) return string.Empty;
                                                        var salary = (int)list.GetValueOf<PersonInfo>(x => x.Salary);
                                                        return salary * 0.8;
                                                    };//Note: It's a static field, not a property.

        //and for .... [IncludeInGrouping(true)]
        /*[IncludedGroupFieldEqualityComparer("CalculatedField")]
        public static Func<object, object, bool> IncludedGroupFieldEqualityComparer =
                                                    (val1, val2) =>
                                                    {
                                                        if (val1 == null && val2 == null) return true;
                                                        if (val1 == null || val2 == null) return false;
                                                        return val1.ToString() == val2.ToString();
                                                    };//Note: It's a static field, not a property.*/
    }

    public static class PersonnelDataSource
    {
        public static IList<PersonInfo> CreatePersonnelList()
        {
            return new List<PersonInfo>
            {
                new PersonInfo
                {
                    Id = 1,
                    Name = "Edward",
                    DateOfBirth = new DateTime(1900, 1, 1),
                    DateOfDeath = new DateTime(1990, 10, 15),
                    JobTitle = JobTitle.ChiefInformationOfficer,
                    Salary = 5000
                },
                new PersonInfo
                {
                    Id = 2,
                    Name = "Margaret",
                    DateOfBirth = new DateTime(1950, 2, 9),
                    DateOfDeath = null,
                    JobTitle = JobTitle.AnalystProgrammer,
                    Salary = 4000
                },
                new PersonInfo
                {
                    Id = 3,
                    Name = "Grant",
                    DateOfBirth = new DateTime(1975, 6, 13),
                    DateOfDeath = null,
                    JobTitle = JobTitle.Programmer,
                    Salary = 3500
                }
            };
        }
    }
}