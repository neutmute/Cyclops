﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;
using PetStore.Infrastructure;
using Sprocker.Core.Mapping;

namespace PetStore.IntegrationTest
{
    [TestClass]
    public class AddressMapTest
    {

        public void AddressMap_CanDiscover_ProcPerameters()
        {
            AddressMap addressMap = new AddressMap();
            
            // setup an asm trust to test internals [AS]
        }
    }
}
