using System;
using System.Collections.Generic;

namespace Sprocker.Core.Mapping
{
    /// <summary>
    /// Container for object to sproc maps
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TCriteria"></typeparam>
    public class MapContext<TEntity, TCriteria> : IMapContext<TEntity, TCriteria>
    {
        public List<CriteriaMap<TCriteria>> CriteriaMaps { get; set; }
        public List<ResultMap<TEntity>> ResultMaps { get; set; }

        public MapContext()
        {
            CriteriaMaps = new List<CriteriaMap<TCriteria>>();
            ResultMaps = new List<ResultMap<TEntity>>();
        }

        public IMapContext<TEntity, TCriteria> ToColumn(string columnName)
        {
            throw new NotImplementedException();
        }
    }
}
