using System;
using System.Collections.Generic;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Inject Custom Rows Builder Class.
    /// </summary>
    public class InjectCustomRowsBuilder
    {
        readonly IList<IList<CellData>> _rows = new List<IList<CellData>>();

        /// <summary>
        /// List of the new rows.
        /// </summary>
        internal IList<IList<CellData>> Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// Available data
        /// </summary>
        public EventsArguments EventsArgs { set; get; }

        /// <summary>
        /// Custom Rows Builder
        /// </summary>
        /// <param name="customRowsBuilder"></param>
        public void AddRow(Action<CustomRowsBuilder> customRowsBuilder)
        {
            var builder = new CustomRowsBuilder { EventsArgs = this.EventsArgs };
            customRowsBuilder(builder);
            _rows.Add(builder.RowCells);
        }
    }
}
