using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sprocker.Core.FluentInterface.Behaviours;

namespace Sprocker.Core.Mapping
{
    /// <summary>
    /// might need a new builder per typer to avoid this T variance problem
    /// </summary>
    public class SprockerBuilder<TEntity,TMap> : INodeBuilder, IChildNodeBuilder
    {
        private SprockerExecute<TMap> _sprockerMapExecutor { get; set; }

        public INodeBuilder MapGraph()
        {
            _sprockerMapExecutor = new SprockerExecute<TMap>();
            return this;
        }

        public INodeBuilder InputMapper(IParameterMapper parameterMapper)
        {
            throw new NotImplementedException();
        }

        public IChildNodeBuilder Proc(string procName)
        {
            throw new NotImplementedException();
        }

        public IChildNodeBuilder IsTransactional(bool isTransactional)
        {
            throw new NotImplementedException();
        }
    }
}
