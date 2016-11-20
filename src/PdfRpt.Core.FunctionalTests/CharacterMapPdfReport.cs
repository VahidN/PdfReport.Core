using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.FluentInterface;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class CharacterMapPdfReport
    {
        [TestMethod]
        public void Verify_CharacterMapPdfReport_Can_Be_Created()
        {
            var report = CreateCharacterMapPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateCharacterMapPdfReport()
        {
            var fontFamilyName = "Wingdings";
            var fontProvider = new GenericFontProvider(
                                TestUtils.GetWingdingFontPath(),
                                TestUtils.GetVerdanaFontPath())
            {
                Size = 17,
                Color = new BaseColor(System.Drawing.Color.DarkRed.ToArgb())
            };

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
                    defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                    defaultHeader.Message("Characters Map");
                });
            })
            .MainTableTemplate(template =>
            {
                template.BasicTemplate(BasicTemplate.SilverTemplate);
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                table.MainTableType(TableType.HorizontalStackPanel);
                table.HorizontalStackPanelPreferences(columnsPerRow: 10);
            })
            .MainTableDataSource(dataSource =>
            {
                var listOfRows = new List<CharacterInfo>();
                for (var index = 0x20; index <= 0xFF; index++)
                {
                    listOfRows.Add(new CharacterInfo
                    {
                        Character = Convert.ToChar(index),
                        CharacterCode = string.Format("0x{0:X}", index)
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
                    column.HeaderCell("#");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<CharacterInfo>(x => x.Character);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(1);
                    column.HeaderCell(fontFamilyName + " Characters", mergeHeaderCell: true);
                    column.ColumnItemsTemplate(itemsTemplate =>
                        {
                            itemsTemplate.CustomTemplate(new CellTemplate(fontProvider));
                        });
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public class CellTemplate : IColumnItemsTemplate
    {
        readonly IPdfFont _customFont;
        public CellTemplate(IPdfFont customFont)
        {
            _customFont = customFont;
        }

        /// <summary>
        /// This method is called at the end of the cell's rendering.
        /// </summary>
        /// <param name="cell">The current cell</param>
        /// <param name="position">The coordinates of the cell</param>
        /// <param name="canvases"></param>
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
            var pdfCell = new PdfPCell();
            var table = new PdfGrid(1) { RunDirection = PdfWriter.RUN_DIRECTION_LTR };

            // Please note that All columns and properties of an object will create a single cell here.

            var idx = attributes.RowData.ColumnNumber;
            var data = attributes.RowData.TableRowData;

            var character = data.GetSafeStringValueOf<CharacterInfo>(x => x.Character, propertyIndex: idx);
            table.AddCell(new PdfPCell(_customFont.FontSelector.Process(character)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });

            var characterCode = data.GetSafeStringValueOf<CharacterInfo>(x => x.CharacterCode, propertyIndex: idx);
            table.AddCell(new PdfPCell(attributes.BasicProperties.PdfFont.FontSelector.Process(characterCode)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER });

            pdfCell.AddElement(table);

            return pdfCell;
        }
    }
}
