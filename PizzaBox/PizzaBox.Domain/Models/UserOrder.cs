using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class UserOrder
    {
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public int? PresetPizza { get; set; }
        public int? CustomPizza { get; set; }
        public decimal TotalCost { get; set; }
    }
}
