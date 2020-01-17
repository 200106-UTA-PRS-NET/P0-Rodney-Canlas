using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class Account
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Passphrase { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
