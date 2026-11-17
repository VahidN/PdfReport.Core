using System;

namespace PdfRpt.Core.Core.Helper
{
    /// <summary>
    /// System's fonts folder path
    /// </summary>
    public static class FontsDirPath
    {
        /// <summary>
        /// Returns System's fonts folder path
        /// </summary>
        public static string SystemFontsFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            }
        }
    }
}