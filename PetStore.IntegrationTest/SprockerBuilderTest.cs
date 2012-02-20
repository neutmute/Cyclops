using System;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;
using Sprocker.Core;
using Sprocker.Core.FluentInterface;

namespace PetStore.IntegrationTest
{
    [TestClass]
    public class SprockerBuilderTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            SprockerBuilder sprockerBuilder = new SprockerBuilder();

            IParameterMapper parameterMapper = new SprocParameterMapper<AddressCriteria>();

            IRowMapper<Order> orderMapper = MapBuilder<Order>.MapAllProperties().Build();
            IRowMapper<OrderLine> orderlineMapper = MapBuilder<OrderLine>.MapAllProperties().Build();
            IRowMapper<Product> ProductMapper = MapBuilder<Product>.MapAllProperties().Build();

            sprockerBuilder.Configure()
                .InputMapper(parameterMapper)
                .OutputMapper(addressMapper)
                .StoredProcedure("ProcName")
                .MapChildNode(Expression<Func<OrderLine,"OrderLines">>)
                    //.StoredProcedure("ProcName")
                .MapChildNode()
                    .InputMapper(parameterMapper)
                    .StoredProcedure("ProcName");

            



        }
    }
}
