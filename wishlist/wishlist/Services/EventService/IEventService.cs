using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models;

namespace wishlist.Services.EventService
{
    public interface IEventService
    {
        Task<Event> GetEventByIdAsync(long eventId);
    }
}
