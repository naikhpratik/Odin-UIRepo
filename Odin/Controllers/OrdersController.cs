using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.ViewModels;
using Odin.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using RazorEngine;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Odin.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IAccountHelper _accountHelper;
        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper,IAccountHelper accountHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountHelper = accountHelper;
        }
        
        // GET: Orders
        public ViewResult Index()
        {
            var userId = User.Identity.GetUserId();
            try
            {

               _accountHelper.SendEmailConfirmationTokenAsync("06bb5638-796e-4fb7-8e4e-dd95d898b123");
            }
            catch (AggregateException e)
            {
                string em = e.Message;
            }
            //SendEmailConfirmationTokenAsync("06bb5638-796e-4fb7-8e4e-dd95d898b123").Wait();
            var orders = _unitOfWork.Orders.GetOrdersFor(userId);

            var orderVms = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderIndexViewModel>>(orders);

            return View(orderVms);
        }
       
        //private string SendEmailConfirmationTokenAsync(string userID) //, string subject, string bdy)
        //{
        //    //ApplicationUserManager userM = new ApplicationUserManager();
        //    //Forgery check
        //    string code = UserManager.GenerateEmailConfirmationToken(userID);
        //    //The link sent to the Transferees for access to reset password
        //    var callbackUrl = Url.Action("ResetPassword", "Account", new { userID, code = code }, protocol: Request.Url.Scheme);
        //    //transferee's email address
        //    var eml = UserManager.GetEmail(userID);
        //    //var user = UserManager.FindByName(eml);
        //    var name = eml.Substring(0, eml.IndexOf("@")).Replace(".", " ");// user.FirstName + " " + user.LastName; 
        //    var subject = "Create Password";
        //    var templateFolderPath = Server.MapPath(@"~\Views\Mailers\");
        //    string template = System.IO.File.ReadAllText(templateFolderPath + "SetNewPassword.cshtml");
        //    var body = Razor.Parse(template, new { Name = name, Link = callbackUrl });
        //    //send the email, specify the content mime type
        //    var response = _emailHelper.SendEmail_SG(eml, subject, body, SendGrid.MimeType.Html);
        //    return callbackUrl;
        //}
    }
}