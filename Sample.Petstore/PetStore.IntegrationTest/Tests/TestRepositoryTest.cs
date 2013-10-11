using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;
using PetStore.Infrastructure;

namespace PetStore.IntegrationTest.Tests
{
    [TestClass]
    public class TestRepositoryTest
    {
        public static TestRepository GetNewRepo()
        {
            var repo = new TestRepository();
            repo.Database = new SqlDatabase(Constants.TestDatabaseConnectionString);
            return repo;
        }

        [TestMethod]
        public void BinaryLogDump_EmitsExpectedBinary()
        {
            TestRepository repo = GetNewRepo();

            var input = new List<KeyValuePair<int, byte[]>>();

            input.Add(new KeyValuePair<int, byte[]>(1, new byte[] { 0x01, 0x02 }));

            var command = repo.BinaryLogDump(input);
            DbCommandDumper dump = new DbCommandDumper(command.DbCommand);
            var output = dump.GetLogDump();

            Assert.IsTrue(output.Contains("0x0102"));
        }
    }
}
