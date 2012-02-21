using System;
using System.Collections.Generic;

namespace Sprocker.Core.Mapping
{
    /// <summary>
    /// Container for object to sproc maps
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MapContext<TEntity, TCriteria> : IMapContext<TEntity, TCriteria>
    {
        List<CriteriaMap> criteriaMaps = new List<CriteriaMap>();
        List<ResultMap> resultMaps = new List<ResultMap>();

        public IMapContext<TEntity, TCriteria> ToColumn(string columnName)
        {
            throw new NotImplementedException();
        }
    }
}
