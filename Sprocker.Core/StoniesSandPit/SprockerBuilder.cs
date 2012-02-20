using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sprocker.Core.FluentInterface.Behaviours;

namespace Sprocker.Core.FluentInterface
{
    /// <summary>
    /// might need a new builder per typer to avoid this T variance problem
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SprockerBuilder : INodeBuilder, IChildNodeBuilder
    {
        private Sprocker _sprocker { get; set; }

        public INodeBuilder Configure()
        {
            _sprocker = new Sprocker();
            return this;
        }

        public IChildNodeBuilder ConfigureChildNode()
        {
            return this;
        }

        // could use an unbound generic perhaps?
        //Type unboundSprocker = typeof(Sprocker<>);

        // IsTransactional

        // StoredProcedure

        // InputMapper

        // OutputMapper

        // AutoMap
        public INodeBuilder InputMapper(IParameterMapper parameterMapper)
        {
            throw new NotImplementedException();
        }

        public IChildNodeBuilder StoredProcedure(string procName)
        {
            throw new NotImplementedException();
        }

        public IChildNodeBuilder IsTransactional(bool isTransactional)
        {
            throw new NotImplementedException();
        }

        public INodeBuilder MapChildNode()
        {
            throw new NotImplementedException();
        }
    }
}
