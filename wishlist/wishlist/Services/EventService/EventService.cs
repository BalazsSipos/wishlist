using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using whishlist;
using wishlist.Models;
using Microsoft.EntityFrameworkCore;

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
            var eventItem = await applicationDbContext.Events.Include(e => e.Gifts).Include(e => e.Invitations).FirstOrDefaultAsync(e => e.EventId == eventId);
            if (eventItem == null)
            {
                return null;
            }
            return eventItem;
        }
    }
}