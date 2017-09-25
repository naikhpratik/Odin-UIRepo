using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Odin.Data.Core.Models;
using Odin.Interfaces;

namespace Odin.Filters
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        private const string TokenHeaderName = "Token";

        [Inject]
        public IConfigHelper Config { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (Authorize(httpContext))
                return true;

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var errorResponse =
                ErrorResponse.CreateResponse(new List<string> { "You are not authorized to make this request." });

            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            filterContext.Result = new JsonResult() { Data = errorResponse };
        }

        private bool Authorize(HttpContextBase httpContext)
        {
            if (string.IsNullOrEmpty(httpContext.Request.Headers[TokenHeaderName]))
                return false;

            var token = httpContext.Request.Headers[TokenHeaderName];

            if (!token.Equals(Config.GetSeApiToken()))
                return false;

            return true;
        }

    }
}