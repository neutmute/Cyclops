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
    public class AddressMap : SprockerMap
    {
        public AddressMap()
        {
            DefineMappings()
                .Proc("Address_GetAll")
                .ParameterType<AddressCriteria>() 
                .ResultType<Address>()
                .AutoMapAll()
                .Build();
        }
    }
}
