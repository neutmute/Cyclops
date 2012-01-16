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

        /// <summary>
        /// <see cref="Regex"/> for stripping the parameter token (@) from the beginning of a stored procedure name.
        /// </summary>
        private static readonly Regex SqlParameterToken = new Regex("^" + SqlRepository.ParameterToken);

        private SaveEntityParameterMapper(Database database, Dictionary<string, Func<TEntity, object>> parameterMaps)
        {
            _database = database;
            _parameterMaps = parameterMaps;
        }

        /// <summary>
        /// Assigns <paramref name="parameterValues"/> to the parametes of <paramref name="command"/>.
        /// </summary>
        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
            if (parameterValues.Length != 1)
            {
                throw SprockerException.Create("The SaveEntityParameterMapper can only be used with a single domain entity parameter.");
            }

            TEntity entity = parameterValues[0] as TEntity;
            Type entityType = typeof (TEntity);

            if (entity == null)
            {
                throw SprockerException.Create("The supplied domain entity parameter was not a valid instance of {0}.", entityType.FullName);
            }

            // TODO: [PF] Cache the results of the DiscoverParameters call.
            _database.DiscoverParameters(command);

            for (int i = SqlRepository.UserParametersStartIndex; i < command.Parameters.Count; i++)
            {
                DbParameter dbParameter = command.Parameters[i];

                string parameterName = dbParameter.ParameterName;
                string propertyName = SqlParameterToken.Replace(parameterName, String.Empty);

                // Check for a mapping function keyed with the parameter name (including the @)
                if (_parameterMaps.ContainsKey(parameterName))
                {
                    object funcResult = _parameterMaps[parameterName](entity);
                    dbParameter.Value = (funcResult == null) ? DBNull.Value : funcResult;
                }
                    // Check for a mapping function keyed with the guessed property name (not including the @)
                else if (_parameterMaps.ContainsKey(propertyName))
                {
                    object funcResult = _parameterMaps[propertyName](entity);
                    dbParameter.Value = (funcResult == null) ? DBNull.Value : funcResult;
                }
                    // Automatically map @ModifiedBy to the current Windows identity (if a mapping function was not already specified)
                else if (dbParameter.ParameterName == "@ModifiedBy")
                {

                    dbParameter.Value = "HARDCODED USER NAME";
                }


                // Try to find a publi instance property with the guessed property name
                PropertyInfo propInfo = entityType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
                if (propInfo == null)
                {
                    // If no matching property was found and the guessed property name is the entity type name with "Id" on the end,
                    // look for a public instance property named "Id".
                    if (propertyName == entityType.Name + "Id")
                    {
                        propInfo = entityType.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
                    }
                    if (propInfo == null)
                    {
                        throw SprockerException.Create(
                            "SaveEntityParameterMapper cannot map parameters: {0} does not contain a public property called '{1}'."
                            , entityType
                            , propertyName);
                    }
                }

                // Map the property value to the parameter
                object propertyValue = propInfo.GetValue(entity, null);
                dbParameter.Value = (propertyValue == null) ? DBNull.Value : propertyValue;
            }
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
