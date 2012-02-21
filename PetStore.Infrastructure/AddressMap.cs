using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetStore.Domain;
using Sprocker.Core.Mapping;

namespace PetStore.Infrastructure
{
    public class AddressMap : SprocMap<Address,AddressCriteria>
    {
        public AddressMap()
        {

            Proc("Address_Get"); 
            AutoMapAll();
            MapInput(c => c.aProp);
            MapResult(a => a.AddressLine1).ToColumn("");

        }
    }
}
