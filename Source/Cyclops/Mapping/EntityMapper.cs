﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Cyclops
{
    public class EntityMapper
    {
        /// <summary>
        /// Useful for when a dataTable has been filtered with a LINQ query. ie: Whale.MetricMapRule filter
        /// </summary>
        public static List<TEntity> Map<TEntity>(IEnumerable<DataRow> dataRows, IRowMapper<TEntity> mapper)
        {
            var dataRowList = dataRows.ToList();
            if (dataRowList.Count == 0)
            {
                return new List<TEntity>();
            }
            var dataTable = dataRowList.CopyToDataTable();
            return Map(dataTable, mapper);
        }

        /// <summary>
        /// Useful consistent syntax/shortcut for mapping an sub object in the graph - eg: AlarmTrigger options
        /// </summary>
        public static TEntity Map<TEntity>(IDataRecord dataRecord, IRowMapper<TEntity> mapper)
        {
            return mapper.MapRow(dataRecord);
        }

        public static List<TEntity> Map<TEntity>(DataTable dataTable, IRowMapper<TEntity> mapper)
        {
            CyclopsResultSetMapper<TEntity> setMapper = new CyclopsResultSetMapper<TEntity>(mapper);
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
