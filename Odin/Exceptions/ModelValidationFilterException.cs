using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Odin.Exceptions
{
    public class ModelValidationFilterException : Exception
    {
        public IEnumerable<string> modelErrors;
        public ModelValidationFilterException() : base()
        {
        }

        public ModelValidationFilterException(string message) : base(message)
        {
            
        }

        public ModelValidationFilterException(IEnumerable<string> modelErrors)
        {
            this.modelErrors = modelErrors;
        }
    }
}