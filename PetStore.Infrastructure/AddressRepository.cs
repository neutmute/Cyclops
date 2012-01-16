using System;
using System.Collections.Generic;
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
            return Database.ExecuteSprocAccessor<Address>(90, "dbo.Address_GetAll").ToList();
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
    }
}
