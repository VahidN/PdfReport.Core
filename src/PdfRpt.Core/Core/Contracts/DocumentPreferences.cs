using System.Collections.Generic;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Document settings
    /// </summary>
    public class DocumentPreferences
    {
        /// <summary>
        /// Pages Preferences
        /// </summary>
        public PagePreferences PagePreferences { set; get; }

        /// <summary>
        /// Wrapping the main table in multiple columns per page. 
        /// Leave it as null if you don't want to use it.
        /// </summary>
        public MultipleColumnsPerPage MultipleColumnsPerPage { set; get; }

        /// <summary>
        /// Add a diagonal watermark text to each page
        /// Leave it as null if you don't want to use it.
        /// </summary>
        public DiagonalWatermark DiagonalWatermark { set; get; }

        /// <summary>
        /// Holds MainTable's general properties.
        /// </summary>
        public MainTablePreferences MainTablePreferences { set; get; }

        /// <summary>
        /// Indicates default values of print dialog box.
        /// </summary>
        public PrintingPreferences PrintingPreferences { set; get; }

        /// <summary>
        /// Specifies the Adobe Reader's settings when a document is opened.
        /// </summary>
        public PdfViewerPreferences ViewerPreferences { set; get; }

        /// <summary>
        /// Pdf document's meta-data properties
        /// </summary>
        public DocumentMetadata DocumentMetadata { get; set; }

        /// <summary>
        /// Adds the optional file attachments at the document level.
        /// To make The attachments panel visible automatically, Set the DocumentPreferences.ViewerPreferences.PageMode to ViewerPageMode.UseAttachments.
        /// </summary>
        public IList<FileAttachment> FileAttachments { get; set; }

        /// <summary>
        /// Applies Compression. It can be null.
        /// </summary>
        public CompressionSettings CompressionSettings { set; get; }

        /// <summary>
        /// Sets the desired exporters such as ExportToExcel.
        /// It can be null.
        /// </summary>
        public IList<IDataExporter> ExportSettings { get; set; }

        /// <summary>
        /// Groups preferences.
        /// </summary>
        public GroupsPreferences GroupsPreferences { set; get; }

        /// <summary>
        /// Promises predictable and consistent output for press printing.
        /// Choose None (default value), if you want a shiny PDF with transparent images
        /// and the document should be encrypted or needs embedded files.
        /// </summary>
        public PdfXConformance ConformanceLevel { set; get; }
    }
}
