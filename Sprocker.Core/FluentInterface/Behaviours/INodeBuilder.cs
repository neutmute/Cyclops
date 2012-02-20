using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core.FluentInterface.Behaviours
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">this is required for the row mapper</typeparam>
    public interface INodeBuilder
    {
        INodeBuilder InputMapper(IParameterMapper parameterMapper);
        IChildNodeBuilder StoredProcedure(String procName);
    }
}
