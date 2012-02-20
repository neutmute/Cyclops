using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Sprocker.Core.StoniesSandPit
{
    /// <summary>
    /// Map an object graph to Stored Procs
    /// </summary>
    public class SprocMap<TEntity, TCriteria>
    {
        // parameter map

        // result map

        // children 
        	//One-to-many (Collection mappings)
            //Many-to-many
            //One-to-one
            
        //DAAB

            //.MapAllProperties()
            //.DoNotMap(p => p.IsActive)
            //.DoNotMap(p => p.State)
            //.Map(p=> p.State).ToColumn("LegacyCode")
            //.Build();

        // Fluent NHibernate

        public SprocMap<TEntity, TCriteria> MapPrameter(Expression<Func<TCriteria, object>> prameterExpression)
        {

            return this;
        }

        public SprocMap<TEntity, TCriteria> MapResult(Expression<Func<TEntity, object>> memberExpression)
        {

            return this;
        }

        public SprocMap<TEntity, TCriteria> Proc(string name)
        {

            return this;
        }

        public void IsTransactional(bool isTransactional)
        {
        
        }

    }
}
