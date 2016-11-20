
namespace PdfRpt.Core.Contracts
{    
    /// <summary>
    /// Possible font styles
    /// </summary>    
    public enum DocumentFontStyle
    {
        /// <summary>
        /// Undefined value.
        /// </summary>
        None = -1,

        /// <summary>
        /// This is a possible font style.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// This is a possible font style.
        /// </summary>
        Bold = 1,

        /// <summary>
        /// This is a possible font style.
        /// </summary>
        Italic = 2,

        /// <summary>
        /// This is a possible font style.
        /// </summary>
        Underline = 4,

        /// <summary>
        /// This is a possible font style.
        /// </summary>         
        Strikethru = 8,

        /// <summary>
        /// This is a possible font style.
        /// </summary>
        BoldItalic = Bold | Italic
    }
}
