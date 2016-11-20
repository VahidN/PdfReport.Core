using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using PdfRpt.Aggregates.Numbers;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Export
{
    /// <summary>
    /// Microsoft Excel Worksheet DataExporter
    /// </summary>
    public class ExportToExcel : IDataExporter
    {
        #region Fields

        readonly IDictionary<int, string> _uniqueHeaders = new Dictionary<int, string>();
        int _outlineLevel = -1;
        int _col = 1;
        IList<ColumnAttributes> _columns;
        DocumentPreferences _documentPreferences;
        readonly IDictionary<HorizontalAlignment, ExcelHorizontalAlignment> _horizontalAlignment =
             new Dictionary<HorizontalAlignment, ExcelHorizontalAlignment>
             {
                 { HorizontalAlignment.Center, ExcelHorizontalAlignment.Center},
                 { HorizontalAlignment.Justified, ExcelHorizontalAlignment.Justify},
                 { HorizontalAlignment.JustifiedAll, ExcelHorizontalAlignment.Justify},
                 { HorizontalAlignment.Left, ExcelHorizontalAlignment.Left},
                 { HorizontalAlignment.None, ExcelHorizontalAlignment.Center},
                 { HorizontalAlignment.Right, ExcelHorizontalAlignment.Right},
                 { HorizontalAlignment.Undefined, ExcelHorizontalAlignment.Center}
             };
        readonly MemoryStream _memoryStream;
        readonly ExcelPackage _package;
        int _row = 1;
        readonly IDictionary<Type, RowFunctions> _rowFunctions = new Dictionary<Type, RowFunctions>
        {
            { typeof(Average) , RowFunctions.Average },
            { typeof(Maximum) , RowFunctions.Max },
            { typeof(Minimum) , RowFunctions.Min },
            { typeof(StdDev) , RowFunctions.StdDev },
            { typeof(Sum) , RowFunctions.Sum },
            { typeof(Variance) , RowFunctions.Var }
        };
        ExcelWorksheet _worksheet;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// ctor.
        /// </summary>        
        public ExportToExcel()
        {
            _memoryStream = new MemoryStream();
            _package = new ExcelPackage(_memoryStream);
        }

        #endregion Constructors

        #region Properties (11)

        /// <summary>
        /// DateTime Format such as yyyy-MM-dd hh:mm
        /// </summary>
        public string DateTimeFormat { set; get; }

        /// <summary>
        /// Sets or gets the produced file's description.
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// Sets or gets the produced file's name.
        /// </summary>
        public string FileName { set; get; }

        /// <summary>
        /// Footer's Text
        /// </summary>
        public string Footer { set; get; }

        /// <summary>
        /// Header's Text
        /// </summary>
        public string Header { set; get; }

        /// <summary>
        /// Number format such as #,##0
        /// </summary>
        public string Numberformat { set; get; }

        /// <summary>
        /// Sets the view mode of the worksheet to PageLayout.
        /// </summary>
        public bool PageLayoutView { set; get; }

        /// <summary>
        /// Show GridLines in the worksheet.
        /// </summary>
        public bool ShowGridLines { set; get; }

        /// <summary>
        /// Sets or gets the produced table's style.
        /// </summary>
        public TableStyles TableStyle { set; get; }

        /// <summary>
        /// TimeSpan Format such as hh:mm:ss
        /// </summary>
        public string TimeSpanFormat { set; get; }

        /// <summary>
        /// Sets or gets the WorksheetName.
        /// </summary>
        public string WorksheetName { set; get; }

        #endregion Properties

        #region Methods (25)

        // Public Methods (3) 

        /// <summary>
        /// Fires before closing the document
        /// </summary>
        /// <returns>returns the final produced file stream</returns>
        public byte[] ClosingDocument()
        {
            autoFitColumns();
            addToTable();
            _package.Save();
            _package.Dispose();
            return _memoryStream.ToArray();
        }

        /// <summary>
        /// Fires when the document is opened.
        /// </summary>
        /// <param name="pdfColumnsAttributes">Sets columns definitions of the current report at startup</param>
        /// <param name="documentPreferences">Document settings.</param>
        public void DocumentOpened(IList<ColumnAttributes> pdfColumnsAttributes, DocumentPreferences documentPreferences)
        {
            _columns = pdfColumnsAttributes;
            _documentPreferences = documentPreferences;

            if (string.IsNullOrEmpty(WorksheetName))
                throw new InvalidOperationException("Please set the WorksheetName.");

            _worksheet = _package.Workbook.Worksheets.Add(WorksheetName);
            initSettings();
            printHeader();
        }

        /// <summary>
        /// Fires after adding a row to the main table.
        /// </summary>
        /// <param name="cellsData">cells data</param>
        /// <param name="isNewGroupStarted">Indicates starting a new group</param>
        public void RowAdded(IList<CellData> cellsData, bool isNewGroupStarted)
        {
            //todo: isNewGroupStarted ?
            _col = 1;
            foreach (var item in cellsData)
            {
                var itemType = typeof(string);
                if (item.PropertyValue != null)
                {
                    itemType = item.PropertyValue.GetType();
                }

                if (isByteArrayImageField(item))
                {
                    addImageFromStream((byte[])item.PropertyValue);
                }
                else if (isImageFilePathField(item))
                {
                    addImageFromFile(item);
                }
                else if (itemType.IsNumericType())
                {
                    setNumericItem(item);
                }
                else if (!string.IsNullOrEmpty(item.FormattedValue))
                {
                    _worksheet.Cells[_row, _col].Value = item.FormattedValue.ToSafeString();
                }
                else if (itemType == typeof(DateTime))
                {
                    setDateTimeItem(item);
                }
                else if (itemType == typeof(TimeSpan))
                {
                    setTimeSpanItem(item);
                }
                else
                {
                    _worksheet.Cells[_row, _col].Value = item.PropertyValue;
                }

                formatCell(item);
                _col++;
            }

            setGroups(isNewGroupStarted);

            _row++;
        }

        private void setGroups(bool isNewGroupStarted)
        {
            if (isNewGroupStarted)
            {
                _worksheet.Row(_row).OutlineLevel = 0;
                _outlineLevel = 1;
                _worksheet.OutLineSummaryBelow = false;
            }
            else if (_outlineLevel != -1)
            {
                _worksheet.Row(_row).OutlineLevel = _outlineLevel;
                _worksheet.OutLineSummaryBelow = false;
            }
        }
        // Private Methods (22) 

        private void addHeaderFooter()
        {
            if (!string.IsNullOrEmpty(Header))
                _worksheet.HeaderFooter.OddHeader.CenteredText = Header;

            if (!string.IsNullOrEmpty(Footer))
                _worksheet.HeaderFooter.OddFooter.CenteredText = Footer;
        }

        private void addImageFromFile(CellData item)
        {
            var filePath = item.PropertyValue.ToSafeString();
            if (!File.Exists(filePath)) return;

            var data = File.ReadAllBytes(filePath);
            this.addImageFromStream(data);
        }

        void addImageFromStream(byte[] data)
        {
            if (data == null) return;
            using (var ms = new MemoryStream(data))
            {
                var image = Image.FromStream(ms);
                _worksheet.Row(_row).Height = (image.Height + 1).Pixel2RowHeight();
                _worksheet.Column(_col).Width = _worksheet.Pixel2ColumnWidth(image.Width + 1);
                var picture = _worksheet.Drawings.AddPicture("pic" + _row + _col, image);
                picture.From.Column = _col - 1;
                picture.From.Row = _row - 1;
                picture.From.ColumnOff = 2.Pixel2Mtu();
                picture.From.RowOff = 2.Pixel2Mtu();
                picture.SetSize(image.Width, image.Height);
            }
        }

        private void addNumberformat()
        {
            var numStyle = _package.Workbook.Styles.CreateNamedStyle("TableNumber");
            numStyle.Style.Numberformat.Format = Numberformat;
        }

        private void addToTable()
        {
            var tbl = _worksheet.Tables.Add(new ExcelAddressBase(1, 1, _row - 1, _columns.Count), "Data");
            tbl.ShowHeader = true;
            tbl.TableStyle = TableStyle;
            setAggregateFunctions(tbl);
        }

        private void autoFitColumns()
        {
            _worksheet.Cells[1, 1, _row - 1, _columns.Count].AutoFitColumns();
        }

        private void formatCell(CellData item)
        {
            if (item.PropertyValue == null) return;
            _worksheet.Cells[_row, _col].Style.HorizontalAlignment = getHorizontalAlignment(_col - 1);
            _worksheet.Cells[_row, _col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }

        ExcelHorizontalAlignment getHorizontalAlignment(int idx)
        {
            if (_columns[idx].CellsHorizontalAlignment == null)
                return ExcelHorizontalAlignment.Center;
            return _horizontalAlignment[_columns[idx].CellsHorizontalAlignment.Value];
        }

        private void initFormats()
        {
            if (string.IsNullOrEmpty(Numberformat))
                Numberformat = "#,##0";

            if (string.IsNullOrEmpty(DateTimeFormat))
                DateTimeFormat = "yyyy-MM-dd hh:mm";

            if (string.IsNullOrEmpty(TimeSpanFormat))
                TimeSpanFormat = "hh:mm:ss";
        }

        private void initSettings()
        {
            initFormats();
            addNumberformat();
            setRunDirection();
            setDocumentMetadata();
            setPageLayout();
            addHeaderFooter();
            setOrientation();
        }

        private bool isByteArrayImageField(CellData item)
        {
            return (item.PropertyValue != null) &&
                   (item.PropertyValue.GetType() == typeof(byte[])) &&
                   (_columns[_col - 1].ColumnItemsTemplate is ByteArrayImageField);
        }

        private bool isImageFilePathField(CellData item)
        {
            return (item.PropertyValue != null) &&
                   (_columns[_col - 1].ColumnItemsTemplate is ImageFilePathField);
        }

        private void printHeader()
        {
            //we can not have duplicate captions here.
            var duplicates = _columns.GroupBy(x => x.HeaderCell.Caption).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            var counterDictionary = new Dictionary<string, int>();
            foreach (var item in _columns)
            {
                //totals don't like [ & ] !
                var caption = item.HeaderCell.Caption;
                caption = caption.Replace("[", "(").Replace("]", ")").Trim();
                if (duplicates.Contains(caption))
                {
                    if (counterDictionary.ContainsKey(caption))
                        counterDictionary[caption]++;
                    else
                        counterDictionary.Add(caption, 1);
                    caption = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", caption, counterDictionary[caption]);
                }
                _worksheet.Cells[_row, _col].Value = caption;
                _uniqueHeaders.Add(_col, caption);
                setHorizontalAlignment(item);
                _col++;
            }
            _row++;
        }

        private void setAggregateFunctions(ExcelTable tbl)
        {
            var idx = 0;
            foreach (var item in _columns)
            {
                idx++;
                if (item.AggregateFunction == null) continue;

                var function = item.AggregateFunction;
                var aggregateFunction = item.AggregateFunction as AggregateProvider;
                if (aggregateFunction != null)
                {
                    function = aggregateFunction.ColumnAggregateFunction;
                }

                var type = function.GetType();
                RowFunctions rowFunction;
                if (_rowFunctions.TryGetValue(type, out rowFunction))
                {
                    if (!tbl.ShowTotal) tbl.ShowTotal = true;
                    tbl.Columns[_uniqueHeaders[idx]].DataCellStyleName = "TableNumber";
                    tbl.Columns[_uniqueHeaders[idx]].TotalsRowFunction = rowFunction;
                    _worksheet.Cells[_row, idx].Style.Numberformat.Format = Numberformat;
                }
            }
        }

        private void setDateTimeItem(CellData item)
        {
            _worksheet.Cells[_row, _col].Value = item.PropertyValue;
            _worksheet.Cells[_row, _col].Style.Numberformat.Format = DateTimeFormat;
        }

        private void setDocumentMetadata()
        {
            if (_documentPreferences.DocumentMetadata == null) return;

            _package.Workbook.Properties.Title = _documentPreferences.DocumentMetadata.Title;
            _package.Workbook.Properties.Author = _documentPreferences.DocumentMetadata.Author;
            _package.Workbook.Properties.Subject = _documentPreferences.DocumentMetadata.Subject;
            _package.Workbook.Properties.Keywords = _documentPreferences.DocumentMetadata.Keywords;
        }

        private void setHorizontalAlignment(ColumnAttributes item)
        {
            if (item.CellsHorizontalAlignment == null)
                item.CellsHorizontalAlignment = HorizontalAlignment.Center;
            _worksheet.Cells[_row, _col].Style.HorizontalAlignment = _horizontalAlignment[item.CellsHorizontalAlignment.Value];
            _worksheet.Cells[_row, _col].Style.Font.Bold = true;
        }

        private void setNumericItem(CellData item)
        {
            _worksheet.Cells[_row, _col].Value = item.PropertyValue;
            _worksheet.Cells[_row, _col].Style.Numberformat.Format = Numberformat;
        }

        private void setOrientation()
        {
            if (_documentPreferences.PagePreferences.Orientation == PageOrientation.Portrait)
            {
                _worksheet.PrinterSettings.Orientation = eOrientation.Portrait;
            }
        }

        private void setPageLayout()
        {
            _worksheet.View.ShowGridLines = ShowGridLines;
            _worksheet.View.PageLayoutView = PageLayoutView;
        }

        private void setRunDirection()
        {
            if (_documentPreferences.PagePreferences.RunDirection != null &&
                _documentPreferences.PagePreferences.RunDirection.Value == PdfRunDirection.RightToLeft)
            {
                _worksheet.View.RightToLeft = true;
            }
        }

        private void setTimeSpanItem(CellData item)
        {
            _worksheet.Cells[_row, _col].Value = item.PropertyValue;
            _worksheet.Cells[_row, _col].Style.Numberformat.Format = TimeSpanFormat;
        }

        #endregion Methods
    }
}