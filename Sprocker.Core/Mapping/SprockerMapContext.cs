using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace TheSprocker.Core.Mapping
{
    /// <summary>
    /// Map an object graph to Stored Procs
    /// </summary>
    /// <remarks>
    /// most of the work is here:
    /// 
    /// reflect the criteria object
    /// get parameters off proc 
    /// match these two together
    /// set the type of parameter from the reflected type 
    /// compile an expression tree that will allow the executor to call the proc
    /// save the expression tree here. 
    /// 
    /// reflect the entity
    /// get the output parameters 
    /// match on name
    /// compile the expression tree that will allow the exector to create the type
    /// save the expression tree here
    /// 
    /// </remarks>
    public class SprockerMapContext
    {
        /// <summary>
        /// TODO: inject this 
        /// </summary>
        private TypeReflector typeReflector = new TypeReflector();

        /// <summary>
        /// 
        /// </summary>
        private  SprocInspector sprocInspector = new SprocInspector();

        /// <summary>
        /// Name of the stored proc
        /// </summary>
        public string ProcName { get; set; }

        /// <summary>
        /// attempt to match purely based on convention 
        /// </summary>
        public bool AutomapAll { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        public Type ParamtererType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Type ResultType { get; set; }

        /// <summary>
        /// Stores the parameters of the proc
        /// </summary>
        public List<IDataParameter> SprocParameters { get; set; } //SqlParameter

        /// <summary>
        /// 
        /// </summary>
        public IList<PropertyInfo> ParameterMembers { get; set; }

        //List<CriteriaMap<TEntity>> CriteriaMaps { get; set; }
        
        //List<ResultMap<TEntity>> ResultMaps { get; set; }


        /// <summary>
        /// ctor
        /// </summary>
        public SprockerMapContext(string procName)
        {
            ProcName = procName;
            //CriteriaMaps = new List<CriteriaMap<TEntity>>();
            //ResultMaps = new List<ResultMap<TEntity>>();
        }

        public void AutoMap()
        {
            // specify that convention over configuration should be used
            // NB: extend this with .Ignore() .ToColumn() ? .All()

            //reflect the criteria object
            ParameterMembers = typeReflector.LocateMappingCandidates(ParamtererType);

            //get parameters off proc 

            sprocInspector.ProcName = ProcName;
            SprocParameters = sprocInspector.discoverProcParmeters();

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




        ///// <summary>
        ///// Explicit mapping overide. 
        ///// </summary>
        ///// <param name="parameterExpression"></param>
        //public void MapInput(Expression<Func<TCriteria, object>> parameterExpression)
        //{
        //    CriteriaMap<TCriteria> criteriaMap = new CriteriaMap<TCriteria>();
        //    criteriaMap.CriteriaExpressions.Add(parameterExpression);

        //    // add expression to parameter maps. 
        //   // MapContext.CriteriaMaps.Add(criteriaMap);
        //}

        ///// <summary>
        ///// Explicit output mapping overide. 
        ///// </summary>
        ///// <param name="memberExpression"></param>
        //public void MapResult(Expression<Func<TEntity, object>> memberExpression)
        //{

        //    // add expression to output maps 
        //}
    }
}
