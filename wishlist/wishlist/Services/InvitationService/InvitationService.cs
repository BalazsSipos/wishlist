using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Services.EventService;

namespace wishlist.Services.InvitationService
{
    public class InvitationService : IInvitationService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IEventService eventService;

        public InvitationService(ApplicationDbContext applicationDbContext, IEventService eventService)
        {
            this.applicationDbContext = applicationDbContext;
            this.eventService = eventService;
        }

        public async Task SaveInvitationAsync(string InvitedEmail, long id)
        {
            var eventItem = await eventService.GetEventByIdAsync(id);
            Invitation invitation = new Invitation
            {
                IsEmailSent = false,
                Event = eventItem,
                InvitedEmail = InvitedEmail
            };
            await applicationDbContext.Invitations.AddAsync(invitation);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
