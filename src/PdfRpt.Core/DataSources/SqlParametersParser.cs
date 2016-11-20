using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace PdfRpt.DataSources
{
    /// <summary>
    /// A class to parse defined parameters in sql statements automatically.
    /// </summary>
    public static class SqlParametersParser
    {
        #region Fields (2)

        const string Pattern = @"(?<Parameter>@\w*)";
        static readonly Regex RegEx = new Regex(Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion Fields

        #region Methods (3)

        // Public Methods (3) 

        /// <summary>
        /// Parsing defined parameters in sql statements and converting them to DbParameters automatically
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <param name="sqlWithAtSignParameters">sql statement with parameters started with @</param>
        /// <param name="parametersValues">parameter(s) value(s)</param>
        public static void ApplySafeParameters(DbCommand cmd, string sqlWithAtSignParameters, params object[] parametersValues)
        {
            if (string.IsNullOrEmpty(sqlWithAtSignParameters)) return;
            if (parametersValues == null || parametersValues.Length < 0) return;
            var parms = createParameters(cmd, sqlWithAtSignParameters, parametersValues);
            if (parms == null) return;
            foreach (var param in parms)
                cmd.Parameters.Add(param);
        }

        private static IEnumerable<DbParameter> createParameters(DbCommand cmd, string commandText, params object[] paramValues)
        {
            if (string.IsNullOrEmpty(commandText)) return null;
            if (paramValues == null || paramValues.Length == 0) return null;
            var coll = new List<DbParameter>();

            var idx = commandText.IndexOf("@", StringComparison.CurrentCultureIgnoreCase);
            if (idx == -1)
                return null;

            var parmString = commandText.Substring(idx);
            parmString = parmString.Replace(",", " ,");
            var mc = RegEx.Matches(parmString);

            var matchesList = removeDuplicates(mc);

            if (matchesList.Count != paramValues.Length)
                throw new ArgumentOutOfRangeException("paramValues");

            for (var i = 0; i < matchesList.Count; i++)
            {
                DbParameter parm = cmd.CreateParameter();
                parm.ParameterName = matchesList[i];
                parm.Value = paramValues[i] ?? DBNull.Value;
                coll.Add(parm);
            }

            return coll;
        }

        private static IList<string> removeDuplicates(MatchCollection ms)
        {
            var lstMatches = new List<string>();

            foreach (Match match in ms)
            {
                if (!lstMatches.Contains(match.Value))
                    lstMatches.Add(match.Value);
            }

            return lstMatches;
        }

        #endregion Methods
    }
}
