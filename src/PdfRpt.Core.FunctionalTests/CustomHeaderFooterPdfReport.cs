using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class CustomHeaderFooterPdfReport
    {
        readonly CustomHeader _customHeader = new CustomHeader();

        [TestMethod]
        public void Verify_CustomHeaderFooterPdfReport_Can_Be_Created()
        {
            var report = CreateCustomHeaderFooterPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateCustomHeaderFooterPdfReport()
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
                footer.CustomFooter(new CustomFooter(footer.PdfFont, PdfRunDirection.LeftToRight));
            })
            .PagesHeader(header =>
            {
                header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                header.CustomHeader(_customHeader);
            })
            .MainTableTemplate(template =>
            {
                template.BasicTemplate(BasicTemplate.SilverTemplate);
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                table.MultipleColumnsPerPage(new MultipleColumnsPerPage
                {
                    ColumnsGap = 22,
                    ColumnsPerPage = 2,
                    ColumnsWidth = 250,
                    IsRightToLeft = false,
                    TopMargin = 7
                });
            })
            .MainTableDataSource(dataSource =>
            {
                var rows = new List<Task>();
                var rnd = new Random();
                for (int i = 1; i < 210; i++)
                {
                    rows.Add(new Task
                    {
                        Assignee = new User
                        {
                            Id = i,
                            Name = "user-" + i
                        },
                        IsActive = rnd.Next(0, 2) == 1,
                        Name = "task-" + i
                    });
                }
                dataSource.StronglyTypedList(rows);
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
                    column.PropertyName<Task>(x => x.Name);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(3);
                    column.HeaderCell("Task Name");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Task>(x => x.Assignee.Name); // nested property support
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("Assignee");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<Task>(x => x.IsActive);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(3);
                    column.Width(2);
                    column.HeaderCell("Active");
                    column.ColumnItemsTemplate(template =>
                    {
                        template.Checkmark(checkmarkFillColor: Color.Green, crossSignFillColor: Color.DarkRed);
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

    public class CustomFooter : IPageFooter
    {
        PdfContentByte _pdfContentByte;
        readonly IPdfFont _pdfRptFont;
        readonly iTextSharp.text.Font _font;
        readonly PdfRunDirection _direction;
        PdfTemplate _template;

        public CustomFooter(IPdfFont pdfRptFont, PdfRunDirection direction)
        {
            _direction = direction;
            _pdfRptFont = pdfRptFont;
            _font = _pdfRptFont.Fonts[0];
        }

        public void ClosingDocument(PdfWriter writer, Document document, IList<SummaryCellData> columnCellsSummaryData)
        {
            _template.BeginText();
            _template.SetFontAndSize(_pdfRptFont.Fonts[0].BaseFont, 8);
            _template.SetTextMatrix(0, 0);
            _template.ShowText((writer.PageNumber - 1).ToString());
            _template.EndText();
        }

        public void PageFinished(PdfWriter writer, Document document, IList<SummaryCellData> columnCellsSummaryData)
        {
            var pageSize = document.PageSize;
            var text = "Page " + writer.PageNumber + " / ";
            var textLen = _font.BaseFont.GetWidthPoint(text, _font.Size);
            var center = (pageSize.Left + pageSize.Right) / 2;
            var align = _direction == PdfRunDirection.RightToLeft ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;

            ColumnText.ShowTextAligned(
                        canvas: _pdfContentByte,
                        alignment: align,
                        phrase: new Phrase(text, _font),
                        x: center,
                        y: pageSize.GetBottom(25),
                        rotation: 0,
                        runDirection: (int)_direction,
                        arabicOptions: 0);

            var x = _direction == PdfRunDirection.RightToLeft ? center - textLen : center + textLen;
            _pdfContentByte.AddTemplate(_template, x, pageSize.GetBottom(25));
        }

        public void DocumentOpened(PdfWriter writer, IList<SummaryCellData> columnCellsSummaryData)
        {
            _pdfContentByte = writer.DirectContent;
            _template = _pdfContentByte.CreateTemplate(50, 50);
        }
    }

    public class CustomHeader : IPageHeader
    {
        public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> rowdata, IList<SummaryCellData> summaryData)
        {
            return null; // don't render anything.
        }

        iTextSharp.text.Image _image;
        public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
        {
            if (_image == null) //cache is empty
            {
                var templatePath = System.IO.Path.Combine(TestUtils.GetBaseDir(), "data", "PdfHeaderTemplate.pdf");
                _image = PdfImageHelper.GetITextSharpImageFromPdfTemplate(pdfWriter, templatePath);
            }

            var table = new PdfGrid(1);
            var cell = new PdfPCell(_image, true) { Border = 0 };
            table.AddCell(cell);
            return table;

            //Note: return null if you want to skip this callback and render nothing. Also in this case, you need to set the header.CacheHeader(cache: false) too.
        }
    }
}
