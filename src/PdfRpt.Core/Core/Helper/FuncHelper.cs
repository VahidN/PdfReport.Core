using System;
using System.Collections.Generic;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Formula properties helpers
    /// </summary>
    public static class FuncHelper
    {
        /// <summary>
        /// Applies CalculatedFieldFormula safely.
        /// It returns string.Empty if tableRowData or formula are null.
        /// </summary>
        /// <param name="formula">A formula to apply</param>
        /// <param name="tableRowData">Passed data</param>
        /// <returns>Invoked formula</returns>
        public static object ApplyCalculatedFieldFormula(
                                    Func<IList<CellData>, object> formula,
                                    IList<CellData> tableRowData)
        {
            if (tableRowData == null) return string.Empty;
            return formula == null ? string.Empty : formula.Invoke(tableRowData);
        }

        /// <summary>
        /// Applies formula safely.
        /// It returns string.Empty if data or formula are null.
        /// </summary>
        /// <param name="formula">A formula to apply</param>
        /// <param name="data">Passed data</param>
        /// <returns>Invoked formula</returns>
        public static string ApplyFormula(
                                    Func<object, string> formula,
                                    object data)
        {
            if (data == null) return string.Empty;
            return formula == null ? data.ToString() : formula.Invoke(data);
        }
    }
}
