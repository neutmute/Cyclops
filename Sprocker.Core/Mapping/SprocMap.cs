using System;
using System.Linq.Expressions;

namespace Sprocker.Core.Mapping
{
    /// <summary>
    /// Map an object graph to Stored Procs
    /// </summary>
    public class SprocMap<TEntity, TCriteria> 
    {
        public MapContext<TEntity, TCriteria> Type { get; set; }

        // parameter map

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

        public void MapInput(Expression<Func<TCriteria, object>> prameterExpression)
        {

        }

        public IMapContext<TEntity, TCriteria> MapResult(Expression<Func<TEntity, TCriteria, object>> memberExpression)
        {

            return new MapContext<TEntity, TCriteria>();
        }

        public void Proc(string name)
        {

        }

        public void IsTransactional(bool isTransactional)
        {
        
        }

    }
}
