using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class UserOrder
    {
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public string OrderContent { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime OrderDateTime { get; set; }
    }
}
