using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// A helper class to add a background image to each page of the current document
    /// </summary>
    public class BackgroundImageTemplate
    {
        PdfTemplate _backgroundImageTemplate;

        /// <summary>
        /// Document settings
        /// </summary>
        public DocumentPreferences PageSetup { set; get; }

        /// <summary>
        /// Apply background image to the added templates
        /// </summary>
        /// <param name="document">PDF Document</param>
        public void ApplyBackgroundImage(Document document)
        {
            var path = PageSetup.PagePreferences.BackgroundImageFilePath;
            var position = PageSetup.PagePreferences.BackgroundImagePosition;
            if (string.IsNullOrEmpty(path)) return;

            var img = path.GetITextSharpImageFromImageFile();
            img.Alignment = Image.UNDERLYING;
            if (position == null)
            {
                img.SetAbsolutePosition((document.PageSize.Width - img.Width) / 2,
                                        (document.PageSize.Height - img.Height) / 2);
            }
            else
            {
                img.SetAbsolutePosition(position.Value.X, position.Value.Y);
            }
            _backgroundImageTemplate.AddImage(img);
        }

        /// <summary>
        /// Create an empty template
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        /// <param name="document">PDF Document</param>
        public void InitBackgroundImageTemplate(PdfWriter writer, Document document)
        {
            if (string.IsNullOrEmpty(PageSetup.PagePreferences.BackgroundImageFilePath)) return;
            _backgroundImageTemplate = writer.DirectContent.CreateTemplate(document.PageSize.Width, document.PageSize.Height);
        }

        /// <summary>
        /// Add an empty template to each page
        /// </summary>
        /// <param name="writer">PdfWriter</param>
        public void ReserveBackgroundImageSpace(PdfWriter writer)
        {
            if (_backgroundImageTemplate == null) return;
            writer.DirectContent.AddTemplate(_backgroundImageTemplate, 0, 0);
        }
    }
}
