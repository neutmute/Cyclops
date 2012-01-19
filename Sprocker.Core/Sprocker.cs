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

    /// <summary>
    /// 
    /// ok the idea here is to wrap the sproc accessor provided bu DAABs database objects.
    /// 
    /// We need to 
    /// 
    /// support collections of IParameterMappers both
    ///     *  SaveEntityParameterMapper
    ///     *  
    ///
    /// those need to be passed to the DataAccessor class (SprocAccessor)
    /// 
    /// </summary>
    public class Sprocker
    {
        public Database Database { get; set; }

        public Sprocker(Database database)
        {
            Database = database;
        }


        // collection mappers

        



    }
}
