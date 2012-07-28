using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Cyclops
{
    /// <summary>
    /// Inherit from this and inject to your your own SqlRepository
    /// </summary>

    [Obsolete("Use Extension Methods")]
    public class MapHelper
    {
        [Obsolete("Use ToColumnAsEnum")]
        public T ToEnum<T>(IDataRecord row, string columnName)
        {
            var valueObject = row[columnName];
            int valueByte;
            try
            {
                valueByte =Convert.ToInt32(valueObject);
            }
            catch(Exception e)
            {
                string typeName = valueObject == null ? "<null>" : valueObject.GetType().Name;
                throw CyclopsException.Create(e, "Failed to convert '{0}' of type {2} from column '{1}' to an int32 for Enum mapping", valueObject, columnName, typeName);
            }

            return (T)Enum.ToObject(typeof(T), valueByte);
        }

        [Obsolete("Use ToColumn")]
        public virtual int ToInt(IDataRecord dr, string columnName)
        {
            object value = dr[columnName];
            if (value is DBNull)
            {
                throw CyclopsException.Create("{0} was unexpectedly NULL", columnName);
            }
            return Convert.ToInt32(value);
        }

        [Obsolete("Use ToColumn")]
        public virtual int? ToNullableInt(IDataRecord dr, string columnName)
        {
            return ToIntNullable(dr, columnName);
        }

        [Obsolete("Use ToColumn")]
        public virtual int? ToIntNullable(IDataRecord dr, string columnName)
        {
            if (dr[columnName] == DBNull.Value)
            {
                return null;
            }
            return Convert.ToInt32(dr[columnName]);
        }

        [Obsolete("Use ToColumn")]
        public string ToStringNullable(IDataRecord dr, string columnName)
        {
            if (dr[columnName] == DBNull.Value)
            {
                return null;
            }
            return dr[columnName].ToString();
        }
    }
}
