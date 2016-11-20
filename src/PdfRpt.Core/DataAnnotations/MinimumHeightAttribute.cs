using System;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinimumHeightAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>
        /// <param name="minimumHeight">
        /// Height of each row will be calculated automatically based on its content. 
        /// To assure a certain cell height, without losing any content, you can set the MinimumHeight.
        /// Set MinimumHeight to zero (its default value) to ignore this setting.
        /// </param>
        public MinimumHeightAttribute(float minimumHeight)
        {
            MinimumHeight = minimumHeight;
        }

        /// <summary>
        /// Height of each row will be calculated automatically based on its content. 
        /// To assure a certain cell height, without losing any content, you can set the MinimumHeight.
        /// Set MinimumHeight to zero (its default value) to ignore this setting.
        /// </summary>
        public float MinimumHeight { private set; get; }
    }
}