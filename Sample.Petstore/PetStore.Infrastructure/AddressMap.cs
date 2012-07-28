//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using PetStore.Domain;
//using Cyclops.Mapping;
//using Cyclops.FluentInterface;
//using Cyclops.ExtensionMethods;

//namespace PetStore.Infrastructure
//{
//    public class AddressMap : CyclopsMap
//    {
//        public AddressMap()
//        {
//            DefineMappings()
//                .Proc("Address_GetAll")
//                .ParameterType<AddressCriteria>() 
//                .ResultType<Address>()
//                .AutoMapAll()
//                .Build();
//        }
//    }
//}
