using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using Odin.Interfaces;
namespace Odin.Helpers
{
    public class EmailHelper : IEmailHelper
    {
        public string SendEmail_SG(string to, string subject, string content, string contentType)
        {
            ConfigHelper _configHelper = new ConfigHelper();
            //this is needed for SendGrid emailing
            var apiKey = _configHelper.GetSendGridAPIKey();
            var client = new SendGridClient(apiKey);
            //set the message and its components
            var from = new EmailAddress(_configHelper.GetDWOdinTeamEmailFrom()); //DWOdinTeamEmailFrom, "DwellWorks Odin Team"
            SendGridMessage msg = null;
            if (contentType == MimeType.Html)
                msg = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, "", content);
            else if (contentType == MimeType.Text)
                msg = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, content, "");
            if (msg != null)
            {
                msg.SetBypassListManagement(true);
                //send the email
                var response = client.SendEmailAsync(msg);
                return response.Status.ToString();
            }
            return "Message not sent";
        }
        public string SendEmail_FS(string to, string subject, string content, string contentType, string attFile, byte[] bits)
        {
            ConfigHelper _configHelper = new ConfigHelper();
            //this is needed for SendGrid emailing
            var apiKey = _configHelper.GetSendGridAPIKey();
            var client = new SendGridClient(apiKey);
            //set the message and its components
            var from = new EmailAddress(_configHelper.GetDWOdinTeamEmailFrom()); //DWOdinTeamEmailFrom, "DwellWorks Odin Team"
            SendGridMessage msg = null;
            if (contentType == MimeType.Html)
                msg = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, "", content);
            else if (contentType == MimeType.Text)
                msg = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, content, "");

            msg.AddAttachment(filename: attFile, content: Convert.ToBase64String(bits));
            if (msg != null)
            {
                msg.SetBypassListManagement(true);
                //send the email
                var response = client.SendEmailAsync(msg);
                return response.Status.ToString();
            }
            return "Message not sent";
        }
    }
}