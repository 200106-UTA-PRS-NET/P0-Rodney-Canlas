using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    class UsersOrderHistoryNonDB
    {
        public Dictionary<string, List<OrderNonDB>> orderHistory { get; set; } = new Dictionary<string, List<OrderNonDB>>();
    }
}
