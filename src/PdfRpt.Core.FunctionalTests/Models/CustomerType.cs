using System.ComponentModel;

namespace PdfRpt.Core.FunctionalTests.Models
{
    public enum CustomerType
    {
        [Description("Ordinary User")]
        Ordinary,

        [Description("Special User")]
        Special
    }
}
