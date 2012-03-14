using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace TheSprocker.Core.FluentInterface.Behaviours
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">this is required for the row mapper</typeparam>
    public interface IRootMapBuilder
    {
        IRootMapBuilder Proc(String procedureName);
        IRootMapBuilder AutoMapAll(); // another interface for the exclusions
        void Build();
    }
}
