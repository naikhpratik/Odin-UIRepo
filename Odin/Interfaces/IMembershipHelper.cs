using Odin.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Odin.Interfaces
{
    public interface IAccountHelper
    {
        Task<string> SendEmailConfirmationTokenAsync<T>(string userId, UrlHelper url) where T : ApplicationUser;
    }
}
