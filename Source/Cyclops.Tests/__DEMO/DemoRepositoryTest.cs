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
             
            repo.ExecProcForNonQuery();
            repo.ExecProcForDataTable();

            repo.MapToProcFromObject(new MyClass {Param1 = 3, Param2 = "MappedNonQuery!"});
            repo.MapToProcFromEnum(new MyEnumClass { Param1 = 4, Param2 = "MappedEnum", Colour = Colour.Green });

            repo.MapToProcWithValue(new MyClass { Param1 = 5, Param2 = "Call Enum proc with no matching property so map manually" });
            repo.MapToProcWithNull(new MyClass { Param1 = 6, Param2 = "Call Enum proc with no matching property so map manually with null" });
            repo.MapToProcWithFunc(new MyClass { Param1 = 6, Param2 = "Call proc with custom mapping" });

            repo.MapToObjectSimple();
            repo.MapToObjectWithColumns();
            repo.MapToObjectWithEnum();
            repo.MapToObjectWithFunc();
        }
    }
}
