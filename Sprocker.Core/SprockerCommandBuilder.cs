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
  

    public class SprockerCommandBuilder<TEntity> where TEntity : class
    {
        private readonly string _procedureName;
        private readonly Database _database;
        
        public ISprockerCommandBuilderContext<TEntity> MapAllParameters()
        {
            ISprockerCommandBuilderContext<TEntity> context = new SprockerCommandBuilderContext(_database, _procedureName);
            return context;
        }

        public SprockerCommandBuilder()
        {
            
        }

        public SprockerCommandBuilder(Database database, string procedureName)
        {
            _procedureName = procedureName;
            _database = database;
        }

        private class SprockerCommandBuilderContext : ISprockerCommandBuilderContext<TEntity>
        {
            private readonly Dictionary<string, Func<TEntity, object>> _parameterMaps;
            private readonly string _procedureName;
            private readonly Database _database;

            public SprockerCommandBuilderContext(Database database, string procedureName)
            {
                _procedureName = procedureName;
                _database = database;
                _parameterMaps = new Dictionary<string, Func<TEntity, object>>();
            }

            public ISprockerCommandBuilderContextMap<TEntity> Map(string parameterName)
            {
                return new SprockerCommandBuilderContextParameterMap(parameterName, this);
            }

            public SprockerCommand Build(TEntity entity)
            {
                SprockerCommand command = new SprockerCommand(_database, _procedureName);
                ParameterMapper<TEntity> mapper = new ParameterMapper<TEntity>(_database, _parameterMaps);
                mapper.AssignParameters(command, entity);
                return command;
            }

            private class SprockerCommandBuilderContextParameterMap : ISprockerCommandBuilderContextMap<TEntity>
            {
                private readonly string _parameterName;
                private readonly SprockerCommandBuilderContext _builderContext;

                public SprockerCommandBuilderContextParameterMap(string name, SprockerCommandBuilderContext builderContext)
                {
                    _parameterName = name;
                    _builderContext = builderContext;
                }

                public ISprockerCommandBuilderContext<TEntity> WithFunc(Func<TEntity, object> mappingFunc)
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
    public interface ISprockerCommandBuilderContext<TEntity> : IFluentInterface where TEntity : class
    {
        /// <summary>
        /// Specifies that a parameter with the specified <paramref name="parameterName"/> should be mapped.
        /// Must be followed by <see cref="ISprockerCommandBuilderContextMap{TEntity}.WithFunc"/>.
        /// </summary>
        ISprockerCommandBuilderContextMap<TEntity> Map(string parameterName);

        SprockerCommand Build(TEntity entity);
    }

    /// <summary>
    /// Fluent interface for defining mapping functions 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISprockerCommandBuilderContextMap<TEntity> : IFluentInterface
        where TEntity : class
    {
        /// <summary>
        /// Defines the mapping function for the parameter specified in the preceding <see cref="ISprockerCommandBuilderContext{TEntity}.Map"/>.
        /// </summary>
        ISprockerCommandBuilderContext<TEntity> WithFunc(Func<TEntity, object> mappingFunc);
    }
}
