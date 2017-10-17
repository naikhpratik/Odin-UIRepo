using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Odin.ViewModels.Shared
{
    public class ManagerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}