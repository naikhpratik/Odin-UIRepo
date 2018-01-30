using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Newtonsoft.Json;
using Odin.Data.Core.Dtos;
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
            
            var telemetry = new ExceptionTelemetry(new ModelValidationFilterException());
            if(actionContext.ActionArguments.TryGetValue("orderDto", out var orderDto))
                telemetry.Properties["TrackingID"] = ((OrderDto)orderDto).TrackingId;
            telemetry.Properties["ModelErrors"] = JsonConvert.SerializeObject(modelState.ErrorDescriptions());
            ai.TrackException(telemetry);

            if (!modelState.IsValid)
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.BadRequest, ErrorResponse.CreateResponse(modelState));
        }
    }
}