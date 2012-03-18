using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheSprocker.Core.Mapping;

namespace TheSprocker.Core.Mapping
{
    public abstract class SprockerMap
    {
        public SprockerMapBuilder SprockerBuilder { get; set; }

        protected SprockerMapBuilder DefineMappings()
        {
            SprockerBuilder = new SprockerMapBuilder();
            return SprockerBuilder;
        }

        public SprockerMapContext GetMapContext()
        {
            // some guard or something 

            return SprockerBuilder.SprocMap;
        }
    }
}
