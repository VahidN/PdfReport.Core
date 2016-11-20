
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Sets the encryption options for this document.
    /// </summary>
    public class DocumentSecurity
    {
        /// <summary>
        /// Sets the encryption preferences for this document.
        /// It can be null.
        /// </summary>
        public Encryption EncryptionPreferences { set; get; }

        /// <summary>
        /// Sets the digital signature's info.
        /// It can be null.
        /// </summary>
        public Signature DigitalSignature { set; get; }
    }
}
