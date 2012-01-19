using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using PetStore.Domain;
using Sprocker.Core;

namespace PetStore.Infrastructure
{
    public class CustomerRepository : SqlRepository, ICustomerRepository
    {

        public Customer GetOne(Predicate<Customer> filter)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetAll(Predicate<Customer> filter)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetAll()
        {
            return Database.ExecuteSprocAccessor<Customer>(90, "dbo.Customer_Get").ToList();
        }

        public Customer Save(Customer customer)
        {
            DbCommand command = DbCommandBuilder<Customer>.MapAllParameters(Database, "Customer_Save")
                                                            .Build(customer);


            Database.ExecuteNonQuery(command);
            customer.Id = command.GetParameterValue<int>("@Id");
            return customer;
        }

        public void Delete(Customer instance)
        {
            throw new NotImplementedException();
        }
    }
}
