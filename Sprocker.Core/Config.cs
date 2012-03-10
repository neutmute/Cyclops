using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheSprocker.Core
{
    public static class Config
    {
        public static string ConnectionString = @"Data Source=(local)\Sql2008;Initial Catalog=PetStore.TestDatabase;Integrated Security=SSPI;";

        static Config()
        {
            //read from config section...
        }
    }
}
