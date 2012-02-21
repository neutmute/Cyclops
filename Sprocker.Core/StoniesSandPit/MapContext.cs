using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sprocker.Core.StoniesSandPit
{
    public class MapContext<TEntity> : IMapContext<TEntity>
    {
        public IMapContext<TEntity> ToColumn(string columnName)
        {
            throw new NotImplementedException();
        }
    }
}
