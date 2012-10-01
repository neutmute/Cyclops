using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetStore.Domain;

namespace PetStore.IntegrationTest.Infrastructure
{
    public class CustomerRepositoryTests
    {

        public static Customer GetUnpersistedCustomer()
        {
            var customer = new Customer();

            customer.FirstName = "Captain";
            customer.LastName = "Hero";
            customer.IsActive = true;
            customer.IsDeleted = false;
            customer.DateCreated = DateTime.Parse("2010-01-02 3:45");
            customer.DateModified = customer.DateCreated.AddHours(1);
            customer.CreatedBy = "unit test";
            customer.ModifiedBy = "unit test";
            return customer;
        }
    }
}
