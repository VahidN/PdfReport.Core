using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.VectorCharts;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class VectorPieChartPdfReport
    {
        [TestMethod]
        public void Verify_VectorPieChartPdfReport_Can_Be_Created()
        {
            var report = CreateVectorPieChartPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report);
        }

        public string CreateVectorPieChartPdfReport()
        {
            var fonts = new GenericFontProvider(
                                TestUtils.GetTahomaFontPath(),
                                TestUtils.GetVerdanaFontPath()
                                );

            var finalFile = Path.Combine(TestUtils.GetOutputFolder(), "PieChartPdfReport.pdf");

            var document = new Document(PageSize.A4);

            var fileStream = new FileStream(finalFile, FileMode.Create);
            var writer = PdfWriter.GetInstance(document, fileStream);
            document.AddAuthor("Vahid");
            document.Open();
            var canvas = writer.DirectContent;

            var img = new PieChart
            {
                Direction = PdfRunDirection.RightToLeft,
                ContentByte = canvas,
                PdfFont = fonts,
                Segments = new List<PieChartSegment>
                              {
                                      new PieChartSegment(100, new BaseColor(130, 197, 91), "عنوان يك"),
                                      new PieChartSegment(80, new BaseColor(95, 182, 85), "عنوان دو"),
                                      new PieChartSegment(60, new BaseColor(88, 89, 91), "عنوان سه"),
                                      new PieChartSegment(50, new BaseColor(67, 66, 61), "عنوان چهار"),
                                      new PieChartSegment(50, new BaseColor(173, 216, 230), "عنوان پنچ")
                              }
            }.Draw();
            document.Add(img);

            var img2 = new PieChart
            {
                ContentByte = canvas,
                PdfFont = fonts,
                Segments = new List<PieChartSegment>
                                         {
                                            new PieChartSegment{ Value=5, Color = new BaseColor(130, 197, 91), Label = "Title 1" },
                                            new PieChartSegment{ Value=60, Color = new BaseColor(95, 182, 85), Label = "Title 2" },
                                            new PieChartSegment{ Value=35, Color = new BaseColor(88, 89, 91), Label = "Title 3" },
                                            new PieChartSegment{ Value=0, Color = new BaseColor(67, 66, 61), Label = "Title 4" }
                                         }
            }.Draw();
            img2.SetAbsolutePosition(310, 620);
            img2.ScalePercent(150);
            document.Add(img2);

            document.Close();
            fileStream.Dispose();

            return finalFile;
        }
    }
}