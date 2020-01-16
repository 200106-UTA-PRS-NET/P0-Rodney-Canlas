using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class Account
    {
        public Account()
        {
            UserOrder = new HashSet<UserOrder>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<UserOrder> UserOrder { get; set; }
    }
}
