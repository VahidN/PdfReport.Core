
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// A watermark text to display
    /// </summary>
    public class DiagonalWatermark
    {
        /// <summary>
        /// Watermark text to display
        /// </summary>
        public string Text { set; get; }

        /// <summary>
        /// Possible run direction values, left-to-right or right-to-left
        /// </summary>
        public PdfRunDirection? RunDirection { set; get; }

        /// <summary>
        /// Custom font's definitions
        /// </summary>
        public IPdfFont Font { set; get; }

        /// <summary>
        /// Transparency setting. Default FillOpacity is 0.5f.
        /// </summary>
        public float FillOpacity { set; get; }

        /// <summary>
        /// Transparency setting. Default StrokeOpacity is 1f.
        /// </summary>
        public float StrokeOpacity { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public DiagonalWatermark()
        {
            FillOpacity = 0.5f;
            StrokeOpacity = 1;
        }
    }
}
