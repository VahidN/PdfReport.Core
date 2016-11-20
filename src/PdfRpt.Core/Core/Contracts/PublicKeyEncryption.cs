
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Using a personal information exchange file to encrypt the pdf file.
    /// </summary>
    public class PublicKeyEncryption
    {
        /// <summary>
        /// Certificate file's password
        /// </summary>
        public string PfxPassword { set; get; }

        /// <summary>
        /// Certificate file's path
        /// </summary>
        public string PfxPath { set; get; }
    }
}
