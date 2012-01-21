using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core
{
    public class EntityMapper
    {
        public static List<TEntity> Map<TEntity>(DataTable dataTable, IRowMapper<TEntity> mapper)
        {
            SprockerResultSetMapper<TEntity> setMapper = new SprockerResultSetMapper<TEntity>(mapper);
            List<TEntity> entities = new List<TEntity>();
            using (var reader = dataTable.CreateDataReader())
            {
                entities.AddRange(setMapper.MapSet(reader).ToList());
            }
            return entities;
        }

        public static List<TEntity> Map<TEntity>(DataTable dataTable) where TEntity :new()
        {
            return Map(dataTable, GetDefaultMapper<TEntity>());
        }

        public static IRowMapper<TEntity> GetDefaultMapper<TEntity>() where TEntity : new()
        {
            return MapBuilder<TEntity>.BuildAllProperties();
        }
    }
}
