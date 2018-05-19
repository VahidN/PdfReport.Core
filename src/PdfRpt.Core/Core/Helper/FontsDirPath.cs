using System;
using System.IO;
#if NETSTANDARD1_6
using System.Runtime.InteropServices;
#endif

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
#if NETSTANDARD1_6
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    var home = Environment.GetEnvironmentVariable("HOME");
                    return Path.Combine(home, "Library", "Fonts");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var home = Environment.GetEnvironmentVariable("HOME");
                    return Path.Combine(home, ".fonts");
                }
                else
                {
                    throw new PlatformNotSupportedException();
                }
#else
                return Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
#endif
            }
        }
    }
}