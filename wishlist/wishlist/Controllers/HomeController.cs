
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wishlist.Services;
using wishlist.Services.EventService;

namespace wishlist.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEventService eventService;

        public HomeController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = User;
            var events = await eventService.FindEventsByManagerNameOrEmailAsync(user.Identity.Name);
            return View(events);
        }
    }
}