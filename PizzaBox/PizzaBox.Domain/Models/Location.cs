using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class Location
    {
        public Location()
        {
            Inventory = new HashSet<Inventory>();
            UserOrder = new HashSet<UserOrder>();
        }

        public int LocationId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? ZipCode { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<UserOrder> UserOrder { get; set; }
    }
}
