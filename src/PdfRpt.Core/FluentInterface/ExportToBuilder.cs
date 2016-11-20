using System.Collections.Generic;
using OfficeOpenXml.Table;
using PdfRpt.Core.Contracts;
using PdfRpt.Export;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// PdfRpt DataExporter Builder Class.
    /// </summary>
    public class ExportToBuilder
    {
        readonly PdfReport _pdfReport;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public ExportToBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;

            if (_pdfReport.DataBuilder.CustomExportSettings == null)
                _pdfReport.DataBuilder.CustomExportSettings = new List<IDataExporter>();
        }

        /// <summary>
        /// Microsoft Excel Worksheet DataExporter
        /// </summary>
        /// <param name="description">the produced file's description</param>
        /// <param name="fileName">the produced file's name</param>
        /// <param name="worksheetName">the WorksheetName</param>
        /// <param name="footer">Footer's Text</param>
        /// <param name="header">Header's Text</param>
        /// <param name="numberformat">Number format such as #,##0</param>
        /// <param name="dateTimeFormat">DateTime Format such as yyyy-MM-dd hh:mm</param>
        /// <param name="timeSpanFormat">TimeSpan Format such as hh:mm:ss</param>
        /// <param name="pageLayoutView">Sets the view mode of the worksheet to PageLayout.</param>
        /// <param name="showGridLines">Show GridLines in the worksheet.</param>
        /// <param name="tableStyle">the produced table's style</param>
        public void ToExcel(
                     string description = "Exported Data",
                     string fileName = "data.xlsx",
                     string worksheetName = "worksheet1",
                     string footer = "",
                     string header = "",
                     string numberformat = "#,##0",
                     string dateTimeFormat = "yyyy-MM-dd hh:mm",
                     string timeSpanFormat = "hh:mm:ss",
                     bool pageLayoutView = false,
                     bool showGridLines = false,
                     TableStyles tableStyle = TableStyles.Dark9
                     )
        {
            var exportToExcel = new ExportToExcel
            {
                DateTimeFormat = dateTimeFormat,
                Description = description,
                FileName = fileName,
                Footer = footer,
                Header = header,
                Numberformat = numberformat,
                PageLayoutView = pageLayoutView,
                ShowGridLines = showGridLines,
                TableStyle = tableStyle,
                TimeSpanFormat = timeSpanFormat,
                WorksheetName = worksheetName
            };
            _pdfReport.DataBuilder.CustomExportSettings.Add(exportToExcel);
        }

        /// <summary>
        /// CSV DataExporter
        /// </summary>
        /// <param name="description">the produced file's description</param>
        /// <param name="fileName">the produced file's name</param> 
        public void ToCsv(string description = "Exported Data", string fileName = "data.csv")
        {
            var exportToCsv = new ExportToCsv
            {
                Description = description,
                FileName = fileName
            };
            _pdfReport.DataBuilder.CustomExportSettings.Add(exportToCsv);
        }

        /// <summary>
        /// XML DataExporter
        /// </summary>
        /// <param name="description">the produced file's description</param>
        /// <param name="fileName">the produced file's name</param>
        public void ToXml(string description = "Exported Data", string fileName = "data.xml")
        {
            var exportToXml = new ExportToXml
            {
                Description = description,
                FileName = fileName
            };
            _pdfReport.DataBuilder.CustomExportSettings.Add(exportToXml);
        }

        /// <summary>
        /// Sets the desired exporters such as ExportToExcel.
        /// </summary>
        /// <param name="exportSettings">export settings</param>
        public void ToCustomFormat(IDataExporter exportSettings)
        {
            _pdfReport.DataBuilder.CustomExportSettings.Add(exportSettings);
        }
    }
}
