using System;
using System.Collections.Generic;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Defines how to display the current cell's data
    /// </summary>
    public class InlineFieldBuilder
    {
        private readonly InlineField _builder = new InlineField();

        internal InlineField InlineField
        {
            get { return _builder; }
        }

        /// <summary>
        /// Table's Cells Definitions. If you don't set this value, 
        /// it will be filled by using current template's settings internally.
        /// </summary>
        public void BasicProperties(CellBasicProperties properties)
        {
            _builder.BasicProperties = properties;
        }

        /// <summary>
        /// Defines the current cell's properties, based on the other cells values.
        /// Here IList contains actual row's cells values.
        /// It can be null.
        /// </summary>
        public void ConditionalFormatFormula(Func<IList<CellData>, CellBasicProperties> formula)
        {
            _builder.ConditionalFormatFormula = formula;
        }

        /// <summary>
        /// It will be called at the end of the cell's rendering.
        /// </summary>
        public void DrawOnCell(System.Action<InlineFieldData> cell)
        {
            _builder.DrawOnCell = cell;
        }

        /// <summary>
        /// Custom cell's content template as a PdfPCell
        /// </summary>
        public void RenderCell(Func<InlineFieldData, iTextSharp.text.pdf.PdfPCell> cell)
        {
            _builder.RenderCell = cell;
        }
    }
}
