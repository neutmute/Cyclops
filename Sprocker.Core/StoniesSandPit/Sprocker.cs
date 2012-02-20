using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core.FluentInterface
{
    /// <summary>
    /// 
    /// I need:
    /// 
    /// a single database reference to deal with
    ///  
    /// 
    /// 
    /// 
    /// </summary>
    public class Sprocker
    {
        // protect the client from this mess.
        public Database database { get; set; }

       // private IEnumerable<SprocAccessor<>> SprocAccessors { get; set; }

        public bool isTransactional { get; set; }

        public string StoredProc { get; set; }

        public Sprocker()
        {
            
        }
    }
}
