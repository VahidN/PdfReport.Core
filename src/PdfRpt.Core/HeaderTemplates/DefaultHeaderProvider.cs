using System;
using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.HeaderTemplates
{
    /// <summary>
    /// Defines dynamic headers for pages and individual groups.
    /// </summary>
    public class DefaultHeaderProvider : IPageHeader
    {
        /// <summary>
        /// A message to show.
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// Message's FontColor.
        /// </summary>
        public BaseColor MessageFontColor { set; get; }

        /// <summary>
        /// Message's FontStyle.
        /// </summary>
        public DocumentFontStyle MessageFontStyle { set; get; }

        /// <summary>
        /// Message's font.
        /// </summary>
        public IPdfFont PdfFont { set; get; }

        /// <summary>
        /// An optional logo path.
        /// </summary>
        public string ImagePath { set; get; }

        /// <summary>
        /// A Possible run direction value, left-to-right or right-to-left.
        /// </summary>
        public PdfRunDirection? RunDirection { set; get; }

        /// <summary>
        /// Fires when a new groups is being created.
        /// </summary>
        /// <param name="pdfDoc"></param>
        /// <param name="pdfWriter"></param>
        /// <param name="newGroupInfo"></param>
        /// <param name="summaryData"></param>
        /// <returns></returns>
        public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData)
        {
            if (GroupHeaderCells == null) return null;

            var cells = GroupHeaderCells(newGroupInfo, summaryData);
            foreach (var cell in cells)
            {
                if (cell == null || cell.BasicProperties == null) continue;
                if (cell.BasicProperties.PdfFont == null)
                    cell.BasicProperties.PdfFont = PdfFont;
            }

            return TableHelper.SimpleTable(
                columnsNumber: GroupHeaderColumnsNumber,
                pdfCellAttributesList: cells,
                nullRowBackgroundColor: BaseColor.White
                );
        }

        /// <summary>
        /// Return dynamic cells of the group header.
        /// </summary>
        public Func<IList<CellData>, IList<SummaryCellData>, List<CellAttributes>> GroupHeaderCells { set; get; }

        /// <summary>
        /// Number of columns of GroupHeader.
        /// </summary>
        public int GroupHeaderColumnsNumber { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public DefaultHeaderProvider()
        {
            GroupHeaderColumnsNumber = 1;
            MessageFontColor = new BaseColor(Color.Black.ToArgb());
            MessageFontStyle = DocumentFontStyle.Bold;
        }

        /// <summary>
        /// Fires when a new page is being added.
        /// </summary>
        /// <param name="pdfDoc"></param>
        /// <param name="pdfWriter"></param>
        /// <param name="summaryData"></param>
        /// <returns></returns>
        public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
        {
            return TableHelper.SimpleTable(
                columnsNumber: 1,
                pdfCellAttributesList: new List<CellAttributes>
                    {
                        new CellAttributes
                        {
                            BasicProperties = new CellBasicProperties
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                ShowBorder = false,
                                FontColor = new BaseColor(Color.Black.ToArgb()),
                                BackgroundColor = null
                            },
                            RowData = new CellRowData
                            {
                                Value = ImagePath,
                                PdfRowType = RowType.MainHeaderRow
                            },
                            ItemTemplate = new ImageFilePathField()
                        },
                        new CellAttributes
                        {
                            BasicProperties = new CellBasicProperties
                            {
                                BorderColor = BaseColor.Black,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                PdfFont = PdfFont,
                                RunDirection = RunDirection ?? PdfRunDirection.RightToLeft,
                                FontColor = MessageFontColor,
                                ShowBorder = false,
                                BackgroundColor = null,
                                PdfFontStyle = MessageFontStyle
                            },
                            RowData = new CellRowData
                            {
                                Value = Message,
                                PdfRowType = RowType.MainHeaderRow
                            }
                        },
                        null
                    },
                    showBorder: true
                );
        }
    }
}