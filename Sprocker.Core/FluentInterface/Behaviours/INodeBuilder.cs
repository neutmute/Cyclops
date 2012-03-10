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
    public interface IBaseBuilder
    {
        IBaseBuilder Proc(String procedureName);
        IBaseBuilder AutoMapAll(); // another interface for the exclusions
        void Build();
    }
}
