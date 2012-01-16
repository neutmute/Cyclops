using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Sprocker.Core
{
    public class Sprocker : SqlDatabase
    {
        public Sprocker(string connectionString) : base(connectionString)
        {
        }

        public Sprocker(string connectionString, IDataInstrumentationProvider instrumentationProvider) : base(connectionString, instrumentationProvider)
        {
        }
    }
}
