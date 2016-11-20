using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// PdfCellAttributes helper class.
    /// </summary>
    public static class TableCellDefinitionsExt
    {
        /// <summary>
        /// Create a PdfPCell based on the PdfCell Attributes.
        /// </summary>
        /// <param name="pdfRptTableCellDefinition">PdfCell Attributes</param>
        /// <param name="defaultItemTemplate">Default ItemTemplate</param>
        /// <returns>A PdfPCell</returns>
        public static PdfPCell CreateSafePdfPCell(this CellAttributes pdfRptTableCellDefinition, IColumnItemsTemplate defaultItemTemplate)
        {
            mapBasicProperties(pdfRptTableCellDefinition);
            runConditionalFormatFormula(pdfRptTableCellDefinition);

            var pdfPCell = runOnItemsTemplate(pdfRptTableCellDefinition, defaultItemTemplate);
            pdfPCell.ApplyStyles(pdfRptTableCellDefinition);
            return pdfPCell;
        }

        private static void mapBasicProperties(CellAttributes pdfRptTableCellDefinition)
        {
            if (pdfRptTableCellDefinition.ItemTemplate != null && pdfRptTableCellDefinition.ItemTemplate.BasicProperties != null)
            {
                pdfRptTableCellDefinition.ItemTemplate.BasicProperties.MapBasicPropertiesTo(pdfRptTableCellDefinition.BasicProperties);
            }
            applyTemplateColors(pdfRptTableCellDefinition.BasicProperties, pdfRptTableCellDefinition.BasicProperties);
        }

        private static void applyTemplateColors(CellBasicProperties from, CellBasicProperties to)
        {
            if (to.PdfFont != null)
            {
                if (from.PdfFontStyle != null)
                    to.PdfFont.Style = from.PdfFontStyle.Value;

                if (from.FontColor != null)
                    to.PdfFont.Color = from.FontColor;
            }
        }

        private static void runConditionalFormatFormula(CellAttributes pdfRptTableCellDefinition)
        {
            if (pdfRptTableCellDefinition.ItemTemplate != null && pdfRptTableCellDefinition.ItemTemplate.ConditionalFormatFormula != null)
            {
                var conditionalPdfCellAttributes = pdfRptTableCellDefinition.ItemTemplate.ConditionalFormatFormula(pdfRptTableCellDefinition.RowData.TableRowData);
                if (conditionalPdfCellAttributes != null)
                {
                    conditionalPdfCellAttributes.MapBasicPropertiesTo(pdfRptTableCellDefinition.BasicProperties);
                    applyTemplateColors(conditionalPdfCellAttributes, pdfRptTableCellDefinition.BasicProperties);
                }
            }
        }

        private static PdfPCell runOnItemsTemplate(CellAttributes pdfRptTableCellDefinition, IColumnItemsTemplate defaultItemTemplate)
        {
            return
                pdfRptTableCellDefinition.ItemTemplate == null ?
                    defaultItemTemplate.RenderingCell(pdfRptTableCellDefinition) :
                    pdfRptTableCellDefinition.ItemTemplate.RenderingCell(pdfRptTableCellDefinition);
        }

        /// <summary>
        /// Applies PdfCellAttributes to a PdfPCell.
        /// </summary>
        /// <param name="pdfPCell">A PdfPCell.</param>
        /// <param name="pdfRptTableCellDefinition">PdfCell Attributes</param>
        public static void ApplyStyles(this PdfPCell pdfPCell, CellAttributes pdfRptTableCellDefinition)
        {
            if (pdfRptTableCellDefinition.BasicProperties.RunDirection.HasValue)
                pdfPCell.RunDirection = (int)pdfRptTableCellDefinition.BasicProperties.RunDirection;

            pdfPCell.BorderColor = pdfRptTableCellDefinition.BasicProperties.BorderColor;
            pdfPCell.BackgroundColor = pdfRptTableCellDefinition.BasicProperties.BackgroundColor;

            if (pdfRptTableCellDefinition.BasicProperties.HorizontalAlignment != null)
                pdfPCell.HorizontalAlignment = (int)pdfRptTableCellDefinition.BasicProperties.HorizontalAlignment;

            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.Rotation = pdfRptTableCellDefinition.BasicProperties.Rotation;
            pdfPCell.UseAscender = true;
            pdfPCell.UseDescender = true;

            if (pdfRptTableCellDefinition.BasicProperties.CellPadding > 0)
                pdfPCell.Padding = pdfRptTableCellDefinition.BasicProperties.CellPadding;

            if (pdfRptTableCellDefinition.BasicProperties.PaddingBottom > 0)
                pdfPCell.PaddingBottom = pdfRptTableCellDefinition.BasicProperties.PaddingBottom;

            if (pdfRptTableCellDefinition.BasicProperties.PaddingLeft > 0)
                pdfPCell.PaddingLeft = pdfRptTableCellDefinition.BasicProperties.PaddingLeft;

            if (pdfRptTableCellDefinition.BasicProperties.PaddingRight > 0)
                pdfPCell.PaddingRight = pdfRptTableCellDefinition.BasicProperties.PaddingRight;

            if (pdfRptTableCellDefinition.BasicProperties.PaddingTop > 0)
                pdfPCell.PaddingTop = pdfRptTableCellDefinition.BasicProperties.PaddingTop;

            if (pdfRptTableCellDefinition.BasicProperties.FixedHeight > 0)
                pdfPCell.FixedHeight = pdfRptTableCellDefinition.BasicProperties.FixedHeight;

            if (pdfRptTableCellDefinition.BasicProperties.MinimumHeight > 0)
                pdfPCell.MinimumHeight = pdfRptTableCellDefinition.BasicProperties.MinimumHeight;

            if (pdfRptTableCellDefinition.BasicProperties.BorderWidth > 0)
                pdfPCell.BorderWidth = pdfRptTableCellDefinition.BasicProperties.BorderWidth;

            if (!pdfRptTableCellDefinition.BasicProperties.ShowBorder)
                pdfPCell.BorderWidth = 0;
        }

        /// <summary>
        /// Maps ItemTemplate.BasicProperties to pdfRptTableCellDefinition.BasicProperties.
        /// </summary>
        /// <param name="fromPdfCellAttributes">From PdfCell Attributes</param>
        /// <param name="toPdfCellAttributes">To PdfCell Attributes</param>
        public static void MapBasicPropertiesTo(this CellBasicProperties fromPdfCellAttributes, CellBasicProperties toPdfCellAttributes)
        {
            if (fromPdfCellAttributes != null)
            {
                var borderWidth = fromPdfCellAttributes.BorderWidth;
                if (borderWidth > 0)
                    toPdfCellAttributes.BorderWidth = borderWidth;

                var cellPadding = fromPdfCellAttributes.CellPadding;
                if (cellPadding > 0)
                    toPdfCellAttributes.CellPadding = cellPadding;

                var fixedHeight = fromPdfCellAttributes.FixedHeight;
                if (fixedHeight > 0)
                    toPdfCellAttributes.FixedHeight = fixedHeight;

                var minimumHeight = fromPdfCellAttributes.MinimumHeight;
                if (minimumHeight > 0)
                    toPdfCellAttributes.MinimumHeight = minimumHeight;

                var fontColor = fromPdfCellAttributes.FontColor;
                if (fontColor != null)
                    toPdfCellAttributes.FontColor = fontColor;

                var backgroundColor = fromPdfCellAttributes.BackgroundColor;
                if (backgroundColor != null)
                    toPdfCellAttributes.BackgroundColor = backgroundColor;

                var pdfFont = fromPdfCellAttributes.PdfFont;
                if (pdfFont != null)
                    toPdfCellAttributes.PdfFont = pdfFont;

                var rotation = fromPdfCellAttributes.Rotation;
                if (rotation != 0)
                    toPdfCellAttributes.Rotation = rotation;

                var runDirection = fromPdfCellAttributes.RunDirection;
                if (runDirection != null && runDirection != PdfRunDirection.None)
                    toPdfCellAttributes.RunDirection = runDirection;

                var horizontalAlignment = fromPdfCellAttributes.HorizontalAlignment;
                if (horizontalAlignment != null && horizontalAlignment != HorizontalAlignment.None)
                    toPdfCellAttributes.HorizontalAlignment = horizontalAlignment;

                var displayFormatFormula = fromPdfCellAttributes.DisplayFormatFormula;
                if (displayFormatFormula != null && toPdfCellAttributes.DisplayFormatFormula == null)
                    toPdfCellAttributes.DisplayFormatFormula = displayFormatFormula;
            }
        }
    }
}