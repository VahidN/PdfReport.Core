using System;
using System.Collections.Generic;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Main table's cells and rows events
    /// </summary>
    public class Events
    {
        /// <summary>
        /// Fires when there's no data to render
        /// </summary>
        public Action<EventsArguments> DataSourceIsEmpty { set; get; }

        /// <summary>
        /// Fires after a cell has been added.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> CellAdded { set; get; }

        /// <summary>
        /// Fires when a cell is created.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> CellCreated { set; get; }

        /// <summary>
        /// Fires before closing the Document.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> DocumentClosing { set; get; }

        /// <summary>
        /// Fires when Document is created.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> DocumentOpened { set; get; }

        /// <summary>
        /// Fires after a group has been added.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> GroupAdded { set; get; }

        /// <summary>
        /// Fires after MainTable has been added.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> MainTableAdded { set; get; }

        /// <summary>
        /// Fires when MainTable is created.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> MainTableCreated { set; get; }

        /// <summary>
        /// Fires after a row has been added.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> RowAdded { set; get; }

        /// <summary>
        /// Fires before a row has been added.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> RowStarted { set; get; }

        /// <summary>
        /// Fires before RowStartedEvent when the RowType is DataTableRow.
        /// Now you will have time to inject your custom and calculated rows between the predefined rows.
        /// Please note that your returned IList of PdfCellData should contain all of the field names
        /// of the actual data source.
        /// It can be null to stop processing.
        /// </summary>
        public Func<EventsArguments, IList<IList<CellData>>> RowStartedInjectCustomRows { set; get; }

        /// <summary>
        /// Fires after RowAddedEvent when the RowType is DataTableRow.
        /// Now you will have time to inject your custom and calculated rows between the predefined rows.
        /// Please note that your returned IList of PdfCellData should contain all of the field names
        /// of the actual data source.
        /// It can be null to stop processing.
        /// </summary>
        public Func<EventsArguments, IList<IList<CellData>>> RowAddedInjectCustomRows { set; get; }

        /// <summary>
        /// Fires after each part of the MainTable has been added to the current page.
        /// </summary>
        /// <returns></returns>
        public Action<EventsArguments> PageTableAdded { set; get; }


        /// <summary>
        /// Fires before a row has been added.
        /// </summary>
        /// <returns>return true to skip adding this row.</returns>
        public Func<EventsArguments, bool> ShouldSkipRow { set; get; }

        /// <summary>
        /// Fires before a header row has been added.
        /// </summary>
        /// <returns>return true to skip adding this row.</returns>
        public Func<EventsArguments, bool> ShouldSkipHeader { set; get; }

        /// <summary>
        /// Fires before a footer row has been added.
        /// </summary>
        /// <returns>return true to skip adding this row.</returns>
        public Func<EventsArguments, bool> ShouldSkipFooter { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public Events()
        {
            PageTableAdded = delegate { };
            DataSourceIsEmpty = delegate { };
            CellAdded = delegate { };
            CellCreated = delegate { };
            DocumentClosing = delegate { };
            DocumentOpened = delegate { };
            GroupAdded = delegate { };
            MainTableAdded = delegate { };
            MainTableCreated = delegate { };
            RowAdded = delegate { };
            RowStarted = delegate { };
            ShouldSkipRow = delegate { return false; };
            ShouldSkipHeader = delegate { return false; };
            ShouldSkipFooter = delegate { return false; };
        }
    }
}