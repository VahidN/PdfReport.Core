using System.ComponentModel;

namespace PdfRpt.Core.FunctionalTests.Models
{
    public enum OrderType
    {
        Ordinary,

        [Description("From Company A")]
        FromCompanyA,

        [Description("From Company B")]
        FromCompanyB
    }
}
