using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using PetStore.Domain;
using PetStore.Infrastructure;

namespace PetStore.IntegrationTest
{
    public class ExposedCustomerRepository : CustomerRepository
    {
        public static ExposedCustomerRepository Construct()
        {
            var customerRepository = new ExposedCustomerRepository();
            customerRepository.Database = new SqlDatabase(Constants.TestDatabaseConnectionString);
            return customerRepository;
        }

        private ExposedCustomerRepository()
        {
            
        }

        public CyclopsCommand GetCustomerSaveCommand(Customer customer)
        {
            return BuildCustomerSaveCommand(customer);
        }
    }
}
