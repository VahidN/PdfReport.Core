
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Holds the signing related info
    /// </summary>
    public class SigningInfo
    {
        /// <summary>
        /// Sets the signing reason
        /// </summary>
        public string Reason { set; get; }

        /// <summary>
        /// Sets the signing contact
        /// </summary>
        public string Contact { set; get; }

        /// <summary>
        /// Sets the signing location
        /// </summary>
        public string Location { set; get; }
    }
}
