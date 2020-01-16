using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class UserOrder
    {
        public int OrderId { get; set; }
        public int LocationId { get; set; }
        public string Username { get; set; }
        public int? PresetPizza { get; set; }
        public int? CustomPizza { get; set; }
        public decimal TotalCost { get; set; }

        public virtual Location Location { get; set; }
        public virtual Account UsernameNavigation { get; set; }
    }
}
