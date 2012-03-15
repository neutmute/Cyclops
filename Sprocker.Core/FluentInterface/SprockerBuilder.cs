using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using TheSprocker.Core.FluentInterface.Behaviours;

namespace TheSprocker.Core.Mapping
{
    /// <summary>
    /// might need a new builder per type to avoid this T variance problem
    /// </summary>
    public class SprockerMapBuilder<TRestultEntity> : IRootMapBuilder
    {
        // open generic?
        public SprocMap Sprocmap { get; private set; }

        public IRootMapBuilder Proc(string procedureName)
        {
            throw new NotImplementedException();
        }

        public IRootMapBuilder AutoMapAll()
        {
            throw new NotImplementedException();
        }

        public void Build()
        {
            throw new NotImplementedException();
        }
    }
}
