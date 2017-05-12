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
    }
}