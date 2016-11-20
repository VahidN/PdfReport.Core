using iTextSharp.text.pdf;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Sets subsets of the PDF specification (ISO 15930-1 to ISO 15930-8) that promise
    /// predictable and consistent output for press printing.
    /// The PDF/A specification (ISO 19005-1:2005): Document Management—Electronic Document File Format
    /// for Long-Term Preservation.
    /// You can check the PDF/A conformance with the Preflight tool of Adobe Acrobat Pro for instance.
    /// In Acrobat X, Preflight is an option under the Print Production Tools panel.
    /// More info: http://en.wikipedia.org/wiki/PDF/A
    /// </summary>
    public enum PdfXConformance
    {
        /// <summary>
        /// Default value.
        /// </summary>
        PDFXNONE = PdfWriter.PDFXNONE,

        /// <summary>
        /// The main goal of PDF/X-1a is to support blind exchange of PDF documents.
        /// Blind exchange means you can deliver PDF documents to a print service provider
        /// with hardly any technical discussion.
        /// </summary>
        PDFX1A2001 = PdfWriter.PDFX1A2001,

        /// <summary>
        /// Sets the conformance to PDF/X-3.
        /// A PDF/X-3 file can also contain color-managed data.
        /// </summary>
        PDFX32002 = PdfWriter.PDFX32002
    }
}
