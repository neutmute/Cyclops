using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PetStore.Domain;
using Sprocker.Core;

namespace PetStore.Infrastructure
{
    public class OrderRepository : SqlRepository //, IOrderRepository
    {
        public IRowMapper<Order> GetRowMapper(Func<int, Customer> getCustomer)
        {
            IRowMapper<Order> rowMapper =
                MapBuilder<Order>.MapAllProperties()

               .Map(o => o.Customer).WithFunc(row => getCustomer((int) row["CustomerId"]))
               .Build();

            return rowMapper;
        }

        public Order GetOne(Predicate<Order> filter)
        {
            return GetAll(filter).FirstOrDefault();
        }

        public List<Order> GetAll(Predicate<Order> filter)
        {
            return GetAll().FindAll(filter).ToList();
        }

        public List<Order> GetAll()
        {
            var tableSet = ConstructCommand("dbo.Order_Get").ExecuteTableSet();
            
            // Construct the customers first (child objects)
            List<Customer> customerList = CustomerRepository.MapAddresses(tableSet["Customer"]);

            // Build a method to find a customer given an Id
            Func<int, Customer> getCustomer = i => customerList.Find(c => i == c.Id);

            // Construct the parent (orders) and pass in the function with how to find the required customer for the rowmapper
            List<Order> orders = EntityMapper.Map(tableSet["Order"], GetRowMapper(getCustomer));

            return orders;
        }

        public Order Save(Order instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(Order instance)
        {
            throw new NotImplementedException();
        }

        public Order GetOrderForCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
