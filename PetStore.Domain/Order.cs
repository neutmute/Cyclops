using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetStore.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        //public int CustomerId { get; set; }
        //public int BillToAddressId { get; set; }
        //public int ShipToAddressId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }

        public List<OrderLine> OrderLines { get; private set; }

        public Customer Customer {get;set;}

        public Order()
        {
            OrderLines = new List<OrderLine>();
        }
    }
}
