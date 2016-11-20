using System;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// IPdfReportDataExporter Manager Class
    /// </summary>
    public class ExporterManager
    {
        readonly SharedData _sharedData;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="sharedData">holds shared info between rendering classes.</param>
        public ExporterManager(SharedData sharedData)
        {
            _sharedData = sharedData;
        }

        /// <summary>
        /// Calls exporter.OnRowAdded
        /// </summary>
        /// <param name="finalRowDataList">row's data</param>
        /// <param name="currentRowInfoData">row's info</param>
        public void ApplyExporter(IList<CellData> finalRowDataList, LastRenderedRowData currentRowInfoData)
        {
            if (_sharedData.PageSetup.ExportSettings == null) return;
            foreach (var exporter in _sharedData.PageSetup.ExportSettings)
            {
                exporter.RowAdded(finalRowDataList, currentRowInfoData.IsNewGroupStarted);
            }
        }

        /// <summary>
        /// Calls exporter.OnInitDocument.
        /// </summary>
        public void InitExporter()
        {
            if (_sharedData.PageSetup.ExportSettings == null) return;
            foreach (var exporter in _sharedData.PageSetup.ExportSettings)
            {
                exporter.DocumentOpened(_sharedData.PdfColumnsAttributes, _sharedData.PageSetup);
            }
        }

        /// <summary>
        /// Calls exporter.OnCloseDocument.
        /// </summary>
        public void CloseExporter()
        {
            if (_sharedData.PageSetup.ExportSettings == null) return;
            foreach (var exporter in _sharedData.PageSetup.ExportSettings)
            {
                var data = exporter.ClosingDocument();
                if (data == null) continue;
                addAsAttachment(exporter, data);
            }
        }

        private void addAsAttachment(IDataExporter exporter, byte[] data)
        {
            if (string.IsNullOrEmpty(exporter.FileName))
                throw new InvalidOperationException("Please fill the exporter.FileName.");

            if (string.IsNullOrEmpty(exporter.Description))
                exporter.Description = "Exported data";

            var pdfDictionary = new PdfDictionary();
            pdfDictionary.Put(PdfName.Moddate, new PdfDate(DateTime.Now));
            var fs = PdfFileSpecification.FileEmbedded(_sharedData.PdfWriter, null, exporter.FileName, data, true, null, pdfDictionary);
            _sharedData.PdfWriter.AddFileAttachment(exporter.Description, fs);
        }
    }
}
