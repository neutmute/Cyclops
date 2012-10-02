using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PetStore.Domain;
using Cyclops;

namespace PetStore.Infrastructure
{
    public class CustomerRepository : CyclopsRepository, ICustomerRepository
    {
        private static IRowMapper<Customer> _rowMapper;

        static CustomerRepository()
        {
            _rowMapper = MapBuilder<Customer>
                                        .MapAllProperties()
                                        .Build();
        }

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
            var command = BuildCustomerSaveCommand(customer);
            command.ExecuteNonQuery();
            customer.Id = command.GetParameterValue<int>("@Id");
            return customer;
        }

        /// <summary>
        /// Useful for testing
        /// </summary>
        protected CyclopsCommand BuildCustomerSaveCommand(Customer customer)
        {
            var command = ConstructCommand<Customer>("dbo.Customer_Save")
                                        .MapAllParameters()
                                        .Build(customer);
            return command;
        }

        public void Delete(Customer instance)
        {
            ConstructCommand("dbo.Customer_Delete").ExecuteNonQuery(instance.Id, instance.IsDeleted);
        }

        public static List<Customer> MapAddresses(DataTable customerTable)
        {
            return EntityMapper.Map(customerTable, _rowMapper);
        }
    }
}
