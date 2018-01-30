using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace Odin.Filters
{
    /// <summary>
    /// Log Exception to Application Insights in azure
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AiHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext?.HttpContext != null && filterContext.Exception != null)
            {
                var ai = new TelemetryClient();
                ai.TrackException(filterContext.Exception);   
            }
            base.OnException(filterContext);
        }
    }
}