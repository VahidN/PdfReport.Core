using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class QuestionsFormPdfReport
    {
        [TestMethod]
        public void Verify_QuestionsFormPdfReport_Can_Be_Created()
        {
            var report = CreateQuestionsFormPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateQuestionsFormPdfReport()
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
                 template.BasicTemplate(BasicTemplate.SilverTemplate);
             })
             .MainTablePreferences(table =>
             {
                 table.ColumnsWidthsType(TableColumnWidthType.Relative);
             })
             .MainTableDataSource(dataSource =>
             {
                 var listOfRows = new List<Question>();
                 for (int i = 1; i <= 20; i++)
                 {
                     listOfRows.Add(new Question
                     {
                         Id = i,
                         QuestionText = "A very long text. A very long text. A very long text. A very long text. A very long text. A very long text. A very long text. A very long text. A very long text. متن " + i,
                         Answer1 = "A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. گزينه " + i,
                         Answer2 = "A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. " + i,
                         Answer3 = "A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. " + i,
                         Answer4 = "A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. A very long item. " + i,
                         PicturePath = TestUtils.GetImagePath("01.png")
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
                     column.PropertyName<Question>(x => x.Id);
                     column.HeaderCell(caption: "Questions");
                     column.Width(1);
                     column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                     column.IsVisible(true);
                     column.Order(1);
                     column.ColumnItemsTemplate(template =>
                     {
                         template.CustomTemplate(new EntryTemplate(PdfRunDirection.LeftToRight));
                     });
                 });
             })
             .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public class EntryTemplate : IColumnItemsTemplate
    {
        readonly PdfRunDirection _pdfRunDirection;
        public EntryTemplate(PdfRunDirection pdfRunDirection)
        {
            _pdfRunDirection = pdfRunDirection;
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
            var data = attributes.RowData.TableRowData;
            var id = data.GetSafeStringValueOf<Question>(x => x.Id);
            var questionText = data.GetSafeStringValueOf<Question>(x => x.QuestionText);
            var answer1 = data.GetSafeStringValueOf<Question>(x => x.Answer1);
            var answer2 = data.GetSafeStringValueOf<Question>(x => x.Answer2);
            var answer3 = data.GetSafeStringValueOf<Question>(x => x.Answer3);
            var answer4 = data.GetSafeStringValueOf<Question>(x => x.Answer4);
            var picturePath = data.GetSafeStringValueOf<Question>(x => x.PicturePath);

            var font = attributes.BasicProperties.PdfFont;

            var relativeWidths = getRelativeWidths();

            var mainTable = new PdfGrid(relativeWidths)
            {
                RunDirection = (int)_pdfRunDirection,
                WidthPercentage = 100,
                SpacingBefore = 5,
                SpacingAfter = 5
            };

            addQuestionText(id, questionText, font, mainTable);
            addOptions(answer1, answer2, answer3, answer4, font, mainTable);
            addImageCell(picturePath, mainTable);

            return new PdfPCell(mainTable);
        }

        private float[] getRelativeWidths()
        {
            var relativeWidths = new float[] { 1, 5 };
            if (_pdfRunDirection == PdfRunDirection.LeftToRight)
            {
                relativeWidths = relativeWidths.Reverse().ToArray();
            }
            return relativeWidths;
        }

        private void addOptions(string answer1, string answer2, string answer3, string answer4, IPdfFont font, PdfGrid mainTable)
        {
            var optionsTable = new PdfGrid(numColumns: 2)
            {
                RunDirection = (int)_pdfRunDirection,
                WidthPercentage = 100,
            };

            //---------------- row - 1
            optionsTable.AddCell(new PdfPCell(font.FontSelector.Process("a) " + answer1))
            {
                Border = 0,
                Padding = 5
            });
            optionsTable.AddCell(new PdfPCell(font.FontSelector.Process("b) " + answer2))
            {
                Border = 0,
                Padding = 5
            });

            //---------------- row - 2
            optionsTable.AddCell(new PdfPCell(font.FontSelector.Process("c) " + answer3))
            {
                Border = 0,
                Padding = 5
            });
            optionsTable.AddCell(new PdfPCell(font.FontSelector.Process("d) " + answer4))
            {
                Border = 0,
                Padding = 5
            });
            mainTable.AddCell(new PdfPCell(optionsTable) { Border = 0 });
        }

        private static void addQuestionText(string id, string questionText, IPdfFont font, PdfGrid mainTable)
        {
            mainTable.AddCell(new PdfPCell(font.FontSelector.Process(id + ") " + questionText))
            {
                Border = 0,
                Padding = 5,
                Colspan = 2
            });
        }

        private static void addImageCell(string picturePath, PdfGrid mainTable)
        {
            mainTable.AddCell(new PdfPCell(PdfImageHelper.GetITextSharpImageFromImageFile(picturePath))
            {
                Border = 0,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });
        }
    }
}