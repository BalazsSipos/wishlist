using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wishlist.Services.EmailService
{
    public interface IEmailService
    {
        Task SendMailToBuyers(long eventId, bool isSendToAll);
        SendGridMessage MessageToBuyers(string invitedEmail, string userName, long eventId);
        string CreateEmailBodyHtmlFromOrder(long eventId);
    }
}
