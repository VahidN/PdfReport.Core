
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// PageOrientation values
    /// </summary>
    public enum PageOrientation
    {
        /// <summary>
        /// Portrait Orientation.
        /// </summary>
        Portrait = 0,

        /// <summary>
        /// Landscape Orientation.
        /// In this case, you need to set the PdfRptPrintingPreferences.PickTrayByPdfSize to true.
        /// </summary>
        Landscape = 1
    }
}
