using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sprocker.Core.FluentInterface.Behaviours;

namespace Sprocker.Core.Mapping
{
    /// <summary>
    /// might need a new builder per typer to avoid this T variance problem
    /// </summary>
    public class SprockerBuilder : INodeBuilder, IChildNodeBuilder
    {
        private Sprocker _sprocker { get; set; }

        public INodeBuilder MapGraph()
        {
            _sprocker = new Sprocker();
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
