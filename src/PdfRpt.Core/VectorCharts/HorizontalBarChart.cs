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
    /// Draws a Horizontal BarChart.
    /// </summary>
    public class HorizontalBarChart : IVectorChart
    {
        #region Fields

        private float _bottomMargin;
        private float _chartHeight;
        float _leftMargin;
        BarChartItem _maxLabelLengthItem;
        BarChartItem _maxValueItem;
        float _maxValueWidth;
        private float _spaceBetweenBars;
        PdfTemplate _template;
        float _textHeight;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Horizontal BarChart
        /// </summary>
        public HorizontalBarChart()
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
        /// Its default value is 300.
        /// </summary>
        public float ChartWidth { set; get; }

        /// <summary>
        /// Drawing canvas
        /// </summary>
        public PdfContentByte ContentByte { set; get; }

        /// <summary>
        /// RTL or LTR. Its default value is LTR.
        /// </summary>
        public PdfRunDirection Direction { set; get; }

        /// <summary>
        /// Its default value is BaseColor.DARK_GRAY.
        /// </summary>
        public BaseColor HorizontalBarBorderColor { set; get; }

        /// <summary>
        /// Its default value is 0.4f
        /// </summary>
        public float HorizontalBarBorderLineWidth { set; get; }

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

        #endregion Properties

        #region Methods

        // Public Methods

        /// <summary>
        /// Draws a Horizontal BarChart.
        /// This method returns a vector image which can be painted on a table's cell or part of a page.
        /// </summary>
        /// <returns>An instance of iTextSharp.text.pdf.Image</returns>
        public Image Draw()
        {
            sanityCheck();
            initValues();
            createTemplate();
            drawArea();
            drawSegments();
            drawXYAxis();
            restoreStates();
            return Image.GetInstance(_template);
        }
        // Private Methods

        private void addMarkerLineToChart(float x1)
        {
            _template.SetLineWidth(HorizontalLineWidth);
            _template.SetColorStroke(HorizontalGridColor);
            _template.MoveTo(x1, Margin + _bottomMargin);
            _template.LineTo(x1, Margin + _bottomMargin - 4);
            _template.Stroke();
        }

        private void addMarkerTextToChart(float x1, double xAxisValue)
        {
            var textWidth = getTextWidth(xAxisValue.ToString(CultureInfo.InvariantCulture));
            ColumnText.ShowTextAligned(
                canvas: _template,
                alignment: Element.ALIGN_LEFT,
                phrase: PdfFont.FontSelector.Process(xAxisValue.ToString(CultureInfo.InvariantCulture)),
                x: x1 - (textWidth / 2),
                y: _bottomMargin + Margin - _textHeight - 4,
                rotation: 0,
                runDirection: (int)Direction,
                arabicOptions: 0);
        }

        private void createTemplate()
        {
            ContentByte.SaveState();
            _template = ContentByte.CreateTemplate(ChartWidth, _chartHeight);
            _chartHeight -= Margin;
            ChartWidth -= _maxValueWidth;
            _template.SaveState();
        }

        private void drawArea()
        {
            _template.SetColorStroke(BorderColor);
            _template.SetColorFill(BackgroundColor);
            _template.Rectangle(0, 0, _template.Width, _template.Height);
            _template.FillStroke();
        }

        private float drawHorizontalBar(float top, BarChartItem item)
        {
            var barValue = (((float)item.Value * 100f / (float)_maxValueItem.Value)) * (ChartWidth - _leftMargin - Margin - Margin) / 100f;

            _template.SetColorStroke(HorizontalBarBorderColor);
            _template.SetLineWidth(HorizontalBarBorderLineWidth);
            _template.SetColorFill(item.Color);
            _template.Rectangle(_leftMargin + Margin, top, barValue, BarWidth);
            _template.FillStroke();

            return barValue;
        }

        private void drawHorizontalLabel(float top, BarChartItem item, float barValue)
        {
            ColumnText.ShowTextAligned(
                canvas: _template,
                alignment: Element.ALIGN_LEFT,
                phrase: PdfFont.FontSelector.Process(item.Value.ToString(CultureInfo.InvariantCulture)),
                x: _leftMargin + Margin + barValue + 2,
                y: top + 2,
                rotation: 0,
                runDirection: (int)Direction,
                arabicOptions: 0);
        }

        private void drawSegments()
        {
            // this value is used to increment the x-axis marker value.
            var xMarkerValue = (float)Math.Ceiling(_maxValueItem.Value / ScaleFactor);

            // get the scale based on the current max x value and other chart element area adjustments.
            var scale = ((xMarkerValue * 100f / (float)_maxValueItem.Value)) *
                ((ChartWidth - _leftMargin - Margin - Margin) / 100f);

            var x1 = _leftMargin + Margin;

            double xAxisValue = 0;

            for (var i = 0; i <= ScaleFactor; i++)
            {
                addMarkerLineToChart(x1);
                drawVerticalGrid(x1);
                addMarkerTextToChart(x1, xAxisValue);

                x1 += scale;
                xAxisValue += xMarkerValue;
            }
        }

        private void drawVerticalGrid(float x1)
        {
            _template.SetLineWidth(HorizontalLineWidth);
            _template.SetColorStroke(HorizontalGridColor);
            _template.MoveTo(x1, _bottomMargin + Margin);
            _template.LineTo(x1, _chartHeight);
            _template.Stroke();
        }

        private void drawXAxis()
        {
            _template.SetLineWidth(AxisLineWidth);
            _template.SetColorStroke(AxisLineColor);
            _template.MoveTo(Margin + _leftMargin, Margin + _bottomMargin);
            _template.LineTo(ChartWidth - Margin, Margin + _bottomMargin);
            _template.Stroke();
        }

        private void drawXYAxis()
        {
            drawYAxis();
            drawXAxis();
        }

        private void drawYAxis()
        {
            drawYAxisLine();

            var top = Margin + _bottomMargin + 5f;
            foreach (var item in Items)
            {
                drawYAxisLabel(top, item);
                var barValue = drawHorizontalBar(top, item);
                drawHorizontalLabel(top, item, barValue);
                top += _spaceBetweenBars + BarWidth;
            }
        }

        private void drawYAxisLabel(float top, BarChartItem item)
        {
            ColumnText.ShowTextAligned(
                canvas: _template,
                alignment: Element.ALIGN_LEFT,
                phrase: PdfFont.FontSelector.Process(item.Label),
                x: Margin,
                y: top + 2,
                rotation: 0,
                runDirection: (int)Direction,
                arabicOptions: 0);
        }

        private void drawYAxisLine()
        {
            _template.SetLineWidth(AxisLineWidth);
            _template.SetColorStroke(AxisLineColor);
            _template.MoveTo(Margin + _leftMargin, Margin + _bottomMargin);
            _template.LineTo(Margin + _leftMargin, _chartHeight);
            _template.Stroke();
        }

        private float getTextHeight(string text)
        {
            var baseFont = PdfFont.Fonts.First().BaseFont;
            var ascend = baseFont.GetAscentPoint(text, PdfFont.Size);
            var descend = baseFont.GetDescentPoint(text, PdfFont.Size);
            return ascend - descend;
        }

        private float getTextWidth(string text)
        {
            var width = PdfFont.Fonts.First().BaseFont.GetWidthPoint(text, PdfFont.Size);
            return width;
        }

        private void initValues()
        {
            _maxValueItem = Items.OrderByDescending(x => x.Value).First();
            _maxLabelLengthItem = Items.OrderByDescending(x => x.Label.Length).First();
            _maxValueWidth = getTextWidth(_maxValueItem.Value.ToString(CultureInfo.InvariantCulture));

            var maxLabelLengthItemWidth = getTextWidth(_maxLabelLengthItem.Label);
            _leftMargin = maxLabelLengthItemWidth + Margin;

            _textHeight = getTextHeight(_maxLabelLengthItem.Label);
            _spaceBetweenBars = _textHeight * 0.7f;
            _bottomMargin = _textHeight;
            _chartHeight = _bottomMargin + (2 * Margin) + (Items.Count() * (_spaceBetweenBars + BarWidth));
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
            Direction = PdfRunDirection.LeftToRight;
            BorderColor = BaseColor.DarkGray;
            BackgroundColor = BaseColor.Gray;
            ChartWidth = 300;
            AxisLineColor = BaseColor.Yellow;
            HorizontalBarBorderColor = BaseColor.DarkGray;
            ScaleFactor = 10;
            AxisLineWidth = 1;
            HorizontalLineWidth = 0.4f;
            HorizontalBarBorderLineWidth = 0.4f;
        }

        #endregion Methods
    }
}