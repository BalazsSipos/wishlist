using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models.Identity;

namespace wishlist.Models
{
    public class Event
    {
        public long EventId { get; set; }
        public AppUser AppUser { get; set; }
        public EventType EventType { get; set; }
        public DateTime EventDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public string PhotoUrl { get; set; }
        public List<Gift> Gifts { get; set; }
        public List<Invitation> Invitations { get; set; }
    }
}
