
namespace PdfRpt.Core.FunctionalTests.Models
{
    public class Order
    {
        public int Id { set; get; }
        public int Price { set; get; }
        public string Description { set; get; }
        public OrderType Type { set; get; }
    }
}
