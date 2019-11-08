using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wishlist.Services.InvitationService;

namespace wishlist.Controllers
{
    public class InvitationController : Controller
    {
        private readonly IInvitationService invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            this.invitationService = invitationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddInvitation(string InvitedEmail, long id)
        {
            await invitationService.SaveInvitationAsync(InvitedEmail, id);
            return RedirectToAction(nameof(EventController.Show), "Event", new { id = id });
        }
    }
}