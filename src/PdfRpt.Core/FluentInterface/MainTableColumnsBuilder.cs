using System;
using System.Collections.Generic;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// MainTable Columns Builder Class.
    /// </summary>
    public class MainTableColumnsBuilder
    {
        readonly IList<ColumnAttributes> _pdfColumns;

        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        internal IList<ColumnAttributes> PdfColumns
        {
            get { return _pdfColumns; }
        }

        /// <summary>
        /// ctor.
        /// </summary>
        public MainTableColumnsBuilder()
        {
            _pdfColumns = new List<ColumnAttributes>();
        }

        /// <summary>
        /// Column Builder
        /// </summary>
        /// <param name="pdfColumnAttributesBuilder"></param>
        public void AddColumn(Action<ColumnAttributesBuilder> pdfColumnAttributesBuilder)
        {
            var builder = new ColumnAttributesBuilder();
            pdfColumnAttributesBuilder(builder);
            _pdfColumns.Add(builder.PdfColumnAttributes);
        }
    }
}
