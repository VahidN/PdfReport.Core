using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class PersianFontsListToPdfReport
    {
        [TestMethod]
        public void Verify_PersianFontsListToPdfReport_Can_Be_Created()
        {
            var report = CreatePersianFontsListToPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreatePersianFontsListToPdfReport()
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
                 footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
             })
             .PagesHeader(header =>
             {
                 header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                 header.DefaultHeader(defaultHeader =>
                 {
                     defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                     defaultHeader.Message("Installed 'B ' fonts list");
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
                 var listOfRows = new List<FontSample>();

                 // Register all the fonts of a directory
                 var fontsDir = System.IO.Path.Combine(TestUtils.GetBaseDir(), "fonts");
                 FontFactory.RegisterDirectory(fontsDir);

                 // Enumerate the current set of system fonts
                 foreach (string fontName in FontFactory.RegisteredFonts)
                 {
                     if (!fontName.ToLowerInvariant().StartsWith("b ")) continue;

                     listOfRows.Add(new FontSample
                     {
                         FontName = fontName,
                         EnglishTextSample = "Sample Text 1,2,3",
                         PersianTextSample = "نمونه متن 1,2,3"
                     });
                 }
                 dataSource.StronglyTypedList<FontSample>(listOfRows);
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
                     column.HeaderCell("#");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<FontSample>(x => x.FontName);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.Width(2);
                     column.HeaderCell("Font name");
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<FontSample>(x => x.EnglishTextSample);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(2);
                     column.Width(3);
                     column.HeaderCell("Sample Text");
                     column.ColumnItemsTemplate(t => t.CustomTemplate(new FontsListCellTemplate(20)));
                 });

                 columns.AddColumn(column =>
                 {
                     column.PropertyName<FontSample>(x => x.PersianTextSample);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(3);
                     column.Width(3);
                     column.HeaderCell("نمونه متن");
                     column.ColumnItemsTemplate(t => t.CustomTemplate(new FontsListCellTemplate(20)));
                 });
             })
             .MainTableEvents(events =>
             {
                 events.DataSourceIsEmpty(message: "There is no data available to display.");
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public class FontsListCellTemplate : IColumnItemsTemplate
    {
        readonly float _fontSize;
        public FontsListCellTemplate(float fontSize)
        {
            _fontSize = fontSize;
        }

        /// <summary>
        /// This method is called at the end of the cell's rendering.
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases">An array of PdfContentByte to add text or graphics</param>
        /// <param name="attributes">Current cell's custom attributes</param>
        public void CellRendered(PdfPCell cell, Rectangle position, PdfContentByte[] canvases, CellAttributes attributes)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public CellBasicProperties BasicProperties { set; get; }

        /// <summary>
        /// Defines the current cell's properties, based on the other cells values.
        /// Here IList contains actual row's cells values.
        /// It can be null.
        /// </summary>
        public Func<IList<CellData>, CellBasicProperties> ConditionalFormatFormula { set; get; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public PdfPCell RenderingCell(CellAttributes attributes)
        {
            return new PdfPCell(new Phrase(attributes.RowData.Value.ToSafeString(), getCellCurrentFont(attributes)));
        }

        iTextSharp.text.Font getCellCurrentFont(CellAttributes attributes)
        {
            var fontName = attributes.RowData.TableRowData.GetSafeStringValueOf<FontSample>(x => x.FontName);
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, true, _fontSize, (int)attributes.BasicProperties.PdfFont.Style, attributes.BasicProperties.PdfFont.Color);
        }
    }
}