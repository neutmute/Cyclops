using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace PetStore.IntegrationTest
{
    public class Config
    {
        public static string ConnectionString 
        {
            get { return ConfigurationManager.ConnectionStrings["PetStore"].ConnectionString; }
        }
    }
}
