//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using Microsoft.Practices.EnterpriseLibrary.Data;

//namespace TheSprocker.Core
//{
//    /// <summary>
//    /// </summary>
//    /// <typeparam name="TResult"></typeparam>
//    public class Sprocker
//    {
//        public Sprocker()
//        {
            
//        }

//        public void Execute()
//        {
//            using (SqlConnection conn = new SqlConnection(Config.ConnectionString))
//            {
//                SqlCommand cmd = new SqlCommand("Address_Get", conn);

//                //cmd.Parameters.  get these from our map. 

//                conn.Open();
//                SqlDataReader dr = cmd.ExecuteReader();
//                try
//                {
//                    // map our stuff on the way out and return the entity. 

//                    while (dr.Read())
//                        Console.WriteLine(dr.GetString(0));
//                }
//                finally
//                {
//                    dr.Close();
//                }
//            }
//        }
//    }

//}
