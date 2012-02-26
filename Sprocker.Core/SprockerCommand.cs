using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NLog;

namespace Sprocker.Core
{

    /// <summary>
    /// </summary>
    /// <remarks>SprockerCommand name since 'Sprocker' by itself conflicts with namespace</remarks>
    public class SprockerCommand
    {
        #region Fields

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly SprockerParameterMapper _parameterMapper;
        #endregion
        
        #region Properties

        internal DbCommand DbCommand {get; private set;}

        public Database Database { get; set; }

        public string CommandText { get; set; }

        public DbParameterCollection Parameters { get { return DbCommand.Parameters; } }

        #endregion

        #region Constructor

        public SprockerCommand(Database database, string procedureName)
        {
            Database = database;
            CommandText = procedureName;
            DbCommand = Database.GetStoredProcCommand(CommandText);
            _parameterMapper = new SprockerParameterMapper(Database);
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
    }

    #region ResultSet Mapper
    /// <summary>
    /// This private class is replicated without alteration from the base <see cref="CommandAccessor{TResult}"/> class
    /// as it is called here by some overridden mehtods.
    /// </summary>
    class SprockerResultSetMapper<TResult> : IResultSetMapper<TResult>
    {
        readonly IRowMapper<TResult> _rowMapper;

        public SprockerResultSetMapper(IRowMapper<TResult> rowMapper)
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
    internal class SprockerParameterMapper : IParameterMapper
    {
        readonly Database _database;
        public SprockerParameterMapper(Database database)
        {
            _database = database;
        }

        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
            if (parameterValues.Length > 0)
            {
                GuardParameterDiscoverySupported();
                _database.AssignParameters(command, parameterValues);
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
