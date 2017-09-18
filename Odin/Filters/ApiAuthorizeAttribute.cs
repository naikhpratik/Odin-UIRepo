using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Ninject;
using Odin.Data.Core.Models;
using Odin.Interfaces;

namespace Odin.Filters
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        private const string TokenHeaderName = "Token";

        [Inject]
        public IConfigHelper Config { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Authorize(actionContext))
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var errorResponse =
                ErrorResponse.CreateResponse(new List<string> { "You are not authorized to make this request." });
            var response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, errorResponse);

            actionContext.Response = response;
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Headers.Contains(TokenHeaderName))
                return false;

            var token = actionContext.Request.Headers.GetValues(TokenHeaderName).FirstOrDefault();

            if (string.IsNullOrEmpty(token))
                return false;

            if (!token.Equals(Config.GetSeApiToken()))
                return false;

            return true;
        }
    }
}