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
        public SprockerMapContext SprocMap { get; set; }

        public IRootMapBuilder Proc(string procedureName)
        {
            SprocMap = new SprockerMapContext(procedureName);
            return this;
        }

        public IRootMapBuilder AutoMapAll()
        {
            SprocMap.AutomapAll = true;
            return this;
        }

        public SprockerMapContext Build()
        {
            //TODO: [AS] trigger all the work here. for now just run in tests.  
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
