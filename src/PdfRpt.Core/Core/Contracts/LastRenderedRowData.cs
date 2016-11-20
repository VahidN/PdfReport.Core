using System.Collections.Generic;

namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// A class to hold last result of the actual rendering engine of iTextSharp during its processes.
    /// For instance defining a header is not equal to adding its content to the page. Only after its 
    /// addition/rendering we can find its actual height.
    /// </summary>
    public class LastRenderedRowData
    {
        /// <summary>
        /// Actual height of the header after its addition to the page
        /// </summary>
        public float HeaderHeight { set; get; }

        /// <summary>
        /// Last rendered group's row number 
        /// </summary>
        public int LastGroupRowNumber { set; get; }

        /// <summary>
        /// Last rendered overall row number 
        /// </summary>
        public int LastOverallDataRowNumber { set; get; }

        /// <summary>
        /// Last rendered group number 
        /// </summary>
        public int LastRenderedGroupNumber { set; get; }

        /// <summary>
        /// Last rendered row number which can be different in multiple groups
        /// </summary>
        public int LastRenderedRowNumber { set; get; }

        /// <summary>
        /// Determines if a new group is being added to the document
        /// </summary>
        public bool IsNewGroupStarted { set; get; }

        /// <summary>
        /// Last defined group number 
        /// </summary>
        public int LastGroupNumber { set; get; }

        /// <summary>
        /// Determines firstRow and lastRow of the page in overall row numbers 
        /// </summary>
        public IList<int> PagesBoundaries { set; get; }

        /// <summary>
        /// Previous row's data.
        /// </summary>
        public IList<CellData> PreviousTableRowData { set; get; }

        /// <summary>
        /// ctor.
        /// </summary>
        public LastRenderedRowData()
        {
            PagesBoundaries = new List<int>();
            PreviousTableRowData = new List<CellData>();
        }
    }
}
