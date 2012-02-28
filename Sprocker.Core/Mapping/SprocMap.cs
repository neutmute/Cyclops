using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Sprocker.Core.Mapping
{
    /// <summary>
    /// Map an object graph to Stored Procs
    /// </summary>
    public class SprocMap<TEntity, TCriteria>
    {
        public string ProcedureName { get; private set; }

        public IMapContext<TEntity, TCriteria> MapContext { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public SprocMap()
        {
            MapContext = new MapContext<TEntity, TCriteria>();
        }

        public void MapInput(Expression<Func<TCriteria, object>> parameterExpression)
        {
            CriteriaMap<TCriteria> criteriaMap = new CriteriaMap<TCriteria>();
            criteriaMap.CriteriaExpressions.Add(parameterExpression);

            // add expression to parameter maps. 
            MapContext.CriteriaMaps.Add(criteriaMap);
        }

        public void MapResult(Expression<Func<TEntity, object>> memberExpression)
        {

            // add expression to output maps 
        }

        public void Proc(string name)
        {
            ProcedureName = name;
        }

        /// <summary>
        /// maybe we allow transaction settings to be passed in?
        /// </summary>
        /// <param name="isTransactional"></param>
        public void IsTransactional(bool isTransactional)
        {

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
                command.CommandText = ProcedureName;

                // Open Connection
                connection.Open();

                // Discover Parameters for Stored Procedure
                // Populate command.Parameters Collection.
                SqlCommandBuilder.DeriveParameters(command);

                foreach (SqlParameter sqlParameter in command.Parameters)
                {
                    Console.WriteLine(sqlParameter.Value);
                }
            }
        }
    }
}
