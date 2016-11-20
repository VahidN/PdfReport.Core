
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// If MainTableType is set to HorizontalStackPanel, here you can define its preferences such as
    /// number of columns per row.
    /// </summary>
    public class HorizontalStackPanelPreferences
    {
        /// <summary>
        /// Number of columns per row.
        /// </summary>
        public int ColumnsPerRow { set; get; }
    }
}
