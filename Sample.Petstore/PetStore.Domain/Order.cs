﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetStore.Domain
{
    public enum OrderStatus
    {
        InShoppingCart = 0,
        Picking = 1,
        Shipped = 2,
        Delivered = 3,
        OnBackOrder =4
    }

    public class Order
    {
        public int Id { get; set; }
        public OrderStatus Status  {get;set;}
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
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

        public override string ToString()
        {
            return string.Format("Id={0}, OrderDate={1}, Customer=[{2}]", Id, OrderDate, Customer);
        }
    }
}
