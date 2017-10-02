
using Microsoft.AspNet.Identity.Owin;
using Odin.Interfaces;
using RazorEngine;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Odin.Helpers
{
    public class AccountHelper : IAccountHelper
    {
        private readonly IEmailHelper _emailHelper;
        private System.Web.Mvc.UrlHelper _urlHelper;
        public AccountHelper(IEmailHelper emailHelper, System.Web.Mvc.UrlHelper urlHelper)
        {
            _emailHelper = emailHelper;
            _urlHelper = urlHelper;
        }
       
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                // return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                return _userManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public async Task<string> SendEmailConfirmationTokenAsync(string userID) //, string subject, string bdy)
        {
            //ApplicationUserManager userM = new ApplicationUserManager();
            //Forgery check
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            //The link sent to the Transferees for access to reset password
            try
            {
                var callbackUrl = _urlHelper.Action("ResetPassword", "Account", new { userID, code = code }, protocol: HttpContext.Current.Request.Url.Scheme);
            }
            catch (Exception e)
            {
                string mess = e.Message;
            }
            //transferee's email address
            var eml = await UserManager.GetEmailAsync(userID);
            //var user = UserManager.FindByName(eml);
            var name = eml.Substring(0, eml.IndexOf("@")).Replace(".", " ");// user.FirstName + " " + user.LastName; 
            var subject = "Create Password";
            var templateFolderPath = HttpContext.Current.Server.MapPath(@"~\Views\Mailers\");
            string template = System.IO.File.ReadAllText(templateFolderPath + "SetNewPassword.cshtml");
            var body = Razor.Parse(template, new { Name = name, Link = "" });
            //send the email, specify the content mime type
            var response = _emailHelper.SendEmail_SG(eml, subject, body, SendGrid.MimeType.Html);
            return ""; // callbackUrl;
            }
    }
}