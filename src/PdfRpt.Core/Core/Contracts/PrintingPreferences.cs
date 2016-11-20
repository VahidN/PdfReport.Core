
namespace PdfRpt.Core.Contracts
{
    /// <summary>
    /// Indicates default values of print dialog box.
    /// </summary>
    public class PrintingPreferences
    {
        /// <summary>
        /// Print scaling values.
        /// </summary>
        public PrintScaling PrintScaling { set; get; }

        /// <summary>
        /// Indicates whether duplex printing should be used or not.
        /// </summary>
        public PrintSide PrintSide { set; get; }

        /// <summary>
        /// If set to true, the check box in the Print dialog box associated with input paper tray will be checked 
        /// (Automatically check the "Choose paper source by PDF page size" checkbox on the print dialog). 
        /// </summary>
        public bool PickTrayByPdfSize { set; get; }

        /// <summary>
        /// Supported values are the integers 2 through 5. Values outside this range are ignored.
        /// </summary>
        public int NumberOfCopies { set; get; }

        /// <summary>
        /// Show the standard print dialog after opening the document automatically.
        /// </summary>
        public bool ShowPrintDialogAutomatically { set; get; }
    }
}
