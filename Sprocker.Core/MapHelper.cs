using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Sprocker.Core
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
    }
}
