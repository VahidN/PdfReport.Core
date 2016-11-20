
namespace PdfRpt.Core.Contracts
{    
    /// <summary>
    /// Possible cells horizontal alignment values
    /// </summary>    
    public enum HorizontalAlignment
    {
        /// <summary>
        /// Undefined value.
        /// </summary>
        None = -10,

        /// <summary>
        /// A possible value for paragraph Element.  This specifies that the text is aligned to the center
        /// and extra whitespace should be placed equally on the left and right.
        /// </summary>
        Center = 1,

        /// <summary>
        /// A possible value for paragraph Element. This specifies that the text is aligned to the left
        /// indent and extra whitespace should be placed on the right.
        /// </summary>
        Undefined = -1,

        /// <summary>
        /// A possible value for paragraph Element. This specifies that the text is aligned to the left
        /// indent and extra whitespace should be placed on the right.
        /// </summary>
        Left = 0,

        /// <summary>
        /// A possible value for paragraph Element. This specifies that the text is aligned to the right
        /// indent and extra whitespace should be placed on the left.
        /// </summary>
        Right = 2,

        /// <summary>
        /// A possible value for paragraph Element. This specifies that extra whitespace should be spread
        /// out through the rows of the paragraph with the text lined up with the left and right indent
        /// except on the last line which should be aligned to the left.
        /// </summary>
        Justified = 3,

        /// <summary>
        /// Does the same as Justified but the last line is also spread out.
        /// </summary>
        JustifiedAll = 8
    }
}
