using System;
using System.Collections.Generic;
using iTextSharp.text;
using PdfRpt.Core.Contracts;
using PdfRpt.HeaderTemplates;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Default Header Provider Builder Class.
    /// </summary>
    public class DefaultHeaderProviderBuilder
    {
        readonly PdfReport _pdfReport;
        readonly DefaultHeaderProvider _defaultHeaderProvider;

        /// <summary>
        /// Gets the Default Header Provider.
        /// </summary>
        internal DefaultHeaderProvider DefaultHeaderProvider
        {
            get { return _defaultHeaderProvider; }
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="pdfReport"></param>
        public DefaultHeaderProviderBuilder(PdfReport pdfReport)
        {
            _pdfReport = pdfReport;
            _defaultHeaderProvider = new DefaultHeaderProvider { PdfFont = _pdfReport.DataBuilder.PdfFont };
        }

        /// <summary>
        /// A message to show.
        /// </summary>
        public void Message(string value)
        {
            _defaultHeaderProvider.Message = value;
        }

        /// <summary>
        /// Message's FontColor.
        /// </summary>
        public void MessageFontColor(BaseColor color)
        {
            _defaultHeaderProvider.MessageFontColor = color;
        }

        /// <summary>
        /// Message's FontStyle.
        /// </summary>
        public void MessageFontStyle(DocumentFontStyle fontStyle)
        {
            _defaultHeaderProvider.MessageFontStyle = fontStyle;
        }

        /// <summary>
        /// An optional logo path.
        /// </summary>
        public void ImagePath(string path)
        {
            _defaultHeaderProvider.ImagePath = path;
        }

        /// <summary>
        /// A Possible run direction value, left-to-right or right-to-left.
        /// </summary>
        public void RunDirection(PdfRunDirection? direction)
        {
            _defaultHeaderProvider.RunDirection = direction;
        }

        /// <summary>
        /// Return dynamic cells of the group header.
        /// </summary>
        public void GroupHeaderCells(Action<HeaderCellsBuilder> headerCellsBuilder)
        {
            Func<IList<CellData>, IList<SummaryCellData>, List<CellAttributes>> func = (rowdata, summaryData) =>
            {
                var builder = new HeaderCellsBuilder { RowData = rowdata, SummaryData = summaryData };
                headerCellsBuilder(builder);
                return builder.HeaderCells;
            };

            _defaultHeaderProvider.GroupHeaderCells = func;
        }

        /// <summary>
        /// Number of columns of GroupHeader.
        /// </summary>
        public void GroupHeaderColumnsNumber(int columnsNumber)
        {
            _defaultHeaderProvider.GroupHeaderColumnsNumber = columnsNumber;
        }
    }
}
