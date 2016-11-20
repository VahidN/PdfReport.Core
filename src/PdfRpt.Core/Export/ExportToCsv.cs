using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Export
{
    /// <summary>
    /// CSV DataExporter
    /// </summary>
    public class ExportToCsv : IDataExporter
    {
        #region Fields

        readonly StringBuilder _sb;
        IList<ColumnAttributes> _pdfColumnsAttributes;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// ctor.
        /// </summary>        
        public ExportToCsv()
        {
            _sb = new StringBuilder();
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
            return Encoding.UTF8.GetBytes(_sb.ToString());
        }

        /// <summary>
        /// Fires when the document is opened.
        /// </summary>
        /// <param name="pdfColumnsAttributes">Sets columns definitions of the current report at startup</param>
        /// <param name="documentPreferences">Document settings.</param>
        public void DocumentOpened(IList<ColumnAttributes> pdfColumnsAttributes, DocumentPreferences documentPreferences)
        {
            _pdfColumnsAttributes = pdfColumnsAttributes;
            initWriter();
        }

        /// <summary>
        /// Fires after adding a row to the main table.
        /// </summary>
        /// <param name="cellsData">cells data</param>
        /// <param name="isNewGroupStarted">Indicates starting a new group</param>
        public void RowAdded(IList<CellData> cellsData, bool isNewGroupStarted)
        {
            foreach (var item in cellsData)
            {
                if (isByteArray(item))
                {
                    _sb.Append(makeValueCsvFriendly("[BLOB]")).Append(",");
                }
                else if (!string.IsNullOrEmpty(item.FormattedValue))
                {
                    _sb.Append(makeValueCsvFriendly(item.FormattedValue.ToSafeString())).Append(",");
                }
                else
                {
                    _sb.Append(makeValueCsvFriendly(item.PropertyValue)).Append(",");
                }
            }
            _sb.AppendLine();
        }

        // Private Methods

        private void initWriter()
        {
            foreach (string field in _pdfColumnsAttributes.Select(x => x.PropertyName).ToList())
                _sb.Append(makeValueCsvFriendly(field)).Append(",");

            _sb.AppendLine();
        }

        private static bool isByteArray(CellData item)
        {
            return (item.PropertyValue != null) &&
                   (item.PropertyValue.GetType() == typeof(byte[]));
        }

        private static string makeValueCsvFriendly(object value)
        {
            if (value == null) return string.Empty;
            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds.ApproxEquals(0))
                    return ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            var output = value.ToString();
            if (output.Contains(",") || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';
            return output;
        }

        #endregion Methods
    }
}
