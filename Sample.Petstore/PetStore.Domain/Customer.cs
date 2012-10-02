using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodaTime;

namespace PetStore.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmailPromotion { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }

        public Customer()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("Id={0}, FirstName={1}", Id, FirstName);
        }
    }
}
