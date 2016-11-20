using System;

namespace PdfRpt.Core.FunctionalTests.Models
{
    public class ClassSession
    {
        public int DayNumber { get; set; }
        public int WeekNumber { get; set; }
        public bool HasSession { get; set; }
        public int Percent { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }

    public class WeekClassSessions
    {
        public bool IsSelected { get; set; }
        public string ClassName { get; set; }        
        public int WeekNumber { get; set; }
        public string WeekTitle { get; set; }
        public ClassSession WD0 { get; set; }
        public ClassSession WD1 { get; set; }
        public ClassSession WD2 { get; set; }
        public ClassSession WD3 { get; set; }
        public ClassSession WD4 { get; set; }
        public ClassSession WD5 { get; set; }
        public ClassSession WD6 { get; set; }
    }
}