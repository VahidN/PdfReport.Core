
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Pages and groups summary values settings
    /// </summary>
    public class SummaryCellSettings
    {
        /// <summary>
        /// Displaying summary values of each pages by considering the previous pages data.
        /// </summary>
        public SummaryCellAttributes OverallSummarySettings { set; get; }

        /// <summary>
        /// Displaying total summary values of the previous page at beginning of the each page.
        /// </summary>
        public SummaryCellAttributes PreviousPageSummarySettings { set; get; }

        /// <summary>
        /// Displaying summary values of individual pages, regardless of the previous pages
        /// </summary>
        public SummaryCellAttributes PageSummarySettings { set; get; }

        /// <summary>
        /// Sets summary cell's settings of the all groups. 
        /// It will be printed at the end of the rendering of all groups.
        /// It can be null if grouping is not enabled.
        /// </summary>
        public SummaryCellAttributes AllGroupsSummarySettings { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public SummaryCellSettings()
        {
            OverallSummarySettings = new SummaryCellAttributes();
            PreviousPageSummarySettings = new SummaryCellAttributes();
            PageSummarySettings = new SummaryCellAttributes();
            AllGroupsSummarySettings = new SummaryCellAttributes();
        }
    }
}
