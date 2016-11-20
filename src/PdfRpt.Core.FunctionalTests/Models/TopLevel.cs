namespace PdfRpt.Core.FunctionalTests.Models
{
    public class TopLevel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string LastName { set; get; }
        public NestedType NestedType { set; get; }
    }
}