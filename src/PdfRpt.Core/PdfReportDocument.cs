using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.Core.PdfTable;

namespace PdfRpt
{
    /// <summary>
    /// PDF report generator class
    /// </summary>
    public class PdfReportDocument
    {
        PdfConformance _pdfConformance;
        DocumentSettings _pdfDocumentSettings;
        IPdfReportData _pdfRptData;
        RenderMainTable _pdfRptRenderMainTable;
        Stream _stream;

        /// <summary>
        /// ctor
        /// </summary>
        public PdfReportDocument()
        {
            LastRenderedRowData = new LastRenderedRowData();
        }

        /// <summary>
        /// Summary cells data of the main table's columns
        /// </summary>
        public IList<SummaryCellData> ColumnSummaryCellsData { private set; get; }

        /// <summary>
        /// Holds last result of the actual rendering engine of iTextSharp during its processes.
        /// </summary>
        public LastRenderedRowData LastRenderedRowData { private set; get; }

        /// <summary>
        /// It sets PdfStreamOutput to a new MemoryStream() and then returns the content as a byte array.
        /// </summary>
        public bool OutputAsByteArray { set; get; }

        /// <summary>
        /// Close the document by closing the underlying stream. Its default value is true.
        /// If you want to access the PDF stream after it has been created, set it to false.
        /// </summary>
        public bool CloseStream { set; get; } = true;

        /// <summary>
        /// PDF Document object
        /// </summary>
        public Document PdfDoc { get; private set; }

        /// <summary>
        /// Reports' definition data
        /// </summary>
        public IPdfReportData PdfRptData
        {
            get { return _pdfRptData; }
            set { _pdfRptData = value; }
        }

        /// <summary>
        /// PdfWriter object
        /// </summary>
        public PdfWriter PdfWriter { get; private set; }

        // Public Methods (1)

        /// <summary>
        /// Start generating the report based on the PdfRptData
        /// </summary>
        public byte[] GeneratePdf()
        {
            checkNullValues();

            byte[] data;
            try
            {
                PdfDoc = new Document(DocumentSettings.GetPageSizeAndColor(_pdfRptData.DocumentPreferences),
                            _pdfRptData.DocumentPreferences.PagePreferences.Margins.Left,
                            _pdfRptData.DocumentPreferences.PagePreferences.Margins.Right,
                            _pdfRptData.DocumentPreferences.PagePreferences.Margins.Top,
                            _pdfRptData.DocumentPreferences.PagePreferences.Margins.Bottom);

                data = createPdf();
            }
            finally
            {
                if (!CloseStream)
                {
                    // close the document without closing the underlying stream
                    PdfWriter.CloseStream = false;
                }

                PdfDoc?.Close();
                if (PdfWriter != null && CloseStream)
                {
                    PdfWriter.CloseStream = true;
                    PdfWriter.Close();
                    PdfWriter = null;
                }

                if (CloseStream)
                {
                    _stream?.Dispose();
                }
                else
                {
                    _pdfRptData.PdfStreamOutput.Position = 0;
                }
            }
            return data;
        }

        private void addMainTable()
        {
            _pdfRptRenderMainTable = new RenderMainTable
            {
                PdfRptData = _pdfRptData,
                ColumnSummaryCellsData = ColumnSummaryCellsData,
                PdfDoc = PdfDoc,
                PdfWriter = PdfWriter,
                CurrentRowInfoData = LastRenderedRowData
            };
            _pdfRptRenderMainTable.AddToDocument();
        }

        private void checkNullValues()
        {
            if (_pdfRptData.DocumentPreferences.PagePreferences.Margins == null)
            {
                _pdfRptData.DocumentPreferences.PagePreferences.Margins = new DocumentMargins
                {
                    Bottom = 50,
                    Left = 36,
                    Right = 36,
                    Top = 36
                };
            }

            if (_pdfRptData.DocumentPreferences.PagePreferences.Size == null)
            {
                _pdfRptData.DocumentPreferences.PagePreferences.Size = PageSize.A4;
            }
        }

        private byte[] createPdf()
        {
            if (OutputAsByteArray)
                _pdfRptData.PdfStreamOutput = new MemoryStream();

            _stream = _pdfRptData.PdfStreamOutput;
            initPdfWriter(_stream);
            initSettings();
            _pdfDocumentSettings.ApplyBeforePdfDocOpenSettings();
            _pdfDocumentSettings.SetEncryption();
            PdfDoc.Open();
            _pdfConformance.SetColorProfile();

            if (_pdfRptData.MainTableEvents != null)
                _pdfRptData.MainTableEvents.DocumentOpened(new EventsArguments { PdfDoc = PdfDoc, PdfWriter = PdfWriter, ColumnCellsSummaryData = ColumnSummaryCellsData, PageSetup = _pdfRptData.DocumentPreferences, PdfFont = _pdfRptData.PdfFont, PdfColumnsAttributes = _pdfRptData.PdfColumnsAttributes });

            _pdfDocumentSettings.ApplySettings();
            _pdfDocumentSettings.AddFileAttachments();
            addMainTable();
            _pdfDocumentSettings.ApplySignature(_stream);

            if (_pdfRptData.MainTableEvents != null)
                _pdfRptData.MainTableEvents.DocumentClosing(new EventsArguments { PdfDoc = PdfDoc, PdfWriter = PdfWriter, PdfStreamOutput = _stream, ColumnCellsSummaryData = ColumnSummaryCellsData, PageSetup = _pdfRptData.DocumentPreferences, PdfFont = _pdfRptData.PdfFont, PdfColumnsAttributes = _pdfRptData.PdfColumnsAttributes });

            return flushFileInBrowser();
        }

        private byte[] flushFileInBrowser()
        {
            if (!OutputAsByteArray)
                return null;

            // close the document without closing the underlying stream
            PdfWriter.CloseStream = false;
            PdfDoc.Close();
            _pdfRptData.PdfStreamOutput.Position = 0;

            // write pdf bytes to output stream
            var pdf = ((MemoryStream)_pdfRptData.PdfStreamOutput).ToArray();
            return pdf;
        }

        private void initPdfWriter(Stream stream)
        {
            PdfWriter = PdfWriter.GetInstance(PdfDoc, stream);

            var pageEvents = new PageEvents
            {
                PdfRptHeader = _pdfRptData.Header,
                PageSetup = _pdfRptData.DocumentPreferences,
                PdfRptFooter = _pdfRptData.Footer,
                CurrentRowInfoData = LastRenderedRowData,
                ColumnSummaryCellsData = ColumnSummaryCellsData,
                MainTableEvents = _pdfRptData.MainTableEvents,
                PdfFont = _pdfRptData.PdfFont,
                PdfColumnsAttributes = _pdfRptData.PdfColumnsAttributes
            };
            PdfWriter.PageEvent = pageEvents;
            _pdfConformance = new PdfConformance { PdfWriter = PdfWriter, PageSetup = _pdfRptData.DocumentPreferences };
            _pdfConformance.SetConformanceLevel();
        }

        private void initSettings()
        {
            _pdfDocumentSettings = new DocumentSettings
            {
                DocumentSecurity = _pdfRptData.DocumentSecurity,
                PageSetup = _pdfRptData.DocumentPreferences,
                PdfDoc = PdfDoc,
                PdfWriter = PdfWriter,
                DocumentProperties = _pdfRptData.DocumentPreferences.DocumentMetadata
            };
        }
    }
}