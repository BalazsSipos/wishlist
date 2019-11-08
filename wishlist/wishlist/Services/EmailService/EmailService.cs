using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Services.EventService;
using wishlist.Services.InvitationService;

namespace wishlist.Services.EmailService
{
    public class EmailService : IEmailService
    {
        IEventService eventService;
        IInvitationService invitationService;

        public EmailService(IEventService eventService, IInvitationService invitationService)
        {
            this.eventService = eventService;
            this.invitationService = invitationService;
        }

        public async Task SendMailToBuyers(long eventId, bool isSendToAll)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var eventItem = await eventService.GetEventByIdAsync(eventId);
            if(eventItem != null)
            {
                foreach (Invitation invitation in eventItem.Invitations)
                {
                    if (isSendToAll || (!isSendToAll && invitation.IsEmailSent == false))
                    {
                        var msg = MessageToBuyers(invitation.InvitedEmail, eventItem.AppUser.Email, eventId);
                        var response = await client.SendEmailAsync(msg);
                    }
                }
                await invitationService.ChangeInvitationStatusToSent(eventId);
            }
        }

        public SendGridMessage MessageToBuyers(string invitedEmail, string userName, long eventId)
        {
            var messageToBuyers = new SendGridMessage()
            {
                From = new EmailAddress("admin@wishlist.com", "WishList Team"),
                Subject = "New wishlist for: " + userName,
                HtmlContent = CreateEmailBodyHtmlFromOrder(eventId)
            };
            messageToBuyers.AddTo(new EmailAddress(invitedEmail));
            return messageToBuyers;
        }

        public string CreateEmailBodyHtmlFromOrder(long eventId)
        {
            string emailHtml = "<div><h4>You can find my newest wishlist here: https://localhost:44332/event/show/" + eventId + "</h4></div>";
            return emailHtml;
        }
    }
}
