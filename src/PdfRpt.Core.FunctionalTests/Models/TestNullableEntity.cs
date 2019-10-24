using System;

namespace PdfRpt.Core.FunctionalTests.Models
{
    public class TestNullableEntity
    {
        public int Id { set; get; }
        public bool? NullableBoolean { set; get; }
        public DateTime? NullableDateTime { set; get; }
    }
}