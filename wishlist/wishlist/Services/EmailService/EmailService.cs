using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Services.EventService;

namespace wishlist.Services.EmailService
{
    public class EmailService : IEmailService
    {
        IEventService eventService;

        public EmailService(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public async Task SendMailToBuyers(long eventId)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var eventItem = await eventService.GetEventByIdAsync(eventId);
            foreach (Invitation invitation in eventItem.Invitations)
            {
                var msg = MessageToBuyers(invitation.InvitedEmail, eventItem.AppUser.Email, eventId);
                var response = await client.SendEmailAsync(msg);
            }
        }

        public SendGridMessage MessageToBuyers(string invitedEmail, string userName, long eventId)
        {
            var messageToBuyers = new SendGridMessage()
            {
                From = new EmailAddress("admin@wishlist.com", "WishList Team"),
                Subject = "New wishlist for: " + userName,
                //PlainTextContent = submittedOrder.Restaurant.Manager.Email,
                HtmlContent = CreateEmailBodyHtmlFromOrder(eventId)
            };
            messageToBuyers.AddTo(new EmailAddress(invitedEmail));
            return messageToBuyers;
        }

        public string CreateEmailBodyHtmlFromOrder(long eventId)
        {
            string emailHtml = "<div><h4>You can find my newest wishlist here: https://localhost:44332/event/show/" + eventId + "</h4></div>";
            //orderHtml += "<table><thead><tr><th>Order ID</th><th>Items</th><th>Quantity</th><th>Date and time of the order</th></tr></thead><tbody>";
            //orderHtml += "<tr><td>" + submittedOrder.OrderId + "</td><td>";
            //for (int i = 0; i < submittedOrder.CartItems.Count; i++)
            //{
            //    orderHtml += submittedOrder.CartItems[i].Meal.Name + "<br/>";
            //}
            //orderHtml += "</td><td>";
            //for (int i = 0; i < submittedOrder.CartItems.Count; i++)
            //{
            //    orderHtml += submittedOrder.CartItems[i].Quantity + "<br/>";
            //}
            //orderHtml += "</td><td>" + submittedOrder.DateSubmitted + "</td>";
            //orderHtml += "</tr></tbody></table>";
            //orderHtml += "<br/><br/>";
            //orderHtml += "<table><tbody><tr><th>Name</th><td>" + submittedOrder.User.Email + "</td></tr>";
            //orderHtml += "<tr><th>Country</th><td>" + submittedOrder.Address.Country + "</td></tr>";
            //orderHtml += "<tr><th>City</th><td>" + submittedOrder.Address.City + "</td></tr>";
            //orderHtml += "<tr><th>Zip Code</th><td>" + submittedOrder.Address.ZipCode + "</td></tr>";
            //orderHtml += "<tr><th>Address</th><td>" + submittedOrder.Address.Street + "</td></tr></tbody></table>";
            //orderHtml += "<br/><br/>";
            //orderHtml += "You can check all your current open orders at the following link: ";
            //orderHtml += "<a href=\"http://localhost:53967/Order/CurrentOrder\">Current orders</a>";
            return emailHtml;
        }
    }
}
