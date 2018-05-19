using System;
using iTextSharp.text;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Core.Helper;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// A Generic Font Class
    /// </summary>
    public class GenericFontProviderBuilder
    {
        private BaseColor _color = new BaseColor(System.Drawing.Color.Black.ToArgb());
        private int _size = 9;
        private DocumentFontStyle _fontStyle = DocumentFontStyle.Normal;
        private string _font1 = System.IO.Path.Combine(FontsDirPath.SystemFontsFolder, "tahoma.ttf");
        private string _font2 = System.IO.Path.Combine(FontsDirPath.SystemFontsFolder, "arial.ttf");

        internal GenericFontProvider GenericFontProvider
        {
            get
            {
                return
                    new GenericFontProvider(_font1, _font2)
                    {
                        Color = _color,
                        Size = _size,
                        Style = _fontStyle
                    };
            }
        }

        /// <summary>
        /// Sets the fonts path. Its default value is tahoma.
        /// </summary>
        /// <param name="font1">Font1's path</param>
        /// <param name="font2">Font2's path</param>
        public void Path(string font1, string font2)
        {
            _font1 = font1;
            _font2 = font2;
        }

        /// <summary>
        /// Font's color. Its default value is black.
        /// </summary>
        public void Color(System.Drawing.Color value)
        {
            _color = new BaseColor(value.ToArgb());
        }

        /// <summary>
        /// Font's size. Its default value is 9.
        /// </summary>
        public void Size(int value)
        {
            _size = value;
        }

        /// <summary>
        /// Font's style. Its default value is Normal.
        /// </summary>
        public void Style(DocumentFontStyle value)
        {
            _fontStyle = value;
        }
    }
}