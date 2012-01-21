using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PetStore.Domain;
using Sprocker.Core;

namespace PetStore.Infrastructure
{
    public class AddressRepository : SqlRepository, IAddressRepository
    {
        public Address GetOne(Predicate<Address> filter)
        {
            throw new NotImplementedException();
        }

        public List<Address> GetAll(Predicate<Address> filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Address> GetAll()
        {
            return ConstructCommand("dbo.Address_Get").ExecuteRowMap<Address>();
        }

        public Address Save(Address instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(Address instance)
        {
            throw new NotImplementedException();
        }

        public Address GetAddressForCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public static List<Address> MapAddresses(DataTable addressTable)
        {
            return EntityMapper.Map<Address>(addressTable);
        }
    }
}
