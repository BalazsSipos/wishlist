
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using wishlist.Services;
using System.Threading.Tasks;
using wishlist.Services;


namespace wishlist.Controllers
{
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
            var events = await eventService.FindEventsByUserAsync(user);
            return View(events);
        }
    }
}