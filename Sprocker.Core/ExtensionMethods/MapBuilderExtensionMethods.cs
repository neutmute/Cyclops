using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace TheSprocker.Core
{
    /// <summary>
    /// Extension methods for EntLib <see cref="MapBuilder{TResult}"/> class and its associated
    /// <see cref="IMapBuilderContext{TResult}"/> and <see cref="IMapBuilderContextMap{TResult, Object}"/>
    /// classes.
    /// </summary>
    public static class MapBuilderExtensionMethods
    {
        /// <summary>
        /// Map enums by convention where 'EnumType' has column 'EnumTypeId'
        /// </summary>
        public static IMapBuilderContext<TResult> MapAllEnums<TResult>(this IMapBuilderContext<TResult> context) where TResult : class
        {
            var properties =
                from property in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                where IsEnumMappableProperty(property)
                select property;

            foreach (var property in properties)
            {
                context = context.Map(property).ToColumnAsEnum(property.PropertyType.Name + "Id");
            }

            return context;
        }

        private static bool IsEnumMappableProperty(PropertyInfo property)
        {
            return property.CanWrite
              && typeof(Enum).IsAssignableFrom(property.PropertyType);
        }

        /// <summary>
        /// Maps the current property to a column with the given name as an enum value. 
        /// </summary>
        /// <typeparam name="TEnum">The enum type that the current property should be mapped to.</typeparam>
        /// <typeparam name="TResult">The type that is being mapped form the data row.</typeparam>
        /// <param name="contextMap">The current <see cref="IMapBuilderContextMap{TResult, TEnum}"/>.</param>
        /// <param name="columnName">The name of the column the current property should be mapped to.</param>
        /// <returns>The fluent interface that can be used further specify mappings.</returns>
        public static IMapBuilderContext<TResult> ToColumnAsEnum<TResult, TEnum>(this IMapBuilderContextMap<TResult, TEnum> contextMap, string columnName)
        {
            return contextMap.WithFunc(row =>
                                           {
                                               int fieldOrdinal = row.GetOrdinal(columnName);
                                               Type fieldType = row.GetFieldType(fieldOrdinal);
                                               switch(fieldType.FullName)
                                               {
                                                   // Attempting to cast anything other than object to TEnum causes a compile exception but 
                                                   // (TEnum) row.GetValue will cause a runtime exception if the type of the field is anything
                                                   // other than Int32. The only way to avoid compile time and runtime exceptions for fields
                                                   // that are not Int32 is the 'convert to Int32 - cast to object - cast to TEnum' method below.
                                                   case "System.Byte":
                                                   case "System.Int16":
                                                   case "System.Int64":
                                                       return (TEnum) (object) Convert.ToInt32(row.GetValue(fieldOrdinal));
                                                   case "System.Int32":
                                                       return (TEnum) row.GetValue(fieldOrdinal);
                                                   default:
                                                       throw SprockerException.Create("Mapping from the value not currently supported");
                                                       //return row[fieldOrdinal].ToString().ToEnumFromCode<TEnum>();
                                               }
                                           });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultSetField"></param>
        /// <returns></returns>
        public static T? ToOptional<T>(this object resultSetField) where T : struct
        {
            return (resultSetField == null || resultSetField == DBNull.Value) ? default(T) : (T)resultSetField;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultSetField"></param>
        /// <returns></returns>
        public static string ToOptional(this object resultSetField)
        {
            return (resultSetField == null || resultSetField == DBNull.Value) ? null : (string)resultSetField;
        }
    }
}
