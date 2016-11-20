using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// This class tries to find some Pdf elements width, before rendering.
    /// </summary>
    public static class ElementsWidth
    {
        /// <summary>
        /// Tries to auto resize the specified table columns.
        /// </summary>
        /// <param name="table">pdf table</param>
        public static void AutoResizeTableColumns(this PdfGrid table)
        {
            if (table == null) return;
            var currentRowWidthsList = new List<float>();
            var previousRowWidthsList = new List<float>();

            foreach (PdfPRow row in table.Rows)
            {
                currentRowWidthsList.Clear();
                currentRowWidthsList.AddRange(row.GetCells().Select(cell => cell.GetCellWidth()));

                if (!previousRowWidthsList.Any())
                {
                    previousRowWidthsList = new List<float>(currentRowWidthsList);
                }
                else
                {
                    for (int i = 0; i < previousRowWidthsList.Count; i++)
                    {
                        if (previousRowWidthsList[i] < currentRowWidthsList[i])
                            previousRowWidthsList[i] = currentRowWidthsList[i];
                    }
                }
            }

            if (previousRowWidthsList.Any())
                table.SetTotalWidth(previousRowWidthsList.ToArray());
        }

        /// <summary>
        /// Tries to find the PdfPCell's width, before rendering
        /// </summary>
        /// <param name="cell">pdf cell</param>
        /// <returns>its estimated width</returns>
        public static float GetCellWidth(this PdfPCell cell)
        {
            if (cell == null) return 0;

            var rulesWidth = new List<float>();
            var startWidth = cell.PaddingLeft + cell.PaddingRight + 1 + cell.BorderWidth;
            var cellWidth = startWidth;

            var compositeElements = cell.CompositeElements;
            if (compositeElements == null)
            {
                if (cell.Image != null)
                {
                    cellWidth += cell.Image.Width;
                }

                if (cell.Phrase != null)
                {
                    cellWidth += GetPhraseWidth(cell.Phrase);
                }

                if (cell.Table != null)
                {
                    cellWidth += cell.Table.TotalWidth;
                }

                return cellWidth;
            }

            foreach (IElement baseLevel in compositeElements)
            {
                if (baseLevel is Phrase)
                {
                    cellWidth += GetPhraseWidth((Phrase)baseLevel);
                    rulesWidth.Add(cellWidth);
                    cellWidth = startWidth;
                }
                else if (baseLevel is List)
                {
                    cellWidth = getListWidth(rulesWidth, startWidth, cellWidth, baseLevel);
                    rulesWidth.Add(cellWidth);
                    cellWidth = startWidth;
                }
                else if (baseLevel is PdfGrid)
                {
                    rulesWidth.Add(cellWidth);
                    cellWidth = startWidth + ((PdfGrid)baseLevel).TotalWidth;
                    rulesWidth.Add(cellWidth);
                    cellWidth = startWidth;
                }
            }

            return rulesWidth.Concat(new[] { cellWidth }).Max();
        }

        private static float getListWidth(IList<float> rulesWidth, float startWidth, float cellWidth, IElement baseLevel)
        {
            foreach (var li in ((List)baseLevel).Items)
            {
                rulesWidth.Add(cellWidth);
                cellWidth = startWidth + ((ListItem)li).IndentationLeft;
                foreach (Chunk c in ((ListItem)li).Chunks)
                    cellWidth += c.GetWidthPoint();
            }
            return cellWidth;
        }

        /// <summary>
        /// Tries to find the Phrase's width, before rendering
        /// </summary>
        /// <param name="phrase">the phrase</param>
        /// <returns>its estimated width</returns>
        public static float GetPhraseWidth(this Phrase phrase)
        {
            return phrase.OfType<Chunk>().Sum(inner => (inner).GetWidthPoint());
        }
    }
}
