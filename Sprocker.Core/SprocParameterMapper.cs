using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core
{
    /// <summary>
    /// TODO: [AS] try and build a decoupled parameter mapper... might be having a lend of myself. 
    /// </summary>
    public class SprocParameterMapBuilder<TMapTo> : IParameterMapper
    {
        // one per instance? or a static collection?
        private readonly ParameterCache parameterCache = new ParameterCache();

        public SprocParameterMapBuilder()
        {
            
        }

        public void AssignParameters(DbCommand command, object[] parameterValues)
        {
          //  throw new NotImplementedException();
        }
    }
}
