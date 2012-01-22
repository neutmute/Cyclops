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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAll()
        {
            // This proc returns customer and order dataTables
            DataSet dataSet = ConstructCommand("dbo.Order_Get").ExecuteDataSet();

            // Just have to know the order with magic numbers
            DataTable orderTable = dataSet.Tables[0];
            DataTable customerTable = dataSet.Tables[1];

            // Construct the customers first (child objects)
            List<Customer> customerList = CustomerRepository.MapAddresses(customerTable);

            // Build a method to find a customer given an Id
            Func<int, Customer> getCustomer = i => customerList.Find(c => i == c.Id);

            // Construct the orders and pass in the function with how to find the required customer
            List<Order> orders = EntityMapper.Map(orderTable, GetRowMapper(getCustomer));

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
