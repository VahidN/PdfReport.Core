using System;
using iTextSharp.text;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Basic properties of the main table's cells
    /// </summary>
    public class CellBasicProperties
    {
        /// <summary>
        /// Height of each row will be calculated automatically based on its content. 
        /// Also you can set the FixedHeight to define the height yourself.
        /// In this case the overflowed text with be trimmed. 
        /// Set FixedHeight to zero (its default value) to ignore this setting.
        /// </summary>
        public float FixedHeight { set; get; }

        /// <summary>
        /// Height of each row will be calculated automatically based on its content. 
        /// To assure a certain cell height, without losing any content, you can set the MinimumHeight.
        /// Set MinimumHeight to zero (its default value) to ignore this setting.
        /// </summary>
        public float MinimumHeight { set; get; }

        /// <summary>
        /// Cell's padding value
        /// </summary>
        public float CellPadding { set; get; }

        /// <summary>
        /// Cell's PaddingBottom value
        /// </summary>
        public float PaddingBottom { get; set; }

        /// <summary>
        /// Cell's PaddingLeft value
        /// </summary>
        public float PaddingLeft { get; set; }

        /// <summary>
        /// Cell's PaddingRight value
        /// </summary>
        public float PaddingRight { get; set; }

        /// <summary>
        /// Cell's PaddingTop value
        /// </summary>
        public float PaddingTop { get; set; }

        /// <summary>
        /// Cell's border width value
        /// </summary>
        public float BorderWidth { set; get; }

        /// <summary>
        /// Cell's font value
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// Cell's font's style value
        /// </summary>
        public DocumentFontStyle? PdfFontStyle { set; get; }

        /// <summary>
        /// Cell's content rotation angle value
        /// </summary>
        public int Rotation { set; get; }

        /// <summary>
        /// Cell's run direction value, LTR or RTL
        /// </summary>
        public PdfRunDirection? RunDirection { set; get; }

        /// <summary>
        /// Sets visibility of the cell's border
        /// </summary>
        public bool ShowBorder { set; get; }

        /// <summary>
        /// Cell's background color value
        /// </summary>
        public BaseColor BackgroundColor { set; get; }

        /// <summary>
        /// Cell's border color value
        /// </summary>
        public BaseColor BorderColor { set; get; }

        /// <summary>
        /// Cell's font color value
        /// </summary>
        public BaseColor FontColor { set; get; }

        /// <summary>
        /// Cell's template horizontal alignment  value
        /// </summary>
        public HorizontalAlignment? HorizontalAlignment { set; get; }

        /// <summary>
        /// Fires before each cell of this column is being rendered as a string. 
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public Func<object, string> DisplayFormatFormula { set; get; }
    }
}
