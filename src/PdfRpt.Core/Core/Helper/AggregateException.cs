using System;
using System.Collections.Generic;
using System.Linq;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Represents one or more errors that occur during application execution.
    /// </summary>
    public class AggregateException : Exception
    {
        /// <summary>
        /// One or more errors that occurred
        /// </summary>
        public IEnumerable<Exception> InnerExceptions { get; private set; }


        /// <summary>
        /// Creates a new <see cref="AggregateException"/> containing inner exceptions
        /// </summary>
        /// <param name="innerExceptions">The exceptions that were the cause of this exception</param>
        public AggregateException(params Exception[] innerExceptions)
            : this((IEnumerable<Exception>)innerExceptions)
        {

        }


        /// <summary>
        /// Creates a new <see cref="AggregateException"/> containing inner exceptions
        /// </summary>
        /// <param name="innerExceptions">The exceptions that were the cause of this exception</param>
        public AggregateException(IEnumerable<Exception> innerExceptions)
            : this("Multiple exceptions occurred. See InnerExceptions for more details.", innerExceptions)
        {

        }


        /// <summary>
        /// Creates a new <see cref="AggregateException"/> containing inner exceptions with a message
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerExceptions">The exceptions that were the cause of this exception</param>
        public AggregateException(string message, params Exception[] innerExceptions)
            : this(message, (IEnumerable<Exception>)innerExceptions)
        {

        }


        /// <summary>
        /// Creates a new <see cref="AggregateException"/> containing inner exceptions with a message
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerExceptions">The exceptions that were the cause of this exception</param>
        public AggregateException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions.FirstOrDefault())
        {
            InnerExceptions = new List<Exception>(innerExceptions);
        }
    }
}
