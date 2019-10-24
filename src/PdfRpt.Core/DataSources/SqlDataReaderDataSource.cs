using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PdfRpt.Core.Contracts;

namespace PdfRpt.DataSources
{
    /// <summary>
    /// SQL Server DataSource
    /// </summary>
    public class SqlDataReaderDataSource : IDataSource
    {
        #region Fields (3)

        private readonly string _connectionString;
        private readonly object[] _paramValues;
        private readonly string _sql;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Converts the selected records to an IEnumerable of the Pdf Cells Data
        /// </summary>
        /// <param name="connectionString">the connection string</param>
        /// <param name="sql">SQL statement to select the required records</param>
        /// <param name="parametersValues">values of the parameters started with @</param>
        public SqlDataReaderDataSource(string connectionString, string sql, params object[] parametersValues)
        {
            _connectionString = connectionString;
            _sql = sql;
            _paramValues = parametersValues;
        }

        #endregion Constructors

        #region Methods (1)

        // Public Methods (1)

        /// <summary>
        /// The data to render.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IList<CellData>> Rows()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                using (var sqlCommand = new SqlCommand(_sql, sqlConnection) { CommandTimeout = 1200 })
                {
                    SqlParametersParser.ApplySafeParameters(sqlCommand, _sql, _paramValues);
                    sqlCommand.Connection.Open();

                    using (var sqlReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            var result = new List<CellData>();
                            for (var i = 0; i < sqlReader.FieldCount; i++)
                            {
                                var value = sqlReader.GetValue(i);
                                var pdfCellData = new CellData
                                {
                                    PropertyName = sqlReader.GetName(i),
                                    PropertyValue = value == DBNull.Value ? null : value,
                                    PropertyIndex = i,
                                    PropertyType = sqlReader.GetFieldType(i)
                                };
                                result.Add(pdfCellData);
                            }
                            yield return result;
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}
