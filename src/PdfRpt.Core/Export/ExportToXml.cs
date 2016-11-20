using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Export
{
    /// <summary>
    /// XML DataExporter
    /// </summary>
    public class ExportToXml : IDataExporter
    {
        #region Fields

        XmlWriter _xmlWriter;
        readonly MemoryStream _memoryStream;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// ctor.
        /// </summary>        
        public ExportToXml()
        {
            _memoryStream = new MemoryStream();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Sets or gets the produced file's description.
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// Sets or gets the produced file's name.
        /// </summary>
        public string FileName { set; get; }

        #endregion Properties

        #region Methods

        // Public Methods 

        /// <summary>
        /// Fires before closing the document
        /// </summary>
        /// <returns>returns the final produced file stream</returns>
        public byte[] ClosingDocument()
        {
            closeWriter();
            return _memoryStream.ToArray();
        }

        /// <summary>
        /// Fires when the document is opened.
        /// </summary>
        /// <param name="pdfColumnsAttributes">Sets columns definitions of the current report at startup</param>
        /// <param name="documentPreferences">Document settings.</param>
        public void DocumentOpened(IList<ColumnAttributes> pdfColumnsAttributes, DocumentPreferences documentPreferences)
        {
            initWriter();
        }

        /// <summary>
        /// Fires after adding a row to the main table.
        /// </summary>
        /// <param name="cellsData">cells data</param>
        /// <param name="isNewGroupStarted">Indicates starting a new group</param>
        public void RowAdded(IList<CellData> cellsData, bool isNewGroupStarted)
        {
            _xmlWriter.WriteStartElement("Record");
            foreach (var item in cellsData)
            {
                var name = XmlConvert.EncodeLocalName(item.PropertyName);
                if (isByteArray(item))
                {
                    _xmlWriter.WriteElementString(name, "[BLOB]");
                }
                else if (!string.IsNullOrEmpty(item.FormattedValue))
                {
                    _xmlWriter.WriteElementString(name, item.FormattedValue.ToSafeString());
                }
                else
                {
                    _xmlWriter.WriteElementString(name, item.PropertyValue.ToSafeString());
                }
            }
            _xmlWriter.WriteEndElement();
        }

        // Private Methods

        private void closeWriter()
        {
            _xmlWriter.WriteEndElement();
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Flush();
        }

        private void initWriter()
        {
            var wSettings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                CheckCharacters = true
            };
            _xmlWriter = XmlWriter.Create(_memoryStream, wSettings);
            _xmlWriter.WriteStartDocument();
            // Write the root node
            _xmlWriter.WriteStartElement("ArrayOfRecords");
        }

        private static bool isByteArray(CellData item)
        {
            return (item.PropertyValue != null) &&
                   (item.PropertyValue.GetType() == typeof(byte[]));
        }

        #endregion Methods
    }
}
