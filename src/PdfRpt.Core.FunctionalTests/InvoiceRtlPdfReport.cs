using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.FunctionalTests.Templates;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfRpt.Core.FunctionalTests
{
    public class InvoicePdfViewModel
    {
        public string Status { set; get; }
        public string Details { set; get; }
    }

    [TestClass]
    public class InvoiceRtlPdfReport
    {
        [TestMethod]
        public void Verify_InvoiceRtlPdfReport_Can_Be_Created()
        {
            var reportBytes = CreateInvoiceRtlPdfReport();
            TestUtils.VerifyPdfFileIsReadable(reportBytes);
        }

        private static PdfGrid createHeader(PagesHeaderBuilder header)
        {
            var table = new PdfGrid(numColumns: 1)
            {
                WidthPercentage = 100,
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                SpacingAfter = 7
            };

            var basefont = BaseFont.CreateFont(TestUtils.GetTahomaFontPath(), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var gillsans = new iTextSharp.text.Font(basefont, 30, iTextSharp.text.Font.BOLD, new BaseColor(ColorTranslator.FromHtml("#009cde").ToArgb()));
            FontSelector selector = new FontSelector();
            selector.AddFont(gillsans);
            var title = selector.Process("PayPing");

            var pdfCell = new PdfPCell(title)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                BorderWidthLeft = 0,
                BorderWidthRight = 0,
                BorderWidthTop = 0,
                BorderWidthBottom = 1,
                PaddingBottom = 10,
                BorderColorBottom = new BaseColor(System.Drawing.Color.LightGray.ToArgb()),
                HorizontalAlignment = Element.ALIGN_CENTER
            };

            table.AddCell(pdfCell);
            return table;
        }

        private static IList<InvoicePdfViewModel> getDataSourceList()
        {
            var dataSourceList = new List<InvoicePdfViewModel>();
            dataSourceList.Add(new InvoicePdfViewModel { Status = "پرداخت کننده", Details = "sdfsddssdf" });
            dataSourceList.Add(new InvoicePdfViewModel { Status = "توضیح پرداخت", Details = "thhtrhrh" });
            dataSourceList.Add(new InvoicePdfViewModel { Status = "دریافت کننده(فروشنده)", Details = "name" });
            dataSourceList.Add(new InvoicePdfViewModel { Status = "کد خرید", Details = "payment.Code" });
            dataSourceList.Add(new InvoicePdfViewModel { Status = "تاریخ پرداخت", Details = "hjghjgh" });
            dataSourceList.Add(new InvoicePdfViewModel { Status = "شماره پرداخت", Details = "kukuuuuuuuuuuuuu" });
            dataSourceList.Add(new InvoicePdfViewModel
            {
                Status = "مبلغ",
                Details =
                $"{2545450:#,0} تومان".ToPersianNumbers()
            });
            return dataSourceList;
        }

        public byte[] CreateInvoiceRtlPdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.RightToLeft);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A5);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "IList Rpt.", Subject = "Test Rpt", Title = "Test" });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
                doc.PrintingPreferences(new PrintingPreferences
                {
                    ShowPrintDialogAutomatically = false
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
                footer.DefaultFooter("تاريخ: " + PersianDate.ToPersianDateTime(DateTime.Now, "/", true).FixWeakCharacters(),
                                 PdfRunDirection.RightToLeft);
            })
            .PagesHeader(header =>
            {
                header.CacheHeader(cache: true);
                header.InlineHeader(inlineHeader =>
                {
                    inlineHeader.AddPageHeader(data => createHeader(header));
                });

            })
            .MainTableTemplate(template =>
            {
                template.CustomTemplate(new TransparentTemplate());
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                table.SpacingAfter(0);
            })
            .MainTableDataSource(dataSource =>
            {
                dataSource.StronglyTypedList(getDataSourceList());
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
                    column.FixedHeight(30);
                    column.HeaderCell("#");
                    column.AddHeadingCell("فاکتور پرداخت", mergeHeaderCell: true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<InvoicePdfViewModel>(x => x.Status);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(6);
                    column.MinimumHeight(30);
                    column.HeaderCell("اطلاعات");
                    column.AddHeadingCell(string.Empty, mergeHeaderCell: true);
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<InvoicePdfViewModel>(x => x.Details);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(5);
                    column.HeaderCell("جزئیات");
                    column.AddHeadingCell(string.Empty, mergeHeaderCell: true);
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "رکوردی یافت نشد.");

                events.MainTableAdded(args =>
                {
                    var infoTable = new PdfGrid(numColumns: 1)
                    {
                        WidthPercentage = 100
                    };

                    infoTable.AddSimpleRow(
                         (cellData, properties) =>
                         {
                             cellData.Value = $"این فاکتور به منزله پرداخت شما به اشکان است.";
                             properties.PdfFont = events.PdfFont;
                             properties.FontColor = BaseColor.Gray;
                             properties.RunDirection = PdfRunDirection.RightToLeft;
                         });
                    infoTable.AddSimpleRow(
                         (cellData, properties) =>
                         {
                             cellData.Value = "هر گونه سوال در مورد خدمات یا محصول را با فروشنده در میان گذارید";
                             properties.PdfFont = events.PdfFont;
                             properties.FontColor = BaseColor.Gray;
                             properties.RunDirection = PdfRunDirection.RightToLeft;
                         });
                    args.PdfDoc.Add(infoTable.AddBorderToTable(borderColor: BaseColor.LightGray, spacingBefore: 1f));

                });
            })
               .Export(export =>
               {
                   export.ToExcel();
               })
               .GenerateAsByteArray();
        }
    }
}