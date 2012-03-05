using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Sprocker.Core.Mapping
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
    public abstract class SprocMap<TCriteria, TEntity>
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

        /// <summary>
        /// 
        /// </summary>
        internal void discoverProcParmeters()
        {

            using (
                SqlConnection connection =
                    new SqlConnection(
                        @"Data Source=(local)\Sql2008;Initial Catalog=PetStore.TestDatabase;Integrated Security=SSPI;")
                )
            {
                // Create Command
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = ProcName;

                // Open Connection
                connection.Open();

                // Discover Parameters for Stored Procedure
                // Populate command.Parameters Collection.
                SqlCommandBuilder.DeriveParameters(command);

                foreach (SqlParameter sqlParameter in command.Parameters)
                {
                    Console.WriteLine(sqlParameter.Value);
                    SprocParameters.Add(sqlParameter);
                }
            }
        }
    }
}
