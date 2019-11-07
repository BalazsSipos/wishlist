using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models.Identity;

namespace wishlist.Models
{
    public class Gift
    {
        public long GiftId { get; set; }
        public Event Event { get; set; }
        public List<UserGift> BuyerUsers { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
        public string GiftUrl { get; set; }
        public string PhotoUrl { get; set; }
        public int Quantity { get; set; }
    }
}
