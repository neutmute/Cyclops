using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheSprocker.Core.Mapping;

namespace TheSprocker.Core.Mapping
{
    public abstract class Map<TType>
    {
        public SprockerBuilder<TType> SprockerBuilder { get; set; }

        public SprockerBuilder<TType> Define()
        {
            SprockerBuilder = new SprockerBuilder<TType>();
            return SprockerBuilder;
        }


        
    }
}
