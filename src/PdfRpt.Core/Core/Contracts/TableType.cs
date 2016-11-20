
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Possible values of the TableType
    /// </summary>
    public enum TableType
    {
        /// <summary>
        /// A normal PdfGrid.
        /// </summary>
        NormalTable,

        /// <summary>
        /// Horizontal StackPanel writes the values of data source based on PdfRunDirection from ltr or rtl of each row.
        /// For creating vertical StackPanel, just use the MainTablePreferences.MultipleColumnsPerPage method.
        /// All columns and properties of an object will create a single cell here.
        /// </summary>
        HorizontalStackPanel
    }
}
