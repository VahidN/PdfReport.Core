using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.VectorCharts
{
    /// <summary>
    /// Draws a Vertical BarChart.
    /// </summary>
    public class VerticalBarChart : IVectorChart
    {
        #region Fields

        float _chartWidth;
        float _leftMargin;
        BarChartItem _maxValueItem;
        PdfTemplate _template;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Vertical BarChart
        /// </summary>
        public VerticalBarChart()
        {
            setDefaults();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Its default value is BaseColor.YELLOW
        /// </summary>
        public BaseColor AxisLineColor { set; get; }

        /// <summary>
        /// Its default value is 1.
        /// </summary>
        public float AxisLineWidth { set; get; }

        /// <summary>
        /// BarChart's area background color. Its default value is BaseColor.GRAY.
        /// </summary>
        public BaseColor BackgroundColor { set; get; }

        /// <summary>
        /// Its default value is 10.
        /// </summary>
        public float BarWidth { set; get; }

        /// <summary>
        /// BarChart's area border color. Its default value is BaseColor.DARK_GRAY.
        /// </summary>
        public BaseColor BorderColor { set; get; }

        /// <summary>
        /// Its default value is 50.
        /// </summary>
        public float BottomMargin { set; get; }

        /// <summary>
        /// Its default value is 200.
        /// </summary>
        public float ChartHeight { set; get; }

        /// <summary>
        /// Drawing canvas
        /// </summary>
        public PdfContentByte ContentByte { set; get; }

        /// <summary>
        /// RTL or LTR. Its default value is LTR.
        /// </summary>
        public PdfRunDirection Direction { set; get; }

        /// <summary>
        /// Its default value is LightGray.
        /// </summary>
        public BaseColor HorizontalGridColor { set; get; }

        /// <summary>
        /// Its default value is 0.4f
        /// </summary>
        public float HorizontalLineWidth { set; get; }

        /// <summary>
        /// Segments of BarChart to draw.
        /// </summary>
        public IEnumerable<BarChartItem> Items { set; get; }

        /// <summary>
        /// BarChart's Margin from the edges. Its default value is 10.
        /// </summary>
        public float Margin { set; get; }

        /// <summary>
        /// Labels font. Use GenericFontProvider class to initialize it.
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// Set the scale factor for y-axis marker.
        /// Its default value is 10.
        /// </summary>
        public float ScaleFactor { set; get; }

        /// <summary>
        /// Its default value is 10.
        /// </summary>
        public float SpaceBetweenBars { set; get; }

        /// <summary>
        /// Its default value is BaseColor.DARK_GRAY.
        /// </summary>
        public BaseColor VerticalBarBorderColor { set; get; }

        /// <summary>
        /// Its default value is 0.4f
        /// </summary>
        public float VerticalBarBorderLineWidth { set; get; }

        /// <summary>
        /// Its default value is -60.
        /// After changing this value, you need to set XAxisLabelXLocationDelta and XAxisLabelYLocation too.
        /// </summary>
        public float XAxisLabelRotation { set; get; }

        /// <summary>
        /// Its default value is 0.
        /// </summary>
        public float XAxisLabelXLocationDelta { set; get; }

        /// <summary>
        /// Its default value is (BottomMargin + Margin - 4).
        /// </summary>
        public float XAxisLabelYLocation { set; get; }

        #endregion Properties

        #region Methods

        // Public Methods

        /// <summary>
        /// Draws a Vertical BarChart.
        /// This method returns a vector image which can be painted on a table's cell or part of a page.
        /// </summary>
        /// <returns>An instance of iTextSharp.text.pdf.Image</returns>
        public Image Draw()
        {
            sanityCheck();
            initValues();
            createTemplate();
            drawArea();
            drawXYAxis();
            drawSegments();
            restoreStates();
            return Image.GetInstance(_template);
        }
        // Private Methods

        private void addMarkerLineToChart(float y1)
        {
            _template.SetLineWidth(HorizontalLineWidth);
            _template.SetColorStroke(HorizontalGridColor);
            _template.MoveTo(Margin + _leftMargin, y1);
            _template.LineTo(Margin + _leftMargin - 4, y1);
            _template.Stroke();
        }

        private void addMarkerTextToChart(float y1, double yAxisValue)
        {
            ColumnText.ShowTextAligned(
                canvas: _template,
                alignment: Element.ALIGN_LEFT,
                phrase: PdfFont.FontSelector.Process(yAxisValue.ToString(CultureInfo.InvariantCulture)),
                x: Margin,
                y: y1 - 2,
                rotation: 0,
                runDirection: (int)Direction,
                arabicOptions: 0);
        }

        private void createTemplate()
        {
            ContentByte.SaveState();
            _template = ContentByte.CreateTemplate(_chartWidth, ChartHeight);
            ChartHeight -= Margin;
            _template.SaveState();
        }

        private void drawArea()
        {
            _template.SetColorStroke(BorderColor);
            _template.SetColorFill(BackgroundColor);
            _template.Rectangle(0, 0, _template.Width, _template.Height);
            _template.FillStroke();
        }

        private void drawHorizontalGrid(float y1)
        {
            _template.SetLineWidth(HorizontalLineWidth);
            _template.SetColorStroke(HorizontalGridColor);
            _template.MoveTo(Margin + _leftMargin, y1);
            _template.LineTo(_chartWidth - Margin, y1);
            _template.Stroke();
        }

        private void drawSegments()
        {
            var left = Margin + _leftMargin + 5f;
            foreach (var item in Items)
            {
                drawXAxisLabel(left, item);
                var barValue = drawVerticalBar(left, item);
                drawVerticalBarLabel(left, item, barValue);
                left = left + SpaceBetweenBars + BarWidth;
            }
        }

        private float drawVerticalBar(float left, BarChartItem item)
        {
            var barValue = (((float)item.Value * 100f / (float)_maxValueItem.Value)) * (ChartHeight - BottomMargin - Margin - Margin) / 100f;

            _template.SetColorStroke(VerticalBarBorderColor);
            _template.SetLineWidth(VerticalBarBorderLineWidth);
            _template.SetColorFill(item.Color);
            _template.Rectangle(left, Margin + BottomMargin, BarWidth, barValue);
            _template.FillStroke();

            return barValue;
        }

        private void drawVerticalBarLabel(float left, BarChartItem item, float barValue)
        {
            var itemValueWidth = getTextWidth(item.Value.ToString(CultureInfo.InvariantCulture));
            left = left - ((itemValueWidth - BarWidth) / 2);

            ColumnText.ShowTextAligned(
                canvas: _template,
                alignment: Element.ALIGN_LEFT,
                phrase: PdfFont.FontSelector.Process(item.Value.ToString(CultureInfo.InvariantCulture)),
                x: left,
                y: barValue + Margin + BottomMargin + 1.2f,
                rotation: 0,
                runDirection: (int)Direction,
                arabicOptions: 0);
        }

        private void drawXAxis()
        {
            _template.SetLineWidth(AxisLineWidth);
            _template.SetColorStroke(AxisLineColor);
            _template.MoveTo(Margin + _leftMargin, Margin + BottomMargin);
            _template.LineTo(_chartWidth - Margin, Margin + BottomMargin);
            _template.Stroke();
        }

        private void drawXAxisLabel(float left, BarChartItem item)
        {
            ColumnText.ShowTextAligned(
                canvas: _template,
                alignment: Element.ALIGN_LEFT,
                phrase: PdfFont.FontSelector.Process(item.Label),
                x: left + XAxisLabelXLocationDelta,
                y: XAxisLabelYLocation,
                rotation: XAxisLabelRotation,
                runDirection: (int)Direction,
                arabicOptions: 0);
        }

        private void drawXYAxis()
        {
            drawYAxis();
            drawXAxis();
        }

        private void drawYAxis()
        {
            drawYAxisLine();

            // this value is used to increment the y-axis marker value.
            var yMarkerValue = (float)Math.Ceiling(_maxValueItem.Value / ScaleFactor);

            // get the scale based on the current max y value and other chart element area adjustments.
            var scale = ((yMarkerValue * 100f / (float)_maxValueItem.Value)) *
                ((ChartHeight - Margin - Margin - BottomMargin) / 100f);

            var y1 = Margin + BottomMargin;

            double yAxisValue = 0;

            for (var i = 0; i <= ScaleFactor; i++)
            {
                addMarkerLineToChart(y1);
                drawHorizontalGrid(y1);
                addMarkerTextToChart(y1, yAxisValue);

                y1 += scale;
                yAxisValue += yMarkerValue;
            }
        }

        private void drawYAxisLine()
        {
            _template.SetLineWidth(AxisLineWidth);
            _template.SetColorStroke(AxisLineColor);
            _template.MoveTo(Margin + _leftMargin, Margin + BottomMargin);
            _template.LineTo(Margin + _leftMargin, ChartHeight - Margin);
            _template.Stroke();
        }

        private float getTextWidth(string text)
        {
            var width = PdfFont.Fonts.First().BaseFont.GetWidthPoint(text, PdfFont.Size);
            return width;
        }

        private void initValues()
        {
            _maxValueItem = Items.OrderByDescending(x => x.Value).First();
            var maxValueWidth = getTextWidth(_maxValueItem.Value.ToString(CultureInfo.InvariantCulture));
            _leftMargin = maxValueWidth + Margin;
            _chartWidth = _leftMargin + (Items.Count() * (SpaceBetweenBars + BarWidth)) + (2 * Margin);
        }

        private void restoreStates()
        {
            _template.RestoreState();
            ContentByte.RestoreState();
        }

        private void sanityCheck()
        {
            if (PdfFont == null)
                throw new NullReferenceException("`PdfFont` is null.");

            if (ContentByte == null)
                throw new NullReferenceException("`ContentByte` is null.");

            if (Items == null)
                throw new NullReferenceException("`Items` is null.");

            if (!Items.Any())
                throw new InvalidOperationException("There is no BarChartItem to draw.");
        }

        private void setDefaults()
        {
            BarWidth = 10;
            Margin = 10;
            HorizontalGridColor = new BaseColor(System.Drawing.Color.LightGray.ToArgb());
            SpaceBetweenBars = 10;
            BottomMargin = 50;
            Direction = PdfRunDirection.LeftToRight;
            BorderColor = BaseColor.DarkGray;
            BackgroundColor = BaseColor.Gray;
            ChartHeight = 200;
            AxisLineColor = BaseColor.Yellow;
            VerticalBarBorderColor = BaseColor.DarkGray;
            ScaleFactor = 10;
            AxisLineWidth = 1;
            HorizontalLineWidth = 0.4f;
            VerticalBarBorderLineWidth = 0.4f;
            XAxisLabelRotation = -60;
            XAxisLabelYLocation = BottomMargin + Margin - 4;
        }

        #endregion Methods
    }
}