using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetStore.Domain
{
    /// <summary>
    /// A Pet Store product
    /// Gerbils etc..
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
    }
}
