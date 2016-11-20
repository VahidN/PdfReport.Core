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
    public sealed class ColumnItemsTemplateAttribute : Attribute
    {
        /// <summary>
        /// A typeof(IPdfColumnItemsTemplate) value, such as typeof(TextBlockField).
        /// </summary>
        public Type TemplateType { private set; get; }

        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="templateType">A typeof(IPdfColumnItemsTemplate) value, such as typeof(TextBlockField).</param>
        public ColumnItemsTemplateAttribute(Type templateType)
        {
            if (templateType == null)
                throw new ArgumentNullException("templateType");

            if (!templateType.GetInterfaces().Contains(typeof(IColumnItemsTemplate)))
                throw new ArgumentException("The templateType Type must typeof(IPdfColumnItemsTemplate).", "templateType");

            if (templateType.GetConstructor(Type.EmptyTypes) == null)
                throw new ArgumentException("The templateType type must declare a public parameterless consructor.", "templateType");

            TemplateType = templateType;
        }
    }
}