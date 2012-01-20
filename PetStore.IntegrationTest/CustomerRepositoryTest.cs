using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Targets;
using PetStore.Domain;
using PetStore.Infrastructure;
using Sprocker.Core;
using NLog;

namespace PetStore.IntegrationTest
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        public MemoryTarget GetMemoryTarget()
        {
            return GetMemoryTarget("${message}|${exception:format=tostring}", LogLevel.Info);
        }

        /// <summary>
        /// Get a target to allow assertions to be made against the Nlog
        /// </summary>
        public MemoryTarget GetMemoryTarget(LogLevel logLevel)
        {
            return GetMemoryTarget("${message}", logLevel);
        }

        /// <summary>
        /// Get a target to allow assertions to be made against the Nlog
        /// </summary>
        public MemoryTarget GetMemoryTarget(string layout, LogLevel logLevel)
        {
            MemoryTarget memoryTarget = new MemoryTarget { Layout = layout };
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(memoryTarget, logLevel);
            return memoryTarget;
        }

        public Customer GetWellKnownCustomer()
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

        [TestMethod]
        public void Customer_Get_Success()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            customerRepository.Database = new SprockerSqlDatabase(Constants.TestDatabaseConnectionString);

            customerRepository.Database.ExecuteNonQuery(CommandType.Text,
                                                        @"
DELETE FROM Customer;

declare @p1 int
exec Customer_Save @Id=@p1 output,@Title=N'Super He',@FirstName=N'Captain',@LastName=N'Hero',@EmailPromotion=0,@IsActive=1,@IsDeleted=0,@DateCreated='2012-01-16 22:35:02.423',@CreatedBy='Dave Jesser',@DateModified='2012-01-16 22:35:02.423',@ModifiedBy='Xandir'
");

            List<Customer> customers = customerRepository.GetAll();

            Assert.AreEqual(1, customers.Count);
        }

        [TestMethod]
        public void Customer_Saves_Success()
        {
            CustomerRepository customerRepository = CreateCustomerRepo();

            Customer customer = GetWellKnownCustomer();

            Assert.AreEqual(0, customer.Id, "Initial state not persisted");
            
            customerRepository.Save(customer);

            Assert.IsTrue(customer.Id > 0, "Persisted! Probably.");

            
        }

        private CustomerRepository CreateCustomerRepo()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            customerRepository.Database = new SprockerSqlDatabase(Constants.TestDatabaseConnectionString);
            return customerRepository;
        }

        [TestMethod]
        public void Sprocker_MapsParameters_Success()
        {
            CustomerRepository customerRepository = CreateCustomerRepo();
            Customer customer = new Customer {Id = 1, IsDeleted = false};
            customerRepository.Delete(customer);
        }

        [TestMethod]
        public void Sprocker_LogsFailure()
        {
            MemoryTarget target = GetMemoryTarget();
            try
            {
                CustomerRepository customerRepository = CreateCustomerRepo();
                Customer customer = new Customer { Id = 1, IsDeleted = true };
                customerRepository.Delete(customer);
            }
            catch (Exception e)
            {
                
            }

            List<string> logs = target.Logs.ToList();
            Assert.AreEqual(logs[0], "Failed CommandText: \r\nDECLARE @RETURN_VALUE INT\r\n\t\t,@Id INT\r\n\t\t,@IsDeleted BIT;\r\nSELECT @RETURN_VALUE = -5\r\n\t\t,@Id = 1\r\n\t\t,@IsDeleted = 1;\r\nEXEC dbo.Customer_Delete\r\n\t\t@RETURN_VALUE = @RETURN_VALUE \r\n\t\t,@Id = @Id \r\n\t\t,@IsDeleted = @IsDeleted ;|");
        }
    }
}
