using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt
{
    /// <summary>
    /// A Generic Font Class
    /// </summary>
    public class GenericFontProvider : IPdfFont
    {
        readonly string _mainFontPath;
        readonly string _defaultFontPath;
        readonly Font _mainFont;
        readonly Font _defaultFont;

        /// <summary>
        /// Sets registered font's name.
        /// </summary>
        /// <param name="font">pdfRptFont should contain at least 2 fonts.</param>
        public GenericFontProvider(IPdfFont font)
        {
            _mainFont = font.Fonts[0];
            _defaultFont = font.Fonts[1];
            registerFonts(_mainFont.Familyname, _defaultFont.Familyname);
            setDefaults(font, _mainFont);
            setDefaults(font, _defaultFont);
            setDefaults(_mainFont);
        }

        private void setDefaults(Font font)
        {
            Color = font.Color;
            Size = (int)font.Size;
            Style = (DocumentFontStyle)font.Style;
        }

        private void setDefaults(IPdfFont baseFont, Font toFont)
        {
            toFont.Color = baseFont.Color;
            toFont.Size = baseFont.Size;
            toFont.SetStyle((int)baseFont.Style);
        }

        /// <summary>
        /// Sets registered font's name.
        /// </summary>
        /// <param name="mainFont">main font</param>
        /// <param name="defaultFont">default font</param>
        public GenericFontProvider(Font mainFont, Font defaultFont)
        {
            _mainFont = mainFont;
            _defaultFont = defaultFont;
            setDefaults(_mainFont);
            registerFonts(_mainFont.Familyname, _defaultFont.Familyname);
        }

        /// <summary>
        /// Sets registered font's name
        /// </summary>
        /// <param name="mainFontPath">main font's path</param>
        /// <param name="defaultFontPath">default font's path</param>
        public GenericFontProvider(string mainFontPath, string defaultFontPath)
        {
            mainFontPath.CheckDirectoryExists();
            mainFontPath.CheckFileExists();

            defaultFontPath.CheckDirectoryExists();
            defaultFontPath.CheckFileExists();

            registerFonts(mainFontPath, defaultFontPath);
            _mainFontPath = mainFontPath;
            _defaultFontPath = defaultFontPath;
            defaultValues();
        }

        private static void registerFonts(string mainFontPath, string defaultFontPath)
        {
            if (!FontFactory.IsRegistered(mainFontPath))
            {
                FontFactory.Register(mainFontPath);
            }

            if (!FontFactory.IsRegistered(defaultFontPath))
            {
                FontFactory.Register(defaultFontPath);
            }
        }

        private void defaultValues()
        {
            Color = BaseColor.Black;
            Size = 9;
            Style = DocumentFontStyle.Normal;
        }

        /// <summary>
        /// Font's color
        /// </summary>
        public BaseColor Color { get; set; }

        /// <summary>
        /// Font's size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Font's style
        /// </summary>
        public DocumentFontStyle Style { set; get; }

        /// <summary>
        /// Available fonts list
        /// </summary>
        public IList<Font> Fonts
        {
            get { return getFonts(); }
        }

        private IList<Font> getFonts()
        {
            if (!string.IsNullOrEmpty(_mainFontPath) && !string.IsNullOrEmpty(_defaultFontPath))
            {
                return new List<Font>
                {
                     FontFactory.GetFont(_mainFontPath, BaseFont.IDENTITY_H, true, Size, (int)Style, Color),
                     FontFactory.GetFont(_defaultFontPath, BaseFont.IDENTITY_H, true, Size, (int)Style, Color)
                };
            }

            return new List<Font>
            {
                FontFactory.GetFont(_mainFont.Familyname, BaseFont.IDENTITY_H, true, Size, (int)Style, Color),
                FontFactory.GetFont(_defaultFont.Familyname, BaseFont.IDENTITY_H, true, Size, (int)Style, Color)
            };
        }

        /// <summary>
        /// FontSelector will be used for processing the input text and creating the phrases
        /// </summary>
        public FontSelector FontSelector
        {
            get { return getFontSelector(); }
        }

        private FontSelector getFontSelector()
        {
            var fontSelector = new FontSelector();
            foreach (var font in Fonts)
            {
                fontSelector.AddFont(font);
            }
            return fontSelector;
        }
    }
}
