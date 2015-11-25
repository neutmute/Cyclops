using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Kraken.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Targets;
using PetStore.Domain;
using PetStore.Infrastructure;
using Cyclops;
using Common.Logging;

namespace PetStore.IntegrationTest
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        public static CustomerRepository GetCustomerRepository()
        {
            var customerRepository = new CustomerRepository();
            customerRepository.Database = new SqlDatabase(Config.ConnectionString);
            return customerRepository;
        }

        public static Customer GetPersistedCustomer()
        {
            var repo = GetCustomerRepository();
            var customer = GetWellKnownCustomer();
            repo.Save(customer);
            return customer;
        }

        public static Customer GetWellKnownCustomer()
        {
            Customer customer = new Customer
                                    {
                                        FirstName = "Captain",
                                        LastName = "Hero",
                                        Title = "Super Hero",
                                        IsActive = true,
                                        CreatedBy = "Dave Jesser",
                                        ModifiedBy = "Xandir"
                                    };
            return customer;
        }

        public static MemoryTarget GetMemoryTarget()
        {
            return GetMemoryTarget("${message}|${exception:format=tostring}", NLog.LogLevel.Info);
        }

        /// <summary>
        /// Get a target to allow assertions to be made against the Nlog
        /// </summary>
        public static MemoryTarget GetMemoryTarget(NLog.LogLevel logLevel)
        {
            return GetMemoryTarget("${message}", logLevel);
        }

        /// <summary>
        /// Get a target to allow assertions to be made against the Nlog
        /// </summary>
        public static MemoryTarget GetMemoryTarget(string layout, NLog.LogLevel logLevel)
        {
            MemoryTarget memoryTarget = new MemoryTarget { Layout = layout };
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(memoryTarget, logLevel);
            return memoryTarget;
        }

        [TestCategory("SqlIntegration")]
        [TestMethod]
        public void Customer_Save_Success()
        {
            GetPersistedCustomer();
        }

        [TestCategory("SqlIntegration")]
        [TestMethod]
        public void Customer_Get_Success()
        {
           var customerRepository = GetCustomerRepository();
            customerRepository.Database.ExecuteNonQuery(CommandType.Text,
                                                        @"
DELETE FROM OrderLine;
DELETE FROM Customer_Order;
DELETE FROM [Order];
DELETE FROM Customer;


declare @p1 int
exec Customer_Save @Id=@p1 output,@Title=N'Super He',@FirstName=N'Captain',@LastName=N'Hero',@EmailPromotion=0,@IsActive=1,@IsDeleted=0,@DateCreated='2012-01-16 22:35:02.423',@CreatedBy='Dave Jesser',@DateModified='2012-01-16 22:35:02.423',@ModifiedBy='Xandir'
");

            List<Customer> customers = customerRepository.GetAll();

            Assert.AreEqual(1, customers.Count);
        }

        [TestCategory("SqlIntegration")]
        [TestMethod]
        public void Customer_Saves_Success()
        {
            var customerRepository = GetCustomerRepository();

            Customer customer = GetWellKnownCustomer();

            Assert.AreEqual(0, customer.Id, "Initial state not persisted");
            
            customerRepository.Save(customer);

            var customerReloaded = customerRepository.GetOne(c => c.Id == customer.Id);

            Assert.IsTrue(customer.Id > 0, "Persisted! Probably.");
            Assert.IsTrue(customerReloaded.Id == customer.Id, "Persisted! Definitely.");
        }

        [TestCategory("SqlIntegration")]
        [TestMethod]
        public void Cyclops_MapsParameters_Success()
        {
            CustomerRepository customerRepository = GetCustomerRepository();
            Customer customer = new Customer {Id = 1, IsDeleted = false};
            customerRepository.Delete(customer);
        }

        [TestCategory("SqlIntegration")]
        [TestMethod]
        public void Cyclops_LogsFailure()
        {
            MemoryTarget target = GetMemoryTarget();
            try
            {
                CustomerRepository customerRepository = GetCustomerRepository();
                Customer customer = new Customer { Id = 1, IsDeleted = true };
                customerRepository.Delete(customer);
            }
            catch (Exception)
            {
                
            }

            List<string> logs = target.Logs.ToList();
            Assert.IsTrue(logs[0].Contains("You cannot delete an already deleted customer"));
        }
    }
}
