using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetStore.Domain
{
    /// <summary>
    /// Contains criteria for requesting addresses.
    /// </summary>
    public class AddressCriteria
    {
        public bool IsActive { get; set; }
        public string Country { get; set; }
    }
}
