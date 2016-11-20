
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Holds the CertificateFile's info
    /// </summary>
    public class CertificateFile
    {
        /// <summary>
        /// Certificate file's password
        /// </summary>
        public string PfxPassword { set; get; }

        /// <summary>
        /// Certificate file's path
        /// </summary>
        public string PfxPath { set; get; }

        /// <summary>
        /// If sets to true the signature and all the other content will be added as a
        /// new revision thus not invalidating existing signatures.
        /// Set it to true if you don't want to lose the EncryptionOptions.
        /// </summary>
        public bool AppendSignature { set; get; }
    }
}
