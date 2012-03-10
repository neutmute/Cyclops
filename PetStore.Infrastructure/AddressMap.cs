using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetStore.Domain;
using TheSprocker.Core.Mapping;
using TheSprocker.Core.FluentInterface;
using TheSprocker.Core.ExtensionMethods;

namespace PetStore.Infrastructure
{
    public class AddressMap : Map<Address>
    {
        public AddressMap()
        {
            Define()
                .Proc("Address_GetAll")
                .Criteria<AddressCriteria>()
                .AutoMapAll()
                .Build();
        }
    }
}
