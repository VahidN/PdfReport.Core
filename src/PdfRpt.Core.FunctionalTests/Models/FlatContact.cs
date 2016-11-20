
namespace PdfRpt.Core.FunctionalTests.Models
{
    public class FlatContact
    {
        public int Id;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PhoneType PhoneType { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
