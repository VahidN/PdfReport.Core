using System.ComponentModel;

namespace PdfRpt.Core.FunctionalTests.Models
{
    public enum JobTitle
    {
        [Description("Grunt")]
        Grunt,

        [Description("Programmer")]
        Programmer,

        [Description("Analyst Programmer")]
        AnalystProgrammer,

        [Description("Project Manager")]
        ProjectManager,

        [Description("Chief Information Officer")]
        ChiefInformationOfficer,
    }
}
