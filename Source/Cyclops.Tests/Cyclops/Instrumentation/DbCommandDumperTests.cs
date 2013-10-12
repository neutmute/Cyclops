using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Infrastructure;
using PetStore.IntegrationTest.Infrastructure;

namespace PetStore.IntegrationTest.Cyclops.Instrumentation
{
    [TestClass]
    public class DbCommandDumperTests
    {
        public static TestRepository GetNewRepo()
        {
            var repo = new TestRepository();
            repo.Database = new SqlDatabase(Config.ConnectionString);
            return repo;
        }

        /// <summary>
        /// Didn't quote the text when started using this data type.
        /// Tests that quotes are contained in the expected data
        /// </summary>
        [TestMethod]
        public void DateTimeOffset_LoggedOk()
        {
            var repo = ExposedCustomerRepository.Construct();
            var customer = CustomerRepositoryTests.GetUnpersistedCustomer();
            customer.DateOfBirth = DateTimeOffset.Parse("2012-09-28 05:15:14.9344427 +10:00");

            var command = repo.GetCustomerSaveCommand(customer);

            var commandDumper = new DbCommandDumper(command.DbCommand);
            var dump = commandDumper.GetLogDump();

            Assert.IsTrue(dump.Contains("'28-Sep-12 05:15:14 +10:00'"));
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
