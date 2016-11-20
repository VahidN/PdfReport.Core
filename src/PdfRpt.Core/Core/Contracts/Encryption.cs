
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Sets the encryption options for this document.
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// Using a personal information exchange file to encrypt athe pdf file.
        /// </summary>
        public PublicKeyEncryption PublicKeyEncryption { set; get; }

        /// <summary>
        /// Using the AES algorithm to encrypt the pdf file.
        /// </summary>
        public PasswordEncryption PasswordEncryption { set; get; }

        /// <summary>
        /// Sets the in use encryption algorithm.
        /// </summary>
        public EncryptionType EncryptionType { set; get; }

        /// <summary>
        /// Document Permissions.
        /// </summary>
        public DocumentPermissions DocumentPermissions { set; get; }
    }
}
