using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wishlist.Models.RequestModels.Event;
using wishlist.Services;
using wishlist.Services.EventService;

namespace wishlist.Controllers
{
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AddEventRequest addEventRequest = await eventService.BuildEmptyAddEventRequestAsync(null);
            return View(addEventRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEventRequest addEventRequest)
        {
            if (ModelState.IsValid)
            {
                await eventService.SaveEventAsync(addEventRequest, User);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View(await eventService.BuildEmptyAddEventRequestAsync(addEventRequest));
        }
    }
}