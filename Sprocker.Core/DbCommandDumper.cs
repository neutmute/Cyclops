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

        public DbCommandDumper(DbCommand command)
        {
            Command = command;
        }
        
        /// <summary>
        /// Log a simulated, reproducible command text to the log
        /// </summary>
        /// <remarks>Does not recreate TVP</remarks>
        /// <param name="durationMilliseconds">When known, pass in MS for extra log metadata</param>
        /// <returns></returns>
        public string GetLogDump(int? durationMilliseconds = null)
        {
            // Format parameters into a user friendly string
            string parametersAsString = ConvertSqlParametersToCsv();

            StringBuilder simulatedCommandText = new StringBuilder();
            simulatedCommandText.Append(Command.CommandText);

            // When formulating the log text, need to do a little more work when it is a stored proc
            // in order to simulate executable text
            if (Command.CommandType == CommandType.StoredProcedure)
            {
                simulatedCommandText.Append("EXEC " + simulatedCommandText + "\r\n");
                bool prePendCommaForParameter = false;
                foreach (SqlParameter p in Command.Parameters)
                {
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

            if (durationMilliseconds != null)
            {
                traceMessage.AppendFormat("-- Execution time: {0}ms\r\n", durationMilliseconds);
            }

            traceMessage.AppendFormat("{0}\r\n", parametersAsString);
            traceMessage.Append(simulatedCommandText);
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
            foreach (SqlParameter p in Command.Parameters)
            {
                string parameterValue = ConvertToSqlString(p.Value);
                selectValues.Add(string.Format(selectFormat, p.ParameterName, parameterValue));

                // rough declaration of declare. near enough to help debug
                string declarationText = string.Format(declareFormat, p.ParameterName, p.SqlDbType.ToString().ToUpper());

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
                        declarationText += string.Format("({0})", sizeString);
                        break;
                }


                declareValues.Add(declarationText);
            }

            string declareSql = string.Empty;
            string selectSql = string.Empty;

            if (selectValues.Count > 0)
            {
                declareSql = "DECLARE " + ToCsv(declareValues) + ";";
                selectSql = "SELECT " + ToCsv(selectValues) + ";";
            }

            return declareSql + "\r\n" + selectSql;
        }

        private static string ToCsv(List<string> list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string value in list)
            {
                sb.Append("\t\t,");
                sb.AppendLine(value);
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
