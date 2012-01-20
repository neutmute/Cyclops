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
    /// 
    /// ok the idea here is to wrap the sproc accessor provided bu DAABs database objects.
    /// 
    /// We need to 
    /// 
    /// support collections of IParameterMappers both
    ///     *  SaveEntityParameterMapper
    ///     *  
    ///
    /// those need to be passed to the DataAccessor class (SprocAccessor)
    /// 
    /// </summary>
    /// <remarks>SprockerCommand name since 'Sprocker' by itself conflicts with namespace</remarks>
    public class SprockerCommand
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private SprockerParameterMapper _parameterMapper;
        
        internal DbCommand DbCommand {get; private set;}

        public Database Database { get; set; }

        public string CommandText { get; set; }

        public DbParameterCollection Parameters { get { return DbCommand.Parameters; } }
        
        public SprockerCommand(Database database, string procedureName)
        {

            Database = database;
            CommandText = procedureName;
            DbCommand = Database.GetStoredProcCommand(CommandText);
            _parameterMapper = new SprockerParameterMapper(Database);
        }

        public int ExecuteNonQuery(params object[] parameters)
        {
            _parameterMapper.AssignParameters(DbCommand, parameters);
            return ExecuteNonQuery();
        }

        public int ExecuteNonQuery()
        {
            int result = 0;
            try
            {
                result = Database.ExecuteNonQuery(DbCommand);
            }
            catch (Exception)
            {
                Log.Info("Failed CommandText: {0}", new DbCommandDumper(DbCommand).GetLogDump());
                throw;
            }
            return result;
        }

        public List<TEntity> ExecuteRowMap<TEntity>()  where TEntity : new()
        {
            IRowMapper<TEntity> defaultRowMapper = MapBuilder<TEntity>.BuildAllProperties();
            return ExecuteRowMap(defaultRowMapper);
        }

        public List<TEntity> ExecuteRowMap<TEntity>(IRowMapper<TEntity> mapper)
        {
            DataTable dataTable = ExecuteDataTable();
            SprockerResultSetMapper<TEntity> setMapper = new SprockerResultSetMapper<TEntity>(mapper);
            
            List<TEntity> entities = new List<TEntity>();
            using (var reader = dataTable.CreateDataReader())
            {
                entities.AddRange(setMapper.MapSet(reader).ToList());
            }

            return entities;
        }

        private DataSet ExecuteDataSet()
        {
            DataSet dataSet;

            try
            {
                dataSet = Database.ExecuteDataSet(DbCommand);
            }
            catch (Exception)
            {
                Log.Info("Failed CommandText: {0}", new DbCommandDumper(DbCommand).GetLogDump());
                throw; 
            }

            return dataSet;
        }

        private DataTable ExecuteDataTable()
        {
            DataSet dataSet = ExecuteDataSet();
            return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;
        }

        /// <summary>
        /// Extract an output param
        /// </summary>
        public T GetParameterValue<T>(string parameterName) where T : struct
        {
            SqlParameter sqlParameter = ((SqlParameter) Parameters[parameterName]);
            object value = sqlParameter.Value;
            return (value == null || value == DBNull.Value) ? default(T) : (T)value;
        }

    }

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
}
