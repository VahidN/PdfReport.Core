using System;
using System.Collections.Generic;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Events Builder Class.
    /// </summary>
    public class MainTableEventsBuilder
    {
        readonly PdfReport _pdfReport;
        private IPdfFont _font;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public MainTableEventsBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
            _font = new GenericFontProvider(_pdfReport.DataBuilder.PdfFont);
        }

        /// <summary>
        /// Gets/Sets the default fonts of the document.
        /// </summary>
        public IPdfFont PdfFont
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// Fires when there's no data to render.
        /// </summary>
        /// <param name="message">a message to show</param>
        public void DataSourceIsEmpty(string message)
        {
            _pdfReport.DataBuilder.DataSourceIsEmptyEvent(message);
        }

        /// <summary>
        /// Fires after a cell has been added.
        /// </summary>
        public void CellAdded(Action<EventsArguments> onCellAdded)
        {
            _pdfReport.DataBuilder.CellAddedEvent(onCellAdded);
        }

        /// <summary>
        /// Fires when a cell is created.
        /// </summary>
        public void CellCreated(Action<EventsArguments> onCellCreated)
        {
            _pdfReport.DataBuilder.CellCreatedEvent(onCellCreated);
        }

        /// <summary>
        /// Fires before closing the Document.
        /// </summary>
        public void DocumentClosing(Action<EventsArguments> onDocumentClosing)
        {
            _pdfReport.DataBuilder.DocumentClosingEvent(onDocumentClosing);
        }

        /// <summary>
        /// Fires when Document is created.
        /// </summary>
        public void DocumentOpened(Action<EventsArguments> onDocumentOpened)
        {
            _pdfReport.DataBuilder.DocumentOpenedEvent(onDocumentOpened);
        }

        /// <summary>
        /// Fires after a group has been added.
        /// </summary>
        public void GroupAdded(Action<EventsArguments> onGroupAdded)
        {
            _pdfReport.DataBuilder.GroupAddedEvent(onGroupAdded);
        }

        /// <summary>
        /// Fires after MainTable has been added.
        /// </summary>
        public void MainTableAdded(Action<EventsArguments> onMainTableAdded)
        {
            _pdfReport.DataBuilder.MainTableAddedEvent(onMainTableAdded);
        }

        /// <summary>
        /// Fires when MainTable is created.
        /// </summary>
        public void MainTableCreated(Action<EventsArguments> onMainTableCreated)
        {
            _pdfReport.DataBuilder.MainTableCreatedEvent(onMainTableCreated);
        }

        /// <summary>
        /// Fires after a row has been added.
        /// </summary>
        public void RowAdded(Action<EventsArguments> onRowAdded)
        {
            _pdfReport.DataBuilder.RowAddedEvent(onRowAdded);
        }

        /// <summary>
        /// Fires before a row has been added.
        /// </summary>
        public void RowStarted(Action<EventsArguments> onRowStarted)
        {
            _pdfReport.DataBuilder.RowStartedEvent(onRowStarted);
        }

        /// <summary>
        /// Fires before a row has been added.
        /// </summary>
        public void ShouldSkipRow(Func<EventsArguments, bool> onShouldSkipRow)
        {
            _pdfReport.DataBuilder.ShouldSkipRowEvent(onShouldSkipRow);
        }

        /// <summary>
        /// Fires before a header row has been added.
        /// </summary>
        public void ShouldSkipHeader(Func<EventsArguments, bool> onShouldSkipHeader)
        {
            _pdfReport.DataBuilder.ShouldSkipHeaderEvent(onShouldSkipHeader);
        }

        /// <summary>
        /// Fires before a footer row has been added.
        /// </summary>
        public void ShouldSkipFooter(Func<EventsArguments, bool> onShouldSkipFooter)
        {
            _pdfReport.DataBuilder.ShouldSkipFooterEvent(onShouldSkipFooter);
        }

        /// <summary>
        /// Fires after each part of the MainTable has been added to the current page.
        /// </summary>
        public void PageTableAdded(Action<EventsArguments> onPageTableAdded)
        {
            _pdfReport.DataBuilder.PageTableAddedEvent(onPageTableAdded);
        }

        /// <summary>
        /// Fires before RowStartedEvent when the RowType is DataTableRow.
        /// </summary>
        public void RowStartedInjectCustomRows(Action<InjectCustomRowsBuilder> customRowsBuilder)
        {
            Func<EventsArguments, IList<IList<CellData>>> func = args =>
            {
                var builder = new InjectCustomRowsBuilder { EventsArgs = args };
                customRowsBuilder(builder);
                return builder.Rows;
            };
            _pdfReport.DataBuilder.RowStartedInjectCustomRowsEvent(func);
        }

        /// <summary>
        /// Fires after RowAddedEvent when the RowType is DataTableRow.
        /// </summary>
        public void RowAddedInjectCustomRows(Action<InjectCustomRowsBuilder> customRowsBuilder)
        {
            Func<EventsArguments, IList<IList<CellData>>> func = args =>
            {
                var builder = new InjectCustomRowsBuilder { EventsArgs = args };
                customRowsBuilder(builder);
                return builder.Rows;
            };
            _pdfReport.DataBuilder.RowAddedInjectCustomRowsEvent(func);
        }
    }
}
