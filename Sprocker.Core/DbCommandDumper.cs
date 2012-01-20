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
    public class DbCommandDumper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public DbCommand Command { get; set; }

        public DbCommandDumper(DbCommand command)
        {
            Command = command;
        }
        
        public string GetLogDump()
        {
            // Format parameters into a user friendly string
            string parametersAsString = ConvertSqlParametersToCsv();

            string commandText = Command.CommandText;

            // When formulating the log text, need to do a little more work when it is a stored proc
            // in order to simulate executable text
            if (Command.CommandType == CommandType.StoredProcedure)
            {
                commandText = "EXEC " + commandText + "\r\n";
                bool prePendCommaForParameter = false;
                foreach (SqlParameter p in Command.Parameters)
                {
                    commandText += string.Format(
                        "\t\t{1}{0} = {0} "
                        , p.ParameterName
                        , prePendCommaForParameter ? "," : string.Empty);

                    prePendCommaForParameter = true;

                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        commandText += " OUTPUT";
                    }
                    commandText += "\r\n";
                }
                commandText = commandText.Remove(commandText.Length - 2, 2); // remove trailing semi colon
            }

            string traceMessage = string.Format("\r\n{0}\r\n{1};", parametersAsString, commandText);
            return traceMessage;
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
