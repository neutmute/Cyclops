using System;
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
            SprockerBuilder<Order> sprockerBuilder = new SprockerBuilder<Order>();

            IParameterMapper parameterMapper = new SprocParameterMapper<AddressCriteria>();
            IRowMapper<Order> addressMapper = MapBuilder<Order>.MapAllProperties().Build();


            sprockerBuilder.Configure()
                .InputMapper(parameterMapper)
                .OutputMapper(addressMapper)
                .StoredProcedure("ProcName")
                .MapChildNode()
                    .InputMapper(parameterMapper)
                    .OutputMapper(addressMapper)
                    .StoredProcedure("ProcName")
                .MapChildNode()
                    .InputMapper(parameterMapper)
                    .OutputMapper(addressMapper)
                    .StoredProcedure("ProcName");



        }
    }
}
