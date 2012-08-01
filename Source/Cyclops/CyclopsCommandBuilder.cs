using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Cyclops
{
    #region CyclopsCommandBuilder

    public class CyclopsCommandBuilder<TEntity> where TEntity : class
    {
        private readonly string _procedureName;
        private readonly Database _database;
        
        public ICyclopsCommandBuilderContext<TEntity> MapAllParameters()
        {
            ICyclopsCommandBuilderContext<TEntity> context = new CyclopsCommandBuilderContext(_database, _procedureName);
            return context;
        }

        public CyclopsCommandBuilder()
        {
            
        }

        public CyclopsCommandBuilder(Database database, string procedureName)
        {
            _procedureName = procedureName;
            _database = database;
        }

        private class CyclopsCommandBuilderContext : ICyclopsCommandBuilderContext<TEntity>
        {
            private readonly Dictionary<string, Func<TEntity, object>> _parameterMaps;
            private readonly string _procedureName;
            private readonly Database _database;

            public CyclopsCommandBuilderContext(Database database, string procedureName)
            {
                _procedureName = procedureName;
                _database = database;
                _parameterMaps = new Dictionary<string, Func<TEntity, object>>();
            }

            /// <summary>
            /// Automatically map enums assuming convention EnumType maps to @EnumTypeId
            /// </summary>
            public ICyclopsCommandBuilderContext<TEntity> MapAllEnums()
            {
                const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
                var properties =    (from property in typeof(TEntity).GetProperties(bindingFlags)
                                     where IsEnumMappableProperty(property)
                                     select property).ToList();

                for (int propertyIndex = 0; propertyIndex < properties.Count; propertyIndex++)
                {
                    var property = properties[propertyIndex];
                    var paramName = string.Format("@{0}Id", property.Name);

                    Map(paramName).WithFunc(e =>
                                                {
                                                    var enumValue = property.GetValue(e, bindingFlags, null, null, null);
                                                    return Convert.ToInt32(enumValue);
                                                });
                }
                return this;
            }

            private static bool IsEnumMappableProperty(PropertyInfo property)
            {
                return typeof(Enum).IsAssignableFrom(property.PropertyType);
            }

            public ICyclopsCommandBuilderContextMap<TEntity> Map(string parameterName)
            {
                return new CyclopsCommandBuilderContextParameterMap(parameterName, this);
            }

            public CyclopsCommand Build(TEntity entity)
            {
                CyclopsCommand command = new CyclopsCommand(_database, _procedureName);
                ParameterMapper<TEntity> mapper = new ParameterMapper<TEntity>(_database, _parameterMaps);
                mapper.AssignParameters(command, entity);
                return command;
            }

            public CyclopsCommand Build()
            {
                return Build(null);
            }

            private class CyclopsCommandBuilderContextParameterMap : ICyclopsCommandBuilderContextMap<TEntity>
            {
                private readonly string _parameterName;
                private readonly CyclopsCommandBuilderContext _builderContext;

                public CyclopsCommandBuilderContextParameterMap(string name, CyclopsCommandBuilderContext builderContext)
                {
                    _parameterName = name;
                    _builderContext = builderContext;
                }

                public ICyclopsCommandBuilderContext<TEntity> WithFunc(Func<TEntity, object> mappingFunc)
                {
                    _builderContext._parameterMaps[_parameterName] = mappingFunc;
                    return _builderContext;
                }

                /// <summary>
                /// Syntactic sugar to easily map in a null
                /// </summary>
                public ICyclopsCommandBuilderContext<TEntity> WithNull()
                {
                    _builderContext._parameterMaps[_parameterName] = e => DBNull.Value;
                    return _builderContext;
                }

                public ICyclopsCommandBuilderContext<TEntity> WithValue(object value)
                {
                    _builderContext._parameterMaps[_parameterName] = e => value;
                    return _builderContext;
                }
            }
        }
    }
    #endregion

    #region ICyclopsCommandBuilderContext

    /// <summary>
    /// Fluent interface for defining parameter mappings
    /// </summary>
    public interface ICyclopsCommandBuilderContext<TEntity> : IFluentInterface where TEntity : class
    {
        /// <summary>
        /// Specifies that a parameter with the specified <paramref name="parameterName"/> should be mapped.
        /// Must be followed by <see cref="ICyclopsCommandBuilderContextMap{TEntity}.WithFunc"/>.
        /// </summary>
        ICyclopsCommandBuilderContextMap<TEntity> Map(string parameterName);

        /// <summary>
        /// Automatically map enums assuming convention EnumType maps to @EnumTypeId
        /// </summary>
        ICyclopsCommandBuilderContext<TEntity> MapAllEnums();

        CyclopsCommand Build(TEntity entity);

        CyclopsCommand Build();
    }

    #endregion
    
    #region ICyclopsCommandBuilderContextMap
    /// <summary>
    /// Fluent interface for defining mapping functions 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICyclopsCommandBuilderContextMap<TEntity> : IFluentInterface
        where TEntity : class
    {
        /// <summary>
        /// Defines the mapping function for the parameter specified in the preceding <see cref="ICyclopsCommandBuilderContext{TEntity}.Map"/>.
        /// </summary>
        ICyclopsCommandBuilderContext<TEntity> WithFunc(Func<TEntity, object> mappingFunc);

        /// <summary>
        /// Syntactic sugar to easily map in a null
        /// </summary>
        ICyclopsCommandBuilderContext<TEntity> WithNull();

        ICyclopsCommandBuilderContext<TEntity> WithValue(object value);
    }
    #endregion
}
