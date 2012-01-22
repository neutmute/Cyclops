﻿using System;
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
            DbCommandLogger commandLogger = new DbCommandLogger(this);
            try
            {
                result = Database.ExecuteNonQuery(DbCommand);
            }
            catch (Exception)
            {
                Log.Info("Failed CommandText: {0}", new DbCommandDumper(DbCommand).GetLogDump());
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

        public TableSet ExecuteTableSet()
        {
            return TableSet.Create(ExecuteDataSet());
        }

        public DataSet ExecuteDataSet()
        {
            DataSet dataSet;
            DbCommandLogger commandLogger = new DbCommandLogger(this);
            try
            {
                dataSet = Database.ExecuteDataSet(DbCommand);
            }
            catch (Exception)
            {
                Log.Info("Failed CommandText: {0}", new DbCommandDumper(DbCommand).GetLogDump());
                throw;
            }
            finally
            {
                commandLogger.Complete();
            }

            return dataSet;
        }
        
        private DataTable ExecuteDataTable()
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