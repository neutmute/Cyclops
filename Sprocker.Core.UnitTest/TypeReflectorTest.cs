using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;
using TheSprocker.Core.Mapping;

namespace TheSprocker.Core.UnitTest
{
    [TestClass]
    public class TypeReflectorTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            TypeReflector<Address> reflector = new TypeReflector<Address>();
            IList<PropertyInfo> members = reflector.LocateMappingCandidates();

            foreach (var propertyInfo in members)
            {
                Console.WriteLine(propertyInfo.Name);
            }
        }
    }
}
