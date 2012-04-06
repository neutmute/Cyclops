using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace TheSprocker.Core
{
    public class EntityMapper
    {
        /// <summary>
        /// Useful for when a dataTable has been filtered with a LINQ query. ie: Whale.MetricMapRule filter
        /// </summary>
        public static List<TEntity> Map<TEntity>(IEnumerable<DataRow> dataRows, IRowMapper<TEntity> mapper)
        {
            var dataTable = dataRows.CopyToDataTable();
            return Map(dataTable, mapper);
        }

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
