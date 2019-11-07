using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wishlist.Models.RequestModels.Event;
using wishlist.Services;
using wishlist.Services.EventService;
using wishlist.Services.GiftService;

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

            AddGiftWithDataRequest addGiftRequest = new AddGiftWithDataRequest()
            {
                EventId = id
            };
            return View(addGiftRequest);
        }

        [HttpPost]
        public async Task<IActionResult> AddWithData(AddGiftWithDataRequest addGiftWithDataRequest)
        {
            if (!await eventService.ValidateAccessAsync(addGiftWithDataRequest.EventId, User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (ModelState.IsValid)
            {
                await giftService.SaveGiftAsync(addGiftWithDataRequest);
                return RedirectToAction(nameof(HomeController.Index), "Home", new { id = addGiftWithDataRequest.EventId });
            }
            return View(addGiftWithDataRequest);
        }

        [HttpPost]
        public async Task<IActionResult> AddWithUrl(AddGiftWithUrlRequest addGiftWithUrlRequest)
        {
            if (!await eventService.ValidateAccessAsync(addGiftWithUrlRequest.EventId, User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (ModelState.IsValid)
            {
                await giftService.SaveGiftFromArukeresoAsync("hello");
                return RedirectToAction(nameof(HomeController.Index), "Home", new { id = addGiftWithUrlRequest.EventId });
            }
            return View(addGiftWithUrlRequest);
        }

    }
}