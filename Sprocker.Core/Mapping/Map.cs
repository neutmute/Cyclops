using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheSprocker.Core.Mapping;

namespace TheSprocker.Core.Mapping
{
    public abstract class Map<TRootType>
    {
        public SprockerMapBuilder<TRootType> SprockerBuilder { get; set; }

        public SprockerMapBuilder<TRootType> DefineMappings()
        {
            SprockerBuilder = new SprockerMapBuilder<TRootType>();
            return SprockerBuilder;
        }
    }
}
