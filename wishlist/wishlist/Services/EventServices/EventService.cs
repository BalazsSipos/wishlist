using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using wishlist;
using wishlist.Models;

namespace wishlist.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public EventService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<List<Event>> FindEventsByUserAsync(ClaimsPrincipal user)
        {
            var events = await applicationDbContext.Events.Where(e => e.AppUser.UserName == user.Identity.Name).ToListAsync();
            return events;
        }
    }
}