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
            DataSet dataSet = ConstructCommand("dbo.Order_Get").ExecuteDataSet();
            DataTable orderTable = dataSet.Tables[0];
            DataTable customerTable = dataSet.Tables[1];

            List<Customer> customerList = CustomerRepository.MapAddresses(customerTable);
            Func<int, Customer> getCustomer = i => customerList.Find(c => i == c.Id);
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
