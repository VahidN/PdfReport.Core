using System;

namespace PdfRpt.Core.SampleWebApp.Models
{
    public class User
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string LastName { set; get; }
        public long Balance { set; get; }
        public DateTime RegisterDate { set; get; }
    }
}