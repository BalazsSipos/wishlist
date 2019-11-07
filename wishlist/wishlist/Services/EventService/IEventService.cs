using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using wishlist.Models;

namespace wishlist.Services.EventService
{
    public interface IEventService
    {
        Task<Event> GetEventByIdAsync(long eventId);
        Task<bool> ValidateAccessAsync(long eventId, ClaimsPrincipal user);
        Task<List<Event>> FindEventByManagerNameOrEmailAsync(string managerName);
        Task<List<Event>> FindEventsByUserAsync(ClaimsPrincipal user);
    }
}