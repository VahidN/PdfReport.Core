using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// PdfRptEvents Args
    /// </summary>
    public class EventsArguments
    {
        /// <summary>
        /// Pdf document's font
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// Defining which properties of MainTableDataSource should be rendered and how.
        /// </summary>
        public IList<ColumnAttributes> PdfColumnsAttributes { set; get; }


        /// <summary>
        /// Document settings.
        /// </summary>
        public DocumentPreferences PageSetup { set; get; }

        /// <summary>
        /// Pdf Document Object
        /// </summary>
        public Document PdfDoc { set; get; }

        /// <summary>
        /// Pdf Writer Object
        /// </summary>
        public PdfWriter PdfWriter { set; get; }

        /// <summary>
        /// Cell's info
        /// </summary>
        public CellAttributes Cell { set; get; }

        /// <summary>
        /// Main table's cell type
        /// </summary>
        public CellType CellType { set; get; }

        /// <summary>
        /// Main table's row type
        /// </summary>
        public RowType RowType { set; get; }

        /// <summary>
        /// Column Number.
        /// </summary>
        public int ColumnNumber { set; get; }

        /// <summary>
        /// PDF file stream
        /// </summary>
        public Stream PdfStreamOutput { set; get; }

        /// <summary>
        /// Summary Data
        /// </summary>
        public IList<SummaryCellData> ColumnCellsSummaryData { set; get; }

        /// <summary>
        /// Main table.
        /// </summary>
        public PdfGrid Table { set; get; }

        /// <summary>
        /// Current row's data.
        /// </summary>
        public IList<CellData> TableRowData { set; get; }

        /// <summary>
        /// Current row's data indexed Property.
        /// </summary>
        public TableRowDataIndexer TableRowDataIdx { set; get; }

        /// <summary>
        /// Previous row's data.
        /// </summary>
        public IList<CellData> PreviousTableRowData { set; get; }

        /// <summary>
        /// Previous row's data indexed Property.
        /// </summary>
        public PreviousTableRowDataIndexer PreviousTableRowDataIdx { set; get; }

        /// <summary>
        /// Acts as an indexer for the PreviousTableRowData property.
        /// </summary>
        public class PreviousTableRowDataIndexer
        {
            private readonly EventsArguments _owner;

            /// <summary>
            /// ctor.
            /// </summary>
            /// <param name="owner">parent object</param>
            public PreviousTableRowDataIndexer(EventsArguments owner)
            {
                _owner = owner;
            }

            /// <summary>
            /// Previous row's data indexed Property.
            /// </summary>
            /// <param name="propertyName">A property to find</param>
            /// <returns></returns>
            public object this[string propertyName]
            {
                get { return _owner.PreviousTableRowData.GetValueOf(propertyName); }
                set
                {
                    _owner.PreviousTableRowData.SetValueOf(propertyName, value);
                }
            }

            /// <summary>
            /// Length of the PreviousTableRowData.
            /// </summary>
            public int Length
            {
                get { return _owner.PreviousTableRowData.Count; }
            }

        }

        /// <summary>
        /// Acts as an indexer for the TableRowData property.
        /// </summary>
        public class TableRowDataIndexer
        {
            private readonly EventsArguments _owner;

            /// <summary>
            /// ctor.
            /// </summary>
            /// <param name="owner">parent object</param>
            public TableRowDataIndexer(EventsArguments owner)
            {
                _owner = owner;
            }

            /// <summary>
            /// Current row's data indexed Property.
            /// </summary>
            /// <param name="propertyName">A property to find</param>
            /// <returns></returns>
            public object this[string propertyName]
            {
                get { return _owner.TableRowData.GetValueOf(propertyName); }
                set
                {
                    _owner.TableRowData.SetValueOf(propertyName, value);
                }
            }

            /// <summary>
            /// Length of the TableRowData.
            /// </summary>
            public int Length
            {
                get { return _owner.TableRowData.Count; }
            }
        }

        /// <summary>
        /// Returns the Overall Aggregate Value of the specified property.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expression">the specified property</param>
        /// <param name="nullValue">An optional nullValue of the expected object.</param>
        /// <returns></returns>
        public string LastOverallAggregateValueOf<TEntity>(Expression<Func<TEntity, object>> expression, string nullValue = "0")
        {
            var propertyName = PropertyHelper.Name(expression);
            return LastOverallAggregateValueOf(propertyName, nullValue);
        }

        /// <summary>
        /// Returns the Overall Aggregate Value of the specified property.
        /// </summary>
        /// <param name="propertyName">the specified property</param>
        /// <param name="nullValue">An optional nullValue of the expected object.</param>
        /// <returns></returns>
        public string LastOverallAggregateValueOf(string propertyName, string nullValue = "0")
        {
            var data = ColumnCellsSummaryData.Where(x => x.CellData.PropertyName.Equals(propertyName)).OrderByDescending(x => x.OverallRowNumber).FirstOrDefault();
            return data == null ? nullValue : data.OverallAggregateValue.ToSafeString(nullValue);
        }

        /// <summary>
        /// Returns the Overall Aggregate Value of the specified property.
        /// </summary>
        /// <param name="propertyName">the specified property</param>
        /// <returns></returns>
        public object LastOverallAggregateValue(string propertyName)
        {
            var data = ColumnCellsSummaryData.Where(x => x.CellData.PropertyName.Equals(propertyName)).OrderByDescending(x => x.OverallRowNumber).FirstOrDefault();
            return data == null ? null : data.OverallAggregateValue;
        }

        /// <summary>
        /// Returns the Overall Aggregate Value of the specified property.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expression">the specified property</param>
        /// <returns></returns>
        public object LastOverallAggregateValue<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            var propertyName = PropertyHelper.Name(expression);
            return LastOverallAggregateValue(propertyName);
        }

        /// <summary>
        /// ctor.
        /// </summary>
        public EventsArguments()
        {
            TableRowDataIdx = new TableRowDataIndexer(this);
            PreviousTableRowDataIdx = new PreviousTableRowDataIndexer(this);
        }
    }
}