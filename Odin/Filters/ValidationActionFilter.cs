using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.ApplicationInsights;
using Odin.Data.Core.Models;
using Odin.Data.Extensions;
using Odin.Exceptions;

namespace Odin.Filters
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;
            
            var ai = new TelemetryClient();
            ai.TrackException(new ModelValidationFilterException(modelState.ErrorDescriptions()));

            if (!modelState.IsValid)
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.BadRequest, ErrorResponse.CreateResponse(modelState));
        }
    }
}