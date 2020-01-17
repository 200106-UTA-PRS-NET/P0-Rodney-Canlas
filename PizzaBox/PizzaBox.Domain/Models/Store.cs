using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class Store
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? ZipCode { get; set; }
    }
}
