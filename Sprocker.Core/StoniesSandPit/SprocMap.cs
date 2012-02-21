using System;
using System.Linq.Expressions;

namespace Sprocker.Core.StoniesSandPit
{
    /// <summary>
    /// Map an object graph to Stored Procs
    /// </summary>
    public class SprocMap<TEntity, TCriteria> 
    {
        // parameter map
            //Cache

        // result map
            //Cache expression

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

            //        Id(x => x.Id);
            //Map(x => x.Name);
            //HasManyToMany(x => x.Products)
            //    .Cascade.All()
            //    .Table("StoreProduct");
            //HasMany(x => x.Staff)
            //    .Cascade.All()
            //    .Inverse();

        public void AutoMapAll()
        {
        
        
        }


        public void MapInput(Expression<Func<TCriteria, object>> prameterExpression)
        {

        }

        public IMapContext<TEntity> MapResult(Expression<Func<TEntity, object>> memberExpression)
        {

            return new MapContext<TEntity>();
        }

        public void Proc(string name)
        {


        }

        public void IsTransactional(bool isTransactional)
        {
        
        }

    }
}
