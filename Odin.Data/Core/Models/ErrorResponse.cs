using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Odin.Data.Core.Models
{
    public class ErrorResponse
    {
        public IEnumerable<string> Errors { get; private set; }

        public static ErrorResponse CreateResponse(ModelStateDictionary modelState)
        {
            var errorResponse = new ErrorResponse { Errors = modelState.ErrorDescriptions() };
            return errorResponse;
        }

        public static ErrorResponse CreateResponse(List<string> errors)
        {
            var errorResponse = new ErrorResponse { Errors = errors };
            return errorResponse;
        }
    }
}
