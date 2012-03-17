using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using TheSprocker.Core.FluentInterface.Behaviours;

namespace TheSprocker.Core.Mapping
{
    /// <summary>
    /// might need a new builder per type to avoid this T variance problem
    /// </summary>
    public class SprockerMapBuilder : IRootMapBuilder
    {
        // open generic?
        public SprockerMapContext SprocMap { get; set; }

        public IRootMapBuilder Proc(string procedureName)
        {
            SprocMap.ProcName = procedureName;
            return this;
        }

        public IRootMapBuilder AutoMapAll()
        {
            SprocMap.AutomapAll = true;
            return this;
        }

        public SprockerMapContext Build()
        {
            return SprocMap;
        }

        public IRootMapBuilder ParameterType<TParameterType>()
        {
            SprocMap.ParamtererType = typeof(TParameterType);
            return this;
        }

        public IRootMapBuilder ResultType<TResultType>()
        {
            SprocMap.ResultType = typeof(TResultType);
            return this;
        }
    }
}
