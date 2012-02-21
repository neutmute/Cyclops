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
            MapInput(c => c.aProp);
            //MapResult(a => a.AddressLine2).ToColumn("County");
        }
    }
}
