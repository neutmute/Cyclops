using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NLog;

namespace Sprocker.Core
{
    internal class DbCommandDumper
    {
        public DbCommand Command { get; set; }

        public int? DurationMs { get; set; }

        public Exception ExceptionTrapped { get; set; }
        
        public DbCommandDumper(DbCommand command)
        {
            Command = command;
        }

        /// <summary>
        /// Log a simulated, reproducible command text to the log
        /// </summary>
        /// <remarks>Does not recreate TVP</remarks>
        /// <returns></returns>
        public string GetLogDump()
        {
            // Format parameters into a user friendly string
            string parametersAsString = ConvertSqlParametersToCsv();

            StringBuilder simulatedCommandText = new StringBuilder();

            // When formulating the log text, need to do a little more work when it is a stored proc
            // in order to simulate executable text
            if (Command.CommandType == CommandType.StoredProcedure)
            {
                simulatedCommandText.Append("EXEC " + Command.CommandText + "\r\n");
                bool prePendCommaForParameter = false;
                foreach (SqlParameter p in Command.Parameters)
                {
                    if (p.ParameterName == "@RETURN_VALUE")
                    {
                        continue;
                    }

                    simulatedCommandText.AppendFormat(
                        "\t\t{1}{0} = {0} "
                        , p.ParameterName
                        , prePendCommaForParameter ? "," : string.Empty);

                    prePendCommaForParameter = true;

                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        simulatedCommandText.Append(" OUTPUT");
                    }
                    simulatedCommandText.Append("\r\n");
                }
                simulatedCommandText = simulatedCommandText.Remove(simulatedCommandText.Length - 2, 2); // remove trailing semi colon
            }

            // Construct final log dump string
            StringBuilder traceMessage = new StringBuilder();
            traceMessage.Append("\r\n");

            if (ExceptionTrapped != null)
            {
                traceMessage.AppendLine("-- COMMAND FAILED: " + ExceptionTrapped.Message);
            }

            if (DurationMs != null)
            {
                traceMessage.AppendFormat("-- Execution time: {0}ms\r\n", DurationMs);
            }

            traceMessage.AppendFormat("{0}\r\n", parametersAsString);
            traceMessage.Append(simulatedCommandText);
            traceMessage.AppendLine();
            return traceMessage.ToString();
        }

        /// <summary>
        /// Formats this command's parameters to a name=value csv
        /// </summary>
        private string ConvertSqlParametersToCsv()
        {
            const string declareFormat = "{0} {1}";
            const string selectFormat = "{0} = {1}";

            List<string> selectValues = new List<string>();
            List<string> declareValues = new List<string>();
            string tableValueParamInserts = string.Empty;

            foreach (SqlParameter p in Command.Parameters)
            {
                string parameterValue = ConvertToSqlString(p.Value);
                string assignmentText = string.Format(selectFormat, p.ParameterName, parameterValue);
                string sqlTypeText = p.SqlDbType.ToString().ToUpper();

                switch (p.SqlDbType)
                {
                    case SqlDbType.NVarChar:
                    case SqlDbType.NChar:
                    case SqlDbType.VarChar:
                    case SqlDbType.Char:
                        string sizeString = p.Size.ToString();
                        if (p.Size == -1)   // handle varchar max
                        {
                            sizeString = "MAX";
                        }
                        sqlTypeText += string.Format("({0})", sizeString);
                        break;

                    case SqlDbType.Structured:
                        sqlTypeText = "dbo." + p.TypeName;
                        assignmentText = string.Empty;

                        DataTable dataTable = p.Value as DataTable;
                        if (dataTable != null)
                        {
                            StringBuilder tableInserts = new StringBuilder();
                            foreach (DataRow dataRow in dataTable.Rows)
                            {
                                tableInserts.AppendFormat("INSERT INTO {0} VALUES (", p.ParameterName);
                                for(int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                                {
                                    tableInserts.AppendFormat("{0},", ConvertToSqlString(dataRow[columnIndex]));
                                }
                                tableInserts.Remove(tableInserts.Length - 1, 1); //trailing comma removal
                                tableInserts.AppendFormat(")\r\n");
                            }
                            tableValueParamInserts += tableInserts.ToString();
                        }
                        break;
                }

                string declarationText = string.Format(declareFormat, p.ParameterName, sqlTypeText);
                declareValues.Add(declarationText);

                selectValues.Add(assignmentText);
            }

            string declareSql = string.Empty;
            string selectSql = string.Empty;

            if (selectValues.Count > 0)
            {
                declareSql = "DECLARE " + ToCsv(declareValues) + ";";
                selectSql = "SELECT " + ToCsv(selectValues) + ";";

                if (!string.IsNullOrEmpty(tableValueParamInserts))
                {
                    selectSql += "\r\n\r\n" + tableValueParamInserts;
                }
            }

            return declareSql + "\r\n\r\n" + selectSql;
        }

        private static string ToCsv(List<string> list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string value in list)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append("\t\t,");
                    sb.AppendLine(value);
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(0, 3); // Remove leading ", "
            }

            // Keep the semicolon on the last line
            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }

        /// <summary>
        /// Given a dotnet object, convert to its sql string equivalent
        /// </summary>
        /// <param name="o">The object to be converted</param>
        /// <returns>A sql string representation of the object</returns>
        private static string ConvertToSqlString(object o)
        {
            string s;

            if (o == null || o == DBNull.Value)
            {
                s = "NULL";
            }
            else
            {
                Type type = o.GetType();

                if (type == typeof(DateTime))
                {
                    s = string.Format("'{0}'", Convert.ToDateTime(o).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (type == typeof(bool))
                {
                    bool b = Convert.ToBoolean(o);
                    s = b ? "1" : "0";
                }
                else if (type == typeof(string))
                {
                    s = string.Format("'{0}'", Convert.ToString(o).Replace("'", "''"));
                }
                else // for remaining data types; for most purposes these should only be numerics
                {
                    s = Convert.ToString(o);
                }
            }

            return s;
        }
    }
}
