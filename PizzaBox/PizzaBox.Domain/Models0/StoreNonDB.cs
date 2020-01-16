using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    class StoreNonDB
    {
        public List<OrderNonDB> completedOrders { get; set; }
        public double sales { get; set; }
        //public Dictionary<Item, int> inventory { get; set; }
        public List<UserNonDB> users { get; set; }

    }
}
