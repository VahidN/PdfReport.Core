
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Hold's digital signature's info.
    /// </summary>
    public class Signature
    {
        /// <summary>
        /// Sets the CertificateFile's info.
        /// </summary>
        public CertificateFile CertificateFile { set; get; }

        /// <summary>
        /// Sets the signing reason.
        /// </summary>
        public SigningInfo SigningInfo { set; get; }

        /// <summary>
        /// VisibleSignature's info.
        /// It can be null.
        /// </summary>
        public VisibleSignature VisibleSignature { set; get; }
    }
}
