using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace TheSprocker.Core.Mapping
{
    /// <summary>
    /// Map an object graph to Stored Procs
    /// </summary>
    /// <remarks>
    /// most of the work is here:
    /// 
    /// refelect the critera object
    /// get parmeters off proc 
    /// match these two together
    /// set the type of parameter from the refelcted type 
    /// compile an expression tree that will allow the executor to call the proc
    /// save the expression tree here. 
    /// 
    /// refelct the entity
    /// get the output parameters 
    /// match on name
    /// compile the exprestion tree that will allow the exector to create the type
    /// save the expression tree here
    /// 
    /// </remarks>
    public class SprocMap<TCriteria, TEntity>
    {
        /// <summary>
        /// Name of the stored proc
        /// </summary>
        public string ProcName { get; private set; }

        /// <summary>
        /// Stores the parameters of the proc
        /// </summary>
        public List<IDataParameter> SprocParameters { get; set; } //SqlParameter

        /// <summary>
        /// Stores the map sets. 
        /// </summary>
        public IMapContext<TEntity, TCriteria> MapContext { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public SprocMap(string procName)
        {
            ProcName = procName;
            MapContext = new MapContext<TEntity, TCriteria>();
        }

        public void AutoMap()
        {
            // specify that convention over configuration should be used
            // NB: extend this with .Ignore() .ToColumn() ? .All()

             //reflect the criteria object
             //get parameters off proc 
             //match these two together
             //set the type of parameter from the reflected type 
             //compile an expression tree that will allow the executor to call the proc
             //save the expression tree here. 
             
             //reflect the entity
             //get the output parameters 
             //match on name
             //compile the expression tree that will allow the exector to create the type
             //save the expression tree here




        }


        /// <summary>
        /// Explicit mapping overide. 
        /// </summary>
        /// <param name="parameterExpression"></param>
        public void MapInput(Expression<Func<TCriteria, object>> parameterExpression)
        {
            CriteriaMap<TCriteria> criteriaMap = new CriteriaMap<TCriteria>();
            criteriaMap.CriteriaExpressions.Add(parameterExpression);

            // add expression to parameter maps. 
            MapContext.CriteriaMaps.Add(criteriaMap);
        }

        /// <summary>
        /// Explicit output mapping overide. 
        /// </summary>
        /// <param name="memberExpression"></param>
        public void MapResult(Expression<Func<TEntity, object>> memberExpression)
        {

            // add expression to output maps 
        }
    }
}
