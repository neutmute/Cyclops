//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using PetStore.Domain;
//using PetStore.Infrastructure;
//using Cyclops.Mapping;

//namespace PetStore.IntegrationTest
//{
//    [TestClass]
//    public class AddressMapTest
//    {
//        [TestMethod]
//        public void AddressMap_CanDiscover_ProcPerameters()
//        {
//            AddressMap addressMap = new AddressMap();
//            CyclopsMapContext CyclopsMapContext = addressMap.GetMapContext();

//            CyclopsMapContext.AutoMap();

//            Assert.IsNotNull(CyclopsMapContext.SprocParameters);
//            Assert.IsTrue(CyclopsMapContext.SprocParameters.Count>0);
//        }

//        [TestMethod]
//        public void Address_map_Maps()
//        {

//            AddressMap addressMap = new AddressMap();
     
//        }
//    }
//}
