using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Common.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace Cyclops
{
    /// <summary>
    /// </summary>
    /// <remarks>CyclopsCommand name since 'Cyclops' by itself conflicts with namespace</remarks>
    public class CyclopsCommand
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly CyclopsParameterMapper _parameterMapper;
        #endregion
        
        #region Properties

        internal DbCommand DbCommand {get; private set;}

        public Database Database { get; set; }

        public string CommandText { get; set; }

        public int CommandTimeout
        {
            get { return DbCommand.CommandTimeout; }
            set { DbCommand.CommandTimeout = value; }
        }

        public DbParameterCollection Parameters { get { return DbCommand.Parameters; } }

        #endregion

        #region Constructor
        public CyclopsCommand(Database database, CommandType commandType, string commandTextFormat, params object[] commandTextArgs) : this(database, "placeholderCommand")
        {
            DbCommand.CommandText = string.Format(commandTextFormat, commandTextArgs);
            DbCommand.CommandType = commandType;
        }

        public CyclopsCommand(Database database, string procedureName)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }

            if (procedureName == null)
            {
                throw new ArgumentNullException("procedureName");
            }

            Database = database;
            CommandText = procedureName;
            DbCommand = Database.GetStoredProcCommand(CommandText);
            _parameterMapper = new CyclopsParameterMapper(Database);

            ReadGlobalConfig();
        }

        private void ReadGlobalConfig()
        {
            const string commandTimeoutKey = "CommandTimeout";
            if (ConfigurationManager.AppSettings[commandTimeoutKey] != null)
            {
                CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings[commandTimeoutKey]);
            }
        }
        #endregion
        
        #region Execute* Methods

        public int ExecuteNonQuery(params object[] parameters)
        {
            _parameterMapper.AssignParameters(DbCommand, parameters);
            return ExecuteNonQuery();
        }

        public int ExecuteNonQuery()
        {
            int result;
            var commandLogger = new DbCommandLogger(this);
            try
            {
                result = Database.ExecuteNonQuery(DbCommand);
            }
            catch (Exception e)
            {
                commandLogger.ExceptionTrapped(e);
                throw;
            }
            finally
            {
                commandLogger.Complete();
            }
            return result;
        }

        public List<TEntity> ExecuteRowMap<TEntity>()  where TEntity : new()
        {
            return ExecuteRowMap(EntityMapper.GetDefaultMapper<TEntity>());
        }

        public List<TEntity> ExecuteRowMap<TEntity>(IRowMapper<TEntity> mapper)
        {
            DataTable dataTable = ExecuteDataTable();
            List<TEntity> entities = EntityMapper.Map(dataTable, mapper);
            return entities;
        }

        public TableSet ExecuteTableSet(params object[] parameters)
        {
            _parameterMapper.AssignParameters(DbCommand, parameters);
            return ExecuteTableSet();
        }

        public TableSet ExecuteTableSet()
        {
            return TableSet.Create(ExecuteDataSet());
        }

        public DataSet ExecuteDataSet()
        {
            DataSet dataSet;
            var commandLogger = new DbCommandLogger(this);
            try
            {
                dataSet = Database.ExecuteDataSet(DbCommand);
            }
            catch (Exception e)
            {
                commandLogger.ExceptionTrapped(e);
                throw;
            }
            finally
            {
                commandLogger.Complete();
            }

            return dataSet;
        }

        public DataTable ExecuteDataTable(params object[] parameters)
        {
            _parameterMapper.AssignParameters(DbCommand, parameters);
            return ExecuteDataTable();
        }

        public DataTable ExecuteDataTable()
        {
            DataSet dataSet = ExecuteDataSet();
            switch(dataSet.Tables.Count)
            {
                case 0:                 
                    return null;
                case 1:                 
                    return dataSet.Tables[0];
                default:                            // If more than one and we only asked for one, infer that this proc is returning a named record set load pattern
                    return dataSet.Tables[1];
            }
        }

        #endregion

        #region Parameter Helper Methods
        /// <summary>
        /// Extract an output param
        /// </summary>
        public T GetParameterValue<T>(string parameterName) where T : struct
        {
            SqlParameter sqlParameter = ((SqlParameter) Parameters[parameterName]);
            object value = sqlParameter.Value;
            return (value == null || value == DBNull.Value) ? default(T) : (T)value;
        }

        public void SetParameterToStructuredType(string parameterName, string typeName)
        {
            
            ((SqlParameter)DbCommand.Parameters[parameterName]).SqlDbType = SqlDbType.Structured;
            ((SqlParameter)DbCommand.Parameters[parameterName]).TypeName = typeName;
        }
        #endregion

        #region AddParameter helpers (for when dynamic SQL is used and parameters can't be automatically bound)

        /// <summary>
        /// Create and set the parameter (use when commandType is Text)
        /// </summary>
        public void AddParameter(string name, int value)
        {
            AddParameter(name, value, ParameterDirection.Input);
        }

        /// <summary>
        /// Create and set the parameter (use when commandType is Text)
        /// </summary>
        public void AddParameter(string name, int value, ParameterDirection direction)
        {
            var parameter = new SqlParameter(name, value);
            parameter.Direction = direction;
            AddParameter(parameter);
        }

        /// <summary>
        /// Create and set the parameter (use when commandType is Text)
        /// </summary>
        public void AddParameter(string name, string value)
        {
            var p = new SqlParameter(name, SqlDbType.NVarChar);
            p.Value = value;
            AddParameter(p);
        }

        /// <summary>
        /// Create and set the parameter (use when commandType is Text)
        /// </summary>
        public void AddParameter(string name, DateTime value)
        {
            SqlParameter parameter = new SqlParameter(name, value);
            AddParameter(parameter);
        }

        /// <summary>
        /// Create and set the parameter (use when commandType is Text)
        /// </summary>
        public void AddParameter(string name, object value)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }

            var parameter = new SqlParameter(name, value);
            AddParameter(parameter);
        }

        /// <summary>
        /// Create and set the parameter (use when commandType is Text)
        /// </summary>
        public void AddParameter(string name, object value, SqlDbType dataType)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }

            SqlParameter parameter = new SqlParameter(name, dataType);
            parameter.Value = value;
            AddParameter(parameter);
        }

        public void AddParameter(DbParameter parameter)
        {
            Parameters.Add(parameter);
        }
        #endregion
    }

    #region ResultSet Mapper
    /// <summary>
    /// This private class is replicated without alteration from the base <see cref="CommandAccessor{TResult}"/> class
    /// as it is called here by some overridden mehtods.
    /// </summary>
    class CyclopsResultSetMapper<TResult> : IResultSetMapper<TResult>
    {
        readonly IRowMapper<TResult> _rowMapper;

        public CyclopsResultSetMapper(IRowMapper<TResult> rowMapper)
        {
            _rowMapper = rowMapper;
        }

        public IEnumerable<TResult> MapSet(IDataReader reader)
        {
            using (reader)
            {
                while (reader.Read())
                {
                    yield return _rowMapper.MapRow(reader);
                }
            }
        }

    }
    #endregion
    
    #region ParameterMapper

    /// <summary>
    /// This private class is replicated without alteration from the base <see cref="SprocAccessor{TResult}"/> class
    /// as it is called here by some overridden mehtods.
    /// </summary>
    internal class CyclopsParameterMapper : IParameterMapper
    {
        readonly Database _database;
        public CyclopsParameterMapper(Database database)
        {
            _database = database;
        }

        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
            if (parameterValues.Length > 0)
            {
                GuardParameterDiscoverySupported();
                try
                {
                    _database.AssignParameters(command, parameterValues);
                }
                catch(InvalidOperationException e)
                {
                    var paramNames = (
                                        from DbParameter parameter in command.Parameters 
                                        where parameter.Direction != ParameterDirection.ReturnValue
                                        select parameter.ParameterName
                                     ).ToList();

                    string message = string.Format(
                        "Failed to auto assign parameters. [{0}] => {2}({1})"
                        , string.Join(",", parameterValues)
                        , string.Join(",", paramNames)
                        , command.CommandText
                        );

                    throw CyclopsException.Create(e, message);
                }
            }
        }

        private void GuardParameterDiscoverySupported()
        {
            if (!_database.SupportsParemeterDiscovery)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                                  "The database type \"{0}\" does not support automatic parameter discovery. Use an IParameterMapper instead.",
                                  _database.GetType().FullName));
            }
        }
    }

    #endregion
}
