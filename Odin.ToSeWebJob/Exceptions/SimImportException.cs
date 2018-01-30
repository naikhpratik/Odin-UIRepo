using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.ToSeWebJob.Exceptions
{
    public class SimImportException : Exception
    {
        public SimImportException()
        {
        }

        public SimImportException(string message) : base(message)
        {
        }

        public SimImportException(string message, Exception inner) : base(message, inner)
        { }
    }
}
