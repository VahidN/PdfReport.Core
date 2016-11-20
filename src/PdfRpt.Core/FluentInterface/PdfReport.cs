using System;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// PdfReport Builder Class.
    /// </summary>
    public class PdfReport
    {
        internal DataBuilder DataBuilder { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public PdfReport()
        {
            DataBuilder = new DataBuilder();
        }

        /// <summary>
        /// Sets the documents's default fonts.
        /// </summary>
        /// <param name="pdfRptDefaultFontsBuilder"></param>
        /// <returns></returns>
        public PdfReport DefaultFonts(Action<DefaultFontsBuilder> pdfRptDefaultFontsBuilder)
        {
            var builder = new DefaultFontsBuilder(this);
            pdfRptDefaultFontsBuilder(builder);
            return this;
        }

        /// <summary>
        /// Gets the Main interface of PdfRpt
        /// </summary>
        public IPdfReportData PdfRptDataBuilder
        {
            get { return DataBuilder; }
        }

        /// <summary>
        /// Document settings
        /// </summary>
        /// <param name="pdfRptPagePreferencesBuilder">Document settings builder</param>
        /// <returns>PdfReport</returns>
        public PdfReport DocumentPreferences(Action<DocumentPreferencesBuilder> pdfRptPagePreferencesBuilder)
        {
            var builder = new DocumentPreferencesBuilder(this);
            pdfRptPagePreferencesBuilder(builder);
            return this;
        }

        /// <summary>
        /// MainTable's general properties
        /// </summary>
        /// <param name="pdfRptMainTablePreferencesBuilder">MainTable's general properties builder</param>
        /// <returns></returns>
        public PdfReport MainTablePreferences(Action<MainTablePreferencesBuilder> pdfRptMainTablePreferencesBuilder)
        {
            var builder = new MainTablePreferencesBuilder(this);
            pdfRptMainTablePreferencesBuilder(builder);
            return this;
        }

        /// <summary>
        /// Sets the desired exporters such as ExportToExcel.
        /// You can set multiple exporters here.
        /// </summary>
        /// <param name="pdfRptExportToBuilder">DataExporter Builder</param>
        /// <returns></returns>
        public PdfReport Export(Action<ExportToBuilder> pdfRptExportToBuilder)
        {
            var builder = new ExportToBuilder(this);
            pdfRptExportToBuilder(builder);
            return this;
        }

        /// <summary>
        /// Generates the final PDF file.
        /// </summary>
        /// <param name="pdfRptFileBuilder">Pdf RptFile Builder Settings</param>
        /// <returns>IPdfReportData</returns>
        public IPdfReportData Generate(Action<FileBuilder> pdfRptFileBuilder)
        {
            var builder = new FileBuilder(this);
            pdfRptFileBuilder(builder);

            var rpt = PdfRptDataBuilder;
            new PdfReportDocument
            {
                PdfRptData = rpt
            }.GeneratePdf();
            return rpt;
        }

        /// <summary>
        /// Generates the final PDF file.
        /// </summary>
        public byte[] GenerateAsByteArray()
        {
            var rpt = PdfRptDataBuilder;
            return new PdfReportDocument
            {
                PdfRptData = rpt,
                OutputAsByteArray = true
            }.GeneratePdf();
        }

        /// <summary>
        /// Sets the encryption preferences for this document.
        /// </summary>
        /// <param name="pdfRptEncryptedFileBuilder">encryption preferences builder</param>
        /// <returns></returns>
        public PdfReport Encrypt(Action<EncryptedFileBuilder> pdfRptEncryptedFileBuilder)
        {
            var builder = new EncryptedFileBuilder();
            pdfRptEncryptedFileBuilder(builder);

            if (DataBuilder.DocumentSecurity == null)
                DataBuilder.DocumentSecurity = new DocumentSecurity();

            DataBuilder.DocumentSecurity.EncryptionPreferences = builder.EncryptionPreferences;
            return this;
        }

        /// <summary>
        /// Sets ths digital signature's info.
        /// </summary>
        /// <param name="pdfRptSignedFileBuilder">digital signature's info builder</param>
        /// <returns></returns>
        public PdfReport Sign(Action<SignedFileBuilder> pdfRptSignedFileBuilder)
        {
            var builder = new SignedFileBuilder(this);
            pdfRptSignedFileBuilder(builder);

            if (DataBuilder.DocumentSecurity == null)
                DataBuilder.DocumentSecurity = new DocumentSecurity();

            DataBuilder.DocumentSecurity.DigitalSignature = builder.DigitalSignature;
            return this;
        }

        /// <summary>
        /// Sets the Main table's cells and rows events.
        /// </summary>
        /// <param name="mainTableEventsBuilder">Events Builder</param>
        /// <returns></returns>
        public PdfReport MainTableEvents(Action<MainTableEventsBuilder> mainTableEventsBuilder)
        {
            var builder = new MainTableEventsBuilder(this);
            mainTableEventsBuilder(builder);
            return this;
        }

        /// <summary>
        /// Sets the default template.
        /// </summary>
        /// <param name="mainTableTemplateBuilder">Template Builder</param>
        /// <returns></returns>
        public PdfReport MainTableTemplate(Action<MainTableTemplateBuilder> mainTableTemplateBuilder)
        {
            var builder = new MainTableTemplateBuilder(this);
            mainTableTemplateBuilder(builder);
            return this;
        }

        /// <summary>
        /// Defines dynamic headers for pages and individual groups.
        /// </summary>
        /// <param name="pagesHeaderBuilder">Pages Header Builder</param>
        /// <returns></returns>
        public PdfReport PagesHeader(Action<PagesHeaderBuilder> pagesHeaderBuilder)
        {
            var builder = new PagesHeaderBuilder(this);
            pagesHeaderBuilder(builder);
            return this;
        }

        /// <summary>
        /// Defines custom footer of the each page.
        /// </summary>
        /// <param name="pagesFooterBuilder">Pages Footer Builder</param>
        /// <returns></returns>
        public PdfReport PagesFooter(Action<PagesFooterBuilder> pagesFooterBuilder)
        {
            var builder = new PagesFooterBuilder(this);
            pagesFooterBuilder(builder);
            return this;
        }

        /// <summary>
        /// Main table's data source. The data to render.
        /// </summary>
        /// <param name="mainTableDataSourceBuilder">Data Source Builder</param>
        /// <returns></returns>
        public PdfReport MainTableDataSource(Action<MainTableDataSourceBuilder> mainTableDataSourceBuilder)
        {
            var builder = new MainTableDataSourceBuilder(this);
            mainTableDataSourceBuilder(builder);
            return this;
        }

        /// <summary>
        /// Pages and groups summary values settings
        /// </summary>
        /// <param name="mainTableSummarySettingsBuilder">Summary Settings Builder</param>
        /// <returns></returns>
        public PdfReport MainTableSummarySettings(Action<MainTableSummarySettingsBuilder> mainTableSummarySettingsBuilder)
        {
            var builder = new MainTableSummarySettingsBuilder();
            mainTableSummarySettingsBuilder(builder);
            DataBuilder.SetSummarySettings(builder.SummarySettings);
            return this;
        }

        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="mainTableColumnsBuilder">Columns Builder</param>
        /// <returns></returns>
        public PdfReport MainTableColumns(Action<MainTableColumnsBuilder> mainTableColumnsBuilder)
        {
            var builder = new MainTableColumnsBuilder();
            mainTableColumnsBuilder(builder);
            DataBuilder.SetColumnsAttributes(builder.PdfColumns);
            return this;
        }

        /// <summary>
        /// If you don't set PdfColumnsDefinitions, list of the main table's columns will be extracted from MainTableDataSource automatically.
        /// Here you can control how cells should be rendered based on their specific data types.
        /// </summary>
        /// <param name="mainTableAdHocColumnsConventionsBuilder">Conventions Builder</param>
        /// <returns></returns>
        public PdfReport MainTableAdHocColumnsConventions(Action<MainTableAdHocColumnsConventionsBuilder> mainTableAdHocColumnsConventionsBuilder)
        {
            var builder = new MainTableAdHocColumnsConventionsBuilder();
            mainTableAdHocColumnsConventionsBuilder(builder);
            DataBuilder.SetAdHocColumnsConventions(builder.PdfRptAdHocColumnsConventions);
            return this;
        }
    }
}