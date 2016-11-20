
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Using the AES algorithm to encrypt the pdf file.
    /// </summary>
    public class PasswordEncryption
    {
        /// <summary>
        /// The user password.
        /// It can be null.
        /// </summary>
        public string ReadPassword { set; get; }

        /// <summary>
        /// The owner password.
        /// It can't be null, otherwise these settings will be ignored.
        /// </summary>
        public string EditPassword { set; get; }
    }
}
