using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.VectorCharts;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class VectorBarChartPdfReport
    {
        [TestMethod]
        public void Verify_VectorBarChartPdfReport_Can_Be_Created()
        {
            var report = CreateVectorBarChartPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report);
        }

        public string CreateVectorBarChartPdfReport()
        {
            var fonts = new GenericFontProvider(
                                TestUtils.GetVerdanaFontPath(),
                                TestUtils.GetTahomaFontPath());

            var finalFile = Path.Combine(TestUtils.GetOutputFolder(), "BarChartPdfReport.pdf");

            var document = new Document(PageSize.A4);

            var fileStream = new FileStream(finalFile, FileMode.Create);
            var writer = PdfWriter.GetInstance(document, fileStream);
            document.AddAuthor("Vahid");
            document.Open();
            var canvas = writer.DirectContent;

            var items = new List<BarChartItem>
                      {
                          new BarChartItem(10, "Item 1 caption",  new BaseColor(130, 197, 91)),
                          new BarChartItem(100, "Item 2 caption", new BaseColor(95, 182, 85)),
                          new BarChartItem(60, "Item 3 caption",  new BaseColor(130, 197, 91)),
                          new BarChartItem(70, "Item 4 caption",  new BaseColor(88, 89, 91)),
                          new BarChartItem(120, "Item 5 caption", new BaseColor(173, 216, 230)),
                          new BarChartItem(0, "Item 6 caption",  BaseColor.Yellow),
                          new BarChartItem(210, "Item 7 caption", BaseColor.Magenta),
                          new BarChartItem(150, "Item 8 caption", BaseColor.Orange),
                          new BarChartItem(50, "Item 9 caption", BaseColor.Pink),
                          new BarChartItem(20, "Item 10 caption", BaseColor.Cyan),
                          new BarChartItem(100, "Item 11 caption", BaseColor.Blue),
                          new BarChartItem(90, "عنوان آيتم 12", BaseColor.Green),
                      };

            var img = new VerticalBarChart
            {
                PdfFont = fonts,
                ContentByte = canvas,
                Items = items
            }.Draw();
            document.Add(img);

            var img2 = new HorizontalBarChart
            {
                PdfFont = fonts,
                ContentByte = canvas,
                Items = items
            }.Draw();
            document.Add(img2);

            document.Close();
            fileStream.Dispose();

            return finalFile;
        }
    }
}