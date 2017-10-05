using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Odin.IntegrationTests.Extensions
{
    public static class HttpActionResultExtensions
    {
        public static T GetContent<T>(this IHttpActionResult result) where T : class
        {
            if (!(result is OkNegotiatedContentResult<T> contentResult))
                return null;

            return contentResult.Content;
            
        }
    }
}
