using System;
using System.Linq;
using System.Reflection;
using PdfRpt.Core.Contracts;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CustomAggregateFunctionAttribute : Attribute
    {
        /// <summary>
        /// A typeof(IAggregateFunction) value.
        /// </summary>
        public Type AggregateFunctionType { private set; get; }

        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="aggregateFunctionType">A typeof(IAggregateFunction) value, such as typeof(Sum).</param>
        public CustomAggregateFunctionAttribute(Type aggregateFunctionType)
        {
            if (aggregateFunctionType == null)
                throw new ArgumentNullException("aggregateFunctionType");

            if (!aggregateFunctionType.GetTypeInfo().GetInterfaces().Contains(typeof(IAggregateFunction)))
                throw new ArgumentException("The aggregateFunctionType Type must typeof(IAggregateFunction).", "aggregateFunctionType");

            if (aggregateFunctionType.GetConstructor(Type.EmptyTypes) == null)
                throw new ArgumentException("The aggregateFunctionType type must declare a public parameterless consructor.", "aggregateFunctionType");

            AggregateFunctionType = aggregateFunctionType;
        }
    }
}