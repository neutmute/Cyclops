using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.IntegrationTest.Infrastructure;

namespace PetStore.IntegrationTest.Cyclops.Instrumentation
{
    [TestClass]
    public class DbCommandDumperTests
    {
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
    }
}
