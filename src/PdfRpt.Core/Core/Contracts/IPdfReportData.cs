using System;
using System.Collections.Generic;
using System.IO;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Main interface of PdfRpt
    /// </summary>
    public interface IPdfReportData
    {
        /// <summary>
        /// Document settings.
        /// </summary>
        DocumentPreferences DocumentPreferences { get; set; }

        /// <summary>
        /// Main table's data source. The data to render.
        /// </summary>
        /// <returns></returns>
        Func<IDataSource> MainTableDataSource { set; get; }

        /// <summary>
        /// Defines dynamic headers of pages and individual groups.
        /// </summary>
        IPageHeader Header { get; set; }

        /// <summary>
        /// Defines footer of the each page.
        /// Leave it as null if you you want to use PageSetup.ShowDefaultFooter = true.
        /// </summary>
        IPageFooter Footer { get; set; }

        /// <summary>
        /// Pages and groups summary values settings.
        /// </summary>
        SummaryCellSettings SummarySettings { get; set; }

        /// <summary>
        /// PDF file's name
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// PDF file's stream
        /// </summary>
        Stream PdfStreamOutput { get; set; }

        /// <summary>
        /// Main table's template
        /// </summary>
        ITableTemplate Template { get; set; }

        /// <summary>
        /// Main table's cells and rows events
        /// </summary>
        Events MainTableEvents { get; set; }

        /// <summary>
        /// Pdf document's font
        /// </summary>
        IPdfFont PdfFont { get; set; }

        /// <summary>
        /// Defining which properties of MainTableDataSource should be rendered and how.
        /// If you don't set it, list of the main table's columns will be extracted from MainTableDataSource automatically.
        /// </summary>
        /// <returns></returns>
        IList<ColumnAttributes> PdfColumnsAttributes { get; set; }

        /// <summary>
        /// If you don't set PdfColumnsDefinitions, list of the main table's columns will be extracted from MainTableDataSource automatically.
        /// Here you can control how cells should be rendered based on their specific data types.
        /// </summary>
        AdHocColumnsConventions AdHocColumnsConventions { get; set; }

        /// <summary>
        /// Sets the encryption options for this document.
        /// Leave it as null if you don't want to use it.
        /// </summary>
        DocumentSecurity DocumentSecurity { get; set; }
    }
}
