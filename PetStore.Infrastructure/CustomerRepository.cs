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
            return BuildCommand("dbo.Customer_Get").Execute<Customer>();
        }

        public Customer Save(Customer customer)
        {
            SprockerCommand command = SprockerCommandBuilder<Customer>
                                        .MapAllParameters(Database, "dbo.Customer_Save")
                                        .Build(customer);

            command.ExecuteNonQuery();
            customer.Id = command.GetParameterValue<int>("@Id");
            return customer;
        }

        public void Delete(Customer instance)
        {
            BuildCommand("dbo.Customer_Delete").ExecuteNonQuery(instance.Id, instance.IsDeleted);
        }
    }
}
