using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Odin.ViewModels
{
    public class ConsultantViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string FullName => $"{FirstName} {LastName}";
    }
}