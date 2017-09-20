using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Odin.ViewModels
{
    public class ProgramManagerViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ProgramManagerFullName => $"{FirstName} {LastName}";
    }
}