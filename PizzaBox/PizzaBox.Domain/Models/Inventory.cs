using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class Inventory
    {
        public int LocationId { get; set; }
        public int? CheesePizza { get; set; }
        public int? PepperoniPizza { get; set; }
        public int? VegetarianPizza { get; set; }
        public int? ThinCrust { get; set; }
        public int? ThickCrust { get; set; }
        public int? MarinaraSauce { get; set; }
        public int? WhiteGarlicSauce { get; set; }
        public int? PepperoniTopping { get; set; }
        public int? SausageTopping { get; set; }
        public int? ChickenTopping { get; set; }
        public int? PineappleTopping { get; set; }
        public int? VeggieTopping { get; set; }
        public int? JalapenoTopping { get; set; }

        public virtual Location Location { get; set; }
    }
}
