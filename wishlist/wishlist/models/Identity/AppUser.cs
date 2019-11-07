using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wishlist.models.Identity
{
    public class AppUser : IdentityUser
    {
        public List<Event> Events { get; set; }
        public List<UserGift> ReservedGifts { get; set; }
    }
}
