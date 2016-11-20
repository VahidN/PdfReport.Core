using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.VectorCharts
{
    /// <summary>
    /// Draws a PieChart.
    /// </summary>
    public class PieChart : IVectorChart
    {
        #region Fields

        float _circleCenterX0;
        float _circleCenterY0;
        float _circleRadius;
        float _endAngle;
        float _initialChartHeight;
        float _initialChartWidth;
        float _labelRowsGap;
        float _labelY;
        float _startAngle;
        PdfTemplate _template;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// PieChart
        /// </summary>
        public PieChart()
        {
            setDefaults();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// PieChart's area background color. Its default value is BaseColor.GRAY.
        /// </summary>
        public BaseColor BackgroundColor { set; get; }

        /// <summary>
        /// PieChart's area border color. Its default value is BaseColor.DARK_GRAY.
        /// </summary>
        public BaseColor BorderColor { set; get; }

        /// <summary>
        /// Label's bullet height. Its default value is 7.
        /// </summary>
        public float BulletHeight { set; get; }

        /// <summary>
        /// Label's bullet width. Its default value is 7.
        /// </summary>
        public float BulletWidth { set; get; }

        /// <summary>
        /// Drawing canvas
        /// </summary>
        public PdfContentByte ContentByte { set; get; }

        /// <summary>
        /// RTL or LTR. Its default value is LTR.
        /// </summary>
        public PdfRunDirection Direction { set; get; }

        /// <summary>
        /// PieChart's Margin from the edges. Its default value is 10.
        /// </summary>
        public float Margin { set; get; }

        /// <summary>
        /// Labels font. Use GenericFontProvider class to initialize it.
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// PieChart's Radius. Its default value is 50f.
        /// </summary>
        public float Radius { set; get; }

        /// <summary>
        /// Segments of PieChart to draw.
        /// </summary>
        public IEnumerable<PieChartSegment> Segments { set; get; }

        #endregion Properties

        #region Methods

        // Public Methods

        /// <summary>
        /// Draws a PieChart.
        /// This method returns a vector image which can be painted on a table's cell or part of a page.
        /// </summary>
        /// <returns>An instance of iTextSharp.text.pdf.Image</returns>
        public Image Draw()
        {
            sanityCheck();
            calculateSegmentsAngles();
            initValues();
            createTemplate();
            drawArea();
            drawCircle();
            drawSegments();
            restoreStates();
            return Image.GetInstance(_template);
        }
        // Private Methods

        private void calculateSegmentsAngles()
        {
            var total = Segments.Sum(x => x.Value);
            foreach (var segment in Segments)
            {
                segment.Angle = (float)(segment.Value * 360 / total);
            }
        }

        private void createTemplate()
        {
            ContentByte.SaveState();
            _template = ContentByte.CreateTemplate(_initialChartWidth, _initialChartHeight);
            _template.SaveState();
        }

        private void drawArea()
        {
            _template.SetColorStroke(BorderColor);
            _template.SetColorFill(BackgroundColor);
            _template.Rectangle(0, 0, _template.Width, _template.Height);
            _template.FillStroke();
        }

        private void drawCircle()
        {
            _template.SetLineWidth(1f);
            _template.Circle(_circleCenterX0, _circleCenterY0, _circleRadius);

            _template.SetColorStroke(BaseColor.Gray);
            _template.Stroke();
            _template.SetLineWidth(0f);
        }

        private void drawLabel(PieChartSegment segment)
        {
            _template.SetColorStroke(segment.Color);
            _template.SetColorFill(segment.Color);
            _template.Rectangle(_circleCenterX0 + Radius + Margin, _labelY, BulletWidth, BulletHeight);
            _template.FillStroke();

            ColumnText.ShowTextAligned(
                canvas: _template,
                alignment: Element.ALIGN_LEFT,
                phrase: PdfFont.FontSelector.Process(segment.Label),
                x: _circleCenterX0 + Radius + (2 * Margin),
                y: _labelY,
                rotation: 0,
                runDirection: (int)Direction,
                arabicOptions: 0);

            _labelY -= _labelRowsGap;
        }

        private void drawSegment(PieChartSegment segment)
        {
            var x1OnCircle = (float)(_circleCenterX0 + Radius * Math.Cos(_startAngle * Math.PI / 180));
            var y1OnCircle = (float)(_circleCenterY0 + Radius * Math.Sin(_startAngle * Math.PI / 180));
            _endAngle += segment.Percentage <= 50 ? segment.Angle : 180;
            var x2 = (float)(_circleCenterX0 + Radius * Math.Cos(_endAngle * Math.PI / 180));
            var y2 = (float)(_circleCenterY0 + Radius * Math.Sin(_endAngle * Math.PI / 180));

            //draw the triangle within the circle
            _template.SetColorStroke(segment.Color);
            _template.SetColorFill(segment.Color);
            _template.MoveTo(_circleCenterX0, _circleCenterY0);
            _template.LineTo(x1OnCircle, y1OnCircle);
            _template.LineTo(x2, y2);
            _template.LineTo(_circleCenterX0, _circleCenterY0);
            _template.FillStroke();

            //draw the arc
            _template.Arc(
                _circleCenterX0 - Radius,
                _circleCenterY0 - Radius,
                _circleCenterX0 + Radius,
                _circleCenterY0 + Radius,
                _startAngle,
                segment.Percentage <= 50 ? segment.Angle : 180);
            _template.FillStroke();
        }

        private void drawSegments()
        {
            foreach (var segment in Segments)
            {
                drawLabel(segment);

                if (segment.Percentage.ApproxEquals(0))
                    continue;

                drawSegment(segment);
                drawWideSegment(segment);

                _startAngle += segment.Angle;
            }
        }

        private void drawWideSegment(PieChartSegment segment)
        {
            if (!(segment.Percentage > 50))
            {
                return;
            }

            var triangleX1OnCircle = (float)(_circleCenterX0 + Radius * Math.Cos((_startAngle + 180) * Math.PI / 180));
            var triangleY1OnCircle = (float)(_circleCenterY0 + Radius * Math.Sin((_startAngle + 180) * Math.PI / 180));

            _endAngle += segment.Angle - 180;
            var triangleX2OnCircle = (float)(_circleCenterX0 + Radius * Math.Cos(_endAngle * Math.PI / 180));
            var triangleY2OnCircle = (float)(_circleCenterY0 + Radius * Math.Sin(_endAngle * Math.PI / 180));

            _template.SetColorStroke(segment.Color);
            _template.SetColorFill(segment.Color);

            //draw the triangle within the circle
            _template.MoveTo(_circleCenterX0, _circleCenterY0);
            _template.LineTo(triangleX1OnCircle, triangleY1OnCircle);
            _template.LineTo(triangleX2OnCircle, triangleY2OnCircle);
            _template.LineTo(_circleCenterX0, _circleCenterY0);
            _template.FillStroke();

            //draw the arc
            _template.Arc(_circleCenterX0 - Radius, _circleCenterY0 - Radius, _circleCenterX0 + Radius, _circleCenterY0 + Radius, _startAngle + 180, segment.Angle - 180);
            _template.FillStroke();
        }

        private float getMaxTextHeight()
        {
            var segment = Segments.OrderByDescending(x => x.Label.Length).First();
            var firstFont = PdfFont.Fonts[0];
            var baseFont = firstFont.BaseFont;
            var ascend = baseFont.GetAscentPoint(segment.Label, PdfFont.Size);
            var descend = baseFont.GetDescentPoint(segment.Label, PdfFont.Size);
            var maxTextHeight = ascend - descend;
            if (maxTextHeight.ApproxEquals(0))
            {
                throw new InvalidOperationException($"maxTextHeight with {firstFont.Familyname} is zero.");
            }
            return maxTextHeight;
        }

        private float getMaxTextWidth()
        {
            var segment = Segments.OrderByDescending(x => x.Label.Length).First();
            var width = PdfFont.Fonts.First().BaseFont.GetWidthPoint(segment.Label, PdfFont.Size);
            return width;
        }

        private void initValues()
        {
            _startAngle = 0;
            _endAngle = 0;

            _circleRadius = Radius + 0.5f;
            _circleCenterX0 = _circleRadius + Margin;

            _labelRowsGap = getMaxTextHeight() * 1.5f;
            _labelY = (Segments.Count() * _labelRowsGap) + (2 * Margin);

            _initialChartWidth = getMaxTextWidth() + _circleCenterX0 + Radius + Margin + BulletWidth + (2 * Margin);

            _initialChartHeight = _labelY + (2 * Margin);
            var circleHeight = ((2 * _circleRadius) + (2 * Margin));
            if (circleHeight > _initialChartHeight)
            {
                _initialChartHeight = circleHeight;
            }

            _circleCenterY0 = _initialChartHeight / 2;
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

            if (Segments == null)
                throw new NullReferenceException("`Segments` is null.");

            if (!Segments.Any())
                throw new InvalidOperationException("There is no PieChartSegment to draw.");
        }

        private void setDefaults()
        {
            Radius = 50f;
            BulletHeight = 7;
            BulletWidth = 7;
            Margin = 10;
            Direction = PdfRunDirection.LeftToRight;
            BorderColor = BaseColor.DarkGray;
            BackgroundColor = BaseColor.Gray;
        }

        #endregion Methods
    }
}