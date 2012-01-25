using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core.FluentInterface
{
    public class Sprocker
    {
        // protect the client from this mess.
        public Database database { get; set; }

        public bool isTransactional { get; set; }

        public string StoredProc { get; set; }

        public Sprocker()
        {
            
        }
    }
}
