using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;
using PetStore.Infrastructure;
using Sprocker.Core;

namespace PetStore.IntegrationTest
{
    [TestClass]
    public class OrderRepositoryTest
    {
        public static Order GetUnpersistedOrder()
        {
            Order order = new Order();
            order.Customer = new Customer {Id = 1};
            order.CreatedBy = "Toot";
            order.ModifiedBy = "Xandir";

            order.OrderLines.Add(new OrderLine { ProductId = 1, UnitPriceCents = 100, CreatedBy = "Captain Hero", ModifiedBy = "Wooldoor" });
            order.OrderLines.Add(new OrderLine { ProductId = 2, UnitPriceCents = 89, CreatedBy = "Captain Hero", ModifiedBy="Wooldoor" });

            return order;
        }

        private OrderRepository GetNewRepo()
        {
            OrderRepository repo = new OrderRepository();
            repo.Database = new SprockerSqlDatabase(Constants.TestDatabaseConnectionString);
            return repo;
        }

        [TestMethod]
        public void Order_Get()
        {
            OrderRepository repo = GetNewRepo();
            Order order = repo.GetOne(o => o.Id == 1);
        }

        [TestMethod]
        public void TableValuedParameterDemo()
        {
            OrderRepository repo = GetNewRepo();
            Order order = GetUnpersistedOrder();

            repo.Save(order);
        }


    }
}
