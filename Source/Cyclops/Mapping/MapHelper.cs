using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Cyclops
{
    /// <summary>
    /// Used for some quick unpacking where a full row mapper is not required or suitable.
    /// Use of this should be the exception rather than the rule
    /// Inherit from this and inject to your your own SqlRepository
    /// </summary>
    public class MapHelper
    {
        /// <summary>
        /// You should prefer ToColumnAsEnum extension over this where possible as it is less verbose
        /// </summary>
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

        public virtual int ToInt(IDataRecord dr, string columnName)
        {
            object value = dr[columnName];
            if (value is DBNull)
            {
                throw CyclopsException.Create("{0} was unexpectedly NULL", columnName);
            }
            return Convert.ToInt32(value);
        }

        public virtual long ToLong(IDataRecord dr, string columnName)
        {
            object value = dr[columnName];
            if (value is DBNull)
            {
                throw CyclopsException.Create("{0} was unexpectedly NULL", columnName);
            }
            return Convert.ToInt64(value);
        }

        public virtual bool ToBoolean(IDataRecord dr, string columnName)
        {
            object value = dr[columnName];
            if (value is DBNull)
            {
                throw CyclopsException.Create("{0} was unexpectedly NULL", columnName);
            }
            return Convert.ToBoolean(value);
        }

        public virtual int? ToIntNullable(IDataRecord dr, string columnName)
        {
            if (dr[columnName] == DBNull.Value)
            {
                return null;
            }
            return Convert.ToInt32(dr[columnName]);
        }

        public virtual int? ToIntNullable(DataRow dr, string columnName)
        {
            if (dr[columnName] == DBNull.Value)
            {
                return null;
            }
            return Convert.ToInt32(dr[columnName]);
        }

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
