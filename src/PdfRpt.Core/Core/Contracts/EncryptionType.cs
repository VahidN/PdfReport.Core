
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// EncryptionType Values
    /// </summary>
    public enum EncryptionType
    {
        /// <summary>
        /// Using the AES algorithm to encrypt the pdf file.
        /// </summary>
        PasswordEncryption,

        /// <summary>
        /// Using a personal information exchange file to encrypt athe pdf file.
        /// </summary>
        PublicKeyEncryption
    }
}
