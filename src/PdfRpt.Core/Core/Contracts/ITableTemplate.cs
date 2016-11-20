using System.Collections.Generic;
using iTextSharp.text;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Main table's template
    /// </summary>
    public interface ITableTemplate
    {
        /// <summary>
        /// Alternating rows background color value.
        /// </summary>
        BaseColor AlternatingRowBackgroundColor { get; }

        /// <summary>
        /// Alternating rows font color value.
        /// </summary>
        BaseColor AlternatingRowFontColor { get; }

        /// <summary>
        /// Cells border color value.
        /// </summary>
        BaseColor CellBorderColor { get; }

        /// <summary>
        /// Main table's header background color value. 
        /// At least one color and max. 2 colors should be specified. If 2 colors are introduced a gradient by using the PdfShading will be generated.
        /// </summary>
        IList<BaseColor> HeaderBackgroundColor { get; }

        /// <summary>
        /// Header's caption horizontal alignment.
        /// </summary>
        HorizontalAlignment HeaderHorizontalAlignment { get; }

        /// <summary>
        /// Main table's headers font color value.
        /// </summary>
        BaseColor HeaderFontColor { get; }

        /// <summary>
        /// Rows background color value.
        /// </summary>
        BaseColor RowBackgroundColor { get; }

        /// <summary>
        /// Rows font color value.
        /// </summary>
        BaseColor RowFontColor { get; }

        /// <summary>
        /// Remaining rows background color value.
        /// At least one color and max. 2 colors should be specified. If 2 colors are introduced a gradient by using the PdfShading will be generated.
        /// </summary>
        IList<BaseColor> PreviousPageSummaryRowBackgroundColor { get; }

        /// <summary>
        /// Remaining rows font color value.
        /// </summary>
        BaseColor PreviousPageSummaryRowFontColor { get; }

        /// <summary>
        /// Summary rows background color value.
        /// At least one color and max. 2 colors should be specified. If 2 colors are introduced a gradient by using the PdfShading will be generated.
        /// </summary>
        IList<BaseColor> SummaryRowBackgroundColor { get; }

        /// <summary>
        /// Summary rows font color value.
        /// </summary>
        BaseColor SummaryRowFontColor { get; }

        /// <summary>
        /// Pages summary row background color value.
        /// At least one color and max. 2 colors should be specified. If 2 colors are introduced a gradient by using the PdfShading will be generated.
        /// </summary>
        IList<BaseColor> PageSummaryRowBackgroundColor { get; }

        /// <summary>
        /// Pages summary rows font color value.
        /// </summary>
        BaseColor PageSummaryRowFontColor { get; }

        /// <summary>
        /// Sets visibility of the main table's grid lines.
        /// </summary>
        bool ShowGridLines { get; }
    }
}
