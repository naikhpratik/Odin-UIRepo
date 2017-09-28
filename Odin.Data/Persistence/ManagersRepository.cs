using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Data.Helpers;

namespace Odin.Data.Persistence
{
    public class ManagersRepository : IManagersRepository
    {
        private readonly ApplicationDbContext _context;

        public ManagersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Manager GetManagerBySeContactUid(int seContactUid)
        {
            return _context.Managers.FirstOrDefault(m => m.SeContactUid == seContactUid);
        }

        public void Add(Manager manager, string userRole)
        {
            var userStore = new UserStore<Manager>(_context);
            var userManager = new UserManager<Manager>(userStore);
            manager.UserName = manager.Email;
            var result = userManager.Create(manager, PasswordHelper.TemporaryPassword());
            userManager.AddToRole(manager.Id, userRole);
        }
    }
}
