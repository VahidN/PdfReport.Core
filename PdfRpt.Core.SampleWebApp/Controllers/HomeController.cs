using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PdfRpt.Core.SampleWebApp.Reports;

namespace PdfRpt.Core.SampleWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var reportBytes = InMemoryPdfReport.CreateInMemoryPdfReport(_hostingEnvironment.WebRootPath);
            return File(reportBytes, "application/pdf", "report.pdf");
        }

        /// <summary>
        /// GET /home/streaming
        /// </summary>
        public IActionResult Streaming()
        {
            var outputStream = new MemoryStream();
            InMemoryPdfReport.CreateStreamingPdfReport(_hostingEnvironment.WebRootPath, outputStream);
            return new FileStreamResult(outputStream, "application/pdf")
            {
                FileDownloadName = "report.pdf"
            };
        }
    }
}