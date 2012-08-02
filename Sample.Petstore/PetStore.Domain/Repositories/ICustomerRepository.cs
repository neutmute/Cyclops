using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetStore.Domain.Core;

namespace PetStore.Domain
{
    public interface ICustomerRepository : IEntityRepository<Customer>
    {
        
    }
}
