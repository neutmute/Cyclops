using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheSprocker.Core.FluentInterface.Behaviours;

namespace TheSprocker.Core.ExtensionMethods
{
    public static class FluentInterfaceExtensionMethods
    {
        public static IBaseBuilder Criteria<TCriteria>(this IBaseBuilder sprockerBuilder)
        {
            return sprockerBuilder;
        }
    }
}
