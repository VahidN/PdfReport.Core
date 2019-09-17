using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class ParallelPdfReport
    {
        [TestMethod]
        public void Verify_ParallelPdfReport1_Can_Be_Created()
        {
            var tests = new List<Action>();

            for (var i = 0; i < 40; i++)
            {
                tests.Add(() => new InMemoryPdfReport().Verify_InMemoryPdfReport_Can_Be_Created());
            }
            Parallel.Invoke(tests.ToArray());
        }

        [TestMethod]
        public void Verify_ParallelPdfReport2_Can_Be_Created()
        {
            var tests = new List<Action>();

            for (var i = 0; i < 40; i++)
            {
                tests.Add(() => new HtmlCellTemplatePdfReport().CreateHtmlCellTemplatePdfReport().GenerateAsByteArray());
            }
            Parallel.Invoke(tests.ToArray());
        }
    }
}