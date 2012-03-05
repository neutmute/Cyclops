using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetStore.Domain;
using Sprocker.Core.Mapping;

namespace PetStore.Infrastructure
{
    public class AddressMap : SprocMap<int,Address>
    {
        public AddressMap("Address_Get")
        {

            this.SprocParameters[0].Value = 10;

            // map overrides criteria to the proc

            // map overrides for the output of the proc. 

            // set other settings 
        }
    }
}
