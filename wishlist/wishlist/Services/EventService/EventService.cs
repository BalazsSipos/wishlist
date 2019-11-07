using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wishlist;
using wishlist.Models;

namespace wishlist.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public EventService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Event> GetEventByIdAsync(long eventId)
        {
            var eventItem = await applicationDbContext.Events.Include(e => e.Gifts).Include(e => e.Invitations).Include(e => e.EventType).FirstOrDefaultAsync(e => e.EventId == eventId);
            if (eventItem == null)
            {
                return null;
            }
            return eventItem;
        }

        public async Task<bool> ValidateAccessAsync(long eventId, ClaimsPrincipal user)
        {
            List<Event> ownedEvents = await FindEventsByManagerNameOrEmailAsync(user.Identity.Name);
            Event currentEvent = await GetEventByIdAsync(eventId);
            return ownedEvents.Contains(currentEvent);
        }

        public async Task<List<Event>> FindEventsByManagerNameOrEmailAsync(string managerName)
        {
            var eventList = await applicationDbContext.Events.Include(e => e.Gifts).AsQueryable().Where(e => e.AppUser.UserName == managerName).OrderBy(e => e.Name).ToListAsync();
            return eventList;
        }
    }
}