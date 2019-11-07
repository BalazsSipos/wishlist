using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wishlist.Models
{
    public class Invitation
    {
        public long InvitationId { get; set; }
        public Event Event { get; set; }
        public string InvitedEmail { get; set; }
        public bool IsEmailSent { get; set; }
    }
}
