
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Summary Cell's Location
    /// </summary>
    public enum SummaryLocation
    {
        /// <summary>
        /// It will use the first cell before the first defined aggregate cell.
        /// </summary>
        AtFirstDefinedAggregateCell,

        /// <summary>
        /// Displays summary at end of RowNumberColumn
        /// </summary>
        AtRowNumberColumn,

        /// <summary>
        /// It will use the specified LabelColumnProperty value.
        /// </summary>
        AtSpecifiedLabelColumnProperty        
    }
}
