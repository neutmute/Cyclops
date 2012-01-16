using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core
{
  

    public class DbCommandBuilder<TEntity> where TEntity : class
    {
        public static IDbCommandBuilderContext<TEntity> MapAllParameters(Database database, string storedProcedureName)
        {
            IDbCommandBuilderContext<TEntity> context = new DbCommandBuilderContext(database, storedProcedureName);
            return context;
        }

        private class DbCommandBuilderContext : IDbCommandBuilderContext<TEntity>
        {
            private readonly Dictionary<string, Func<TEntity, object>> _parameterMaps;
            private readonly string _storedProcedureName;
            private readonly Database _database;

            public DbCommandBuilderContext(Database database, string storedProcedureName)
            {
                _storedProcedureName = storedProcedureName;
                _database = database;
                _parameterMaps = new Dictionary<string, Func<TEntity, object>>();
            }

            public IDbCommandBuilderContextMap<TEntity> Map(string parameterName)
            {
                return new DbCommandBuilderContextParameterMap(parameterName, this);
            }

            public DbCommand Build(TEntity entity)
            {
                DbCommand command = _database.GetStoredProcCommand(_storedProcedureName);
                ParameterMapper<TEntity> mapper = new ParameterMapper<TEntity>(_database, _parameterMaps);
                mapper.AssignParameters(command, entity);
                return command;
            }

            private class DbCommandBuilderContextParameterMap : IDbCommandBuilderContextMap<TEntity>
            {
                private readonly string _parameterName;
                private readonly DbCommandBuilderContext _builderContext;

                public DbCommandBuilderContextParameterMap(string name, DbCommandBuilderContext builderContext)
                {
                    _parameterName = name;
                    _builderContext = builderContext;
                }

                public IDbCommandBuilderContext<TEntity> WithFunc(Func<TEntity, object> mappingFunc)
                {
                    _builderContext._parameterMaps[_parameterName] = mappingFunc;
                    return _builderContext;
                }
            }
        }
    }

    /// <summary>
    /// Fluent interface for defining parameter mappings
    /// </summary>
    public interface IDbCommandBuilderContext<TEntity> : IFluentInterface where TEntity : class
    {
        /// <summary>
        /// Specifies that a parameter with the specified <paramref name="parameterName"/> should be mapped.
        /// Must be followed by <see cref="IDbCommandBuilderContextMap{TEntity}.WithFunc"/>.
        /// </summary>
        IDbCommandBuilderContextMap<TEntity> Map(string parameterName);

        DbCommand Build(TEntity entity);
    }

    /// <summary>
    /// Fluent interface for defining mapping functions 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDbCommandBuilderContextMap<TEntity> : IFluentInterface
        where TEntity : class
    {
        /// <summary>
        /// Defines the mapping function for the parameter specified in the preceding <see cref="IDbCommandBuilderContext{TEntity}.Map"/>.
        /// </summary>
        IDbCommandBuilderContext<TEntity> WithFunc(Func<TEntity, object> mappingFunc);
    }
}
