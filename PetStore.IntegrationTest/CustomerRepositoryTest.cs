using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;
using PetStore.Infrastructure;
using Sprocker.Core;

namespace PetStore.IntegrationTest
{
    [TestClass]
    public class CustomerRepositoryTest
    {
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
            CustomerRepository customerRepository = new CustomerRepository();
            customerRepository.Database = new SprockerSqlDatabase(Constants.TestDatabaseConnectionString);

            Customer customer = GetWellKnownCustomer();

            Assert.AreEqual(0, customer.Id, "Initial state not persisted");
            
            customerRepository.Save(customer);

            Assert.IsTrue(customer.Id > 0, "Persisted! Probably.");

            
        }
    }
}
