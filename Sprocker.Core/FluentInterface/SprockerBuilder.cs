using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using TheSprocker.Core.FluentInterface.Behaviours;

namespace TheSprocker.Core.Mapping
{
    /// <summary>
    /// might need a new builder per typer to avoid this T variance problem
    /// </summary>
    public class SprockerBuilder<TEntity> : IBaseBuilder
    {
        public IBaseBuilder Proc(string procedureName)
        {
            throw new NotImplementedException();
        }

        public IBaseBuilder AutoMapAll()
        {
            throw new NotImplementedException();
        }

        public void Build()
        {
            throw new NotImplementedException();
        }
    }
}
