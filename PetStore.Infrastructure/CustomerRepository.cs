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
            return GetAll(filter).FirstOrDefault();
        }

        public List<Customer> GetAll(Predicate<Customer> filter)
        {
            return GetAll().FindAll(filter);
        }

        public List<Customer> GetAll()
        {
            return ConstructCommand("dbo.Customer_Get").ExecuteRowMap<Customer>();
        }

        public Customer Save(Customer customer)
        {
            SprockerCommand command = ConstructCommand<Customer>("dbo.Customer_Save")
                                        .MapAllParameters()
                                        .Build(customer);

            command.ExecuteNonQuery();
            customer.Id = command.GetParameterValue<int>("@Id");
            return customer;
        }

        public void Delete(Customer instance)
        {
            ConstructCommand("dbo.Customer_Delete").ExecuteNonQuery(instance.Id, instance.IsDeleted);
        }
    }
}
