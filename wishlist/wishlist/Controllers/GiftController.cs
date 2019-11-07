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
    public class GiftController : Controller
    {
        private readonly IEventService eventService;
        private readonly IGiftService giftService;

        public GiftController(IEventService eventService, IGiftService giftService)
        {
            this.eventService = eventService;
            this.giftService = giftService;
        }

        [HttpGet]
        public async Task<IActionResult> Add(long id)
        {
            if (!await eventService.ValidateAccessAsync(id, User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            AddGiftRequest addGiftRequest = new AddGiftRequest()
            {
                eventId = id
            };
            return View(addGiftRequest);
        }
    }
}