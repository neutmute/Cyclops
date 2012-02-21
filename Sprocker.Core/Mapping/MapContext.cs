using System;

namespace Sprocker.Core.Mapping
{
    public class MapContext<TEntity> : IMapContext<TEntity>
    {
        public IMapContext<TEntity> ToColumn(string columnName)
        {
            throw new NotImplementedException();
        }
    }
}
