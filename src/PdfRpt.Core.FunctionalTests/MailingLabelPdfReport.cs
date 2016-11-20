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
    public class MailingLabelPdfReport
    {
        [TestMethod]
        public void Verify_MailingLabelPdfReport_Can_Be_Created()
        {
            var report = CreateMailingLabelPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateMailingLabelPdfReport()
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
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                table.ShowHeaderRow(false);
                table.MultipleColumnsPerPage(new MultipleColumnsPerPage
                {
                    ColumnsGap = 7,
                    ColumnsPerPage = 3,
                    ColumnsWidth = 160,
                    IsRightToLeft = true,
                    TopMargin = 7
                });
            })
            .MainTableDataSource(dataSource =>
            {
                var listOfRows = new List<User>();
                for (int i = 0; i < 200; i++)
                {
                    listOfRows.Add(new User { Id = i, LastName = "Last Name " + i, Name = "Name " + i, Balance = i + 1000 });
                }
                dataSource.StronglyTypedList<User>(listOfRows);
            })
            .MainTableColumns(columns =>
            {
                columns.AddColumn(column =>
                {
                    column.PropertyName<User>(x => x.Name);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(0);
                    column.Width(1);
                    column.ColumnItemsTemplate(t => t.CustomTemplate(
                        new MailingLabelCellTemplate
                        {
                            BasicProperties = new CellBasicProperties { CellPadding = 16 }
                        }));
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public class MailingLabelCellTemplate : IColumnItemsTemplate
    {
        readonly Random _rnd = new Random();

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

            var fileName = $"{_rnd.Next(1, 5):00}.png";
            var photo = PdfImageHelper.GetITextSharpImageFromImageFile(TestUtils.GetImagePath(fileName));
            table.AddCell(new PdfPCell(photo) { Border = 0, MinimumHeight = photo.Height, VerticalAlignment = Element.ALIGN_BOTTOM });

            var name = attributes.RowData.TableRowData.GetSafeStringValueOf<User>(x => x.Name);
            table.AddCell(new PdfPCell(attributes.BasicProperties.PdfFont.FontSelector.Process(name)) { Border = 0 });

            var lastName = attributes.RowData.TableRowData.GetSafeStringValueOf<User>(x => x.LastName);
            table.AddCell(new PdfPCell(attributes.BasicProperties.PdfFont.FontSelector.Process(lastName)) { Border = 0 });

            pdfCell.AddElement(table);

            return pdfCell;
        }
    }
}