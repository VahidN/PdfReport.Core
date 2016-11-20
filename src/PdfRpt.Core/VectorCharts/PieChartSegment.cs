using iTextSharp.text;

namespace PdfRpt.VectorCharts
{
    /// <summary>
    /// Represents a PieChart's segment data
    /// </summary>
    public class PieChartSegment
    {
        /// <summary>
        /// ctor.
        /// </summary>
        public PieChartSegment()
        { }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="value">Value of the chart's segment.</param>        
        /// <param name="color">Color of the segment.</param>
        /// <param name="label">Label of the segment.</param>
        public PieChartSegment(double value, BaseColor color, string label)
        {
            Value = value;
            Label = label;
            Color = color;
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="value">Value of the chart's segment.</param>
        /// <param name="color">Color of the segment.</param>
        /// <param name="label">Label of the segment.</param>        
        public PieChartSegment(double value, System.Drawing.Color color, string label)
        {
            Value = value;
            Label = label;
            Color = new BaseColor(color.ToArgb());
        }

        /// <summary>
        /// Value of the chart's segment.
        /// </summary>
        public double Value { set; get; }

        /// <summary>
        /// Label of the segment.
        /// </summary>
        public string Label { set; get; }

        /// <summary>
        /// Color of the segment.
        /// </summary>
        public BaseColor Color { set; get; }

        /// <summary>
        /// Segment's angle.
        /// </summary>
        internal float Angle { set; get; }

        /// <summary>
        /// PieChart's Percentage
        /// </summary>
        internal float Percentage
        {
            get { return Angle * 100 / 360; }
        }
    }
}