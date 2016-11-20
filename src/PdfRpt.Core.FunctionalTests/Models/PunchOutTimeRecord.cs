using System;

namespace PdfRpt.Core.FunctionalTests.Models
{
    public class PunchOutTimeRecord
    {
        public int Id { set; get; }
        public string EmployeeName { set; get; }
        public DateTime LogTime { set; get; }
    }
}
