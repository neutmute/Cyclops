using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;
using PetStore.Infrastructure;
using Cyclops;

namespace PetStore.IntegrationTest
{
    /// <summary>
    /// Summary description for AddressRepositoryTest
    /// </summary>
    [TestClass]
    public class AddressRepositoryTest
    {
        private IAddressRepository _addressRepository;

         [TestInitialize()]
         public void MyTestInitialize()
         {
             _addressRepository = new AddressRepository();
             ((CyclopsRepository)_addressRepository).Database = new SqlDatabase(Constants.TestDatabaseConnectionString);
         }



        [TestMethod]
        public void AddressRepository_BasicUse_GetsAll()
        {
            List<Address> addresses = _addressRepository.GetAll();

            Console.WriteLine(addresses.Count);
        }


        [TestMethod]
        public void AddressRepository_NonGenericAccessor_GetsAll()
        {
            //IAddressRepository addressRepository = new AddressRepository();
            //((SqlRepository)addressRepository).Database = new CyclopsSqlDatabase(Constants.TestDatabaseConnectionString);

            //List<Address> addresses = addressRepository.GetAll();

            //Console.WriteLine(addresses.Count);

        }

        [TestMethod]
        public void AddressRepository_BasicUse_SaveInstance()
        {
           // IAddressRepository addressRepository = new AddressRepository();
           // ((SqlRepository)addressRepository).Database = new CyclopsSqlDatabase(Constants.TestDatabaseConnectionString);

           // Address address = new Address();
           // address.AddressLine1 = "";
            
           // addressRepository.Save()
            
           // List<Address> addresses = addressRepository.GetAll();

           //// Console.WriteLine(addresses.Count);

        }


    }
}
