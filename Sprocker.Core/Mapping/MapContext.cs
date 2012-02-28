using System;
using System.Collections.Generic;

namespace Sprocker.Core.Mapping
{
    /// <summary>
    /// Container for all the mappings related to a given 
    /// Proc or collection of procs
    /// </summary>
    /// <typeparam name="TEntity">the output object graph - maps to the procs results </typeparam>
    /// <typeparam name="TCriteria">the input criteria object - maps to the procs parameters</typeparam>
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
            //throw new NotImplementedException();
            return null;
        }
    }
}
