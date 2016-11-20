using System;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Aggregate Function Builder Class
    /// </summary>
    public class AggregateFunctionBuilder
    {
        IAggregateFunction _aggregateFunction;

        /// <summary>
        /// Gets the Aggregate Function
        /// </summary>
        internal IAggregateFunction AggregateFunction
        {
            get { return _aggregateFunction; }
        }

        /// <summary>
        /// Custom Aggregate Function
        /// It can be null.
        /// </summary>
        public void CustomAggregateFunction(IAggregateFunction aggregateFunction)
        {
            _aggregateFunction = aggregateFunction;
        }

        /// <summary>
        /// Fires before rendering of this cell.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public void DisplayFormatFormula(Func<object, string> formula)
        {
            _aggregateFunction.DisplayFormatFormula = formula;
        }

        /// <summary>
        /// A set of a predefined aggregate functions.
        /// It only works with numbers. If you want to apply it on other data types, you need to create your own AggregateFunction by implementing the IAggregateFunc.
        /// </summary>
        public void NumericAggregateFunction(AggregateFunction aggregateFunction)
        {
            _aggregateFunction = new AggregateProvider(aggregateFunction);
        }
    }
}
