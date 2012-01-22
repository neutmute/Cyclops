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

    }
}
