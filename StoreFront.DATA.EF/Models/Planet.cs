using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Planet
    {
        public Planet()
        {
            Manufacturers = new HashSet<Manufacturer>();
            UserDetails = new HashSet<UserDetail>();
        }

        public int PlanetId { get; set; }
        public string? PlanetName { get; set; }
        public string? PlanetCapital { get; set; }

        public virtual ICollection<Manufacturer> Manufacturers { get; set; }
        public virtual ICollection<UserDetail> UserDetails { get; set; }
    }
}
