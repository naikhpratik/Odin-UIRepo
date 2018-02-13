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
            if (modelState.IsValid) return;

            var ai = new TelemetryClient();

            var telemetry = new ExceptionTelemetry(new ModelValidationFilterException());
            if (actionContext.ActionArguments.TryGetValue("orderDto", out var orderDto))
            {
                var dto = orderDto as OrderDto;
                if (dto != null)
                    telemetry.Properties["TrackingID"] = dto.TrackingId;
                else
                {
                    telemetry.Properties["OrderDto is Null"] = true.ToString();
                }
            }
            telemetry.Properties["ModelErrors"] = JsonConvert.SerializeObject(modelState.ErrorDescriptions());
            ai.TrackException(telemetry);

            actionContext.Response = actionContext.Request
                .CreateResponse(HttpStatusCode.BadRequest, ErrorResponse.CreateResponse(modelState));
        }
    }
    
}