using iTextSharp.text;

namespace PdfRpt.VectorCharts
{
    /// <summary>
    /// BarChartItem
    /// </summary>
    public class BarChartItem
    {
        /// <summary>
        /// BarChartItem
        /// </summary>
        public BarChartItem()
        { }

        /// <summary>
        /// BarChartItem
        /// </summary>
        /// <param name="value">Value of the chart's item.</param>
        /// <param name="label">Label of the item.</param>
        /// <param name="color">Color of the item.</param>
        public BarChartItem(double value, string label, BaseColor color)
        {
            Value = value;
            Label = label;
            Color = color;
        }

        /// <summary>
        /// BarChartItem
        /// </summary>
        /// <param name="value">Value of the chart's item.</param>
        /// <param name="label">Label of the item.</param>
        /// <param name="color">Color of the item.</param>
        public BarChartItem(double value, string label, System.Drawing.Color color)
        {
            Value = value;
            Label = label;
            Color = new BaseColor(color.ToArgb());
        }

        /// <summary>
        /// Value of the chart's item.
        /// </summary>
        public double Value { set; get; }

        /// <summary>
        /// Label of the item.
        /// </summary>
        public string Label { set; get; }

        /// <summary>
        /// Color of the item.
        /// </summary>
        public BaseColor Color { set; get; }
    }
}