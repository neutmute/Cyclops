using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core
{
    /// <summary>
    /// A replacement for SprocAccessor that hides more of the details of 
    /// Graph construction from the user. 
    /// 
    /// Can be created using a fluent interface builder pattern. 
    /// Can be created using a fluent interface builder pattern. 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class Sprocker<TResult>
    {
        // injected later.
        private Database _database { get; set; }

        public Sprocker()
        {
            
        }
    }

}
