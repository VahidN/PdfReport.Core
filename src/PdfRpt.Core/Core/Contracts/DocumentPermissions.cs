
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Document Permissions
    /// </summary>
    public class DocumentPermissions
    {
        /// <summary>
        /// This setting will be ignored, if no EncryptionType is used or at least EditPassword is not set (ReadPassword can be null).
        /// </summary>
        public bool AllowPrinting { set; get; }

        /// <summary>
        /// This setting will be ignored, if no EncryptionType is used or at least EditPassword is not set (ReadPassword can be null).
        /// </summary>
        public bool AllowModifyContents { set; get; }

        /// <summary>
        /// This setting will be ignored, if no EncryptionType is used or at least EditPassword is not set (ReadPassword can be null).
        /// </summary>
        public bool AllowCopy { set; get; }

        /// <summary>
        /// This setting will be ignored, if no EncryptionType is used or at least EditPassword is not set (ReadPassword can be null).
        /// </summary>
        public bool AllowModifyAnnotations { set; get; }

        /// <summary>
        /// This setting will be ignored, if no EncryptionType is used or at least EditPassword is not set (ReadPassword can be null).
        /// </summary>
        public bool AllowFillIn { set; get; }

        /// <summary>
        /// This setting will be ignored, if no EncryptionType is used or at least EditPassword is not set (ReadPassword can be null).
        /// </summary>
        public bool AllowScreenReaders { set; get; }

        /// <summary>
        /// This setting will be ignored, if no EncryptionType is used or at least EditPassword is not set (ReadPassword can be null).
        /// </summary>
        public bool AllowAssembly { set; get; }

        /// <summary>
        /// This setting will be ignored, if no EncryptionType is used or at least EditPassword is not set (ReadPassword can be null).
        /// </summary>
        public bool AllowDegradedPrinting { set; get; }
    }
}
