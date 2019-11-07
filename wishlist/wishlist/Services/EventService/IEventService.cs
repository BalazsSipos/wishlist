using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using wishlist.Models;

namespace wishlist.Services.EventService
{
    public interface IEventService
    {
        Task<List<Event>> FindEventsByUserAsync(ClaimsPrincipal user);
    }
}