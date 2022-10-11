using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class UserDetail
    {
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int PlanetId { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual Planet Planet { get; set; } = null!;
    }
}
