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
        public async Task<IActionResult> AddWithUrl(long id)
        {
            if (!await eventService.ValidateAccessAsync(id, User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            AddGiftWithUrlRequest addGiftWithUrlRequest = new AddGiftWithUrlRequest()
            {
                EventId = id,
                Quantity = 1
            };
            return View(addGiftWithUrlRequest);
        }

        [HttpGet]
        public async Task<IActionResult> AddWithData(long id)
        {
            if (!await eventService.ValidateAccessAsync(id, User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            AddGiftWithDataRequest addGiftWithDataRequest = new AddGiftWithDataRequest()
            {
                EventId = id,
                Quantity = 1
            };
            return View(addGiftWithDataRequest);
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
                return RedirectToAction(nameof(EventController.Show), "Event", new { id = addGiftWithDataRequest.EventId });
            }
            return View(addGiftWithDataRequest);
        }

        [HttpPost]
        public async Task<IActionResult> AddWithUrl(AddGiftWithUrlRequest addGiftWithUrlRequest)
        {
            if (!await eventService.ValidateAccessAsync(addGiftWithUrlRequest.EventId, User))
            {
                return RedirectToAction(nameof(EventController.Show), "Show");
            }

            if (ModelState.IsValid)
            {
                await giftService.SaveGiftFromArukeresoAsync(addGiftWithUrlRequest);
                return RedirectToAction(nameof(EventController.Show), "Event", new { id = addGiftWithUrlRequest.EventId });
            }
            return View(addGiftWithUrlRequest);
        }

    }
}