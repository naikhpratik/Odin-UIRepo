
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Odin.Controllers;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Interfaces;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Odin.Helpers
{
    public class AccountHelper : IAccountHelper
    {
        private readonly IEmailHelper _emailHelper;
        private readonly ApplicationDbContext _context;
        public AccountHelper(ApplicationDbContext context, IEmailHelper emailHelper)
        {
            _emailHelper = emailHelper;
            _context = context;
        }

        public async Task<string> SendEmailConfirmationTokenAsync<T>(string userId, UrlHelper url) where T : ApplicationUser //, string subject, string bdy)
        {
            var userStore = new UserStore<T>(_context);
            var userManager = new UserManager<T>(userStore);

            var provider = new DpapiDataProtectionProvider("Odin");
            var userTokenProvider = new DataProtectorTokenProvider<T, string>(provider.Create("EmailConfirmation")) as IUserTokenProvider<T, string>;
            userManager.UserTokenProvider = new DataProtectorTokenProvider<T, string>(provider.Create("EmailConfirmation")) as IUserTokenProvider<T, string>;
           
            //Forgery check
            string code = await userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = url.Action("ResetPassword", "Account", new { userId, code = code });
            //The link sent to the Transferees for access to reset password
            //var callbackUrl = Url.Action("ResetPassword", "Account", new { userID, code = code }, protocol: Request.Url.Scheme);
            //transferee's email address
            var eml = await userManager.GetEmailAsync(userId);
            var user = await userManager.FindByNameAsync(eml);
            var name = user.FirstName + " " + user.LastName; // eml.Substring(0, eml.IndexOf("@")).Replace(".", " ");
            var subject = "Create Password";
            var templateFolderPath = HttpContext.Current.Server.MapPath(@"~\Views\Mailers\");
            string template = System.IO.File.ReadAllText(templateFolderPath + "SetNewPassword.cshtml");
            var body = Razor.Parse(template, new { Name = name, Link = callbackUrl });
            //send the email, specify the content mime type
            var response = _emailHelper.SendEmail_SG(eml, subject, body, SendGrid.MimeType.Html);
            return callbackUrl;
        }

        public Task<string> SendEmailConfirmationTokenAsync(string userId, string eml)
        {
            throw new NotImplementedException();
        }
    }
}