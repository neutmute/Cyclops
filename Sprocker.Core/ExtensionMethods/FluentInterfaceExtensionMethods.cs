using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheSprocker.Core.FluentInterface.Behaviours;

namespace TheSprocker.Core.ExtensionMethods
{
    public static class FluentInterfaceExtensionMethods
    {
        public static IRootMapBuilder ParameterType<TParameterType>(this IRootMapBuilder sprockerBuilder)
        {
            return sprockerBuilder;
        }

        public static IRootMapBuilder ResultType<TResultType>(this IRootMapBuilder sprockerBuilder)
        {
            return sprockerBuilder;
        }
    }
}
