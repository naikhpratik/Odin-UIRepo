using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;

namespace Odin.Data.Helpers
{
    public static class UserHelper
    {
        public static UserManager<T> GetUserManager<T>(ApplicationDbContext context) where T : ApplicationUser
        {
            var userStore = new UserStore<T>(context);
            var userManager = new UserManager<T>(userStore);
            userManager.UserValidator = new UserValidator<T>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            return userManager;
        }

         
    }
}
