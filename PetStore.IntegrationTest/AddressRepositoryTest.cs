using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;
using PetStore.Infrastructure;
using Sprocker.Core;

namespace PetStore.IntegrationTest
{
    /// <summary>
    /// Summary description for AddressRepositoryTest
    /// </summary>
    [TestClass]
    public class AddressRepositoryTest
    {
        private IAddressRepository _addressRepository;

        public AddressRepositoryTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

         [TestInitialize()]
         public void MyTestInitialize()
         {
             _addressRepository = new AddressRepository();
             ((SqlRepository)_addressRepository).Database = new SprockerSqlDatabase(Constants.TestDatabaseConnectionString);
         }



        [TestMethod]
        public void AddressRepository_BasicUse_GetsAll()
        {


            //List<Address> addresses = addressRepository.GetAll();

            //Console.WriteLine(addresses.Count);

        }


        [TestMethod]
        public void AddressRepository_NonGenericAccessor_GetsAll()
        {
            //IAddressRepository addressRepository = new AddressRepository();
            //((SqlRepository)addressRepository).Database = new SprockerSqlDatabase(Constants.TestDatabaseConnectionString);

            //List<Address> addresses = addressRepository.GetAll();

            //Console.WriteLine(addresses.Count);

        }

        [TestMethod]
        public void AddressRepository_BasicUse_SaveInstance()
        {
           // IAddressRepository addressRepository = new AddressRepository();
           // ((SqlRepository)addressRepository).Database = new SprockerSqlDatabase(Constants.TestDatabaseConnectionString);

           // Address address = new Address();
           // address.AddressLine1 = "";
            
           // addressRepository.Save()
            
           // List<Address> addresses = addressRepository.GetAll();

           //// Console.WriteLine(addresses.Count);

        }


    }
}
