namespace PdfRpt.Core.Contracts
{    
    /// <summary>
    /// Defines metadata information of the Document.
    /// </summary>
    public class DocumentMetadata
    {
        /// <summary>
        /// Adds the title to a Document.
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// Adds the subject to a Document.
        /// </summary>
        public string Subject { set; get; }

        /// <summary>
        /// Adds the author to a Document.
        /// </summary>
        public string Author { set; get; }

        /// <summary>
        /// Adds the creator to a Document.
        /// </summary>
        public string Application { set; get; }

        /// <summary>
        /// Adds the keywords to a Document.
        /// </summary>
        public string Keywords { set; get; }
    }
}
