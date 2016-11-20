using System.IO;
using PdfRpt.Core.Helper;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Pdf RptFile Builder Class.
    /// </summary>
    public class FileBuilder
    {
        readonly PdfReport _pdfReport;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public FileBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
        }

        /// <summary>
        /// Sets produced PDF file's path and name.
        /// It can be null if you are using an in memory stream.
        /// </summary>
        /// <param name="fileName">produced PDF file's path and name</param>
        public void AsPdfFile(string fileName)
        {
            fileName.CheckDirectoryExists();
            _pdfReport.DataBuilder.SetFileName(fileName);
        }

        /// <summary>
        /// Sets the PDF file's stream.
        /// It can be null. In this case a new FileStream will be used automatically and you need to provide the FileName.
        /// </summary>
        /// <param name="pdfStreamOutput">the PDF file's stream</param>
        public void AsPdfStream(Stream pdfStreamOutput)
        {
            _pdfReport.DataBuilder.SetStreamOutput(pdfStreamOutput);
        }

        /// <summary>
        /// Sets the output PDF file's byte array.
        /// </summary>
        /// <param name="outputPdfBytes">output data</param>
        public void AsByteArray(out byte[] outputPdfBytes)
        {
            outputPdfBytes = _pdfReport.GenerateAsByteArray();
        }
    }
}