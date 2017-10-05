using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Odin.IntegrationTests.Helpers
{
    public static class ResultHelper
    {
        public static bool GetContent<T>(IHttpActionResult result, out T content) where T : class
        {
            content = null;
            if (!(result is OkNegotiatedContentResult<T> contentResult))
                return false;

            content = contentResult.Content;

            return true;
        }
    }
}
