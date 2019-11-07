using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wishlist.Services.EventService;

namespace wishlist.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Show(long id)
        {
            var eventItem = await eventService.GetEventByIdAsync(id);

            return View(eventItem);
        }
    }
}