using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace TheSprocker.Core.Mapping
{
    /// <summary>
    /// Execute the map
    /// </summary>
    /// <remarks>
    /// 
    /// get the expression trees from the maps and call them on the database 
    /// 
    /// </remarks>
    public class SprockerExecutor<TMap> //where TMap :
    {
        //Transaction transaction // perhaps one can be passed?

        //warn on overwrite? basic last modified warning? throw?

        public SprockerExecutor()
        {
            
        }

        public void Execute()
        {
            using (SqlConnection conn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Address_Get", conn);

                //cmd.Parameters.  get these from our map. 

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    // map our stuff on the way out and return the entity. 

                    while (dr.Read())
                        Console.WriteLine(dr.GetString(0));
                }
                finally
                {
                    dr.Close();
                }
            }
        }
    }
}
