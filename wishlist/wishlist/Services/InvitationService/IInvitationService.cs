using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models;

namespace wishlist.Services.InvitationService
{
    public interface IInvitationService
    {
        Task SaveInvitationAsync(string InvitedEmail, long id);
    }
}
