using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.PdfTable
{
    /// <summary>
    /// Wrapping the main table in multiple columns per page.
    /// </summary>
    public class TablesInColumns
    {
        #region Properties (4)

        /// <summary>
        /// Holds last result of the actual rendering engine of iTextSharp during its processes.
        /// </summary>
        public LastRenderedRowData CurrentRowInfoData { set; get; }

        /// <summary>
        /// Document settings
        /// </summary>
        public DocumentPreferences PageSetup { get; set; }

        /// <summary>
        /// PDF Document object
        /// </summary>
        public Document PdfDoc { get; set; }

        /// <summary>
        /// PdfWriter object
        /// </summary>
        public PdfWriter PdfWriter { get; set; }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1)

        /// <summary>
        /// Wrapping an element in multiple columns per page.
        /// </summary>
        /// <param name="element">The element to wrap</param>
        public void Wrap(IElement element)
        {
            var count = 0;
            float l = 0;
            var columnsWidth = PageSetup.MultipleColumnsPerPage.ColumnsWidth;
            var columnsMargin = PageSetup.MultipleColumnsPerPage.ColumnsGap;
            var columnsPerPage = PageSetup.MultipleColumnsPerPage.ColumnsPerPage - 1;
            var r = columnsWidth;
            var isRtl = PageSetup.MultipleColumnsPerPage.IsRightToLeft;
            var startNewPage = false;

            if (PageSetup.PagePreferences.RunDirection == null)
                PageSetup.PagePreferences.RunDirection = PdfRunDirection.LeftToRight;

            var ct = new ColumnText(PdfWriter.DirectContent)
                         {
                             RunDirection = (int)PageSetup.PagePreferences.RunDirection
                         };

            ct.AddElement(element);
            var top = PdfDoc.Top - CurrentRowInfoData.HeaderHeight - PageSetup.MultipleColumnsPerPage.TopMargin;
            var status = 0;

            // render the column as long as it has content
            while (ColumnText.HasMoreText(status))
            {
                if (startNewPage) PdfDoc.NewPage();

                if (isRtl)
                {
                    ct.SetSimpleColumn(
                        PdfDoc.Right - l, PdfDoc.Bottom,
                        PdfDoc.Right - r, top
                    );
                }
                else
                {
                    ct.SetSimpleColumn(
                        PdfDoc.Left + l, PdfDoc.Bottom,
                        PdfDoc.Left + r, top
                    );
                }

                l += columnsWidth + columnsMargin;
                r += columnsWidth + columnsMargin;

                // render as much content as possible
                status = ct.Go();

                // go to a new page if you've reached the last column
                if (++count > columnsPerPage)
                {
                    count = 0;
                    l = 0;
                    r = columnsWidth;
                    startNewPage = true;
                }
                else
                {
                    startNewPage = false;
                }
            }
        }

        #endregion Methods
    }
}
