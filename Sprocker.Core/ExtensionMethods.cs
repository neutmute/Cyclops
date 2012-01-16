using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sprocker.Core
{

    /// <summary>
    /// Extension methods relating to the <see cref="System.Enum"/> class.
    /// </summary>
    /// <remarks>
    /// Reflection is used in these extension methods. be aware of performance considerations. 
    /// </remarks>
    public static class EnumExtensionMethods
    {
        /// <summary>
        /// Returns an instance of <see cref="System.Enum"/> type <typeparam name="T" /> which represents
        /// the first member decorated with an <see cref="EnumCodeAttribute"/> having the specified 
        /// <paramref name="code"/>. If no enum member has the specified code, the default value of the enum
        /// is returned.
        /// </summary>
        public static T ToEnumFromCode<T>(this string code)
        {
            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo t in fields)
            {
                EnumCodeAttribute[] attributes = (EnumCodeAttribute[])t.GetCustomAttributes(typeof(EnumCodeAttribute), false);
                if ((attributes.Length > 0) && (code == attributes[0].Code))
                {
                    return (T)Enum.Parse(typeof(T), t.Name);
                }
            }
            return (T)Enum.ToObject(typeof(T), 0);
        }

        /// <summary>
        /// If the specified <paramref name="enumValue"/> is decorated with an <see cref="EnumCodeAttribute"/>, this
        /// method returns the value <see cref="EnumCodeAttribute.Code"/> property of that attribute, otherwise it 
        /// returns the numeric representation of the enum value.
        /// </summary>
        public static string GetCode(this Enum enumValue)
        {
            EnumCodeAttribute attribute = GetAttribute<EnumCodeAttribute>(enumValue);
            if (attribute != null)
            {
                return attribute.Code;
            }
            return Convert.ToInt32(enumValue).ToString();
        }

        /// <summary>
        /// If the specified <paramref name="enumValue"/> is decorated with an <see cref="DescriptionAttribute"/>, this
        /// method returns the value <see cref="DescriptionAttribute.Description"/> property of that attribute, otherwise it 
        /// returns the result of the enum value's <see cref="Enum.ToString()"/> method.
        /// </summary>
        public static string GetDescription(this Enum enumValue)
        {
            DescriptionAttribute attribute = GetAttribute<DescriptionAttribute>(enumValue);
            if (attribute != null)
            {
                return attribute.Description;
            }
            return enumValue.ToString();
        }

        /// <summary>
        /// Gets the first <see cref="System.Attribute"/> of type T that the 
        /// <paramref name="enumValue"/> is decorated with, or the default of T
        /// if the enum value is not decorated with any attributes of that type.
        /// </summary>
        /// <typeparam name="T">A <see cref="Type"/> that inherits from <see cref="Attribute"/>.</typeparam>
        public static T GetAttribute<T>(this Enum enumValue)
        {
            object[] attributes = enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(T), false);
            if (attributes.Length > 0)
            {
                return (T)attributes[0];
            }
            return default(T);
        }
    }

    /// <summary>
    /// When applied to a <see cref="System.Enum"/> member, <see cref="EnumCodeAttribute"/> allows 
    /// the member to define a non-numeric code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumCodeAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="EnumCodeAttribute"/> with the specified <see cref="Code"/>.
        /// </summary>
        /// <param name="code"></param>
        public EnumCodeAttribute(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Gets the code that represents the <see cref="Enum"/> member.
        /// </summary>
        public string Code { get; private set; }
    }

    /// <summary>
    /// Convenient retrieval of an output parameter
    /// </summary>
    public static class DbCommandExtension
    {
        public static T GetParameterValue<T>(this IDbCommand command, string parameterName) where T : struct
        {
            SqlParameter sqlParameter = ((SqlParameter)command.Parameters[parameterName]);
            object value = sqlParameter.Value;
            return (value == null || value == DBNull.Value) ? default(T) : (T)value;
        }
    }
}
