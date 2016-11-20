namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Supported PDF versions values. Select at least PDF version 1.5, If you want the best compression support.
    /// http://en.wikipedia.org/wiki/Portable_Document_Format
    /// </summary>
    public enum PdfVersion
    {
        /// <summary>
        /// Adobe Reader 3.0+ supports PDF version 1.2
        /// </summary>
        Version12 = 1,

        /// <summary>
        /// Adobe Reader 4.0+ supports PDF version 1.3
        /// </summary>
        Version13 = 2,

        /// <summary>
        /// Adobe Reader 5.0+ supports PDF version 1.4
        /// </summary>
        Version14 = 3,

        /// <summary>
        /// Adobe Reader 6.0+ supports PDF version 1.5
        /// </summary>
        Version15 = 0,

        /// <summary>
        /// Adobe Reader 7.0+ supports PDF version 1.6
        /// </summary>
        Version16 = 4,

        /// <summary>
        /// Adobe Reader 8.0+ supports PDF version 1.7
        /// </summary>
        Version17 = 5
    }
}
