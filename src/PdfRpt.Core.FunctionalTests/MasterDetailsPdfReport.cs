using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.CustomDataSources;
using PdfRpt.FluentInterface;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class MasterDetailsPdfReport
    {
        [TestMethod]
        public void Verify_MasterDetailsPdfReport_Can_Be_Created()
        {
            var report = CreateMasterDetailsPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateMasterDetailsPdfReport()
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
            .PagesHeader(header =>
            {
                header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                header.CustomHeader(new MasterDetailsHeaders { PdfRptFont = header.PdfFont });
            })
            .PagesFooter(footer =>
            {
                footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
            })
            .MainTableTemplate(t => t.BasicTemplate(BasicTemplate.SilverTemplate))
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                table.GroupsPreferences(new GroupsPreferences
                {
                    GroupType = GroupType.HideGroupingColumns,
                    RepeatHeaderRowPerGroup = true,
                    ShowOneGroupPerPage = false,
                    SpacingBeforeAllGroupsSummary = 5f,
                    NewGroupAvailableSpacingThreshold = 170
                });
            })
            .MainTableDataSource(dataSource =>
            {
                dataSource.CustomDataSource(() => new SqliteDataReaderDataSource(
                   connectionString:
                   $"Data Source={System.IO.Path.Combine(TestUtils.GetBaseDir(), "data", "blogs.sqlite")}",
                   sql: @"select 
                            tblParents.BirthDate as ParentBirthDate,
                            tblParents.Name as ParentName,
                            tblParents.LastName as ParentLastName,
                            tblKids.Name as KidName,
                            tblKids.BirthDate as KidBirthDate
                            from tblParents
                                left outer join tblKids
                                     on tblKids.ParentId = tblParents.Id
                            order by 
                                tblParents.Name,
                                tblParents.LastName,
                                tblKids.Name"
               ));
            })
            .MainTableColumns(columns =>
            {
                columns.AddColumn(column =>
                {
                    column.PropertyName("rowNo");
                    column.IsRowNumber(true);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(0);
                    column.Width(1);
                    column.HeaderCell("#");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ParentBirthDate");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(1);
                    column.Width(2);
                    column.HeaderCell("ParentBirthDate");
                    column.Group(
                    (val1, val2) =>
                    {
                        var date1 = DateTime.Parse(val1.ToString());
                        var date2 = DateTime.Parse(val2.ToString());
                        return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ParentName");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(2);
                    column.Width(2);
                    column.HeaderCell("ParentName");
                    column.Group(
                    (val1, val2) =>
                    {
                        return val1.ToString() == val2.ToString();
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("ParentLastName");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(3);
                    column.Width(2);
                    column.HeaderCell("ParentLastName");
                    column.Group(
                    (val1, val2) =>
                    {
                        return val1.ToString() == val2.ToString();
                    });
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("KidName");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(4);
                    column.Width(2);
                    column.HeaderCell("Child Name");
                    column.IsVisible(true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName("KidBirthDate");
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.Order(5);
                    column.Width(2);
                    column.HeaderCell("BirthDate");
                    column.IsVisible(true);
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Export(e => e.ToExcel())
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public class MasterDetailsHeaders : IPageHeader
    {
        public IPdfFont PdfRptFont { set; get; }

        public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData)
        {
            var parentName = newGroupInfo.GetSafeStringValueOf("ParentName");
            var parentLastName = newGroupInfo.GetSafeStringValueOf("ParentLastName");
            var parentBirthDate = newGroupInfo.GetSafeStringValueOf("ParentBirthDate");

            var table = new PdfGrid(relativeWidths: new[] { 1f, 5f }) { WidthPercentage = 100 };
            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Name:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = parentName;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });
            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Last Name:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = parentLastName;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });
            table.AddSimpleRow(
               (cellData, cellProperties) =>
               {
                   cellData.Value = "Birth Date:";
                   cellProperties.PdfFont = PdfRptFont;
                   cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                   cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
               },
               (cellData, cellProperties) =>
               {
                   cellData.Value = parentBirthDate;
                   cellProperties.PdfFont = PdfRptFont;
                   cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
               });
            return table.AddBorderToTable(borderColor: BaseColor.LightGray, spacingBefore: 5f);
        }

        public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
        {
            var table = new PdfGrid(numColumns: 1) { WidthPercentage = 100 };
            table.AddSimpleRow(
               (cellData, cellProperties) =>
               {
                   cellData.CellTemplate = new ImageFilePathField();
                   cellData.Value = TestUtils.GetImagePath("01.png");
                   cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
               });
            table.AddSimpleRow(
               (cellData, cellProperties) =>
               {
                   cellData.Value = "Family rpt";
                   cellProperties.PdfFont = PdfRptFont;
                   cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                   cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
               });
            return table.AddBorderToTable();
        }
    }
}