using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace Sprocker.Core
{
    public class ParameterMapper<TEntity> where TEntity : class
    {
        private readonly Database _database;
        private readonly Dictionary<string, Func<TEntity, object>> _parameterMaps;
        private static readonly ParameterCache parameterCache = new ParameterCache();

        /// <summary>
        /// <see cref="Regex"/> for stripping the parameter token (@) from the beginning of a stored procedure name.
        /// </summary>
        private static readonly Regex SqlParameterToken = new Regex("^" + SqlRepository.ParameterToken);


        public ParameterMapper(Database database, Dictionary<string, Func<TEntity, object>> parameterMaps)
        {
            _parameterMaps = parameterMaps;
            _database = database;
        }

        /// <summary>
        /// Assigns <paramref name="parameterValues"/> to the parametes of <paramref name="command"/>.
        /// </summary>
        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
            if (parameterValues.Length != 1)
            {
                throw SprockerException.Create("The ParameterMapper can only be used with a single domain entity parameter.");
            }

            TEntity entity = parameterValues[0] as TEntity;
            Type entityType = typeof(TEntity);

            if (entity == null)
            {
                throw SprockerException.Create("The supplied domain entity parameter was not a valid instance of {0}.", entityType.FullName);
            }

            AssignParameters(command, entity);
        }

        public void AssignParameters(DbCommand command, TEntity entity)
        {
            // Normally the database.AssignParameters would handle this but that call is being bypassed
            parameterCache.SetParameters(command, _database);

            Type entityType = entity.GetType();

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
    }
}
