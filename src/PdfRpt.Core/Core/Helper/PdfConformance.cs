using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Applies PDF/A Conformance.
    /// </summary>
    public class PdfConformance
    {
        /// <summary>
        /// PdfWriter Object.
        /// </summary>
        public PdfWriter PdfWriter { get; set; }
        /// <summary>
        /// Document settings.
        /// </summary>
        public DocumentPreferences PageSetup { get; set; }

        /// <summary>
        /// Sets PDF/A Conformance Level.
        /// </summary>
        public void SetConformanceLevel()
        {
            if (PageSetup.ConformanceLevel == PdfXConformance.PDFXNONE)
            {
                // Sets the transparency blending colorspace to RGB.
                // The default blending colorspace is CMYK and will result in faded colors in the screen and in printing.
                PdfWriter.RgbTransparencyBlending = true;
                return;
            }

            if ((int)PageSetup.ConformanceLevel <= (int)PdfXConformance.PDFX32002)
            {
                PdfWriter.PdfxConformance = (int)PageSetup.ConformanceLevel;
            }
        }

        /// <summary>
        /// Sets PDF/A Conformance ColorProfile.
        /// </summary>
        public void SetColorProfile()
        {
            if (PageSetup.ConformanceLevel == PdfXConformance.PDFXNONE) return;

            var pdfDictionary = new PdfDictionary(PdfName.Outputintent);
            pdfDictionary.Put(PdfName.Outputconditionidentifier, new PdfString("sRGB IEC61966-2.1"));
            pdfDictionary.Put(PdfName.Info, new PdfString("sRGB IEC61966-2.1"));
            pdfDictionary.Put(PdfName.S, PdfName.GtsPdfa1);

            var profileStream = StreamHelper.GetResourceByName("PdfRpt.Core.Core.Helper.srgb.profile");
            var pdfICCBased = new PdfIccBased(IccProfile.GetInstance(profileStream));
            pdfICCBased.Remove(PdfName.Alternate);
            pdfDictionary.Put(PdfName.Destoutputprofile, PdfWriter.AddToBody(pdfICCBased).IndirectReference);

            PdfWriter.ExtraCatalog.Put(PdfName.Outputintents, new PdfArray(pdfDictionary));
        }
    }
}
