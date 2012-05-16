using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TheSprocker.Core
{
    /// <summary>
    /// Inherit from this and inject to your your own SqlRepository
    /// </summary>
    public class MapHelper
    {
        public T ToEnum<T>(IDataRecord row, string columnName)
        {
            return (T)Enum.ToObject(typeof(T), (byte)row[columnName]);
        }

        public virtual int ToInt(IDataRecord dr, string columnName)
        {
            object value = dr[columnName];
            if (value is DBNull)
            {
                throw SprockerException.Create("{0} was unexpectedly NULL", columnName);
            }
            return Convert.ToInt32(value);
        }

        public virtual int? ToNullableInt(IDataRecord dr, string columnName)
        {
            if (dr[columnName] == DBNull.Value)
            {
                return null;
            }
            return Convert.ToInt32(dr[columnName]);
        }
    }
}
