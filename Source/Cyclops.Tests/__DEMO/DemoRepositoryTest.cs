using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops;
using Cyclops.Tests.__DEMO;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;
using PetStore.Infrastructure;

namespace PetStore.IntegrationTest.Tests
{
    [TestClass]
    public class DemoRepositoryTest
    {
        [TestMethod]
        public void Demonstrate()
        {
            var repo = new DemoRepository { Database = new SqlDatabase(Config.ConnectionString) };

            repo.SimpleParametersNonQuery();
            repo.SimpleParametersDataTable();
            repo.MappedNonQuery(new MyClass {Param1 = 3, Param2 = "MappedNonQuery!"});
            repo.MappedEnum(new MyEnumClass { Param1 = 4, Param2 = "MappedEnum", Colour = Colour.Green });
        }
    }
}
