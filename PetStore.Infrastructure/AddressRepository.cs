using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
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
             IRowMapper<Address> ProgramGroupRowMapper = MapBuilder<Address>
            .MapAllProperties()
            .DoNotMap(p => p.IsActive)
            .DoNotMap(p => p.State)
            .Map(p=> p.State).ToColumn("LegacyCode")
            .Build();

            List<Address> results = Database.ExecuteSprocAccessor<Address>("dbo.Address_Get",ProgramGroupRowMapper).ToList();

            return results;

            //return ConstructCommand("dbo.Address_Get").ExecuteRowMap<Address>();
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
