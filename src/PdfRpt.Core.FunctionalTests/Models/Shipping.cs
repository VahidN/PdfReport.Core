using System;

namespace PdfRpt.Core.FunctionalTests.Models
{
    public class Shipping
    {
        public string Type { set; get; }
        public int Number { set; get; }

        public int OrderNumber { set; get; }
        public string Name { set; get; }

        public int Quantity { set; get; }
        public float Weight { set; get; }

        public string Destination { set; get; }
        public DateTime ClearanceDate { set; get; }

        public string Description { set; get; }
    }
}