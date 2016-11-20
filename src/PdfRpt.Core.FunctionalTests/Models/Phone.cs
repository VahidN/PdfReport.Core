
namespace PdfRpt.Core.FunctionalTests.Models
{
    public class Phone
    {
        public Phone(PhoneType phoneType, string areaCode, string phoneNumber)
        {
            this.PhoneType = phoneType;
            this.AreaCode = areaCode;
            this.PhoneNumber = phoneNumber;
        }

        public PhoneType PhoneType { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
