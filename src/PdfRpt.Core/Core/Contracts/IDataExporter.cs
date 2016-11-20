using System.Collections.Generic;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// DataExporters contract
    /// </summary>
    public interface IDataExporter
    {
        /// <summary>
        /// Sets or gets the produced file's name.
        /// </summary>
        string FileName { set; get; }

        /// <summary>
        /// Sets or gets the produced file's description.
        /// </summary>
        string Description { set; get; }

        /// <summary>
        /// Fires when the document is opened.
        /// </summary>
        /// <param name="pdfColumnsAttributes">Sets columns definitions of the current report at startup.</param>
        /// <param name="documentPreferences">Document settings.</param>
        void DocumentOpened(IList<ColumnAttributes> pdfColumnsAttributes, DocumentPreferences documentPreferences);

        /// <summary>
        /// Fires after adding a row to the main table.
        /// </summary>
        /// <param name="cellsData">cells data</param>
        /// <param name="isNewGroupStarted">Indicates starting a new group</param>
        void RowAdded(IList<CellData> cellsData, bool isNewGroupStarted);

        /// <summary>
        /// Fires before closing the document
        /// </summary>
        /// <returns>returns the final produced file's stream data</returns>
        byte[] ClosingDocument();
    }
}
