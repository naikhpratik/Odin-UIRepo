using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;

namespace Odin.PropBotWebJob.Helpers
{
    public static class EmailHelper
    {
        public static void SendErrorEmail(string subject, string content)
        {
            try
            {
                var apiKey = ConfigHelper.GetSendGridAPIKey();
                var client = new SendGridClient(apiKey);

                List<EmailAddress> emailTos = new List<EmailAddress>();
                var tos = ConfigHelper.GetEmailErrorTo();
                foreach (var to in tos)
                {
                    emailTos.Add(new EmailAddress(to));
                }

                EmailAddress emailFrom = new EmailAddress(ConfigHelper.GetEmailErrorFrom());

                var message =
                    MailHelper.CreateSingleEmailToMultipleRecipients(emailFrom, emailTos, subject, content, String.Empty);
                if (message != null)
                {
                    message.SetBypassListManagement(true);
                    client.SendEmailAsync(message);
                }
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
