using System;
using System.Collections.Generic;

namespace StoreFront.UI.MVC.Models
{
    public partial class Planet
    {
        public Planet()
        {
            Manufacturers = new HashSet<Manufacturer>();
        }

        public int PlanetId { get; set; }
        public string? PlanetName { get; set; }
        public string? PlanetCapital { get; set; }

        public virtual ICollection<Manufacturer> Manufacturers { get; set; }
    }
}
