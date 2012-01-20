using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sprocker.Core;

namespace Sprocker.Core
{
    /// <summary>
    /// <see cref="SaveEntityParameterMapper{TEntity}"/> will automatically map public instance properties 
    /// of a domain entity to parameters on a stored procedure.
    /// </summary>
    public class SaveEntityParameterMapper<TEntity> : IParameterMapper
        where TEntity : class
    {
        private readonly Dictionary<string, Func<TEntity, object>> _parameterMaps;
        private readonly Database _database;
        
        private SaveEntityParameterMapper(Database database, Dictionary<string, Func<TEntity, object>> parameterMaps)
        {
            _database = database;
            _parameterMaps = parameterMaps;
        }

        /// <summary>
        /// Assigns <paramref name="parameterValues"/> to the parametes of <paramref name="command"/>.
        /// </summary>
        public void AssignParameters(SprockerCommand command, object[] parameterValues)
        {
            ParameterMapper<TEntity> parameterMapper = new ParameterMapper<TEntity>(_database, _parameterMaps);
            parameterMapper.AssignParameters(command, parameterValues);
        }
   
        /// <summary>
        /// Initiate building a <see cref="SaveEntityParameterMapper{TEntity}"/>.
        /// </summary>
        public static IParameterMapBuilderContext<TEntity> MapAllParameters()
        {
            IParameterMapBuilderContext<TEntity> context = new ParameterMapBuilderContext();
            return context;
        }

        /// <summary>
        /// An implementation of <see cref="IParameterMapBuilderContext{TEntity}"/> that supports defining mapping 
        /// functions for a <see cref="SaveEntityParameterMapper{TEntity}"/>.
        /// </summary>
        private class ParameterMapBuilderContext : IParameterMapBuilderContext<TEntity>
        {
            private readonly Dictionary<string, Func<TEntity, object>> _parameterMaps;

            public ParameterMapBuilderContext()
            {
                _parameterMaps = new Dictionary<string, Func<TEntity, object>>();
            }

            public IParameterMapBuilderContextMap<TEntity> Map(string parameterName)
            {
                return new ParameterMapBuilderContextMap(parameterName, this);
            }

            public SaveEntityParameterMapper<TEntity> Build(Database database)
            {
                return new SaveEntityParameterMapper<TEntity>(database, _parameterMaps);
            }

            private class ParameterMapBuilderContextMap : IParameterMapBuilderContextMap<TEntity>
            {
                private readonly string _parameterName;
                private readonly ParameterMapBuilderContext _builderContext;

                public ParameterMapBuilderContextMap(string name, ParameterMapBuilderContext builderContext)
                {
                    _parameterName = name;
                    _builderContext = builderContext;
                }

                public IParameterMapBuilderContext<TEntity> WithFunc(Func<TEntity, object> mappingFunc)
                {
                    _builderContext._parameterMaps[_parameterName] = mappingFunc;
                    return _builderContext;
                }
            }
        }

        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Fluent interface for defining parameter mappings for a <see cref="Sprocker.Core.SaveEntityParameterMapper{TEntity}"/>.
    /// </summary>
    public interface IParameterMapBuilderContext<TEntity> : IFluentInterface
        where TEntity : class
    {
        /// <summary>
        /// Specifies that a parameter with the specified <paramref name="parameterName"/> should be mapped.
        /// Must be followed by <see cref="IParameterMapBuilderContextMap{TEntity}.WithFunc"/>.
        /// </summary>
        IParameterMapBuilderContextMap<TEntity> Map(string parameterName);

        /// <summary>
        /// Builds a <see cref="Sprocker.Core.SaveEntityParameterMapper{TEntity}"/> with the mappings defined in the fluent interface.
        /// </summary>
        SaveEntityParameterMapper<TEntity> Build(Database database);
    }

    /// <summary>
    /// Fluent interface for defining mapping functions for a <see cref="Sprocker.Core.SaveEntityParameterMapper{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IParameterMapBuilderContextMap<TEntity> : IFluentInterface
        where TEntity : class
    {
        /// <summary>
        /// Defines the mapping function for the parameter specified in the preceding <see cref="IParameterMapBuilderContext{TEntity}.Map"/>.
        /// </summary>
        IParameterMapBuilderContext<TEntity> WithFunc(Func<TEntity, object> mappingFunc);
    }
}
