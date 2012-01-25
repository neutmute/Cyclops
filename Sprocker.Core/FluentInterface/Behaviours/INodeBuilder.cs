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
    public interface INodeBuilder<T>
    {
        INodeBuilder<T> InputMapper(IParameterMapper parameterMapper);
        INodeBuilder<T> OutputMapper(IRowMapper<T> rowMapper);
        IChildNodeBuilder<T> StoredProcedure(String procName);
    }
}
