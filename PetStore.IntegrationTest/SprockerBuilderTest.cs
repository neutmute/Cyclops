﻿//using System;
//using System.Linq.Expressions;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using PetStore.Domain;
//using TheSprocker.Core;
//using TheSprocker.Core.FluentInterface;
//using TheSprocker.Core.Mapping;

//namespace PetStore.IntegrationTest
//{
//    [TestClass]
//    public class SprockerBuilderTest
//    {
//        [Ignore]
//        [TestMethod]
//        public void SprockerBuilder_can_build_graph()
//        {       
//            // this idea was going alright but sort of sucks. the goal was to re-use the existing row mappers...

//            //SprockerBuilder sprockerBuilder = new SprockerBuilder();

//            //IParameterMapper parameterMapper = new SprocParameterMapBuilder<AddressCriteria>();

//            //IRowMapper<OrderLine> orderlineMapper = MapBuilder<OrderLine>.MapAllProperties().Build();
//            //IRowMapper<Product> ProductMapper = MapBuilder<Product>.MapAllProperties()
//            //    .DoNotMap(p => p.Name)
//            //    .Map(p => p.Sku).ToColumn("Arse")
//            //    .Build();

//            //sprockerBuilder.MapGraph()
//            //    .InputMapper(parameterMapper)
//            //    .OutputMapper(MapBuilder<Order>.MapAllProperties().Build())
//            //    .Proc("ProcName");

//        }
//    }
//}
