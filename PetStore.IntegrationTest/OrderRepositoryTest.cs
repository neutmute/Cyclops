using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Kraken.Framework.TestMonkey;
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
            order.OrderLines.Add(new OrderLine { ProductId = 2, UnitPriceCents = 89, CreatedBy = "Toot", ModifiedBy="Xandir" });

            return order;
        }

        private OrderRepository GetNewRepo()
        {
            OrderRepository repo = new OrderRepository();
            repo.Database = new SprockerSqlDatabase(Constants.TestDatabaseConnectionString);
            return repo;
        }

        //[TestMethod]
        //public void Order_Get()
        //{
        //    OrderRepository repo = GetNewRepo();
        //    Order order = repo.GetOne(o => o.Id == 1);


        //}

        /// <summary>
        /// Test expects data to have been generated and requries transactions to rollback other tests.
        /// just hacked for now
        /// </summary>
        [TestMethod]
        public void Order_Get_Success()
        {
            OrderRepository repo = GetNewRepo();
            Order order = repo.Get(1);

            Assert.AreEqual(1, order.Id);
            Assert.IsTrue(order.OrderLines.Count > 0);
            Assert.IsNotNull(order.Customer);
        }

        /// <summary>
        /// Test expects data to have been generated and requries transactions to rollback other tests.
        /// just hacked for now
        /// </summary>
        [TestMethod]
        public void TableValuedParameterDemo()
        {
            OrderRepository repo = GetNewRepo();
            Order order = GetUnpersistedOrder();
            repo.Save(order);
        }

        /// <summary>
        /// AssertBuilder! OMG! sorry - had to for testing my valueinjector
        /// </summary>
        [TestMethod]
        public void OrderLineList_MapsTo_DataTAble()
        {

            OrderRepository repo = GetNewRepo();
            List<OrderLine> lines = GetUnpersistedOrder().OrderLines;
            repo.MapToDataTable("OrderLineTableType", lines);

            AssertBuilder builder = new AssertBuilder();
            //builder.Generate(lines, "lines");

            // AssertBuilder.Generate(lines, "lines"); // The following assertions were generated on 24-Jan-2012
            #region CodeGen Assertions
            Assert.AreEqual(2, lines.Count);
            Assert.AreEqual(0, lines[0].Id);
            Assert.AreEqual(0, lines[0].OrderId);
            Assert.AreEqual(1, lines[0].ProductId);
            Assert.AreEqual(0, lines[0].OrderQty);
            Assert.AreEqual(100, lines[0].UnitPriceCents);
            Assert.AreEqual(false, lines[0].IsActive);
            Assert.AreEqual(false, lines[0].IsDeleted);
            Assert.AreEqual(DateTime.MinValue, lines[0].DateCreated);
            Assert.AreEqual("Captain Hero", lines[0].CreatedBy);
            Assert.AreEqual(DateTime.MinValue, lines[0].DateModified);
            Assert.AreEqual("Wooldoor", lines[0].ModifiedBy);
            Assert.AreEqual(0, lines[1].Id);
            Assert.AreEqual(0, lines[1].OrderId);
            Assert.AreEqual(2, lines[1].ProductId);
            Assert.AreEqual(0, lines[1].OrderQty);
            Assert.AreEqual(89, lines[1].UnitPriceCents);
            Assert.AreEqual(false, lines[1].IsActive);
            Assert.AreEqual(false, lines[1].IsDeleted);
            Assert.AreEqual(DateTime.MinValue, lines[1].DateCreated);
            Assert.AreEqual("Toot", lines[1].CreatedBy);
            Assert.AreEqual(DateTime.MinValue, lines[1].DateModified);
            Assert.AreEqual("Xandir", lines[1].ModifiedBy);
            #endregion
        }
    }
}
