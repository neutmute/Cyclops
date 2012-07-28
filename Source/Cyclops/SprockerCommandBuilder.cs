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

namespace TheSprocker.Core
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

            /// <summary>
            /// Automatically map enums assuming convention EnumType maps to @EnumTypeId
            /// </summary>
            public ISprockerCommandBuilderContext<TEntity> MapAllEnums()
            {
                var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
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

            public SprockerCommand Build()
            {
                return Build(null);
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

                /// <summary>
                /// Syntactic sugar to easily map in a null
                /// </summary>
                public ISprockerCommandBuilderContext<TEntity> WithNull()
                {
                    _builderContext._parameterMaps[_parameterName] = e => DBNull.Value;
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

        /// <summary>
        /// Automatically map enums assuming convention EnumType maps to @EnumTypeId
        /// </summary>
        ISprockerCommandBuilderContext<TEntity> MapAllEnums();

        SprockerCommand Build(TEntity entity);

        SprockerCommand Build();
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

        /// <summary>
        /// Syntactic sugar to easily map in a null
        /// </summary>
        ISprockerCommandBuilderContext<TEntity> WithNull();
    }
}
