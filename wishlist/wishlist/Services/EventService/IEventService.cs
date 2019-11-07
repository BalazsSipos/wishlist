using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using wishlist.Models;

namespace wishlist.Services.EventService
{
    public interface IEventService
    {
        Task<Event> GetEventByIdAsync(long eventId);
        Task<bool> ValidateAccessAsync(long eventId, ClaimsPrincipal user);
        Task<List<Event>> FindEventsByManagerNameOrEmailAsync(string managerName);
        Task<Event> FindEventByGiftId(long id);
    }
}