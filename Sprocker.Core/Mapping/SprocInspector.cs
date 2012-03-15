﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TheSprocker.Core.Mapping
{
    public class SprocInspector
    {
        public string ProcName { get; set; }

        /// <summary>
        /// Stores the parameters of the proc
        /// </summary>
        public List<IDataParameter> SprocParameters { get; set; } //SqlParameter

        

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