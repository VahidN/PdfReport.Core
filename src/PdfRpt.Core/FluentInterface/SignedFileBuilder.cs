using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// digital signature's info builder class.
    /// </summary>
    public class SignedFileBuilder
    {
        readonly PdfReport _pdfReport;
        readonly Signature _digitalSignature;

        /// <summary>
        /// digital signature's info
        /// </summary>
        internal Signature DigitalSignature
        {
            get { return _digitalSignature; }
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public SignedFileBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
            _digitalSignature = new Signature();
        }

        /// <summary>
        /// Sets the CertificateFile's info.
        /// </summary>
        /// <param name="appendSignature">If sets to true the signature and all the other content will be added as a new revision thus not invalidating existing signatures. Set it to true if you don't want to lose the EncryptionOptions.</param>
        /// <param name="pfxPassword">Certificate file's password</param>
        /// <param name="pfxPath">Certificate file's path</param>
        public void WithCertificateFile(bool appendSignature, string pfxPassword, string pfxPath)
        {
            _digitalSignature.CertificateFile = new CertificateFile
            {
                AppendSignature = appendSignature,
                PfxPassword = pfxPassword,
                PfxPath = pfxPath
            };
        }

        /// <summary>
        /// Sets the signing related info.
        /// </summary>
        /// <param name="reason">Sets the signing reason</param>
        /// <param name="contact">Sets the signing contact</param>
        /// <param name="location">Sets the signing location</param>
        public void SigningInfo(string reason, string contact, string location)
        {
            _digitalSignature.SigningInfo = new SigningInfo
            {
                Reason = reason,
                Contact = contact,
                Location = location
            };
        }

        /// <summary>
        /// Sets VisibleSignature's info.
        /// It can be null.
        /// </summary>
        /// <param name="text">Sets the signature text identifying the signer.</param>
        /// <param name="useLastPageToShowSignature">If it sets to true, value of the Page property will be ignored.</param>
        /// <param name="position">Position and dimension of the field in the page.</param>
        /// <param name="runDirection">Possible run direction values, left-to-right or right-to-left</param>
        /// <param name="pageNumberToShowSignature">The page to place the field. The fist page is 1.</param>
        /// <param name="imagePath">Signature's image. It can be null.</param>
        public void VisibleSignature(string text, bool useLastPageToShowSignature, iTextSharp.text.Rectangle position, PdfRunDirection runDirection, int pageNumberToShowSignature = 1, string imagePath = null)
        {
            _digitalSignature.VisibleSignature = new VisibleSignature
            {
                CustomText = text,
                UseLastPageToShowSignature = useLastPageToShowSignature,
                Position = position,
                RunDirection = runDirection,
                Font = _pdfReport.DataBuilder.PdfFont,
                ImagePath = imagePath,
                PageNumberToShowSignature = pageNumberToShowSignature
            };
        }
    }
}
