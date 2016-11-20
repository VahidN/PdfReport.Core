using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Custom font's definitions
    /// </summary>
    public interface IPdfFont
    {
        #region Data Members (5)

        /// <summary>
        /// Font's color
        /// </summary>
        BaseColor Color { set; get; }

        /// <summary>
        /// You need to define at least 2 fonts.
        /// First one will be used as the main font and second one, default font.
        /// Sometimes first font has not the necessary data to display the current character, 
        /// in this case, 2nd font will be used automatically.
        /// </summary>
        IList<Font> Fonts { get; }

        /// <summary>
        /// FontSelector will be used for processing the input text and creating the phrases
        /// </summary>
        FontSelector FontSelector { get; }

        /// <summary>
        /// Font's size
        /// </summary>
        int Size { set; get; }

        /// <summary>
        /// Font's style
        /// </summary>
        DocumentFontStyle Style { set; get; }

        #endregion Data Members
    }
}
