using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace Odin.Data.Extensions
{
    public static class ModelStateExtensions
    {
        public static IEnumerable<string> ErrorDescriptions(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                return modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return null;
        }
    }
}
