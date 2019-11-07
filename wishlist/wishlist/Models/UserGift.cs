﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models.Identity;

namespace wishlist.Models
{
    public class UserGift
    {
        public long UserGiftId { get; set; }
        public Gift Gift { get; set; }
        public AppUser BuyerUser { get; set; }
    }
}
