using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    class OrderNonDB
    {
        public int orderID { get; set; }
        public List<PizzaNonDB> pizzas { get; set; }
        public double cost { get; set; }
    }
}
