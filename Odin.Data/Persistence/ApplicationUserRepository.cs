using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Odin.Data.Core.Models;
using Odin.Data.Helpers;

namespace Odin.Data.Persistence
{
    public class ApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateUser<T>(T user, string password) where T : ApplicationUser
        {
            var userStore = new UserStore<T>(_context);
            var userManager = new UserManager<T>(userStore);

            userManager.Create(user, password);
        }
    }
}
