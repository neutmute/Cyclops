using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sprocker.Core.Mapping
{
    /// <summary>
    /// Holds the mapping between a criteria object and proc prams'
    /// </summary>
    /// <typeparam name="TCriteria"></typeparam>
    public class CriteriaMap<TCriteria>
    {
        public List<Expression<Func<TCriteria, object>>> CriteriaExpressions { get; set; }

        private SqlParameterCollection sqlParameterCollection; 
        
        //private readonly ParameterCache parameterCache = new ParameterCache();

        public CriteriaMap()
        {
            
        

        }
    }
}
