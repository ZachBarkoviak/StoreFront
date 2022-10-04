using System;
using System.Collections.Generic;

namespace StoreFront.UI.MVC.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Products = new HashSet<Product>();
        }

        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; } = null!;
        public int ManufacturerPlanetId { get; set; }
        public string? ContactName { get; set; }

        public virtual Planet ManufacturerPlanet { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
