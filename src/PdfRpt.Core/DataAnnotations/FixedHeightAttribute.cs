using System;

namespace PdfRpt.DataAnnotations
{
    /// <summary>
    /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FixedHeightAttribute : Attribute
    {
        /// <summary>
        /// Defining how a property of MainTableDataSource should be rendered as a column's cell.
        /// </summary>        
        /// <param name="fixedHeight">
        /// Height of each row will be calculated automatically based on its content. 
        /// Also you can set the FixedHeight to define the height yourself.
        /// In this case the overflowed text with be trimmed. 
        /// Set FixedHeight to zero (its default value) to ignore this setting. 
        /// </param>
        public FixedHeightAttribute(float fixedHeight)
        {
            FixedHeight = fixedHeight;
        }

        /// <summary>
        /// Height of each row will be calculated automatically based on its content. 
        /// Also you can set the FixedHeight to define the height yourself.
        /// In this case the overflowed text with be trimmed. 
        /// Set FixedHeight to zero (its default value) to ignore this setting.
        /// </summary>
        public float FixedHeight { private set; get; }
    }
}