namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Summary cell's alignment
    /// </summary>
    public class AggregateValuePosition
    {
        /// <summary>
        /// Sets summary cell's value horizontal alignment
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Horizontal position.
        /// </summary>
        public float X { get; set; }
    }
}