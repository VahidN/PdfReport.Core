using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.FunctionalTests.Templates
{
    public class GradientTestTemplate : ITableTemplate
    {
        public HorizontalAlignment HeaderHorizontalAlignment
        {
            get { return HorizontalAlignment.Center; }
        }

        public BaseColor AlternatingRowBackgroundColor
        {
            get { return new BaseColor(Color.WhiteSmoke.ToArgb()); }
        }

        public BaseColor CellBorderColor
        {
            get { return new BaseColor(Color.LightGray.ToArgb()); }
        }

        public IList<BaseColor> HeaderBackgroundColor
        {
            get
            {
                return new List<BaseColor>
                {
                    new BaseColor(ColorTranslator.FromHtml("#990000").ToArgb()),
                    new BaseColor(ColorTranslator.FromHtml("#e80000").ToArgb())
                };
            }
        }

        public BaseColor RowBackgroundColor
        {
            get { return null; }
        }

        public IList<BaseColor> PreviousPageSummaryRowBackgroundColor
        {
            get
            {
                return new List<BaseColor>
                {
                    new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()),
                    new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())
                };
            }
        }

        public IList<BaseColor> SummaryRowBackgroundColor
        {
            get
            {
                return new List<BaseColor>
                {
                    new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()),
                    new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())
                };
            }
        }

        public IList<BaseColor> PageSummaryRowBackgroundColor
        {
            get
            {
                return new List<BaseColor>
                {
                    new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()),
                    new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())
                };
            }
        }

        public BaseColor AlternatingRowFontColor
        {
            get { return new BaseColor(ColorTranslator.FromHtml("#333333").ToArgb()); }
        }

        public BaseColor HeaderFontColor
        {
            get { return new BaseColor(Color.White.ToArgb()); }
        }

        public BaseColor RowFontColor
        {
            get { return new BaseColor(ColorTranslator.FromHtml("#333333").ToArgb()); }
        }

        public BaseColor PreviousPageSummaryRowFontColor
        {
            get { return new BaseColor(Color.Black.ToArgb()); }
        }

        public BaseColor SummaryRowFontColor
        {
            get { return new BaseColor(Color.Black.ToArgb()); }
        }

        public BaseColor PageSummaryRowFontColor
        {
            get { return new BaseColor(Color.Black.ToArgb()); }
        }

        public bool ShowGridLines
        {
            get { return true; }
        }
    }
}