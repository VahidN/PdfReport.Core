using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Encryption Preferences Class.
    /// </summary>
    public class EncryptedFileBuilder
    {
        readonly Encryption _encryptionPreferences;

        /// <summary>
        /// ctor.
        /// </summary>
        public EncryptedFileBuilder()
        {
            _encryptionPreferences = new Encryption(); 
        }

        /// <summary>
        /// Gets the encryption options for this document.
        /// </summary>
        internal Encryption EncryptionPreferences
        {
            get { return _encryptionPreferences; }
        }

        /// <summary>
        /// Using the AES algorithm to encrypt the pdf file.
        /// </summary>
        /// <param name="editPassword">The owner password. It can't be null, otherwise these settings will be ignored.</param>
        /// <param name="readPassword">The user password. It can be null.</param>
        public void WithPassword(string editPassword, string readPassword = "")
        {
            _encryptionPreferences.EncryptionType = EncryptionType.PasswordEncryption;
            _encryptionPreferences.PasswordEncryption = new PasswordEncryption
            {
                ReadPassword = readPassword,
                EditPassword = editPassword
            };
        }

        /// <summary>
        /// Document Permissions.
        /// </summary>
        /// <param name="documentPermissions">Document Permissions.</param>
        public void WithPermissions(DocumentPermissions documentPermissions)
        {
            _encryptionPreferences.DocumentPermissions = documentPermissions;
        }

        /// <summary>
        /// Using a personal information exchange file to encrypt the pdf file.
        /// </summary>
        /// <param name="pfxPassword">Certificate file's password</param>
        /// <param name="pfxPath">Certificate file's path</param>
        public void WithPublicKey(string pfxPassword, string pfxPath)
        {
            _encryptionPreferences.EncryptionType = EncryptionType.PublicKeyEncryption;
            _encryptionPreferences.PublicKeyEncryption = new PublicKeyEncryption
            {
                PfxPassword = pfxPassword,
                PfxPath = pfxPath
            };
        }
    }
}
